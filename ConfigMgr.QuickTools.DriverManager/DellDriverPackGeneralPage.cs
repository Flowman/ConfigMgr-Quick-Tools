using Microsoft.ConfigurationManagement.AdminConsole;
using Microsoft.ConfigurationManagement.AdminConsole.DialogFramework;
using Microsoft.ConfigurationManagement.AdminConsole.WizardFramework;
using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Net;
using System.Text;
using System.Windows.Forms;

namespace ConfigMgr.QuickTools.DriverManager
{
    public partial class DellDriverPackGeneralPage : SmsPageControl
    {
        private BackgroundWorker progressWorker;
        private ProgressInformationDialog progressInformationDialog;
        private ModifyRegistry registry = new ModifyRegistry();
        private bool valiadated = false;
        private string previousOS;
        private string previousArchitecture;
        private bool downloadedCatalog = false;

        public DellDriverPackGeneralPage(SmsPageData pageData)
            : base(pageData)
        {
            FormTitle = "Dell Driver Pack";
            Title = "General";
            Headline = "Select OS and Architecture to narrow down selections";

            InitializeComponent();

            pageData.ProgressBarStyle = ProgressBarStyle.Continuous;
        }

        public override void InitializePageControl()
        {
            base.InitializePageControl();
            Initialized = true;

            labelInformation.Text = "Welcome to the Dell Driver Pack Download tool.\r\n\r\nThis tool gives you a quick way to download Dell ConfigMgr Driver Packages.\r\n\r\nSelect the required Operating System and Architecture and the next page will show you available downloads for your models. ";

            buttonOptions.Image = new Icon(Properties.Resources.options, new Size(16, 16)).ToBitmap();
            comboBoxOS.SelectedIndex = 0;
            comboBoxArchitecture.SelectedIndex = 0;
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
                Title = "Downloading Catalog",
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

            StringBuilder sb = new StringBuilder();

            if (string.IsNullOrEmpty(registry.Read("DriverSourceFolder")))
            {
                ((SmsWizardPage)Parent).WizardForm.EnableButton(ButtonType.Next, false);
                sb.AppendLine("No driver source structure specified!");
            }

            if (string.IsNullOrEmpty(registry.Read("TempDownloadPath")))
            {
                ((SmsWizardPage)Parent).WizardForm.EnableButton(ButtonType.Next, false);
                sb.AppendLine("No temporary download folder specified!");
            }

            if (string.IsNullOrEmpty(registry.Read("DellCatalogURI")))
            {
                ((SmsWizardPage)Parent).WizardForm.EnableButton(ButtonType.Next, false);
                sb.AppendLine("No Dell Catalog URL specified!");
            }

            labelOptions.Text = sb.ToString();
        }

        public override bool OnDeactivate()
        {
            UserData["Architecture"] = comboBoxArchitecture.Text;
            UserData["OS"] = comboBoxOS.Text;

            return base.OnDeactivate();
        }

        private void ProgressWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            e.Result = DownloadCatalog((BackgroundWorker)sender);
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

        private bool DownloadCatalog(BackgroundWorker progressWorker)
        {
            bool flag = false;

            progressWorker.ReportProgress(0, "Downloading DriverPackCatalog.cab...");

            if (downloadedCatalog == false)
            {
                Uri DellXMLCabinetSource = new Uri(registry.Read("DellCatalogURI"));
                string tempFile = Path.Combine(Path.GetTempPath(), "DriverPackCatalog.cab");

                using (WebClient client = new WebClient())
                {
                    client.DownloadFile(DellXMLCabinetSource, tempFile);
                }

                downloadedCatalog = true;
                previousOS = comboBoxOS.Text;
                previousArchitecture = comboBoxArchitecture.Text;
            }

            flag = true;

            progressWorker.ReportProgress(100, "Done");

            return flag;
        }

        private void ButtonOptions_Click(object sender, EventArgs e)
        {
            ShowDialog("QuickToolsOptions", PropertyManager);
        }
    }
}
