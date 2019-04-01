using Microsoft.ConfigurationManagement.AdminConsole;
using Microsoft.ConfigurationManagement.AdminConsole.DialogFramework;
using System;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Management;
using System.Windows.Forms;
using Microsoft.ConfigurationManagement.AdminConsole.WizardFramework;
using System.Drawing;

namespace ConfigMgr.QuickTools.DriverManager
{
    public partial class DriverGrabberGeneralPage : SmsPageControl
    {
        private BackgroundWorker validateWorker;
        private ValidateInformationDialog validateInfoDialog;
        private ModifyRegistry registry = new ModifyRegistry();
        private bool valiadated = false;

        public DriverGrabberGeneralPage(SmsPageData pageData)
            : base(pageData)
        {
            FormTitle = "Driver Grabber";
            Title = "Checking prerequisites";
            Headline = "Before grabbing drivers let's check requirements";

            InitializeComponent();

            pageData.ProgressBarStyle = ProgressBarStyle.Continuous;
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
                return base.OnNavigating(navigationType);
            }
            validateInfoDialog = new ValidateInformationDialog
            {
                Title = "Validating requirments"
            };
            validateWorker = new BackgroundWorker();
            validateWorker.DoWork += new DoWorkEventHandler(ValidateWorker_DoWork);
            validateWorker.ProgressChanged += new ProgressChangedEventHandler(ValidateWorker_ProgressChanged);
            validateWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(ValidateWorker_RunWorkerCompleted);
            validateWorker.WorkerReportsProgress = true;
            UseWaitCursor = true;
            validateWorker.RunWorkerAsync();
            validateInfoDialog.ShowDialog(this);
            if (!validateInfoDialog.Result)
                return false;
            return base.OnNavigating(navigationType);
        }

        public override void OnActivated()
        {
            base.OnActivated();

            if (string.IsNullOrEmpty(registry.Read("DriverSourceFolder")))
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

        private void ValidateWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            validateInfoDialog.UpdateProgressText(e.UserState as string);
        }

        private void ValidateWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            validateInfoDialog.Result = (bool)e.Result;
            valiadated = (bool)e.Result;
            validateInfoDialog.UpdateProgressValue(100);
            validateInfoDialog.CloseDialog();

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
                if (sender as BackgroundWorker == validateWorker)
                {
                    validateWorker.Dispose();
                    validateWorker = null;
                    UseWaitCursor = false;
                }
            }
        }

        private bool ValidateConnections(BackgroundWorker validateWorker)
        {
            bool flag = false;
            try
            {
                validateWorker.ReportProgress(0, "Validating connection to WMI");
                ConnectionOptions options = new ConnectionOptions
                {
                    Authentication = AuthenticationLevel.PacketPrivacy,
                    Impersonation = ImpersonationLevel.Impersonate,
                    EnablePrivileges = true,
                    Timeout = TimeSpan.FromSeconds(5)
                };
                ManagementScope scope = new ManagementScope(string.Format(@"\\{0}\root\cimv2", PropertyManager["Name"].StringValue), options);
                scope.Connect();

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

            if (validateInfoDialog.ReceivedRequestToClose)
                return false;

            try
            {
                validateWorker.ReportProgress(50, "Validating connection to ADMIN$ share");
                CredentialCache netCache = new CredentialCache
                {
                    { new Uri(string.Format(@"\\{0}\admin$", PropertyManager["Name"].StringValue)), "Digest", CredentialCache.DefaultNetworkCredentials }
                };
                Directory.GetAccessControl(string.Format(@"\\{0}\admin$", PropertyManager["Name"].StringValue));
                checkBoxShare.Checked = true;
                labelShareStatus.Text = "Connected successfully";
                flag = true;
            }
            catch (Exception ex)
            {
                checkBoxShare.Checked = false;
                labelShareStatus.Text = ex.Message;
                flag = false;
            }

            validateWorker.ReportProgress(100, "Verification completed");

            return flag;
        }

        private void ButtonOptions_Click(object sender, EventArgs e)
        {
            DriverOptions options = new DriverOptions(this);
            options.ShowDialog();
            if (options.DialogResult == DialogResult.OK)
            {
                ((SmsWizardPage)Parent).WizardForm.EnableButton(ButtonType.Next, true);
                labelOptions.Text = "";
            }
        }
    }
}
