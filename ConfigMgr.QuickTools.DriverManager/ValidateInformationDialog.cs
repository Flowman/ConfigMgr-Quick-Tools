using Microsoft.ConfigurationManagement.AdminConsole;
using System.ComponentModel;
using System.Windows.Forms;

namespace ConfigMgr.QuickTools.DriverManager
{
    public partial class ValidateInformationDialog : SmsDialogBase
    {
        internal bool ReceivedRequestToClose { get; set; }
        internal bool CanClose { get; set; }
        internal bool Result { get; set; }

        public ValidateInformationDialog()
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
        }

        internal void CloseDialog()
        {
            Enabled = true;
            CanClose = true;
            Close();
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
