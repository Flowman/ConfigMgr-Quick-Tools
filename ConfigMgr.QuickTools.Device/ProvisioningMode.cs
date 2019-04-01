using Microsoft.ConfigurationManagement.AdminConsole;
using Microsoft.ConfigurationManagement.ManagementProvider;
using Microsoft.ConfigurationManagement.AdminConsole.Schema;
using System;
using System.Management;
using System.Runtime.InteropServices;
using Microsoft.ConfigurationManagement.AdminConsole.DialogFramework;
using System.Threading;

namespace ConfigMgr.QuickTools.Device
{
    public static class ProvisioningMode
    {
        public static void RunSetProvisioningMode(object sender, ScopeNode scopeNode, ActionDescription action, IResultObject selectedResultObjects, PropertyDataUpdated dataUpdatedDelegate, Status status)
        {
            ProcessProvisioningMode(scopeNode, action, selectedResultObjects, true);
        }

        public static void RunGetProvisioningMode(object sender, ScopeNode scopeNode, ActionDescription action, IResultObject selectedResultObjects, PropertyDataUpdated dataUpdatedDelegate, Status status)
        {
            ProcessProvisioningMode(scopeNode, action, selectedResultObjects, false);
        }

        public static void ProcessProvisioningMode(IResultObject resultObject, bool mode)
        {
            try
            {
                ManagementClass clientaction = new ManagementClass(string.Format(@"\\{0}\root\{1}:{2}", resultObject["Name"].StringValue, "ccm", "SMS_Client"));
                object[] methodArgs = { "False" };
                clientaction.InvokeMethod("SetClientProvisioningMode", methodArgs);
                clientaction.Dispose();
            }
            catch (ManagementException ex)
            {
                ExceptionUtilities.TraceException(ex);
                SccmExceptionDialog.ShowDialog(SnapIn.Console, ex, "An error occured while invoking WMI method.");
            }
            catch (COMException ex)
            {
                ExceptionUtilities.TraceException(ex);
                SccmExceptionDialog.ShowDialog(SnapIn.Console, ex, "An error occured while connecting to host.");
            }
            catch (Exception ex)
            {
                ExceptionUtilities.TraceException(ex);
                SccmExceptionDialog.ShowDialog(SnapIn.Console, ex);
            }
        }

        private static void ProcessProvisioningMode(ScopeNode scopeNode, ActionDescription action, IResultObject selectedResultObjects, bool mode)
        {
            if (selectedResultObjects.ObjectClass == "SMS_Collection")
            {
                ConnectionManagerBase connectionManagerInstance = (scopeNode as ConsoleParentNode).RootConnectionNode.GetConnectionManagerInstance("WQL");

                try
                {
                    string query = string.Format("SELECT * FROM SMS_FullCollectionMembership WHERE CollectionID='{0}'", selectedResultObjects["CollectionID"].StringValue);
                    using (IResultObject resultObject = connectionManagerInstance.QueryProcessor.ExecuteQuery(query))
                    {
                        using (ProvisioningModeDialog provisioningMode = new ProvisioningModeDialog(resultObject, action, mode))
                        {
                            provisioningMode.ShowDialog(SnapIn.Console);
                            return;
                        }
                    }
                }
                catch (SmsQueryException ex)
                {
                    ExceptionUtilities.TraceException(ex);
                    SccmExceptionDialog.ShowDialog(SnapIn.Console, ex);
                    return;
                }
            }

            if (selectedResultObjects.Count == 1)
            {
                foreach (IResultObject resultObject in selectedResultObjects)
                {
                    ThreadPool.QueueUserWorkItem(arg => { ProcessProvisioningMode(resultObject, mode); });
                }
            }
            else
            {
                using (ProvisioningModeDialog provisioningMode = new ProvisioningModeDialog(selectedResultObjects, action, mode))
                {
                    provisioningMode.ShowDialog(SnapIn.Console);
                    return;
                }
            }
        }
    }
}
