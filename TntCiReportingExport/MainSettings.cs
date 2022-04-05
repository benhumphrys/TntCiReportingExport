using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;
using Kofax.ReleaseLib;
using Tnt.KofaxCapture.TntCiReportingExport.Properties;

namespace Tnt.KofaxCapture.TntCiReportingExport
{
    /// <summary>
    /// Holds the main settings for the Export Connector.
    /// </summary>
    internal sealed class MainSettings
    {
        private readonly IReleaseSetupData _setupData;
        private readonly IReleaseData _releaseData;
        private readonly IList<string> _setupDataErrors = new List<string>();

        /// <summary>
        /// Gets the Kofax Batch Name.
        /// </summary>
        public string BatchName => _setupData != null ? string.Empty : _releaseData.BatchName;

        /// <summary>
        /// Gets the Kofax Batch Class Name.
        /// </summary>
        public string BatchClassName => _setupData != null ? _setupData.BatchClassName : _releaseData.BatchClassName;

        /// <summary>
        /// Gets the Kofax Document Class Name.
        /// </summary>
        public string DocumentClassName => _setupData != null ? _setupData.DocClassName : _releaseData.DocClassName;

        /// <summary>
        /// Gets or sets the Name.
        /// </summary>
        public string Name
        {
            get => _setupData != null ? _setupData.Name : _releaseData.Name;
            set
            {
                if (_setupData != null)
                {
                    _setupData.Name = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the OutputDirectoryPath.
        /// </summary>
        public string OutputDirectoryPath
        {
            get => _setupData != null ? _setupData.ImageFilePath : _releaseData.ImageFilePath;
            set
            {
                if (_setupData != null)
                {
                    _setupData.ImageFilePath = value;
                }
            }
        }

        /// <summary>
        /// Gets the list of setup errors.
        /// </summary>
        public ReadOnlyCollection<string> SetupDataErrors => new ReadOnlyCollection<string>(_setupDataErrors);

        /// <summary>
        /// Indicates if the current settings are new.
        /// </summary>
        public bool NewSettings => _setupData != null && (_setupData.New != 0);

        /// <summary>
        /// Initializes a new instance of the Scansation.DocViewerExport.MainSettings class using setup data.
        /// </summary>
        /// <param name="data">Setup data to use as the backing store.</param>
        public MainSettings(IReleaseSetupData data) 
        {
            _setupData = data ?? throw new ArgumentNullException(nameof(data));
        }

        /// <summary>
        /// Initializes a new instance of the Tnt.KofaxCapture.TntCiReportingExport.MainSettings class using setup data.
        /// </summary>
        /// <param name="data">Setup data to use as the backing store.</param>
        public MainSettings(IReleaseData data) 
        {
            _releaseData = data ?? throw new ArgumentNullException(nameof(data));
        }

        /// <summary>
        /// Save settings for setup mode.
        /// </summary>
        public void SaveSetupSettings()
        {
            // Finish by applying the settings.
            _setupData.Apply();
        }
        
        /// <summary>
        /// Determines if the setup data has the expected properties, and if so, returns true.
        /// </summary>
        /// <returns>True if the setup data is valid; false if not.</returns>
        /// <remarks>If this method returns false, the SetupDataErrors collection will contain textual details of 
        /// the error(s).</remarks>
        public bool IsSetupDataValid()
        {
            _setupDataErrors.Clear();

            if (string.IsNullOrWhiteSpace(OutputDirectoryPath))
            {
                _setupDataErrors.Add(Resources.OutputDirectoryCannotBeBlank);
            }

            return _setupDataErrors.Count == 0;
        }

        /// <summary>
        /// Get the report XML from the Custom Storage String.
        /// </summary>
        /// <returns>Value of the Custom Storage String, or null if it does not exist.</returns>
        public string GetReportXml(KfxReleaseScript kfxReleaseScript)
        {
            try
            {
                // ReSharper disable once UseIndexedProperty
                return kfxReleaseScript.DocumentData.get_DocumentCustomStorageString("TntKtmQaQcReport");
            }
            catch (COMException)
            {
                // Report XMl does not exist.
                return null;
            }
        }
    }
}
