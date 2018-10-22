using Microsoft.ConfigurationManagement.AdminConsole;
using Microsoft.ConfigurationManagement.ManagementProvider;
using Microsoft.ConfigurationManagement.AdminConsole.Schema;
using System.Windows.Forms;
using System;
using System.IO;
using System.Net;
using System.ComponentModel;
using Microsoft.Deployment.Compression.Cab;
using Microsoft.Deployment.Compression;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Linq;
using Microsoft.ConfigurationManagement.AdminConsole.DialogFramework;

namespace Zetta.ConfigMgr.QuickTools
{
    public partial class S : SmsCustomDialog
    {
        private Queue<Uri> _downloadUrls = new Queue<Uri>();
        private IResultObject resultObject;
        private static WebClient client = new WebClient();

        public S(IResultObject selectedResultObject)
        {
            resultObject = selectedResultObject;
            InitializeComponent();
        }

        public static void Show(object sender, ScopeNode scopeNode, ActionDescription action, IResultObject selectedResultObject, PropertyDataUpdated dataUpdatedDelegate, Status status)
        {
            new S(selectedResultObject).Show();
        }

        private void DriverGrabberDialog_Shown(object sender, EventArgs e)
        {
            Uri DellXMLCabinetSource = new Uri("http://downloads.dell.com/catalog/DriverPackCatalog.cab");
            string tempFile = Path.Combine(Path.GetTempPath(), "DriverPackCatalog.cab");

            WebClient webClient = new WebClient();
            webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(Completed);
            webClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(ProgressChanged);
            webClient.DownloadFileAsync(DellXMLCabinetSource, tempFile);
        }

        private void ProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
        }

        private void Completed(object sender, AsyncCompletedEventArgs e)
        {
            string tempFile = Path.Combine(Path.GetTempPath(), "DriverPackCatalog.cab");
            FileStream innerCab = new FileStream(tempFile, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);

            CabEngine engine = new CabEngine();
            foreach (ArchiveFileInfo archiveFileInfo in engine.GetFileInfo(innerCab))
            {
                Stream stream = engine.Unpack(innerCab, archiveFileInfo.Name);

                XElement catalog = XElement.Load(stream);
                XNamespace ns = catalog.GetDefaultNamespace();

                IEnumerable<XElement> nodeList = catalog.Elements(ns + "DriverPackage").Where(
                    x => x.Element(ns + "SupportedOperatingSystems").Element(ns + "OperatingSystem").Attribute("osArch").Value == comboBoxArch.Text &&
                    x.Element(ns + "SupportedOperatingSystems").Element(ns + "OperatingSystem").Attribute("osCode").Value == comboBoxOS.Text
                    );
                foreach (XElement package in nodeList)
                {
                    XElement result = package.Element(ns + "SupportedSystems").Element(ns + "Brand").Element(ns + "Model");
                    string model = result.Attribute("name").Value;
                    DataGridViewRow dataGridViewRow = new DataGridViewRow();
                    dataGridViewRow.CreateCells(dataGridViewDriverPackages);

                    dataGridViewRow.Cells[0].Value = false;
                    dataGridViewRow.Cells[1].Value = model;
                    dataGridViewRow.Cells[2].Value = "Queued";

                    dataGridViewRow.Tag = package;
                    dataGridViewDriverPackages.Rows.Add(dataGridViewRow);
                }
            }

            string query = "SELECT DISTINCT Model FROM SMS_G_System_COMPUTER_SYSTEM WHERE Manufacturer = 'Dell Inc.'";
            List<IResultObject> models = Utility.SearchWMIToList(resultObject.ConnectionManager, query);

            foreach (IResultObject model in models)
            {
                DataGridViewRow row = dataGridViewDriverPackages.Rows
                    .Cast<DataGridViewRow>()
                    .Where(r => r.Cells[1].Value.ToString().Equals(model["Model"].StringValue))
                    .FirstOrDefault();

                if (row != null)
                {
                    row.Cells[2].Value = "Found";
                }
            }
        }

        private void DownloadFile()
        {
            if (_downloadUrls.Any())
            {

                client.DownloadProgressChanged += Client_DownloadProgressChanged;
                client.DownloadFileCompleted += Client_DownloadFileCompleted;

                Uri url = _downloadUrls.Dequeue();
                string filename = Path.GetFileName(url.LocalPath);


                client.DownloadFileAsync(url, "C:\\Test\\" + filename);

                labelStatus.Text = "Downloading: " + filename;

                return;
            }
            labelStatus.Text = "Download complete";
        }

        private void Client_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                using (SccmExceptionDialog sccmExceptionDialog = new SccmExceptionDialog(e.Error))
                {
                    sccmExceptionDialog.ShowDialog();
                }
            }
            else
            {
                //add file to extract list
            }
            if (e.Cancelled)
            {
                ConnectionManagerBase.SmsTraceSource.TraceInformation("User canceled download files");
                labelStatus.Text = "Download canceled";
            }
            else
            {
                DownloadFile();
            }
        }

        private void Client_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            double bytesIn = double.Parse(e.BytesReceived.ToString());
            double totalBytes = double.Parse(e.TotalBytesToReceive.ToString());
            double percentage = bytesIn / totalBytes * 100;
            progressBar1.Value = int.Parse(Math.Truncate(percentage).ToString());
        }

        private void button1_Click(object sender, EventArgs e)
        {

            foreach (DataGridViewRow dataGridViewRow in dataGridViewDriverPackages.Rows)
            {
                if (Convert.ToBoolean(dataGridViewRow.Cells[columnImport.Name].Value) == true)
                {
                    XElement package = dataGridViewRow.Tag as XElement;
                    Uri uri = new Uri(string.Format("http://downloads.dell.com/{0}", package.Attribute("path").Value));

                    _downloadUrls.Enqueue(uri);
                }
            }

            // Starts the download
            progressBar1.Value = 0;

            DownloadFile();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (client != null)
            {
                client.CancelAsync();
            }
        }
    }
}
