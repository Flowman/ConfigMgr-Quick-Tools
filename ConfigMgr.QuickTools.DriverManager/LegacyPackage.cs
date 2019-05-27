using Microsoft.ConfigurationManagement.ManagementProvider;
using System.IO;

namespace ConfigMgr.QuickTools.DriverManager
{
    internal class LegacyPackage : Package
    {
        private readonly ConnectionManagerBase connectionManager;
        private IResultObject packageObject;
        private readonly ModifyRegistry registry = new ModifyRegistry();

        public string PackageVersion { get; protected set; }

        public IResultObject Package
        {
            get
            {
                if (packageObject == null)
                    CreateLegacyPackge();
                return packageObject;
            }
        }

        public LegacyPackage(ConnectionManagerBase connection, string name, string source, string target)
        {
            connectionManager = connection;
            Name = name;
            Source = source;
            Target = target;

            GetVersionFromFile();

            Hash = Utility.CreateHashForFolder(Source);
            Import = !File.Exists(Path.Combine(Source, Hash + ".hash")) || CheckVersion();
        }

        public bool Create()
        {
            return !CreateLegacyPackge() ? false : true;
        }

        private bool CreateLegacyPackge()
        {
            string query = string.Format("SELECT * FROM SMS_Package WHERE NAME = '{0}'", Name);
            packageObject = Utility.GetFirstWMIInstance(connectionManager, query);

            if (packageObject == null)
            {
                if (!Directory.Exists(Target))
                    Directory.CreateDirectory(Target);

                IResultObject instance = connectionManager.CreateInstance("SMS_Package");
                instance["Name"].StringValue = Name;
                instance["Description"].StringValue = "";
                instance["PkgSourceFlag"].IntegerValue = 2;
                instance["PkgSourcePath"].StringValue = Target;
                instance["PkgFlags"].IntegerValue |= 16777216;
                instance["Manufacturer"].StringValue = Vendor;
                instance["Version"].StringValue = FileVersion;
                try
                {
                    instance.Put();
                    instance.Get();
                }
                catch (SmsQueryException ex)
                {
                    Exception.Add(ex);

                    return false;
                }

                Utility.AddObjectToFolder(connectionManager, registry.ReadString("LegacyConsoleFolder"), instance["PackageID"].StringValue, 2);

                packageObject = instance;
            }

            return true;
        }

        private bool CheckVersion()
        {
            return GetPackageVersion() && PackageVersion != FileVersion ? true : false;
        }

        private bool GetPackageVersion()
        {
            string query = string.Format("SELECT * FROM SMS_Package WHERE NAME = '{0}'", Name);
            packageObject = Utility.GetFirstWMIInstance(connectionManager, query);

            if (packageObject != null)
            {
                PackageVersion = packageObject["Version"].StringValue;
            }

            return string.IsNullOrEmpty(PackageVersion) ? false : true;
        }
    }
}
