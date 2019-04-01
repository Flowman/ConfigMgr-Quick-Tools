using Microsoft.ConfigurationManagement.AdminConsole;
using Microsoft.ConfigurationManagement.AdminConsole.DialogFramework;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Net;
using System.Management;
using System.IO;
using System.Xml.Linq;
using System.Linq;
using System.Globalization;
using System.Text;

namespace ConfigMgr.QuickTools.Device.PropertiesDialog
{
    public partial class ResultClientHealthControl : SmsPageControl
    {
        private BackgroundWorker backgroundWorker;

        public ResultClientHealthControl(SmsPageData pageData)
          : base(pageData)
        {
            InitializeComponent();
            buttonRefresh.Image = new Icon(Properties.Resources.reload, new Size(16, 16)).ToBitmap();
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

        private void ButtonSURefresh_Click(object sender, EventArgs e)
        {
            listViewListClientHealth.IsLoading = true;
            listViewListClientHealth.UpdateColumnWidth(columnHeaderDescription);
            listViewListClientHealth.Items.Clear();
            
            backgroundWorker = new BackgroundWorker();
            backgroundWorker.DoWork += new DoWorkEventHandler(InfoWorker_DoWork);
            backgroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(InfoWorker_RunWorkerCompleted);
            backgroundWorker.WorkerSupportsCancellation = false;
            backgroundWorker.WorkerReportsProgress = false;
            buttonRefresh.Enabled = false;
            Cursor = Cursors.WaitCursor;
            backgroundWorker.RunWorkerAsync();
        }

        private void InfoWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            CredentialCache netCache = new CredentialCache();

            try
            {
                netCache.Add(new Uri(string.Format(@"\\{0}", PropertyManager["Name"].StringValue)), "Digest", CredentialCache.DefaultNetworkCredentials);

                string clientHealthFile = string.Format(@"\\{0}\admin$\CCM\CcmEvalReport.xml", PropertyManager["Name"].StringValue);

                if (File.Exists(clientHealthFile))
                {
                    XElement clientHealth = XElement.Load(clientHealthFile);
                    XNamespace ns = clientHealth.GetDefaultNamespace();

                    foreach (XElement node in clientHealth.Descendants(ns + "HealthCheck"))
                    {
                        listViewListClientHealth.Items.Add(new ListViewItem()
                        {
                            Text = node.Attribute("Description").Value,
                            SubItems = { node.Value }
                        });
                    }

                    XElement summary = clientHealth.Descendants(ns + "Summary").First();
                    if (summary != null)
                    {
                        DateTimeFormatInfo formatInfo = CultureInfo.CurrentUICulture.DateTimeFormat;
                        DateTime time = DateTime.Parse(summary.Attribute("EvaluationTime").Value);
                        labelLastScan.Text = time.ToString(formatInfo);
                    }
                }
                else
                {
                    labelLastScan.Text = "CcmEvalReport.xml has not been generated";
                }
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

        private void InfoWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
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
                    listViewListClientHealth.UpdateColumnWidth(columnHeaderDescription);
                    buttonRefresh.Enabled = true;
                }
            }
        }

        private void ButtonEval_Click(object sender, EventArgs e)
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

        private void ListViewListClientHealth_CopyKeyEvent(object sender, EventArgs e)
        {
            StringBuilder buffer = new StringBuilder();
            foreach (ListViewItem item in listViewListClientHealth.SelectedItems)
            {
                foreach (ListViewItem.ListViewSubItem subitem in item.SubItems)
                {
                    buffer.Append(subitem.Text);
                    buffer.Append("\t");
                }
                buffer.AppendLine();
            }
            buffer.Remove(buffer.Length - 1, 1);
            Clipboard.SetData(DataFormats.Text, buffer.ToString());
        }
    }
}
