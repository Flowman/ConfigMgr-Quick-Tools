using Microsoft.ConfigurationManagement.ManagementProvider;
using System;
using System.Collections.Generic;
using System.IO;

namespace ConfigMgr.QuickTools.DriverManager
{
    internal class LegacyPackage
    {
        private readonly ConnectionManagerBase connectionManager;
        private IResultObject packageObject;

        public string Name { get; private set; }
        public string Vendor { get; set; }
        public string Source { get; private set; }
        public string Target { get; private set; }
        public bool Import { get; private set; }
        public string Hash { get; private set; }
        public List<Exception> Exception { get; private set; } = new List<Exception>();
        public bool HasException { get { return Exception.Count > 0 ? true : false; } }
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

            Hash = Utility.CreateHashForFolder(Source);
            Import = !File.Exists(Path.Combine(Source, Hash + ".hash"));
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

                // TODO: add an option for folder location
                //Utility.AddObjectToFolder(connectionManager, Vendor, instance["PackageID"].StringValue, 23);

                packageObject = instance;
            }

            return true;
        }

        public void CreateHashFile()
        {
            try
            {
                string[] fileList = Directory.GetFiles(Source, "*.hash");
                foreach (string file in fileList)
                {
                    File.Delete(file);
                }
                File.Create(Path.Combine(Source, Hash + ".hash"));
            }
            catch
            {
                Exception.Add(new SystemException("Cannot create Hash file."));
            }
        }
    }
}
