using Microsoft.ConfigurationManagement.AdminConsole;
using Microsoft.ConfigurationManagement.ManagementProvider;
using Microsoft.ConfigurationManagement.AdminConsole.DialogFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using System.Diagnostics;
using System.Management;
using System.Drawing;
using System.Globalization;

namespace ConfigMgr.QuickTools.SoftwareUpdates
{
    public partial class CleanSoftwareUpdatesPage : SmsPageControl
    {
        private SmsBackgroundWorker backgroundWorker;

        public CleanSoftwareUpdatesPage(SmsPageData pageData)
            : base(pageData)
        {
            InitializeComponent();

            FormTitle = "Clean Up Software Updates";
            Title = "Select Updates";
            Headline = "Clean up selected software updates from groups";

            Updater.CheckUpdates();
        }

        public override void InitializePageControl()
        {
            base.InitializePageControl();

            dataGridViewUpdates.Rows.Clear();

            ControlsInspector.AddControl(dataGridViewUpdates, new ControlDataStateEvaluator(ValidateSelectedUpdatesPackages), "Select updates to remove");

            string query = string.Format("SELECT SU.CI_ID,SU.LocalizedDisplayName,SU.IsExpired,SU.IsSuperseded,SU.ArticleID FROM SMS_SoftwareUpdate AS SU JOIN SMS_CIRelation AS CIR ON SU.CI_ID = CIR.ToCIID WHERE CIR.RelationType = 1 AND (SU.IsExpired = 1 OR SU.IsSuperseded = 1)");

            backgroundWorker = new SmsBackgroundWorker();
            backgroundWorker.QueryProcessorCompleted += new EventHandler<RunWorkerCompletedEventArgs>(BackgroundWorker_RunWorkerCompleted);
            backgroundWorker.QueryProcessorObjectsReady += new EventHandler<QueryProcessorObjectsEventArgs>(BackgroundWorker_QueryProcessorObjectsReady);
            UseWaitCursor = true;
            QueryProcessor.ProcessQuery(backgroundWorker, query);
        }

        public override bool OnDeactivate()
        {
            dataGridViewUpdates.EndEdit();
            return base.OnDeactivate();
        }

        private void BackgroundWorker_QueryProcessorObjectsReady(object sender, QueryProcessorObjectsEventArgs e)
        {
            if (e.ResultObjects == null)
                return;

            foreach (IResultObject resultObject in e.ResultObjects)
            {
                DataGridViewRow dataGridViewRow = new DataGridViewRow();
                dataGridViewRow.CreateCells(dataGridViewUpdates);
                dataGridViewRow.Cells[0] = new DataGridViewImageCell
                {
                    Value = resultObject["IsExpired"].BooleanValue ? new Icon(Properties.Resources.expiredupdate, new Size(16, 16)).ToBitmap() : new Icon(Properties.Resources.supersededupdate, new Size(16, 16)).ToBitmap()
                };
                dataGridViewRow.Cells[1].Value = false;
                dataGridViewRow.Cells[2].Value = resultObject["LocalizedDisplayName"].StringValue;
                dataGridViewRow.Cells[3].Value = resultObject["ArticleID"].StringValue;

                dataGridViewRow.Tag = resultObject;
                dataGridViewUpdates.Rows.Add(dataGridViewRow);
            }
        }

        private void BackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                if (e.Error != null)
                {
                    using (SccmExceptionDialog sccmExceptionDialog = new SccmExceptionDialog(e.Error))
                    {
                        sccmExceptionDialog.ShowDialog();
                    }
                }
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
                    UseWaitCursor = false;
                    dataGridViewUpdates.Sort(columnTitle, ListSortDirection.Ascending);
                    Utility.UpdateDataGridViewColumnsSize(dataGridViewUpdates, columnTitle);
                }
            }
        }

        public override void PostApply(BackgroundWorker worker, DoWorkEventArgs e)
        {
            try
            {
                Dictionary<string, string> error = new Dictionary<string, string>();
                List<string> successSUG = new List<string>();
                List<string> successDP = new List<string>();
                List<IResultObject> updates = (List<IResultObject>)UserData["UpdateItems"];
                List<int> updateList = updates.Select(x => x["CI_ID"].IntegerValue).Distinct().ToList();
                string query;

                foreach (IResultObject groupObject in Utility.SearchWMI(ConnectionManager, "SELECT * FROM SMS_AuthorizationList"))
                {
                    // get wmi object instace
                    groupObject.Get();

                    worker.ReportProgress(33, string.Format("Removing update(s) from software update group: {0}", groupObject["LocalizedDisplayName"].StringValue));

                    List<int> compare = updateList.Except(groupObject["Updates"].IntegerArrayValue).ToList();
                    List<int> keep = groupObject["Updates"].IntegerArrayValue.Except(updateList).ToList();

                    bool er = false;

                    if (updateList.Except(compare).ToList().Count > 0)
                    {
                        groupObject["Updates"].IntegerArrayValue = keep.ToArray();
                        try
                        {
                            groupObject.Put();
                            groupObject.Get();
                        }
                        catch (SmsQueryException ex)
                        {
                            ManagementException managementException = ex.InnerException as ManagementException;
                            error.Add("Could not remove updates: " + managementException.ErrorInformation["Description"].ToString(), groupObject["LocalizedDisplayName"].StringValue);
                            er = true;
                        }

                        if (!er)
                        {
                            successSUG.Add(groupObject["LocalizedDisplayName"].StringValue);
                        }
                    }
                }

                if (checkBoxRemoveContent.Checked == true)
                {
                    worker.ReportProgress(50, "Querying package content for removal");
                    
                    Dictionary<string, List<int>> packages = new Dictionary<string, List<int>>();

                    query = string.Format("SELECT SMS_PackageToContent.ContentID,SMS_PackageToContent.PackageID from SMS_PackageToContent JOIN SMS_CIToContent ON SMS_CIToContent.ContentID = SMS_PackageToContent.ContentID WHERE SMS_CIToContent.CI_ID IN ({0}) ORDER by PackageID", string.Join(",", updateList));
                    using (IResultObject resultObject = ConnectionManager.QueryProcessor.ExecuteQuery(query))
                    {
                        foreach (IResultObject resultObject1 in resultObject)
                        {
                            string packageID = resultObject1["PackageID"].StringValue;
                            if (!packages.ContainsKey(packageID))
                                packages.Add(packageID, new List<int>());

                            packages[packageID].Add(resultObject1["ContentID"].IntegerValue);
                        }
                    }

                    foreach(KeyValuePair<string, List<int>> item in packages)
                    {
                        query = string.Format("SELECT * FROM SMS_SoftwareUpdatesPackage WHERE PackageID = '{0}'", item.Key);
                        IResultObject package = Utility.GetFirstWMIInstance(ConnectionManager, query);

                        worker.ReportProgress(66, string.Format("Removing content from deployment package: {0}", package["Name"].StringValue));

                        Dictionary<string, object> methodParameters = new Dictionary<string, object>
                        {
                            { "bRefreshDPs", true },
                            { "ContentIDs", item.Value.ToArray() }
                        };

                        bool er = false;

                        try
                        {
                            package.ExecuteMethod("RemoveContent", methodParameters);
                        }
                        catch (SmsQueryException ex)
                        {
                            ManagementException managementException = ex.InnerException as ManagementException;
                            error.Add("Could not remove content: " + managementException.ErrorInformation["Description"].ToString(), package["Name"].StringValue);
                            er = true;
                        }

                        if (!er)
                        {
                            successDP.Add(package["Name"].StringValue);
                        }
                    }
                }

                PrepareCompletion(successSUG, successDP, error);
                AddRefreshResultObject(null, PropertyDataUpdateAction.RefreshAll);
                base.PostApply(worker, e);
            }
            catch (Exception ex)
            {
                AddRefreshResultObject(null, PropertyDataUpdateAction.RefreshAll);
                PrepareError(ex.Message);
                throw;
            }
        }

        public override void OnAddSummary(SummaryRequestHandler handler)
        {
            base.OnAddSummary(handler);
            PrepareSummary();
        }

        private void RemoveAllSummary()
        {
            foreach (string id in Enumerable.ToList(Enumerable.Select(GetSummaryItems(), i => i.Id)))
                RemoveItem(id);
        }

        private void PrepareSummary()
        {
            RemoveAllSummary();

            List<IResultObject> list = new List<IResultObject>();
            foreach (DataGridViewRow dataGridViewRow in dataGridViewUpdates.Rows)
            {
                if (Convert.ToBoolean(dataGridViewRow.Cells[columnRemove.Name].Value) == true)
                {
                    if (dataGridViewRow.Tag is IResultObject update)
                    {
                        list.Add(update);
                    }
                }
            }
            UserData["UpdateItems"] = list;

            AddAction("GeneralDescription", string.Format("The following update will be cleaned out ({0}):", list.Count));
            AddAction("UpdateInformation", string.Empty);

            foreach (IResultObject update in list)
                AddActionDetailMessage("UpdateInformation", update["LocalizedDisplayName"].StringValue);
        }

        private void PrepareCompletion(List<string> successSUG, List<string> successDP, Dictionary<string, string> error)
        {
            RemoveAllSummary();

            if (successSUG.Count > 0)
            {
                AddAction("SUG", string.Format("The following software update groups where cleaned out ({0}):", successSUG.Count));
                UpdateActionStatus("SUG", SmsSummaryAction.ActionStatus.CompleteWithSuccess);

                foreach (string item in successSUG)
                    AddActionDetailMessage("SUG", item);

                if (successDP.Count > 0)
                {
                    AddAction("EmptyLine", string.Empty);
                    AddAction("DP", string.Format("The following deployment packages where updated ({0}):", successDP.Count));
                    UpdateActionStatus("DP", SmsSummaryAction.ActionStatus.CompleteWithSuccess);

                    foreach (string item in successDP)
                        AddActionDetailMessage("DP", item);
                }

                AddAction("EmptyLine", string.Empty);
            }

            if (error.Count > 0)
            {
                AddAction("UpdateError", string.Format("The following software update groups cannot be disabled ({0}):", error.Count));
                UpdateActionStatus("UpdateError", SmsSummaryAction.ActionStatus.CompleteWithErrors);
                foreach (KeyValuePair<string, string> item in error)
                {
                    AddActionDetailMessage("UpdateError", string.Format("{0}: {1}", item.Value, item.Key));
                }
            }
        }

        private void PrepareError(string errorMessage)
        {
            RemoveAllSummary();
            AddAction("ErrorInfo", errorMessage);
            UpdateActionStatus("ErrorInfo", SmsSummaryAction.ActionStatus.CompleteWithErrors);
        }

        private ControlDataState ValidateSelectedUpdatesPackages()
        {
            foreach (DataGridViewRow dataGridViewRow in dataGridViewUpdates.Rows)
            {
                if (Convert.ToBoolean(dataGridViewRow.Cells[1].Value, CultureInfo.InvariantCulture))
                    return ControlDataState.Valid;
            }
            return ControlDataState.Invalid;
        }

        private void DataGridViewUpdates_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (!dataGridViewUpdates.IsCurrentCellDirty)
                return;
            dataGridViewUpdates.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }

        private void DataGridViewUpdates_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            ControlsInspector.InspectAll();
            Dirty = !ReadOnly;
        }

        private void ButtonDeselectAll_Click(object sender, EventArgs e)
        {
            if (dataGridViewUpdates.Rows.Count > 0)
            {
                dataGridViewUpdates.BeginEdit(true);
                foreach (DataGridViewRow dataGridViewRow in dataGridViewUpdates.Rows)
                    dataGridViewRow.Cells[1].Value = false;
                dataGridViewUpdates.EndEdit();
            }
        }

        private void ButtonSelectSuperseded_Click(object sender, EventArgs e)
        {
            if (dataGridViewUpdates.Rows.Count > 0)
            {
                dataGridViewUpdates.BeginEdit(true);
                foreach (DataGridViewRow dataGridViewRow in dataGridViewUpdates.Rows)
                {
                    IResultObject resultObject = dataGridViewRow.Tag as IResultObject;
                    if (resultObject["IsSuperseded"].BooleanValue)
                        dataGridViewRow.Cells[1].Value = true;
                }
                dataGridViewUpdates.EndEdit();
            }
        }

        private void ButtonSelectExpired_Click(object sender, EventArgs e)
        {
            if (dataGridViewUpdates.Rows.Count > 0)
            {
                dataGridViewUpdates.BeginEdit(true);
                foreach (DataGridViewRow dataGridViewRow in dataGridViewUpdates.Rows)
                {
                    IResultObject resultObject = dataGridViewRow.Tag as IResultObject;
                    if (resultObject["IsExpired"].BooleanValue)
                        dataGridViewRow.Cells[1].Value = true;
                }
                dataGridViewUpdates.EndEdit();
            }
        }
    }
}
