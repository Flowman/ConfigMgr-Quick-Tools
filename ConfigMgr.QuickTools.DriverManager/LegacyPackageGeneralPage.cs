using Microsoft.ConfigurationManagement.AdminConsole;
using Microsoft.ConfigurationManagement.AdminConsole.DialogFramework;
using Microsoft.ConfigurationManagement.AdminConsole.WizardFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Security.AccessControl;
using System.Text;
using System.Windows.Forms;


namespace ConfigMgr.QuickTools.DriverManager
{
    public partial class LegacyPackageGeneralPage : SmsPageControl
    {
        private BackgroundWorker progressWorker;
        private ProgressInformationDialog progressInformationDialog;
        private readonly ModifyRegistry registry = new ModifyRegistry();
        private bool valiadated = false;

        public LegacyPackageGeneralPage(SmsPageData pageData)
            : base(pageData)
        {
            FormTitle = "Driver Legacy Package Manager";
            Title = "General";
            Headline = "Information";

            InitializeComponent();

            pageData.ProgressBarStyle = ProgressBarStyle.Continuous;

            Updater.CheckUpdates();
        }

        public override void InitializePageControl()
        {
            base.InitializePageControl();

            labelInformation.Text = string.Format("Welcome to the Driver Legacy Package Manager Import tool.\r\n\r\nThis tool gives you a quick way to work with your drivers as no ConfigMgr skill are required. Just create your driver structure and manage all drivers on a storage level.\r\n\r\nThe tool will import your drivers and create packages from {0}.", registry.ReadString("DriverSourceFolder"));

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
                Title = "Processing Driver Source Folder",
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

            bool flag = true;
            StringBuilder sb = new StringBuilder();

            if (string.IsNullOrEmpty(registry.ReadString("DriverSourceFolder")))
            {
                flag = false;
                sb.AppendLine("No driver source path specified!");
            }
            else if (!Utility.CheckFolderPermissions(registry.ReadString("DriverSourceFolder"), FileSystemRights.Modify))
            {
                flag = false;
                sb.AppendLine("Access denied to driver source folder");
            }

            if (string.IsNullOrEmpty(registry.ReadString("LegacyPackageFolder")))
            {
                flag = false;
                sb.AppendLine("No legacy package path specified!");
            }
            else if (!Utility.CheckFolderPermissions(registry.ReadString("LegacyPackageFolder"), FileSystemRights.Modify))
            {
                flag = false;
                sb.AppendLine("Access denied to legacy package folder");
            }

            ((SmsWizardPage)Parent).WizardForm.EnableButton(ButtonType.Next, flag);

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
            bool flag;

            List<LegacyPackage> legacyPackages = new List<LegacyPackage>();

            string sourceDirectory = registry.ReadString("DriverSourceFolder");
            UserData["sourceDirectory"] = sourceDirectory;
            string legacyDirectory = registry.ReadString("LegacyPackageFolder");

            progressWorker.ReportProgress(0, "Validating source folder");

            if (registry.ReadBool("LegacyFolderStructure"))
            {
                string[] subdirectoryEntries = Directory.GetDirectories(sourceDirectory, "*", SearchOption.TopDirectoryOnly);
                int totalVendors = subdirectoryEntries.Length;
                int num = 0;
                foreach (string vendorDirectory in subdirectoryEntries)
                {
                    string name = new DirectoryInfo(vendorDirectory).Name;
                    int start = 100 / totalVendors * num;
                    progressWorker.ReportProgress(start, string.Format("Processing Driver Source for Vendor: {0}", name));
                    // create vendor object
                    Vendor vendor = new Vendor(progressWorker, ConnectionManager, vendorDirectory)
                    {
                        ProgressStart = start,
                        TotalVendors = totalVendors
                    };
                    // get driver packages for vendor
                    if (vendor.GetLegacyPackages(progressInformationDialog))
                    {
                        foreach (LegacyPackage package in vendor.LegacyPackages)
                        {
                            legacyPackages.Add(package);
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
                int totalPackges = subdirectoryEntries.Length;
                int num = 0;
                foreach (string packageDirectory in subdirectoryEntries)
                {
                    string packageName = new DirectoryInfo(packageDirectory).Name;
                    int start = 100 / totalPackges * num;
                    progressWorker.ReportProgress(start, string.Format("Processing Driver Source: {0}", packageName));
                    // create vendor object
                    string vendor = packageName.Split(new[] { '-' }, 2)[0];
                    string targetDirectory = Path.Combine(legacyDirectory, packageName);

                    LegacyPackage package = new LegacyPackage(ConnectionManager, packageName, packageDirectory, targetDirectory)
                    {
                        Vendor = vendor
                    };

                    legacyPackages.Add(package);

                    ++num;

                    if (progressInformationDialog.ReceivedRequestToClose)
                        return false;
                }
                flag = true;
            }

            UserData["LegacyPackages"] = legacyPackages;

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
