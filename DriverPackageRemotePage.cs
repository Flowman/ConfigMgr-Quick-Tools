using Microsoft.ConfigurationManagement.AdminConsole;
using Microsoft.ConfigurationManagement.AdminConsole.DialogFramework;
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

namespace Zetta.ConfigMgr.QuickTools
{
    public partial class DriverPackageRemotePage : SmsPageControl
    {
        private ModifyRegistry registry = new ModifyRegistry();
        private BackgroundWorker backgroundWorker;
        private string architecture = null;

        public DriverPackageRemotePage(SmsPageData pageData)
            : base(pageData)
        {
            InitializeComponent();
            Headline = "Select a remote machine to grab drivers from";
            Title = "Select Machine";
            panelComplete.Dock = DockStyle.Bottom;
            panelProcessing.Dock = DockStyle.Bottom;
            panelDone.Dock = DockStyle.Bottom;
            pageData.ProgressBarStyle = ProgressBarStyle.Continuous;
        }

        public override void InitializePageControl()
        {
            base.InitializePageControl();

            ControlsInspector.AddControl(dataGridViewDrivers, new ControlDataStateEvaluator(ValidateSelectedDrivers), "Select drivers to capture");
            ControlsInspector.AddControl(panelDone, new ControlDataStateEvaluator(ValidateCapture), "Drivers need to be captured before continuing to the next step");
        }

        public override void OnActivated()
        {
            base.OnActivated();

            if (!(bool)UserData["UserCredentials"])
            {
                textBoxPassword.Text = null;
                textBoxUsername.Text = null;
            }

            labelPassword.Visible = (bool)UserData["UserCredentials"];
            labelUsername.Visible = (bool)UserData["UserCredentials"];
            textBoxPassword.Visible = (bool)UserData["UserCredentials"];
            textBoxUsername.Visible = (bool)UserData["UserCredentials"];

            labelDestination.Text = registry.Read("browseFolderControlSource");

            Initialized = true;
        }

        public override bool OnDeactivate()
        {
            CredentialCache netCache = new CredentialCache();
            if (!string.IsNullOrEmpty(labelDestination.Text))
            {
                netCache.Remove(new Uri(labelDestination.Text), "Digest");
            }
            if (!string.IsNullOrEmpty(textBoxMachine.Text))
            {
                netCache.Remove(new Uri(string.Format(@"\\{0}", textBoxMachine.Text)), "Basic");
            }
            return base.OnDeactivate();
        }

        private void buttonConnect_Click(object sender, EventArgs e)
        {
            if (backgroundWorker != null && backgroundWorker.IsBusy)
            {
                backgroundWorker.CancelAsync();
                UseWaitCursor = false;
                buttonConnect.Text = "Cancelling..";
            }
            else
            {
                dataGridViewDrivers.Rows.Clear();

                panelComplete.Visible = false;
                panelProcessing.Visible = true;

                backgroundWorker = new BackgroundWorker();
                backgroundWorker.DoWork += new DoWorkEventHandler(infoWorker_DoWork);
                backgroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(infoWorker_RunWorkerCompleted);
                backgroundWorker.ProgressChanged += new ProgressChangedEventHandler(infoWorker_ProgressChanged);
                backgroundWorker.WorkerSupportsCancellation = true;
                backgroundWorker.WorkerReportsProgress = true;
                progressBarObjects.Value = 0;
                labelProcessingObject.Text = null;
                textBoxDestination.Text = null;
                UseWaitCursor = true;
                backgroundWorker.RunWorkerAsync();
            }
        }

        private void infoWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker backgroundWorker = sender as BackgroundWorker;
            ManagementScope scope = new ManagementScope();
            UserData["CaptureComputer"] = textBoxMachine.Text;
            string remoteComputer = textBoxMachine.Text;

            buttonConnect.Text = "Cancel";

            backgroundWorker.ReportProgress(33, "Connecting..");

            try
            {
                if (string.IsNullOrEmpty(textBoxUsername.Text))
                {
                    scope = Utility.GetWMIScope(remoteComputer, @"cimv2");
                }
                else
                {
                    scope = Utility.GetWMIScope(remoteComputer, @"cimv2", textBoxUsername.Text, textBoxPassword.Text);
                }
            }
            catch (IOException ex)
            {
                throw new InvalidOperationException(string.Format("{0}: {1}", ex.GetType().Name, ex.Message));
            }

            ObjectQuery query = new ObjectQuery("SELECT * FROM Win32_PnPSignedDriver WHERE DriverProviderName IS NOT NULL");
            List<ManagementObject> signedDriver = Utility.SearchWMIToList(scope, query);

            backgroundWorker.ReportProgress(66, "Processing data.");

            foreach (ManagementObject item in signedDriver)
            {
                if (backgroundWorker.CancellationPending)
                {
                    e.Cancel = true;
                    return;
                }

                string provider = (string)item["DriverProviderName"];
                string description = (string)item["DeviceName"];
                string oeminf = (string)item["InfName"];

                if (provider.Length > 0 && description.Length > 0)
                {
                    if (provider == "Microsoft" || DriverExists(description, oeminf))
                        continue;

                    DataGridViewRow dataGridViewRow = new DataGridViewRow();
                    dataGridViewRow.CreateCells(dataGridViewDrivers);

                    dataGridViewRow.Cells[0].Value = false;
                    dataGridViewRow.Cells[1].Value = provider;
                    dataGridViewRow.Cells[2].Value = description;
                    dataGridViewRow.Cells[3].Value = (string)item["DriverVersion"];
                    dataGridViewRow.Cells[4].Value = oeminf;
                    dataGridViewRow.Cells[5].Value = (string)item["DeviceClass"];

                    dataGridViewRow.Tag = item;
                    dataGridViewDrivers.Rows.Add(dataGridViewRow);
                }
            }

            query = new ObjectQuery("SELECT * FROM Win32_ComputerSystem");
            ManagementObject computerSystem = Utility.GetFirstWMIInstance(scope, query);

            query = new ObjectQuery("SELECT * FROM Win32_OperatingSystem");
            ManagementObject operatingSystem = Utility.GetFirstWMIInstance(scope, query);

            string osName;

            string currentversion = (string) operatingSystem["Version"];

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
                case "10.0.10240":
                    osName = "Win10";
                    break;
                default:
                    osName = "Unknown";
                    break;
            }

            if ((string)operatingSystem["OSArchitecture"] == "64-bit")
                architecture = "x64";
            else
                architecture = "x86";

            textBoxDestination.Text = string.Format(@"{0}\{1}\{2}-{3}", ((string)computerSystem["Manufacturer"]).Trim(), ((string)computerSystem["Model"]).Trim(), osName, architecture);

            backgroundWorker.ReportProgress(100, "Done.");

            if (backgroundWorker.CancellationPending)
            {
                backgroundWorker.Dispose();
                e.Cancel = true;
            }
        }

        private void infoWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if ((sender as BackgroundWorker).CancellationPending)
                return;
            progressBarObjects.Value = e.ProgressPercentage;
            labelProcessingObject.Text = (e.UserState as string);
        }

        private void infoWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                if (e.Error != null)
                {
                    using (SccmExceptionDialog sccmExceptionDialog = new SccmExceptionDialog(e.Error))
                    {
                        int num = (int)sccmExceptionDialog.ShowDialog();
                    }
                    progressBarObjects.Value = 100;
                    labelProcessingObject.Text = string.Format("Error: {0}", e.Error.Message);
                }
                else if (e.Cancelled)
                {
                    progressBarObjects.Value = 100;
                    labelProcessingObject.Text = "Cancelled by user.";
                }
                else
                {
                    panelComplete.Visible = true;
                    panelProcessing.Visible = false;
                }
            }
            finally
            {
                if (sender as BackgroundWorker == backgroundWorker)
                {
                    backgroundWorker.Dispose();
                    backgroundWorker = null;
                    UseWaitCursor = false;

                    buttonConnect.Text = "Connect";
                }
            }
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

        private void buttonSelect_Click(object sender, EventArgs e)
        {
            dataGridViewDrivers.BeginEdit(true);
            bool flag = sender == buttonSelectAll;
            foreach (DataGridViewRow dataGridViewRow in dataGridViewDrivers.Rows)
                dataGridViewRow.Cells[0].Value = flag ? true : false;
            dataGridViewDrivers.EndEdit();
        }

        private ControlDataState ValidateSelectedDrivers()
        {
            foreach (DataGridViewRow dataGridViewRow in dataGridViewDrivers.Rows)
            {
                if (Convert.ToBoolean(dataGridViewRow.Cells[0].Value, CultureInfo.InvariantCulture))
                {
                    buttonCapture.Enabled = true;
                    return ControlDataState.Valid;
                }
            }
            buttonCapture.Enabled = false;
            return ControlDataState.Invalid;
        }

        private ControlDataState ValidateCapture()
        {
            if (panelDone.Visible == true)
            {
                return ControlDataState.Valid;
            }

            return ControlDataState.Invalid;
        }

        private void dataGridViewDrivers_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (!dataGridViewDrivers.IsCurrentCellDirty)
                return;
            dataGridViewDrivers.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }

        private void dataGridViewDrivers_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            ControlsInspector.InspectAll();
            Dirty = !ReadOnly;
        }

        private void dataGridViewDrivers_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            //Utility.UpdateDataGridViewColumnsSize(dataGridViewDrivers, columnDesc);
        }

        private void buttonCapture_Click(object sender, EventArgs e)
        {
            panelComplete.Visible = false;
            panelProcessing.Visible = true;

            buttonConnect.Enabled = false;
            buttonCapture.Enabled = false;
            textBoxMachine.Enabled = false;

            backgroundWorker = new BackgroundWorker();
            backgroundWorker.DoWork += new DoWorkEventHandler(captureWorker_DoWork);
            backgroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(captureWorker_RunWorkerCompleted);
            backgroundWorker.ProgressChanged += new ProgressChangedEventHandler(infoWorker_ProgressChanged);
            backgroundWorker.WorkerSupportsCancellation = true;
            backgroundWorker.WorkerReportsProgress = true;
            progressBarObjects.Value = 0;
            labelProcessingObject.Text = null;
            UseWaitCursor = true;
            backgroundWorker.RunWorkerAsync();
        }

        private void captureWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker backgroundWorker = sender as BackgroundWorker;

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
                if (string.IsNullOrEmpty(textBoxUsername.Text))
                {
                    netCache.Add(new Uri(string.Format(@"\\{0}", (string)UserData["CaptureComputer"])), "Digest", CredentialCache.DefaultNetworkCredentials);
                }
                else
                {
                    NetworkCredential credentials = new NetworkCredential(textBoxUsername.Text, textBoxPassword.Text);
                    netCache.Add(new Uri(string.Format(@"\\{0}", (string)UserData["CaptureComputer"])), "Basic", credentials);
                }
            }
            catch (IOException ex)
            {
                throw new InvalidOperationException(string.Format("{0}: {1}", ex.GetType().Name, ex.Message));
            }

            if (!Directory.Exists(destinationFolder))
                Directory.CreateDirectory(destinationFolder);

            int num = 0;

            foreach (DataGridViewRow dataGridViewRow in dataGridViewDrivers.Rows)
            {
                if (backgroundWorker.CancellationPending)
                {
                    e.Cancel = true;
                    return;
                }

                int percent = num * 100 / dataGridViewDrivers.Rows.Count;

                backgroundWorker.ReportProgress(percent, string.Format("Processing Driver: {0}", (string)dataGridViewRow.Cells[columnDesc.Name].Value));

                if (Convert.ToBoolean(dataGridViewRow.Cells[columnCapture.Name].Value) == true)
                {
                    try
                    {
                        string driverFolder = Path.Combine(destinationFolder, (string)dataGridViewRow.Cells[columnClass.Name].Value, (string)dataGridViewRow.Cells[columnDesc.Name].Value, (string)dataGridViewRow.Cells[columnVersion.Name].Value);
                        if (!Directory.Exists(driverFolder))
                            Directory.CreateDirectory(driverFolder);

                        string infFile = (string)dataGridViewRow.Cells[columnOEMInf.Name].Value;

                        string remoteFolder = string.Format(@"\\{0}\admin$", (string)UserData["CaptureComputer"]);

                        string remoteINFFile = Path.Combine(remoteFolder, "inf", infFile);

                        string inf = Path.Combine(driverFolder, infFile);

                        if (!File.Exists(inf))
                            File.Copy(remoteINFFile, inf);

                        IniData iniData = parser.ReadFile(inf);

                        string catalogFile = getCatalog(iniData);

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
                                        backgroundWorker.ReportProgress(percent, string.Format("Processing Driver: {0}\n Copying File: {1}", (string)dataGridViewRow.Cells[columnDesc.Name].Value, key.KeyName));
                                        File.Copy(copyFile, Path.Combine(driverFolder, key.KeyName));
                                    }
                                }
                            }
                            foreach (string path in Directory.GetDirectories(fileInfo.DirectoryName.ToString()))
                            {
                                DirectoryInfo source = new DirectoryInfo(path);
                                backgroundWorker.ReportProgress(percent, string.Format("Processing Driver: {0}\n Copying Folder: {1}", (string)dataGridViewRow.Cells[columnDesc.Name].Value, source.Name));
                                string target = Path.Combine(driverFolder, source.Name);
                                Copy(path, target);
                            }
                        }

                    }
                    catch 
                    { 
                    
                    }
                }

                ++num;
            }

            if (backgroundWorker.CancellationPending)
            {
                backgroundWorker.Dispose();
                e.Cancel = true;
            }
        }

        private void captureWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                if (e.Error != null)
                {
                    using (SccmExceptionDialog sccmExceptionDialog = new SccmExceptionDialog(e.Error))
                    {
                        int num = (int)sccmExceptionDialog.ShowDialog();
                    }
                }
                else if (e.Cancelled)
                {

                }
            }
            finally
            {
                if (sender as BackgroundWorker == backgroundWorker)
                {
                    backgroundWorker.Dispose();
                    backgroundWorker = null;
                    UseWaitCursor = false;
                    panelComplete.Visible = false;
                    panelProcessing.Visible = false;
                    panelDone.Visible = true;
                    ControlsInspector.InspectAll();

                    CredentialCache netCache = new CredentialCache();
                    netCache.Remove(new Uri(labelDestination.Text), "Digest");
                    netCache.Remove(new Uri(string.Format(@"\\{0}", textBoxMachine.Text)), "Basic");
                }
            }
        }

        private string getCatalog(IniData Data)
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
            catch
            {

            }

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

        private void textBoxMachine_TextChanged(object sender, EventArgs e)
        {
            buttonConnect.Enabled = string.IsNullOrEmpty(textBoxMachine.Text) ? false : true;
        }
    }
}
