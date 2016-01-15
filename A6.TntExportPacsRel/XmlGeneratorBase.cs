using System;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Xml.Linq;
using Kofax.ReleaseLib;
using Tnt.KofaxCapture.A6.TntExportPacsRel.Properties;

namespace Tnt.KofaxCapture.A6.TntExportPacsRel
{
    internal abstract class XmlGeneratorBase
    {
        private readonly CultureInfo _culture = CultureInfo.CurrentCulture;
        private const KfxInfoReturnValue DocMessage = KfxInfoReturnValue.KFX_REL_DOC_MESSAGE;
        private const int MaximumRetries = 10;
        private const int WaitBetweenRetries = 1 * 1000;

        /// <summary>
        /// Gets the LogMessage property.
        /// </summary>
        protected Action<string, KfxInfoReturnValue> LogMessage { get; }

        /// <summary>
        /// Gets or sets the Xml property.
        /// </summary>
        protected XDocument Xml { get; set; }

        /// <summary>
        /// Initializes a new instance of the Tnt.KofaxCapture.A6.TntExportPacsRel.XmlGeneratorBase class.
        /// </summary>
        /// <param name="logMessage">Delegate to allow logging.</param>
        protected XmlGeneratorBase(Action<string, KfxInfoReturnValue> logMessage)
        {
            if (logMessage == null) throw new ArgumentNullException(nameof(logMessage));
            LogMessage = logMessage;
        }

        /// <summary>
        /// Write the generated XML to disk.
        /// </summary>
        /// <param name="xmlFilePath">Path to write the XML to.</param>
        public void Save(string xmlFilePath)
        {
            if (xmlFilePath == null) throw new ArgumentNullException(nameof(xmlFilePath));

            var complete = false;
            var attempts = 0;

            while (!complete && attempts < MaximumRetries)
            {
                attempts++;

                try
                {
                    LogMessage(string.Format(_culture, Resources.WritingXml, xmlFilePath), DocMessage);
                    Xml.Save(xmlFilePath);
                    complete = true;
                }
                catch (IOException ex)
                {
                    LogMessage(string.Format(_culture, Resources.ErrorWritingXml, attempts, ex.Message), DocMessage);

                    if (attempts < MaximumRetries)
                    {
                        Thread.Sleep(WaitBetweenRetries);
                    }
                    else
                    {
                        throw new ExportException(string.Format(Resources.CannotWriteXml, ex.Message), ex);
                    }
                }
            }
        }
    }
}