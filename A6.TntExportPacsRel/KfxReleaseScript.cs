using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using Kofax.ReleaseLib;
using Tnt.KofaxCapture.A6.TntExportPacsRel.Properties;

namespace Tnt.KofaxCapture.A6.TntExportPacsRel
{
    [ClassInterface(ClassInterfaceType.None),
    Guid("6883A852-56B9-42FD-8535-F249FA55FBB7"),
    ProgId("A6.TntExportPacs.kfxReleaseScript"),
    ComVisible(true)]
    public class KfxReleaseScript : IKfxReleaseScript, IDisposable
    {
        private const StringComparison IgnoreCase = StringComparison.InvariantCultureIgnoreCase;
        private const KfxLinkSourceType BatchVar = KfxLinkSourceType.KFX_REL_BATCHFIELD;
        private const KfxLinkSourceType IndexVar = KfxLinkSourceType.KFX_REL_INDEXFIELD;
        private const KfxInfoReturnValue DocMessage = KfxInfoReturnValue.KFX_REL_DOC_MESSAGE;
        private const KfxInfoReturnValue ErrorMessage = KfxInfoReturnValue.KFX_REL_DOC_ERROR;

        private const int MaximumRetries = 10;
        private const int WaitBetweenRetries = 1 * 1000;
       
        private readonly string _codeModule = MethodBase.GetCurrentMethod().DeclaringType?.FullName;
        private readonly CultureInfo _culture = CultureInfo.CurrentCulture;
        
        private bool _disposed;
        private bool _open;
        private MainSettings _settings;
        private string _customLogFilePath;
        private int _documentCount;
        private MisAuditData _misAuditData;
        private string _startTime;
        private string _batchName;
        private readonly List<string> _outputFilePaths = new List<string>();
        private bool _batchError;
        private MeridioGenerator _meridioGenerator;
        private StandardAuditData _standardAuditData;

        /// <summary>
        /// Gets or set the DocumentData.
        /// </summary>
        public ReleaseData DocumentData { get; set; }

        /// <summary>
        /// Gets or sets whether to skip outputting to the custom log file.
        /// </summary>
        public bool SkipCustomLog { get; set; }

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

            _startTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            _misAuditData = null;
            _documentCount = 0;
            _outputFilePaths.Clear();
            _open = true;
            _batchError = false;
            _standardAuditData = new StandardAuditData();
            _meridioGenerator = null;

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

                DeleteOutputFilePaths();
                _batchError = true;
                return KfxReturnValue.KFX_REL_ERROR;
            }
            finally
            {
                _outputFilePaths.Clear();
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
            _outputFilePaths.Clear();
            LogMessage(Resources.ClosingScript, DocMessage);

            try
            {
                // Save the standard audit file.
                if (_misAuditData != null && _misAuditData.TotalDocumentCount > 0)
                {
                    OutputStandardAuditXml();

                    var auditMkrFilePath = GetMeridioFilePath("AuditMarker", "mkr");
                    WriteTextToDisk(auditMkrFilePath, string.Empty);
                }

                // Save the batch MKR files (only if no batch error).
                if (!_batchError)
                {
                    if (_misAuditData != null && _misAuditData.TotalDocumentCount > 0)
                    {
                        var meridioMkrFilePath = GetMeridioFilePath("BatchMarker", "mkr");
                        WriteTextToDisk(meridioMkrFilePath, string.Empty);
                    }
                }
                
                // Output the MIS audit XML if required.
                if (_misAuditData != null)
                {
                    OutputMisAuditXml(_misAuditData);
                }

                return KfxReturnValue.KFX_REL_SUCCESS;
            }
            catch (Exception ex)
            {
                var message = string.Format(Resources.ErrorFinalisingExport, ex.Message, ex.InnerException ?? ex);
                DocumentData.LogError(0, 0, 0, message, _codeModule + "." + Utility.GetCurrentMethod(), 0);
                LogMessage(message, ErrorMessage);

                DeleteOutputFilePaths();
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
            // Therefore, you should call GC.SupressFinalize to
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

            if (_meridioGenerator == null)
            {
                // If this is the first document, create the batch-level data.
                _misAuditData = GetMisAuditData("B_MISA6AuditPath", "A6.TntExportPacsRel");
                _standardAuditData = GetStandardAuditData();
                _meridioGenerator = new MeridioGenerator(_settings, (s, t) => LogMessage(s, t));
            }

            // Determine if we can run for this document, or whether to skip output.
            var skipOutput = IsDocSkippedForOutput();

            // No more processing for this document if we are skipping it.
            if (skipOutput) return;

            // Record the document's number of images.
            _misAuditData.TotalDocumentCount++;
            _misAuditData.TotalImageCount += DocumentData.ImageFiles.Count;

            // Copy the image to the output directory.
            var batchName = _settings.GetFieldValue("ExternalBatchName", IndexVar, true).CleanInvalidFileNameChars();
            var tiffFileName = $"BatchImage_{batchName}_{_documentCount.ToString().PadLeft(3, '0')}.tif";
            var tiffFilePath = Path.Combine(_settings.OutputDirectoryPath, tiffFileName);
            CopyDocumentToOutputDirectory(_settings.OutputDirectoryPath, tiffFilePath);

            // Add the document-level Meridio data.
            _meridioGenerator.AddDocumentLevelData(_settings, tiffFilePath);

            // Save the XML document in its current state (i.e. once for each document release).
            SaveMeridioFile();
        }

        /// <summary>
        /// Output the standard audit XML.
        /// </summary>
        private void OutputStandardAuditXml()
        {
            var auditFilePath = GetMeridioFilePath("AuditReport", "xml");
            var standardAuditGenerator = new StandardAuditGenerator(_standardAuditData, (s, v) => LogMessage(s, v));
            standardAuditGenerator.Save(auditFilePath);
        }

        /// <summary>
        /// Get the standard audit data.
        /// </summary>
        /// <returns>Standard audit data.</returns>
        private StandardAuditData GetStandardAuditData()
        {
            var batchDateTime = _settings.GetFieldValue<DateTime>("B_ScanDateTime", BatchVar, true);

            var standardAuditData = new StandardAuditData
            {
                DomainAndUserName = _settings.GetFieldValue("B_DomainAndUserName", BatchVar),
                MachineName = _settings.GetFieldValue("B_WorkstationName", BatchVar),
                Date = batchDateTime.ToString("dd/MM/yyyy"),
                Time = batchDateTime.ToString("HH:mm:ss")
            };
            return standardAuditData;
        }

        /// <summary>
        /// Generate and output the audit XML using the specify details.
        /// </summary>
        /// <param name="auditData">Audit XML details.</param>
        private void OutputMisAuditXml(MisAuditData auditData)
        {
            if (auditData == null) throw new ArgumentNullException(nameof(auditData));

            var misGenerator = new MisAuditGenerator(_batchName, auditData, (s, v) => LogMessage(s, v), _startTime);
            misGenerator.Save(auditData.AuditFilePath);
        }

        /// <summary>
        /// Save the Meridio XML file.
        /// </summary>
        private void SaveMeridioFile()
        {
            var meridioFilePath = GetMeridioFilePath("BatchControl", "xml");
            _meridioGenerator.Save(meridioFilePath);
        }

        /// <summary>
        /// Gets a Meridio file path.
        /// </summary>
        /// <param name="prefix">Prefix to assign.</param>
        /// <param name="fileExt">File extension.</param>
        /// <returns>Meridio file path.</returns>
        private string GetMeridioFilePath(string prefix, string fileExt)
        {
            if (prefix == null) throw new ArgumentNullException(nameof(prefix));
            if (fileExt == null) throw new ArgumentNullException(nameof(fileExt));

            var batchName = new StringBuilder(_batchName.CleanInvalidFileNameChars());

            if (prefix.StartsWith("Audit", IgnoreCase))
            {
                batchName[0] = 'A';
            }
            
            var meridioFileName = $"{prefix}_{batchName}.{fileExt}";
            var meridioFilePath = Path.Combine(_settings.OutputDirectoryPath, meridioFileName);
            return meridioFilePath;
        }

        /// <summary>
        /// Determine if we can run, or whether to skip output.
        /// </summary>
        /// <returns>True if the document should be skipped; false if not.</returns>
        private bool IsDocSkippedForOutput()
        {
            var docStatusValue = _settings.GetFieldValue("DocStatus", IndexVar);

            return !docStatusValue.Equals("SUCCESS", IgnoreCase);
        }

        /// <summary>
        /// Generate audit details for output during CloseScript().
        /// </summary>
        /// <param name="auditPathBatchFieldName">Audit path batch field name.</param>
        /// <param name="fallbackAuditDirectoryName">fallback audit directory name (used if the auditPathBatchFieldName
        /// batch field does not exist).</param>
        /// <returns>We generate part of the audit detail during ReleaseDoc() because index field values are not
        /// available during CloseScript().</returns>
        private MisAuditData GetMisAuditData(string auditPathBatchFieldName, 
                                             string fallbackAuditDirectoryName)
        {
            if (auditPathBatchFieldName == null) throw new ArgumentNullException(nameof(auditPathBatchFieldName));
            if (fallbackAuditDirectoryName == null) throw new ArgumentNullException(nameof(fallbackAuditDirectoryName));

            var computerName = Environment.MachineName.CleanInvalidFileNameChars();
            var batchName = _settings.BatchName.CleanInvalidFileNameChars();
            var auditFileName = $"{computerName}_A6.RELEASE.EXE_{batchName}_0.XML";
            var auditDirectoryPath = GetAuditDirectoryPath(auditPathBatchFieldName, fallbackAuditDirectoryName);

            var auditDetails = new MisAuditData()
            {
                AuditFilePath = Path.Combine(auditDirectoryPath, auditFileName),
                ScanDepot = _settings.GetFieldValue("B_ScanDepot", BatchVar, true),
                ScanDate = _settings.GetFieldValue<DateTime>("B_ScanDateTime", BatchVar, true).ToString("yyyy-MM-dd HH:mm:ss"),
                BatchType = _settings.GetFieldValue("B_DocumentType", BatchVar, true),
                RoundId = _settings.GetFieldValue("B_RoundID", BatchVar, true),
            };
            return auditDetails;
        }

        /// <summary>
        /// Retrieve the audit directory path.
        /// </summary>
        /// <param name="auditPathBatchFieldName">Audit path batch field name.</param>
        /// <param name="fallbackAuditDirectoryName">fallback audit directory name (used if the auditPathBatchFieldName
        /// batch field does not exist).</param>
        /// <returns>Audit directory path.</returns>
        private string GetAuditDirectoryPath(string auditPathBatchFieldName, string fallbackAuditDirectoryName)
        {
            var auditDirectoryPath = _settings.GetFieldValue(auditPathBatchFieldName, BatchVar);

            var commonAppPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData,
                Environment.SpecialFolderOption.DoNotVerify);
            var fallbackDirectoryPath = Path.Combine(commonAppPath, fallbackAuditDirectoryName);

            if (string.IsNullOrEmpty(auditDirectoryPath))
            {
                auditDirectoryPath = fallbackDirectoryPath;
            }

            if (!Directory.Exists(auditDirectoryPath))
            {
                try
                {
                    CreateDirectory(auditDirectoryPath);
                }
                catch (ExportException)
                {
                    // Can't create the audit directory.  Default to fallback, or if already using fallback, 
                    // theow exception as we're unable to write anything.
                    if (auditDirectoryPath.Equals(fallbackDirectoryPath, IgnoreCase))
                    {
                        throw;
                    }

                    auditDirectoryPath = fallbackDirectoryPath;
                    CreateDirectory(auditDirectoryPath);
                }
            }
            return auditDirectoryPath;
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
        /// Copy the document file to the directory.
        /// </summary>
        /// <param name="workingDirectoryPath">Directory path to copy the document to.</param>
        /// <param name="documentFilePath">Path to move the released document to after Kofax has copied it.</param>
        /// <returns>Path to the document.</returns>
        private void CopyDocumentToOutputDirectory(string workingDirectoryPath, string documentFilePath)
        {
            if (string.IsNullOrEmpty(workingDirectoryPath)) throw new ArgumentNullException(nameof(workingDirectoryPath));
            if (string.IsNullOrEmpty(documentFilePath)) throw new ArgumentNullException(nameof(documentFilePath));

            var complete = false;
            var attempts = 0;

            while (!complete && attempts < MaximumRetries)
            {
                attempts++;

                try
                {
                    LogMessage(Resources.CopyingDocument, DocMessage);

                    // Output the TIFF file.
                    DocumentData.ImageFiles.Copy(workingDirectoryPath);
                    var sourceFilePath = DocumentData.ImageFiles.ReleasedDirectory;

                    CheckFileCopied(sourceFilePath);
                    _outputFilePaths.Add(sourceFilePath);
                    LogMessage(string.Format(Resources.FileCopiedTo, sourceFilePath), DocMessage);

                    MoveFile(sourceFilePath, documentFilePath);
                    _outputFilePaths[_outputFilePaths.Count - 1] = documentFilePath;
                    complete = true;
                }
                catch (COMException ex)
                {
                    LogMessage(string.Format(_culture, Resources.ErrorCopyingDocument, attempts, ex.Message), DocMessage);

                    if (attempts < MaximumRetries)
                    {
                        Thread.Sleep(WaitBetweenRetries);
                    }
                    else
                    {
                        throw;
                    }
                }
                catch (IOException ex)
                {
                    LogMessage(string.Format(_culture, Resources.ErrorMovingDoc, attempts, ex.Message), DocMessage);

                    if (attempts < MaximumRetries)
                    {
                        Thread.Sleep(WaitBetweenRetries);
                    }
                    else
                    {
                        throw;
                    }
                }
            }
        }


        /// <summary>
        /// Create the specified directory.
        /// </summary>
        /// <param name="directoryPath">Path to the directory to create.</param>
        private static void CreateDirectory(string directoryPath)
        {
            if (directoryPath == null) throw new ArgumentNullException(nameof(directoryPath));

            var retries = 0;

            while (retries < MaximumRetries)
            {
                try
                {
                    Directory.CreateDirectory(directoryPath);
                    return;
                }
                catch (IOException)
                {
                    retries++;
                }
                catch (UnauthorizedAccessException)
                {
                    retries++;
                }
                catch (ArgumentException)
                {
                    retries++;
                }
                catch (NotSupportedException)
                {
                    retries++;
                }

                Thread.Sleep(WaitBetweenRetries);
            }

            throw new ExportException(string.Format(Resources.DirectoryCreationMaxFailure, directoryPath,
                MaximumRetries));
        }

        /// <summary>
        /// Move the specified source file to the destination file, deleting the destination first if it already exists.
        /// </summary>
        /// <param name="sourceFilePath">Source file path.</param>
        /// <param name="destFilePath">Destination file path.</param>
        private void MoveFile(string sourceFilePath, string destFilePath)
        {
            if (sourceFilePath == null) throw new ArgumentNullException(nameof(sourceFilePath));
            if (destFilePath == null) throw new ArgumentNullException(nameof(destFilePath));

            LogMessage(string.Format(_culture, Resources.MovingFile, sourceFilePath, destFilePath), DocMessage);
            if (File.Exists(destFilePath))
            {
                LogMessage(string.Format(_culture, Resources.DeletingExistingFile, destFilePath), DocMessage);
                File.Delete(destFilePath);
            }

            File.Move(sourceFilePath, destFilePath);
        }

        /// <summary>
        /// Check that the specified file has been copied and exists on disk.
        /// </summary>
        /// <param name="filePath">File path to check.</param>
        private void CheckFileCopied(string filePath)
        {
            if (filePath == null) throw new ArgumentNullException(nameof(filePath));

            if (!File.Exists(filePath))
            {
                throw new ExportException(string.Format(_culture, Resources.CouldNotFindExpectedFile,
                                                              filePath));
            }
        }

        /// <summary>
        /// Get the settings required for release.
        /// </summary>
        private void ReadSettings()
        {
            LogMessage(Resources.GettingSettings, DocMessage);
            _settings = new MainSettings(DocumentData);
            _batchName = _settings.GetFieldValue("ExternalBatchName", IndexVar, true);
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

        /// <summary>
        /// Delete the output file paths.
        /// </summary>
        private void DeleteOutputFilePaths()
        {
            foreach (var outputFilePath in _outputFilePaths)
            {
                Utility.DeleteNonCriticalFile(outputFilePath);
            }
        }
    }
}
