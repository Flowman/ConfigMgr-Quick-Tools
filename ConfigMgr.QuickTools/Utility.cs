using Microsoft.ConfigurationManagement.AdminConsole;
using Microsoft.ConfigurationManagement.ManagementProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;
using System.Collections;
using System.IO;
using System.Security.Cryptography;
using System.Threading;
using System.Management;
using System.Windows.Forms;
using Microsoft.ConfigurationManagement.ManagementProvider.DialogFramework;
using System.Net;
using System.Security.AccessControl;

namespace ConfigMgr.QuickTools
{
    public class Utility
    {
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

        public static bool AddObjectToFolder(ConnectionManagerBase connectionManager, string folderName, string objectId, int objectType)
        {
            try
            {
                IResultObject resultObject1;
                try
                {
                    string query = string.Format("SELECT * FROM SMS_ObjectContainerNode WHERE Name LIKE '{0}' AND ObjectType={1}", WqlEscapeString(folderName), objectType);
                    resultObject1 = connectionManager.QueryProcessor.ExecuteQuery(query);
                }
                catch (SmsQueryException ex)
                {
                    ExceptionUtilities.TraceException(ex);
                    throw;
                }
                IResultObject resultObject2 = null;
                int? nullable = new int?();
                foreach (IResultObject resultObject3 in resultObject1)
                {
                    if (resultObject3["ObjectType"].IntegerValue == objectType && resultObject3["ParentContainerNodeID"].IntegerValue == 0)
                    {
                        resultObject2 = resultObject3;
                        break;
                    }
                }
                if (resultObject2 == null)
                {
                    try
                    {
                        nullable = CreateFolder(connectionManager, folderName, objectType);
                    }
                    catch (Exception ex)
                    {
                        ExceptionUtilities.TraceException(ex);
                        return false;
                    }
                }
                else
                    nullable = new int?(resultObject2["ContainerNodeID"].IntegerValue);
                IResultObject resultObject4 = null;
                string query2 = string.Format("Select * From SMS_ObjectContainerItem Where InstanceKey='{0}'", objectId);
                IResultObject resultObject5 = connectionManager.QueryProcessor.ExecuteQuery(query2);
                if (resultObject5 != null)
                {
                    IEnumerator enumerator = resultObject5.GetEnumerator();
                    if (enumerator.MoveNext())
                        resultObject4 = (IResultObject)enumerator.Current;
                    if (resultObject4 != null)
                    {
                        if (resultObject4["MemberID"] == null)
                            resultObject4 = null;
                    }
                }
                if (resultObject4 == null)
                {
                    resultObject4 = connectionManager.CreateInstance("SMS_ObjectContainerItem");
                }
                if (resultObject4 == null)
                {
                    return false;
                }
                resultObject4["InstanceKey"].StringValue = objectId;
                resultObject4["ObjectType"].IntegerValue = objectType;
                resultObject4["ContainerNodeID"].IntegerValue = nullable.Value;
                resultObject4.Put();
                resultObject4.Get();
                return true;
            }
            catch (Exception ex)
            {
                ExceptionUtilities.TraceException(ex);
                return false;
            }
        }

        public static int? CreateFolder(ConnectionManagerBase connectionManager, string folderName, int objectType)
        {
            IResultObject instance = connectionManager.CreateInstance("SMS_ObjectContainerNode");
            instance["Name"].StringValue = folderName;
            instance["ObjectType"].IntegerValue = objectType;
            instance["ParentContainerNodeID"].IntegerValue = 0;
            instance.Put();
            instance.Get();
            if (instance == null)
                return new int?();
            return new int?(instance["ContainerNodeID"].IntegerValue);
        }

        public static string WqlEscapeString(string text)
        {
            if (string.IsNullOrEmpty(text))
                return text;
            text = text.Replace("_", "[_]");
            text = text.Replace("'", "''");
            return text;
        }

        public static string CreateHashForFolder(string path)
        {
            // assuming you want to include nested folders
            var exts = new[] { ".hash", ".version" };
            List<string> files = Directory
                .GetFiles(path, "*.*", SearchOption.AllDirectories)
                .Where(file => !exts.Any(x => file.EndsWith(x, StringComparison.OrdinalIgnoreCase)))
                .OrderBy(p => p).ToList();
            // create a temp file with all the files as content
            string tempHashFile = Path.Combine(Path.GetTempPath(), RandomString(16, false) + ".hash");

            string hash = null;

            using (SHA256 shaHash = SHA256.Create())
            {
                if (files.Count > 0)
                {
                    // create a new file     
                    using (FileStream fs = File.Create(tempHashFile))
                    {
                        StreamWriter writer = new StreamWriter(fs);
                        for (int i = 0; i < files.Count; i++)
                        {
                            string file = files[i];
                            string content = file + " " + file.Length + Environment.NewLine;
                            writer.Write(content);
                        }
                        writer.Close();
                    }
                    shaHash.ComputeHash(Encoding.UTF8.GetBytes(File.ReadAllText(tempHashFile)));
                    hash = BitConverter.ToString(shaHash.Hash).Replace("-", "");

                    File.Delete(tempHashFile);
                }
            }

            return hash;
        }

        public static ManagementScope GetWMIScope(string host, string space, string username = null, string password = null)
        {
            // TODO update to have all the wmi connection catch here, so we can remove it from the other places
            ConnectionOptions options = new ConnectionOptions
            {
                Authentication = AuthenticationLevel.PacketPrivacy,
                Impersonation = ImpersonationLevel.Impersonate,
                EnablePrivileges = true,
                Timeout = TimeSpan.FromSeconds(5)
            };
            if (username != null)
            {
                options.Username = username;
                options.Password = password;
            }

            ManagementScope scope = new ManagementScope(string.Format(@"\\{0}\root\{1}", host, space), options);
            scope.Connect();

            if (scope.IsConnected == true)
            {
                return scope;
            }
            else
            {
                throw new InvalidOperationException("Cannot connect to WMI");
            }
        }

        public static IEnumerable<ManagementObject> SearchWMI(ManagementScope scope, ObjectQuery query)
        {
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(scope, query);
            using (ManagementObjectCollection queryCollection = searcher.Get())
            {
                foreach (ManagementObject managmentObject in queryCollection)
                {
                    yield return managmentObject;
                }
            }
        }

        public static List<ManagementObject> SearchWMIToList(ManagementScope scope, string query)
        {
            return SearchWMIToList(scope, new ObjectQuery(query));
        }

        public static List<ManagementObject> SearchWMIToList(ManagementScope scope, ObjectQuery query)
        {
            List<ManagementObject> list = new List<ManagementObject>();
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(scope, query);
            using (ManagementObjectCollection queryCollection = searcher.Get())
            {
                foreach (ManagementObject managmentObject in queryCollection)
                {
                    list.Add(managmentObject);
                }
            }
            return list;
        }

        public static ManagementObject GetFirstWMIInstance(ManagementScope scope, string query)
        {
            return GetFirstWMIInstance(scope, new ObjectQuery(query));
        }

        public static ManagementObject GetFirstWMIInstance(ManagementScope scope, ObjectQuery query)
        {
            ManagementObject managmentObject = null;
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(scope, query);
            using (ManagementObjectCollection queryCollection = searcher.Get())
            {
                foreach (ManagementObject managmentObject1 in queryCollection)
                    managmentObject = managmentObject1;
            }
            return managmentObject;
        }

        public static ManagementObject GetFirstWMIInstance(string host, string space, string query, string select = "*")
        {
            ManagementScope scope = GetWMIScope(host, space);

            ObjectQuery wmiquery = new ObjectQuery(string.Format("SELECT {0} FROM {1}", select, query));
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(scope, wmiquery);
            var enu = searcher.Get().GetEnumerator();
            if (!enu.MoveNext()) throw new Exception("Unexpected WMI query failure");
            return (ManagementObject)enu.Current;
        }

        public static IEnumerable<IResultObject> SearchWMI(ConnectionManagerBase connectionManager, string query)
        {
            using (IResultObject resultObjects = connectionManager.QueryProcessor.ExecuteQuery(query))
            {
                foreach (IResultObject resultObject in resultObjects)
                {
                    yield return resultObject;
                }
            }
        }

        public static List<IResultObject> SearchWMIToList(ConnectionManagerBase connectionManager, string query)
        {
            List<IResultObject> list = new List<IResultObject>();
            using (IResultObject resultObjects = connectionManager.QueryProcessor.ExecuteQuery(query))
            {
                foreach (IResultObject resultObject in resultObjects)
                {
                    list.Add(resultObject);
                }
            }
            return list;
        }

        public static IResultObject GetFirstWMIInstance(ConnectionManagerBase connectionManager, string query)
        {
            IResultObject resultObject = null;
            using (IResultObject resultObjects = connectionManager.QueryProcessor.ExecuteQuery(query))
            {
                foreach (IResultObject resultObject1 in resultObjects)
                    resultObject = resultObject1;
            }
            return resultObject;
        }

        public static void RequestLock(ConnectionManagerBase connectionManager, string objectPath)
        {
            if (connectionManager == null)
                throw new ArgumentNullException("connectionManager");
            if (string.IsNullOrEmpty(objectPath))
                return;
            DateTime dateTime = DateTime.Now.AddSeconds(ReadObjectLockTimeout());
            Dictionary<string, object> methodParameters = new Dictionary<string, object>
            {
                ["ObjectRelPath"] = objectPath,
                ["RequestTransfer"] = true
            };
            while (DateTime.Now < dateTime)
            {
                IResultObject resultObject;
                try
                {
                    resultObject = connectionManager.ExecuteMethod("SMS_ObjectLock", "RequestLock", methodParameters);
                }
                catch (SmsQueryException ex)
                {
                    ExceptionUtilities.TraceException(ex);
                    Thread.Sleep(1000);
                    continue;
                }
                if (resultObject == null)
                {
                    Thread.Sleep(1000);
                }
                else
                {
                    using (resultObject)
                    {
                        if (resultObject["RequestState"].ObjectValue != null)
                        {
                            if (resultObject["RequestState"].IntegerValue == 10 || resultObject["RequestState"].IntegerValue == 11)
                                break;
                            if (resultObject["RequestState"].IntegerValue == 12)
                                break;
                        }
                    }
                    Thread.Sleep(1000);
                }
            }
        }

        public static void ReleaseLock(ConnectionManagerBase connectionManager, string objectPath)
        {
            if (connectionManager == null)
                throw new ArgumentNullException("connectionManager");
            if (string.IsNullOrEmpty(objectPath))
                return;
            Dictionary<string, object> methodParameters = new Dictionary<string, object>();
            methodParameters["ObjectRelPath"] = objectPath;
            connectionManager.ExecuteMethod("SMS_ObjectLock", "ReleaseLock", methodParameters);
        }

        private static int ReadObjectLockTimeout()
        {
            RegistryKey registryKey = null;
            try
            {
                registryKey = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\ConfigMgr10\\AdminUI", false);
                if (registryKey != null)
                {
                    try
                    {
                        object obj = registryKey.GetValue("ObjectLockTimeOut");
                        if (obj != null)
                            return (int)obj;
                    }
                    catch (InvalidCastException)
                    {
                    }
                }
                else
                {
                    registryKey = Registry.LocalMachine.OpenSubKey("Software\\Microsoft\\ConfigMgr10\\AdminUI", false);
                    if (registryKey != null)
                    {
                        try
                        {
                            object obj = registryKey.GetValue("ObjectLockTimeOut");
                            if (obj != null)
                                return (int)obj;
                        }
                        catch (InvalidCastException)
                        {
                        }
                    }
                }
            }
            finally
            {
                if (registryKey != null)
                    registryKey.Close();
            }
            return 30;
        }

        public static void UpdateDataGridViewColumnsSize(DataGridView dataGridView, DataGridViewColumn targetColumn)
        {
            if (dataGridView == null || targetColumn == null || !dataGridView.Columns.Contains(targetColumn))
                return;
            int num = dataGridView.Width - dataGridView.Margin.Left - dataGridView.Margin.Right;
            foreach (DataGridViewColumn dataGridViewColumn in dataGridView.Columns)
            {
                if (dataGridViewColumn != targetColumn && dataGridViewColumn.Visible == true)
                    num -= dataGridViewColumn.Width;
            }

            VScrollBar vScrollbar = dataGridView.Controls.OfType<VScrollBar>().First();
            if (vScrollbar.Visible)
            {
                num -= SystemInformation.VerticalScrollBarWidth;
            }
            if (num >= dataGridView.Width - dataGridView.Margin.Left - dataGridView.Margin.Right || num <= targetColumn.MinimumWidth)
                return;
            targetColumn.Width = num;
        }

        public static string RandomString(int size, bool lowerCase)
        {
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }
            if (lowerCase)
                return builder.ToString().ToLower();
            return builder.ToString();
        }

        public static bool InclusiveBetween(IComparable a, IComparable b, IComparable c)
        {
            return a.CompareTo(b) >= 0 && a.CompareTo(c) <= 0;
        }

        public static DialogResult ShowDialog(string dialogId, IResultObject resultObject)
        {
            ISmsForm dialog = DialogFactoryBase.DialogFactory.CreateDialog(dialogId, resultObject, (PropertyDataUpdated)null, false);
            using (dialog.Form)
            {
                dialog.Initialize();
                dialog.Form.ShowInTaskbar = false;
                return dialog.Form.ShowDialog();
            }
        }
        public static long GetFileSize(Uri url)
        {
            long result = -1;

            WebRequest req = WebRequest.Create(url);
            req.Method = "HEAD";
            using (WebResponse resp = req.GetResponse())
            {
                if (long.TryParse(resp.Headers.Get("Content-Length"), out long ContentLength))
                {
                    result = ContentLength;
                }
            }

            return result;
        }

        public static long GetFileSize(string url)
        {
            return GetFileSize(new Uri(url)); 
        }

        public static void Copy(string sourceDir, string targetDir, bool overwrite = false)
        {
            if (!Directory.Exists(targetDir) || overwrite)
            {
                Directory.CreateDirectory(targetDir);

                foreach (var file in Directory.GetFiles(sourceDir))
                {
                    File.Copy(file, Path.Combine(targetDir, Path.GetFileName(file)), overwrite);
                }

                foreach (var directory in Directory.GetDirectories(sourceDir))
                    Copy(directory, Path.Combine(targetDir, Path.GetFileName(directory)), overwrite);
            }
        }

        public static bool CheckFolderPermissions(string directoryPath, FileSystemRights accessType)
        {
            bool hasAccess = true;
            try
            {
                AuthorizationRuleCollection collection = Directory.GetAccessControl(directoryPath).GetAccessRules(true, true, typeof(System.Security.Principal.NTAccount));
                foreach (FileSystemAccessRule rule in collection)
                {
                    if ((rule.FileSystemRights & accessType) > 0)
                    {
                        return hasAccess;
                    }
                }
            }
            catch
            {
                hasAccess = false;
            }
            return hasAccess;
        }
    }

    public class ModifyRegistry
    {
        public string SubKey { get; set; } = "SOFTWARE\\Microsoft\\ConfigMgr10\\QuickTools";
        public RegistryKey BaseRegistryKey { get; set; } = Registry.CurrentUser;

        protected object Read(string KeyName)
        {
            object result = default(object);

            try
            {
                RegistryKey rk = BaseRegistryKey;
                RegistryKey sk1 = rk.OpenSubKey(SubKey);

                return sk1 == null ? result : sk1.GetValue(KeyName);
            }
            catch (Exception) { }

            return result;
        }

        public int ReadInt(string KeyName)
        {
            object obj = Read(KeyName);

            return obj != null ? int.Parse(obj.ToString()) : 0;
        }

        public double? ReadDouble(string KeyName)
        {
            object obj = Read(KeyName);
            double? result = null;
            if (obj != null)
            {
                result = double.Parse(obj.ToString());
            }

            return result;
        }

        public bool ReadBool(string KeyName)
        {
            bool result = default(bool);
            string resultStr = (string)Read(KeyName);
            if (!string.IsNullOrEmpty(resultStr))
            {
                result = bool.Parse(resultStr);
            }

            return result;
        }

        public string ReadString(string KeyName)
        {
            string result = string.Empty;
            string resultStr = (string)Read(KeyName);
            if (!string.IsNullOrEmpty(resultStr))
            {
                result = resultStr;
            }

            return result;
        }

        public bool Write(string KeyName, object Value)
        {
            try
            {
                using (RegistryKey rk = BaseRegistryKey)
                {
                    RegistryKey sk1 = rk.CreateSubKey(SubKey);
                    sk1.SetValue(KeyName, Value);
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
