using Microsoft.ConfigurationManagement.AdminConsole;
using Microsoft.ConfigurationManagement.AdminConsole.DialogFramework;
using Microsoft.ConfigurationManagement.ManagementProvider;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Management;

namespace ConfigMgr.QuickTools.Device.PropertiesDialog
{
    public partial class ResultSUComplianceControl : SmsPageControl
    {
        private BackgroundWorker backgroundWorker;

        public ResultSUComplianceControl(SmsPageData pageData)
          : base(pageData)
        {
            InitializeComponent();

            buttonSURefresh.Image = new Icon(Properties.Resources.reload, new Size(16, 16)).ToBitmap();

            Title = "Software Updates Compliance";
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

        private void ButtonSURefresh_Click(object sender, EventArgs e)
        {
            listViewSoftwareUpdates.IsLoading = true;
            listViewSoftwareUpdates.UpdateColumnWidth(columnHeaderAssignment);
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
                string host = PropertyManager["Name"].StringValue;

                ManagementScope scope = Utility.GetWMIScope(host, @"ccm\SoftwareUpdates\DeploymentAgent");
                ObjectQuery query = new ObjectQuery("SELECT * FROM CCM_AssignmentCompliance");
                List<ManagementObject> compliance = Utility.SearchWMIToList(scope, query);
                // it is faster to run two separate queries than one. SELECT SMS_AuthorizationList.LocalizedDisplayName FROM SMS_UpdateGroupAssignment JOIN SMS_AuthorizationList ON SMS_AuthorizationList.CI_ID = SMS_UpdateGroupAssignment.AssignedUpdateGroup WHERE SMS_UpdateGroupAssignment.AssignmentUniqueID =          
                List<IResultObject> assignments = Utility.SearchWMIToList(ConnectionManager, "SELECT * FROM SMS_UpdateGroupAssignment");
                List<IResultObject> updateGroups = Utility.SearchWMIToList(ConnectionManager, "SELECT * FROM SMS_AuthorizationList");

                foreach (ManagementObject item in compliance)
                {
                    var assignment = assignments.Where(x => x.Properties["AssignmentUniqueID"].StringValue.Equals(item.Properties["AssignmentId"].Value)).FirstOrDefault();
                    if (assignment != null)
                    {
                        var updateGroup = updateGroups.Where(x => x.Properties["CI_ID"].IntegerValue.Equals(assignment.Properties["AssignedUpdateGroup"].IntegerValue)).FirstOrDefault();
                        if (updateGroup != null)
                        {
                            listViewSoftwareUpdates.Items.Add(new ListViewItem()
                            {
                                Text = updateGroup["LocalizedDisplayName"].StringValue,
                                SubItems = {
                                    ((bool)item.Properties["IsCompliant"].Value ? "Compliant" : "Non-Compliant")
                                }
                            });
                        }
                    }
                }

                ManagementObject lastscan = Utility.GetFirstWMIInstance(host, @"ccm\scanagent", "CCM_ScanUpdateSourceHistory");

                DateTime time = ManagementDateTimeConverter.ToDateTime(lastscan["LastCompletionTime"].ToString());
                labelLastScan.Text = time.ToString("dd'/'MM'/'yyyy HH:mm");

                ManagementObject scansource = Utility.GetFirstWMIInstance(host, @"ccm\SoftwareUpdates\WUAhandler", "CCM_UpdateSource");

                labelCABSource.Text = scansource["ContentLocation"].ToString();
                labelCABVersion.Text = scansource["ContentVersion"].ToString();
            }
            catch (ManagementException ex)
            {
                MessageBox.Show("An error occured while querying for WMI data: " + ex.Message);
            }
            catch (UnauthorizedAccessException unauthorizedErr)
            {
                MessageBox.Show("Connection error " + "(user name or password might be incorrect): " + unauthorizedErr.Message);
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
                    listViewSoftwareUpdates.UpdateColumnWidth(columnHeaderAssignment);
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
