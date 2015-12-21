using Microsoft.ConfigurationManagement.ManagementProvider;
using System.Collections.Generic;
using System.IO;
using System.ComponentModel;

namespace Zetta.ConfigMgr.IntegrationKit
{
    internal class Vendor
    {
        #region Private
        private ConnectionManagerBase connectionManager;
        private BackgroundWorker backgroundWorker;
        private int percentWorker;
        private string packageLocation;
        #endregion

        #region State
        public string Name { get; private set; }
        public List<DriverPackage> Packages { get; set; }        
        #endregion

        #region Initialization
        private Vendor()
        {
            Packages = new List<DriverPackage>();            
        }

        public Vendor(BackgroundWorker worker, int percent, ConnectionManagerBase connection, string directory, string location)
            : this()
        {
            connectionManager = connection;
            backgroundWorker = worker;
            percentWorker = percent;
            packageLocation = location;

            Name = new DirectoryInfo(directory).Name;
            
            getDriverPackages(directory);
        }
        #endregion

        internal DriverPackage AddPackage(string name, string source, string target)
        {
            DriverPackage package = new DriverPackage(connectionManager, name, source, target);
            package.Vendor = Name;
            Packages.Add(package);

            return package;
        }

        private void getDriverPackages(string directory)
        {
            string[] modelEntries = Directory.GetDirectories(directory, "*", SearchOption.TopDirectoryOnly);
            int num = 0;
            foreach (string modelDirectory in modelEntries)
            {
                int percent = num * 100 / modelEntries.Length;
                backgroundWorker.ReportProgress(percentWorker, string.Format("Processing Driver Packages for Vendor: {0}\n\nProgress: {1}%", Name, percent));

                string modelName = new DirectoryInfo(modelDirectory).Name;

                string[] architectureEntries = Directory.GetDirectories(modelDirectory, "*", SearchOption.TopDirectoryOnly);
                foreach (string architectureDirectory in architectureEntries)
                {
                    string architectureName = new DirectoryInfo(architectureDirectory).Name;
                    string driverPackageName = string.Join("-", Name, modelName, architectureName);
                    string target = Path.Combine(packageLocation, Name + Path.DirectorySeparatorChar + driverPackageName);

                    DriverPackage package = AddPackage(driverPackageName, architectureDirectory, target);
                }
                ++num;
            }
        }
    }
}
