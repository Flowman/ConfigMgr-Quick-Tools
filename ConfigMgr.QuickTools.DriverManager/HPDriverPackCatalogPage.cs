using ByteSizeLib;
using Microsoft.ConfigurationManagement.AdminConsole;
using Microsoft.Deployment.Compression;
using Microsoft.Deployment.Compression.Cab;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using System.Xml.Linq;

namespace ConfigMgr.QuickTools.DriverManager
{
    public partial class HPDriverPackCatalogPage : SmsPageControl
    {
        private BackgroundWorker backgroundWorker;
        private readonly ModifyRegistry registry = new ModifyRegistry();
        private XElement catalog;
        private Process extractProcess;
        private WebClient webClient;
        private readonly Queue<KeyValuePair<HPDriverPackage, Uri>> _downloadUrls = new Queue<KeyValuePair<HPDriverPackage, Uri>>();
        private bool downloadQueueFinished = false;
        private readonly Queue<KeyValuePair<HPDriverPackage, string>> _extractFiles = new Queue<KeyValuePair<HPDriverPackage, string>>();
        private bool extractQueueFinished = false;
        private readonly Dictionary<string, string> error = new Dictionary<string, string>();
        private readonly List<string> successful = new List<string>();
        private int totalPacks;
        private int downloadedPacks = 0;
        private readonly int extracted = 1;
        private string currentDownloadFileName;
        private string currentModel;
        private HPDriverPackage currentPackage;
        private string os;
        private string structure;
        private string tempFolderPath;
        private string sourceFolderPath;

        public HPDriverPackCatalogPage(SmsPageData pageData)
            : base(pageData)
        {
            Title = "Select Driver Pack";
            Headline = "Select driver pack to download";

            InitializeComponent();

            checkBoxOverwrite.Visible = false;
        }

        public override void InitializePageControl()
        {
            base.InitializePageControl();

            ControlsInspector.AddControl(dataGridViewDriverPackages, new ControlDataStateEvaluator(ValidateSelectedPack), "Select driver pack to download");
            dataGridViewDriverPackages.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            Initialized = false;
        }

        protected override void ApplyChanges()
        {
            base.ApplyChanges();
        }

        public override void PostApply(BackgroundWorker worker, DoWorkEventArgs e)
        {
            try
            {
                backgroundWorker = worker;

                worker.ReportProgress(0, "Queuing up Driver Pack downloads ...");

                List<HPDriverPackage> packages = (List<HPDriverPackage>)UserData["DriverPackItems"];
                totalPacks = packages.Count;

                foreach (HPDriverPackage package in packages)
                {
                    Uri uri = new Uri(package.Url);

                    _downloadUrls.Enqueue(new KeyValuePair<HPDriverPackage, Uri>(package, uri));
                }

                DownloadFiles();

                do // wait for all the download to finish
                {
                    Thread.Sleep(1000);
                } while (downloadQueueFinished == false);

                worker.ReportProgress(50, "Extracting Driver Packs ...");

                ExtractFiles();

                do // wait for all the extractions to finish
                {
                    Thread.Sleep(500);
                } while (extractQueueFinished == false);

                PrepareCompletion(successful, error);
                base.PostApply(worker, e);
            }
            catch (Exception ex)
            {
                PrepareError(ex.Message);
                throw;
            }
        }

        public override void OnAddSummary(SummaryRequestHandler handler)
        {
            base.OnAddSummary(handler);

            List<HPDriverPackage> list = new List<HPDriverPackage>();
            foreach (DataGridViewRow dataGridViewRow in dataGridViewDriverPackages.Rows)
            {
                if (Convert.ToBoolean(dataGridViewRow.Cells[columnSelected.Name].Value) == true)
                {
                    if (dataGridViewRow.Tag is HPDriverPackage pack)
                    {
                        list.Add(pack);
                    }
                }
            }
            UserData["DriverPackItems"] = list;

            AddAction("GeneralDescription", string.Format("The following driver pack(s) will be imported ({0}):", list.Count));
            AddAction("DriverPackInformation", string.Empty);

            foreach (HPDriverPackage package in list)
            {
                AddActionDetailMessage("DriverPackInformation", package.Model);
            }
        }

        private void PrepareCompletion(List<string> successful, Dictionary<string, string> error)
        {
            RemoveAllSummary();

            if (successful.Count > 0)
            {
                AddAction("GeneralDescription", string.Format("The following driver pack(s) was imported ({0}):", successful.Count));
                UpdateActionStatus("GeneralDescription", SmsSummaryAction.ActionStatus.CompleteWithSuccess);
                AddAction("DriverPackInformation", string.Empty);
                foreach (string message in successful)
                    AddActionDetailMessage("DriverPackInformation", message);

                AddAction("EmptyLine", string.Empty);
            }

            if (error.Count > 0)
            {
                AddAction("ErrorDescription", string.Format("The following driver pack(s) failed to import ({0}):", error.Count));
                UpdateActionStatus("ErrorDescription", SmsSummaryAction.ActionStatus.CompleteWithErrors);
                AddAction("ErrorInformation", string.Empty);
                foreach (KeyValuePair<string, string> item in error)
                {
                    AddActionDetailMessage("ErrorInformation", string.Format("{0}:\n - {1}", item.Key, item.Value));
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

            Regex regex = new Regex(@"Windows\s(\d\.\d|\d{2}|\d).*(\d{2})-bit");
            Match match = regex.Match(UserData["OS"].ToString());

            os = match.Success ? string.Format("Win{0}-{1}", match.Groups[1].Value, match.Groups[2].Value) : "unknown";
            structure = registry.ReadString("LegacyFolderStructure");
            sourceFolderPath = registry.ReadString("DriverSourceFolder");
            tempFolderPath = registry.ReadString("TempDownloadPath");

            ProcessCatalog();

            Utility.UpdateDataGridViewColumnsSize(dataGridViewDriverPackages, columnPack);
        }

        public override bool OnDeactivate()
        {
            dataGridViewDriverPackages.EndEdit();
            return base.OnDeactivate();
        }

        private void ProcessCatalog()
        {
            dataGridViewDriverPackages.Rows.Clear();

            bool known = false;
            string prefix = string.IsNullOrEmpty(registry.ReadString("HPFolderPrefix")) ? "HP" : registry.ReadString("HPFolderPrefix");

            string tempFile = Path.Combine(Path.GetTempPath(), "HPClientDriverPackCatalog.cab");
            using (FileStream innerCab = new FileStream(tempFile, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite))
            {
                CabEngine engine = new CabEngine();
                foreach (ArchiveFileInfo archiveFileInfo in engine.GetFileInfo(innerCab))
                {
                    using (Stream stream = engine.Unpack(innerCab, archiveFileInfo.Name))
                    {
                        catalog = XElement.Load(stream);

                        IEnumerable<XElement> nodeList = catalog.Element("HPClientDriverPackCatalog").Element("ProductOSDriverPackList").Elements("ProductOSDriverPack").Where(
                            x => x.Element("OSName").Value == UserData["OS"].ToString()
                            );
                        foreach (XElement node in nodeList)
                        {
                            HPDriverPackage package = new HPDriverPackage(node)
                            {
                                SoftPaq = catalog.Element("HPClientDriverPackCatalog").Element("SoftPaqList").Elements("SoftPaq").Where(
                                x => x.Element("Id").Value == node.Element("SoftPaqId").Value
                                ).FirstOrDefault(),
                                Vendor = prefix 
                            };
                            package.ProcessSoftPaq();
                            package.GenerateModelFolderName(os, structure);

                            DataGridViewRow dataGridViewRow = new DataGridViewRow();
                            dataGridViewRow.CreateCells(dataGridViewDriverPackages);

                            dataGridViewRow.Cells[0].Value = false;
                            dataGridViewRow.Cells[1].Value = package.Model;
                            dataGridViewRow.Cells[2].Value = package.VersionShort;
                            dataGridViewRow.Cells[3].Value = package.Size.ToString();

                            dataGridViewRow.Tag = package;
                            dataGridViewDriverPackages.Rows.Add(dataGridViewRow);
                        }
                    }
                }
            }

            // TODO: will implement when I have an HP device to test with
            //string query = "SELECT DISTINCT Model FROM SMS_G_System_COMPUTER_SYSTEM WHERE Manufacturer = 'HP' OR Manufacturer = 'Hewlett-Packard'";
            //List<IResultObject> models = Utility.SearchWMIToList(ConnectionManager, query);

            //foreach (IResultObject model in models)
            //{
            //    string testModel = model["Model"].StringValue;
            //    testModel = Regex.Replace(testModel, "HP", "", RegexOptions.IgnoreCase);
            //    testModel = Regex.Replace(testModel, "COMPAQ", "", RegexOptions.IgnoreCase);
            //    testModel = Regex.Replace(testModel, "SFF", "Small Form Factor");
            //    testModel = Regex.Replace(testModel, "USDT", "Desktop");
            //    testModel = Regex.Replace(testModel, " TWR", " Tower");
            //    testModel = testModel.TrimEnd("PC").Trim();

            //    DataGridViewRow row = dataGridViewDriverPackages.Rows
            //        .Cast<DataGridViewRow>()
            //        .Where(r => r.Cells[1].Value.ToString().Equals(testModel))
            //        .FirstOrDefault();

            //    if (row != null)
            //    {
            //        row.Cells[0].Value = true;
            //        row.Cells[4].Value = "Model detected";
            //        known = true;
            //    }
            //}

            foreach (DataGridViewRow dataGridViewRow in dataGridViewDriverPackages.Rows)
            {
                HPDriverPackage package = (HPDriverPackage)dataGridViewRow.Tag;

                string path = Path.Combine(sourceFolderPath, package.FolderName);

                if (Directory.Exists(path))
                {
                    string[] fileList = Directory.GetFiles(path, "*.version");
                    if (File.Exists(Path.Combine(path, package.VersionFile)))
                    {
                        dataGridViewRow.Cells[0].Value = false;
                        dataGridViewRow.Cells[4].Value = "Downloaded";
                    }
                    else if (fileList.Length > 0)
                    {
                        dataGridViewRow.Cells[0].Value = false;
                        dataGridViewRow.Cells[4].Value = "New version";
                    }
                }
            }

            dataGridViewDriverPackages.Sort(known ? columnStatus : columnPack, known ? ListSortDirection.Descending : ListSortDirection.Ascending);

            Initialized = true;
        }

        private void DownloadFiles()
        {
            if (_downloadUrls.Any())
            {
                KeyValuePair<HPDriverPackage, Uri> data = _downloadUrls.Dequeue();

                HPDriverPackage package = data.Key;

                Uri url = data.Value;
                string filename = Path.GetFileName(url.LocalPath);

                currentDownloadFileName = Path.Combine(tempFolderPath, filename);
                currentModel = package.Model;
                currentPackage = package;

                // check if file already exists and is the same size as the one that will be downloaded
                if (File.Exists(currentDownloadFileName))
                {
                    long fileSize = new FileInfo(currentDownloadFileName).Length;
                    long webSize = Utility.GetFileSize(url);

                    if (fileSize == webSize)
                    {
                        _extractFiles.Enqueue(new KeyValuePair<HPDriverPackage, string>(package, currentDownloadFileName));

                        CheckDownloadQueueCompleted();

                        return;
                    }
                }

                // enable https downloads
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                webClient = new WebClient();
                webClient.DownloadProgressChanged += Client_DownloadProgressChanged;
                webClient.DownloadFileCompleted += Client_DownloadFileCompleted;
                webClient.DownloadFileAsync(url, currentDownloadFileName);

                return;
            }
        }

        private void Client_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            // I hate calculating progress bars
            double bytesIn = double.Parse(e.BytesReceived.ToString());
            double totalBytes = double.Parse(e.TotalBytesToReceive.ToString());
            double percentage = bytesIn / totalBytes * 100;
            double start = 100 / totalPacks * downloadedPacks;
            backgroundWorker.ReportProgress(Convert.ToInt32((start + (percentage / totalPacks)) * 0.5),
                string.Format("Downloading: {0} - {1} of {2}", currentModel, ByteSize.FromBytes(bytesIn).ToString("#"), ByteSize.FromBytes(totalBytes).ToString("#")
                ));
        }

        private void Client_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            try
            {
                if (e.Error != null)
                {
                    error.Add(currentModel, "Could download driver pack: " + e.Error.Message);
                }
                else
                {
                    // add the downloaded file into the extraction list
                    _extractFiles.Enqueue(new KeyValuePair<HPDriverPackage, string>(currentPackage, currentDownloadFileName));
                }
            }
            finally
            {
                if (sender as WebClient == webClient)
                {
                    webClient.Dispose();
                    webClient = null;

                    CheckDownloadQueueCompleted();
                }
            }
        }

        private void CheckDownloadQueueCompleted()
        {
            // if queue is still active download next file
            if (_downloadUrls.Count > 0)
            {
                ++downloadedPacks;
                DownloadFiles();
            }
            else
            {
                // all downloads are done, kill of the while loop
                downloadQueueFinished = true;
            }
        }

        private void ExtractFiles()
        {
            if (_extractFiles.Any())
            {
                KeyValuePair<HPDriverPackage, string> data = _extractFiles.Dequeue();

                // add model so we can re-use it in functions
                HPDriverPackage package = data.Key;
                currentModel = package.Model;

                // update my best friend the progress bar
                backgroundWorker.ReportProgress(Convert.ToInt32(50 + ((50 / totalPacks * extracted) * 0.5)), string.Format("Processing {0} : extracting to temp folder", currentModel));
                // generate model folder name

                // hp sp extract does not work directly to network share, put in temp folder first and than copy to share
                string tempFolder = Path.Combine(tempFolderPath, "extract", package.FolderName);
                string destinationFolder = Path.Combine(sourceFolderPath, package.FolderName);

                using (extractProcess = new Process())
                {
                    try
                    {
                        // Start a process to print a file and raise an event when done.
                        extractProcess.StartInfo.FileName = data.Value;
                        extractProcess.StartInfo.Arguments = string.Format("-pdf -f \"{0}\" -s -e", tempFolder);
                        extractProcess.StartInfo.CreateNoWindow = true;
                        extractProcess.StartInfo.UseShellExecute = true;
                        extractProcess.StartInfo.Verb = "RunAs";
                        extractProcess.Start();
                        extractProcess.WaitForExit();
                        extractProcess.Close();

                        // update my best friend the progress bar
                        backgroundWorker.ReportProgress(Convert.ToInt32(50 + ((100 / totalPacks * extracted) * 0.5) - 1), string.Format("Processing {0} : moving to destination folder", currentModel));

                        if (!Directory.Exists(tempFolder))
                            throw new DirectoryNotFoundException("Temp folder not found " + tempFolder);

                        // add a version file to the folder so we can check later if there is an update to the download
                        string versionFile = Path.Combine(tempFolderPath, "extract", package.FolderName, package.VersionFile);
                        File.Create(versionFile).Close();

                        // wipe old soruce folder
                        if (registry.ReadBool("WipeSource"))
                            Directory.Delete(destinationFolder, true);

                        // remove old version file
                        if (Directory.Exists(destinationFolder))
                        {
                            string[] fileList = Directory.GetFiles(destinationFolder, "*.version");
                            foreach (string file in fileList)
                            {
                                File.Delete(file);
                            }
                        }

                        Utility.Copy(tempFolder, destinationFolder, true);

                        Directory.Delete(tempFolder, true);
                    }
                    catch (Exception ex)
                    {
                        error.Add(currentModel, "Cannot extract driver pack: " + ex.Message);

                        CheckExtractQueueCompleted();

                        return;
                    }
                }

                successful.Add(currentModel);

                CheckExtractQueueCompleted();

                return;
            }
        }

        private void CheckExtractQueueCompleted()
        {
            // if queue is still active download next file
            if (_extractFiles.Count > 0)
            {
                ExtractFiles();
            }
            else
            {
                // all downloads are done, kill of the while loop
                extractQueueFinished = true;
            }
        }

        private ControlDataState ValidateSelectedPack()
        {
            foreach (DataGridViewRow dataGridViewRow in dataGridViewDriverPackages.Rows)
            {
                if (Convert.ToBoolean(dataGridViewRow.Cells[0].Value, CultureInfo.InvariantCulture))
                {
                    return ControlDataState.Valid;
                }
            }
            return ControlDataState.Invalid;
        }

        private void DataGridViewDriverPackages_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (!dataGridViewDriverPackages.IsCurrentCellDirty)
                return;
            dataGridViewDriverPackages.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }

        private void DataGridViewDriverPackages_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            ControlsInspector.InspectAll();
            Dirty = !ReadOnly;
        }

        private void DataGridViewDriverPackages_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            Utility.UpdateDataGridViewColumnsSize(dataGridViewDriverPackages, columnPack);
        }

        private void ButtonDeselectAll_Click(object sender, EventArgs e)
        {
            dataGridViewDriverPackages.Focus();
            dataGridViewDriverPackages.BeginEdit(true);
            bool flag = sender == buttonDeselectAll;
            foreach (DataGridViewRow dataGridViewRow in dataGridViewDriverPackages.Rows)
                dataGridViewRow.Cells[0].Value = flag ? false : true;
            dataGridViewDriverPackages.EndEdit();
        }

        private void ContextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            if (dataGridViewDriverPackages.SelectedRows.Count == 0 || dataGridViewDriverPackages.SelectedRows.Count > 1)
                e.Cancel = true;
        }

        private void DataGridViewDriverPackages_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex != -1 && e.RowIndex != -1 && e.Button == MouseButtons.Right)
            {
                dataGridViewDriverPackages.ClearSelection();
                dataGridViewDriverPackages.Rows[e.RowIndex].Selected = true;
            }
        }

        private void DataGridViewDriverPackages_KeyUp(object sender, KeyEventArgs e)
        {
            Utility.SelectDataGridViewWithSpace(e, (DataGridView)sender, columnSelected);
        }

        private void ReleaseNotesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dataGridViewDriverPackages.SelectedRows)
            {
                if (row.Tag is HPDriverPackage package)
                {
                    Process process = new Process();
                    try
                    {
                        process.StartInfo.FileName = package.ReleaseNotesUrl;
                        process.StartInfo.UseShellExecute = true;
                        process.Start();
                    }
                    catch (Win32Exception) { }
                    finally
                    {
                        process?.Dispose();
                    }
                }
            }
        }
    }
}
