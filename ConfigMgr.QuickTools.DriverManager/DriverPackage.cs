using Microsoft.ConfigurationManagement.ManagementProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.IO;
using System.Collections;

namespace ConfigMgr.QuickTools.DriverManager
{
    internal class DriverPackage
    {
        #region Private
        private readonly ConnectionManagerBase connectionManager;
        private IResultObject packageObject;
        private IResultObject categoryObject;
        #endregion

        #region State
        public string Name { get; private set; }
        public string Vendor { get; set; }
        public string Source { get; private set; }
        public string Target { get; private set; }
        public string Hash { get; private set; }
        public string[] Infs { get; private set; }
        public bool Import { get; private set; }
        public Dictionary<string, Driver> Drivers { get; private set; } = new Dictionary<string, Driver>();
        public Dictionary<string, string> ImportError { get; private set; } = new Dictionary<string, string>();
        public Dictionary<string, string> ImportWarning { get; private set; } = new Dictionary<string, string>();
        public List<Exception> Exception { get; private set; } = new List<Exception>();
        public bool HasDriverError { get { return ImportError.Count > 0 ? true : false; } }
        public bool HasDriverWarning { get { return ImportWarning.Count > 0 ? true : false; } }
        public bool HasException { get { return Exception.Count > 0 ? true : false; } }

        public IResultObject Category
        {
            get
            {
                if (categoryObject == null)
                    CreateDriverCategory();
                return categoryObject;
            }
        }

        public IResultObject Package
        {
            get
            {
                if (packageObject == null)
                    CreateDriverPackge();
                return packageObject;
            }
        }
        #endregion

        public DriverPackage(ConnectionManagerBase connection, string name, string source, string target)
        {
            connectionManager = connection;
            Name = name;
            Source = source;
            Target = target;

            Hash = Utility.CreateHashForFolder(Source);
            Import = !File.Exists(Path.Combine(Source, Hash + ".hash"));
            string[] infFiles = Directory.GetFiles(Source, "*.inf", SearchOption.AllDirectories);
            Infs = infFiles.Where(x => Path.GetFileName(x) != "autorun.inf").ToArray();
        }

        public bool Create()
        {
            return !CreateDriverCategory() || !CreateDriverPackge() ? false : true;
        }

        private bool CreateDriverPackge()
        {
            string query = string.Format("SELECT * FROM SMS_DriverPackage WHERE NAME = '{0}'", Name);
            packageObject = Utility.GetFirstWMIInstance(connectionManager, query);

            if (packageObject == null)
            {
                IResultObject instance = connectionManager.CreateInstance("SMS_DriverPackage");
                instance["Name"].StringValue = Name;
                instance["Description"].StringValue = "";
                instance["PkgSourceFlag"].IntegerValue = 2;
                instance["PkgSourcePath"].StringValue = Target;
                instance["PkgFlags"].IntegerValue |= 16777216;
                try
                {
                    instance.Put();
                    instance.Get();
                }
                catch (SmsQueryException ex)
                {
                    string str;
                    switch (ex.ExtendStatusErrorCode)
                    {
                        case 4:
                            str = "The driver package storage path is not empty.";
                            break;
                        case 1078462229:
                            str = "The system does not have read or write permissions to the driver package's source path.";
                            break;
                        default:
                            str = "An error occurred while importing the selected driver package.";
                            break;
                    }
                    Exception.Add(new SystemException(str));

                    return false;
                }

                Utility.AddObjectToFolder(connectionManager, Vendor, instance["PackageID"].StringValue, 23);

                packageObject = instance;
            }

            return true;
        }

        public void CreateHashFile()
        {
            try
            {
                string[] fileList = Directory.GetFiles(Source, "*.hash");
                foreach (string file in fileList)
                {
                    File.Delete(file);
                }
                File.Create(Path.Combine(Source, Hash + ".hash"));
            }
            catch
            {
                Exception.Add(new SystemException("Cannot create Hash file."));
            }
        }

        #region Drivers
        public IEnumerable<IResultObject> GetDriversInPackge()
        {
            string query = string.Format("SELECT * FROM SMS_Driver WHERE CI_ID IN (SELECT DC.CI_ID FROM SMS_DriverContainer AS DC WHERE DC.PackageID='{0}') ORDER BY LocalizedDisplayName", Package["PackageID"].StringValue);
            using (IResultObject resultObjects = connectionManager.QueryProcessor.ExecuteQuery(query))
            {
                foreach (IResultObject resultObject in resultObjects)
                {
                    yield return resultObject;
                }
            }
        }

        public bool DriverContentExists(Driver driver)
        {
            return DriverContentExists(driver.Object);
        }

        public bool DriverContentExists(IResultObject driverObject)
        {
            string query = string.Format("SELECT * FROM SMS_CIToContent WHERE CI_ID='{0}'", driverObject["CI_ID"].IntegerValue);
            using (IResultObject contentObject = Utility.GetFirstWMIInstance(connectionManager, query))
            {
                if (contentObject != null)
                {
                    contentObject.Get();
                    if (Directory.Exists(Path.Combine(Target, contentObject["ContentUniqueID"].StringValue)) && Directory.Exists(driverObject["ContentSourcePath"].StringValue))
                        return true;
                }
            }

            return false;
        }

        public bool AddDriverToDriverPack(Driver driver)
        {
            List<int> contentIDs = new List<int>();

            string query = string.Format("SELECT * FROM SMS_CIToContent WHERE CI_ID='{0}'", driver.Object["CI_ID"].IntegerValue);

            foreach (IResultObject resultObject in connectionManager.QueryProcessor.ExecuteQuery(query))
                contentIDs.Add(resultObject["ContentID"].IntegerValue);

            List<string> packageSources = new List<string>
            {
                driver.Object["ContentSourcePath"].StringValue
            };

            Dictionary<string, object> methodParameters = new Dictionary<string, object>
            {
                { "bRefreshDPs", false },
                { "ContentIDs", contentIDs.ToArray() },
                { "ContentSourcePath", packageSources.ToArray() }
            };

            try
            {
                Package.ExecuteMethod("AddDriverContent", methodParameters);
            }
            catch (SmsQueryException ex)
            {
                string str = string.Format("{0} ({1})", driver.Object["LocalizedDisplayName"].StringValue, driver.Object["DriverINFFile"].StringValue);
                ManagementException managementException = ex.InnerException as ManagementException;
                ImportError.Add(str, "Could not be added to package: " + managementException.ErrorInformation["Description"].ToString());
                return false;
            }

            return true;
        }

        public bool AddDriversToDriverPack()
        {
            List<int> contentIDs = new List<int>();
            List<string> packageSources = new List<string>();

            foreach (KeyValuePair<string, Driver> driver in Drivers)
            {
                if (driver.Value.Import)
                {
                    string query = string.Format("SELECT * FROM SMS_CIToContent WHERE CI_ID='{0}'", driver.Value.Object["CI_ID"].IntegerValue);

                    foreach (IResultObject resultObject in connectionManager.QueryProcessor.ExecuteQuery(query))
                        contentIDs.Add(resultObject["ContentID"].IntegerValue);

                    packageSources.Add(driver.Value.Object["ContentSourcePath"].StringValue);
                }
            }

            Dictionary<string, object> methodParameters = new Dictionary<string, object>
            {
                { "bRefreshDPs", true },
                { "ContentIDs", contentIDs.ToArray() },
                { "ContentSourcePath", packageSources.ToArray() }
            };

            try
            {
                Package.ExecuteMethod("AddDriverContent", methodParameters);
            }
            catch (SmsException ex)
            {
                Exception.Add(ex);
                return false;
            }

            return true;
        }

        public bool RemoveDriverFromDriverPack(IResultObject driverObject)
        {
            List<int> contentIDs = new List<int>();

            string query = string.Format("SELECT * FROM SMS_CIToContent WHERE CI_ID='{0}'", driverObject["CI_ID"].IntegerValue);

            foreach (IResultObject resultObject in connectionManager.QueryProcessor.ExecuteQuery(query))
                contentIDs.Add(resultObject["ContentID"].IntegerValue);

            Dictionary<string, object> methodParameters = new Dictionary<string, object>
            {
                { "bRefreshDPs", false },
                { "ContentIDs", contentIDs.ToArray() }
            };

            try
            {
                Package.ExecuteMethod("RemoveDriverContent", methodParameters);
            }
            catch (SmsQueryException ex)
            {
                string str = string.Format("{0} ({1})", driverObject["LocalizedDisplayName"].StringValue, driverObject["DriverINFFile"].StringValue);
                ManagementException managementException = ex.InnerException as ManagementException;
                ImportWarning.Add(str, managementException.ErrorInformation["Description"].ToString());
            }

            return true;
        }
        #endregion

        #region Driver Category
        private IResultObject GetDriverCategory()
        {
            string query = string.Format("SELECT * FROM SMS_CategoryInstance WHERE CategoryTypeName='DriverCategories' AND LocalizedCategoryInstanceName='{0}'", Name);
            IResultObject categoryObject = Utility.GetFirstWMIInstance(connectionManager, query);

            if (categoryObject != null)
            {
                categoryObject.Get();
                int integerValue = categoryObject["CategoryInstanceID"].IntegerValue;
                if (categoryObject["LocalizedCategoryInstanceName"].StringValue == Name)
                    return categoryObject;
            }

            return null;
        }

        private bool CreateDriverCategory()
        {
            categoryObject = GetDriverCategory();

            if (categoryObject == null)
            {
                IResultObject instance = connectionManager.CreateInstance("SMS_CategoryInstance");
                instance["CategoryInstance_UniqueID"].StringValue = string.Format("DriverCategories: {0}", Guid.NewGuid().ToString());
                instance["CategoryTypeName"].StringValue = "DriverCategories";
                List<IResultObject> list = new List<IResultObject>();
                IResultObject embeddedObjectInstance = connectionManager.CreateEmbeddedObjectInstance("SMS_Category_LocalizedProperties");
                embeddedObjectInstance["CategoryInstanceName"].StringValue = Name;
                embeddedObjectInstance["LocaleID"].IntegerValue = 0;
                list.Add(embeddedObjectInstance);
                instance.SetArrayItems("LocalizedInformation", list);
                try
                {
                    instance.Put();
                    instance.Get();
                }
                catch (SmsQueryException ex)
                {
                    Exception.Add(ex);
                    return false;
                }

                categoryObject = instance;
            }
            
            return true;
        }

        public bool AddDriverToCategory(Driver driver)
        {
            // get category unique id
            string categoryUniqueID = Category["CategoryInstance_UniqueID"].StringValue;
            ArrayList categories = new ArrayList();
            // check if driver has any category
            if (driver.Object["CategoryInstance_UniqueIDs"].StringArrayValue != null)
            {
                // check if category is in driver categories list
                categories.AddRange(driver.Object["CategoryInstance_UniqueIDs"].StringArrayValue);
                foreach (string str in categories)
                {
                    // if category exist return
                    if (str == categoryUniqueID)
                    {
                        return true;
                    }
                }
            }
            // add category to driver
            categories.Add(categoryUniqueID);
            driver.Object["CategoryInstance_UniqueIDs"].StringArrayValue = (string[])categories.ToArray(typeof(string));
            try
            {
                driver.Object.Put();
                driver.Object.Get();
            }
            catch (SmsQueryException ex)
            {
                string str = string.Format("{0} ({1})", driver.Object["LocalizedDisplayName"].StringValue, driver.Object["DriverINFFile"].StringValue);
                ManagementException managementException = ex.InnerException as ManagementException;
                ImportError.Add(str, "Cannot be added to category: " + managementException.ErrorInformation["Description"].ToString());
                return false;
            }

            return true;
        }

        public bool RemoveDriverFromCategory(IResultObject driverObject)
        {
            // get category unique id
            string categoryUniqueID = Category["CategoryInstance_UniqueID"].StringValue;
            ArrayList categories = new ArrayList();
            ArrayList removeCategory = new ArrayList();
            // check if driver has any category
            if (driverObject["CategoryInstance_UniqueIDs"].StringArrayValue != null)
            {
                // check if category is in driver categories list
                categories.AddRange(driverObject["CategoryInstance_UniqueIDs"].StringArrayValue);
                foreach (string str in categories)
                {
                    // if category exist add it to list for removal
                    if (str != categoryUniqueID)
                    {
                        removeCategory.Add(str);
                    }
                }
            }
            // remove category from driver
            driverObject["CategoryInstance_UniqueIDs"].StringArrayValue = (string[])removeCategory.ToArray(typeof(string));
            try
            {
                driverObject.Put();
                driverObject.Get();
            }
            catch (SmsQueryException ex)
            {
                string str = string.Format("{0} ({1})", driverObject["LocalizedDisplayName"].StringValue, driverObject["DriverINFFile"].StringValue);
                ManagementException managementException = ex.InnerException as ManagementException;
                ImportError.Add(str, "Cannot be removed from category: " + managementException.ErrorInformation["Description"].ToString());
                return false;
            }

            return true;
        }
        #endregion

    }
}
