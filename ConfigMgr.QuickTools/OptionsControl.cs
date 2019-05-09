using Microsoft.ConfigurationManagement.AdminConsole;
using System;
using System.Windows.Forms;

namespace ConfigMgr.QuickTools
{
    public partial class OptionsControl : SmsPageControl
    {
        private readonly ModifyRegistry registry = new ModifyRegistry();

        public OptionsControl(SmsPageData pageData)
          : base(pageData)
        {
            InitializeComponent();
            Title = "Auto Updates";
        }

        public override void InitializePageControl()
        {
            base.InitializePageControl();

            if (string.IsNullOrEmpty(registry.ReadString("UpdateEnabled")))
            {
                registry.Write("UpdateEnabled", true);
            }

            if (string.IsNullOrEmpty(registry.ReadString("UpdateLastCheck")))
            {
                registry.Write("UpdateLastCheck", DateTime.Now.ToString());
            }

            if (registry.ReadInt("UpdateInterval") == 0)
            {
                registry.Write("UpdateInterval", 7);
            }

            labelLastChecked.Text = registry.ReadString("UpdateLastCheck");

            numericUpDownInterval.Value = registry.ReadInt("UpdateInterval");

            checkEnable.Checked = registry.ReadBool("UpdateEnabled");

            Dirty = false;

            Initialized = true;
        }

        protected override bool ApplyChanges(out Control errorControl, out bool showError)
        {
            registry.Write("UpdateEnabled", checkEnable.Checked);
            registry.Write("UpdateInterval", (int)numericUpDownInterval.Value);

            Dirty = false;

            return base.ApplyChanges(out errorControl, out showError);
        }

        private void CheckEnable_CheckedChanged(object sender, EventArgs e)
        {
            Dirty = true;
        }

        private void NumericUpDownInterval_ValueChanged(object sender, EventArgs e)
        {
            Dirty = true;
        }

        private void ButtonCheckNow_Click(object sender, EventArgs e)
        {
            Updater.CheckUpdatesNow();
        }
    }
}
