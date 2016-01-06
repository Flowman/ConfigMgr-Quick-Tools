using Microsoft.ConfigurationManagement.ManagementProvider;
using System;
using System.Text.RegularExpressions;

namespace Zetta.ConfigMgr.QuickTools
{
    internal class SUDeployment
    {

        #region Private
        private ConnectionManagerBase connectionManager;
        private IResultObject collectionObject;
        private IResultObject deploymentObject;
        private DateTime patchTuesday;
        private Exception errorExceptions;
        #endregion

        #region State
        public string Name { get; protected set; }
        public bool IsRequired { get; set; }
        public bool Enabled { get; set; }
        public bool UserUIExperience { get; set; }
        public bool NotifyUser { get; set; }
        public int SuppressReboot { get; set; }
        public bool RebootOutsideOfServiceWindows { get; set; }
        public bool OverrideServiceWindows { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime DeadLineTime { get; set; }
        public bool IsPhased { get; protected set; }
        public bool HasError { get { return errorExceptions == null ? false : true; } }
        public Exception Error { get { return errorExceptions; } }

        public IResultObject Collection
        {
            get
            {
                return collectionObject;
            }
        }
        #endregion

        #region Initialization
        private SUDeployment()
        {
            Name = string.Format("Software Updates - {0}", DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss tt"));
            Enabled = false;
            IsRequired = true;
            SuppressReboot = 0;
            RebootOutsideOfServiceWindows = false;
            OverrideServiceWindows = false;
            UserUIExperience = false;
            NotifyUser = false;

            patchTuesday = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 01).Next(DayOfWeek.Tuesday).AddDays((2 - 1) * 7);
            StartTime = patchTuesday;
            DeadLineTime = patchTuesday;
        }


        public SUDeployment(ConnectionManagerBase connection, IResultObject collection)
            : this()
        {
            connectionManager = connection;
            collectionObject = collection;

            Match match = Regex.Match(collection["Name"].StringValue, @"(?<=\[)([ANSIR]*)\s?(.*)(?=\])");

            IsPhased = match.Success ? true : false;

            if (match.Groups[1].Success)
            {
                string key = match.Groups[1].Value;

                if (Regex.IsMatch(key, @"[A]"))
                {
                    IsRequired = false;
                }
                if (Regex.IsMatch(key, @"[N]"))
                {
                    UserUIExperience = true;
                    NotifyUser = false;
                }
                if (Regex.IsMatch(key, @"[I]"))
                {
                    OverrideServiceWindows = true;
                }
                if (Regex.IsMatch(key, @"[R]"))
                {
                    RebootOutsideOfServiceWindows = true;
                }
                if (Regex.IsMatch(key, @"[S]"))
                {
                    SuppressReboot = 3;
                }
            }

            if (match.Groups[2].Success)
            {
                string[] values = match.Groups[2].Value.Split(new char[0], StringSplitOptions.RemoveEmptyEntries);

                if (0 < values.Length && Regex.IsMatch(values[0], @"(\+[0-9]+)"))
                {
                    StartTime = StartTime.AddDays(Convert.ToDouble(values[0].TrimStart('+')));
                    DeadLineTime = DeadLineTime.AddDays(Convert.ToDouble(values[0].TrimStart('+')));
                }

                if (1 < values.Length && Regex.IsMatch(values[1], @"(([01]\d|2[0-3]):?[0-5]\d)"))
                {
                    string[] time = values[1].Split(':');
                    StartTime = StartTime.ChangeTime(Convert.ToInt16(time[0]), Convert.ToInt16(time[1]), 0, 0);
                    DeadLineTime = DeadLineTime.ChangeTime(Convert.ToInt16(time[0]), Convert.ToInt16(time[1]), 0, 0);
                }

                if (2 < values.Length && Regex.IsMatch(values[2], @"(\+[0-9]+)"))
                {
                    DeadLineTime = DeadLineTime.AddDays(Convert.ToDouble(values[2].TrimStart('+')));
                }

                if (3 < values.Length && Regex.IsMatch(values[3], @"(([01]\d|2[0-3]):?[0-5]\d)"))
                {
                    string[] time = values[3].Split(':');
                    DeadLineTime = DeadLineTime.ChangeTime(Convert.ToInt16(time[0]), Convert.ToInt16(time[1]), 0, 0);
                }

            }

        }
        #endregion

        public bool CreateDeployment(IResultObject selectedObject)
        {
            IResultObject instance = connectionManager.CreateInstance("SMS_UpdateGroupAssignment");

            instance["AssignedUpdateGroup"].IntegerValue = selectedObject["CI_ID"].IntegerValue;

            instance["AssignedCIs"].IntegerArrayValue = selectedObject["Updates"].IntegerArrayValue;
            instance["AssignmentName"].StringValue = Name;
            instance["AssignmentDescription"].StringValue = "Generated by Zetta Integration Kit";
            instance["ApplyToSubTargets"].BooleanValue = false;
            instance["UseGMTTimes"].BooleanValue = false;

            instance["TargetCollectionID"].StringValue = collectionObject["CollectionID"].StringValue;
            
            instance["NotifyUser"].BooleanValue = NotifyUser;
            instance["UserUIExperience"].BooleanValue = UserUIExperience;

            instance["WoLEnabled"].BooleanValue = false;

            instance["StartTime"].DateTimeValue = StartTime;
            if (IsRequired)
                instance["EnforcementDeadline"].DateTimeValue = DeadLineTime;

            instance["SuppressReboot"].IntegerValue = SuppressReboot;
            instance["RebootOutsideOfServiceWindows"].BooleanValue = RebootOutsideOfServiceWindows;
            instance["OverrideServiceWindows"].BooleanValue = OverrideServiceWindows;

            instance["Enabled"].BooleanValue = Enabled ? true : false;

            try
            {
                instance.Put();
                instance.Get();
            }
            catch (SmsQueryException ex)
            {
                errorExceptions = ex;
                return false;
            }

            deploymentObject = instance;

            return true;
        }
    }

    public static partial class DateTimeExtensions
    {
        public static DateTime Next(this DateTime date, DayOfWeek dayOfWeek)
        {
            return date.AddDays((dayOfWeek < date.DayOfWeek ? 7 : 0) + dayOfWeek - date.DayOfWeek);
        }

        public static DateTime ChangeTime(this DateTime dateTime, int hours, int minutes, int seconds, int milliseconds)
        {
            return new DateTime(
                dateTime.Year,
                dateTime.Month,
                dateTime.Day,
                hours,
                minutes,
                seconds,
                milliseconds,
                dateTime.Kind);
        }
    }
}
