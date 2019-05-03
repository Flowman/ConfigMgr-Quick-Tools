using Microsoft.ConfigurationManagement.AdminConsole;
using Microsoft.ConfigurationManagement.AdminConsole.Common;
using Microsoft.ConfigurationManagement.AdminConsole.DialogFramework;
using Microsoft.ConfigurationManagement.ManagementProvider;
using System;
using System.ComponentModel;
using System.Windows.Forms;
using System.Diagnostics;
using System.Text;

namespace ConfigMgr.QuickTools.Device.PropertiesDialog
{
    public partial class ResultCollectionsControl : SmsPageControl
    {
        private SmsBackgroundWorker backgroundWorker;

        public ResultCollectionsControl(SmsPageData pageData)
          : base(pageData)
        {
            InitializeComponent();
            Title = "Collections";

            Updater.CheckUpdates();
        }

        public override void InitializePageControl()
        {
            base.InitializePageControl();

            listViewListCollections.UpdateColumnWidth(columnHeaderCollection);
            listViewListCollections.Items.Clear();
            listViewListCollections.IsLoading = true;

            string query = string.Format("SELECT SMS_Collection.* FROM SMS_FullCollectionMembership, SMS_Collection where ResourceID = '{0}' and SMS_FullCollectionMembership.CollectionID = SMS_Collection.CollectionID", PropertyManager["ResourceID"].IntegerValue);

            backgroundWorker = new SmsBackgroundWorker();
            backgroundWorker.QueryProcessorCompleted += new EventHandler<RunWorkerCompletedEventArgs>(BackgroundWorker_RunWorkerCompleted);
            backgroundWorker.QueryProcessorObjectsReady += new EventHandler<QueryProcessorObjectsEventArgs>(BackgroundWorker_QueryProcessorObjectsReady);
            ConnectionManagerBase.SmsTraceSource.TraceEvent(TraceEventType.Information, 1, "InitializePageControl");
            Cursor = Cursors.WaitCursor;
            QueryProcessor.ProcessQuery(backgroundWorker, query);
        }

        private void BackgroundWorker_QueryProcessorObjectsReady(object sender, QueryProcessorObjectsEventArgs e)
        {
            if (e.ResultObjects == null)
                return;

            foreach (IResultObject resultObject in e.ResultObjects)
            {
                listViewListCollections.Items.Add(new ListViewItem()
                {
                    Text = resultObject["Name"].StringValue,
                    SubItems = {
                        ResourceDisplayClass.GetAliasDisplayText(resultObject, "CollectionID")
                    }
                });
                resultObject.Dispose();
            }
        }

        private void BackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                if (e.Error != null)
                    SccmExceptionDialog.ShowDialog(this, e.Error, "Error");
                else if (e.Cancelled)
                    ConnectionManagerBase.SmsTraceSource.TraceEvent(TraceEventType.Information, 1, "User canceled");
                else
                    Initialized = true;
            }
            finally
            {
                if (sender as SmsBackgroundWorker == backgroundWorker)
                {
                    backgroundWorker.Dispose();
                    backgroundWorker = null;
                    Cursor = Cursors.Default;
                    listViewListCollections.IsLoading = false;
                    listViewListCollections.UpdateColumnWidth(columnHeaderCollection);
                }
            }
        }

        private void ListViewListCollections_CopyKeyEvent(object sender, EventArgs e)
        {
            StringBuilder buffer = new StringBuilder();
            foreach (ListViewItem item in listViewListCollections.SelectedItems)
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
