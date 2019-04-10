using Microsoft.ConfigurationManagement.AdminConsole;
using System.Diagnostics;
using System.Windows.Forms;

namespace ConfigMgr.QuickTools.Warranty
{
    public partial class OptionsControl : SmsPageControl
    {
        private ModifyRegistry registry = new ModifyRegistry();

        public OptionsControl(SmsPageData pageData)
          : base(pageData)
        {
            InitializeComponent();
            Title = "Dell Warranty";
        }

        public override void InitializePageControl()
        {
            base.InitializePageControl();

            if (string.IsNullOrEmpty(registry.Read("DellAPIURI")))
            {
                registry.Write("DellAPIURI", "https://api.dell.com/support/assetinfo/v4/");
            }

            textBoxAPIUri.Text = registry.Read("DellAPIURI");
            textBoxAPIKey.Text = registry.Read("DellAPIKey");

            Dirty = false;

            if (PropertyManager != null)
            {
                UpdateUserData();

                PropertyManager.UserDataObject = UserData;
            }

            Initialized = true;
        }

        protected override bool ApplyChanges(out Control errorControl, out bool showError)
        {
            registry.Write("DellAPIURI", textBoxAPIUri.Text);
            registry.Write("DellAPIKey", textBoxAPIKey.Text);

            Dirty = false;

            if (PropertyManager != null)
            {
                UpdateUserData();

                PropertyManager.UserDataObject = UserData;
            }

            return base.ApplyChanges(out errorControl, out showError);
        }

        private void UpdateUserData()
        {
            if (string.IsNullOrEmpty(textBoxAPIKey.Text) || string.IsNullOrEmpty(textBoxAPIUri.Text))
            {
                UserData["HttpResponse"] = "No Dell TechDirect API key set in options";
                UserData["RefreshEnabled"] = false;
            }
            else
            {
                UserData["HttpResponse"] = "";
                UserData["RefreshEnabled"] = true;
            }
        }

        private void LinkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(linkLabel1.Text);
        }

        private void TextBoxAPIKey_TextChanged(object sender, System.EventArgs e)
        {
            Dirty = true;
        }

        private void TextBoxAPIUri_TextChanged(object sender, System.EventArgs e)
        {
            Dirty = true;
        }
    }
}
