using Microsoft.ConfigurationManagement.AdminConsole;
using Microsoft.ConfigurationManagement.AdminConsole.OsdCommon;
using System;
using System.Linq;
using System.Windows.Forms;


namespace ConfigMgr.QuickTools.DriverManager
{
    public partial class OptionsControl : SmsPageControl
    {
        private ControlsValidator controlsValidator;
        private ModifyRegistry registry = new ModifyRegistry();

        public OptionsControl(SmsPageData pageData)
          : base(pageData)
        {
            InitializeComponent();
            Title = "Driver Manager";
        }

        public override void InitializePageControl()
        {
            base.InitializePageControl();

            browseFolderControlSource.Controls.OfType<SmsOsdTextBox>().First().Text = registry.Read("DriverSourceFolder");
            browseFolderControlPackage.Controls.OfType<SmsOsdTextBox>().First().Text = registry.Read("DriverPackageFolder");

            string structure = registry.Read("LegacyFolderStructure");

            checkBoxLegacyFolder.Checked = string.IsNullOrEmpty(structure) ? false : Convert.ToBoolean(structure);

            ControlsInspector = new ControlsInspector();
            controlsValidator = new ControlsValidator(ControlsInspector);

            browseFolderControlSource.SetErrorMessageFolderPath("Specify a valid UNC path");
            browseFolderControlSource.SetCustomValidateFilePath(new ControlDataStateEvaluator(ValidateSourceDirectory));
            browseFolderControlSource.SetValidator(controlsValidator);

            browseFolderControlPackage.SetErrorMessageFolderPath("Specify a valid UNC path");
            browseFolderControlPackage.SetCustomValidateFilePath(new ControlDataStateEvaluator(ValidatePackageDirectory));
            browseFolderControlPackage.SetValidator(controlsValidator);

            ControlsInspector.InspectAll();

            Dirty = false;

            Initialized = true;
        }

        protected override bool ApplyChanges(out Control errorControl, out bool showError)
        {
            registry.Write("DriverSourceFolder", browseFolderControlSource.FolderPath);
            registry.Write("DriverPackageFolder", browseFolderControlPackage.FolderPath);
            registry.Write("LegacyFolderStructure", checkBoxLegacyFolder.Checked);

            Dirty = false;

            return base.ApplyChanges(out errorControl, out showError);
        }

        private ControlDataState ValidateSourceDirectory()
        {
            return !OsdUtilities.CheckNetFolderPath(browseFolderControlSource.FolderPath) ? ControlDataState.Invalid : ControlDataState.Valid;
        }

        private ControlDataState ValidatePackageDirectory()
        {
            return !OsdUtilities.CheckNetFolderPath(browseFolderControlPackage.FolderPath) ? ControlDataState.Invalid : ControlDataState.Valid;
        }

        private void BrowseFolderControlSource_FolderTextChanged(object sender, EventArgs e)
        {
            Dirty = true;
        }

        private void BrowseFolderControlPackage_FolderTextChanged(object sender, EventArgs e)
        {
            Dirty = true;
        }

        private void CheckBoxLegacyFolder_CheckedChanged(object sender, EventArgs e)
        {
            Dirty = true;
        }
    }
}
