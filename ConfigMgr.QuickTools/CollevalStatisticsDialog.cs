using Microsoft.ConfigurationManagement.AdminConsole;
using Microsoft.ConfigurationManagement.ManagementProvider;
using Microsoft.ConfigurationManagement.AdminConsole.Schema;
using Microsoft.ConfigurationManagement.AdminConsole.DialogFramework;
using System.Windows.Forms;
using System;
using System.Management;
using System.ComponentModel;
using System.IO;
using System.Text.RegularExpressions;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Globalization;

namespace ConfigMgr.QuickTools
{
    public partial class CollevalStatisticsDialog : SmsCustomDialog
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private BackgroundWorker backgroundWorker;
        private readonly ConnectionManagerBase connectionManager;

        public CollevalStatisticsDialog(ActionDescription action, ScopeNode scopeNode)
        {
            InitializeComponent();

            Title = action.DisplayName;

            Updater.CheckUpdates();

            connectionManager = Microsoft.ConfigurationManagement.AdminConsole.UtilityClass.ConnectionManagerFromScope(scopeNode, "WQL");

            progressBar1.Style = ProgressBarStyle.Marquee;

            listViewListCollections.UpdateColumnWidth(columnHeaderName);
            listViewListCollections.Items.Clear();
            listViewListCollections.IsLoading = true;
        }

        public static void Show(object sender, ScopeNode scopeNode, ActionDescription action, IResultObject selectedObject, PropertyDataUpdated dataUpdatedDelegate, Status Status)
        {
            using (CollevalStatisticsDialog dialog = new CollevalStatisticsDialog(action, scopeNode))
            {
                dialog.ShowDialog();
                return;
            }
        }

        private void CollevalStatisticsDialog_Shown(object sender, EventArgs e)
        {
            backgroundWorker = new BackgroundWorker();
            backgroundWorker.DoWork += new DoWorkEventHandler(InfoWorker_DoWork);
            backgroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(InfoWorker_RunWorkerCompleted);
            backgroundWorker.WorkerSupportsCancellation = false;
            backgroundWorker.WorkerReportsProgress = false;
            UseWaitCursor = true;
            backgroundWorker.RunWorkerAsync();
        }

        private void InfoWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                string server = connectionManager.NamedValueDictionary["ServerName"].ToString();
                DateTime currentTime = DateTime.Now.AddMinutes(-5);

                ManagementObject env = Utility.GetFirstWMIInstance(server, @"cimv2", "Win32_Environment WHERE Name = 'SMS_LOG_PATH'");

                string logPath = env["VariableValue"].ToString();
                string source = string.Format(@"\\{0}\{1}\colleval.log", server, logPath.Replace(":", "$"));

                string input = ReadInput(source);

                Regex regex = new Regex(@"\[Express Evaluator\] Starting.*(\d.-\d.-\d{4} \d.:\d.:\d.)", RegexOptions.RightToLeft);
                Match startMatch = regex.Match(input);
                regex = new Regex(@"\[Express Evaluator\] Exiting.*(\d.-\d.-\d{4} \d.:\d.:\d.)", RegexOptions.RightToLeft);
                Match endMatch = regex.Match(input);

                if (startMatch.Success && endMatch.Success)
                {
                    CultureInfo provider = CultureInfo.InvariantCulture;
                    DateTime startTime = DateTime.ParseExact(startMatch.Groups[1].Value, "MM-dd-yyyy HH:mm:ss", provider);
                    DateTime endTime = DateTime.ParseExact(endMatch.Groups[1].Value, "MM-dd-yyyy HH:mm:ss", provider);
                    TimeSpan duration = endTime - startTime;

                    if (endTime.CompareTo(startTime) > 0)
                    {
                        // get collection count
                        regex = new Regex(@"PF: Found\s(\d.)\scandidate collections for incremental evaluation.*(\d.-\d.-\d{4}\s\d.:\d.:\d.)", RegexOptions.RightToLeft);
                        Match match = regex.Match(input);                      

                        int collectionCount = match.Success ? Convert.ToInt32(match.Groups[1].Value) : 0;
                        // get evaluated collections
                        regex = new Regex(@"\[Express Evaluator] successfully evaluated collection.*\[(.*)\].*used\s(.*)\sseconds.*(\d.-\d.-\d{4} \d.:\d.:\d.)");
                        MatchCollection matches = regex.Matches(input);

                        List<IResultObject> collections = Utility.SearchWMIToList(connectionManager, "SELECT CollectionID,Name FROM SMS_Collection");

                        foreach (Match obj in matches)
                        {
                            DateTime collectionTime = DateTime.ParseExact(obj.Groups[3].Value, "MM-dd-yyyy HH:mm:ss", provider);

                            if (Utility.InclusiveBetween(collectionTime, startTime, endTime))
                            {
                                IResultObject collection = collections.Where(x => x.Properties["CollectionID"].StringValue.Equals(obj.Groups[1].Value)).FirstOrDefault();
                                listViewListCollections.Items.Add(new ListViewItem()
                                {
                                    Text = collection["Name"].StringValue,
                                    SubItems = {
                                        obj.Groups[2].Value,
                                        obj.Groups[1].Value
                                    }
                                });
                            }
                        }

                        labelStartTime.Text = startTime.ToString();
                        labelEndTime.Text = endTime.ToString();
                        labelTimeTaken.Text = duration.ToString(@"mm\:ss");
                        labelCollectionCount.Text = collectionCount.ToString();

                        double[] columnData = (from ListViewItem row in listViewListCollections.Items
                                               where row.SubItems[1].Text.ToString() != string.Empty
                                               select Convert.ToDouble(row.SubItems[1].Text)
                                              ).ToArray();

                        labelCollAverageTime.Text = Math.Round(columnData.Average(), 3).ToString() + " sec";
                        labelCollTime.Text = TimeSpan.FromSeconds(columnData.Sum()).ToString(@"mm\:ss\.fff");
                        labelCollLongTime.Text  = Math.Round(columnData.Max(), 3).ToString() + " sec";
                        labelCollShortTime.Text = Math.Round(columnData.Min(), 3).ToString() + " sec";
                    }
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(string.Format("{0}: {1}", ex.GetType().Name, ex.Message));
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
                    UseWaitCursor = false;
                    progressBar1.Visible = false;
                    listViewListCollections.IsLoading = false;
                    listViewListCollections.UpdateColumnWidth(columnHeaderName);
                }
            }
        }

        private static string ReadInput(string fileName)
        {
            string output;
            using (StreamReader reader = new StreamReader(fileName))
            {
                output = reader.ReadToEnd();
            }
            return output;
        }

        private void ButtonOK_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void ListViewListCollections_CopyKeyEvent(object sender, EventArgs e)
        {
            StringBuilder buffer = new StringBuilder();
            foreach (ListViewItem item in listViewListCollections.SelectedItems)
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
