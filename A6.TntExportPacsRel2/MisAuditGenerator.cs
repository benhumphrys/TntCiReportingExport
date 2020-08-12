using System;
using System.Xml.Linq;
using Kofax.ReleaseLib;

namespace Tnt.KofaxCapture.A6.TntExportPacsRel
{
    /// <summary>
    /// Generates MIS Audit XML.
    /// </summary>
    internal sealed class MisAuditGenerator : XmlGeneratorBase
    {
        /// <summary>
        /// Initializes a new instance of the Tnt.KofaxCapture.A6.TntExportPacsRel.XmlGeneratorBase class.
        /// </summary>
        /// <param name="batchName">Name of the current batch.</param>
        /// <param name="auditData">Audit data to output/</param>
        /// <param name="logMessage">Delegate to allow logging.</param>
        /// <param name="startTime">Start time of batch processing.</param>
        public MisAuditGenerator(string batchName, MisAuditData auditData, 
            Action<string, KfxInfoReturnValue> logMessage, string startTime) : base(logMessage)
        {
            if (batchName == null) throw new ArgumentNullException(nameof(batchName));
            if (auditData == null) throw new ArgumentNullException(nameof(auditData));
            if (startTime == null) throw new ArgumentNullException(nameof(startTime));

            var declaration = new XDeclaration("1.0", "utf-8", null);
            XNamespace xsi = "http://www.w3.org/2001/XMLSchema-instance";
            XNamespace xsd = "http://www.w3.org/2001/XMLSchema";

            var batchInfosElement = new XElement("batchinfos",
                new XAttribute(XNamespace.Xmlns + "xsi", xsi),
                new XAttribute(XNamespace.Xmlns + "xsd", xsd),
                new XElement("batchinfo",
                    new XAttribute("processname", "TNT.AvatarA6.PACSExport"),
                    GetFieldElement("BatchName", "string", batchName),
                    GetFieldElement("MachineName", "string", Environment.MachineName),
                    GetFieldElement("Mode", "string", "SERVICE"),
                    GetFieldElement("StartTime", "datetime", startTime),
                    GetFieldElement("FinishTime", "datetime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
                    GetFieldElement("UserDomain", "string", Environment.UserDomainName),
                    GetFieldElement("UserName", "string", Environment.UserName),
                    GetFieldElement("ScanDepot", "string", auditData.ScanDepot),
                    GetFieldElement("DepotCode", "string", auditData.ScanDepot),
                    GetFieldElement("ScanDate", "string", auditData.ScanDate),
                    GetFieldElement("BatchType", "string", auditData.BatchType),
                    GetFieldElement("RoundID", "string", auditData.RoundId),
                    GetFieldElement("ImageCount", "integer", auditData.TotalImageCount.ToString()),
                    GetFieldElement("DocumentCount", "integer", auditData.TotalDocumentCount.ToString()),
                    GetFieldElement("Status", "string", "Complete")));

            Xml = new XDocument(declaration, batchInfosElement);
        }

        /// <summary>
        /// Retrieve a field XElement based upon the name and value.
        /// </summary>
        /// <param name="name">Name of field.</param>
        /// <param name="dataType">Data type of the field.</param>
        /// <param name="value">Value of field.</param>
        /// <returns>Field XElement.</returns>
        private XElement GetFieldElement(string name, string dataType, string value)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));
            if (value == null) throw new ArgumentNullException(nameof(value));

            return
                new XElement("field",
                    new XAttribute("name", name),
                    new XElement("value",
                        new XAttribute("datatype", dataType),
                        value));
        }
    }
}
