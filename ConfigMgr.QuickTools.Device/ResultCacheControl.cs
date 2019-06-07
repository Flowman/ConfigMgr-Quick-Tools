using Microsoft.ConfigurationManagement.AdminConsole;
using Microsoft.ConfigurationManagement.AdminConsole.DialogFramework;
using ByteSizeLib;
using System;
using System.Management;
using System.Windows.Forms;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.IO;
using System.Security.AccessControl;

namespace ConfigMgr.QuickTools.Device
{
    public partial class ResultCacheControl : SmsPageControl
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private BackgroundWorker backgroundWorker;
        private ManagementObject cacheConfig;

        public ResultCacheControl(SmsPageData pageData)
          : base(pageData)
        {
            InitializeComponent();

            Title = "Client Cache";

            Updater.CheckUpdates();
        }

        public override void InitializePageControl()
        {
            base.InitializePageControl();

            backgroundWorker = new BackgroundWorker();
            backgroundWorker.DoWork += new DoWorkEventHandler(InfoWorker_DoWork);
            backgroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(InfoWorker_RunWorkerCompleted);
            backgroundWorker.WorkerSupportsCancellation = false;
            backgroundWorker.WorkerReportsProgress = false;
            UseWaitCursor = true;
            listViewContent.IsLoading = true;
            backgroundWorker.RunWorkerAsync();

            Dirty = false;
        }

        private void InfoWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                ManagementScope scope = Utility.GetWMIScope(PropertyManager["Name"].StringValue, @"ccm\softmgmtagent");
                // get cache config instace so we can change it on apply
                cacheConfig = Utility.GetFirstWMIInstance(scope, "SELECT * FROM CacheConfig WHERE ConfigKey='Cache'");
                List<ManagementObject> cacheContent = Utility.SearchWMIToList(scope, "SELECT * FROM CacheInfoEx");

                string location = cacheConfig["Location"].ToString();

                ManagementObject disk = Utility.GetFirstWMIInstance(PropertyManager["Name"].StringValue, @"cimv2", string.Format("Win32_LogicalDisk WHERE DeviceID='{0}:'", location[0]));

                ByteSize cacheSize = ByteSize.FromMegaBytes(double.Parse(cacheConfig["Size"].ToString()));
                ByteSize freeSpace = ByteSize.FromBytes(double.Parse(disk["FreeSpace"].ToString()));

                trackBarWithoutFocus1.Maximum = Convert.ToInt32(freeSpace.MegaBytes);
                trackBarWithoutFocus1.TickFrequency = Convert.ToInt32(trackBarWithoutFocus1.Maximum / 10);
                trackBarWithoutFocus1.LargeChange = trackBarWithoutFocus1.TickFrequency;
                numericUpDown1.Maximum = trackBarWithoutFocus1.Maximum;

                trackBarWithoutFocus1.Value = Convert.ToInt32(cacheSize.MegaBytes);

                labelLocation.Text = location;
                labelCacheSize.Text = cacheSize.ToString();
                labelSpaceToUse.Text = cacheSize.ToString();

                trackBarWithoutFocus1.Enabled = true;
                numericUpDown1.Enabled = true;

                UpdateContentListView(cacheContent);
            }
            catch (Exception ex)
            {
                string msg = string.Format("{0}: {1}", ex.GetType().Name, ex.Message);
                log.Error(msg);
                throw new InvalidOperationException(msg);
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
                    listViewContent.IsLoading = false;
                    listViewContent.UpdateColumnWidth(columnHeaderLocation);
                    listViewContent.Sorting = SortOrder.Ascending;
                    Initialized = true;
                }
            }
        }

        protected override bool ApplyChanges(out Control errorControl, out bool showError)
        {
            try
            {
                PutOptions options = new PutOptions
                {
                    Type = PutType.UpdateOnly
                };

                cacheConfig["Size"] = numericUpDown1.Value;
                cacheConfig.Put(options);
            }
            catch (Exception ex)
            {
                string msg = string.Format("{0}: {1}", ex.GetType().Name, ex.Message);
                log.Error(msg);
                throw new InvalidOperationException(msg);
            }

            // need to be false or sccm throws an put error on the orignal property window
            Dirty = false;

            return base.ApplyChanges(out errorControl, out showError);
        }

        private void UpdateContentListView(List<ManagementObject> items)
        {
            listViewContent.Items.Clear();

            foreach (ManagementObject item in items)
            {
                ByteSize size = ByteSize.FromKiloBytes(double.Parse(item["ContentSize"].ToString()));
                DateTime date = ManagementDateTimeConverter.ToDateTime(item["LastReferenced"].ToString());

                listViewContent.Items.Add(new ListViewItem()
                {
                    Text = item["Location"].ToString(),
                    SubItems = {
                            size.ToString(),
                            date.ToShortDateString(),
                            item["CacheId"].ToString()
                        },
                    Tag = item
                });
            }

            double contentSize = items.Sum(item => double.Parse(item["ContentSize"].ToString()));
            ByteSize usedSize = ByteSize.FromKiloBytes(contentSize);
            labelUsedSize.Text = usedSize.ToString();
        }

        private void RemoveCachedItems(ListView.SelectedListViewItemCollection items)
        {
            foreach (ListViewItem item in items)
            {
                try
                {
                    // get the wmi object
                    ManagementObject managementObject = (ManagementObject)item.Tag;
                    // create location string
                    string str = (managementObject["Location"] as string).Replace(':', '$');
                    string path = string.Format(@"\\{0}\{1}", PropertyManager["Name"].StringValue, str);
                    // check if cached content exists and we have persmissions to remove it
                    if (Directory.Exists(path) && Utility.CheckFolderPermissions(path, FileSystemRights.Delete))
                    {
                        // delete folder
                        Directory.Delete(path, true);
                        // remove wmi object
                        managementObject.Delete();
                    }
                    else
                    {
                        System.Windows.MessageBox.Show(string.Format("Cannot remove item: Access denied", items.Count), "Configuration Manager", MessageBoxButton.OK, MessageBoxImage.Hand);
                    }
                }
                catch (Exception ex)
                {
                    log.Error(string.Format("{0}: {1}", ex.GetType().Name, ex.Message));
                    SccmExceptionDialog.ShowDialog(SnapIn.Console, ex);
                }
            }
        }

        private void SetDirty()
        {
            if (!Initialized)
                return;
            Dirty = true;
        }

        private void ContextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            if (listViewContent.SelectedItems.Count == 0)
                e.Cancel = true;
        }

        private void ToolStripMenuItemDelete_Click(object sender, EventArgs e)
        {
            ListView.SelectedListViewItemCollection items = listViewContent.SelectedItems;

            MessageBoxResult result = System.Windows.MessageBox.Show(string.Format("Do you really want to remove {0} items from the cache?", items.Count), "Configuration Manager", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);

            if (result == MessageBoxResult.Yes)
            {
                RemoveCachedItems(items);
                // update the list view
                ManagementScope scope = Utility.GetWMIScope(PropertyManager["Name"].StringValue, @"ccm\softmgmtagent");
                List<ManagementObject> cacheContent = Utility.SearchWMIToList(scope, "SELECT * FROM CacheInfoEx");
                UpdateContentListView(cacheContent);
            }
        }

        private void TrackBar1_ValueChanged(object sender, EventArgs e)
        {
            numericUpDown1.Value = trackBarWithoutFocus1.Value;
            var b = ByteSize.FromMegaBytes(trackBarWithoutFocus1.Value);
            labelSpaceToUse.Text = b.ToString();
            SetDirty();
        }

        private void NumericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            trackBarWithoutFocus1.Value = Convert.ToInt32(numericUpDown1.Value);
            var b = ByteSize.FromMegaBytes(trackBarWithoutFocus1.Value);
            labelSpaceToUse.Text = b.ToString();
            SetDirty();
        }

        private void ButtonClearCache_Click(object sender, EventArgs e)
        {
            listViewContent.Items.OfType<ListViewItem>().ToList().ForEach(item => item.Checked = true);
            ListView.SelectedListViewItemCollection items = listViewContent.SelectedItems;

            MessageBoxResult result = System.Windows.MessageBox.Show("Are you sure you want to clear the cache?", "Configuration Manager", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);

            if (result == MessageBoxResult.Yes)
            {
                RemoveCachedItems(items);
                // update the list view
                ManagementScope scope = Utility.GetWMIScope(PropertyManager["Name"].StringValue, @"ccm\softmgmtagent");
                List<ManagementObject> cacheContent = Utility.SearchWMIToList(scope, "SELECT * FROM CacheInfoEx");
                UpdateContentListView(cacheContent);
            }
        }
    }
}
