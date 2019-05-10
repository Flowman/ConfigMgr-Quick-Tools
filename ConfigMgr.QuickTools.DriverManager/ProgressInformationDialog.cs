using Microsoft.ConfigurationManagement.AdminConsole;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace ConfigMgr.QuickTools.DriverManager
{
    public partial class ProgressInformationDialog : SmsDialogBase
    {
        internal bool ReceivedRequestToClose { get; set; }
        internal bool CanClose { get; set; }
        internal bool Result { get; set; }
        internal ProgressBarStyle ProgressBarStyle{ get { return progressBar.Style; } set { progressBar.Style = value; } }

        public ProgressInformationDialog()
        {
            InitializeComponent();
        }

        internal void UpdateProgressText(string progressText)
        {
            labelInformation.Text = progressText;
        }

        internal void UpdateProgressValue(int progressValue)
        {
            progressBar.Value = progressValue;

            if (ProgressBarStyle == ProgressBarStyle.Continuous)
            {
                labelPercent.Text = progressValue + "%";
            }
        }

        internal void CloseDialog()
        {
            Enabled = true;
            CanClose = true;
            Close();
        }

        protected override void OnShown(EventArgs e)
        {
            if (ProgressBarStyle == ProgressBarStyle.Continuous)
            {
                progressBar.Width = 480;
                labelPercent.Visible = true;
            }

            base.OnShown(e);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if (!CanClose)
            {
                if (!ReceivedRequestToClose)
                {
                    if (MessageBox.Show("Do you want to cancel?", DialogConstants.MessageBoxTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                    {
                        if (Result)
                        {
                            Enabled = true;
                            CloseDialog();
                            return;
                        }
                        labelInformation.Text = "Cancelling ...";
                        ReceivedRequestToClose = true;
                        Enabled = false;
                    }
                    else if (Result)
                    {
                        Enabled = true;
                        CloseDialog();
                        return;
                    }
                }
                e.Cancel = true;
            }
            else
                base.OnClosing(e);
        }
    }
}
