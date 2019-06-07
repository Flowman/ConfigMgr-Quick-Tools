
using Microsoft.ConfigurationManagement.AdminConsole;
using Microsoft.ConfigurationManagement.ManagementProvider;
using Microsoft.ConfigurationManagement.AdminConsole.DialogFramework;
using Microsoft.ConfigurationManagement.AdminConsole.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using System.Windows;

namespace ConfigMgr.QuickTools.CollectionManagment
{
    public partial class AddResourcesListDialog : SmsCustomDialog
    {
        private long searchCompleteTime = -1L;
        private readonly ControlsInspector controlsInspector;
        private SmsBackgroundWorker backgroundWorkerQueryMachine;

        public AddResourcesListDialog()
        {
            InitializeComponent();

            Title = "Add Resources to Collection";

            Updater.CheckUpdates();

            controlsInspector = new ControlsInspector();
            controlsInspector.AddControl(listViewSelectedResources, new ControlDataStateEvaluator(ValidateResourceValue), "Search for resources");
            controlsInspector.InspectAll();
        }

        private ControlDataState ValidateResourceValue()
        {
            return listViewSelectedResources.Items.Count > 0 ? ControlDataState.Valid : ControlDataState.Invalid;
        }

        private void ButtonSearch_Click(object sender, EventArgs e)
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
                if (string.IsNullOrWhiteSpace(textBoxSeachList.Text))
                    return;
                
                List<string> resourceList = new List<string>();
                foreach (string line in textBoxSeachList.Lines)
                {
                    resourceList.Add(line);
                }

                string query = string.Format("SELECT * From SMS_R_System WHERE Name IN ('{0}') ORDER by Name", string.Join("','", resourceList));

                backgroundWorkerQueryMachine = new SmsBackgroundWorker(20);
                backgroundWorkerQueryMachine.QueryProcessorObjectsReady += new EventHandler<QueryProcessorObjectsEventArgs>(BackgroundWorkerQueryMachine_QueryProcessorObjectsEvent);
                backgroundWorkerQueryMachine.QueryProcessorCompleted += new EventHandler<RunWorkerCompletedEventArgs>(BackgroundWorkerQueryMachine_QueryProcessorCompleted);
                buttonSearch.Text = "Stop";
                UseWaitCursor = true;
                listViewSelectedResources.Items.Clear();
                ConnectionManager.QueryProcessor.ProcessQuery(backgroundWorkerQueryMachine, query);
            }
        }

        private void BackgroundWorkerQueryMachine_QueryProcessorObjectsEvent(object sender, QueryProcessorObjectsEventArgs e)
        {
            List<ListViewItem> list = new List<ListViewItem>();
            foreach (IResultObject resultObject in e.ResultObjects)
            {
                string name = !resultObject.PropertyList.ContainsKey("Name") ? resultObject["ResourceID"].LongValue.ToString() : resultObject["Name"].StringValue;
                string domain = resultObject["ResourceDomainORWorkgroup"].ObjectValue != null ? resultObject["ResourceDomainORWorkgroup"].StringValue : string.Empty;
                string siteCode = (resultObject["SMSAssignedSites"].ObjectValue != null && resultObject["SMSAssignedSites"].StringArrayValue.Length > 0) ? resultObject["SMSAssignedSites"].StringArrayValue[0] : string.Empty;

                ListViewItem listViewItem = new ListViewItem()
                {
                    Text = name,
                    SubItems = {
                        domain,
                        siteCode
                    },
                    Tag = resultObject,
                    Selected = true
                };

                list.Add(listViewItem);
            }
            listViewSelectedResources.BeginUpdate();
            listViewSelectedResources.Items.AddRange(list.ToArray());
            listViewSelectedResources.EndUpdate();
        }

        private void BackgroundWorkerQueryMachine_QueryProcessorCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                if (e.Error != null)
                {
                    SccmExceptionDialog.ShowDialog(this, e.Error, "Error");
                }
                else if (e.Cancelled)
                {
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
            }
            finally
            {
                backgroundWorkerQueryMachine.Dispose();
                backgroundWorkerQueryMachine = null;
                UseWaitCursor = false;
                buttonSearch.Focus();
                buttonSearch.Text = "Search";
                searchCompleteTime = DateTime.Now.Ticks;
                controlsInspector.InspectAll();
                UtilitiesClass.UpdateListViewColumnsSize(listViewSelectedResources, columnMachineName);
            }
        }

        private void AddToCollectionAndClose()
        {
            List<IResultObject> list = new List<IResultObject>();
            foreach (ListViewItem listViewItem in listViewSelectedResources.Items)
            {
                IResultObject resource = (IResultObject)listViewItem.Tag;
                IResultObject instance = ConnectionManager.CreateEmbeddedObjectInstance("SMS_CollectionRuleDirect");
                instance["ResourceClassName"].StringValue = "SMS_R_System";
                instance["RuleName"].StringValue = resource["Name"].StringValue;
                instance["ResourceID"].IntegerValue = resource["ResourceID"].IntegerValue;
                list.Add(instance);
            }

            SelectedObject.ExecuteMethod("AddMembershipRules", new Dictionary<string, object>()
            {
                {
                    "collectionRules",
                    list
                }
            });

            DataUpdatedDelegate(this, new List<PropertyDataUpdateItem>()
            {
                new PropertyDataUpdateItem(SelectedObject, PropertyDataUpdateAction.Update)
            });

            DialogResult = DialogResult.OK;
            Close();
        }

        private void ListViewSelectedResources_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (listViewSelectedResources.SelectedItems.Count == 0)
                return;

            listViewSelectedResources.BeginUpdate();
            while (listViewSelectedResources.SelectedItems.Count > 0)
            {
                listViewSelectedResources.Items.Remove(listViewSelectedResources.SelectedItems[0]);
            }
            listViewSelectedResources.EndUpdate();

            UtilitiesClass.UpdateListViewColumnsSize(listViewSelectedResources, columnMachineName);

            controlsInspector.InspectAll();
        }

        private void ButtonOK_Click(object sender, EventArgs e)
        {
            if (listViewSelectedResources.Items.Count == 0)
            {
                System.Windows.MessageBox.Show("Select one or more resources to add to the collection.", "Configuration Manager", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                listViewSelectedResources.Focus();
            }
            else
            {
                AddToCollectionAndClose();
            }
        }

        private void ButtonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void ButtonClear_Click(object sender, EventArgs e)
        {
            listViewSelectedResources.Items.Clear();
        }
    }
}
