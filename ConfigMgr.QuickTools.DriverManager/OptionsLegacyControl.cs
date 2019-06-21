using Microsoft.ConfigurationManagement.AdminConsole;
using Microsoft.ConfigurationManagement.AdminConsole.OsdCommon;
using System;
using System.Linq;
using System.Windows.Forms;

namespace ConfigMgr.QuickTools.DriverManager
{
    public partial class OptionsLegacyControl : SmsPageControl
    {
        private readonly ModifyRegistry registry = new ModifyRegistry();

        public OptionsLegacyControl(SmsPageData pageData)
          : base(pageData)
        {
            InitializeComponent();
            Title = "Legacy Packages";
        }

        public override void InitializePageControl()
        {
            base.InitializePageControl();

            browseFolderControlLegacyPackage.Controls.OfType<SmsOsdTextBox>().First().Text = registry.ReadString("LegacyPackageFolder");
            checkBoxZipContent.Checked = registry.ReadBool("LegacyPackageZipContent");
            textBoxConsoleFolder.Text = registry.ReadString("LegacyConsoleFolder");
        }

        protected override bool ApplyChanges(out Control errorControl, out bool showError)
        {
            registry.Write("LegacyPackageFolder", browseFolderControlLegacyPackage.FolderPath);
            registry.Write("LegacyPackageZipContent", checkBoxZipContent.Checked);
            registry.Write("LegacyConsoleFolder", textBoxConsoleFolder.Text);

            Dirty = false;

            return base.ApplyChanges(out errorControl, out showError);
        }

        private void Control_Changed(object sender, EventArgs e)
        {
            Dirty = true;
        }
    }
}
