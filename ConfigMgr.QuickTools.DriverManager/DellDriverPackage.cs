using ByteSizeLib;
using System;
using System.Xml.Linq;

namespace ConfigMgr.QuickTools.DriverManager
{
    internal class DellDriverPackage
    {
        private readonly XElement packageObject;
        private readonly XNamespace ns;
        public string Name { get; private set; }
        public string Model { get; private set; }
        public string Version { get; private set; }
        public string VersionFile { get; private set; }
        public string FolderName { get; private set; }
        public string Url { get; private set; }
        public string ReleaseNotesUrl { get; private set; }
        public ByteSize Size { get; private set; }

        public DellDriverPackage(XElement pack)
        {
            packageObject = pack;

            ns = packageObject.GetDefaultNamespace();

            Name = Model = packageObject.Element(ns + "SupportedSystems").Element(ns + "Brand").Element(ns + "Model").Attribute("name").Value;

            Size = ByteSize.FromBytes(double.Parse(packageObject.Attribute("size").Value));

            Version = packageObject.Attribute("dellVersion").Value;
            VersionFile = Version + ".version";

            Url = packageObject.Attribute("path").Value;

            ReleaseNotesUrl = packageObject.Element(ns + "ImportantInfo").Attribute("URL").Value;
        }

        public string GenerateModelFolderName(string os, string structure)
        {
            return FolderName = (string.IsNullOrEmpty(structure) ? false : Convert.ToBoolean(structure))
                ? string.Format(@"{0}\{1}\{2}", "Dell Inc", Model, os)
                : string.Format(@"{0}-{1}-{2}", "Dell Inc", Model, os);
        }
    }
}
