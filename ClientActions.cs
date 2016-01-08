using Microsoft.ConfigurationManagement.AdminConsole;
using Microsoft.ConfigurationManagement.ManagementProvider;
using Microsoft.ConfigurationManagement.AdminConsole.Schema;
using System;
using System.Management;
using System.Runtime.InteropServices;
using Microsoft.ConfigurationManagement.AdminConsole.DialogFramework;
using System.Threading;

namespace Zetta.ConfigMgr.QuickTools
{
    class ClientActions
    {
        public static void RunClientActionMachinePolicy(object sender, ScopeNode scopeNode, ActionDescription action, IResultObject selectedResultObjects, PropertyDataUpdated dataUpdatedDelegate, Status status)
        {
            processClientAction(scopeNode, action, selectedResultObjects, "{00000000-0000-0000-0000-000000000021}");
        }

        public static void RunClientActionApplicationDeployment(object sender, ScopeNode scopeNode, ActionDescription action, IResultObject selectedResultObjects, PropertyDataUpdated dataUpdatedDelegate, Status status)
        {
            processClientAction(scopeNode, action, selectedResultObjects, "{00000000-0000-0000-0000-000000000121}");
        }

        public static void RunClientActionDataCollection(object sender, ScopeNode scopeNode, ActionDescription action, IResultObject selectedResultObjects, PropertyDataUpdated dataUpdatedDelegate, Status status)
        {
            processClientAction(scopeNode, action, selectedResultObjects, "{00000000-0000-0000-0000-000000000003}");
        }

        public static void RunClientActionFileCollection(object sender, ScopeNode scopeNode, ActionDescription action, IResultObject selectedResultObjects, PropertyDataUpdated dataUpdatedDelegate, Status status)
        {
            processClientAction(scopeNode, action, selectedResultObjects, "{00000000-0000-0000-0000-000000000010}");
        }

        public static void RunClientActionHardwareInventory(object sender, ScopeNode scopeNode, ActionDescription action, IResultObject selectedResultObjects, PropertyDataUpdated dataUpdatedDelegate, Status status)
        {
            processClientAction(scopeNode, action, selectedResultObjects, "{00000000-0000-0000-0000-000000000001}");
        }

        public static void RunClientActionSoftwareInventory(object sender, ScopeNode scopeNode, ActionDescription action, IResultObject selectedResultObjects, PropertyDataUpdated dataUpdatedDelegate, Status status)
        {
            processClientAction(scopeNode, action, selectedResultObjects, "{00000000-0000-0000-0000-000000000002}");
        }

        public static void RunClientActionSoftwareMetering(object sender, ScopeNode scopeNode, ActionDescription action, IResultObject selectedResultObjects, PropertyDataUpdated dataUpdatedDelegate, Status status)
        {
            processClientAction(scopeNode, action, selectedResultObjects, "{00000000-0000-0000-0000-000000000031}");
        }

        public static void RunClientActionSoftwareUpdateDeployment(object sender, ScopeNode scopeNode, ActionDescription action, IResultObject selectedResultObjects, PropertyDataUpdated dataUpdatedDelegate, Status status)
        {
            processClientAction(scopeNode, action, selectedResultObjects, "{00000000-0000-0000-0000-000000000108}");
        }

        public static void RunClientActionSoftwareUpdateScan(object sender, ScopeNode scopeNode, ActionDescription action, IResultObject selectedResultObjects, PropertyDataUpdated dataUpdatedDelegate, Status status)
        {
            processClientAction(scopeNode, action, selectedResultObjects, "{00000000-0000-0000-0000-000000000113}");
        }

        public static void RunClientActionUserPolicy(object sender, ScopeNode scopeNode, ActionDescription action, IResultObject selectedResultObjects, PropertyDataUpdated dataUpdatedDelegate, Status status)
        {
            processClientAction(scopeNode, action, selectedResultObjects, "{00000000-0000-0000-0000-000000000027}");
        }

        public static void RunClientActionWindowsInstaller(object sender, ScopeNode scopeNode, ActionDescription action, IResultObject selectedResultObjects, PropertyDataUpdated dataUpdatedDelegate, Status status)
        {
            processClientAction(scopeNode, action, selectedResultObjects, "{00000000-0000-0000-0000-000000000032}");
        }

        public static void ClientAction(IResultObject resultObject, string scheduleId)
        {       
            try
            {
                ManagementClass clientaction = new ManagementClass(string.Format(@"\\{0}\root\{1}:{2}", resultObject["Name"].StringValue, "ccm", "SMS_Client"));
                object[] methodArgs = { scheduleId };
                clientaction.InvokeMethod("TriggerSchedule", methodArgs);
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

        private static void processClientAction(ScopeNode scopeNode, ActionDescription action, IResultObject selectedResultObjects, string schedulerId)
        {
            if (selectedResultObjects.ObjectClass == "SMS_Collection")
            {
                ConnectionManagerBase connectionManagerInstance = (scopeNode as ConsoleParentNode).RootConnectionNode.GetConnectionManagerInstance("WQL");

                string query = string.Format("SELECT * FROM SMS_FullCollectionMembership WHERE CollectionID='{0}'", selectedResultObjects["CollectionID"].StringValue);
                try
                {
                    IResultObject resultObject = connectionManagerInstance.QueryProcessor.ExecuteQuery(query);

                    using (ClientActionsDialog clientActions = new ClientActionsDialog(resultObject, schedulerId, action))
                    {
                        int num2 = (int)clientActions.ShowDialog(SnapIn.Console);
                        return;
                    }
                }
                catch (SmsQueryException ex)
                {
                    ExceptionUtilities.TraceException((Exception)ex);
                    SccmExceptionDialog.ShowDialog(SnapIn.Console, ex);
                    return;
                }
            }

            if (selectedResultObjects.Count == 1)
            {
                foreach (IResultObject resultObject in selectedResultObjects)
                {
                    ThreadPool.QueueUserWorkItem(arg => { ClientAction(resultObject, schedulerId); });
                }
            }
            else
            {
                using (ClientActionsDialog clientActions = new ClientActionsDialog(selectedResultObjects, schedulerId, action))
                {
                    int num2 = (int)clientActions.ShowDialog(SnapIn.Console);
                }
            }
        }
    }
}
