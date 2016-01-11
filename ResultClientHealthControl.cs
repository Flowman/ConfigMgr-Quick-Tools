using Microsoft.ConfigurationManagement.AdminConsole;
using Microsoft.ConfigurationManagement.AdminConsole.DialogFramework;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Net;
using System.Xml;
using System.Management;

namespace Zetta.ConfigMgr.QuickTools
{
    public partial class ResultClientHealthControl : SmsPageControl
    {
        private BackgroundWorker backgroundWorker;

        public ResultClientHealthControl(SmsPageData pageData)
          : base(pageData)
        {
            InitializeComponent();
            buttonRefresh.Image = new Icon(Properties.Resources.update, new Size(16, 16)).ToBitmap();
            buttonEval.Image = new Icon(Properties.Resources.activate, new Size(16, 16)).ToBitmap();
            Title = "Client Health";
        }

        public override void InitializePageControl()
        {
            base.InitializePageControl();

            if(!PropertyManager["IsClient"].BooleanValue)
            {
                buttonRefresh.Enabled = false;
            }

            Initialized = true;
        }

        private void buttonSURefresh_Click(object sender, EventArgs e)
        {
            listViewListClientHealth.IsLoading = true;
            listViewListClientHealth.UpdateColumnWidth(columnHeaderDescription);
            listViewListClientHealth.Items.Clear();
            
            backgroundWorker = new BackgroundWorker();
            backgroundWorker.DoWork += new DoWorkEventHandler(infoWorker_DoWork);
            backgroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(infoWorker_RunWorkerCompleted);
            backgroundWorker.WorkerSupportsCancellation = false;
            backgroundWorker.WorkerReportsProgress = false;
            buttonRefresh.Enabled = false;
            backgroundWorker.RunWorkerAsync();
        }

        private void infoWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            CredentialCache netCache = new CredentialCache();

            try
            {
                netCache.Add(new Uri(string.Format(@"\\{0}", PropertyManager["Name"].StringValue)), "Digest", CredentialCache.DefaultNetworkCredentials);

                string clientHealthFile = string.Format(@"\\{0}\admin$\CCM\CcmEvalReport.xml", PropertyManager["Name"].StringValue);

                XmlDocument xmldoc = new XmlDocument();
                xmldoc.Load(clientHealthFile);
                XmlNode healthChecks = xmldoc.DocumentElement.SelectSingleNode("/ClientHealthReport/HealthChecks");

                foreach (XmlNode node in healthChecks.ChildNodes)
                {
                    listViewListClientHealth.Items.Add(new ListViewItem()
                    {
                        Text = node.Attributes["Description"]?.InnerText,
                        SubItems = {
                            node.InnerText
                        }
                    });
                }

                XmlNode summary = xmldoc.DocumentElement.SelectSingleNode("/ClientHealthReport/Summary");
                DateTime time = DateTime.Parse(summary.Attributes["EvaluationTime"]?.InnerText);
                labelLastScan.Text = time.ToString("yyyy/MM/dd hh:mm:ss tt");
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(string.Format("{0}: {1}", ex.GetType().Name, ex.Message));
            }
            finally
            {
                netCache.Remove(new Uri(string.Format(@"\\{0}", PropertyManager["Name"].StringValue)), "Digest");
            }
        }

        private void infoWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                if (e.Error != null)
                {
                    using (SccmExceptionDialog sccmExceptionDialog = new SccmExceptionDialog(e.Error))
                    {
                        int num = (int)sccmExceptionDialog.ShowDialog();
                    }
                }
            }
            finally
            {
                if (sender as BackgroundWorker == backgroundWorker)
                {
                    backgroundWorker.Dispose();
                    backgroundWorker = null;
                    Cursor = Cursors.Default;
                    listViewListClientHealth.IsLoading = false;
                    buttonRefresh.Enabled = true;
                }
            }
        }

        private void buttonEval_Click(object sender, EventArgs e)
        {
            buttonEval.Enabled = false;

            try
            {
                ManagementClass clientaction = new ManagementClass(string.Format(@"\\{0}\root\cimv2:Win32_Process", PropertyManager["Name"].StringValue));
                object[] methodArgs = { @"C:\WINDOWS\CCM\ccmeval.exe" };
                clientaction.InvokeMethod("Create", methodArgs);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(string.Format("{0}: {1}", ex.GetType().Name, ex.Message));
            }
            finally
            {
                buttonEval.Enabled = true;
            }
        }
    }
}
