using Microsoft.ConfigurationManagement.AdminConsole;
using Microsoft.ConfigurationManagement.ManagementProvider;
using Microsoft.ConfigurationManagement.AdminConsole.Schema;
using Microsoft.ConfigurationManagement.AdminConsole.CollectionProperty.DeployWizard;
using Microsoft.ConfigurationManagement.AdminConsole.Common;
using Microsoft.ConfigurationManagement.AdminConsole.DialogFramework;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Forms;

namespace ConfigMgr.QuickTools.Device
{
    public static class Device
    {
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
                        resultObject1.ExecuteMethod("AddMembershipRules", new Dictionary<string, object>() { { "collectionRules", list } });
                    }
                }
            }
            catch (SmsQueryException ex)
            {
                ExceptionUtilities.TraceException(ex);
                SccmExceptionDialog.ShowDialog(SnapIn.Console, ex);
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
            _ = dataUpdatedDelegate(sender, refreshDataList) ? 1 : 0;
        }
    }
}
