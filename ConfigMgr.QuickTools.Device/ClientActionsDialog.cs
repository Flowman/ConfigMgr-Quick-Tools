using Microsoft.ConfigurationManagement.AdminConsole;
using Microsoft.ConfigurationManagement.ManagementProvider;
using Microsoft.ConfigurationManagement.AdminConsole.Schema;
using System;
using System.Windows.Forms;
using System.Threading;
using System.Management;
using System.Runtime.InteropServices;
using System.Text;

namespace ConfigMgr.QuickTools.Device
{
    public partial class ClientActionsDialog : SmsCustomDialog
    {
        private IResultObject resultObjects;
        private string scheduleId;
        private bool fullScan;
        private int completed;
        private int offline;
        private int failed;
        private int other;
        private int total;
        private CancellationTokenSource cts;

        public delegate void BarDelegate();

        public ClientActionsDialog(IResultObject selectedResultObjects, string id, ActionDescription action, bool full)
        {
            InitializeComponent();

            resultObjects = selectedResultObjects;
            scheduleId = id;
            fullScan = full;
            Title = action.DisplayName;
        }

        private void ClientActionsDialog_Shown(object sender, EventArgs e)
        {
            cts = new CancellationTokenSource();
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
                ThreadPool.QueueUserWorkItem(arg => { ClientAction(resultObject, cts.Token); });
            }

            listViewHosts.UpdateColumnWidth(columnHeaderName);
            labelTotal.Text = total.ToString();
            clientActionProgressBar1.Maximum = total;
            clientActionProgressBar1.Minimum = 0;
            clientActionProgressBar1.Offline = 0;
            clientActionProgressBar1.Complete = 0;
            clientActionProgressBar1.Failed = 0;
            clientActionProgressBar1.Style = ProgressBarStyle.Blocks;

            listViewHosts.IsLoading = false;
        }

        private void ClientAction(IResultObject resultObject, CancellationToken token)
        {
            ListViewItem item = listViewHosts.FindItemWithText(resultObject["Name"].StringValue);

            if (token.IsCancellationRequested)
            {
                item.SubItems[1].Text = "Canceled";
                return;
            }
           
            try
            {
                item.SubItems[1].Text = "Connecting";
                ObjectGetOptions o = new ObjectGetOptions
                {
                    Timeout = new TimeSpan(0, 0, 5)
                };
                if (fullScan)
                {
                    ManagementScope inventoryAgentScope = new ManagementScope(string.Format(@"\\{0}\root\{1}", resultObject["Name"].StringValue, "ccm\\InvAgt"));
                    using (ManagementClass inventoryClass = new ManagementClass(inventoryAgentScope.Path.Path, "InventoryActionStatus", o))
                    {
                        // Query the class for the InventoryActionID object (create query, create searcher object, execute query).
                        ObjectQuery query = new ObjectQuery(string.Format("SELECT * FROM InventoryActionStatus WHERE InventoryActionID = '{0}'", scheduleId));
                        ManagementObjectSearcher searcher = new ManagementObjectSearcher(inventoryAgentScope, query);
                        ManagementObjectCollection queryResults = searcher.Get();

                        // Enumerate the collection to get to the result (there should only be one item returned from the query).
                        foreach (ManagementObject result in queryResults)
                        {
                            // Display message and delete the object.
                            result.Delete();
                        }
                    }
                }
                using (ManagementClass clientaction = new ManagementClass(string.Format(@"\\{0}\root\{1}:{2}", resultObject["Name"].StringValue, "ccm", "SMS_Client"), o))
                {
                    object[] methodArgs = { scheduleId };
                    clientaction.InvokeMethod("TriggerSchedule", methodArgs);
                }
                item.SubItems[1].Text = "Completed";
                ++completed;
            }
            catch (ManagementException ex)
            {
                ExceptionUtilities.TraceException(ex);
                item.SubItems[1].Text = "WMI error: " + ex.Message;
                ++other;
                ++failed;
            }
            catch (COMException ex)
            {
                ExceptionUtilities.TraceException(ex);
                item.SubItems[1].Text = "Offline";
                ++other;
                ++offline;
            }
            catch (Exception ex)
            {
                ExceptionUtilities.TraceException(ex);
                item.SubItems[1].Text = "Error: " + ex.Message;
                ++other;
                ++failed;
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
            clientActionProgressBar1.UpdateValues(total, completed, offline, failed);
            labelCompleted.Text = completed.ToString();
            labelOther.Text = other.ToString();
            labelTotal.Text = total.ToString();
            if (clientActionProgressBar1.Value == clientActionProgressBar1.Maximum)
            {
                buttonOK.Enabled = true;
            }
        }

        private void ButtonOK_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void ButtonCancel_Click(object sender, EventArgs e)
        {
            cts.Cancel();
            buttonOK.Enabled = true;
        }

        private void ClientActionsDialog_FormClosing(object sender, FormClosingEventArgs e)
        {
            cts.Cancel();
        }

        private void ListViewHosts_CopyKeyEvent(object sender, EventArgs e)
        {
            StringBuilder buffer = new StringBuilder();
            foreach (ListViewItem item in listViewHosts.SelectedItems)
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
