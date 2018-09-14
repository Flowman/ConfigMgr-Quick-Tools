using Microsoft.ConfigurationManagement.AdminConsole;
using System;
using System.Windows.Forms;

namespace Zetta.ConfigMgr.QuickTools
{
    public partial class DellWarrantyOptions : SmsCustomDialog
    {
        private ModifyRegistry registry = new ModifyRegistry();

        public DellWarrantyOptions()
        {
            InitializeComponent();

            Title = "Dell Warranty Options";

            if (registry.Read("DellAPIURI") == null)
            {
                registry.Write("DellAPIURI", "https://api.dell.com/support/assetinfo/v4/");
            }

            textBoxAPIUri.Text = registry.Read("DellAPIURI");
            textBoxAPIKey.Text = registry.Read("DellAPIKey");
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            registry.Write("DellAPIURI", textBoxAPIUri.Text);
            registry.Write("DellAPIKey", textBoxAPIKey.Text);

            DialogResult = DialogResult.OK;
            Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
