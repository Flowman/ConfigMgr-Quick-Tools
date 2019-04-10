using Microsoft.ConfigurationManagement.AdminConsole;
using Microsoft.ConfigurationManagement.ManagementProvider;
using Microsoft.ConfigurationManagement.AdminConsole.Schema;
using Microsoft.ConfigurationManagement.AdminConsole.DialogFramework;
using System;
using System.Management;
using System.Runtime.InteropServices;
using System.Threading;
using System.ServiceProcess;

namespace ConfigMgr.QuickTools.Device
{
    public static class ClientActions
    {
        public static void RunClientActionMachinePolicy(object sender, ScopeNode scopeNode, ActionDescription action, IResultObject selectedResultObjects, PropertyDataUpdated dataUpdatedDelegate, Status status)
        {
            ProcessClientAction(scopeNode, action, selectedResultObjects, "{00000000-0000-0000-0000-000000000021}");
        }

        public static void RunClientActionApplicationDeployment(object sender, ScopeNode scopeNode, ActionDescription action, IResultObject selectedResultObjects, PropertyDataUpdated dataUpdatedDelegate, Status status)
        {
            ProcessClientAction(scopeNode, action, selectedResultObjects, "{00000000-0000-0000-0000-000000000121}");
        }

        public static void RunClientActionDataCollection(object sender, ScopeNode scopeNode, ActionDescription action, IResultObject selectedResultObjects, PropertyDataUpdated dataUpdatedDelegate, Status status)
        {
            ProcessClientAction(scopeNode, action, selectedResultObjects, "{00000000-0000-0000-0000-000000000003}");
        }

        public static void RunClientActionFileCollection(object sender, ScopeNode scopeNode, ActionDescription action, IResultObject selectedResultObjects, PropertyDataUpdated dataUpdatedDelegate, Status status)
        {
            ProcessClientAction(scopeNode, action, selectedResultObjects, "{00000000-0000-0000-0000-000000000010}");
        }

        public static void RunClientActionHardwareInventory(object sender, ScopeNode scopeNode, ActionDescription action, IResultObject selectedResultObjects, PropertyDataUpdated dataUpdatedDelegate, Status status)
        {
            ProcessClientAction(scopeNode, action, selectedResultObjects, "{00000000-0000-0000-0000-000000000001}");
        }

        public static void RunClientActionFullHardwareInventory(object sender, ScopeNode scopeNode, ActionDescription action, IResultObject selectedResultObjects, PropertyDataUpdated dataUpdatedDelegate, Status status)
        {
            ProcessClientAction(scopeNode, action, selectedResultObjects, "{00000000-0000-0000-0000-000000000001}", true);
        }

        public static void RunClientActionSoftwareInventory(object sender, ScopeNode scopeNode, ActionDescription action, IResultObject selectedResultObjects, PropertyDataUpdated dataUpdatedDelegate, Status status)
        {
            ProcessClientAction(scopeNode, action, selectedResultObjects, "{00000000-0000-0000-0000-000000000002}");
        }

        public static void RunClientActionSoftwareMetering(object sender, ScopeNode scopeNode, ActionDescription action, IResultObject selectedResultObjects, PropertyDataUpdated dataUpdatedDelegate, Status status)
        {
            ProcessClientAction(scopeNode, action, selectedResultObjects, "{00000000-0000-0000-0000-000000000031}");
        }

        public static void RunClientActionSoftwareUpdateDeployment(object sender, ScopeNode scopeNode, ActionDescription action, IResultObject selectedResultObjects, PropertyDataUpdated dataUpdatedDelegate, Status status)
        {
            ProcessClientAction(scopeNode, action, selectedResultObjects, "{00000000-0000-0000-0000-000000000108}");
        }

        public static void RunClientActionSoftwareUpdateScan(object sender, ScopeNode scopeNode, ActionDescription action, IResultObject selectedResultObjects, PropertyDataUpdated dataUpdatedDelegate, Status status)
        {
            ProcessClientAction(scopeNode, action, selectedResultObjects, "{00000000-0000-0000-0000-000000000113}");
        }

        public static void RunClientActionUserPolicy(object sender, ScopeNode scopeNode, ActionDescription action, IResultObject selectedResultObjects, PropertyDataUpdated dataUpdatedDelegate, Status status)
        {
            ProcessClientAction(scopeNode, action, selectedResultObjects, "{00000000-0000-0000-0000-000000000026}");
        }

        public static void RunClientActionWindowsInstaller(object sender, ScopeNode scopeNode, ActionDescription action, IResultObject selectedResultObjects, PropertyDataUpdated dataUpdatedDelegate, Status status)
        {
            ProcessClientAction(scopeNode, action, selectedResultObjects, "{00000000-0000-0000-0000-000000000032}");
        }

        public static void ClientAction(IResultObject resultObject, string scheduleId, bool fullScan)
        {       
            try
            {
                if (fullScan)
                {
                    ManagementScope inventoryAgentScope = new ManagementScope(string.Format(@"\\{0}\root\{1}", resultObject["Name"].StringValue, "ccm\\InvAgt"));
                    ManagementClass inventoryClass = new ManagementClass(inventoryAgentScope.Path.Path, "InventoryActionStatus", null);

                    // Query the class for the InventoryActionID object (create query, create searcher object, execute query).
                    ObjectQuery query = new ObjectQuery(string.Format("SELECT * FROM InventoryActionStatus WHERE InventoryActionID = '{0}'", scheduleId));
                    using (ManagementObjectSearcher searcher = new ManagementObjectSearcher(inventoryAgentScope, query))
                    {
                        ManagementObjectCollection queryResults = searcher.Get();

                        // Enumerate the collection to get to the result (there should only be one item returned from the query).
                        foreach (ManagementObject result in queryResults)
                        {
                            // Display message and delete the object.
                            result.Delete();
                        }
                    }
                }
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
            //catch (UnauthorizedAccessException ex)
            //{
            //}
            catch (Exception ex)
            {
                ExceptionUtilities.TraceException(ex);
                SccmExceptionDialog.ShowDialog(SnapIn.Console, ex);
            }
        }

        private static void ProcessClientAction(ScopeNode scopeNode, ActionDescription action, IResultObject selectedResultObjects, string schedulerId)
        {
            ProcessClientAction(scopeNode, action, selectedResultObjects, schedulerId, false);
        }

        private static void ProcessClientAction(ScopeNode scopeNode, ActionDescription action, IResultObject selectedResultObjects, string schedulerId, bool full)
        {
            if (selectedResultObjects.ObjectClass == "SMS_Collection")
            {
                ConnectionManagerBase connectionManagerInstance = (scopeNode as ConsoleParentNode).RootConnectionNode.GetConnectionManagerInstance("WQL");

                try
                {
                    string query = string.Format("SELECT * FROM SMS_FullCollectionMembership WHERE CollectionID='{0}'", selectedResultObjects["CollectionID"].StringValue);
                    using (IResultObject resultObject = connectionManagerInstance.QueryProcessor.ExecuteQuery(query))
                    {
                        using (ClientActionsDialog clientActions = new ClientActionsDialog(resultObject, schedulerId, action, full))
                        {
                            clientActions.ShowDialog(SnapIn.Console);
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
                    ThreadPool.QueueUserWorkItem(arg => { ClientAction(resultObject, schedulerId, full); });
                }
            }
            else
            {
                using (ClientActionsDialog clientActions = new ClientActionsDialog(selectedResultObjects, schedulerId, action, full))
                {
                    clientActions.ShowDialog(SnapIn.Console);
                    return;
                }
            }
        }

        public static void RunClientActionRestartService(object sender, ScopeNode scopeNode, ActionDescription action, IResultObject selectedResultObjects, PropertyDataUpdated dataUpdatedDelegate, Status status)
        {
            ThreadPool.QueueUserWorkItem(arg => { RestartService(selectedResultObjects); });
        }

        private static void RestartService(IResultObject resultObject)
        {
            try
            {
                using (ServiceController service = new ServiceController("SMS Agent Host", resultObject["Name"].StringValue))
                {
                    if (service.Status == ServiceControllerStatus.Running)
                    {
                        service.Stop();
                        service.WaitForStatus(ServiceControllerStatus.Stopped, TimeSpan.FromSeconds(20));

                        service.Start();
                    }
                }
            }
            catch (System.ServiceProcess.TimeoutException ex)
            {
                ExceptionUtilities.TraceException(ex);
                SccmExceptionDialog.ShowDialog(SnapIn.Console, ex);
            }
            catch (Exception ex)
            {
                ExceptionUtilities.TraceException(ex);
                SccmExceptionDialog.ShowDialog(SnapIn.Console, ex);
            }
        }
    }
}
