using Microsoft.ConfigurationManagement.AdminConsole;
using Microsoft.ConfigurationManagement.AdminConsole.Common;
using Microsoft.ConfigurationManagement.ManagementProvider;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace ConfigMgr.QuickTools
{
    public partial class QuickDeploymentSettingsPage : SmsPageControl
    {
        public QuickDeploymentSettingsPage(SmsPageData pageData)
            : base(pageData)
        {
            InitializeComponent();
        }
    }
}
