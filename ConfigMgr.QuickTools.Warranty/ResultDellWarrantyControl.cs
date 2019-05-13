using Microsoft.ConfigurationManagement.AdminConsole;
using Microsoft.ConfigurationManagement.AdminConsole.DialogFramework;
using Microsoft.ConfigurationManagement.ManagementProvider;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Text;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;
using System.Threading;

namespace ConfigMgr.QuickTools.Warranty
{
    public partial class ResultDellWarrantyControl : SmsPageControl
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ModifyRegistry registry = new ModifyRegistry();
        private BackgroundWorker backgroundWorker;

        public ResultDellWarrantyControl(SmsPageData pageData)
          : base(pageData)
        {
            InitializeComponent();
            buttonSURefresh.Image = new Icon(Properties.Resources.reload, new Size(16, 16)).ToBitmap();
            buttonOptions.Image = new Icon(Properties.Resources.options, new Size(16, 16)).ToBitmap();
            Title = "Dell Warranty";

            Updater.CheckUpdates();
        }

        public override void InitializePageControl()
        {
            base.InitializePageControl();

            if (string.IsNullOrEmpty(registry.ReadString("DellApiKey")) || string.IsNullOrEmpty(registry.ReadString("DellAPIURI")))
            {
                labelHttpResponse.Text = "No Dell TechDirect API key set in options";
                buttonSURefresh.Enabled = false;
            }
            else
            {
                buttonSURefresh.Enabled = true;
            }

            if (!PropertyManager["IsClient"].BooleanValue)
            {
                labelHttpResponse.Text = "No ConfigMgr client installed";
                buttonSURefresh.Enabled = false;
            }

            Initialized = true;
        }

        private void ButtonSURefresh_Click(object sender, EventArgs e)
        {
            listViewListWarranty.IsLoading = true;
            listViewListWarranty.UpdateColumnWidth(columnHeaderDescription);
            listViewListWarranty.Items.Clear();

            backgroundWorker = new BackgroundWorker();
            backgroundWorker.DoWork += new DoWorkEventHandler(InfoWorker_DoWorkAsync);
            backgroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(InfoWorker_RunWorkerCompleted);
            backgroundWorker.WorkerSupportsCancellation = false;
            backgroundWorker.WorkerReportsProgress = false;
            buttonSURefresh.Enabled = false;
            Cursor = Cursors.WaitCursor;
            backgroundWorker.RunWorkerAsync();
        }

        private void ButtonOptions_Click(object sender, EventArgs e)
        {
            ShowDialog("QuickToolsOptions", PropertyManager);

            Dictionary<string, object> dialogData = (Dictionary<string, object>)PropertyManager.UserDataObject;
            labelHttpResponse.Text = (string)dialogData["HttpResponse"];
            buttonSURefresh.Enabled = (bool)dialogData["RefreshEnabled"];

            if (!PropertyManager["IsClient"].BooleanValue)
            {
                labelHttpResponse.Text = "No ConfigMgr client installed";
                buttonSURefresh.Enabled = false;
            }
        }

        private async void InfoWorker_DoWorkAsync(object sender, DoWorkEventArgs e)
        {
            try
            {
                string query = string.Format("SELECT * FROM SMS_G_System_COMPUTER_SYSTEM WHERE ResourceID = '{0}'", PropertyManager["ResourceID"].IntegerValue);
                IResultObject contentObject = Utility.GetFirstWMIInstance(ConnectionManager, query);

                if (contentObject["Manufacturer"].StringValue == "Dell Inc.")
                {
                    labelModel.Text = contentObject["Model"].StringValue;

                    query = string.Format("SELECT * FROM SMS_G_System_PC_BIOS WHERE ResourceID = '{0}'", PropertyManager["ResourceID"].IntegerValue);
                    contentObject = Utility.GetFirstWMIInstance(ConnectionManager, query);
                    string serviceTag = contentObject["SerialNumber"].StringValue;
                    labelServiceTag.Text = serviceTag;

                    log.InfoFormat("Processing warranty request for service tag : {0}", serviceTag);

                    using (HttpClient client = new HttpClient())
                    {
                        try
                        {
                            labelHttpResponse.Text = "Requesting data from API";

                            client.BaseAddress = new Uri(registry.ReadString("DellAPIURI"));
                            client.DefaultRequestHeaders.Accept.Clear();
                            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));

                            log.InfoFormat("Connecting to dell API : {0}", client.BaseAddress);

                            HttpResponseMessage response = await client.GetAsync(string.Format("getassetwarranty/{0}?apikey={1}", serviceTag, registry.ReadString("DellApiKey")));
                            log.DebugFormat("URL : {0}", response.RequestMessage.RequestUri.OriginalString);
                            log.DebugFormat("Response status code : {0}", response.StatusCode);
                            if (response.IsSuccessStatusCode)
                            {
                                labelHttpResponse.Text = "Success";
                                string resultContent = response.Content.ReadAsStringAsync().Result;
                                XElement assetInformation = XElement.Parse(resultContent);
                                XNamespace ns = assetInformation.GetDefaultNamespace();

                                log.Info("Connected successfully, processing xml data");

                                CultureInfo cultureInfo = Thread.CurrentThread.CurrentCulture;
                                TextInfo textInfo = cultureInfo.TextInfo;

                                var headerData = assetInformation.Descendants(ns + "AssetHeaderData").First();

                                XElement node = headerData.Element(ns + "ShipDate");
                                if (node != null)
                                {
                                    string shipDate = DateTime.Parse(node.Value).ToString();
                                    labelShipDate.Text = shipDate;
                                }

                                IEnumerable<XElement> nodeList = assetInformation.Descendants(ns + "AssetEntitlement");
                                foreach (XElement entitlement in nodeList)
                                {
                                    string serviceLevelDescription = entitlement.Element(ns + "ServiceLevelDescription").Value;
                                    if (serviceLevelDescription != "")
                                    {
                                        string type = entitlement.Element(ns + "EntitlementType").Value;
                                        listViewListWarranty.Items.Add(new ListViewItem()
                                        {
                                            Text = serviceLevelDescription,
                                            SubItems = {
                                                textInfo.ToTitleCase(type.ToLower()),
                                                DateTime.Parse(entitlement.Element(ns + "StartDate").Value).ToShortDateString(),
                                                DateTime.Parse(entitlement.Element(ns + "EndDate").Value).ToShortDateString()
                                            }
                                        });
                                    }
                                }
                            }
                            else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                            {
                                labelHttpResponse.Text = "Unauthorized - Check API Key";
                                log.Warn(labelHttpResponse.Text);
                            }
                            else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                            {
                                labelHttpResponse.Text = "Resource Not found";
                                log.Warn(labelHttpResponse.Text);
                            }
                        }
                        catch (HttpRequestException ex)
                        {
                            string msg = string.Format("{0}: {1}", ex.GetType().Name, ex.Message);
                            log.Error(msg);
                            throw new InvalidOperationException(msg);
                        }

                        log.InfoFormat("Finished processing request for service tag : {0}", serviceTag);
                    }
                }
                else
                {
                    labelServiceTag.Text = "The device is not manufacutred by Dell Inc";
                    labelModel.Text = contentObject["Manufacturer"].StringValue;
                }

                contentObject.Dispose();
            }
            catch (Exception ex)
            {
                string msg = string.Format("{0}: {1}", ex.GetType().Name, ex.Message);
                log.Error(msg);
                throw new InvalidOperationException(msg);
            }
        }

        private void InfoWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
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
            }
            finally
            {
                if (sender as BackgroundWorker == backgroundWorker)
                {
                    backgroundWorker.Dispose();
                    backgroundWorker = null;
                    Cursor = Cursors.Default;
                    listViewListWarranty.IsLoading = false;
                    listViewListWarranty.UpdateColumnWidth(columnHeaderDescription);
                    listViewListWarranty.Sorting = SortOrder.Ascending;
                    buttonSURefresh.Enabled = true;
                }
            }
        }

        private void ListViewListSoftwareUpdates_CopyKeyEvent(object sender, EventArgs e)
        {
            StringBuilder buffer = new StringBuilder();
            foreach (ListViewItem item in listViewListWarranty.SelectedItems)
            {
                foreach (ListViewItem.ListViewSubItem subitem in item.SubItems)
                {
                    buffer.Append(subitem.Text);
                    buffer.Append("\t");
                }
                buffer.AppendLine();
            }
            buffer.Remove(buffer.Length - 1, 1);
            Clipboard.SetData(DataFormats.Text, buffer.ToString());
        }
    }
}
