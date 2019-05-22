using Microsoft.ConfigurationManagement.AdminConsole;
using Microsoft.ConfigurationManagement.ManagementProvider;
using Microsoft.ConfigurationManagement.AdminConsole.Common;
using Microsoft.ConfigurationManagement.AdminConsole.DialogFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using System.Diagnostics;
using System.Collections;
using System.Management;
using System.Globalization;

namespace ConfigMgr.QuickTools
{
    public partial class RemoveRetiredContentPage : SmsPageControl
    {
        private SmsBackgroundWorker backgroundWorker;

        public RemoveRetiredContentPage(SmsPageData pageData)
            : base(pageData)
        {
            InitializeComponent();

            pageData.ProgressBarStyle = ProgressBarStyle.Continuous;

            Headline = "Remove content from retired applications";
            Title = "Select Applications";
            FormTitle = "Clean Up Retired Content";

            Updater.CheckUpdates();
        }

        public override void InitializePageControl()
        {
            base.InitializePageControl();

            dataGridViewApplications.Rows.Clear();

            ControlsInspector.AddControl(dataGridViewApplications, new ControlDataStateEvaluator(ValidateSelectedApplications), "Select an application");

            string query = string.Format("SELECT * FROM SMS_Application WHERE IsLatest = 1 AND IsExpired = 1 AND HasContent = 1");

            backgroundWorker = new SmsBackgroundWorker();
            backgroundWorker.QueryProcessorCompleted += new EventHandler<RunWorkerCompletedEventArgs>(BackgroundWorker_RunWorkerCompleted);
            backgroundWorker.QueryProcessorObjectsReady += new EventHandler<QueryProcessorObjectsEventArgs>(BackgroundWorker_QueryProcessorObjectsReady);
            UseWaitCursor = true;
            QueryProcessor.ProcessQuery(backgroundWorker, query);
        }

        public override bool OnDeactivate()
        {
            dataGridViewApplications.EndEdit();
            return base.OnDeactivate();
        }

        private void BackgroundWorker_QueryProcessorObjectsReady(object sender, QueryProcessorObjectsEventArgs e)
        {
            if (e.ResultObjects == null)
                return;

            foreach (IResultObject resultObject in e.ResultObjects)
            {
                DataGridViewRow dataGridViewRow = new DataGridViewRow();
                dataGridViewRow.CreateCells(dataGridViewApplications);

                dataGridViewRow.Cells[0].Value = false;
                dataGridViewRow.Cells[1].Value = resultObject["LocalizedDisplayName"].StringValue;

                dataGridViewRow.Tag = resultObject;
                dataGridViewApplications.Rows.Add(dataGridViewRow);
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
                    dataGridViewApplications.Sort(columnName, ListSortDirection.Ascending);
                    UseWaitCursor = false;
                }
            }
        }

        public override void PostApply(BackgroundWorker worker, DoWorkEventArgs e)
        {
            try
            {
                int num = 0;
                Dictionary<string, string> error = new Dictionary<string, string>();
                List<string> success = new List<string>();
                List<IResultObject> applications = (List<IResultObject>)UserData["ApplicationItems"];
                List<int> applicationsList = applications.Select(x => x["CI_ID"].IntegerValue).Distinct().ToList();
                string query;

                query = string.Format("SELECT SMS_CIContentPackage.PackageID, SMS_Application.LocalizedDisplayName FROM SMS_CIContentPackage JOIN SMS_Application ON SMS_CIContentPackage.CI_ID = SMS_Application.CI_ID WHERE CI_ID IN ({0})", string.Join(",", applicationsList));
                using (IResultObject resultObject = ConnectionManager.QueryProcessor.ExecuteQuery(query))
                {
                    foreach (IResultObject resultObject1 in resultObject)
                    {
                        List<IResultObject> value = new List<IResultObject>();
                        value = resultObject1.GenericsArray;

                        worker.ReportProgress(num * 100 / value[0].Count, string.Format("Removing content for application: {0}", value[0]["LocalizedDisplayName"].StringValue));

                        bool er = false;

                        try
                        {
                            query = string.Format("SELECT * FROM SMS_ContentPackage WHERE PackageID='{0}'", value[1]["PackageID"].StringValue);
                            using (IResultObject package = Utility.GetFirstWMIInstance(ConnectionManager, query))
                            {
                                package.Delete();
                            }
                        }
                        catch (SmsQueryException ex)
                        {
                            ManagementException managementException = ex.InnerException as ManagementException;
                            error.Add("Could not remove content: " + managementException.ErrorInformation["Description"].ToString(), value[0]["LocalizedDisplayName"].StringValue);
                            er = true;
                        }

                        if (!er)
                        {
                            success.Add(value[0]["LocalizedDisplayName"].StringValue);
                        }

                        num++;
                    }
                }

                PrepareCompletion(success, error);
                AddRefreshResultObject(null, PropertyDataUpdateAction.RefreshAll);
                base.PostApply(worker, e);
            }
            catch (Exception ex)
            {
                AddRefreshResultObject(null, PropertyDataUpdateAction.RefreshAll);
                PrepareError(ex.Message);
                throw;
            }
            finally
            {
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
            foreach (DataGridViewRow dataGridViewRow in dataGridViewApplications.Rows)
            {
                if (Convert.ToBoolean(dataGridViewRow.Cells[columnRemove.Name].Value) == true)
                {
                    if (dataGridViewRow.Tag is IResultObject application)
                    {
                        list.Add(application);
                    }
                }
            }
            UserData["ApplicationItems"] = list;

            AddAction("GeneralDescription", string.Format("The following application(s) content will be removed ({0}):", list.Count));
            AddAction("ApplicationInformation", string.Empty);

            foreach (IResultObject application in (IEnumerable)list)
                AddActionDetailMessage("ApplicationInformation", application["LocalizedDisplayName"].StringValue);
        }

        private void PrepareCompletion(List<string> success, Dictionary<string, string> error)
        {
            RemoveAllSummary();

            if (success.Count > 0)
            {
                AddAction("ApplicationInformation", string.Format("The following application(s) content were removed ({0}):", success.Count));
                UpdateActionStatus("ApplicationInformation", SmsSummaryAction.ActionStatus.CompleteWithSuccess);

                foreach (string message in success)
                    AddActionDetailMessage("ApplicationInformation", message);

                AddAction("EmptyLine", string.Empty);
            }

            if (error.Count > 0)
            {
                AddAction("ApplicationError", string.Format("The following application(s) content cannot be removed ({0}):", error.Count));
                UpdateActionStatus("ApplicationError", SmsSummaryAction.ActionStatus.CompleteWithErrors);
                foreach (KeyValuePair<string, string> item in error)
                {
                    AddActionDetailMessage("ApplicationError", string.Format("{0}: {1}", item.Value, item.Key));
                }
            }
        }

        private void PrepareError(string errorMessage)
        {
            RemoveAllSummary();
            AddAction("ErrorInfo", errorMessage);
            UpdateActionStatus("ErrorInfo", SmsSummaryAction.ActionStatus.CompleteWithErrors);
        }

        private void DataGridViewApplications_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (!dataGridViewApplications.IsCurrentCellDirty)
                return;

            dataGridViewApplications.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }

        private void DataGridViewApplications_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            ControlsInspector.InspectAll();
            Dirty = !ReadOnly;
        }

        private ControlDataState ValidateSelectedApplications()
        {
            foreach (DataGridViewRow dataGridViewRow in dataGridViewApplications.Rows)
            {
                if (Convert.ToBoolean(dataGridViewRow.Cells[0].Value, CultureInfo.InvariantCulture))
                    return ControlDataState.Valid;
            }
            return ControlDataState.Invalid;
        }

        private void ButtonSelect_Click(object sender, EventArgs e)
        {
            dataGridViewApplications.BeginEdit(true);
            bool flag = sender == buttonSelectAll;
            foreach (DataGridViewRow dataGridViewRow in dataGridViewApplications.Rows)
                dataGridViewRow.Cells[0].Value = flag ? true : false;
            dataGridViewApplications.EndEdit();
        }
    }
}
