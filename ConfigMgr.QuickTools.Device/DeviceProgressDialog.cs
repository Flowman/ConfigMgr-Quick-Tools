using Microsoft.ConfigurationManagement.AdminConsole;
using Microsoft.ConfigurationManagement.AdminConsole.Schema;
using System;
using System.Threading;
using System.Windows.Forms;

namespace ConfigMgr.QuickTools.Device
{
    public enum DeviceProgressStatus
    {
        Completed,
        Offline,
        Failed,
        Other
    }

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
        }

        public DeviceProgressDialog(ActionDescription action) : this()
        {
            Title = action.DisplayName;
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

        internal void UpdateProgress(DeviceProgressStatus status)
        {
            UpdateStatus(status);
            UpdateProgress();
        }

        internal void UpdateStatus(DeviceProgressStatus status)
        {
            switch (status)
            {
                case DeviceProgressStatus.Completed:
                    completed++;
                    break;
                case DeviceProgressStatus.Offline:
                    offline++;
                    break;
                case DeviceProgressStatus.Failed:
                    failed++;
                    break;
                case DeviceProgressStatus.Other:
                    other++;
                    break;
            }
        }

        internal void UpdateItem(string name, string text)
        {
            // fix issue when someone sort the list view while processing
            listViewHosts.Sorting = SortOrder.None;
            ListViewItem listViewItem = listViewHosts.FindItemWithText(name);
            listViewItem.SubItems[1].Text = text;
        }

        internal void AddItem(string name, string text = "Queued")
        {
            listViewHosts.Items.Add(new ListViewItem()
            {
                Text = name,
                SubItems = { text }
            });
            listViewHosts.UpdateColumnWidth(columnHeaderStatus);
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

