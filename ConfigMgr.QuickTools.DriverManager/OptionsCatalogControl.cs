using Microsoft.ConfigurationManagement.AdminConsole;
using Microsoft.ConfigurationManagement.AdminConsole.OsdCommon;
using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;


namespace ConfigMgr.QuickTools.DriverManager
{
    public partial class OptionsCatalogControl : SmsPageControl
    {
        private ControlsValidator controlsValidator;
        private ModifyRegistry registry = new ModifyRegistry();

        public OptionsCatalogControl(SmsPageData pageData)
          : base(pageData)
        {
            InitializeComponent();
            Title = "Catalog Options";
        }

        public override void InitializePageControl()
        {
            base.InitializePageControl();

            if (string.IsNullOrEmpty(registry.Read("DellCatalogURI")))
            {
                registry.Write("DellCatalogURI", "http://downloads.dell.com/catalog/DriverPackCatalog.cab");
            }

            if (string.IsNullOrEmpty(registry.Read("TempDownloadPath")))
            {
                registry.Write("TempDownloadPath", Path.GetTempPath());
            }

            textBoxDellCatalogUri.Text = registry.Read("DellCatalogURI");

            browseFolderControlDownload.Controls.OfType<SmsOsdTextBox>().First().Text = registry.Read("TempDownloadPath");

            ControlsInspector = new ControlsInspector();
            controlsValidator = new ControlsValidator(ControlsInspector);

            browseFolderControlDownload.SetErrorMessageFolderPath("Specify a valid path");
            browseFolderControlDownload.SetCustomValidateFilePath(new ControlDataStateEvaluator(ValidateDownloadDirectory));
            browseFolderControlDownload.SetValidator(controlsValidator);

            ControlsInspector.InspectAll();

            Dirty = false;

            Initialized = true;
        }

        protected override bool ApplyChanges(out Control errorControl, out bool showError)
        {
            registry.Write("TempDownloadPath", browseFolderControlDownload.FolderPath);
            registry.Write("DellCatalogURI", textBoxDellCatalogUri.Text);

            Dirty = false;

            return base.ApplyChanges(out errorControl, out showError);
        }

        private ControlDataState ValidateDownloadDirectory()
        {
            return browseFolderControlDownload.FolderPath.Length > 0 ? ControlDataState.Valid : ControlDataState.Invalid;
        }

        private void BrowseFolderControlDownload_FolderTextChanged(object sender, EventArgs e)
        {
            Dirty = true;
        }

        private void TextBoxDellCatalogUri_TextChanged(object sender, EventArgs e)
        {
            Dirty = true;
        }
    }
}
