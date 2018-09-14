using Microsoft.ConfigurationManagement.AdminConsole;
using Microsoft.ConfigurationManagement.ManagementProvider;
using Microsoft.ConfigurationManagement.AdminConsole.Schema;
using System.DirectoryServices;
using System.Windows.Forms;
using System;

namespace Zetta.ConfigMgr.QuickTools
{
    public partial class LAPSDialog : SmsCustomDialog
    {
        const AuthenticationTypes authenticationTypes = AuthenticationTypes.Secure | AuthenticationTypes.Sealing;
        private IResultObject resultObjects;

        public LAPSDialog(IResultObject selectedResultObjects, ActionDescription action)
        {
            InitializeComponent();
            resultObjects = selectedResultObjects;
            Title = action.DisplayName + ": " + resultObjects["Name"].StringValue;
        }

        private void LAPSDialog_Shown(object sender, System.EventArgs e)
        {

            DirectorySearcher search = new DirectorySearcher
            {
               Filter = string.Format("(&(objectCategory=computer)(objectClass=computer)(cn={0}))", resultObjects["Name"].StringValue)
            };
            search.PropertiesToLoad.Add("distinguishedName");
            search.PropertiesToLoad.Add("ms-mcs-admpwdexpirationtime");
            search.PropertiesToLoad.Add("ms-mcs-admpwd");

            SearchResult result = search.FindOne();

            if (result != null)
            {
                long value = (long)result.Properties["ms-mcs-admpwdexpirationtime"][0];
                DateTime expire = DateTime.FromFileTimeUtc(value);

                labelExpire.Text = expire.ToString();
                textBoxPassword.Text = result.Properties["ms-mcs-admpwd"][0].ToString();
            }
        }

        private void buttonOK_Click(object sender, System.EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
