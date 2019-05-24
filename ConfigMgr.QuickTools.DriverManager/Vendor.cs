using Microsoft.ConfigurationManagement.ManagementProvider;
using System.Collections.Generic;
using System.IO;
using System.ComponentModel;

namespace ConfigMgr.QuickTools.DriverManager
{
    internal class Vendor
    {
        #region Private
        private static readonly ModifyRegistry registry = new ModifyRegistry();
        private readonly ConnectionManagerBase connectionManager;
        private readonly BackgroundWorker backgroundWorker;
        #endregion

        #region State
        public string Name { get; private set; }
        public List<DriverPackage> DriverPackages { get; private set; } = new List<DriverPackage>();
        public List<LegacyPackage> LegacyPackages { get; private set; } = new List<LegacyPackage>();
        public string PackageLocation { get; private set; } = registry.ReadString("DriverPackageFolder");
        public string SourceLocation { get; private set; }
        public int ProgressStart { get; set; } = 0;
        public int TotalVendors { get; set; } = 1;
        #endregion

        public Vendor(BackgroundWorker worker, ConnectionManagerBase connection, string directory)
        {
            connectionManager = connection;
            backgroundWorker = worker;

            SourceLocation = directory;
            Name = new DirectoryInfo(directory).Name;
        }

        public bool GetDriverPackages(ProgressInformationDialog progressInformationDialog)
        {
            string[] modelEntries = Directory.GetDirectories(SourceLocation, "*", SearchOption.TopDirectoryOnly);

            int num = 0;

            foreach (string modelDirectory in modelEntries)
            {
                string modelName = new DirectoryInfo(modelDirectory).Name;
                int percent = num * 100 / modelEntries.Length;
                backgroundWorker.ReportProgress(ProgressStart + (percent / TotalVendors), string.Format("Processing Driver Packages for Vendor: {0}\n\n Model: {1}", Name, modelName));

                string[] architectureEntries = Directory.GetDirectories(modelDirectory, "*", SearchOption.TopDirectoryOnly);
                foreach (string sourceDirectory in architectureEntries)
                {
                    string architectureName = new DirectoryInfo(sourceDirectory).Name;
                    string driverPackageName = string.Join("-", Name, modelName, architectureName);
                    string packageName = string.Join("-", Name, modelName, architectureName);
                    string targetDirectory = Path.Combine(PackageLocation, Name, packageName);

                    DriverPackage package = new DriverPackage(connectionManager, driverPackageName, sourceDirectory, targetDirectory)
                    {
                        Vendor = Name
                    };

                    DriverPackages.Add(package);
                }
                ++num;

                if (progressInformationDialog.ReceivedRequestToClose)
                    return false;
            }

            return DriverPackages.Count > 0 ? true : false;
        }

        public bool GetLegacyPackages(ProgressInformationDialog progressInformationDialog)
        {
            string[] modelEntries = Directory.GetDirectories(SourceLocation, "*", SearchOption.TopDirectoryOnly);

            int num = 0;

            foreach (string modelDirectory in modelEntries)
            {
                string modelName = new DirectoryInfo(modelDirectory).Name;
                int percent = num * 100 / modelEntries.Length;
                backgroundWorker.ReportProgress(ProgressStart + (percent / TotalVendors), string.Format("Processing Driver Source for Vendor: {0}\n\n Model: {1}", Name, modelName));

                string[] architectureEntries = Directory.GetDirectories(modelDirectory, "*", SearchOption.TopDirectoryOnly);
                foreach (string sourceDirectory in architectureEntries)
                {
                    string architectureName = new DirectoryInfo(sourceDirectory).Name;
                    string driverPackageName = string.Join("-", Name, modelName, architectureName);
                    string packageName = string.Join("-", Name, modelName, architectureName);
                    string targetDirectory = Path.Combine(PackageLocation, Name, packageName);

                    LegacyPackage package = new LegacyPackage(connectionManager, driverPackageName, sourceDirectory, targetDirectory)
                    {
                        Vendor = Name
                    };

                    LegacyPackages.Add(package);
                }
                ++num;

                if (progressInformationDialog.ReceivedRequestToClose)
                    return false;
            }

            return LegacyPackages.Count > 0 ? true : false;
        }
    }
}
