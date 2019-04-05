using Microsoft.ConfigurationManagement.ManagementProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.IO;
using System.Collections;
using System.Collections.ObjectModel;
using System.Security.Cryptography;
using System.Text;

namespace ConfigMgr.QuickTools.DriverManager
{
    class DriverPackage
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
        public bool Import { get; private set; }
        public Dictionary<string, Driver> Drivers { get; private set; } = new Dictionary<string, Driver>();
        #endregion

        public DriverPackage(ConnectionManagerBase connection, string name, string source, string target)
        {
            connectionManager = connection;
            Name = name;
            Source = source;
            Target = target;

            Hash = CreateMd5ForFolder(Source);
            Import = !File.Exists(Path.Combine(Source, Hash + ".hash"));
            Infs = Directory.GetFiles(Source, "*.inf", SearchOption.AllDirectories);
            Infs = Infs.Where(x => Path.GetFileName(x) != "autorun.inf").ToArray();
        }

        private string CreateMd5ForFolder(string path)
        {
            // assuming you want to include nested folders
            List<string> files = Directory.GetFiles(path, "*.*", SearchOption.AllDirectories).Where(name => !name.EndsWith(".hash")).OrderBy(p => p).ToList();

            string hashFile = Path.Combine(Path.GetTempPath(), Name + ".hash");

            string hash = null;

            MD5 md5 = MD5.Create();

            if (files.Count > 0)
            {
                // Create a new file     
                using (FileStream fs = File.Create(hashFile))
                {
                    StreamWriter writer = new StreamWriter(fs);
                    for (int i = 0; i < files.Count; i++)
                    {
                        string file = files[i];
                        string content = file + " " + file.Length + Environment.NewLine;
                        writer.Write(content);
                    }
                    writer.Close();
                }

                hash = BitConverter.ToString(md5.ComputeHash(Encoding.UTF8.GetBytes(File.ReadAllText(hashFile)))).Replace("-", "");
                //File.Delete(hashFile);
            }

            return hash;
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

    }
}
