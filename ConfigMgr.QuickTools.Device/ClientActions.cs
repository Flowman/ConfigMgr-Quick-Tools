using Microsoft.ConfigurationManagement.AdminConsole;
using Microsoft.ConfigurationManagement.ManagementProvider;
using Microsoft.ConfigurationManagement.AdminConsole.Schema;
using Microsoft.ConfigurationManagement.AdminConsole.DialogFramework;
using System;
using System.Management;
using System.Runtime.InteropServices;
using System.Threading;
using System.ServiceProcess;
using System.ComponentModel;
using System.Windows.Forms;
using System.Reflection;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ConfigMgr.QuickTools.Device
{
    public static class ClientActions
    {
        private static BackgroundWorker progressWorker;
        private static IResultObject resultObjects;
        private static DeviceProgressDialog deviceProgressDialog;
        private static bool showProgressDialog = false;
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

        private static void ProcessAction(ScopeNode scopeNode, ActionDescription action, IResultObject selectedResultObjects)
        {
            resultObjects = selectedResultObjects;

            deviceProgressDialog = new DeviceProgressDialog(action);
            // show dialog if more then one object is selected
            if (resultObjects.ObjectClass == "SMS_Collection" || resultObjects.Count > 1)
            {
                showProgressDialog = true;
            }
            // run the background work so we can update the progress dialog
            progressWorker = new BackgroundWorker();
            progressWorker.DoWork += new DoWorkEventHandler(ClientAction_DoWork);
            progressWorker.WorkerReportsProgress = false;
            progressWorker.RunWorkerAsync();
            // show the dialog if required
            if (showProgressDialog)
                deviceProgressDialog.ShowDialog();

            return;
        }

        private static void ClientAction_DoWork(object sender, DoWorkEventArgs e)
        {
            List<IResultObject> result = new List<IResultObject>();
            if (resultObjects.ObjectClass == "SMS_Collection")
            {
                try
                {
                    string query = string.Format("SELECT * FROM SMS_FullCollectionMembership WHERE CollectionID='{0}'", resultObjects["CollectionID"].StringValue);
                    result = Utility.SearchWMIToList(resultObjects.ConnectionManager, query);
                    resultObjects = resultObjects.ConnectionManager.QueryProcessor.ExecuteQuery(query);
                }
                catch (SmsQueryException ex)
                {
                    SccmExceptionDialog.ShowDialog(SnapIn.Console, ex);
                    return;
                }
            }
            // update the dialog total count
            if (showProgressDialog)
            {
                // query processor does not support count
                deviceProgressDialog.Total = result.Count > 0 ? result.Count : resultObjects.Count;
            }
                
            // get the method to run for the thread pool
            Type type = typeof(ClientActions);
            MethodInfo methodInfo = type.GetMethod(method);

            foreach (IResultObject resultObject in resultObjects)
            {
                if (showProgressDialog)
                    deviceProgressDialog.AddItem(resultObject["Name"].StringValue);

                ThreadPool.QueueUserWorkItem(arg => { methodInfo.Invoke(null, new object[] { resultObject }); });
            }
        }

        private static void ProccessException(IResultObject resultObject, Exception ex)
        {
            MethodInvoker invoker;

            if (ex.GetType().IsAssignableFrom(typeof(ManagementException)))
            {
                if (showProgressDialog)
                {
                    invoker = delegate { deviceProgressDialog.UpdateItem(resultObject["Name"].StringValue, "WMI error: " + ex.Message); };
                    deviceProgressDialog.Invoke(invoker);
                    deviceProgressDialog.UpdateProgress(DeviceProgressStatus.Failed);
                    deviceProgressDialog.UpdateProgress(DeviceProgressStatus.Other);
                }
                else
                {
                    SccmExceptionDialog.ShowDialog(SnapIn.Console, ex, "An error occured while invoking WMI method.");
                }
            }
            else if (ex.GetType().IsAssignableFrom(typeof(COMException)))
            {
                if (showProgressDialog)
                {
                    invoker = delegate { deviceProgressDialog.UpdateItem(resultObject["Name"].StringValue, "Offline"); };
                    deviceProgressDialog.Invoke(invoker);
                    deviceProgressDialog.UpdateProgress(DeviceProgressStatus.Offline);
                    deviceProgressDialog.UpdateProgress(DeviceProgressStatus.Other);
                }
                else
                {
                    SccmExceptionDialog.ShowDialog(SnapIn.Console, ex, "An error occured while connecting to host.");
                }
            }
            else
            {
                if (showProgressDialog)
                {
                    invoker = delegate { deviceProgressDialog.UpdateItem(resultObject["Name"].StringValue, "Error: " + ex.Message); };
                    deviceProgressDialog.Invoke(invoker);
                    deviceProgressDialog.UpdateProgress(DeviceProgressStatus.Failed);
                    deviceProgressDialog.UpdateProgress(DeviceProgressStatus.Other);
                }
                else
                {
                    SccmExceptionDialog.ShowDialog(SnapIn.Console, ex);
                }
            }
        }

        public static void ClientAction(IResultObject resultObject)
        {
            MethodInvoker invoker;
            if (deviceProgressDialog.CancellationTokenSource.IsCancellationRequested)
            {
                if (showProgressDialog)
                {
                    invoker = delegate { deviceProgressDialog.UpdateItem(resultObject["Name"].StringValue, "Canceled"); };
                    deviceProgressDialog.Invoke(invoker);
                }
                return;
            }

            try
            {
                if (showProgressDialog)
                {
                    invoker = delegate { deviceProgressDialog.UpdateItem(resultObject["Name"].StringValue, "Connecting"); };
                    deviceProgressDialog.Invoke(invoker);
                }

                ObjectGetOptions o = new ObjectGetOptions
                {
                    Timeout = TimeSpan.FromSeconds(5)
                };

                if (fullScan)
                {
                    ManagementScope scope = Utility.GetWMIScope(resultObject["Name"].StringValue, @"ccm\InvAgt");
                    ManagementObject result = Utility.GetFirstWMIInstance(scope, string.Format("SELECT * FROM InventoryActionStatus WHERE InventoryActionID = '{0}'", scheduleId));
                    result.Delete();
                }

                using (ManagementClass clientaction = new ManagementClass(string.Format(@"\\{0}\root\{1}:{2}", resultObject["Name"].StringValue, "ccm", "SMS_Client"), o))
                {
                    object[] methodArgs = { scheduleId };
                    clientaction.InvokeMethod("TriggerSchedule", methodArgs);
                }

                if (showProgressDialog)
                {
                    invoker = delegate { deviceProgressDialog.UpdateItem(resultObject["Name"].StringValue, "Completed"); };
                    deviceProgressDialog.Invoke(invoker);
                    deviceProgressDialog.UpdateProgress(DeviceProgressStatus.Completed);
                }
            }
            catch (Exception ex)
            {
                ProccessException(resultObject, ex);
            }
            finally
            {
                if (showProgressDialog)
                {
                    // thread safe update of progress bar
                    invoker = delegate { deviceProgressDialog.UpdateProgress(); };
                    deviceProgressDialog.Invoke(invoker);
                }
            }
        }

        public static async Task RestartServiceAsync(IResultObject resultObject)
        {
            MethodInvoker invoker;
            if (deviceProgressDialog.CancellationTokenSource.IsCancellationRequested)
            {
                if (showProgressDialog)
                {
                    invoker = delegate { deviceProgressDialog.UpdateItem(resultObject["Name"].StringValue, "Canceled"); };
                    deviceProgressDialog.Invoke(invoker);
                }
                return;
            }

            try
            {
                if (showProgressDialog)
                {
                    invoker = delegate { deviceProgressDialog.UpdateItem(resultObject["Name"].StringValue, "Connecting"); };
                    deviceProgressDialog.Invoke(invoker);
                }

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

                if (showProgressDialog)
                {
                    invoker = delegate { deviceProgressDialog.UpdateItem(resultObject["Name"].StringValue, "Completed"); };
                    deviceProgressDialog.Invoke(invoker);
                    deviceProgressDialog.UpdateProgress(DeviceProgressStatus.Completed);
                }
            }
            catch (System.ServiceProcess.TimeoutException ex)
            {
                if (showProgressDialog)
                {
                    invoker = delegate { deviceProgressDialog.UpdateItem(resultObject["Name"].StringValue, "Service timed out: " + ex.Message); };
                    deviceProgressDialog.Invoke(invoker);
                    deviceProgressDialog.UpdateProgress(DeviceProgressStatus.Failed);
                    deviceProgressDialog.UpdateProgress(DeviceProgressStatus.Other);
                }
                else
                {
                    SccmExceptionDialog.ShowDialog(SnapIn.Console, ex);
                }
            }
            catch (Exception ex)
            {
                ProccessException(resultObject, ex);
            }
            finally
            {
                if (showProgressDialog)
                {
                    // thread safe update of progress bar
                    invoker = delegate { deviceProgressDialog.UpdateProgress(); };
                    deviceProgressDialog.Invoke(invoker);
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
                await Task.Delay(250, cancellationToken)
                    .ConfigureAwait(false);
                controller.Refresh();
            }
        }
    }
}
