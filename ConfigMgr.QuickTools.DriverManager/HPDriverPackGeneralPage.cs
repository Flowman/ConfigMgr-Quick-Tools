using Microsoft.ConfigurationManagement.AdminConsole;
using Microsoft.ConfigurationManagement.AdminConsole.DialogFramework;
using Microsoft.ConfigurationManagement.AdminConsole.WizardFramework;
using Microsoft.Deployment.Compression;
using Microsoft.Deployment.Compression.Cab;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;

namespace ConfigMgr.QuickTools.DriverManager
{
    public partial class HPDriverPackGeneralPage : SmsPageControl
    {
        private BackgroundWorker progressWorker;
        private ProgressInformationDialog progressInformationDialog;
        private readonly ModifyRegistry registry = new ModifyRegistry();
        private bool valiadated = false;
        private bool downloadedCatalog = false;
        private bool processedCatalog = false;

        public HPDriverPackGeneralPage(SmsPageData pageData)
            : base(pageData)
        {
            FormTitle = "HP Driver Pack";
            Title = "General";
            Headline = "Select OS and Architecture to narrow down selections";

            InitializeComponent();

            pageData.ProgressBarStyle = ProgressBarStyle.Continuous;

            Updater.CheckUpdates();
        }

        public override void InitializePageControl()
        {
            base.InitializePageControl();
            Initialized = true;

            labelInformation.Text = "Welcome to the HP Driver Pack Download tool.\r\n\r\nThis tool gives you a quick way to download HP ConfigMgr Driver Packages.\r\n\r\nSelect the required Operating System and Architecture and the next page will show you available downloads for your models. ";

            buttonOptions.Image = new Icon(Properties.Resources.options, new Size(16, 16)).ToBitmap();
            comboBoxOS.SelectedIndex = 0;
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
                sb.AppendLine("No driver source structure specified!");
            }

            if (string.IsNullOrEmpty(registry.ReadString("TempDownloadPath")))
            {
                ((SmsWizardPage)Parent).WizardForm.EnableButton(ButtonType.Next, false);
                sb.AppendLine("No temporary download folder specified!");
            }

            if (string.IsNullOrEmpty(registry.ReadString("HPCatalogURI")))
            {
                ((SmsWizardPage)Parent).WizardForm.EnableButton(ButtonType.Next, false);
                comboBoxOS.Enabled = false;
                sb.AppendLine("No HP Catalog URL specified!");
            }
            // check if we already process the catalog xml, then enabled the wizard
            else if (processedCatalog)
            {
                ((SmsWizardPage)Parent).WizardForm.EnableButton(ButtonType.Next, true);
                comboBoxOS.Enabled = true;
            }
            // check if we already have the file and its not older than 1 hour
            else
            {
                ((SmsWizardPage)Parent).WizardForm.EnableButton(ButtonType.Next, false);
                comboBoxOS.Enabled = true;

                string cabFile = Path.Combine(Path.GetTempPath(), "HPClientDriverPackCatalog.cab");

                if (File.Exists(cabFile))
                {
                    long fileSize = new FileInfo(cabFile).Length;
                    long webSize = Utility.GetFileSize(registry.ReadString("HPCatalogURI"));

                    if (fileSize == webSize)
                    {
                        downloadedCatalog = true;
                        if (processedCatalog = ProcessCatalog())
                        {
                            ((SmsWizardPage)Parent).WizardForm.EnableButton(ButtonType.Next, true);
                        }
                    }
                }
            }

            labelOptions.Text = sb.ToString();
        }

        public override bool OnDeactivate()
        {
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
            progressWorker.ReportProgress(0, "Downloading HPClientDriverPackCatalog.cab...");

            if (downloadedCatalog == false)
            {
                Uri HPXMLCabinetSource = new Uri(registry.ReadString("HPCatalogURI"));
                string tempFile = Path.Combine(Path.GetTempPath(), "HPClientDriverPackCatalog.cab");

                using (WebClient client = new WebClient())
                {
                    client.DownloadFile(HPXMLCabinetSource, tempFile);
                }

                downloadedCatalog = true;
            }

            progressWorker.ReportProgress(100, "Done");

            return true;
        }

        private void ButtonOptions_Click(object sender, EventArgs e)
        {
            ShowDialog("QuickToolsOptions", PropertyManager);

            OnActivated();
        }

        private void ComboBoxOS_Click(object sender, EventArgs e)
        {
            if (!(sender as ComboBox).Enabled)
                return;

            if (downloadedCatalog == false)
            {
                (sender as ComboBox).Enabled = false;

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
                {
                    return;
                }
            }

            if (processedCatalog == false)
            {
                processedCatalog = ProcessCatalog();

                OnActivated();
            }
        }
        private bool ProcessCatalog()
        {
            string tempFile = Path.Combine(Path.GetTempPath(), "HPClientDriverPackCatalog.cab");
            using (FileStream innerCab = new FileStream(tempFile, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite))
            {
                CabEngine engine = new CabEngine();
                foreach (ArchiveFileInfo archiveFileInfo in engine.GetFileInfo(innerCab))
                {
                    using (Stream stream = engine.Unpack(innerCab, archiveFileInfo.Name))
                    {
                        XElement catalog = XElement.Load(stream);

                        comboBoxOS.Items.Clear();

                        List<XElement> nodeList = catalog.Element("HPClientDriverPackCatalog").Element("OSList").Elements("OS").ToList();
                        foreach (XElement node in nodeList)
                        {
                            string os = node.Element("Name").Value;

                            comboBoxOS.Items.Add(os);
                            comboBoxOS.SelectedIndex = 0;
                        }
                    }
                }
            }

            return true;
        }
    }
}
