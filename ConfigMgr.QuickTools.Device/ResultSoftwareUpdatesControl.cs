using Microsoft.ConfigurationManagement.AdminConsole;
using Microsoft.ConfigurationManagement.AdminConsole.DialogFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Management;

namespace ConfigMgr.QuickTools.Device.PropertiesDialog
{
    public partial class ResultSoftwareUpdatesControl : SmsPageControl
    {
        private BackgroundWorker backgroundWorker;

        public ResultSoftwareUpdatesControl(SmsPageData pageData)
          : base(pageData)
        {
            InitializeComponent();

            buttonSURefresh.Image = new Icon(Properties.Resources.reload, new Size(16, 16)).ToBitmap();

            Title = "Software Updates";
        }

        public override void InitializePageControl()
        {
            base.InitializePageControl();

            if (!PropertyManager["IsClient"].BooleanValue)
            {
                buttonSURefresh.Enabled = false;
            }

            Initialized = true;
        }

        private void buttonSURefresh_Click(object sender, EventArgs e)
        {
            listViewSoftwareUpdates.IsLoading = true;
            listViewSoftwareUpdates.UpdateColumnWidth(columnHeaderTitle);
            listViewSoftwareUpdates.Items.Clear();

            backgroundWorker = new BackgroundWorker();
            backgroundWorker.DoWork += new DoWorkEventHandler(InfoWorker_DoWork);
            backgroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(InfoWorker_RunWorkerCompleted);
            backgroundWorker.WorkerSupportsCancellation = false;
            backgroundWorker.WorkerReportsProgress = false;
            buttonSURefresh.Enabled = false;
            UseWaitCursor = true;
            backgroundWorker.RunWorkerAsync();
        }

        private void InfoWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                ManagementScope scope = Utility.GetWMIScope(PropertyManager["Name"].StringValue, @"ccm\SoftwareUpdates\UpdatesStore");

                ObjectQuery query = new ObjectQuery("SELECT * FROM CCM_UpdateStatus");
                IEnumerable<IGrouping<string, ManagementObject>> updates = Utility.SearchWMI(scope, query).GroupBy(update => (string)update.Properties["Article"].Value, update => update);

                foreach (IGrouping<string, ManagementObject> update in updates)
                {
                    listViewSoftwareUpdates.Items.Add(new ListViewItem()
                    {
                        Text = (string)update.First().Properties["Article"].Value,
                        SubItems = {
                            (string) update.First().Properties["Title"].Value,
                            (string) update.First().Properties["Status"].Value
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(string.Format("{0}: {1}", ex.GetType().Name, ex.Message));
            }
        }

        private void InfoWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                if (e.Error != null)
                    SccmExceptionDialog.ShowDialog(this, e.Error, "Error");
            }
            finally
            {
                if (sender as BackgroundWorker == backgroundWorker)
                {
                    backgroundWorker.Dispose();
                    backgroundWorker = null;
                    UseWaitCursor = false;
                    listViewSoftwareUpdates.IsLoading = false;
                    listViewSoftwareUpdates.UpdateColumnWidth(columnHeaderTitle);
                    buttonSURefresh.Enabled = true;
                }
            }
        }

        private void ListView_CopyKeyEvent(object sender, EventArgs e)
        {
            Utility.CopyToClipboard((ListView)sender);
        }
    }
}
