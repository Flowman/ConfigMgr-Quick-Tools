using Microsoft.ConfigurationManagement.AdminConsole;
using Microsoft.ConfigurationManagement.ManagementProvider;
using Microsoft.ConfigurationManagement.AdminConsole.Schema;
using Microsoft.ConfigurationManagement.AdminConsole.DialogFramework;
using System;
using System.Management;
using System.Threading;
using System.ServiceProcess;
using System.Reflection;
using System.Threading.Tasks;

namespace ConfigMgr.QuickTools.Device
{
    public static class ClientActions
    {
        private static DeviceProgressDialog deviceProgressDialog;
        private static string scheduleId;
        private static bool fullScan = false;
        private static string method;

        public static void RunClientAction(object sender, ScopeNode scopeNode, ActionDescription action, IResultObject selectedResultObjects, PropertyDataUpdated dataUpdatedDelegate, Status status)
        {
            // set the thread pool method to run
            method = "ClientAction";
            // check if we need full scan
            if (!string.IsNullOrEmpty(action.DialogId))
            {
                string[] tmp = action.DialogId.Split(',');
                scheduleId = tmp[0];
                fullScan = tmp.Length > 1 ? true : false;
            }

            ProcessAction(scopeNode, action, selectedResultObjects);
        }

        public static void RunClientActionRestartService(object sender, ScopeNode scopeNode, ActionDescription action, IResultObject selectedResultObjects, PropertyDataUpdated dataUpdatedDelegate, Status status)
        {
            // set the thread pool method to run
            method = "RestartServiceAsync";
            ProcessAction(scopeNode, action, selectedResultObjects);
        }

        private static void ProcessAction(ScopeNode scopeNode, ActionDescription action, IResultObject resultObjects)
        {
            bool showProgressDialog = false;
            if (resultObjects.ObjectClass == "SMS_Collection" || resultObjects.Count > 1)
            {
                showProgressDialog = true;
            }

            int total = resultObjects.Count;

            if (resultObjects.ObjectClass == "SMS_Collection")
            {
                try
                {
                    total = 0;
                    string query = string.Format("SELECT * FROM SMS_FullCollectionMembership WHERE CollectionID='{0}'", resultObjects["CollectionID"].StringValue);
                    // query processor does not have count implemented?!?!?
                    foreach (IResultObject tmp in resultObjects.ConnectionManager.QueryProcessor.ExecuteQuery(query))
                    {
                        total++;
                    }

                    resultObjects = resultObjects.ConnectionManager.QueryProcessor.ExecuteQuery(query);
                }
                catch (SmsQueryException ex)
                {
                    SccmExceptionDialog.ShowDialog(SnapIn.Console, ex);
                    return;
                }
            }

            if (showProgressDialog)
            {
                Type type = typeof(ClientActions);
                MethodInfo methodInfo = type.GetMethod(method);

                deviceProgressDialog = new DeviceProgressDialog(action, resultObjects, methodInfo)
                {
                    Total = total
                };
                deviceProgressDialog.ShowDialog();
            }
            else
            {
                foreach (IResultObject resultObject in resultObjects)
                {
                    ThreadPool.QueueUserWorkItem(arg => { RunAction(resultObject); });
                }
            }

            return;
        }

        private static void RunAction(IResultObject resultObject)
        {
            try
            {
                Type type = typeof(ClientActions);
                MethodInfo methodInfo = type.GetMethod(method);

                methodInfo.Invoke(null, new object[] { resultObject });
            }
            catch (TargetInvocationException ex)
            {
                SccmExceptionDialog.ShowDialog(SnapIn.Console, ex.InnerException);
            }
        }

        public static void ClientAction(IResultObject resultObject)
        {
            try
            {
                if (fullScan)
                {
                    ManagementScope scope = Utility.GetWMIScope(resultObject["Name"].StringValue, @"ccm\InvAgt");
                    ManagementObject result = Utility.GetFirstWMIInstance(scope, string.Format("SELECT * FROM InventoryActionStatus WHERE InventoryActionID = '{0}'", scheduleId));
                    if (result != null)
                        result.Delete();
                }

                ObjectGetOptions o = new ObjectGetOptions(null, TimeSpan.FromSeconds(5), true);
                using (ManagementClass clientaction = new ManagementClass(string.Format(@"\\{0}\root\ccm:SMS_Client", resultObject["Name"].StringValue), o))
                {
                    object[] methodArgs = { scheduleId };
                    clientaction.InvokeMethod("TriggerSchedule", methodArgs);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static async Task RestartServiceAsync(IResultObject resultObject)
        {
            using (ServiceController service = new ServiceController("SMS Agent Host", resultObject["Name"].StringValue))
            {
                if (service.Status == ServiceControllerStatus.Running)
                {
                    service.Stop();

                    await service.WaitForStatusAsync(ServiceControllerStatus.Stopped, TimeSpan.FromSeconds(20), deviceProgressDialog.CancellationTokenSource.Token);

                    service.Start();
                }
                else if (service.Status == ServiceControllerStatus.Stopped)
                {
                    service.Start();
                }
            }
        }
    }

    public static class ServiceControllerExtensions
    {
        public static async Task WaitForStatusAsync(this ServiceController controller, ServiceControllerStatus desiredStatus, TimeSpan timeout, CancellationToken cancellationToken)
        {
            var utcNow = DateTime.UtcNow;
            controller.Refresh();
            while (controller.Status != desiredStatus)
            {
                if (DateTime.UtcNow - utcNow > timeout)
                {
                    throw new System.TimeoutException($"Failed to wait for '{controller.ServiceName}' to change status to '{desiredStatus}'.");
                }
                await Task.Delay(250, cancellationToken).ConfigureAwait(false);
                controller.Refresh();
            }
        }
    }
}
