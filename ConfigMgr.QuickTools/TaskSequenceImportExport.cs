using Microsoft.ConfigurationManagement.AdminConsole;
using Microsoft.ConfigurationManagement.ManagementProvider;
using Microsoft.ConfigurationManagement.AdminConsole.Schema;
using System.Windows.Forms;
using System.Xml;
using Microsoft.ConfigurationManagement.AdminConsole.Common;
using System.Xml.Linq;

namespace ConfigMgr.QuickTools
{
    class TaskSequenceImportExport
    {
        public static void RunTaskSequenceExport(object sender, ScopeNode scopeNode, ActionDescription action, IResultObject selectedResultObjects, PropertyDataUpdated dataUpdatedDelegate, Status status)
        {
            using (XmlReader readerFromString = SecureXml.CreateXmlReaderFromString(selectedResultObjects["Sequence"].StringValue))
            {
                XElement taskSequencePackage =
                    new XElement("TaskSequencePackage",
                        new XElement("PackageID", selectedResultObjects["PackageID"].StringValue),
                        new XElement("BootImageID", selectedResultObjects["BootImageID"].StringValue),
                        new XElement("Category", selectedResultObjects["Category"].StringValue),
                        new XElement("DependentProgram", selectedResultObjects["DependentProgram"].StringValue),
                        new XElement("Description", selectedResultObjects["Description"].StringValue),
                        new XElement("Duration", selectedResultObjects["Duration"].IntegerValue),
                        new XElement("Name", selectedResultObjects["Name"].StringValue),
                        new XElement("SourceDate", selectedResultObjects["SourceDate"].DateTimeValue),
                        new XElement("SequenceData",
                                XElement.Load(readerFromString)
                            )
                    );

                SaveFileDialog saveFileDialog1 = new SaveFileDialog
                {
                    Filter = "XML files(.xml)|*.xml",
                    Title = "Export Task Sequnce to XML",
                    FileName = selectedResultObjects["PackageID"].StringValue
                };
                saveFileDialog1.ShowDialog();

                if (saveFileDialog1.FileName != "")
                {
                    taskSequencePackage.Save(saveFileDialog1.FileName);
                }
            }
        }
    }
}
