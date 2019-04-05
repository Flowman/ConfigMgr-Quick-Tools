using IniParser;
using IniParser.Model;
using Microsoft.ConfigurationManagement.ManagementProvider;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management;
using System.Text.RegularExpressions;

namespace ConfigMgr.QuickTools.DriverManager
{
    class Driver
    {
        #region Private
        private IniData data;
        #endregion

        #region State
        public bool Import { get; set; }
        public string Version { get; private set; }
        public string Model { get; private set; }
        public IResultObject Object { get; private set; }
        public string InfLocation { get; private set; }

        public Exception Exception { get; private set; }
        public bool HasError { get { return Exception == null ? false : true; } }
        public Exception Warning { get; private set; }
        public bool HasWarning { get { return Warning == null ? false : true; } }
        #endregion

        private Driver()
        {
            Object = null;
            Import = true;
        }

        public Driver(string inf)
            : this()
        {
            InfLocation = inf;

            var parser = new FileIniDataParser();
            parser.Parser.Configuration.AllowDuplicateKeys = true;
            parser.Parser.Configuration.AllowDuplicateSections = true;
            parser.Parser.Configuration.SkipInvalidLines = true;
            parser.Parser.Configuration.CaseInsensitive = true;

            try
            {
                data = parser.ReadFile(inf);
                if (data.Sections.ContainsSection("Version") && data["Version"].ContainsKey("DriverVer"))
                {
                    Version = GetVersion();
                    Model = GetModel();
                }
                else
                {
                    Warning = new SystemException("Invalid inf file.");
                    Import = false;
                }
            }
            catch (Exception ex)
            {
                Exception = ex;
                Import = false;
            }
        }

        internal void AddObject(IResultObject resultObject)
        {
            Object = resultObject;
        }

        internal bool CreateObjectFromInfFile(ConnectionManagerBase connectionManager)
        {
            Dictionary<string, object> methodParameters = new Dictionary<string, object>
            {
                { "DriverPath", Path.GetDirectoryName(InfLocation) },
                { "INFFile", Path.GetFileName(InfLocation) }
            };

            IResultObject instance = null;

            try
            {
                IResultObject resultObject = connectionManager.ExecuteMethod("SMS_Driver", "CreateFromINF", methodParameters);
                instance = connectionManager.CreateInstance(resultObject["Driver"].ObjectValue);
                resultObject.Dispose();
            }
            catch (SmsQueryException ex)
            {
                if (ex.ExtendStatusErrorCode == 183)
                {
                    if (ex.InnerException is ManagementException managementException)
                    {
                        try
                        {
                            string query = string.Format("SELECT * FROM SMS_Driver WHERE CI_UniqueID='{0}'", managementException.ErrorInformation["ObjectInfo"].ToString());
                            instance = Utility.GetFirstWMIInstance(connectionManager, query);

                            if (!Directory.Exists(instance["ContentSourcePath"].StringValue))
                            {
                                instance["ContentSourcePath"].StringValue = Path.GetDirectoryName(InfLocation);
                            }
                        }
                        catch (SmsQueryException ex1)
                        {
                            Exception = ex1;
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
                else if (ex.ExtendStatusErrorCode == 13L)
                {
                    Exception = new SystemException("Invalid inf file.");
                    return false;
                }

            }
            if (instance == null)
            {
                return false;
            }

            instance["IsEnabled"].BooleanValue = true;
            try
            {
                instance.Put();
                instance.Get();
            }
            catch (SmsQueryException ex)
            {
                Exception = ex;
                return false;
            }

            Object = instance;

            return true;
        }

        private string GetVersion()
        {
            string version = null;

            try
            {
                // set version to ini data version
                version = data["Version"]["DriverVer"];
                // check if version is a strings value
                Match match = Regex.Match(version, @"%(.+?)%");
                // if is strings value get value
                if (match.Success)
                {
                    string key = match.Groups[1].Value;
                    // get strings value
                    string strings = data["Strings"][key];
                    // remove dirty chars
                    version = Regex.Replace(version, @"%(.+?)%", Regex.Replace(strings, @"[^0-9a-zA-Z.]", ""));
                }
                // remove date from version
                version = version.Split(',')[1].Trim();
                version = Regex.Replace(version, @"\.[0]*[0]([0-9])", ".$1").Trim();
            }
            catch
            {
                Exception = new SystemException("Cannot get version information from inf.");
            }

            return version;
        }

        private string GetModel()
        {
            string model = null;

            try
            {
                // set manufacturers to ini data manufacuter
                string[] manufacturers = data["Manufacturer"].FirstOrDefault().Value.Split(',');
                string manufacturer = null;
                // check if we have more then one supported os
                if (manufacturers.Length > 1)
                {
                    for (int i = 1; i < manufacturers.Length; i++)
                    {
                        // create the os manufacurer string
                        string test = manufacturers[0].Trim() + "." + manufacturers[i].Trim();
                        if (data.Sections.ContainsSection(test) && data[test].Count != 0)
                        {
                            // if we found driver name string break
                            manufacturer = test;
                            break;
                        }
                    }

                    if (manufacturer == null)
                    {
                        string test = manufacturers[0].Trim();
                        if (data.Sections.ContainsSection(test) && data[test].Count != 0)
                        {
                            manufacturer = test;
                        }
                    }
                }
                else
                {
                    // singel manufacurer string
                    manufacturer = manufacturers[0];
                }
                // get model from os section
                model = data[manufacturer].FirstOrDefault().KeyName;
                // check if model is strings value
                Match match = Regex.Match(model, @"%(.+?)%");
                if (match.Success)
                {
                    string key = match.Groups[1].Value.Trim();
                    // get strings value
                    model = data["Strings"][key].Replace('"', ' ').Trim();
                }
                else
                {
                    model = model.Replace('"', ' ').Trim();
                }
            }
            catch
            {
                Exception = new SystemException("Cannot get model information from inf.");
            }

            return model;
        }
    }
}
