using IniParser;
using IniParser.Model;
using Microsoft.ConfigurationManagement.ManagementProvider;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management;
using System.Text.RegularExpressions;

namespace Zetta.ConfigMgr.QuickTools
{
    internal class Driver
    {
        #region Private
        private IniData Data;
        private string version;
        private string model;
        private IResultObject driverObject;
        private Exception warningExceptions;
        private Exception errorExceptions;
        #endregion

        #region State
        public string InfLocation { get; protected set; }
        public bool Import { get; set; }
        public bool HasWarning { get { return warningExceptions == null ? false : true; } }
        public Exception Warning { get { return warningExceptions; } }
        public bool HasError { get { return errorExceptions == null ? false : true; } }
        public Exception Error { get { return errorExceptions; } }


        public IResultObject Object
        {
            get
            {
                return driverObject;
            }
        }

        public string Version
        {
            get
            {
                return version;
            }
        }

        public string Model
        {
            get
            {
                return model;
            }
        }
        #endregion

        #region Initialization
        private Driver()
        {
            driverObject = null;
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
                Data = parser.ReadFile(inf);
                if (Data.Sections.ContainsSection("Version") && Data["Version"].ContainsKey("DriverVer"))
                {            
                    getVersion();
                    getModel();
                }
                else
                {
                    warningExceptions = new SystemException("Invalid inf file.");
                    Import = false;
                }
            }
            catch (Exception ex)
            {
                errorExceptions = ex;
                Import = false;
            }
        }
        #endregion

        internal void AddObject(IResultObject obj)
        {
            driverObject = obj;
        }

        public bool CreateObjectFromInfFile(ConnectionManagerBase connection)
        {
            Dictionary<string, object> methodParameters = new Dictionary<string, object>();
            methodParameters.Add("DriverPath", Path.GetDirectoryName(InfLocation));
            methodParameters.Add("INFFile", Path.GetFileName(InfLocation));
            IResultObject instance = null;
            try
            {
                IResultObject resultObject1 = connection.ExecuteMethod("SMS_Driver", "CreateFromINF", methodParameters);
                instance = connection.CreateInstance(resultObject1["Driver"].ObjectValue);
                resultObject1.Dispose();
            }
            catch (SmsQueryException ex)
            {
                if (ex.ExtendStatusErrorCode == 183)
                {
                    ManagementException managementException = ex.InnerException as ManagementException;
                    if (managementException != null)
                    {
                        string query = string.Format("SELECT * FROM SMS_Driver WHERE CI_UniqueID='{0}'", managementException.ErrorInformation["ObjectInfo"].ToString());
                        try
                        {
                            IResultObject resultObject2 = connection.QueryProcessor.ExecuteQuery(query);
                            foreach (IResultObject resultObject3 in resultObject2)
                                instance = resultObject3;
                            resultObject2.Dispose();

                            if (!Directory.Exists(instance["ContentSourcePath"].StringValue))
                            {
                                instance["ContentSourcePath"].StringValue = Path.GetDirectoryName(InfLocation);
                            }
                        }
                        catch (SmsQueryException ex1)
                        {
                            errorExceptions = ex1;
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
                    errorExceptions = new SystemException("Invalid inf file.");
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
                errorExceptions = ex;
                return false;
            }

            driverObject = instance;

            return true;
        }

        private void getVersion()
        {
            try
            {
                // set version to ini data version
                string version = Data["Version"]["DriverVer"];
                // check if version is a strings value
                Match match = Regex.Match(version, @"%(.+?)%");
                // if is strings value get value
                if (match.Success)
                {
                    string key = match.Groups[1].Value;
                    // get strings value
                    string strings = Data["Strings"][key];
                    // remove dirty chars
                    version = Regex.Replace(version, @"%(.+?)%", Regex.Replace(strings, @"[^0-9a-zA-Z.]", ""));
                }
                // remove date from version
                version = version.Split(',')[1].Trim();
                version = Regex.Replace(version, @"\.[0]*[0]([0-9])", ".$1").Trim();
                this.version = version;
            }
            catch
            {
                errorExceptions = new SystemException("Cannot get version information from inf.");
            }
        }

        private void getModel()
        {
            try
            {
                // set manufacturers to ini data manufacuter
                string[] manufacturers = Data["Manufacturer"].FirstOrDefault().Value.Split(',');
                string manufacturer = null;
                // check if we have more then one supported os
                if (manufacturers.Length > 1)
                {
                    for (int i = 1; i < manufacturers.Length; i++)
                    {
                        // create the os manufacurer string
                        string test = manufacturers[0].Trim() + "." + manufacturers[i].Trim();
                        if (Data.Sections.ContainsSection(test) && Data[test].Count != 0)
                        {
                            // if we found driver name string break
                            manufacturer = test;
                            break;
                        }
                    }

                    if (manufacturer == null)
                    {
                        string test = manufacturers[0].Trim();
                        if (Data.Sections.ContainsSection(test) && Data[test].Count != 0)
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
                string model = Data[manufacturer].FirstOrDefault().KeyName;
                // check if model is strings value
                Match match = Regex.Match(model, @"%(.+?)%");
                if (match.Success)
                {
                    string key = match.Groups[1].Value.Trim();
                    // get strings value
                    model = Data["Strings"][key].Replace('"', ' ').Trim();
                }
                else
                {
                    model = model.Replace('"', ' ').Trim();
                }

                this.model = model;
            }
            catch
            {
                errorExceptions = new SystemException("Cannot get model information from inf.");
            }
        }

        public void Dispose()
        {
            if (driverObject == null)
                return;
            driverObject.Dispose();
        }
    }
}
