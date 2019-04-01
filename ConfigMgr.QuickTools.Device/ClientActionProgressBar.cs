using System.Drawing;
using System.Windows.Forms;

namespace ConfigMgr.QuickTools.Device
{
    class ClientActionProgressBar : ProgressBar
    {
        private int complete;
        private int offline;
        private int failed;
        private const int WmPaint = 15;

        public int Complete
        {
            get
            {
                return complete;
            }
            set
            {
                complete = value;
                Value = complete + offline + failed;
            }
        }

        public int Offline
        {
            get
            {
                return offline;
            }
            set
            {
                offline = value;
                Value = complete + offline + failed;
            }
        }

        public int Failed
        {
            get
            {
                return failed;
            }
            set
            {
                failed = value;
                Value = complete + offline + failed;
            }
        }

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            if (m.Msg != 15 || Style == ProgressBarStyle.Marquee || offline <= 0 && failed <= 0)
                return;
            using (Graphics graphics = Graphics.FromHwnd(Handle))
            {
                int num = 0;
                if (failed > 0)
                {
                    using (SolidBrush solidBrush = new SolidBrush(Color.Red))
                    {
                        num = (int)((double)failed / (double)Maximum * (double)Width);
                        graphics.FillRectangle(solidBrush, 0, 0, num, Height);
                    }
                }
                if (offline <= 0)
                    return;
                using (SolidBrush solidBrush = new SolidBrush(Color.FromArgb(236, 204, 114)))
                {
                    int width = (int)((double)offline / (double)Maximum * (double)Width);
                    graphics.FillRectangle(solidBrush, num, 0, width, Height);
                }
            }
        }

        public void UpdateValues(int max, int complete, int offline, int failed)
        {
            Maximum = max;
            this.complete = complete;
            this.offline = offline;
            this.failed = failed;
            Value = this.complete + this.offline + this.failed;
        }
    }
}
