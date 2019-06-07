using Microsoft.ConfigurationManagement.AdminConsole;
using Microsoft.ConfigurationManagement.AdminConsole.DialogFramework;
using System;
using System.ComponentModel;
using System.Management;
using System.Windows.Forms;
using Microsoft.ConfigurationManagement.AdminConsole.WizardFramework;
using System.Drawing;
using System.Collections.Generic;
using System.Security.AccessControl;

namespace ConfigMgr.QuickTools.DriverManager
{
    public partial class DriverGrabberGeneralPage : SmsPageControl
    {
        private BackgroundWorker progressWorker;
        private ProgressInformationDialog progressInformationDialog;
        private readonly ModifyRegistry registry = new ModifyRegistry();
        private List<ManagementObject> signedDrivers;
        private bool valiadated = false;

        public DriverGrabberGeneralPage(SmsPageData pageData)
            : base(pageData)
        {
            FormTitle = "Driver Grabber";
            Title = "Checking prerequisites";
            Headline = "Before grabbing drivers let's check requirements";

            InitializeComponent();

            pageData.ProgressBarStyle = ProgressBarStyle.Continuous;

            Updater.CheckUpdates();
        }

        public override void InitializePageControl()
        {
            base.InitializePageControl();
            Initialized = true;
            // This is required to bypass PostApply put as this is not allowed for 
            if (FormType == SmsFormType.Wizard)
                PropertyManagerOverrides.Add("null", null);

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
                if (signedDrivers == null)
                {
                    progressInformationDialog = new ProgressInformationDialog
                    {
                        Title = "Retrieving driver data"
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
                }

                return base.OnNavigating(navigationType);
            }

            progressInformationDialog = new ProgressInformationDialog
            {
                Title = "Validating requirments"
            };
            progressWorker = new BackgroundWorker();
            progressWorker.DoWork += new DoWorkEventHandler(ValidateWorker_DoWork);
            progressWorker.ProgressChanged += new ProgressChangedEventHandler(ProgressWorker_ProgressChanged);
            progressWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(ProgressWorker_RunWorkerCompleted);
            progressWorker.WorkerReportsProgress = true;
            UseWaitCursor = true;
            progressWorker.RunWorkerAsync();
            progressInformationDialog.ShowDialog(this);
            if (!progressInformationDialog.Result)
                return false;

            return false;
        }

        public override void OnActivated()
        {
            base.OnActivated();

            ((SmsWizardPage)Parent).WizardForm.EnableButton(ButtonType.Next, true);

            if (string.IsNullOrEmpty(registry.ReadString("DriverSourceFolder")))
            {
                ((SmsWizardPage)Parent).WizardForm.EnableButton(ButtonType.Next, false);
                labelOptions.Text = "No driver source structure specified!";
            }
        }

        public override bool OnDeactivate()
        {
            return base.OnDeactivate();
        }

        private void ValidateWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            e.Result = ValidateConnections((BackgroundWorker)sender);
        }

        private void ProgressWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            e.Result = GetDrivers((BackgroundWorker)sender);
        }

        private void ProgressWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressInformationDialog.UpdateProgressText(e.UserState as string);
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

        private bool ValidateConnections(BackgroundWorker progressWorker)
        {
            bool flag = false;

            try
            {
                progressWorker.ReportProgress(0, "Validating connection to WMI");

                ManagementScope scope = Utility.GetWMIScope(PropertyManager["Name"].StringValue, "cimv2");

                if (scope.IsConnected == true)
                {
                    checkBoxWMI.Checked = true;
                    labelWMIStatus.Text = "Connected successfully";
                    flag = true;
                }
            }
            catch (Exception ex)
            {
                checkBoxWMI.Checked = false;
                labelWMIStatus.Text = ex.Message;
                flag = false;
            }

            if (progressInformationDialog.ReceivedRequestToClose)
                return false;

            progressWorker.ReportProgress(50, "Validating connection to ADMIN$ share");

            if (Utility.CheckFolderPermissions(string.Format(@"\\{0}\admin$", PropertyManager["Name"].StringValue), FileSystemRights.ReadData))
            {
                checkBoxShare.Checked = true;
                labelShareStatus.Text = "Connected successfully";
            }
            else
            {
                checkBoxShare.Checked = false;
                labelShareStatus.Text = "Access denied";
            }

            progressWorker.ReportProgress(100, "Verification completed");

            return flag;
        }

        private bool GetDrivers(BackgroundWorker progressWorker)
        {
            try
            {
                progressWorker.ReportProgress(0, "Retrieving driver data from WMI");

                ManagementScope scope = Utility.GetWMIScope(PropertyManager["Name"].StringValue, @"cimv2");

                ObjectQuery query = new ObjectQuery("SELECT * FROM Win32_PnPSignedDriver WHERE DriverProviderName != 'Microsoft' AND DriverProviderName IS NOT NULL");
                //ObjectQuery query = new ObjectQuery("SELECT * FROM Win32_PnPSignedDriver WHERE DriverProviderName IS NOT NULL");
                signedDrivers = Utility.SearchWMIToList(scope, query);

                progressWorker.ReportProgress(0, "Retrieving computer system data from WMI");

                query = new ObjectQuery("SELECT * FROM Win32_ComputerSystem");
                ManagementObject computerSystem = Utility.GetFirstWMIInstance(scope, query);

                progressWorker.ReportProgress(0, "Retrieving operating system data from WMI");

                query = new ObjectQuery("SELECT * FROM Win32_OperatingSystem");
                ManagementObject operatingSystem = Utility.GetFirstWMIInstance(scope, query);

                UserData["SignedDrivers"] = signedDrivers;
                UserData["ComputerSystem"] = computerSystem;
                UserData["OperatingSystem"] = operatingSystem;
            }
            catch (Exception ex)
            {
                string msg = string.Format("{0}: {1}", ex.GetType().Name, ex.Message);
                throw new InvalidOperationException(msg);
            }

            return true;
        }

        private void ButtonOptions_Click(object sender, EventArgs e)
        {
            ShowDialog("QuickToolsOptions", PropertyManager);

            OnActivated();
        }
    }
}
