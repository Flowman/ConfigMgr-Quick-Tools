using Microsoft.ConfigurationManagement.AdminConsole;
using Microsoft.ConfigurationManagement.AdminConsole.Common;
using Microsoft.ConfigurationManagement.AdminConsole.DialogFramework;
using Microsoft.ConfigurationManagement.ManagementProvider;
using System;
using System.ComponentModel;
using System.Windows.Forms;
using System.Diagnostics;
using System.Text;

namespace Zetta.ConfigMgr.QuickTools
{
    public partial class ResultCollectionsControl : SmsPageControl
    {
        private SmsBackgroundWorker backgroundWorker;

        public ResultCollectionsControl(SmsPageData pageData)
          : base(pageData)
        {
            InitializeComponent();
            Title = "Collections";
        }

        public override void InitializePageControl()
        {
            base.InitializePageControl();
            listViewListCollections.UpdateColumnWidth(columnHeaderCollection);
            listViewListCollections.Items.Clear();

            listViewListCollections.IsLoading = true;
            string query = string.Format("SELECT SMS_Collection.* FROM SMS_FullCollectionMembership, SMS_Collection where ResourceID = '{0}' and SMS_FullCollectionMembership.CollectionID = SMS_Collection.CollectionID", PropertyManager["ResourceID"].IntegerValue);

            backgroundWorker = new SmsBackgroundWorker();
            backgroundWorker.QueryProcessorCompleted += new EventHandler<RunWorkerCompletedEventArgs>(backgroundWorker_RunWorkerCompleted);
            backgroundWorker.QueryProcessorObjectsReady += new EventHandler<QueryProcessorObjectsEventArgs>(backgroundWorker_QueryProcessorObjectsReady);
            ConnectionManagerBase.SmsTraceSource.TraceEvent(TraceEventType.Information, 1, "InitializePageControl");
            Cursor = Cursors.WaitCursor;
            QueryProcessor.ProcessQuery(backgroundWorker, query);
        }

        private void backgroundWorker_QueryProcessorObjectsReady(object sender, QueryProcessorObjectsEventArgs e)
        {
            ConnectionManagerBase.SmsTraceSource.TraceEvent(TraceEventType.Information, 1, "backgroundWorker1_QueryProcessorObjectsReady");
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

        private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                ConnectionManagerBase.SmsTraceSource.TraceEvent(TraceEventType.Information, 1, "backgroundWorker1_RunWorkerCompleted");
                if (e.Error != null)
                    SccmExceptionDialog.ShowDialog(this, e.Error, "Error");
                else if (e.Cancelled)
                    ConnectionManagerBase.SmsTraceSource.TraceEvent(TraceEventType.Information, 1, "User canceled");
                else
                    Initialized = true;
            }
            finally
            {
                Cursor = Cursors.Default;
                backgroundWorker.Dispose();
                backgroundWorker = null;
                listViewListCollections.IsLoading = false;
                listViewListCollections.UpdateColumnWidth(columnHeaderCollection);
            }
        }

        private void listViewListCollections_CopyKeyEvent(object sender, EventArgs e)
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
