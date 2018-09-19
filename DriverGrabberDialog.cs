using Microsoft.ConfigurationManagement.AdminConsole;
using Microsoft.ConfigurationManagement.ManagementProvider;
using Microsoft.ConfigurationManagement.AdminConsole.Schema;
using System.DirectoryServices;
using System.Windows.Forms;
using System;

namespace Zetta.ConfigMgr.QuickTools
{
    public partial class DriverGrabberDialog : SmsCustomDialog
    {
        private IResultObject resultObject;

        public DriverGrabberDialog(IResultObject selectedResultObject)
        {
            resultObject = selectedResultObject;
            InitializeComponent();
        }

        public static void Show(object sender, ScopeNode scopeNode, ActionDescription action, IResultObject selectedResultObject, PropertyDataUpdated dataUpdatedDelegate, Status status)
        {
            new DriverGrabberDialog(selectedResultObject).Show();
        }
    }
}
