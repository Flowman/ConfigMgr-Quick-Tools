using System;
using System.IO;
using System.Reflection;
using log4net.Layout.Pattern;

namespace ConfigMgr.QuickTools.CMTrace
{
    public class NumericLevelPatternConverter : PatternLayoutConverter
    {
        protected override void Convert(TextWriter writer, log4net.Core.LoggingEvent loggingEvent)
        {
            int LoggingLevel;
            switch (loggingEvent.Level.Value)
            {
                case 10000: LoggingLevel = 4; break;
                case 30000: LoggingLevel = 5; break;
                case 40000: LoggingLevel = 6; break;
                case 60000: LoggingLevel = 2; break;
                case 70000: LoggingLevel = 3; break;
                default: LoggingLevel = 1; break;

            }
            writer.Write(LoggingLevel);
        }
    }

    public class AssemblyNamePatternConverter : log4net.Util.PatternConverter
    {
        override protected void Convert(TextWriter writer, object state)
        {
            string ProgramName;
            try
            {
                ProgramName = (Assembly.GetExecutingAssembly().GetName().Name);
            }
            catch
            {
                ProgramName = "ConfigMgr.QuickTools";
            }
            writer.Write(ProgramName);
        }
    }

    public class UTCOffsetPatternConverter : log4net.Util.PatternConverter
    {
        override protected void Convert(TextWriter writer, object state)
        {
            TimeSpan delta = (TimeSpan.Zero);
            try
            {
                TimeZone TimeZoneInfo = TimeZone.CurrentTimeZone;
                delta = TimeZoneInfo.GetUtcOffset(DateTime.Now);
            }
            catch
            {

            }
            writer.Write(delta.TotalMinutes.ToString());
        }
    }
}
