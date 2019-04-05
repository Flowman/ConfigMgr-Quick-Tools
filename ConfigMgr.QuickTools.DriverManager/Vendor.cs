using Microsoft.ConfigurationManagement.ManagementProvider;
using System.Collections.Generic;
using System.IO;
using System.ComponentModel;
using System;

namespace ConfigMgr.QuickTools.DriverManager
{
    internal class Vendor
    {
        #region Private
        private static ModifyRegistry registry = new ModifyRegistry();
        private readonly ConnectionManagerBase connectionManager;
        private BackgroundWorker backgroundWorker;
        #endregion

        #region State
        public string Name { get; private set; }
        public List<DriverPackage> Packages { get; private set; } = new List<DriverPackage>();
        public string PackageLocation { get; private set; } = registry.Read("DriverPackageFolder");
        public string SourceLocation { get; private set; }
        public double ProgressStart { get; set; } = 0;
        public int TotalVendors { get; set; } = 1;
        #endregion

        public Vendor(BackgroundWorker worker, ConnectionManagerBase connection, string directory)
        {
            connectionManager = connection;
            backgroundWorker = worker;

            SourceLocation = directory;
            Name = new DirectoryInfo(directory).Name;

            GetDriverPackages();
        }

        private void GetDriverPackages()
        {
            string[] modelEntries = Directory.GetDirectories(SourceLocation, "*", SearchOption.TopDirectoryOnly);

            int num = 0;

            foreach (string modelDirectory in modelEntries)
            {
                double percentage = 100 / modelEntries.Length * num;
                backgroundWorker.ReportProgress(Convert.ToInt32(ProgressStart + (percentage / TotalVendors)), string.Format("Processing Driver Packages for Vendor: {0}\n\nProgress: {1}%", Name, percentage));

                string modelName = new DirectoryInfo(modelDirectory).Name;

                string[] architectureEntries = Directory.GetDirectories(modelDirectory, "*", SearchOption.TopDirectoryOnly);
                foreach (string architectureDirectory in architectureEntries)
                {
                    string architectureName = new DirectoryInfo(architectureDirectory).Name;
                    string driverPackageName = string.Join("-", Name, modelName, architectureName);
                    string packageName = string.Join("-", modelName, architectureName);
                    string target = Path.Combine(PackageLocation, Name, packageName);

                    DriverPackage package = new DriverPackage(connectionManager, driverPackageName, architectureDirectory, target)
                    {
                        Vendor = Name
                    };

                    Packages.Add(package);
                }
                ++num;
            }
        }
    }
}
