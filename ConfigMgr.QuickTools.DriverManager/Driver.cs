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
    internal class Driver
    {
        #region Private
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly IniData data;
        #endregion

        #region State
        public bool Import { get; set; }
        public string Version { get; private set; }
        public string Model { get; private set; }
        public IResultObject Object { get; private set; }
        public string InfLocation { get; private set; }
        public Exception Exception { get; private set; }
        public bool HasException { get { return Exception == null ? false : true; } }
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
                    log.Warn(string.Format("InvalidInf: {0}", InfLocation));

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

        internal bool CheckIfExists(ConnectionManagerBase connectionManager)
        {
            // would be good if we could check based on architecture here, but there is no column that supports that in SMS_Driver without digging into the xml
            // we could maybe hack in the description field and append architecture
            string query = string.Format("SELECT * FROM SMS_Driver WHERE LocalizedDisplayName='{0}' AND DriverVersion='{1}' AND DriverINFFile='{2}'", Model, Version, Path.GetFileName(InfLocation));
            IResultObject driverObject = Utility.GetFirstWMIInstance(connectionManager, query);

            if (driverObject != null)
            {
                if (!Directory.Exists(driverObject["ContentSourcePath"].StringValue))
                {
                    log.Debug("UpdateContentSourcePath: " + driverObject["LocalizedDisplayName"].StringValue);
                    try
                    {
                        driverObject["ContentSourcePath"].StringValue = Path.GetDirectoryName(InfLocation);

                        driverObject.Put();
                        driverObject.Get();
                    }
                    catch (SmsQueryException ex)
                    {
                        ManagementException mgmtException = ex.InnerException as ManagementException;
                        Exception = new SystemException(mgmtException.ErrorInformation["Description"].ToString());
                        log.Error(string.Format("PutDriverObject: {0}, {1}, {2}", InfLocation, ex.GetType().Name, Exception.Message));

                        return false;
                    }
                }

                Object = driverObject;
                return true;
            }

            return false;
        }

        internal bool CreateObjectFromInfFile(ConnectionManagerBase connectionManager)
        {
            Dictionary<string, object> methodParameters = new Dictionary<string, object>
            {
                { "DriverPath", Path.GetDirectoryName(InfLocation) },
                { "INFFile", Path.GetFileName(InfLocation) }
            };

            IResultObject driverObject = null;

            try
            {
                log.Debug("CreateFromINF: " + InfLocation);
                using (IResultObject resultObject = connectionManager.ExecuteMethod("SMS_Driver", "CreateFromINF", methodParameters))
                {
                    log.Debug("CreateInstance: " + InfLocation);
                    driverObject = connectionManager.CreateInstance(resultObject["Driver"].ObjectValue);
                }
            }
            catch (SmsQueryException ex)
            {
                // error 183 = driver exist, check if source content is ok.
                if (ex.ExtendStatusErrorCode == 183)
                {
                    if (ex.InnerException is ManagementException managementException)
                    {
                        try
                        {
                            // update content source path if it dose not exist
                            string query = string.Format("SELECT * FROM SMS_Driver WHERE CI_UniqueID='{0}'", managementException.ErrorInformation["ObjectInfo"].ToString());
                            driverObject = Utility.GetFirstWMIInstance(connectionManager, query);

                            if (!Directory.Exists(driverObject["ContentSourcePath"].StringValue))
                            {
                                log.Debug("UpdateContentSourcePath: " + driverObject["LocalizedDisplayName"].StringValue);
                                driverObject["ContentSourcePath"].StringValue = Path.GetDirectoryName(InfLocation);
                            }
                        }
                        catch (SmsQueryException ex1)
                        {
                            ManagementException mgmtException = ex.InnerException as ManagementException;
                            Exception = new SystemException(mgmtException.ErrorInformation["Description"].ToString());
                            log.Error(string.Format("ContentSourcePath: {0}, {1}, {2}", InfLocation, ex1.GetType().Name, Exception.Message));

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
                    log.Error(string.Format("InvalidInf: {0}", InfLocation));

                    return false;
                }
                else
                {
                    ManagementException mgmtException = ex.InnerException as ManagementException;
                    Exception = new SystemException(mgmtException.ErrorInformation["Description"].ToString());
                    log.Error(string.Format("Error: {0}", Exception.Message));

                    return false;
                }
            }

            if (driverObject == null)
            {
                log.Debug("NoObject: " + InfLocation);

                return false;
            }

            driverObject["IsEnabled"].BooleanValue = true;
            try
            {
                driverObject.Put();
                driverObject.Get();
            }
            catch (SmsQueryException ex)
            {
                ManagementException mgmtException = ex.InnerException as ManagementException;
                Exception = new SystemException(mgmtException.ErrorInformation["Description"].ToString());
                log.Error(string.Format("PutDriverObject: {0}, {1}, {2}", InfLocation, ex.GetType().Name, Exception.Message));

                return false;
            }

            Object = driverObject;

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
                log.Error(string.Format("GetVersion: {0}", Exception.Message));
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
                log.Error(string.Format("GetModel: {0}", Exception.Message));
            }

            return model;
        }
    }
}
