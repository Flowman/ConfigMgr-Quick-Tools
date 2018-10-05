using Microsoft.ConfigurationManagement.AdminConsole;
using Microsoft.ConfigurationManagement.ManagementProvider;
using Microsoft.ConfigurationManagement.AdminConsole.Schema;
using System;
using System.Windows.Forms;
using System.Threading;
using System.Management;
using System.Runtime.InteropServices;
using System.Net;
using System.ComponentModel;
using System.IO;
using Microsoft.Deployment.Compression.Cab;
using Microsoft.Deployment.Compression;
using System.Text;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Linq;
using Microsoft.ConfigurationManagement.AdminConsole.DialogFramework;

namespace Zetta.ConfigMgr.QuickTools
{
    public partial class ProvisioningModeDialog : SmsCustomDialog
    {
        private IResultObject resultObjects;
        private int completed;
        private int other;
        private int total;
        private bool mode;
        private Queue<string> _downloadUrls = new Queue<string>();

        public delegate void BarDelegate();

        public ProvisioningModeDialog(IResultObject selectedResultObjects, ActionDescription action, bool selectedMode)
        {
            InitializeComponent();
            resultObjects = selectedResultObjects;
            mode = selectedMode;
            Title = action.DisplayName;
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void ClientActionsDialog_Shown(object sender, EventArgs e)
        {

            Uri DellXMLCabinetSource = new Uri("http://downloads.dell.com/catalog/DriverPackCatalog.cab");
            string tempFile = Path.Combine(Path.GetTempPath(), "DriverPackCatalog.cab");

            WebClient webClient = new WebClient();
            webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(Completed);
            webClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(ProgressChanged);
            webClient.DownloadFileAsync(DellXMLCabinetSource, tempFile);

            //listViewHosts.IsLoading = true;
            //listViewHosts.Items.Clear();

            //foreach (IResultObject resultObject in resultObjects)
            //{
            //    if (!resultObject["IsClient"].BooleanValue)
            //        continue;

            //    ++total;
            //    listViewHosts.Items.Add(new ListViewItem()
            //    {
            //        Text = resultObject["Name"].StringValue,
            //        SubItems = { "Queued" }
            //    });
            //    ThreadPool.QueueUserWorkItem(arg => { ClientAction(resultObject); });
            //}

            //labelTotal.Text = total.ToString();
            //progressBar1.Maximum = total;
            //progressBar1.Minimum = 0;

            //listViewHosts.IsLoading = false;
        }

        private void ClientAction(IResultObject resultObject)
        {
            ListViewItem item = listViewHosts.FindItemWithText(resultObject["Name"].StringValue);

            try
            {
                item.SubItems[1].Text = "Connecting";
                ObjectGetOptions o = new ObjectGetOptions
                {
                    Timeout = new TimeSpan(0, 0, 5)
                };
                if (mode)
                {
                    using (ManagementClass clientaction = new ManagementClass(string.Format(@"\\{0}\root\{1}", resultObject["Name"].StringValue, "ccm"), "SMS_Client", o))
                    {
                        object[] methodArgs = { "False" };
                        clientaction.InvokeMethod("SetClientProvisioningMode", methodArgs);
                    }
                    item.SubItems[1].Text = "Completed";
                }
                else
                {
                    //using (ManagementClass registry = new ManagementClass(string.Format(@"\\{0}\root\{1}", resultObject["Name"].StringValue, "cimv2"), "StdRegProv", o))
                    //{
                    //    ManagementBaseObject inParams = registry.GetMethodParameters("GetStringValue");
                    //    inParams["hDefKey"] = 0x80000002;
                    //    inParams["sSubKeyName"] = @"SOFTWARE\Microsoft\CCM\CcmExec";
                    //    inParams["sValueName"] = "ProvisioningMode";

                    //    ManagementBaseObject outParams = registry.InvokeMethod("GetStringValue", inParams, null);

                    //    if (outParams.Properties["sValue"].Value != null)
                    //    {
                    //        item.SubItems[1].Text = outParams.Properties["sValue"].Value.ToString();
                    //    }
                    //    else
                    //    {
                    //        item.SubItems[1].Text = "No value";
                    //    }
                    //}
                }
                ++completed;
            }
            catch (ManagementException ex)
            {
                ExceptionUtilities.TraceException(ex);
                item.SubItems[1].Text = "WMI error: " + ex.Message;
                ++other;
            }
            catch (COMException ex)
            {
                ExceptionUtilities.TraceException(ex);
                item.SubItems[1].Text = "Offline";
                ++other;
            }
            catch (Exception ex)
            {
                ExceptionUtilities.TraceException(ex);
                item.SubItems[1].Text = "Error: " + ex.Message;
                ++other;
            }
            finally
            {
                try
                {
                    Invoke(new BarDelegate(UpdateBar));
                }
                catch { }
            }
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
                    x => x.Element(ns + "SupportedOperatingSystems").Element(ns + "OperatingSystem").Attribute("osArch").Value == "x64" &&
                    x.Element(ns + "SupportedOperatingSystems").Element(ns + "OperatingSystem").Attribute("osCode").Value == "Windows10"
                    );
                foreach (XElement package in nodeList)
                {
                    XElement result = package.Element(ns + "SupportedSystems").Element(ns + "Brand").Element(ns + "Model");
                    string model = result.Attribute("name").Value;
                    listViewHosts.Items.Add(new ListViewItem()
                    {
                        Text = model,
                        SubItems = { "Queued" },
                        Tag = package
                    });
                }
            }

            string query = "SELECT DISTINCT Model FROM SMS_G_System_COMPUTER_SYSTEM WHERE Manufacturer = 'Dell Inc.'";
            List<IResultObject> models = Utility.SearchWMIToList(resultObjects.ConnectionManager, query);

            string download = null;

            foreach (IResultObject model in models)
            {
                ListViewItem item = listViewHosts.FindItemWithText(model["Model"].StringValue);

                if (item != null)
                {
                    item.SubItems[1].Text = "Found";
                    item.Selected = true;
                    XElement package = (XElement)item.Tag;
                    download = package.Attribute("path").Value;
                }
            }

            if (download != null)
            {
                try
                {
                    Uri uri = new Uri(string.Format("http://downloads.dell.com/{0}", download));
                    string filename = Path.GetFileName(uri.LocalPath);

                    using (var client = new WebClient())
                    {
                        client.DownloadFile(uri, "c:/temp/" + filename);
                    }

                }
                catch (WebException ex)
                {
                    ExceptionUtilities.TraceException(ex);
                    SccmExceptionDialog.ShowDialog(Microsoft.ConfigurationManagement.AdminConsole.SnapIn.Console, ex, "An error occured while downloading the file.");
                }
            }
        

            listViewHosts.Sorting = SortOrder.Ascending;

            MessageBox.Show("Download completed!");
        }

        private void UpdateBar()
        {
            progressBar1.Value++;
            labelCompleted.Text = completed.ToString();
            labelOther.Text = other.ToString();
            labelTotal.Text = total.ToString();
            if (progressBar1.Value == progressBar1.Maximum)
            {
                buttonOK.Enabled = true;
            }
        }

        private void DownloadFile()
        {
            if (_downloadUrls.Any())
            {
                WebClient client = new WebClient();
                client.DownloadProgressChanged += client_DownloadProgressChanged;
                client.DownloadFileCompleted += client_DownloadFileCompleted;

                var url = _downloadUrls.Dequeue();
                string FileName = url.Substring(url.LastIndexOf("/") + 1,
                            (url.Length - url.LastIndexOf("/") - 1));

                client.DownloadFileAsync(new Uri(url), "C:\\Test4\\" + FileName);
                lblFileName.Text = url;
                return;
            }

            // End of the download
            btnGetDownload.Text = "Download Complete";
        }

        private void client_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                // handle error scenario
                throw e.Error;
            }
            if (e.Cancelled)
            {
                // handle cancelled scenario
            }
            DownloadFile();
        }

        void client_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            double bytesIn = double.Parse(e.BytesReceived.ToString());
            double totalBytes = double.Parse(e.TotalBytesToReceive.ToString());
            double percentage = bytesIn / totalBytes * 100;
            progressBar1.Value = int.Parse(Math.Truncate(percentage).ToString());
        }

    }
}
