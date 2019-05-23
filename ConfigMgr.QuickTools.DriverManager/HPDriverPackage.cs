using ByteSizeLib;
using System;
using System.Xml.Linq;

namespace ConfigMgr.QuickTools.DriverManager
{
    internal class HPDriverPackage
    {
        private readonly XElement packageObject;
        public string Name { get; private set; }
        public string Model { get; private set; }
        public string Version { get; private set; }
        public string VersionShort { get; private set; }
        public string VersionFile { get; private set; }   
        public string FolderName { get; private set; }
        public string Url { get; private set; }
        public string ReleaseNotesUrl { get; private set; }
        public ByteSize Size { get; private set; }
        public XElement SoftPaq { get; set; }

        public HPDriverPackage(XElement pack)
        {
            packageObject = pack;

            Name = packageObject.Element("SystemName").Value;
            Model = Name.TrimStart("HP").TrimEnd("PC").Trim().Split('(')[0];
        }

        public void ProcessSoftPaq()
        {
            Size = ByteSize.FromBytes(double.Parse(SoftPaq.Element("Size").Value));
            Version = SoftPaq.Element("Version").Value;
            VersionShort = Version.TrimEnd("A 1").Trim();
            VersionFile = Version + ".version";
            Url = SoftPaq.Element("Url").Value;
            ReleaseNotesUrl = SoftPaq.Element("ReleaseNotesUrl").Value;
        }

        public string GenerateModelFolderName(string os, string structure)
        {
            return FolderName = (string.IsNullOrEmpty(structure) ? false : Convert.ToBoolean(structure))
                ? string.Format(@"{0}\{1}\{2}", "HP", Model, os)
                : string.Format(@"{0}-{1}-{2}", "HP", Model, os);
        }
    }
}

static class StringTrimExtension
{
    public static string TrimStart(this string value, string toTrim)
    {
        if (value.StartsWith(toTrim))
        {
            int startIndex = toTrim.Length;
            return value.Substring(startIndex);
        }
        return value;
    }

    public static string TrimEnd(this string value, string toTrim)
    {
        if (value.EndsWith(toTrim))
        {
            int startIndex = toTrim.Length;
            return value.Substring(0, value.Length - startIndex);
        }
        return value;
    }
}
