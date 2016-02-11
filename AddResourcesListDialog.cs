
using Microsoft.ConfigurationManagement.AdminConsole;
using Microsoft.ConfigurationManagement.ManagementProvider;
using Microsoft.ConfigurationManagement.AdminConsole.DialogFramework;
using Microsoft.ConfigurationManagement.AdminConsole.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using System.Windows;
using System.Diagnostics;

namespace Zetta.ConfigMgr.QuickTools
{
    public partial class AddResourcesListDialog : SmsCustomDialog
    {
        private long searchCompleteTime = -1L;
        private ControlsInspector controlsInspector;
        private SmsBackgroundWorker backgroundWorkerQueryMachine;

        public AddResourcesListDialog()
        {
            InitializeComponent();

            controlsInspector = new ControlsInspector();
            controlsInspector.AddControl(listViewSelectedResources, new ControlDataStateEvaluator(ValidateResourceValue), "Search for resources");
            controlsInspector.InspectAll();

            Title = "Add Resources to Collection";
        }

        private ControlDataState ValidateResourceValue()
        {
            return listViewSelectedResources.Items.Count > 0 ? ControlDataState.Valid : ControlDataState.Invalid;
        }

        private void buttonSearch_Click(object sender, EventArgs e)
        {
            if (searchCompleteTime > 0L && DateTime.Now.Ticks - searchCompleteTime < SystemInformation.DoubleClickTime * 5000L)
                return;
            if (backgroundWorkerQueryMachine != null && backgroundWorkerQueryMachine.IsBusy)
            {
                backgroundWorkerQueryMachine.CancelAsync();
                UseWaitCursor = false;
            }
            else
            {
                List<string> resourceList = new List<string>();

                foreach (string line in textBoxSeachList.Lines)
                {
                    resourceList.Add(line);
                }

                string query = string.Format("SELECT * From SMS_R_System WHERE Name IN ('{0}') ORDER by Name", string.Join("','", resourceList));

                backgroundWorkerQueryMachine = new SmsBackgroundWorker(20);
                backgroundWorkerQueryMachine.QueryProcessorObjectsReady += new EventHandler<QueryProcessorObjectsEventArgs>(backgroundWorkerQueryMachine_QueryProcessorObjectsEvent);
                backgroundWorkerQueryMachine.QueryProcessorCompleted += new EventHandler<RunWorkerCompletedEventArgs>(backgroundWorkerQueryMachine_QueryProcessorCompleted);
                buttonSearch.Text = "Stop";
                UseWaitCursor = true;
                listViewSelectedResources.Items.Clear();
                ConnectionManager.QueryProcessor.ProcessQuery(backgroundWorkerQueryMachine, query);
            }
        }

        private void backgroundWorkerQueryMachine_QueryProcessorObjectsEvent(object sender, QueryProcessorObjectsEventArgs e)
        {
            List<ListViewItem> list = new List<ListViewItem>();
            foreach (IResultObject resultObject in e.ResultObjects)
            {
                ListViewItem listViewItem = new ListViewItem();
                listViewItem.Text = !resultObject.PropertyList.ContainsKey("Name") ? resultObject["ResourceID"].LongValue.ToString() : resultObject["Name"].StringValue;
                string text1 = string.Empty;
                if (resultObject["ResourceDomainORWorkgroup"].ObjectValue != null)
                    text1 = resultObject["ResourceDomainORWorkgroup"].StringValue;
                listViewItem.SubItems.Add(text1);
                string text2 = string.Empty;
                if (resultObject["SMSAssignedSites"].ObjectValue != null && resultObject["SMSAssignedSites"].StringArrayValue.Length > 0)
                    text2 = resultObject["SMSAssignedSites"].StringArrayValue[0];
                listViewItem.SubItems.Add(text2);
                listViewItem.Tag = resultObject;
                listViewItem.Selected = true;
                list.Add(listViewItem);
            }
            listViewSelectedResources.BeginUpdate();
            listViewSelectedResources.Items.AddRange(list.ToArray());
            listViewSelectedResources.EndUpdate();
        }

        private void backgroundWorkerQueryMachine_QueryProcessorCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                if (e.Error != null)
                {
                    string message = string.Format("Error: {0}", listViewSelectedResources.Items.Count);
                    SccmExceptionDialog.ShowDialog(this, e.Error, message);
                }
                else if (e.Cancelled)
                {
                    ConnectionManagerBase.SmsTraceSource.TraceEvent(TraceEventType.Information, 1, "User canceled");
                    if (listViewSelectedResources.Items.Count > 0)
                    {
                        listViewSelectedResources.Items[0].Selected = true;
                        listViewSelectedResources.Items[0].Focused = true;
                    }
                }
                else if (listViewSelectedResources.Items.Count > 0)
                {
                    listViewSelectedResources.Items[0].Selected = true;
                    listViewSelectedResources.Items[0].Focused = true;
                }
                UtilitiesClass.UpdateListViewColumnsSize(listViewSelectedResources, columnMachineName);
                buttonSearch.Focus();
                buttonSearch.Text = "Search";
                UseWaitCursor = false;
                searchCompleteTime = DateTime.Now.Ticks;
                controlsInspector.InspectAll();
            }
            finally
            {
                backgroundWorkerQueryMachine.Dispose();
                backgroundWorkerQueryMachine = null;
            }
        }

        private void AddToCollectionAndClose()
        {
            List<IResultObject> list = new List<IResultObject>();
            foreach (ListViewItem listViewItem in listViewSelectedResources.Items)
            {
                IResultObject resource = (IResultObject)listViewItem.Tag;
                IResultObject embeddedObjectInstance = ConnectionManager.CreateEmbeddedObjectInstance("SMS_CollectionRuleDirect");
                embeddedObjectInstance["ResourceClassName"].StringValue = "SMS_R_System";
                embeddedObjectInstance["RuleName"].StringValue = resource["Name"].StringValue;
                embeddedObjectInstance["ResourceID"].IntegerValue = resource["ResourceID"].IntegerValue;
                list.Add(embeddedObjectInstance);
            }
            SelectedObject.ExecuteMethod("AddMembershipRules", new Dictionary<string, object>()
            {
                {
                    "collectionRules",
                    list
                }
            });
            int num = DataUpdatedDelegate(this, new List<PropertyDataUpdateItem>()
            {
                new PropertyDataUpdateItem(SelectedObject, PropertyDataUpdateAction.Update)
            }) ? 1 : 0;
            DialogResult = DialogResult.OK;
            Close();
        }

        private void listViewSelectedResources_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (listViewSelectedResources.SelectedItems.Count == 0)
                return;
            listViewSelectedResources.BeginUpdate();
            while (listViewSelectedResources.SelectedItems.Count > 0)
                listViewSelectedResources.Items.Remove(listViewSelectedResources.SelectedItems[0]);
            listViewSelectedResources.EndUpdate();
            UtilitiesClass.UpdateListViewColumnsSize(listViewSelectedResources, columnMachineName);
            controlsInspector.InspectAll();
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            if (listViewSelectedResources.Items.Count == 0)
            {
                System.Windows.MessageBox.Show("Select one or more resources to add to the collection.", "Configuration Manager", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                listViewSelectedResources.Focus();
            }
            else
                AddToCollectionAndClose();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void buttonClear_Click(object sender, EventArgs e)
        {
            listViewSelectedResources.Items.Clear();
        }
    }
}
