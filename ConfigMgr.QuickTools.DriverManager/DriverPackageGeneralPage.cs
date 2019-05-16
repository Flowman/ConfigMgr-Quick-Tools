using Microsoft.ConfigurationManagement.AdminConsole;
using Microsoft.ConfigurationManagement.AdminConsole.DialogFramework;
using Microsoft.ConfigurationManagement.AdminConsole.WizardFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace ConfigMgr.QuickTools.DriverManager
{
    public partial class DriverPackageGeneralPage : SmsPageControl
    {
        private BackgroundWorker progressWorker;
        private ProgressInformationDialog progressInformationDialog;
        private ModifyRegistry registry = new ModifyRegistry();
        private bool valiadated = false;

        public DriverPackageGeneralPage(SmsPageData pageData)
            : base(pageData)
        {
            FormTitle = "Driver Package Manager";
            Title = "General";
            Headline = "Information";

            InitializeComponent();

            pageData.ProgressBarStyle = ProgressBarStyle.Continuous;

            Updater.CheckUpdates();
        }

        public override void InitializePageControl()
        {
            base.InitializePageControl();

            labelInformation.Text = string.Format("Welcome to the Driver Package Manager Import tool.\r\n\r\nThis tool gives you a quick way to work with your driver packages as no ConfigMgr skill are required. Just create your driver structure and manage all drivers and packages on a storage level.\r\n\r\nThe tool will import your driver packages from {0}.", registry.ReadString("DriverSourceFolder"));

            Initialized = true;

            buttonOptions.Image = new Icon(Properties.Resources.options, new Size(16, 16)).ToBitmap();
        }

        protected override void ApplyChanges()
        {
            base.ApplyChanges();
        }

        public override void PostApply(BackgroundWorker worker, DoWorkEventArgs e)
        {
            base.PostApply(worker, e);
        }

        public override void OnAddSummary(SummaryRequestHandler handler)
        {
            base.OnAddSummary(handler);
        }

        public override bool OnNavigating(NavigationType navigationType)
        {
            if (navigationType != NavigationType.Forward || valiadated)
            {
                return base.OnNavigating(navigationType);
            }

            progressInformationDialog = new ProgressInformationDialog
            {
                Title = "Processing Driver Packages",
                Height = 130,
                ProgressBarStyle = ProgressBarStyle.Continuous
            };

            progressWorker = new BackgroundWorker();
            progressWorker.DoWork += new DoWorkEventHandler(ProgressWorker_DoWork);
            progressWorker.ProgressChanged += new ProgressChangedEventHandler(ProgressWorker_ProgressChanged);
            progressWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(ProgressWorker_RunWorkerCompleted);
            progressWorker.WorkerReportsProgress = true;
            UseWaitCursor = true;
            progressWorker.RunWorkerAsync();

            progressInformationDialog.ShowDialog(this);
            if (!progressInformationDialog.Result)
                return false;

            return base.OnNavigating(navigationType);
        }

        public override void OnActivated()
        {
            base.OnActivated();

            ((SmsWizardPage)Parent).WizardForm.EnableButton(ButtonType.Next, true);

            StringBuilder sb = new StringBuilder();

            if (string.IsNullOrEmpty(registry.ReadString("DriverSourceFolder")))
            {
                ((SmsWizardPage)Parent).WizardForm.EnableButton(ButtonType.Next, false);
                sb.AppendLine("No driver source path specified!");
            }

            if (string.IsNullOrEmpty(registry.ReadString("DriverPackageFolder")))
            {
                ((SmsWizardPage)Parent).WizardForm.EnableButton(ButtonType.Next, false);
                sb.AppendLine("No driver package path specified!");
            }

            labelOptions.Text = sb.ToString();
        }

        private void ProgressWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            e.Result = ProcessSource((BackgroundWorker)sender);
        }

        private void ProgressWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressInformationDialog.UpdateProgressText(e.UserState as string);
            progressInformationDialog.UpdateProgressValue(e.ProgressPercentage);
        }

        private void ProgressWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            progressInformationDialog.Result = (bool)e.Result;
            valiadated = (bool)e.Result;
            progressInformationDialog.UpdateProgressValue(100);
            progressInformationDialog.CloseDialog();

            try
            {
                if (e.Error != null)
                {
                    using (SccmExceptionDialog sccmExceptionDialog = new SccmExceptionDialog(e.Error))
                    {
                        sccmExceptionDialog.ShowDialog();
                    }
                }
            }
            finally
            {
                if (sender as BackgroundWorker == progressWorker)
                {
                    progressWorker.Dispose();
                    progressWorker = null;
                    UseWaitCursor = false;
                }
            }
        }

        private bool ProcessSource(BackgroundWorker progressWorker)
        {
            bool flag = false;

            List<DriverPackage> driverPackages = new List<DriverPackage>();

            string sourceDirectory = registry.ReadString("DriverSourceFolder");
            UserData["sourceDirectory"] = sourceDirectory;
            string packageDirectory = registry.ReadString("DriverPackageFolder");

            progressWorker.ReportProgress(0, "Validating source folder");

            if (!Directory.Exists(packageDirectory) || !Directory.Exists(packageDirectory))
            {
                throw new InvalidOperationException("Cannot find source or destination folder. Verify that the folder exists in the specified locations.");
            }

            if (registry.ReadBool("LegacyFolderStructure"))
            {
                string[] subdirectoryEntries = Directory.GetDirectories(sourceDirectory, "*", SearchOption.TopDirectoryOnly);
                int totalVendors = subdirectoryEntries.Length;
                int num = 0;
                foreach (string vendorDirectory in subdirectoryEntries)
                {
                    string name = new DirectoryInfo(vendorDirectory).Name;
                    int start = 100 / totalVendors * num;
                    progressWorker.ReportProgress(start, string.Format("Processing Driver Packages for Vendor: {0}", name));
                    // create vendor object
                    Vendor vendor = new Vendor(progressWorker, ConnectionManager, vendorDirectory)
                    {
                        ProgressStart = start,
                        TotalVendors = totalVendors
                    };
                    // get driver packages for vendor
                    if (vendor.GetDriverPackages(progressInformationDialog))
                    {
                        foreach (DriverPackage package in vendor.Packages)
                        {
                            driverPackages.Add(package);
                        }
                    }
                    ++num;

                    if (progressInformationDialog.ReceivedRequestToClose)
                        return false;
                }

                flag = true;
            }
            else
            {
                string[] subdirectoryEntries = Directory.GetDirectories(sourceDirectory, "*", SearchOption.TopDirectoryOnly);
                int totalDriverPackges = subdirectoryEntries.Length;
                int num = 0;
                foreach (string driverPackageDirectory in subdirectoryEntries)
                {
                    string driverPackageName = new DirectoryInfo(driverPackageDirectory).Name;
                    int start = 100 / totalDriverPackges * num;
                    progressWorker.ReportProgress(start, string.Format("Processing Driver Packages: {0}", driverPackageName));
                    // create vendor object

                    string vendor = driverPackageName.Split(new[] { '-' }, 2)[0];
                    string targetDirectory = Path.Combine(packageDirectory, driverPackageName);

                    DriverPackage package = new DriverPackage(ConnectionManager, driverPackageName, driverPackageDirectory, targetDirectory)
                    {
                        Vendor = vendor
                    };

                    driverPackages.Add(package);

                    ++num;

                    if (progressInformationDialog.ReceivedRequestToClose)
                        return false;
                }
                flag = true;
            }

            UserData["DriverPackages"] = driverPackages;

            progressWorker.ReportProgress(100, "Done");

            return flag;
        }

        private void ButtonOptions_Click(object sender, EventArgs e)
        {
            ShowDialog("QuickToolsOptions", PropertyManager);

            OnActivated();
        }
    }
}
