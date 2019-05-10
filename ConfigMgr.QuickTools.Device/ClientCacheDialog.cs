using Microsoft.ConfigurationManagement.AdminConsole;
using Microsoft.ConfigurationManagement.ManagementProvider;
using Microsoft.ConfigurationManagement.AdminConsole.Schema;
using Microsoft.ConfigurationManagement.AdminConsole.DialogFramework;
using System.Windows.Forms;
using System;
using System.Management;
using ByteSizeLib;

namespace ConfigMgr.QuickTools.Device
{
    public partial class ClientCacheDialog : SmsCustomDialog
    {
        private IResultObject resultObjects;

        public ClientCacheDialog(IResultObject selectedResultObjects, ActionDescription action)
        {
            InitializeComponent();
            resultObjects = selectedResultObjects;
            Title = action.DisplayName + ": " + resultObjects["Name"].StringValue;

            Updater.CheckUpdates();
        }

        public static void Show(object sender, ScopeNode scopeNode, ActionDescription action, IResultObject selectedObject, PropertyDataUpdated dataUpdatedDelegate, Status Status)
        {
            using (ClientCacheDialog dialog = new ClientCacheDialog(selectedObject, action))
            {
                dialog.ShowDialog();
                return;
            }
        }

        private void ClientCacheDialog_Shown(object sender, EventArgs e)
        {
            try
            {
                ManagementObject cacheConfig = Utility.GetFirstWMIInstance(resultObjects["Name"].StringValue, @"ccm\softmgmtagent", "CacheConfig WHERE ConfigKey='Cache'");

                string location = cacheConfig["Location"].ToString();
                char c = location[0];
                
                ManagementObject disk = Utility.GetFirstWMIInstance(resultObjects["Name"].StringValue, @"cimv2", string.Format("Win32_LogicalDisk WHERE DeviceID='{0}:'", c));

                var cacheSize = ByteSize.FromMegaBytes(double.Parse(cacheConfig["Size"].ToString()));
                var freeSpace = ByteSize.FromBytes(double.Parse(disk["FreeSpace"].ToString()));

                trackBarWithoutFocus1.Maximum = Convert.ToInt32(freeSpace.MegaBytes);
                trackBarWithoutFocus1.TickFrequency = Convert.ToInt32(trackBarWithoutFocus1.Maximum / 10);
                trackBarWithoutFocus1.LargeChange = trackBarWithoutFocus1.TickFrequency;
                numericUpDown1.Maximum = trackBarWithoutFocus1.Maximum;

                trackBarWithoutFocus1.Value = Convert.ToInt32(cacheSize.MegaBytes);

                labelLocation.Text = location;
                labelCacheSize.Text = cacheSize.ToString();
                labelSpaceToUse.Text = cacheSize.ToString();





            }
            catch (Exception ex)
            {
                ExceptionUtilities.TraceException(ex);
                SccmExceptionDialog.ShowDialog(ex);
            }
        }




        private void ButtonOK_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void TrackBar1_ValueChanged(object sender, EventArgs e)
        {
            numericUpDown1.Value = trackBarWithoutFocus1.Value;
            var b = ByteSize.FromMegaBytes(trackBarWithoutFocus1.Value);
            labelSpaceToUse.Text = b.ToString();

        }

        private void ClientCacheDialog_Load(object sender, EventArgs e)
        {

        }
    }
}
