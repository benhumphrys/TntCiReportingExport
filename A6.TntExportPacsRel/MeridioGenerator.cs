//#define SupportAllFields

using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Xml.Linq;
using Kofax.ReleaseLib;

#if SupportAllFields
using System.Collections.Generic;
using System.Linq;
#endif

namespace Tnt.KofaxCapture.A6.TntExportPacsRel
{
    /// <summary>
    /// Generates Merio XML.
    /// </summary>
    internal class MeridioGenerator : XmlGeneratorBase
    {
        private const KfxLinkSourceType BatchVar = KfxLinkSourceType.KFX_REL_BATCHFIELD;
        private const KfxLinkSourceType IndexVar = KfxLinkSourceType.KFX_REL_INDEXFIELD;

        /// <summary>
        /// Initializes a new instance of the Tnt.KofaxCapture.A6.TntExportPacsRel.MeridioGenerator class.
        /// </summary>
        /// <param name="settings">Current settings.</param>
        /// <param name="logMessage">Delegate to allow logging.</param>
        public MeridioGenerator(MainSettings settings, Action<string, KfxInfoReturnValue> logMessage) : base(logMessage)
        {
            if (settings == null) throw new ArgumentNullException(nameof(settings));

            Xml =
                new XDocument(new XElement("CDSRP",
                    new XElement("scanrelease",
                        new XElement("batch",
                            new XAttribute("id", settings.GetFieldValue("ExternalBatchName", IndexVar, true)),
                            GetProperty("B_DocumentType", BatchVar, "100", settings),
                            GetProperty("B_ScanDepot", BatchVar, "101", settings),
                            GetProperty("B_ScanDateTime", BatchVar, "102", settings),
                            GetProperty("B_ReScanFlag", BatchVar, "104", settings),
                            GetProperty("B_WorkstationName", BatchVar, "106", settings),
                            GetProperty("B_BlankSheetsScanned", BatchVar, "107", settings),
                            GetProperty("B_StoreinDMS", BatchVar, "109", settings)
#if SupportAllFields
                            ,
                            GetProperty("SEND TO 3RD PARTY", BatchVar, "108", settings),
                            GetProperty("ACTION", BatchVar, "501", settings),
                            GetProperty("BATCH TYPE", BatchVar, "502", settings)
#endif
                            ))));
        }

        /// <summary>
        /// Add index data for the current document to the generator.
        /// </summary>
        /// <param name="settings">Current settings.</param>
        /// <param name="imageFilePath">File path to the image.</param>
        public void AddDocumentLevelData(MainSettings settings, string imageFilePath)
        {
            if (settings == null) throw new ArgumentNullException(nameof(settings));
            if (imageFilePath == null) throw new ArgumentNullException(nameof(imageFilePath));

            var batchElement = GetBatchElement();

            batchElement.Add(
                new XElement("document",
                    GetProperty("203", Path.GetFileName(imageFilePath)),
                    GetProperty("ConNumber", IndexVar, "200", settings),
                    GetProperty("TotalNumberofPages", IndexVar, "201", settings),
                    GetProperty("AutoIndexedFlag", IndexVar, "202", settings)
#if SupportAllFields
                    ,
                    GetConsignmentNumbers(settings),
                    GetProperty("MAWB/CBV NUMBER", IndexVar, "204", settings),
                    GetMawbSectorRefs(settings),
                    GetProperty("FLIGHT DEPARTURE DATE", IndexVar, "206", settings),
                    GetProperty("RUNSHEET IDENTIFIER", IndexVar, "220", settings),
                    GetProperty("RUNSHEET TYPE", IndexVar, "221", settings),
                    GetProperty("TOTAL PAGES", IndexVar, "222", settings),
                    GetProperty("TRUCK ID / ROAD", IndexVar, "223", settings),
                    GetProperty("DELIVERY DEPOT", IndexVar, "224", settings),
                    GetProperty("RUNSHEET DATE", IndexVar, "225", settings),
                    GetProperty("PAGE NUMBER", IndexVar, "226", settings),
                    GetProperty("NUMBER OF CONSIGNMENTS ON PAGE", IndexVar, "227", settings),
                    GetPodConsignmentNumbers(settings)
#endif
                    ));
        }

#if SupportAllFields
        /// <summary>
        /// Gets the Consignment Numbers (multiple field values) if any exist.
        /// </summary>
        /// <param name="settings">Current settings.</param>
        /// <returns>Consignment Numbers (multiple field values) if any exist.</returns>
        private IEnumerable<XElement> GetConsignmentNumbers(MainSettings settings)
        {
            if (settings == null) throw new ArgumentNullException(nameof(settings));

            var conNumbers = new List<XElement>();

            for (var i = 1; i <= 17; i++)
            {
                var fieldName = $"CONSIGNMENT NUMBERS{i}";
                var fieldValue = settings.GetFieldValue(fieldName, IndexVar);
                if (string.IsNullOrEmpty(fieldValue)) continue;

                var values = fieldValue.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                conNumbers.AddRange(values.Select(v => GetProperty("200", v)));
            }

            return conNumbers;
        }

        /// <summary>
        /// Gets the MAWB/CBV Sector Refs (multiple field values) if any exist.
        /// </summary>
        /// <param name="settings">Current settings.</param>
        /// <returns>MAWB/CBV Sector Refs (multiple field values) if any exist.</returns>
        private IEnumerable<XElement> GetMawbSectorRefs(MainSettings settings)
        {
            if (settings == null) throw new ArgumentNullException(nameof(settings));

            var sectorRefs = new List<XElement>();

            for (var i = 1; i <= 20; i++)
            {
                var fieldName = $"MAWB/CBV SECTOR REF.{i}";
                var fieldValue = settings.GetFieldValue(fieldName, IndexVar);
                if (string.IsNullOrEmpty(fieldValue)) continue;

                sectorRefs.Add(GetProperty("205", fieldValue));
            }

            return sectorRefs;
        }

        /// <summary>
        /// Gets the POD Consignment Numbers (multiple field values) if any exist.
        /// </summary>
        /// <param name="settings">Current settings.</param>
        /// <returns>POD Consignment Numbers (multiple field values) if any exist.</returns>
        private IEnumerable<XElement> GetPodConsignmentNumbers(MainSettings settings)
        {
            if (settings == null) throw new ArgumentNullException(nameof(settings));

            var conNumbers = new List<XElement>();

            for (var i = 0; i < 10; i++)
            {
                var fieldName = $"POD CONSIGNMENT NUMBER {i + 1}";
                var fieldValue = settings.GetFieldValue(fieldName, IndexVar);
                if (string.IsNullOrEmpty(fieldValue)) continue;

                var conNumber = 228 + i;
                conNumbers.Add(GetProperty($"{conNumber}", fieldValue));
            }

            return conNumbers;
        }
#endif

        /// <summary>
        /// Get the batch element from the XML.
        /// </summary>
        /// <returns>Batch XElement.</returns>
        [SuppressMessage("ReSharper", "PossibleNullReferenceException")]
        private XElement GetBatchElement()
        {
            var batchElement = Xml.Element("CDSRP").Element("scanrelease").Element("batch");
            return batchElement;
        }

        /// <summary>
        /// Gets a Meridio XML property based upon the specified field name and type, and property ID.
        /// </summary>
        /// <param name="fieldName">Name of the index field that the property represents.</param>
        /// <param name="sourceType">Type of the index field.</param>
        /// <param name="propertyId">ID of the property.</param>
        /// <param name="settings">Current settings.</param>
        /// <returns>Meridio property XElement, or null of the specified field does not exist, or has no value.
        /// </returns>
        private XElement GetProperty(string fieldName, KfxLinkSourceType sourceType, string propertyId,
            MainSettings settings)
        {
            if (fieldName == null) throw new ArgumentNullException(nameof(fieldName));
            if (propertyId == null) throw new ArgumentNullException(nameof(propertyId));
            if (settings == null) throw new ArgumentNullException(nameof(settings));
            if (!Enum.IsDefined(typeof (KfxLinkSourceType), sourceType))
                throw new ArgumentOutOfRangeException(nameof(sourceType));

            var value = settings.GetFieldValue(fieldName, sourceType);
            return string.IsNullOrEmpty(value) ? null : GetProperty(propertyId, value);
        }

        /// <summary>
        /// Gets a Meridiio XML property based upon the specified value and ID.
        /// </summary>
        /// <param name="propertyId">ID of the property.</param>
        /// <param name="value">Value of the property.</param>
        /// <returns>Meridio property XElement.</returns>
        private static XElement GetProperty(string propertyId, string value)
        {
            if (propertyId == null) throw new ArgumentNullException(nameof(propertyId));
            if (value == null) throw new ArgumentNullException(nameof(value));

            var element =
                new XElement("property",
                    new XElement("id", propertyId),
                    new XElement("value",
                        new XCData(value)));
            return element;
        }
    }
}
