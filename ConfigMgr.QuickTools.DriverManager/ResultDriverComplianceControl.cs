using Microsoft.ConfigurationManagement.AdminConsole;
using Microsoft.ConfigurationManagement.AdminConsole.DialogFramework;
using Microsoft.ConfigurationManagement.AdminConsole.Common;
using Microsoft.ConfigurationManagement.ManagementProvider;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Management;
using System.Linq;
using System.Security.AccessControl;

namespace ConfigMgr.QuickTools.DriverManager.PropertiesDialog
{

    public partial class ResultDriverComplianceControl : SmsPageControl
    {
        private BackgroundWorker backgroundWorker;

        public ResultDriverComplianceControl(SmsPageData pageData)
          : base(pageData)
        {
            InitializeComponent();

            buttonRefresh.Image = new Icon(Properties.Resources.reload, new Size(16, 16)).ToBitmap();

            Title = "Driver Compliance";
        }

        public override void InitializePageControl()
        {
            base.InitializePageControl();

            if (!PropertyManager["IsClient"].BooleanValue)
            {
                buttonRefresh.Enabled = false;
            }

            Initialized = true;
        }

        private void ButtonRefresh_Click(object sender, EventArgs e)
        {
            listViewDrivers.IsLoading = true;
            listViewDrivers.UpdateColumnWidth(columnHeaderDescription);
            listViewDrivers.Items.Clear();

            backgroundWorker = new BackgroundWorker();
            backgroundWorker.DoWork += new DoWorkEventHandler(InfoWorker_DoWork);
            backgroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(InfoWorker_RunWorkerCompleted);
            backgroundWorker.WorkerSupportsCancellation = false;
            backgroundWorker.WorkerReportsProgress = false;
            buttonRefresh.Enabled = false;
            backgroundWorker.RunWorkerAsync();
        }

        private void InfoWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            if (!Utility.CheckFolderPermissions(string.Format(@"\\{0}\admin$", PropertyManager["Name"].StringValue), FileSystemRights.ReadData))
            {
                throw new InvalidOperationException("Access Denied");
            }

            ManagementScope scope = Utility.GetWMIScope(PropertyManager["Name"].StringValue, @"cimv2");
            ObjectQuery query = new ObjectQuery("SELECT * FROM Win32_PnPSignedDriver WHERE DriverProviderName IS NOT NULL");
            List<ManagementObject> signedDriver = Utility.SearchWMIToList(scope, query);

            List<IResultObject> driverObjects = new List<IResultObject>();

            if (!string.IsNullOrEmpty(textBoxDriverPackage.Text))
            {
                string query2 = string.Format("SELECT * FROM SMS_DriverPackage WHERE NAME = '{0}'", textBoxDriverPackage.Text);
                IResultObject packageObject = Utility.GetFirstWMIInstance(ConnectionManager, query2);
                driverObjects = Utility.SearchWMIToList(ConnectionManager, string.Format("SELECT * FROM SMS_Driver WHERE CI_ID IN (SELECT DC.CI_ID FROM SMS_DriverContainer AS DC WHERE DC.PackageID = '{0}') ORDER BY LocalizedDisplayName", packageObject["PackageID"].StringValue));
            }

            foreach (ManagementObject item in signedDriver)
            {
                try
                {
                    string provider = item["DriverProviderName"].ToString();
                    string description = item["DeviceName"].ToString();
                    string oeminf = item["InfName"].ToString();

                    if (provider.Length > 0 && description.Length > 0)
                    {
                        if (provider == "Microsoft" || DriverExists(description, oeminf))
                            continue;

                        if (!string.IsNullOrEmpty(textBoxDriverPackage.Text))
                        {
                            string remoteINFFile = string.Format(@"\\{0}\admin$\inf\{1}", PropertyManager["Name"].StringValue, oeminf);
                            Driver iniDriver = new Driver(remoteINFFile);

                            IResultObject driver = driverObjects?.Where(x => x.Properties["LocalizedDisplayName"].StringValue.Equals(iniDriver.Model)).FirstOrDefault();
      
                            string driverVersion = driver?["DriverVersion"].StringValue;
                            Color textColor = driverVersion == null ? Color.Black : driverVersion == item["DriverVersion"].ToString() ? Color.Green : Color.Red;

                            listViewDrivers.Items.Add(new ListViewItem()
                            {
                                Text = provider,
                                SubItems = {
                                    description,
                                    item["DriverVersion"].ToString(),
                                    driverVersion
                                },
                                Tag = oeminf,
                                ForeColor = textColor
                            });
                        }
                        else
                        {
                            listViewDrivers.Items.Add(new ListViewItem()
                            {
                                Text = provider,
                                SubItems = {
                                    description,
                                    item["DriverVersion"].ToString(),
                                 },
                                Tag = oeminf
                            });
                        }
                    }
                }
                catch { }
            }

            query = new ObjectQuery("SELECT * FROM Win32_ComputerSystem");
            ManagementObject computerSystem = Utility.GetFirstWMIInstance(scope, query);

            labelManufacturer.Text = (string)computerSystem["Manufacturer"];
            labelProductName.Text = (string)computerSystem["Model"];
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
                    listViewDrivers.IsLoading = false;
                    listViewDrivers.UpdateColumnWidth(columnHeaderDescription);
                    buttonRefresh.Enabled = true;
                }
            }
        }

        private bool DriverExists(string desc, string oeminf)
        {
            foreach (ListViewItem item in listViewDrivers.Items)
            {
                if (item.SubItems[0].Text == desc || (string)item.Tag == oeminf)
                    return true;
            }

            return false;
        }

        private void ButtonBrowse_Click(object sender, EventArgs e)
        {
            BrowsePackageExtend browsePackageExtend = null;
            try
            {
                browsePackageExtend = new BrowsePackageExtend(ConnectionManager)
                {
                    QueryClass = "SMS_DriverPackage",
                    Text = "Select Driver Package"
                };

                if (browsePackageExtend.ShowDialog(this) != DialogResult.OK)
                    return;

                textBoxDriverPackage.Text = BrowsePackageExtend.GetPackageDisplayName(browsePackageExtend.BrowsePackageObject, string.Empty);
            }
            finally
            {
                if (browsePackageExtend != null)
                    browsePackageExtend.Dispose();
            }
        }

        private void ListView_CopyKeyEvent(object sender, EventArgs e)
        {
            Utility.CopyToClipboard((ListView)sender);
        }
    }
}
    
