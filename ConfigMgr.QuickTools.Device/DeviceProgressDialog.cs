using Microsoft.ConfigurationManagement.AdminConsole;
using Microsoft.ConfigurationManagement.AdminConsole.Schema;
using Microsoft.ConfigurationManagement.ManagementProvider;
using System;
using System.Management;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace ConfigMgr.QuickTools.Device
{
    public partial class DeviceProgressDialog : SmsDialogBase
    {
        internal CancellationTokenSource CancellationTokenSource { get; set; } = new CancellationTokenSource();
        internal bool IsLoading { get { return listViewHosts.IsLoading; } set { listViewHosts.IsLoading = value; } }
        internal int Total {
            get { return clientActionProgressBar1.Maximum; }
            set {
                clientActionProgressBar1.Maximum = value;
                labelTotal.Text = value.ToString();
            }
        }

        private readonly IResultObject resultObjects;
        private readonly MethodInfo method;
        private int completed;
        private int offline;
        private int failed;
        private int other;

        public DeviceProgressDialog()
        {
            InitializeComponent();

            IsLoading = true;
            listViewHosts.Items.Clear();

            clientActionProgressBar1.Minimum = 0;
            clientActionProgressBar1.Offline = 0;
            clientActionProgressBar1.Complete = 0;
            clientActionProgressBar1.Failed = 0;
            clientActionProgressBar1.Style = ProgressBarStyle.Blocks;

            UpdateProgress();

            Updater.CheckUpdates();
        }

        public DeviceProgressDialog(ActionDescription action, IResultObject selectedResultObjects, MethodInfo method) : this()
        {
            Title = action.DisplayName;

            resultObjects = selectedResultObjects;
            this.method = method;
        }

        private void DeviceProgressDialog_Shown(object sender, EventArgs e)
        {
            foreach (IResultObject resultObject in resultObjects)
            {
                // speed up the form by not searching for the item later, just include it into the run action
                ListViewItem item = new ListViewItem()
                {
                    Text = resultObject["Name"].StringValue,
                    SubItems = { "Queued" }
                };

                listViewHosts.Items.Add(item);

                ThreadPool.QueueUserWorkItem(arg => { RunAction(resultObject, item); });
            }

            listViewHosts.UpdateColumnWidth(columnHeaderStatus);
        }

        private void RunAction(IResultObject resultObject, ListViewItem item)
        {
            if (CancellationTokenSource.IsCancellationRequested)
            {
                item.SubItems[1].Text = "Canceled";
                return;
            }

            try
            {
                item.SubItems[1].Text = "Connecting";

                method.Invoke(null, new object[] { resultObject });

                completed++;
                item.SubItems[1].Text = "Completed";
            }
            catch (TargetInvocationException ex)
            {
                if (ex.InnerException.GetType().IsAssignableFrom(typeof(ManagementException)))
                {
                    item.SubItems[1].Text = "WMI error: " + ex.InnerException.Message;
                    failed++;
                    other++;
                }
                else if (ex.InnerException.GetType().IsAssignableFrom(typeof(COMException)))
                {
                    item.SubItems[1].Text = "Offline";
                    offline++;
                    other++;
                }
                else
                {
                    item.SubItems[1].Text = "Error: " + ex.InnerException.Message;
                    failed++;
                    other++;
                }
            } 
            finally
            {
                Invoke((MethodInvoker)delegate { UpdateProgress(); });
            }
        }

        internal void UpdateProgress()
        {
            clientActionProgressBar1.UpdateValues(completed, offline, failed);
            labelCompleted.Text = completed.ToString();
            labelOther.Text = other.ToString();

            if (clientActionProgressBar1.Value == clientActionProgressBar1.Maximum)
            {
                IsLoading = false;
                buttonOK.Enabled = true;
            }
        }

        private void ButtonCancel_Click(object sender, EventArgs e)
        {
            CancellationTokenSource.Cancel();
            IsLoading = false;
            buttonOK.Enabled = true;
        }

        private void ButtonOK_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void DeviceProgressDialog_FormClosing(object sender, FormClosingEventArgs e)
        {
            CancellationTokenSource.Cancel();
        }
    }
}

