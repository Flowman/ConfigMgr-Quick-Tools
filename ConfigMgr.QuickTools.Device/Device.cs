using Microsoft.ConfigurationManagement.AdminConsole;
using Microsoft.ConfigurationManagement.ManagementProvider;
using Microsoft.ConfigurationManagement.AdminConsole.Schema;
using Microsoft.ConfigurationManagement.AdminConsole.CollectionProperty.DeployWizard;
using Microsoft.ConfigurationManagement.AdminConsole.Common;
using Microsoft.ConfigurationManagement.AdminConsole.DialogFramework;
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
                using (BrowseCollectionDialog collectionDialog = new BrowseCollectionDialog(selectedObject.ConnectionManager))
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
                                if (resultObject.ObjectClass == "SMS_Collection" && resultObject["CollectionID"].StringValue == collectionResultObject["CollectionID"].StringValue)
                                    return false;
                            }
                            return true;
                        }
                        return !(selectedObject.ObjectClass == "SMS_Collection") || !(selectedObject["CollectionID"].StringValue == collectionResultObject["CollectionID"].StringValue);
                    });

                    if (collectionDialog.ShowDialog() != DialogResult.OK)
                        return;

                    foreach (IResultObject collection in collectionDialog.SelectedCollections)
                    {
                        List<IResultObject> list = new List<IResultObject>();
                        foreach (IResultObject item in selectedObject)
                        {
                            IResultObject instance = selectedObject.ConnectionManager.CreateEmbeddedObjectInstance("SMS_CollectionRuleDirect");
                            instance["ResourceClassName"].StringValue = "SMS_R_System";
                            instance["RuleName"].StringValue = item["Name"].StringValue;
                            instance["ResourceID"].IntegerValue = item["ResourceID"].IntegerValue;
                            list.Add(instance);
                        }
                        collection.ExecuteMethod("AddMembershipRules", new Dictionary<string, object>() { { "collectionRules", list } });
                    }
                }
            }
            catch (SmsQueryException ex)
            {
                SccmExceptionDialog.ShowDialog(SnapIn.Console, ex);
            }
        }

        public static void DeleteDeploymentForMonitoring(object sender, ScopeNode scopeNode, ActionDescription action, IResultObject selectedResultObject, PropertyDataUpdated dataUpdatedDelegate, Status status)
        {
            DeleteDeployment(sender, selectedResultObject, dataUpdatedDelegate, "SoftwareName");
        }

        private static void DeleteDeployment(object sender, IResultObject selectedResultObject, PropertyDataUpdated dataUpdatedDelegate, string displayPropertyName, bool confirmDialog = true)
        {
            IResultObject deploymentFromSummarizer = Utilities.GetCorrespondingDeploymentFromSummarizer(selectedResultObject);

            if (confirmDialog)
            {
                if (System.Windows.MessageBox.Show(string.Format("Delete the selected '{0}' deployment?", ResourceDisplayClass.GetAliasDisplayText(selectedResultObject, displayPropertyName)), "Configuration Manager", MessageBoxButton.YesNo, MessageBoxImage.Exclamation) != MessageBoxResult.Yes)
                    return;
            }
            deploymentFromSummarizer.Delete();

            List<PropertyDataUpdateItem> refreshDataList = new List<PropertyDataUpdateItem>
            {
                new PropertyDataUpdateItem(selectedResultObject, PropertyDataUpdateAction.Delete)
            };

            if (dataUpdatedDelegate == null)
                return;

            dataUpdatedDelegate(sender, refreshDataList);
        }
    }
}
