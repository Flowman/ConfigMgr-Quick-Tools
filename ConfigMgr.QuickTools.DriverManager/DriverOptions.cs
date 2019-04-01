using Microsoft.ConfigurationManagement.AdminConsole;
using Microsoft.ConfigurationManagement.AdminConsole.OsdCommon;
using System;
using System.Linq;
using System.Windows.Forms;


namespace ConfigMgr.QuickTools.DriverManager
{
    public partial class DriverOptions : SmsCustomDialog
    {
        protected virtual ControlsInspector ControlsInspector { get; set; }
        private ControlsValidator controlsValidator;
        private ModifyRegistry registry = new ModifyRegistry();

        public DriverOptions(SmsPageControl parent)
        {
            InitializeComponent();

            Title = "Driver Manager Options";

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
        }

        private void ButtonOK_Click(object sender, EventArgs e)
        {
            registry.Write("DriverSourceFolder", browseFolderControlSource.FolderPath);
            registry.Write("DriverPackageFolder", browseFolderControlPackage.FolderPath);
            registry.Write("LegacyFolderStructure", checkBoxLegacyFolder.Checked);

            ControlsInspector.InspectAll();

            if (ControlsInspector.CurrentInvalidControl == null)
            {
                registry.Write("DriverPackageFolder", browseFolderControlPackage.FolderPath);
                registry.Write("DriverSourceFolder", browseFolderControlSource.FolderPath);

                DialogResult = DialogResult.OK;
                Close();
            }
        }

        private ControlDataState ValidateSourceDirectory()
        {
            return !OsdUtilities.CheckNetFolderPath(browseFolderControlSource.FolderPath) ? ControlDataState.Invalid : ControlDataState.Valid;
        }

        private ControlDataState ValidatePackageDirectory()
        {
            return !OsdUtilities.CheckNetFolderPath(browseFolderControlPackage.FolderPath) ? ControlDataState.Invalid : ControlDataState.Valid;
        }

        private void ButtonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
