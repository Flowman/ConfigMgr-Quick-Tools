using Microsoft.ConfigurationManagement.AdminConsole;
using Microsoft.ConfigurationManagement.ManagementProvider;
using Microsoft.ConfigurationManagement.AdminConsole.Schema;
using Microsoft.ConfigurationManagement.AdminConsole.CollectionProperty.DeployWizard;
using System;
using System.Management;
using System.Runtime.InteropServices;
using Microsoft.ConfigurationManagement.AdminConsole.DialogFramework;
using System.Threading;
using Microsoft.ConfigurationManagement.AdminConsole.Common;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Forms;

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

        public static void RunClientActionFullHardwareInventory(object sender, ScopeNode scopeNode, ActionDescription action, IResultObject selectedResultObjects, PropertyDataUpdated dataUpdatedDelegate, Status status)
        {
            processClientAction(scopeNode, action, selectedResultObjects, "{00000000-0000-0000-0000-000000000001}", true);
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
            processClientAction(scopeNode, action, selectedResultObjects, "{00000000-0000-0000-0000-000000000026}");
        }

        public static void RunClientActionWindowsInstaller(object sender, ScopeNode scopeNode, ActionDescription action, IResultObject selectedResultObjects, PropertyDataUpdated dataUpdatedDelegate, Status status)
        {
            processClientAction(scopeNode, action, selectedResultObjects, "{00000000-0000-0000-0000-000000000032}");
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
                    ManagementObjectSearcher searcher = new ManagementObjectSearcher(inventoryAgentScope, query);
                    ManagementObjectCollection queryResults = searcher.Get();

                    // Enumerate the collection to get to the result (there should only be one item returned from the query).
                    foreach (ManagementObject result in queryResults)
                    {
                        // Display message and delete the object.
                        result.Delete();
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
            catch (Exception ex)
            {
                ExceptionUtilities.TraceException(ex);
                SccmExceptionDialog.ShowDialog(SnapIn.Console, ex);
            }
        }

        private static void processClientAction(ScopeNode scopeNode, ActionDescription action, IResultObject selectedResultObjects, string schedulerId)
        {
            processClientAction(scopeNode, action, selectedResultObjects, schedulerId, false);
        }

        private static void processClientAction(ScopeNode scopeNode, ActionDescription action, IResultObject selectedResultObjects, string schedulerId, bool full)
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
                            int num2 = (int)clientActions.ShowDialog(SnapIn.Console);
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
                    int num2 = (int)clientActions.ShowDialog(SnapIn.Console);
                }
            }
        }

        public static void AddMulitDeviceCollection(object sender, ScopeNode scopeNode, ActionDescription action, IResultObject selectedObject, PropertyDataUpdated dataUpdatedDelegate, Status Status)
        {
            try
            {
                ConnectionManagerBase connectionManager = Microsoft.ConfigurationManagement.AdminConsole.UtilityClass.ConnectionManagerFromScope(scopeNode, "WQL");
                using (BrowseCollectionDialog collectionDialog = new BrowseCollectionDialog(connectionManager))
                {
                    collectionDialog.MultiSelect = true;
                    collectionDialog.CollectionType = CollectionType.Device;
                    collectionDialog.CollectionFilter = (collectionResultObject =>
                    {
                        if (collectionResultObject["IsReferenceCollection"].BooleanValue || collectionResultObject["IsBuiltIn"].BooleanValue)
                            return false;
                        if (selectedObject.Count > 1)
                        {
                            foreach (IResultObject resultObject in selectedObject)
                            {
                                if (string.Equals(resultObject.ObjectClass, "SMS_Collection", StringComparison.OrdinalIgnoreCase) && string.Equals(resultObject["CollectionID"].StringValue, collectionResultObject["CollectionID"].StringValue, StringComparison.OrdinalIgnoreCase))
                                    return false;
                            }
                            return true;
                        }
                        return !string.Equals(selectedObject.ObjectClass, "SMS_Collection", StringComparison.OrdinalIgnoreCase) || !string.Equals(selectedObject["CollectionID"].StringValue, collectionResultObject["CollectionID"].StringValue, StringComparison.OrdinalIgnoreCase);
                    });
                    if (collectionDialog.ShowDialog() != DialogResult.OK)
                        return;
                    foreach (IResultObject resultObject1 in collectionDialog.SelectedCollections)
                    {
                        List<IResultObject> list = new List<IResultObject>();
                        foreach (IResultObject resultObject in selectedObject)
                        {
                            IResultObject embeddedObjectInstance = connectionManager.CreateEmbeddedObjectInstance("SMS_CollectionRuleDirect");
                            embeddedObjectInstance["ResourceClassName"].StringValue = "SMS_R_System";
                            embeddedObjectInstance["RuleName"].StringValue = resultObject["Name"].StringValue;
                            embeddedObjectInstance["ResourceID"].IntegerValue = resultObject["ResourceID"].IntegerValue;
                            list.Add(embeddedObjectInstance);
                        }
                        resultObject1.ExecuteMethod("AddMembershipRules", new Dictionary<string, object>() {{"collectionRules", list} });
                    }
                }
            }
            catch (SmsQueryException ex)
            {
                ExceptionUtilities.TraceException(ex);
                SccmExceptionDialog.ShowDialog(SnapIn.Console, ex);
            }
        }

        public static void LAPSPassword(object sender, ScopeNode scopeNode, ActionDescription action, IResultObject selectedObject, PropertyDataUpdated dataUpdatedDelegate, Status Status)
        {
            using (LAPSDialog dialog = new LAPSDialog(selectedObject, action))
            {
                int num2 = (int)dialog.ShowDialog(SnapIn.Console);
                return;
            }
        }

        public static void DeleteDeploymentForMonitoring(object sender, ScopeNode scopeNode, ActionDescription action, IResultObject selectedResultObject, PropertyDataUpdated dataUpdatedDelegate, Status status)
        {
            DeleteDeployment(sender, scopeNode, action, selectedResultObject, dataUpdatedDelegate, status, "SoftwareName", true);
        }

        private static void DeleteDeployment(object sender, ScopeNode scopeNode, ActionDescription action, IResultObject selectedResultObject, PropertyDataUpdated dataUpdatedDelegate, Status status, string displayPropertyName, bool confirmDialog = true)
        {
            IResultObject deploymentFromSummarizer = Utilities.GetCorrespondingDeploymentFromSummarizer(selectedResultObject);
            if (confirmDialog)
            {
                if (System.Windows.MessageBox.Show(string.Format("Delete the selected '{0}' deployment?", ResourceDisplayClass.GetAliasDisplayText(selectedResultObject, displayPropertyName)), "Configuration Manager", MessageBoxButton.YesNo, MessageBoxImage.Exclamation) != MessageBoxResult.Yes)
                    return;
            }
            deploymentFromSummarizer.Delete();
            List<PropertyDataUpdateItem> refreshDataList = new List<PropertyDataUpdateItem>();
            refreshDataList.Add(new PropertyDataUpdateItem(selectedResultObject, PropertyDataUpdateAction.Delete));
            if (dataUpdatedDelegate == null)
                return;
            int num = dataUpdatedDelegate(sender, refreshDataList) ? 1 : 0;
        }

        public IEnumerable<IResultObject> GetCollectionMembership(ConnectionManagerBase ConnectionManager, int resourceID)
        {
            string query = string.Format("SELECT SMS_Collection.* FROM SMS_FullCollectionMembership, SMS_Collection where ResourceID = '{0}' and SMS_FullCollectionMembership.CollectionID = SMS_Collection.CollectionID", resourceID);
            using (IResultObject resultObject = ConnectionManager.QueryProcessor.ExecuteQuery(query))
            {
                foreach (IResultObject resultObject1 in resultObject)
                {
                    yield return resultObject1;
                }
            }
        }
    }
}
