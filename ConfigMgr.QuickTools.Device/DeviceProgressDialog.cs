using Microsoft.ConfigurationManagement.AdminConsole;
using System;
using System.ComponentModel;
using System.Threading;
using System.Windows.Forms;

namespace ConfigMgr.QuickTools.Device
{
    public partial class DeviceProgressDialog : SmsDialogBase
    {
        internal CancellationTokenSource CancellationTokenSource { get; set; } = new CancellationTokenSource();
        internal bool IsLoading { get { return listViewHosts.IsLoading; } set { listViewHosts.IsLoading = value; } }
        internal int Total { get; set; }
        internal int Completed { get; set; }
        internal int Offline { get; set; }
        internal int Failed { get; set; }
        internal int Other { get; set; }

        public DeviceProgressDialog()
        {
            InitializeComponent();

            IsLoading = true;
            listViewHosts.Items.Clear();

            clientActionProgressBar1.Maximum = Total;
            clientActionProgressBar1.Minimum = 0;
            clientActionProgressBar1.Offline = 0;
            clientActionProgressBar1.Complete = 0;
            clientActionProgressBar1.Failed = 0;
            clientActionProgressBar1.Style = ProgressBarStyle.Blocks;
        }

        internal void UpdateProgress()
        {
            clientActionProgressBar1.UpdateValues(Total, Completed, Offline, Failed);
            labelCompleted.Text = Completed.ToString();
            labelOther.Text = Other.ToString();
            labelTotal.Text = Total.ToString();
        }

        internal void UpdateItem(string name, string text)
        {
            ListViewItem listViewItem = listViewHosts.FindItemWithText(name);
            listViewItem.SubItems[1].Text = text;
        }

        private void ButtonCancel_Click(object sender, EventArgs e)
        {
            CancellationTokenSource.Cancel();
        }

        private void ButtonOK_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}

