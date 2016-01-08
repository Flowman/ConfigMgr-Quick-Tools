using Microsoft.ConfigurationManagement.AdminConsole;
using Microsoft.ConfigurationManagement.ManagementProvider;
using Microsoft.ConfigurationManagement.AdminConsole.Schema;
using System;
using System.Windows.Forms;
using System.Threading;
using System.Management;
using System.Runtime.InteropServices;

namespace Zetta.ConfigMgr.QuickTools
{
    public partial class ClientActionsDialog : SmsDialogBase
    {
        private IResultObject resultObjects;
        private string scheduleId;
        private int completed;
        private int other;
        private int total;

        public delegate void BarDelegate();

        public ClientActionsDialog(IResultObject selectedResultObjects, string id, ActionDescription action)
        {
            InitializeComponent();
            resultObjects = selectedResultObjects;
            scheduleId = id;
            Title = action.DisplayName;
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            Close();
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
                ObjectGetOptions o = new ObjectGetOptions();
                o.Timeout = new TimeSpan(0, 0, 5);
                ManagementClass clientaction = new ManagementClass(string.Format(@"\\{0}\root\{1}:{2}", resultObject["Name"].StringValue, "ccm", "SMS_Client"), o);

                object[] methodArgs = { scheduleId };
                clientaction.InvokeMethod("TriggerSchedule", methodArgs);
                clientaction.Dispose();
                item.SubItems[1].Text = "Completed";
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
                Invoke(new BarDelegate(UpdateBar));
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

            }
        }
    }
}
;