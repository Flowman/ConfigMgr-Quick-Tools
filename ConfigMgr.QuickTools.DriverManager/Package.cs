using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace ConfigMgr.QuickTools.DriverManager
{
    internal class Package
    {
        #region State
        public string Name { get; protected set; }
        public string Model { get; protected set; }
        public string Vendor { get; set; }
        public string Version { get; protected set; }
        public string VersionFile { get; protected set; }
        public string FileVersion { get; protected set; }
        public string Source { get; protected set; }
        public string Target { get; protected set; }
        public string FolderName { get; protected set; }
        public string Hash { get; protected set; }
        public bool Import { get; protected set; }
        public List<Exception> Exception { get; protected set; } = new List<Exception>();
        public bool HasException { get { return Exception.Count > 0 ? true : false; } }
        #endregion

        public string GetVersionFromFile()
        {
            try
            {
                string[] fileList = Directory.GetFiles(Source, "*.version");
                if (fileList.Length >= 1)
                {
                    string str = Path.GetFileName(fileList.FirstOrDefault());
                    FileVersion = str.Split('.')[0];
                }
            }
            catch
            {
                Exception.Add(new SystemException("Cannot get version from file."));
            }

            return FileVersion;
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

        public string GenerateModelFolderName(string os, string structure)
        {
            return FolderName = (string.IsNullOrEmpty(structure) ? false : Convert.ToBoolean(structure))
                ? string.Format(@"{0}\{1}\{2}", Vendor, Model, os)
                : string.Format(@"{0}-{1}-{2}", Vendor, Model, os);
        }
    }
}
