using Microsoft.ConfigurationManagement.AdminConsole;
using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using System.Globalization;
using System.Net;
using IniParser;
using IniParser.Model;
using System.Management;
using System.Collections.Generic;

namespace ConfigMgr.QuickTools.DriverManager
{
    public partial class DriverGrabberSelectPage : SmsPageControl
    {
        #region Private
        private ModifyRegistry registry = new ModifyRegistry();
        private string architecture = null;
        #endregion

        public DriverGrabberSelectPage(SmsPageData pageData)
            : base(pageData)
        {
            Title = "Select drivers";
            Headline = "Select drivers to capture";

            InitializeComponent();
        }

        public override void InitializePageControl()
        {
            base.InitializePageControl();

            labelDestination.Text = registry.ReadString("DriverSourceFolder");

            ControlsInspector.AddControl(dataGridViewDrivers, new ControlDataStateEvaluator(ValidateSelectedDrivers), "Select drivers to capture");

            Initialized = false;

            InitializeDataGridView();
        }

        protected override void ApplyChanges()
        {
            base.ApplyChanges();
        }

        public override void PostApply(BackgroundWorker worker, DoWorkEventArgs e)
        {
            try
            {
                Dictionary<string, string> error = new Dictionary<string, string>();
                List<string> successful = new List<string>();

                var parser = new FileIniDataParser();
                parser.Parser.Configuration.AllowDuplicateKeys = true;
                parser.Parser.Configuration.AllowDuplicateSections = true;
                parser.Parser.Configuration.SkipInvalidLines = true;
                parser.Parser.Configuration.CaseInsensitive = true;

                string sourceFolder = labelDestination.Text;
                string destinationFolder = Path.Combine(sourceFolder, textBoxDestination.Text);

                CredentialCache netCache = new CredentialCache();
                try
                {
                    netCache.Add(new Uri(sourceFolder), "Digest", CredentialCache.DefaultNetworkCredentials);
                    netCache.Add(new Uri(string.Format(@"\\{0}", PropertyManager["Name"].StringValue)), "Digest", CredentialCache.DefaultNetworkCredentials);
                }
                catch (IOException ex)
                {
                    throw new InvalidOperationException(string.Format("{0}: {1}", ex.GetType().Name, ex.Message));
                }

                if (!Directory.Exists(destinationFolder))
                    Directory.CreateDirectory(destinationFolder);

                int num = 0;

                List<ManagementObject> drivers = (List<ManagementObject>)UserData["DriverItems"];
                foreach (ManagementObject driver in drivers)
                {
                    int percent = num * 100 / drivers.Count;

                    try
                    {
                        string driverFolder = Path.Combine(destinationFolder, driver["DeviceClass"].ToString(), driver["DeviceName"].ToString(), driver["DriverVersion"].ToString());
                        if (!Directory.Exists(driverFolder))
                            Directory.CreateDirectory(driverFolder);

                        string infFile = driver["InfName"].ToString();

                        string remoteFolder = string.Format(@"\\{0}\admin$", PropertyManager["Name"].StringValue);

                        string remoteINFFile = Path.Combine(remoteFolder, "inf", infFile);

                        string inf = Path.Combine(driverFolder, infFile);

                        if (!File.Exists(inf))
                            File.Copy(remoteINFFile, inf);

                        IniData iniData = parser.ReadFile(inf);

                        string catalogFile = GetCatalog(iniData);

                        foreach (string file in Directory.GetFiles(Path.Combine(remoteFolder, @"System32\DriverStore\FileRepository"), catalogFile, SearchOption.AllDirectories))
                        {
                            FileInfo fileInfo = new FileInfo(file);
                            if (!File.Exists(Path.Combine(driverFolder, fileInfo.Name)))
                                File.Copy(file, Path.Combine(driverFolder, fileInfo.Name));

                            KeyDataCollection sourceDisksFiles;

                            if (architecture == "x86")
                                sourceDisksFiles = iniData["SourceDisksFiles.x86"];
                            else
                                sourceDisksFiles = iniData["SourceDisksFiles.amd64"];

                            if (sourceDisksFiles == null)
                                sourceDisksFiles = iniData["SourceDisksFiles"];

                            foreach (KeyData key in sourceDisksFiles)
                            {
                                if (key.KeyName.Split('.').Length > 1)
                                {
                                    string copyFile = Path.Combine(fileInfo.DirectoryName.ToString(), key.KeyName);
                                    if (File.Exists(copyFile) && !File.Exists(Path.Combine(driverFolder, key.KeyName)))
                                    {
                                        worker.ReportProgress(percent, string.Format("Processing Driver: {0}\n Copying File: {1}", driver["DeviceName"].ToString(), key.KeyName));
                                        File.Copy(copyFile, Path.Combine(driverFolder, key.KeyName));
                                    }
                                }
                            }
                            foreach (string path in Directory.GetDirectories(fileInfo.DirectoryName.ToString()))
                            {
                                DirectoryInfo source = new DirectoryInfo(path);
                                worker.ReportProgress(percent, string.Format("Processing Driver: {0}\n Copying Folder: {1}", driver["DeviceName"].ToString(), source.Name));
                                string target = Path.Combine(driverFolder, source.Name);
                                Copy(path, target);
                            }
                        }
                        successful.Add(driver["DeviceName"].ToString());
                    }
                    catch (Exception ex)
                    {
                        error.Add("Could not capture driver: " + ex.Message, driver["DeviceName"].ToString());
                    }

                    num++;
                }

                PrepareCompletion(successful, error);
                base.PostApply(worker, e);
            }
            catch (Exception ex)
            {
                PrepareError(ex.Message);
                throw;
            }
            finally
            {
                CredentialCache netCache = new CredentialCache();

                netCache.Remove(new Uri(labelDestination.Text), "Digest");
                netCache.Remove(new Uri(string.Format(@"\\{0}", PropertyManager["Name"].StringValue)), "Basic");
            }
        }

        public override void OnAddSummary(SummaryRequestHandler handler)
        {
            base.OnAddSummary(handler);

            List<ManagementObject> list = new List<ManagementObject>();
            foreach (DataGridViewRow dataGridViewRow in dataGridViewDrivers.Rows)
            {
                if (Convert.ToBoolean(dataGridViewRow.Cells[columnCapture.Name].Value) == true)
                {
                    if (dataGridViewRow.Tag is ManagementObject driver)
                    {
                        list.Add(driver);
                    }
                }
            }
            UserData["DriverItems"] = list;

            AddAction("GeneralDescription", string.Format("The following driver(s) will be captured ({0}):", list.Count));
            AddAction("DriverInformation", string.Empty);

            foreach (ManagementObject driver in list)
                AddActionDetailMessage("DriverInformation", driver["DeviceName"].ToString());
        }

        private void PrepareCompletion(List<string> successful, Dictionary<string, string> error)
        {
            RemoveAllSummary();

            if (successful.Count > 0)
            {
                AddAction("GeneralDescription", string.Format("The following driver(s) were captured ({0}):", successful.Count));
                UpdateActionStatus("GeneralDescription", SmsSummaryAction.ActionStatus.CompleteWithSuccess);
                AddAction("DriverInformation", string.Empty);
                foreach (string message in successful)
                    AddActionDetailMessage("DriverInformation", message);

                AddAction("EmptyLine", string.Empty);
            }

            if (error.Count > 0)
            {
                AddAction("ErrorDescription", string.Format("The following driver(s) failed to capture ({0}):", error.Count));
                UpdateActionStatus("ErrorDescription", SmsSummaryAction.ActionStatus.CompleteWithErrors);
                AddAction("ErrorInformation", string.Empty);
                foreach (KeyValuePair<string, string> item in error)
                {
                    AddActionDetailMessage("ErrorInformation", string.Format("{0}: {1}", item.Value, item.Key));
                }
            }
        }

        private void PrepareError(string errorMessage)
        {
            RemoveAllSummary();
            AddAction("ErrorInfo", errorMessage);
            UpdateActionStatus("ErrorInfo", SmsSummaryAction.ActionStatus.CompleteWithErrors);
        }

        private void RemoveAllSummary()
        {
            foreach (string id in Enumerable.ToList(Enumerable.Select(GetSummaryItems(), i => i.Id)))
                RemoveItem(id);
        }

        public override void OnActivated()
        {
            base.OnActivated();

            Utility.UpdateDataGridViewColumnsSize(dataGridViewDrivers, columnProvider);
        }

        public override bool OnDeactivate()
        {
            dataGridViewDrivers.EndEdit();
            return base.OnDeactivate();
        }

        private void InitializeDataGridView()
        {
            List<ManagementObject> signedDrivers = (List<ManagementObject>)UserData["SignedDrivers"];

            dataGridViewDrivers.Rows.Clear();
            foreach (ManagementObject item in signedDrivers)
            {
                string provider = (string)item["DriverProviderName"];
                string description = (string)item["DeviceName"];
                string oeminf = (string)item["InfName"];

                if (provider.Length > 0 && description.Length > 0)
                {
                    if (DriverExists(description, oeminf))
                        continue;

                    DataGridViewRow dataGridViewRow = new DataGridViewRow();
                    dataGridViewRow.CreateCells(dataGridViewDrivers);

                    dataGridViewRow.Cells[0].Value = false;
                    dataGridViewRow.Cells[1].Value = provider;
                    dataGridViewRow.Cells[2].Value = description;
                    dataGridViewRow.Cells[3].Value = (string)item["DriverVersion"];
                    dataGridViewRow.Cells[4].Value = oeminf;

                    dataGridViewRow.Tag = item;
                    dataGridViewDrivers.Rows.Add(dataGridViewRow);
                }
            }
  
            string osName;
            ManagementObject operatingSystem = (ManagementObject)UserData["OperatingSystem"];

            string currentversion = (string)operatingSystem["Version"];
            string v2 = "10.0";

            var version1 = new Version(currentversion);
            var version2 = new Version(v2);

            var result = version1.CompareTo(version2);
            if (result > 0)
            {
                osName = "Win10";
            }
            else
            {
                switch (currentversion)
                {
                    case "6.1.7601":
                        osName = "Win7";
                        break;
                    case "6.2.9200":
                        osName = "Win8";
                        break;
                    case "6.3.9600":
                        osName = "Win81";
                        break;
                    default:
                        osName = "Unknown";
                        break;
                }
            }

            if ((string)operatingSystem["OSArchitecture"] == "64-bit")
                architecture = "x64";
            else
                architecture = "x86";

            ManagementObject computerSystem = (ManagementObject)UserData["ComputerSystem"];

            if (registry.ReadBool("LegacyFolderStructure"))
            {
                textBoxDestination.Text = string.Format(@"{0}\{1}\{2}-{3}", ((string)computerSystem["Manufacturer"]).Trim(), ((string)computerSystem["Model"]).Trim(), osName, architecture);
            }
            else
            {
                textBoxDestination.Text = string.Format(@"{0}-{1}-{2}-{3}", ((string)computerSystem["Manufacturer"]).Trim(), ((string)computerSystem["Model"]).Trim(), osName, architecture);
            }

            Initialized = true;
        }

        private ControlDataState ValidateSelectedDrivers()
        {
            foreach (DataGridViewRow dataGridViewRow in dataGridViewDrivers.Rows)
            {
                if (Convert.ToBoolean(dataGridViewRow.Cells[0].Value, CultureInfo.InvariantCulture))
                {
                    return ControlDataState.Valid;
                }
            }
            return ControlDataState.Invalid;
        }

        private bool DriverExists(string desc, string oeminf)
        {
            foreach (DataGridViewRow dataGridViewRow in dataGridViewDrivers.Rows)
            {
                if ((string)dataGridViewRow.Cells[2].Value == desc || (string)dataGridViewRow.Cells[4].Value == oeminf)
                    return true;
            }
            return false;
        }

        private string GetCatalog(IniData Data)
        {
            string catalogFile = null;
            try
            {
                // set manufacturers to ini data manufacuter
                string[] manufacturers = Data["Manufacturer"].FirstOrDefault().Value.Split(',');
                string catalog = "CatalogFile";
                // check if we have more then one supported os
                if (manufacturers.Length > 1)
                {
                    for (int i = 1; i < manufacturers.Length; i++)
                    {
                        // create the os manufacurer string
                        string test = manufacturers[0].Trim() + "." + manufacturers[i].Trim();
                        if (Data.Sections.ContainsSection(test) && Data[test].Count != 0)
                        {
                            // if we found driver name string break
                            catalog = "CatalogFile." + manufacturers[i].Trim();
                            break;
                        }
                    }

                }

                // get model from os section
                catalogFile = Data["Version"][catalog];
                if (catalogFile == null)
                    catalogFile = Data["Version"]["CatalogFile"];
            }
            catch { }

            return catalogFile;
        }

        private void Copy(string sourceDir, string targetDir)
        {
            if (!Directory.Exists(targetDir))
            {
                Directory.CreateDirectory(targetDir);

                foreach (var file in Directory.GetFiles(sourceDir))
                    File.Copy(file, Path.Combine(targetDir, Path.GetFileName(file)));

                foreach (var directory in Directory.GetDirectories(sourceDir))
                    Copy(directory, Path.Combine(targetDir, Path.GetFileName(directory)));
            }
        }

        private void ButtonSelect_Click(object sender, EventArgs e)
        {
            if (dataGridViewDrivers.Rows.Count > 0)
            {
                dataGridViewDrivers.BeginEdit(true);
                bool flag = sender == buttonSelectAll;
                foreach (DataGridViewRow dataGridViewRow in dataGridViewDrivers.Rows)
                    dataGridViewRow.Cells[0].Value = flag ? true : false;
                dataGridViewDrivers.EndEdit();
            }
        }

        private void DataGridViewDrivers_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (!dataGridViewDrivers.IsCurrentCellDirty)
                return;
            dataGridViewDrivers.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }

        private void DataGridViewDrivers_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            ControlsInspector.InspectAll();
            Dirty = !ReadOnly;
        }
    }
}
