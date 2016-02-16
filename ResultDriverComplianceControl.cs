using Microsoft.ConfigurationManagement.AdminConsole;
using Microsoft.ConfigurationManagement.AdminConsole.DialogFramework;
using Microsoft.ConfigurationManagement.AdminConsole.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Management;
using Microsoft.ConfigurationManagement.ManagementProvider;
using System.Linq;
using System.Net;
using System.IO;
using System.Text;

namespace Zetta.ConfigMgr.QuickTools
{
    public partial class ResultDriverComplianceControl : SmsPageControl
    {
        private BackgroundWorker backgroundWorker;
        private string packageID;

        public ResultDriverComplianceControl(SmsPageData pageData)
          : base(pageData)
        {
            InitializeComponent();
            buttonRefresh.Image = new Icon(Properties.Resources.update, new Size(16, 16)).ToBitmap();
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

        private void buttonRefresh_Click(object sender, EventArgs e)
        {
            listViewListDrivers.IsLoading = true;
            listViewListDrivers.UpdateColumnWidth(columnHeaderDescription);
            listViewListDrivers.Items.Clear();

            backgroundWorker = new BackgroundWorker();
            backgroundWorker.DoWork += new DoWorkEventHandler(infoWorker_DoWork);
            backgroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(infoWorker_RunWorkerCompleted);
            backgroundWorker.WorkerSupportsCancellation = false;
            backgroundWorker.WorkerReportsProgress = false;
            buttonRefresh.Enabled = false;
            backgroundWorker.RunWorkerAsync();
        }

        private void infoWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            CredentialCache netCache = new CredentialCache();
            try
            {
                netCache.Add(new Uri(string.Format(@"\\{0}", PropertyManager["Name"].StringValue)), "Digest", CredentialCache.DefaultNetworkCredentials);
            }
            catch (IOException ex)
            {
                throw new InvalidOperationException(string.Format("{0}: {1}", ex.GetType().Name, ex.Message));
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
                    string provider = (string)item["DriverProviderName"];
                    string description = (string)item["DeviceName"];
                    string oeminf = (string)item["InfName"];

                    if (provider.Length > 0 && description.Length > 0)
                    {
                        if (provider == "Microsoft" || DriverExists(description, oeminf))
                            continue;

                        if (!string.IsNullOrEmpty(textBoxDriverPackage.Text))
                        {
                            string remoteINFFile = string.Format(@"\\{0}\admin$\inf\{1}", PropertyManager["Name"].StringValue, oeminf);
                            Driver iniDriver = new Driver(remoteINFFile);

                            IResultObject driver = (driverObjects != null) ? driverObjects.Where(x => x.Properties["LocalizedDisplayName"].StringValue.Equals(iniDriver.Model)).FirstOrDefault() : null;
                            if (driver == null)
                                continue;

                            string driverVersion = (driver != null) ? driver["DriverVersion"].StringValue : null;
                            Color textColor = (driverVersion == null) ? Color.Black : (driverVersion == (string)item["DriverVersion"]) ? Color.Green : Color.Red;

                            listViewListDrivers.Items.Add(new ListViewItem()
                            {
                                Text = provider,
                                SubItems = {
                                    description,
                                    (string)item["DriverVersion"],
                                    driverVersion
                                },
                                Tag = oeminf,
                                ForeColor = textColor
                            });
                        }
                        else
                        {
                            listViewListDrivers.Items.Add(new ListViewItem()
                            {
                                Text = provider,
                                SubItems = {
                                    description,
                                    (string)item["DriverVersion"],
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
                    listViewListDrivers.IsLoading = false;
                    listViewListDrivers.UpdateColumnWidth(columnHeaderDescription);
                    buttonRefresh.Enabled = true;
                }
            }
        }

        private bool DriverExists(string desc, string oeminf)
        {
            foreach (ListViewItem item in listViewListDrivers.Items)
            {
                if (item.SubItems[0].Text == desc || (string)item.Tag == oeminf)
                    return true;
            }
            return false;
        }

        private void buttonBrowse_Click(object sender, EventArgs e)
        {
            BrowsePackageExtend browsePackageExtend = null;
            try
            {
                browsePackageExtend = new BrowsePackageExtend(ConnectionManager);
                browsePackageExtend.QueryClass = "SMS_DriverPackage";
                browsePackageExtend.Text = "Select Driver Package";
                if (browsePackageExtend.ShowDialog(this) != DialogResult.OK)
                    return;
                packageID = browsePackageExtend.BrowsePackageObject["PackageID"].StringValue;
                textBoxDriverPackage.Text = BrowsePackageExtend.GetPackageDisplayName(browsePackageExtend.BrowsePackageObject, string.Empty);
            }
            finally
            {
                if (browsePackageExtend != null)
                    browsePackageExtend.Dispose();
            }
        }

        private void listViewListDrivers_CopyKeyEvent(object sender, EventArgs e)
        {
            StringBuilder buffer = new StringBuilder();
            foreach (ListViewItem item in listViewListDrivers.SelectedItems)
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
