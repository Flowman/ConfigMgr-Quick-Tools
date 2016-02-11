using Microsoft.ConfigurationManagement.AdminConsole;
using Microsoft.ConfigurationManagement.ManagementProvider;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Collections;

namespace Zetta.ConfigMgr.QuickTools
{
    public partial class SUDeploymentSettingsPage : SmsPageControl
    {
        public SUDeploymentSettingsPage(SmsPageData pageData)
            : base(pageData)
        {
            InitializeComponent();
            Headline = "Specify deployment settings for this deployments";
            Title = "Deployment Settings";
            pageData.ProgressBarStyle = ProgressBarStyle.Continuous;
        }

        public override void InitializePageControl()
        {
            base.InitializePageControl();

        }

        public override void OnActivated()
        {
            base.OnActivated();

            dataGridViewSettings.Rows.Clear();

            foreach (IResultObject collection in (List<IResultObject>)UserData["Collections"])
            {
                SUDeployment deployment = new SUDeployment(ConnectionManager, collection);

                DataGridViewRow dataGridViewRow = dataGridViewSettings_CreateRow(deployment);

                dataGridViewSettings.Rows.Add(dataGridViewRow);
            }

            Initialized = true;
        }

        public override bool OnDeactivate()
        {
            dataGridViewSettings.EndEdit();
            return base.OnDeactivate();
        }

        public override void PostApply(BackgroundWorker worker, DoWorkEventArgs e)
        {
            bool flag = false;
            try
            {
                int num = 0;
                List<SUDeployment> deployments = (List<SUDeployment>)UserData["DeploymentItems"];
                foreach (SUDeployment deployment in deployments)
                {
                    worker.ReportProgress(num * 100 / deployments.Count, string.Format("Creating deployment for collection: {0}", deployment.Collection["Name"].StringValue));

                    deployment.CreateDeployment(SelectedObject);

                    ++num;
                }

                PrepareCompletion();
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
                int num = flag ? 1 : 0;
            }
        }

        public override bool ValidatePage(bool uiAllowed)
        {
            if (!Initialized)
                return false;
            dataGridViewSettings.EndEdit();
            return base.ValidatePage(uiAllowed);
        }

        private void dataGridViewSettings_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            DataGridViewRow editedRow = dataGridViewSettings.Rows[e.RowIndex];

            SUDeployment deployment = (SUDeployment)editedRow.Tag;

            deployment.Enabled = (bool)editedRow.Cells[0].Value;

            if ((string)editedRow.Cells[2].Value == "Required")
            {
                deployment.IsRequired = true;
            } 
            else
            {
                deployment.IsRequired = false;
            }

            if ((string)editedRow.Cells[3].Value == (string)(editedRow.Cells[3] as DataGridViewComboBoxCell).Items[1])
            {
                deployment.UserUIExperience = true;
                deployment.NotifyUser = false;
            }
            else if ((string)editedRow.Cells[3].Value == (string)(editedRow.Cells[3] as DataGridViewComboBoxCell).Items[0])
            {
                deployment.UserUIExperience = false;
                deployment.NotifyUser = false;
            }
            else
            {
                deployment.UserUIExperience = true;
                deployment.NotifyUser = true;
            }

            deployment.OverrideServiceWindows = (bool)editedRow.Cells[4].Value;
            deployment.RebootOutsideOfServiceWindows = (bool)editedRow.Cells[5].Value;
            deployment.SuppressReboot = (bool)editedRow.Cells[6].Value ? 3 : 0;
            deployment.StartTime = DateTime.Parse((string)editedRow.Cells[7].Value);

            if ((string)editedRow.Cells[8].Value != "")
                deployment.DeadLineTime = DateTime.Parse((string)editedRow.Cells[8].Value);

            dataGridViewSettings_UpdateRow(editedRow, deployment);
        }

        private DataGridViewRow dataGridViewSettings_CreateRow(SUDeployment deployment)
        {
            DataGridViewRow dataGridViewRow = new DataGridViewRow();
            dataGridViewRow.CreateCells(dataGridViewSettings);

            dataGridViewSettings_UpdateRow(dataGridViewRow, deployment);

            return dataGridViewRow;
        }

        private DataGridViewRow dataGridViewSettings_UpdateRow(DataGridViewRow dataGridViewRow, SUDeployment deployment)
        {
            if (deployment.IsPhased)
                dataGridViewRow.ReadOnly = true;

            dataGridViewRow.Cells[0].ReadOnly = false;
            dataGridViewRow.Cells[0].Value = deployment.Enabled;
            dataGridViewRow.Cells[1].Value = deployment.Collection["Name"].StringValue;
            dataGridViewRow.Cells[2].Value = deployment.IsRequired ? "Required" : "Available";

            if (deployment.UserUIExperience && !deployment.NotifyUser)
                dataGridViewRow.Cells[3].Value = (dataGridViewRow.Cells[3] as DataGridViewComboBoxCell).Items[1];
            else if (!deployment.UserUIExperience && !deployment.NotifyUser)
                dataGridViewRow.Cells[3].Value = (dataGridViewRow.Cells[3] as DataGridViewComboBoxCell).Items[0];
            else
                dataGridViewRow.Cells[3].Value = (dataGridViewRow.Cells[3] as DataGridViewComboBoxCell).Items[2];

            for (int i = 4; i <= 6; i++)
            {
                if (!deployment.IsRequired)
                {
                    dataGridViewRow.Cells[i].Value = false;
                    dataGridViewRow.Cells[i].ReadOnly = true;
                    dataGridViewRow.Cells[i].Style.BackColor = Color.LightGray;
                    dataGridViewRow.Cells[i].Style.ForeColor = Color.DarkGray;
                }
                else
                {
                    dataGridViewRow.Cells[i].ReadOnly = false;
                    dataGridViewRow.Cells[i].Style.BackColor = Color.White;
                    dataGridViewRow.Cells[i].Style.ForeColor = Color.Black;
                }
            }

            dataGridViewRow.Cells[4].Value = deployment.OverrideServiceWindows;
            dataGridViewRow.Cells[5].Value = deployment.RebootOutsideOfServiceWindows;
            dataGridViewRow.Cells[6].Value = deployment.SuppressReboot > 0 ? true : false;
            dataGridViewRow.Cells[7].Value = deployment.StartTime.ToString("dd'/'MM'/'yyyy HH:mm");

            if (!deployment.IsRequired)
            {
                dataGridViewRow.Cells[8].Value = "";
                dataGridViewRow.Cells[8].ReadOnly = true;
                dataGridViewRow.Cells[8].Style.BackColor = Color.LightGray;
                dataGridViewRow.Cells[8].Style.ForeColor = Color.DarkGray;
            }
            else
            {
                dataGridViewRow.Cells[8].ReadOnly = false;
                dataGridViewRow.Cells[8].Style.BackColor = Color.White;
                dataGridViewRow.Cells[8].Style.ForeColor = Color.Black;
                dataGridViewRow.Cells[8].Value = deployment.DeadLineTime.ToString("dd'/'MM'/'yyyy HH:mm");
            }
                
            dataGridViewRow.Tag = deployment;

            return dataGridViewRow;
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

            List<SUDeployment> list = new List<SUDeployment>();
            foreach (DataGridViewRow dataGridViewRow in dataGridViewSettings.Rows)
            {
                SUDeployment deployment = dataGridViewRow.Tag as SUDeployment;
                list.Add(deployment);
            }
            UserData["DeploymentItems"] = list;

            AddAction("GeneralDescription", string.Format("The following deployments will be created ({0}):", list.Count));
            AddAction("EmptyLine", string.Empty);
            
            foreach (SUDeployment deployment in (IEnumerable)list)
            {
                AddAction(deployment.Collection["CollectionID"].StringValue, string.Format("Collection: {0}", deployment.Collection["Name"].StringValue));

                AddActionDetailMessage(deployment.Collection["CollectionID"].StringValue, string.Format("Deployment Name: {0}", deployment.Name));

                AddActionDetailMessage(deployment.Collection["CollectionID"].StringValue, string.Format("Available to target computers: {0}", deployment.StartTime.ToString("dd'/'MM'/'yyyy HH:mm")));
                
                if (deployment.IsRequired)
                    AddActionDetailMessage(deployment.Collection["CollectionID"].StringValue, string.Format("Deadline for software update installation: {0}", deployment.DeadLineTime.ToString("dd'/'MM'/'yyyy HH:mm")));

                if (deployment.RebootOutsideOfServiceWindows)
                    AddActionDetailMessage(deployment.Collection["CollectionID"].StringValue, "Restart system outside the maintenance window when deadline is reached: Yes");

                if (deployment.OverrideServiceWindows)
                    AddActionDetailMessage(deployment.Collection["CollectionID"].StringValue, "Install software updates outside the maintenance window when deadline is reached: Yes ");

                if (deployment.UserUIExperience && !deployment.NotifyUser)
                    AddActionDetailMessage(deployment.Collection["CollectionID"].StringValue, "User Notifications: Display in Software Center, and only show notifications for computer restarts");
                else if (!deployment.UserUIExperience && !deployment.NotifyUser)
                    AddActionDetailMessage(deployment.Collection["CollectionID"].StringValue, "User Notifications: Hide in Softwaer Center and all notifcations");
                else
                    AddActionDetailMessage(deployment.Collection["CollectionID"].StringValue, "User Notifications: Display in Software Center and show all notifications");

                AddAction("EmptyLine", string.Empty);
            }

        }

        private void PrepareCompletion()
        {
            RemoveAllSummary();

            bool hasError = false;
            List<string> deploymentSuccess = new List<string>();
            List<SUDeployment> deploymentError = new List<SUDeployment>();
            foreach (SUDeployment deployment in (List<SUDeployment>)UserData["DeploymentItems"])
            {
                if (deployment.HasError)
                {
                    deploymentError.Add(deployment);
                    hasError = true;
                }
                else
                    deploymentSuccess.Add(string.Format("{0} : {1}", deployment.Collection["Name"].StringValue, deployment.Name));
            }

            AddAction("GeneralDescription", "All deployment(s) where created successfully.");
            if (hasError)
            {
                UpdateAction("GeneralDescription", "Some deployment(s) could not be created successfully. See following details.");
                UpdateActionStatus("GeneralDescription", SmsSummaryAction.ActionStatus.CompleteWithErrors);
            }
            AddAction("EmptyLine", string.Empty);

            if (deploymentSuccess.Count > 0)
            {
                AddAction("DeploymentInformation", string.Format("The following deployments(s) were created ({0}):", deploymentSuccess.Count));
                UpdateActionStatus("DeploymentInformation", SmsSummaryAction.ActionStatus.CompleteWithSuccess);

                foreach (string message in deploymentSuccess)
                    AddActionDetailMessage("DeploymentInformation", message);

                AddAction("EmptyLine", string.Empty);
            }

            if (hasError)
            {
                foreach (SUDeployment deployment in deploymentError)
                {
                    AddAction(deployment.Collection["CollectionID"].StringValue, string.Format("{0} : {1}", deployment.Collection["Name"].StringValue, deployment.Name));
                    UpdateActionStatus(deployment.Collection["CollectionID"].StringValue, SmsSummaryAction.ActionStatus.CompleteWithErrors);

                    string str = string.Format("• {0}", deployment.Error.Message);
                    AddActionErrorMessage(deployment.Collection["CollectionID"].StringValue, str);

                    AddAction("EmptyLine", string.Empty);
                }
            }
        }

        private void PrepareError(string errorMessage)
        {
            RemoveAllSummary();
            AddAction("ErrorInfo", errorMessage);
            UpdateActionStatus("ErrorInfo", SmsSummaryAction.ActionStatus.CompleteWithErrors);
        }

        private void dataGridViewSettings_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            Utility.UpdateDataGridViewColumnsSize(dataGridViewSettings, columnCollectionName);
        }
    }
}
