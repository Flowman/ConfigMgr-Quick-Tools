using Microsoft.ConfigurationManagement.AdminConsole;
using Microsoft.ConfigurationManagement.ManagementProvider;
using Microsoft.ConfigurationManagement.AdminConsole.Schema;
using Microsoft.ConfigurationManagement.AdminConsole.DialogFramework;
using System;
using System.Reflection;
using System.Windows;

namespace ConfigMgr.QuickTools.Device
{
    public static class ClientCache
    {
        //private static DeviceProgressDialog deviceProgressDialog;

        public static void ChangeClientCache(object sender, ScopeNode scopeNode, ActionDescription action, IResultObject selectedResultObjects, PropertyDataUpdated dataUpdatedDelegate, Status status)
        {
            bool showProgressDialog = false;
            if (selectedResultObjects.ObjectClass == "SMS_Collection" || selectedResultObjects.Count > 1)
            {
                showProgressDialog = true;
            }

            int total = selectedResultObjects.Count;

            if (selectedResultObjects.ObjectClass == "SMS_Collection")
            {
                try
                {
                    total = 0;
                    string query = string.Format("SELECT * FROM SMS_FullCollectionMembership WHERE CollectionID='{0}'", selectedResultObjects["CollectionID"].StringValue);
                    // query processor does not have count implemented?!?!?
                    foreach (IResultObject tmp in selectedResultObjects.ConnectionManager.QueryProcessor.ExecuteQuery(query))
                    {
                        total++;
                    }

                    selectedResultObjects = selectedResultObjects.ConnectionManager.QueryProcessor.ExecuteQuery(query);
                }
                catch (SmsQueryException ex)
                {
                    SccmExceptionDialog.ShowDialog(SnapIn.Console, ex);
                    return;
                }
            }

            if (showProgressDialog)
            {
                MessageBox.Show("Not implemented yet!", "Configuration Manager", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);
                //Type type = typeof(ClientCache);
                //MethodInfo methodInfo = type.GetMethod("ChangeClientCacheSize");

                //deviceProgressDialog = new DeviceProgressDialog(action, selectedResultObjects, methodInfo)
                //{
                //    Total = total
                //};
                //deviceProgressDialog.ShowDialog();
            }
            else
            {
                Utility.ShowDialog("QuickToolsDeviceCache", selectedResultObjects);
            }

        }

        public static void ChangeClientCacheSize(IResultObject resultObject)
        {
            try
            {

            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
