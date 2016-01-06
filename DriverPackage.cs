using Microsoft.ConfigurationManagement.ManagementProvider;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Management;
using System.IO;
using System.Collections;
using System.Collections.ObjectModel;

namespace Zetta.ConfigMgr.QuickTools
{
    internal class DriverPackage : IDisposable
    {
        #region Private
        private ConnectionManagerBase connectionManager;
        private IResultObject packageObject;
        private IResultObject categoryObject;
        private List<Exception> errorExceptions;
        #endregion

        #region State
        public string Name { get; private set; }
        public string Vendor { get; set; }
        public string Source { get; private set; }
        public string Target { get; private set; }
        public string Hash { get; private set; }
        public string[] Infs { get; set; }
        public Dictionary<string, Driver> Drivers { get; set; }
        public bool Import { get; private set; }
        public Dictionary<string, string> ImportError { get; set; }
        public Dictionary<string, string> ImportWarning { get; set; }
        public bool HasDriverError { get { return ImportError.Count > 0; } }
        public bool HasDriverWarning { get { return ImportWarning.Count > 0; } }
        public bool HasError { get { return errorExceptions.Count > 0; } }
        public ReadOnlyCollection<Exception> Errors { get { return errorExceptions.AsReadOnly(); } }

        public IResultObject Category
        {
            get
            {
                if (categoryObject == null)
                    createDriverCategory();
                return categoryObject;
            }
        }

        public IResultObject Package
        {
            get
            {
                if (packageObject == null)
                    createDriverPackge();
                return packageObject;
            }
        }
        #endregion

        #region Initialization
        private DriverPackage()
        {
            ImportWarning = new Dictionary<string, string>();
            ImportError = new Dictionary<string, string>();

            Drivers = new Dictionary<string, Driver>();

            errorExceptions = new List<Exception>();
        }

        public DriverPackage(ConnectionManagerBase connection, string name, string source, string target)
            : this()
        {
            connectionManager = connection;
            Name = name;
            Source = source;
            Target = target;

            Hash = Utility.CreateMd5ForFolder(Source);
            Import = !File.Exists(Path.Combine(Source, Hash + ".hash"));
            Infs = Directory.GetFiles(Source, "*.inf", SearchOption.AllDirectories);
            Infs = Infs.Where(x => Path.GetFileName(x) != "autorun.inf").ToArray();
        }
        #endregion

        public bool Create()
        {
            createDriverCategory();
            createDriverPackge();

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
                errorExceptions.Add(new SystemException("Could not create Hash file."));
            }
            
        }

        #region Drivers
        public IEnumerable<IResultObject> GetDriversFromPackge()
        {
            string query = string.Format("SELECT * FROM SMS_Driver WHERE CI_ID IN (SELECT DC.CI_ID FROM SMS_DriverContainer AS DC WHERE DC.PackageID='{0}') ORDER BY LocalizedDisplayName", Package["PackageID"].StringValue);
            using (IResultObject resultObject1 = connectionManager.QueryProcessor.ExecuteQuery(query))
            {
                foreach (IResultObject resultObject2 in resultObject1)
                {
                    yield return resultObject2;
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
            IResultObject resultObject1 = connectionManager.QueryProcessor.ExecuteQuery(query);
            IResultObject contentObject = null;
            foreach (IResultObject resultObject2 in resultObject1)
                contentObject = resultObject2;
            resultObject1.Dispose();

            if (contentObject != null)
            {
                contentObject.Get();
                if (Directory.Exists(Path.Combine(Target, contentObject["ContentUniqueID"].StringValue)) && Directory.Exists(driverObject["ContentSourcePath"].StringValue))
                    return true;
            }

            return false;
        }

        public bool AddDriverCategory(Driver driver)
        {
            string stringValue = Category["CategoryInstance_UniqueID"].StringValue;
            ArrayList arrayList = new ArrayList();
            if (driver.Object["CategoryInstance_UniqueIDs"].StringArrayValue != null)
            {
                arrayList.AddRange(driver.Object["CategoryInstance_UniqueIDs"].StringArrayValue);
                foreach (string str in arrayList)
                {
                    if (str == stringValue)
                    {
                        return true;
                    }
                }
            }
            arrayList.Add(stringValue);
            driver.Object["CategoryInstance_UniqueIDs"].StringArrayValue = (string[])arrayList.ToArray(typeof(string));
            try
            {
                driver.Object.Put();
                driver.Object.Get();
            }
            catch (SmsQueryException ex)
            {
                string str = string.Format("{0} ({1})", driver.Object["LocalizedDisplayName"].StringValue, driver.Object["DriverINFFile"].StringValue);
                ManagementException managementException = ex.InnerException as ManagementException;
                ImportError[str] = "Could not be added to category: " + managementException.ErrorInformation["Description"].ToString();
                return false;
            }

            return true;
        }

        public bool RemoveDriverCategory(IResultObject driverObject)
        {
            string categoryID = Category["CategoryInstance_UniqueID"].StringValue;
            ArrayList arrayList = new ArrayList();
            ArrayList arrayList2 = new ArrayList();
            if (driverObject["CategoryInstance_UniqueIDs"].StringArrayValue != null)
            {
                arrayList.AddRange(driverObject["CategoryInstance_UniqueIDs"].StringArrayValue);
                foreach (string str in arrayList)
                {
                    if (str != categoryID)
                    {
                        arrayList2.Add(str);
                    }
                }
            }

            driverObject["CategoryInstance_UniqueIDs"].StringArrayValue = (string[])arrayList2.ToArray(typeof(string));
            try
            {
                driverObject.Put();
                driverObject.Get();
            }
            catch (SmsQueryException ex)
            {
                string str = string.Format("{0} ({1})", driverObject["LocalizedDisplayName"].StringValue, driverObject["DriverINFFile"].StringValue);
                ManagementException managementException = ex.InnerException as ManagementException;
                ImportError[str] = "Could not be removed from category: " + managementException.ErrorInformation["Description"].ToString();
                return false;
            }

            return true;
        }

        public bool AddDriverToDriverPack(Driver driver)
        {
            List<int> contentIDs = new List<int>();

            string query = string.Format("SELECT * FROM SMS_CIToContent WHERE CI_ID='{0}'", driver.Object["CI_ID"].IntegerValue);

            foreach (IResultObject resultObject in connectionManager.QueryProcessor.ExecuteQuery(query))
                contentIDs.Add(resultObject["ContentID"].IntegerValue);

            List<string> packageSources = new List<string>();
            packageSources.Add(driver.Object["ContentSourcePath"].StringValue);

            Dictionary<string, object> methodParameters = new Dictionary<string, object>();
            methodParameters.Add("bRefreshDPs", false);
            methodParameters.Add("ContentIDs", contentIDs.ToArray());
            methodParameters.Add("ContentSourcePath", packageSources.ToArray());

            try
            {
                Package.ExecuteMethod("AddDriverContent", methodParameters);
            }
            catch (SmsQueryException ex)
            {
                string str = string.Format(CultureInfo.CurrentCulture, "{0} ({1})", new object[2]
                {
                     driver.Object["LocalizedDisplayName"].StringValue,
                     driver.Object["DriverINFFile"].StringValue
                });
                ManagementException managementException = ex.InnerException as ManagementException;
                ImportError[str] = "Could not be added to package: " + managementException.ErrorInformation["Description"].ToString();
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

            Dictionary<string, object> methodParameters = new Dictionary<string, object>();
            methodParameters.Add("bRefreshDPs", true);
            methodParameters.Add("ContentIDs", contentIDs.ToArray());
            methodParameters.Add("ContentSourcePath", packageSources.ToArray());

            try
            {
                Package.ExecuteMethod("AddDriverContent", methodParameters);
            }
            catch (SmsException ex)
            {
                errorExceptions.Add(ex);
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

            Dictionary<string, object> methodParameters = new Dictionary<string, object>();
            methodParameters.Add("bRefreshDPs", false);
            methodParameters.Add("ContentIDs", contentIDs.ToArray());

            try
            {
                Package.ExecuteMethod("RemoveDriverContent", methodParameters);
            }
            catch (SmsQueryException ex)
            {
                string str = string.Format("{0} ({1})", driverObject["LocalizedDisplayName"].StringValue, driverObject["DriverINFFile"].StringValue);
                ManagementException managementException = ex.InnerException as ManagementException;
                ImportError[str] = "Could not be removed from package: " + managementException.ErrorInformation["Description"].ToString();
                return false;
            }

            return true;
        }
        #endregion

        #region Driver Category
        private IResultObject getDriverCategory()
        {
            string query = string.Format("SELECT * FROM SMS_CategoryInstance WHERE CategoryTypeName='DriverCategories' AND LocalizedCategoryInstanceName='{0}'", Name);
            IResultObject resultObject1 = connectionManager.QueryProcessor.ExecuteQuery(query);

            IResultObject categoryObject = null;
            foreach (IResultObject resultObject2 in resultObject1)
                categoryObject = resultObject2;
            resultObject1.Dispose();

            if (categoryObject != null)
            {
                categoryObject.Get();
                int integerValue = categoryObject["CategoryInstanceID"].IntegerValue;
                if (categoryObject["LocalizedCategoryInstanceName"].StringValue == Name)
                    return categoryObject;
            }

            return null;
        }

        private void createDriverCategory()
        {
            categoryObject = getDriverCategory();
            if (categoryObject == null)
            {

                IResultObject instance = connectionManager.CreateInstance("SMS_CategoryInstance");
                instance["CategoryInstance_UniqueID"].StringValue = string.Format("DriverCategories: {1}", Guid.NewGuid().ToString());
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
                    errorExceptions.Add(ex);
                    return;
                }

                categoryObject = instance;
            }
        }
        #endregion

        #region Driver Package
        private IResultObject getDriverPackage()
        {
            string query = string.Format("SELECT * FROM SMS_DriverPackage WHERE NAME = '{0}'", Name);
            IResultObject resultObject1 = connectionManager.QueryProcessor.ExecuteQuery(query);

            IResultObject driverPackageObject = null;
            foreach (IResultObject resultObject2 in resultObject1)
                driverPackageObject = resultObject2;
            resultObject1.Dispose();

            return driverPackageObject;
        }

        private void createDriverPackge()
        {
            packageObject = getDriverPackage();
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
                    string str = null;
                    switch (ex.ExtendStatusErrorCode)
                    {
                        case 4:
                            str = "The driver package storage path is NOT empty.";
                            break;
                        case 1078462229:
                            str = "The system does not have read or write permissions to the driver package's source path.";
                            break;
                        default:
                            str = "An error occurred while importing the selected driver package.";
                            break;
                    }
                    errorExceptions.Add(new SystemException(str));
                    return;
                }

                IResultObject folder = null;
                Utility.AddObjectToFolder(connectionManager, Vendor, instance["PackageID"].StringValue, 23, out folder);

                packageObject = instance;
            }
        }
        #endregion

        public void Dispose()
        {
            if (packageObject == null && categoryObject == null)
                return;
            packageObject.Dispose();
            categoryObject.Dispose();
        }
    }
}
