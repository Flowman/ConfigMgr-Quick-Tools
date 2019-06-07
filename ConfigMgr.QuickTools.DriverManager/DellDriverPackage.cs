using ByteSizeLib;
using System.Xml.Linq;

namespace ConfigMgr.QuickTools.DriverManager
{
    internal class DellDriverPackage : Package
    {
        private readonly XElement packageObject;
        private readonly XNamespace ns;

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
    }
}
