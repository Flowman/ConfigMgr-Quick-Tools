using Microsoft.ConfigurationManagement.AdminConsole;
using Microsoft.ConfigurationManagement.AdminConsole.DialogFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Management;
using System.Text;

namespace Zetta.ConfigMgr.QuickTools
{
    public partial class ResultSoftwareUpdatesControl : SmsPageControl
    {
        private BackgroundWorker backgroundWorker;

        public ResultSoftwareUpdatesControl(SmsPageData pageData)
          : base(pageData)
        {
            InitializeComponent();
            buttonSURefresh.Image = new Icon(Properties.Resources.update, new Size(16, 16)).ToBitmap();
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
            listViewListSoftwareUpdates.IsLoading = true;
            listViewListSoftwareUpdates.UpdateColumnWidth(columnHeaderTitle);
            listViewListSoftwareUpdates.Items.Clear();

            backgroundWorker = new BackgroundWorker();
            backgroundWorker.DoWork += new DoWorkEventHandler(infoWorker_DoWork);
            backgroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(infoWorker_RunWorkerCompleted);
            backgroundWorker.WorkerSupportsCancellation = false;
            backgroundWorker.WorkerReportsProgress = false;
            buttonSURefresh.Enabled = false;
            backgroundWorker.RunWorkerAsync();
        }

        private void infoWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                ManagementScope scope = Utility.GetWMIScope(PropertyManager["Name"].StringValue, @"ccm\SoftwareUpdates\UpdatesStore");

                ObjectQuery query = new ObjectQuery("SELECT * FROM CCM_UpdateStatus");
                IEnumerable<IGrouping<string, ManagementObject>> updates = Utility.SearchWMI(scope, query).GroupBy(update => (string)update.Properties["Article"].Value, update => update);

                foreach (IGrouping<string, ManagementObject> update in updates)
                {
                    listViewListSoftwareUpdates.Items.Add(new ListViewItem()
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
                    listViewListSoftwareUpdates.IsLoading = false;
                    listViewListSoftwareUpdates.UpdateColumnWidth(columnHeaderTitle);
                    buttonSURefresh.Enabled = true;
                }
            }
        }

        private void listViewListSoftwareUpdates_CopyKeyEvent(object sender, EventArgs e)
        {
            StringBuilder buffer = new StringBuilder();
            foreach (ListViewItem item in listViewListSoftwareUpdates.SelectedItems)
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
