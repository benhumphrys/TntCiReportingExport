using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Kofax.ReleaseLib;

namespace Tnt.KofaxCapture.A6.TntExportPacsRel
{
    /// <summary>
    /// Generates standard Audit XML.
    /// </summary>
    internal sealed class StandardAuditGenerator : XmlGeneratorBase
    {
        /// <summary>
        /// Initializes a new instance of the Tnt.KofaxCapture.A6.TntExportPacsRel.XmlGeneratorBase class.
        /// </summary>
        /// <param name="auditData">Audit data.</param>
        /// <param name="logMessage">Delegate to allow logging.</param>
        public StandardAuditGenerator(StandardAuditData auditData, Action<string, KfxInfoReturnValue> logMessage) : base(logMessage)
        {
            if (auditData == null) throw new ArgumentNullException(nameof(auditData));

            Xml =
                new XDocument(
                    new XElement("cdsrp",
                        new XElement("scanneraudit",
                            new XElement("action", "1"),
                            new XElement("username",
                                new XCData(auditData.DomainAndUserName)),
                            new XElement("workstation",
                                new XCData(auditData.MachineName)),
                            new XElement("data",
                                new XCData(auditData.Date)),
                            new XElement("time",
                                new XCData(auditData.Time)))));
        }
    }
}
