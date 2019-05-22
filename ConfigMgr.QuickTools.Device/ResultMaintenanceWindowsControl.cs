using Microsoft.ConfigurationManagement.AdminConsole;
using Microsoft.ConfigurationManagement.AdminConsole.Common;
using Microsoft.ConfigurationManagement.AdminConsole.DialogFramework;
using Microsoft.ConfigurationManagement.ManagementProvider;
using System;
using System.ComponentModel;
using System.Windows.Forms;
using System.Diagnostics;
using System.Text;
using System.Collections.Generic;
using Microsoft.ConfigurationManagement.AdminConsole.CollectionProperty;

namespace ConfigMgr.QuickTools.Device.PropertiesDialog
{
    public partial class ResultMaintenanceWindowsControl : SmsPageControl
    {
        private SmsBackgroundWorker backgroundWorker;

        public ResultMaintenanceWindowsControl(SmsPageData pageData)
          : base(pageData)
        {
            InitializeComponent();

            Title = "Maintenance Windows";

            Updater.CheckUpdates();
        }

        public override void InitializePageControl()
        {
            base.InitializePageControl();

            listViewListWindows.UpdateColumnWidth(columnHeaderWindows);
            listViewListWindows.Items.Clear();
            listViewListWindows.IsLoading = true;
            listViewListUpcomingWindows.UpdateColumnWidth(columnHeaderUpcomingTime);
            listViewListUpcomingWindows.Items.Clear();
            listViewListUpcomingWindows.IsLoading = true;

            string query = string.Format("SELECT SMS_Collection.* FROM SMS_FullCollectionMembership, SMS_Collection where ResourceID = '{0}' and ServiceWindowsCount > 0 and SMS_FullCollectionMembership.CollectionID = SMS_Collection.CollectionID", PropertyManager["ResourceID"].IntegerValue);

            backgroundWorker = new SmsBackgroundWorker();
            backgroundWorker.QueryProcessorCompleted += new EventHandler<RunWorkerCompletedEventArgs>(BackgroundWorker_RunWorkerCompleted);
            backgroundWorker.QueryProcessorObjectsReady += new EventHandler<QueryProcessorObjectsEventArgs>(BackgroundWorker_QueryProcessorObjectsReady);
            ConnectionManagerBase.SmsTraceSource.TraceEvent(TraceEventType.Information, 1, "InitializePageControl");
            UseWaitCursor = true;
            QueryProcessor.ProcessQuery(backgroundWorker, query);
        }

        private void BackgroundWorker_QueryProcessorObjectsReady(object sender, QueryProcessorObjectsEventArgs e)
        {
            if (e.ResultObjects == null)
                return;

            foreach (IResultObject resultObject in e.ResultObjects)
            {
                IResultObject collectionSettings = ConnectionManager.GetInstance("SMS_CollectionSettings.CollectionID='" + resultObject["CollectionID"].StringValue + "'");
                collectionSettings.Get();

                foreach (IResultObject arrayItem in collectionSettings.GetArrayItems("ServiceWindows"))
                {
                    if (arrayItem["IsEnabled"].BooleanValue)
                    {
                        listViewListWindows.Items.Add(new ListViewItem()
                        {
                            Text = arrayItem["Name"].StringValue,
                            SubItems = {
                                resultObject["Name"].StringValue,
                                GetServiceWindowType((ServiceWindowTypesControl.ServiceWindowTypes) arrayItem["ServiceWindowType"].IntegerValue)
                            }
                        });

                        List<IResultObject> resultObjectList = ResultObjectHelpers.ScheduleReadFromString(ConnectionManager, arrayItem["ServiceWindowSchedules"].StringValue);
                        IResultObject schedule = resultObjectList[0];

                        DateTime startTime = schedule["StartTime"].DateTimeValue;
                        DateTime currentTime = DateTime.Now;
                        int[] days = { 0, 7, 14, 21 };

                        string date = "";

                        switch (arrayItem["RecurrenceType"].IntegerValue)
                        {
                            case 1:
                                // nothing to do here, just add the date
                                date = startTime.ToString();

                                break;
                            case 2:
                                // daily schedule, check when next windows occures
                                do
                                {
                                    startTime = startTime.AddDays(schedule["DaySpan"].IntegerValue);
                                } while (currentTime.CompareTo(startTime) >= 0);

                                if (currentTime.CompareTo(startTime) <= 0)
                                {
                                    date = startTime.ToString();
                                }

                                break;
                            case 3:
                                // weekly schedule, check when next windows occures
                                while ((int)currentTime.DayOfWeek != (schedule["Day"].IntegerValue - 1))
                                {
                                    currentTime = currentTime.AddDays(1);
                                }

                                currentTime = currentTime.AddDays(days[(schedule["ForNumberOfWeeks"].IntegerValue -1)]);
                                currentTime = currentTime.Date.Add(startTime.TimeOfDay);

                                date = currentTime.ToString();

                                break;
                            case 4:
                                // monthly schedule with week selection, check when next windows occures
                                currentTime = new DateTime(currentTime.Year, currentTime.Month, 1);
                                // add months
                                if (schedule["ForNumberOfMonths"].IntegerValue > 1)
                                {
                                    currentTime = currentTime.AddMonths(schedule["ForNumberOfMonths"].IntegerValue);
                                }
                                // find the day in month
                                while ((int)currentTime.DayOfWeek != (schedule["Day"].IntegerValue - 1))
                                {
                                    currentTime = currentTime.AddDays(1);
                                }
                                // add weeks
                                currentTime = currentTime.AddDays(days[(schedule["WeekOrder"].IntegerValue - 1)]);
                                currentTime = currentTime.Date.Add(startTime.TimeOfDay);

                                date = currentTime.ToLongDateString();

                                break;
                            case 5:
                                // monthly schedule, check when next windows occures
                                int monthDay = schedule["MonthDay"].IntegerValue;
                                // day of month
                                if (monthDay >= 1)
                                {
                                    currentTime = new DateTime(currentTime.Year, currentTime.Month, monthDay);
                                }
                                // last day of month
                                else if (monthDay == 0)
                                {
                                    currentTime = new DateTime(currentTime.Year, currentTime.Month, DateTime.DaysInMonth(currentTime.Year, currentTime.Month));
                                }

                                if (schedule["ForNumberOfMonths"].IntegerValue > 1)
                                {
                                    currentTime = currentTime.AddMonths(schedule["ForNumberOfMonths"].IntegerValue);
                                }
                                currentTime = currentTime.Date.Add(startTime.TimeOfDay);

                                date = currentTime.ToLongDateString();

                                break;
                        }

                        listViewListUpcomingWindows.Items.Add(new ListViewItem()
                        {
                            Text = date,
                            SubItems = {
                                arrayItem["Name"].StringValue,
                            }
                        });

                    }
                }
            }
        }

        private void BackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                if (e.Error != null)
                    SccmExceptionDialog.ShowDialog(this, e.Error, "Error");
                else if (e.Cancelled)
                    ConnectionManagerBase.SmsTraceSource.TraceEvent(TraceEventType.Information, 1, "User canceled");
                else
                    Initialized = true;
            }
            finally
            {
                if (sender as SmsBackgroundWorker == backgroundWorker)
                {
                    backgroundWorker.Dispose();
                    backgroundWorker = null;
                    UseWaitCursor = false;
                    listViewListWindows.IsLoading = false;
                    listViewListUpcomingWindows.IsLoading = false;
                    listViewListWindows.UpdateColumnWidth(columnHeaderWindows);
                    listViewListUpcomingWindows.UpdateColumnWidth(columnHeaderUpcomingTime);
                }
            }
        }

        private void ListViewListWindows_CopyKeyEvent(object sender, EventArgs e)
        {
            StringBuilder buffer = new StringBuilder();
            foreach (ListViewItem item in listViewListWindows.SelectedItems)
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

        private void ListViewListUpcomingWindows_CopyKeyEvent(object sender, EventArgs e)
        {
            StringBuilder buffer = new StringBuilder();
            foreach (ListViewItem item in listViewListUpcomingWindows.SelectedItems)
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

        private static string GetServiceWindowType(ServiceWindowTypesControl.ServiceWindowTypes serviceWindowTypeValue)
        {
            switch (serviceWindowTypeValue)
            {
                case ServiceWindowTypesControl.ServiceWindowTypes.General:
                    return "All deployments";
                case ServiceWindowTypesControl.ServiceWindowTypes.Updates:
                    return "Software updates";
                case ServiceWindowTypesControl.ServiceWindowTypes.OSD:
                    return "Task sequences";
                default:
                    return "";
            }
        }
    }
}
