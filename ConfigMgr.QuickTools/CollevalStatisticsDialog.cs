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

namespace ConfigMgr.QuickTools
{
    public partial class CollevalStatisticsDialog : SmsCustomDialog
    {
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
            Cursor = Cursors.WaitCursor;
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
                    DateTime startTime = DateTime.Parse(startMatch.Groups[1].Value);
                    DateTime endTime = DateTime.Parse(endMatch.Groups[1].Value);

                    if (endTime.CompareTo(startTime) > 0)
                    {
                        regex = new Regex(@"PF:\s(\d.)\scollections in incremental evaluation graph.*(\d.-\d.-\d{4}\s\d.:\d.:\d.)", RegexOptions.RightToLeft);
                        Match match = regex.Match(input);                      

                        int collectionCount = match.Success ? Convert.ToInt32(match.Groups[1].Value) : 0;

                        TimeSpan duration = endTime - startTime;

                        labelStartTime.Text = startTime.ToString();
                        labelEndTime.Text = endTime.ToString();
                        labelTimeTaken.Text = duration.ToString();
                        labelCollectionCount.Text = collectionCount.ToString();

                        regex = new Regex(@"\[Express Evaluator] successfully evaluated collection.*\[(.*)\].*used\s(.*)\sseconds.*(\d.-\d.-\d{4} \d.:\d.:\d.)");
                        MatchCollection matches = regex.Matches(input);

                        foreach (Match test in matches)
                        {
                            DateTime collectionTime = DateTime.Parse(test.Groups[3].Value);

                            if (Utility.InclusiveBetween(collectionTime, startTime, endTime))
                            {
                                listViewListCollections.Items.Add(new ListViewItem()
                                {
                                    Text = "CollName",
                                    SubItems = {
                                        test.Groups[2].Value,
                                        test.Groups[1].Value
                                    }
                                });
                            }
                        }

                        double[] columnData = (from ListViewItem row in listViewListCollections.Items
                                            where row.SubItems[1].Text.ToString() != string.Empty
                                            select Convert.ToDouble(row.SubItems[1].Text)).ToArray();

                        labelCollAverageTime.Text = Math.Round(columnData.Average(), 2).ToString() + " seconds";
                        labelCollTime.Text = Math.Round(columnData.Sum() / 60, 2).ToString() + " minutes";
                        labelCollLongTime.Text  = Math.Round(columnData.Max(), 2).ToString() + " seconds";
                        labelCollShortTime.Text = Math.Round(columnData.Min(), 2).ToString() + " seconds";
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
                    Cursor = Cursors.Default;
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
