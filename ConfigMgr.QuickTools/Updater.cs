using System;
using System.Diagnostics;
using System.Reflection;
using System.Windows.Forms;
using AutoUpdaterDotNET;

namespace ConfigMgr.QuickTools
{
    public class Updater
    {
        private static ModifyRegistry registry = new ModifyRegistry();
        private static bool check = false;

        public static void CheckUpdates()
        {
            CheckOptions();

            if (registry.ReadBool("UpdateEnabled"))
            {
                DateTime date1 = DateTime.Now;
                DateTime date2 = DateTime.Parse(registry.ReadString("UpdateLastCheck")).AddDays(registry.ReadInt("UpdateInterval"));
                int result = DateTime.Compare(date1, date2);

                if (result > 0 && !check)
                {
                    AutoUpdater.Start("https://raw.githubusercontent.com/Flowman/ConfigMgr-Quick-Tools/master/ConfigMgr.QuickTools/AutoUpdate.xml", Assembly.GetExecutingAssembly());
                    AutoUpdater.CheckForUpdateEvent += AutoUpdaterOnCheckForUpdateEvent;

                    // update last check so we dont hammer the poor user with update checks.
                    registry.Write("UpdateLastCheck", DateTime.Now.ToString());

                    check = true;
                }
            }
        }

        public static void CheckUpdatesNow()
        {
            CheckOptions();

            AutoUpdater.Start("https://raw.githubusercontent.com/Flowman/ConfigMgr-Quick-Tools/master/ConfigMgr.QuickTools/AutoUpdate.xml", Assembly.GetExecutingAssembly());
            AutoUpdater.CheckForUpdateEvent += AutoUpdaterOnCheckForUpdateEvent;

            // update last check so we dont hammer the poor user with update checks.
            registry.Write("UpdateLastCheck", DateTime.Now.ToString());
        }

        private static void CheckOptions()
        {
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
        }

        private static void AutoUpdaterOnCheckForUpdateEvent(UpdateInfoEventArgs args)
        {
            if (args != null)
            {
                if (args.IsUpdateAvailable)
                {
                    DialogResult dialogResult =
                        MessageBox.Show(
                            string.Format("There is new version {0} available. You are using version {1}. Do you want to update the application now?", args.CurrentVersion, args.InstalledVersion),
                            "ConfigMgr Quick Tools Update Available",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Information
                        );

                    if (dialogResult.Equals(DialogResult.Yes))
                    {
                        try
                        {
                            if (AutoUpdater.DownloadUpdate())
                            {
                                Process.GetCurrentProcess().Kill();
                            }
                        }
                        catch (Exception exception)
                        {
                            MessageBox.Show(exception.Message, exception.GetType().ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
        }
    }
}
