using Microsoft.ConfigurationManagement.AdminConsole;
using Microsoft.ConfigurationManagement.ManagementProvider;
using Microsoft.ConfigurationManagement.AdminConsole.Schema;
using Microsoft.ConfigurationManagement.AdminConsole.DialogFramework;
using System.DirectoryServices;
using System.Windows.Forms;
using System;

namespace ConfigMgr.QuickTools.Device
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

        public static void Show(object sender, ScopeNode scopeNode, ActionDescription action, IResultObject selectedObject, PropertyDataUpdated dataUpdatedDelegate, Status Status)
        {
            using (LAPSDialog dialog = new LAPSDialog(selectedObject, action))
            {
                dialog.ShowDialog();
                return;
            }
        }

        private void LAPSDialog_Shown(object sender, EventArgs e)
        {
            try
            {
                using (DirectorySearcher search = new DirectorySearcher
                {
                    Filter = string.Format("(&(objectCategory=computer)(objectClass=computer)(cn={0}))", resultObjects["Name"].StringValue)
                })
                {
                    search.PropertiesToLoad.Add("distinguishedName");
                    search.PropertiesToLoad.Add("ms-mcs-admpwdexpirationtime");
                    search.PropertiesToLoad.Add("ms-mcs-admpwd");

                    SearchResult result = search.FindOne();

                    if (result != null)
                    {
                        if (result.Properties.Contains("ms-mcs-admpwdexpirationtime"))
                        {
                            long value = (long)result.Properties["ms-mcs-admpwdexpirationtime"][0];
                            DateTime expire = DateTime.FromFileTimeUtc(value);

                            labelExpire.Text = expire.ToString();
                        }
                        if (result.Properties.Contains("ms-mcs-admpwd"))
                        {
                            textBoxPassword.Text = result.Properties["ms-mcs-admpwd"][0].ToString();
                        }
                        else
                        {
                            textBoxPassword.Text = "No LAPS data";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionUtilities.TraceException(ex);
                SccmExceptionDialog.ShowDialog(ex);
            }
        }

        private void ButtonOK_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
