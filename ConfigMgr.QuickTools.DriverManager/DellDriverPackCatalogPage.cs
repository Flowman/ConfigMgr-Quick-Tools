using ByteSizeLib;
using Microsoft.ConfigurationManagement.AdminConsole;
using Microsoft.ConfigurationManagement.ManagementProvider;
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
using System.Threading;
using System.Windows.Forms;
using System.Xml.Linq;

namespace ConfigMgr.QuickTools.DriverManager
{
    public partial class DellDriverPackCatalogPage : SmsPageControl
    {
        #region Private
        private BackgroundWorker backgroundWorker;
        private readonly ModifyRegistry registry = new ModifyRegistry();
        private XElement catalog;
        private WebClient webClient;
        private readonly Queue<KeyValuePair<DellDriverPackage, Uri>> _downloadUrls = new Queue<KeyValuePair<DellDriverPackage, Uri>>();
        private bool queueFinished = false;
        private readonly Queue<KeyValuePair<DellDriverPackage, string>> _extractFiles = new Queue<KeyValuePair<DellDriverPackage, string>>();
        private bool extractQueueFinished = false;
        private readonly Dictionary<string, string> error = new Dictionary<string, string>();
        private readonly List<string> successful = new List<string>();
        private int totalPacks;
        private int downloadedPacks = 0;
        private readonly int extracted = 1;
        private string currentDownloadFileName;
        private string currentModel;
        private DellDriverPackage currentPackage;
        private string os;
        private string structure;
        private string tempFolderPath;
        private string sourceFolderPath;
        #endregion

        public DellDriverPackCatalogPage(SmsPageData pageData)
            : base(pageData)
        {
            Title = "Select Driver Pack";
            Headline = "Select driver pack to download";

            InitializeComponent();
        }

        public override void InitializePageControl()
        {
            base.InitializePageControl();

            ControlsInspector.AddControl(dataGridViewDriverPackages, new ControlDataStateEvaluator(ValidateSelectedPack), "Select driver pack to download");

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

                List<DellDriverPackage> packages = (List<DellDriverPackage>)UserData["DriverPackItems"];
                totalPacks = packages.Count;

                foreach (DellDriverPackage package in packages)
                {
                    Uri uri = new Uri(string.Format("http://{0}/{1}", UserData["baseLocation"], package.Url));

                    string filename = Path.GetFileName(uri.LocalPath);
                    _downloadUrls.Enqueue(new KeyValuePair<DellDriverPackage, Uri>(package, uri));
                }

                DownloadFiles();

                do // wait for all the download to finish
                {
                    Thread.Sleep(1000);
                } while (queueFinished == false);

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

            List<DellDriverPackage> list = new List<DellDriverPackage>();
            foreach (DataGridViewRow dataGridViewRow in dataGridViewDriverPackages.Rows)
            {
                if (Convert.ToBoolean(dataGridViewRow.Cells[columnImport.Name].Value) == true)
                {
                    if (dataGridViewRow.Tag is DellDriverPackage pack)
                    {
                        list.Add(pack);
                    }
                }
            }
            UserData["DriverPackItems"] = list;

            AddAction("GeneralDescription", string.Format("The following driver pack(s) will be imported ({0}):", list.Count));
            AddAction("DriverPackInformation", string.Empty);

            foreach (DellDriverPackage package in list)
            {
                AddActionDetailMessage("DriverPackInformation", package.Name);
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

            os = string.Format("Win{0}-{1}", UserData["OS"].ToString().Split(' ')[1].Replace(".", ""), UserData["Architecture"].ToString());

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
            string prefix = string.IsNullOrEmpty(registry.ReadString("DellFolderPrefix")) ? "Dell" : registry.ReadString("DellFolderPrefix");

            string tempFile = Path.Combine(Path.GetTempPath(), "DriverPackCatalog.cab");
            using (FileStream innerCab = new FileStream(tempFile, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite))
            {
                CabEngine engine = new CabEngine();
                foreach (ArchiveFileInfo archiveFileInfo in engine.GetFileInfo(innerCab))
                {
                    using (Stream stream = engine.Unpack(innerCab, archiveFileInfo.Name))
                    {
                        catalog = XElement.Load(stream);
                        XNamespace ns = catalog.GetDefaultNamespace();
                        // Get download base location from DriverPackCatalog.xml 
                        UserData["baseLocation"] = catalog.Attribute("baseLocation").Value;

                        IEnumerable<XElement> nodeList = catalog.Elements(ns + "DriverPackage").Where(
                            x => x.Element(ns + "SupportedOperatingSystems").Element(ns + "OperatingSystem").Attribute("osArch").Value == UserData["Architecture"].ToString() &&
                            x.Element(ns + "SupportedOperatingSystems").Element(ns + "OperatingSystem").Attribute("osCode").Value == UserData["OS"].ToString().Replace(" ", string.Empty)
                            );
                        foreach (XElement node in nodeList)
                        {
                            DellDriverPackage package = new DellDriverPackage(node)
                            {
                                Vendor = prefix
                            };
                            package.GenerateModelFolderName(os, structure);

                            DataGridViewRow dataGridViewRow = new DataGridViewRow();
                            dataGridViewRow.CreateCells(dataGridViewDriverPackages);

                            dataGridViewRow.Cells[0].Value = false;
                            dataGridViewRow.Cells[1].Value = package.Model;
                            dataGridViewRow.Cells[2].Value = package.Version;
                            dataGridViewRow.Cells[3].Value = package.Size.ToString();

                            dataGridViewRow.Tag = package;
                            dataGridViewDriverPackages.Rows.Add(dataGridViewRow);
                        }
                    }
                }
            }

            string query = "SELECT DISTINCT Model FROM SMS_G_System_COMPUTER_SYSTEM WHERE Manufacturer = 'Dell Inc.'";
            List<IResultObject> models = Utility.SearchWMIToList(ConnectionManager, query);

            foreach (IResultObject model in models)
            {
                DataGridViewRow row = dataGridViewDriverPackages.Rows
                    .Cast<DataGridViewRow>()
                    .Where(r => r.Cells[1].Value.ToString().Equals(model["Model"].StringValue))
                    .FirstOrDefault();

                if (row != null)
                {
                    row.Cells[0].Value = true;
                    row.Cells[4].Value = "Model detected";
                    known = true;
                }
            }

            foreach (DataGridViewRow dataGridViewRow in dataGridViewDriverPackages.Rows)
            {
                DellDriverPackage package = (DellDriverPackage)dataGridViewRow.Tag;

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
                KeyValuePair<DellDriverPackage, Uri> data = _downloadUrls.Dequeue();

                DellDriverPackage package = data.Key;
                Uri url = data.Value;

                string filename = Path.GetFileName(url.LocalPath);

                currentDownloadFileName = Path.Combine(registry.ReadString("TempDownloadPath"), filename);
                currentModel = package.Model;
                currentPackage = package;

                // check if file already exists and is the same size as the one that will be downloaded
                if (File.Exists(currentDownloadFileName))
                {
                    long fileSize = new FileInfo(currentDownloadFileName).Length;
                    long webSize = Utility.GetFileSize(url);

                    if (fileSize == webSize)
                    {
                        _extractFiles.Enqueue(new KeyValuePair<DellDriverPackage, string>(package, currentDownloadFileName));

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
                    _extractFiles.Enqueue(new KeyValuePair<DellDriverPackage, string>(currentPackage, currentDownloadFileName));
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
                queueFinished = true;
            }
        }

        private void ExtractFiles()
        {
            if (_extractFiles.Any())
            {
                KeyValuePair<DellDriverPackage, string> data = _extractFiles.Dequeue();

                // add model so we can re-use it in functions
                DellDriverPackage package = data.Key;
                currentModel = package.Model;

                // update my best friend the progress bar
                backgroundWorker.ReportProgress(Convert.ToInt32(50 + ((40 / totalPacks * extracted) * 0.5)), string.Format("Processing {0} : extracting to temp folder", currentModel));
                // generate model folder name

                // hp sp extract does not work directly to network share, put in temp folder first and than copy to share
                string tempFolder = Path.Combine(tempFolderPath, "extract", package.FolderName);
                string destinationFolder = Path.Combine(sourceFolderPath, package.FolderName);

                try
                {
                    var cab = new CabInfo(data.Value);
                    cab.Unpack(tempFolder);

                    // update my best friend the progress bar
                    backgroundWorker.ReportProgress(Convert.ToInt32(50 + ((80 / totalPacks * extracted) * 0.5)), string.Format("Processing {0} : moving to destination folder", currentModel));

                    if (!Directory.Exists(tempFolder))
                        throw new DirectoryNotFoundException("Temp folder not found " + tempFolder);

                    // add a version file to the folder so we can check later if there is an update to the download
                    string versionFile = Path.Combine(tempFolderPath, "extract", package.FolderName, package.VersionFile);
                    File.Create(versionFile).Close();

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
                    error.Add(package.Model, "Cannot extract driver pack: " + ex.Message);

                    CheckExtractQueueCompleted();

                    return;
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

        private void ReleaseNotesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dataGridViewDriverPackages.SelectedRows)
            {
                if (row.Tag is DellDriverPackage package)
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
