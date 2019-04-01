using Microsoft.ConfigurationManagement.AdminConsole;
using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace ConfigMgr.QuickTools.Warranty
{
    public partial class DellWarrantyOptions : SmsCustomDialog
    {
        private ModifyRegistry registry = new ModifyRegistry();
        private SmsPageControl parentForm;

        public DellWarrantyOptions(SmsPageControl parent)
        {
            parentForm = parent;
            InitializeComponent();

            Title = "Dell Warranty Options";

            if (string.IsNullOrEmpty(registry.Read("DellAPIURI")))
            {
                registry.Write("DellAPIURI", "https://api.dell.com/support/assetinfo/v4/");
            }

            textBoxAPIUri.Text = registry.Read("DellAPIURI");
            textBoxAPIKey.Text = registry.Read("DellAPIKey");
        }

        private void ButtonOK_Click(object sender, EventArgs e)
        {
            registry.Write("DellAPIURI", textBoxAPIUri.Text);
            registry.Write("DellAPIKey", textBoxAPIKey.Text);

            if (string.IsNullOrEmpty(textBoxAPIKey.Text) || string.IsNullOrEmpty(textBoxAPIUri.Text))
            {
                parentForm.Controls["labelHttpResponse"].Text = "No Dell TechDirect API key set in options";
                parentForm.Controls["buttonSURefresh"].Enabled = false;
            }
            else
            {
                parentForm.Controls["labelHttpResponse"].Text = "";
                parentForm.Controls["buttonSURefresh"].Enabled = true;
            }
           
            DialogResult = DialogResult.OK;
            Close();
        }

        private void ButtonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void LinkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(linkLabel1.Text);
        }
    }
}
