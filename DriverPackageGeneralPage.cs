using Microsoft.ConfigurationManagement.AdminConsole;
using Microsoft.ConfigurationManagement.AdminConsole.OsdCommon;
using System;
using System.Linq;
using System.Windows.Forms;

namespace Zetta.ConfigMgr.QuickTools
{
    public partial class DriverPackageGeneralPage : SmsPageControl
    {
        private ModifyRegistry registry = new ModifyRegistry();

        public DriverPackageGeneralPage(SmsPageData pageData)
            : base(pageData)
        {
            InitializeComponent();
            Headline = "Specify the location of the drivers";
            Title = "Locate Drivers";
            pageData.ProgressBarStyle = ProgressBarStyle.Continuous;
            FormTitle = "Driver Package Manager";
        }

        public override void InitializePageControl()
        {
            base.InitializePageControl();

            browseFolderControlSource.Controls.OfType<SmsOsdTextBox>().First().Text = registry.Read(browseFolderControlSource.Name);
            browseFolderControlPackage.Controls.OfType<SmsOsdTextBox>().First().Text = registry.Read(browseFolderControlPackage.Name);

            ControlsInspector.AddControl(browseFolderControlSource, new ControlDataStateEvaluator(ValidateSourceDirectory), "Set a valid UNC path");
            ControlsInspector.AddControl(browseFolderControlPackage, new ControlDataStateEvaluator(ValidatePackageDirectory), "Set a valid UNC path");
            Initialized = true;
        }

        private void browseFolderControl_FolderTextChanged(object sender, EventArgs e)
        {
            ControlsInspector.InspectAll();
        }

        private ControlDataState ValidatePackageDirectory()
        {
            return !OsdUtilities.CheckNetFolderPath(browseFolderControlPackage.FolderPath) ? ControlDataState.Invalid : ControlDataState.Valid;
        }

        private ControlDataState ValidateSourceDirectory()
        {
            return !OsdUtilities.CheckNetFolderPath(browseFolderControlSource.FolderPath) ? ControlDataState.Invalid : ControlDataState.Valid;
        }

        public override void OnActivated()
        {
            ControlsInspector.InspectAll();
            base.OnActivated();
        }

        public override bool OnNavigating(NavigationType navigationType)
        {
            if (navigationType == NavigationType.Forward)
            {
                UserData["ImportSourceLocation"] = browseFolderControlSource.FolderPath;
                UserData["ImportPackageLocation"] = browseFolderControlPackage.FolderPath;
                registry.Write(browseFolderControlPackage.Name, browseFolderControlPackage.FolderPath);
                registry.Write(browseFolderControlSource.Name, browseFolderControlSource.FolderPath);
            }
            return base.OnNavigating(navigationType);
        }

        private void checkBoxImportRemote_CheckedChanged(object sender, EventArgs e)
        {
            if (!checkBoxImportRemote.Checked)
            {
                UserData["UseXmlProfile"] = false;
                SetActiveBranch("{A2921EED-8647-430F-BCD5-8CF43E68FA17}");
                radioButtonDomain.Enabled = false;
                radioButtonWorkgroup.Enabled = false;
            }
            else if (checkBoxImportRemote.Checked)
            {
                UserData["UseXmlProfile"] = true;
                SetActiveBranch("{56304148-675D-4984-A318-1C45B0980256}");
                radioButtonDomain.Enabled = true;
                radioButtonWorkgroup.Enabled = true;
            }
            else
            {
                UserData["UseXmlProfile"] = false;
                SetActiveBranch(string.Empty);
            }
        }

        private void radioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonDomain.Checked == true)
            {
                UserData["UserCredentials"] = false;
                return;
            }
            else if (radioButtonWorkgroup.Checked == true)
            {
                UserData["UserCredentials"] = true;
                return;
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(linkLabel1.Text);
        }
    }
}

