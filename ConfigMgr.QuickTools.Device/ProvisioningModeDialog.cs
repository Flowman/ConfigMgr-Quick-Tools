using Microsoft.ConfigurationManagement.AdminConsole;
using Microsoft.ConfigurationManagement.ManagementProvider;
using Microsoft.ConfigurationManagement.AdminConsole.Schema;
using System;
using System.Windows.Forms;
using System.Threading;
using System.Management;
using System.Runtime.InteropServices;

namespace ConfigMgr.QuickTools.Device
{
    public partial class ProvisioningModeDialog : SmsCustomDialog
    {
        private IResultObject resultObjects;
        private int completed;
        private int other;
        private int total;
        private bool mode;
   
        public delegate void BarDelegate();

        public ProvisioningModeDialog(IResultObject selectedResultObjects, ActionDescription action, bool selectedMode)
        {
            InitializeComponent();

            resultObjects = selectedResultObjects;
            mode = selectedMode;
            Title = action.DisplayName;
        }

        private void ClientActionsDialog_Shown(object sender, EventArgs e)
        {
            listViewHosts.IsLoading = true;
            listViewHosts.Items.Clear();

            foreach (IResultObject resultObject in resultObjects)
            {
                if (!resultObject["IsClient"].BooleanValue)
                    continue;

                ++total;
                listViewHosts.Items.Add(new ListViewItem()
                {
                    Text = resultObject["Name"].StringValue,
                    SubItems = { "Queued" }
                });
                ThreadPool.QueueUserWorkItem(arg => { ClientAction(resultObject); });
            }

            labelTotal.Text = total.ToString();
            progressBar1.Maximum = total;
            progressBar1.Minimum = 0;

            listViewHosts.IsLoading = false;
        }

        private void ClientAction(IResultObject resultObject)
        {
            ListViewItem item = listViewHosts.FindItemWithText(resultObject["Name"].StringValue);

            try
            {
                item.SubItems[1].Text = "Connecting";
                ObjectGetOptions o = new ObjectGetOptions
                {
                    Timeout = new TimeSpan(0, 0, 5)
                };
                if (mode)
                {
                    using (ManagementClass clientaction = new ManagementClass(string.Format(@"\\{0}\root\{1}", resultObject["Name"].StringValue, "ccm"), "SMS_Client", o))
                    {
                        object[] methodArgs = { "False" };
                        clientaction.InvokeMethod("SetClientProvisioningMode", methodArgs);
                    }
                    item.SubItems[1].Text = "Completed";
                }
                else
                {
                    //using (ManagementClass registry = new ManagementClass(string.Format(@"\\{0}\root\{1}", resultObject["Name"].StringValue, "cimv2"), "StdRegProv", o))
                    //{
                    //    ManagementBaseObject inParams = registry.GetMethodParameters("GetStringValue");
                    //    inParams["hDefKey"] = 0x80000002;
                    //    inParams["sSubKeyName"] = @"SOFTWARE\Microsoft\CCM\CcmExec";
                    //    inParams["sValueName"] = "ProvisioningMode";

                    //    ManagementBaseObject outParams = registry.InvokeMethod("GetStringValue", inParams, null);

                    //    if (outParams.Properties["sValue"].Value != null)
                    //    {
                    //        item.SubItems[1].Text = outParams.Properties["sValue"].Value.ToString();
                    //    }
                    //    else
                    //    {
                    //        item.SubItems[1].Text = "No value";
                    //    }
                    //}
                }
                ++completed;
            }
            catch (ManagementException ex)
            {
                ExceptionUtilities.TraceException(ex);
                item.SubItems[1].Text = "WMI error: " + ex.Message;
                ++other;
            }
            catch (COMException ex)
            {
                ExceptionUtilities.TraceException(ex);
                item.SubItems[1].Text = "Offline";
                ++other;
            }
            catch (Exception ex)
            {
                ExceptionUtilities.TraceException(ex);
                item.SubItems[1].Text = "Error: " + ex.Message;
                ++other;
            }
            finally
            {
                try
                {
                    Invoke(new BarDelegate(UpdateBar));
                }
                catch { }
            }
        }

        private void UpdateBar()
        {
            progressBar1.Value++;
            labelCompleted.Text = completed.ToString();
            labelOther.Text = other.ToString();
            labelTotal.Text = total.ToString();
            if (progressBar1.Value == progressBar1.Maximum)
            {
                buttonOK.Enabled = true;
            }
        }

        private void ButtonOK_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
