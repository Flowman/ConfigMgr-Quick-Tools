using System.Windows.Forms;

namespace ConfigMgr.QuickTools.Device
{
    public partial class TrackBarWithoutFocus : TrackBar
    {
        private const int WM_SETFOCUS = 0x0007;

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_SETFOCUS)
            {
                return;
            }

            base.WndProc(ref m);
        }
    }
}
