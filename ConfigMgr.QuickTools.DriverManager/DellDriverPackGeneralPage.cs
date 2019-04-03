using Microsoft.ConfigurationManagement.AdminConsole;
using Microsoft.ConfigurationManagement.AdminConsole.WizardFramework;
using Microsoft.ConfigurationManagement.ManagementProvider.DialogFramework;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace ConfigMgr.QuickTools.DriverManager
{
    public partial class DellDriverPackGeneralPage : SmsPageControl
    {
        private ModifyRegistry registry = new ModifyRegistry();

        public DellDriverPackGeneralPage(SmsPageData pageData)
            : base(pageData)
        {
            FormTitle = "Dell Driver Pack";
            Title = "Select Architecture";
            Headline = "Select OS and Architecture to narrow down selections";

            InitializeComponent();

            pageData.ProgressBarStyle = ProgressBarStyle.Continuous;
        }

        public override void InitializePageControl()
        {
            base.InitializePageControl();
            Initialized = true;

            buttonOptions.Image = new Icon(Properties.Resources.options, new Size(16, 16)).ToBitmap();
            comboBoxOS.SelectedIndex = 0;
            comboBoxArchitecture.SelectedIndex = 0;
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

            if (string.IsNullOrEmpty(registry.Read("DriverSourceFolder")))
            {
                ((SmsWizardPage)Parent).WizardForm.EnableButton(ButtonType.Next, false);
                labelOptions.Text = "No driver source structure specified!";
            }
        }

        public override bool OnDeactivate()
        {
            UserData["OS"] = comboBoxOS.Text;
            UserData["Architecture"] = comboBoxArchitecture.Text;

            return base.OnDeactivate();
        }

        private void ButtonOptions_Click(object sender, EventArgs e)
        {
            ShowDialog("QuickToolsOptions", PropertyManager);
        }
    }
}
