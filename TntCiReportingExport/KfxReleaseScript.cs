using System;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using Kofax.ReleaseLib;
using Tnt.KofaxCapture.TntCiReportingExport.Properties;

namespace Tnt.KofaxCapture.TntCiReportingExport
{
    [ClassInterface(ClassInterfaceType.None),
    Guid("29700FE6-93F9-45EC-98AB-B83B0BB06EC8"),
    ProgId("TntCiReportingExport.kfxReleaseScript"),
    ComVisible(true)]
    public class KfxReleaseScript : IKfxReleaseScript, IDisposable
    {
        private const KfxInfoReturnValue DocMessage = KfxInfoReturnValue.KFX_REL_DOC_MESSAGE;
        private const KfxInfoReturnValue ErrorMessage = KfxInfoReturnValue.KFX_REL_DOC_ERROR;

        private const int MaximumRetries = 10;
        private const int WaitBetweenRetries = 1 * 1000;
       
        private readonly string _codeModule = MethodBase.GetCurrentMethod()?.DeclaringType?.FullName;
        private readonly CultureInfo _culture = CultureInfo.CurrentCulture;
        
        private bool _disposed;
        private bool _open;
        private MainSettings _settings;
        private string _customLogFilePath;
        private int _documentCount;

        /// <summary>
        /// Gets or set the DocumentData.
        /// </summary>
        public ReleaseData DocumentData { get; set; }

        /// <summary>
        /// Gets or sets whether to skip outputting to the custom log file.
        /// </summary>
        public bool SkipCustomLog { get; set; }

        /// <summary>
        /// Gets or sets the PaddedDocumentCount property.
        /// </summary>
        private string PaddedDocumentCount => _documentCount.ToStringInv().PadLeft(3, '0');

        /// <summary>
        /// Script initialization point.  Perform any necessary initialization such as
        /// logging in to a remote data source, allocating resources, etc.
        /// </summary>
        /// <returns> 
        /// One of the following:
        ///    KFX_REL_SUCCESS, KFX_REL_ERROR,
        ///    KFX_REL_FATALERROR, KFX_REL_REINIT
        ///    KFX_REL_DOCCLASSERROR
        /// </returns>
        /// <remarks>
        /// Called by the Release Controller once when the script object is loaded.
        /// </remarks>
        public KfxReturnValue OpenScript()
        {
            SetupCustomLogFilePath();
            LogMessage(Resources.OpeningScript, DocMessage);
            LogMessage(string.Format(_culture, Resources.ProcessingBatch, DocumentData.BatchName), DocMessage);
            
            _documentCount = 0;
            _open = true;

            return KfxReturnValue.KFX_REL_SUCCESS;
        }

        /// <remarks>  
        /// Document release point.  Use the ReleaseData object to release the
        /// current document's data to the external data repository.
        /// </remarks> 
        /// <returns> 
        /// One of the following:
        ///   KFX_REL_SUCCESS, KFX_REL_ERROR,
        ///   KFX_REL_FATALERROR, KFX_REL_REINIT
        ///   KFX_REL_DOCCLASSERROR,
        /// </returns> 
        /// <remark>    
        /// Called by the Release Controller once for each document to be released.
        /// </remark> 
        public KfxReturnValue ReleaseDoc()
        {
            try
            {
                if (!_open) throw new ExportException(Resources.BadReleaseDocCall);
                LogMessage(string.Format(_culture, Resources.ProcessingDocumentN, _documentCount + 1), DocMessage);

                ReleaseDocInternal();
                return KfxReturnValue.KFX_REL_SUCCESS;
            }
            catch (Exception ex)
            {
                // Catch-all exception to ensure that the batch is properly rejected.
                var message = string.Format(Resources.ErrorExportingDocument, ex.Message, ex.InnerException ?? ex);

                DocumentData.LogError(0, 0, 0, message, _codeModule + "." + Utility.GetCurrentMethod(), 0);
                LogMessage(message, ErrorMessage);
                return KfxReturnValue.KFX_REL_ERROR;
            }
        }

        /// <summary>
        /// Script release point. Perform any  necessary cleanup such as releasing resources, etc.
        /// </summary>
        /// <returns>
        /// One of the following:
        ///    KFX_REL_SUCCESS, KFX_REL_ERROR,
        ///    KFX_REL_FATALERROR, KFX_REL_REINIT
        ///    KFX_REL_DOCCLASSERROR,
        /// </returns>
        /// <remarks>
        /// Called by Release Setup Controller once just before the script object is released.
        /// </remarks>
        public KfxReturnValue CloseScript()
        {
            if (!_open) throw new ExportException(Resources.BadCloseScriptCall);

            _open = false;
            LogMessage(Resources.ClosingScript, DocMessage);

            try
            {

                return KfxReturnValue.KFX_REL_SUCCESS;
            }
            finally
            {
                _open = false;

                // Dispose the rest of the object.
                Dispose();
            }
        }

        /// <summary>
        /// Called by framework when it has finished with the object.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            // This object will be cleaned up by the Dispose method.
            // Therefore, you should call GC.SuppressFinalize to
            // take this object off the finalization queue
            // and prevent finalization code for this object
            // from executing a second time.
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Dispose(bool disposing) executes in two distinct scenarios.
        /// If disposing equals true, the method has been called directly
        /// or indirectly by a user's code. Managed and unmanaged resources
        /// can be disposed.
        /// If disposing equals false, the method has been called by the
        /// runtime from inside the finalizer and you should not reference
        /// other objects. Only unmanaged resources can be disposed.
        /// </summary>
        /// <param name="disposing"></param>
        private void Dispose(bool disposing)
        {
            // Check to see if Dispose has already been called.
            if (_disposed) return;

            // If disposing equals true, dispose all managed
            // and unmanaged resources.  
            if (disposing)
            {
                // Dispose managed resources.
            }

            // Call the appropriate methods to clean up unmanaged resources here.
            // If disposing is false, only the following code is executed.

            // Record that disposing has been done.
            _disposed = true;
        }

        /// <summary>
        /// This destructor will run only if the Dispose method
        /// does not get called.
        /// </summary>
        ~KfxReleaseScript()
        {
            // Do not re-create Dispose clean-up code; just call Dispose().
            Dispose(false);
        }

        /// <summary>
        /// Insert the document using the UCM webservice.
        /// </summary>
        private void ReleaseDocInternal()
        {
            LogMessage(Resources.BeginningExport, DocMessage);
            _documentCount++;

            // Get the settings and arrange the link values so that they are easy to retrieve.
            ReadSettings();

            // Read the document-level CSS, if it exists, and output it to disk.  If it does not exist, do
            // nothing.
            LogMessage(Resources.GettingXml, DocMessage);
            var reportXml = _settings.GetReportXml(this);

            if (string.IsNullOrEmpty(reportXml))
            {
                LogMessage(Resources.NoReportXml, DocMessage);
                return;
            }

            var computerName = Environment.MachineName.CleanInvalidFileNameChars();
            var batchName = _settings.BatchName.CleanInvalidFileNameChars();
            var reportFileName = $"{computerName}_QAQC_{batchName}_{DateTime.Now:yyyy-MM-dd-HH-mm-ss}";
            var reportFilePath = Path.Combine(_settings.OutputDirectoryPath, reportFileName);
            LogMessage(string.Format(Resources.WritingXml, reportFilePath), DocMessage);
            WriteTextToDisk(reportFilePath, reportXml);
        }

        /// <summary>
        /// Attempt to get the log file path for use during CloseScript() (as SendMessage() won't work there).
        /// </summary>
        private void SetupCustomLogFilePath()
        {
            if (_customLogFilePath == null && !SkipCustomLog)
            {
                _customLogFilePath = Utility.GetLogFilePath();
            }
        }

        /// <summary>
        /// Write the text file to to disk.
        /// </summary>
        /// <param name="textFilePath">File path to write the text to.</param>
        /// <param name="contents">Text contents to write to disk.</param>
        private void WriteTextToDisk(string textFilePath, string contents)
        {
            if (contents == null) throw new ArgumentNullException(nameof(contents));
            if (textFilePath == null) throw new ArgumentNullException(nameof(textFilePath));

            var complete = false;
            var attempts = 0;

            while (!complete && attempts < MaximumRetries)
            {
                attempts++;

                try
                {
                    LogMessage(string.Format(_culture, Resources.WritingTextFile, textFilePath), DocMessage);
                    File.WriteAllText(textFilePath, contents);
                    complete = true;
                }
                catch (IOException ex)
                {
                    LogMessage(string.Format(_culture, Resources.ErrorWritingText, attempts, ex.Message), DocMessage);

                    if (attempts < MaximumRetries)
                    {
                        Thread.Sleep(WaitBetweenRetries);
                    }
                    else
                    {
                        throw new ExportException(string.Format(Resources.CannotWriteText, ex.Message), ex);
                    }
                }
            }
        }
        
        /// <summary>
        /// Get the settings required for release.
        /// </summary>
        private void ReadSettings()
        {
            LogMessage(Resources.GettingSettings, DocMessage);
            _settings = new MainSettings(DocumentData);
        }
        
        /// <summary>
        /// Log the specified message to the screen, and also the KC error log.
        /// </summary>
        /// <param name="message">Message to log.</param>
        /// <param name="messageType">Type of message to log.</param>
        /// <param name="errorNumber">Error number to log.</param>
        private void LogMessage(string message, KfxInfoReturnValue messageType, int errorNumber = 0)
        {
            if (message == null) return;

            if (_open)
            {
                DocumentData.SendMessage(message, errorNumber, messageType);
            }

            if (messageType == ErrorMessage)
            {
                DocumentData.LogError(-1, 0, 0, message, _codeModule, 0);
            }

            if (_customLogFilePath != null)
            {
                WriteCustomLogEntry(message);
            }
        }

        /// <summary>
        /// Write to the custom log file.
        /// </summary>
        /// <param name="message">Message to write.</param>
        private void WriteCustomLogEntry(string message)
        {
            if (message == null) return;

            try
            {
                var logMessage = string.Format(_culture, "[{0}] {1}\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                    message);
                File.AppendAllText(_customLogFilePath, logMessage);
            }
                // ReSharper disable once EmptyGeneralCatchClause
            catch (Exception)
            {
                // Never error when writing to the log.
            }
        }
    }
}
