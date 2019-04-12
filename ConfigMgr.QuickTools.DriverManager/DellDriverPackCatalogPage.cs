using ByteSizeLib;
using Microsoft.ConfigurationManagement.AdminConsole;
using Microsoft.ConfigurationManagement.ManagementProvider;
using Microsoft.Deployment.Compression;
using Microsoft.Deployment.Compression.Cab;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        private ModifyRegistry registry = new ModifyRegistry();
        private WebClient webClient;
        private Queue<KeyValuePair<string, Uri>> _downloadUrls = new Queue<KeyValuePair<string, Uri>>();
        private bool queueFinished = false;
        private Dictionary<string, string> error = new Dictionary<string, string>();
        private List<string> successful = new List<string>();
        private int totalPacks;
        private int downloadedPacks = 0;
        private string currentDownloadFileName;
        private string currentDownloadModel;
        private Dictionary<string, string> cabs = new Dictionary<string, string>();
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

                List<XElement> packages = (List<XElement>)UserData["DriverPackItems"];
                totalPacks = packages.Count;

                foreach (XElement package in packages)
                {
                    XNamespace ns = package.GetDefaultNamespace();
                    XElement result = package.Element(ns + "SupportedSystems").Element(ns + "Brand").Element(ns + "Model");

                    Uri uri = new Uri(string.Format("http://{0}/{1}", UserData["baseLocation"], package.Attribute("path").Value));
                    //Uri uri = new Uri("http://github.com/Flowman/pxc-alpine/releases/download/5.7.22-29.26/percona-xtradb-cluster-dev-5.7.22-r0.apk");

                    string filename = Path.GetFileName(uri.LocalPath);
                    _downloadUrls.Enqueue(new KeyValuePair<string, Uri>(result.Attribute("name").Value, uri));
                }

                DownloadFiles();

                do // wait for all the download to finish
                {
                    Thread.Sleep(1000);
                } while (queueFinished == false);

                worker.ReportProgress(50, "Extracting Driver Packs ...");

                string destination = registry.ReadString("DriverSourceFolder");
                int num = 0;
                foreach (KeyValuePair<string, string> item in cabs)
                {
                    try
                    {
                        // still hate to create progress bars
                        double start = 100 / totalPacks * num;
                        worker.ReportProgress(Convert.ToInt32(50 + (start * 0.5)), string.Format("Extracting: {0}", item.Key));

                        string os = string.Format("Win{0}", UserData["OS"].ToString().Split(' ')[1].Replace(".", ""));
                        string folderName = null;

                        string structure = registry.ReadString("LegacyFolderStructure");

                        if (string.IsNullOrEmpty(structure) ? false : Convert.ToBoolean(structure))
                        {
                            folderName = string.Format(@"{0}\{1}\{2}-{3}", "Dell Inc", item.Key, os, UserData["Architecture"].ToString());
                        }
                        else
                        {
                            folderName = string.Format(@"{0}-{1}-{2}-{3}", "Dell Inc", item.Key, os, UserData["Architecture"].ToString());
                        }

                        string destinationFolder = Path.Combine(destination, folderName);

                        var cab = new CabInfo(item.Value);
                        cab.Unpack(destinationFolder);
                        successful.Add(item.Key);
                    }
                    catch (Exception ex)
                    {
                        error.Add(item.Key, "Cannot extract driver pack: " + ex.Message);
                    }
                    ++num;
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

            }
        }

        public override void OnAddSummary(SummaryRequestHandler handler)
        {
            base.OnAddSummary(handler);

            List<XElement> list = new List<XElement>();
            foreach (DataGridViewRow dataGridViewRow in dataGridViewDriverPackages.Rows)
            {
                if (Convert.ToBoolean(dataGridViewRow.Cells[columnImport.Name].Value) == true)
                {
                    if (dataGridViewRow.Tag is XElement pack)
                    {
                        list.Add(pack);
                    }
                }
            }
            UserData["DriverPackItems"] = list;

            AddAction("GeneralDescription", string.Format("The following driver pack(s) will be imported ({0}):", list.Count));
            AddAction("DriverPackInformation", string.Empty);

            foreach (XElement package in list)
            {
                XNamespace ns = package.GetDefaultNamespace();
                XElement result = package.Element(ns + "SupportedSystems").Element(ns + "Brand").Element(ns + "Model");
                AddActionDetailMessage("DriverPackInformation", result.Attribute("name").Value);
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
            string tempFile = Path.Combine(Path.GetTempPath(), "DriverPackCatalog.cab");
            using (FileStream innerCab = new FileStream(tempFile, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite))
            {
                CabEngine engine = new CabEngine();
                foreach (ArchiveFileInfo archiveFileInfo in engine.GetFileInfo(innerCab))
                {
                    using (Stream stream = engine.Unpack(innerCab, archiveFileInfo.Name))
                    {
                        XElement catalog = XElement.Load(stream);
                        XNamespace ns = catalog.GetDefaultNamespace();
                        // Get download base location from DriverPackCatalog.xml 
                        UserData["baseLocation"] = catalog.Attribute("baseLocation").Value;

                        IEnumerable<XElement> nodeList = catalog.Elements(ns + "DriverPackage").Where(
                            x => x.Element(ns + "SupportedOperatingSystems").Element(ns + "OperatingSystem").Attribute("osArch").Value == UserData["Architecture"].ToString() &&
                            x.Element(ns + "SupportedOperatingSystems").Element(ns + "OperatingSystem").Attribute("osCode").Value == UserData["OS"].ToString().Replace(" ", string.Empty)
                            );
                        foreach (XElement package in nodeList)
                        {
                            XElement result = package.Element(ns + "SupportedSystems").Element(ns + "Brand").Element(ns + "Model");
                            string model = result.Attribute("name").Value;
                            DataGridViewRow dataGridViewRow = new DataGridViewRow();
                            dataGridViewRow.CreateCells(dataGridViewDriverPackages);

                            dataGridViewRow.Cells[0].Value = false;
                            dataGridViewRow.Cells[1].Value = model;
                            dataGridViewRow.Cells[2].Value = package.Attribute("dellVersion").Value;

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
                    row.Cells[3].Value = "Model detected";
                    known = true;
                }
            }

            dataGridViewDriverPackages.Sort(known ? columnStatus : columnPack, known ? ListSortDirection.Descending : ListSortDirection.Ascending);

            Initialized = true;
        }

        private void DownloadFiles()
        {
            if (_downloadUrls.Any())
            {
                KeyValuePair<string, Uri> data = _downloadUrls.Dequeue();
                Uri url = data.Value;
                string filename = Path.GetFileName(url.LocalPath);

                currentDownloadFileName = Path.Combine(registry.ReadString("TempDownloadPath"), filename);
                currentDownloadModel = data.Key;
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
            backgroundWorker.ReportProgress(Convert.ToInt32((start + (percentage / totalPacks)) * 0.5), string.Format("Downloading: {0} - {1} of {2}", new object[3]
                {
                    currentDownloadModel,
                    ByteSize.FromBytes(bytesIn).ToString("#"),
                    ByteSize.FromBytes(totalBytes).ToString("#")
                }));
        }

        private void Client_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            try
            {
                if (e.Error != null)
                {
                    error.Add(currentDownloadModel, "Could download driver pack: " + e.Error.Message);
                }
                else
                {
                    // add the downloaded file into the extraction list
                    cabs.Add(currentDownloadModel, currentDownloadFileName);
                }
            }
            finally
            {
                if (sender as WebClient == webClient)
                {
                    webClient.Dispose();
                    webClient = null;
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
    }
}
