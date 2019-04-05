using Microsoft.ConfigurationManagement.AdminConsole;
using Microsoft.ConfigurationManagement.AdminConsole.WizardFramework;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ConfigMgr.QuickTools.DriverManager
{
    public partial class DriverPackageGeneralPage : SmsPageControl
    {
        private ModifyRegistry registry = new ModifyRegistry();

        public DriverPackageGeneralPage(SmsPageData pageData)
            : base(pageData)
        {
            FormTitle = "Driver Package Manager";
            Title = "General";
            Headline = "Information";

            InitializeComponent();

            pageData.ProgressBarStyle = ProgressBarStyle.Continuous;
        }

        public override void InitializePageControl()
        {
            base.InitializePageControl();

            labelInformation.Text = string.Format("Welcome to the Driver Package Manager Import tool.\r\n\r\nThis tool gives you a quick way to work with your driver packages as no ConfigMgr skill are required. Just create your driver structure and manage all drivers and packages on a storage level.\r\n\r\nThe tool will import your driver packages from {0}.", registry.Read("DriverPackageFolder"));

            Initialized = true;

            buttonOptions.Image = new Icon(Properties.Resources.options, new Size(16, 16)).ToBitmap();
        }

        protected override void ApplyChanges()
        {
            base.ApplyChanges();
        }

        public override void PostApply(BackgroundWorker worker, DoWorkEventArgs e)
        {
            base.PostApply(worker, e);
        }

        public override void OnAddSummary(SummaryRequestHandler handler)
        {
            base.OnAddSummary(handler);
        }

        public override void OnActivated()
        {
            base.OnActivated();

            StringBuilder sb = new StringBuilder();

            if (string.IsNullOrEmpty(registry.Read("DriverSourceFolder")))
            {
                ((SmsWizardPage)Parent).WizardForm.EnableButton(ButtonType.Next, false);
                sb.AppendLine("No driver source path specified!");
            }

            if (string.IsNullOrEmpty(registry.Read("DriverPackageFolder")))
            {
                ((SmsWizardPage)Parent).WizardForm.EnableButton(ButtonType.Next, false);
                sb.AppendLine("No driver package path specified!");
            }

            labelOptions.Text = sb.ToString();
        }

        private void ButtonOptions_Click(object sender, System.EventArgs e)
        {
            ShowDialog("QuickToolsOptions", PropertyManager);
        }
    }
}
