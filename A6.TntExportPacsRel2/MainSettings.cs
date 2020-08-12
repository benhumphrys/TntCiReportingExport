using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using Kofax.ReleaseLib;
using Tnt.KofaxCapture.A6.TntExportPacsRel2.Properties;

namespace Tnt.KofaxCapture.A6.TntExportPacsRel2
{
    /// <summary>
    /// Holds the main settings for the Export Connector.
    /// </summary>
    internal sealed class MainSettings
    {
        private const KfxLinkSourceType BatchFieldType = KfxLinkSourceType.KFX_REL_BATCHFIELD;
        private const KfxLinkSourceType IndexFieldType = KfxLinkSourceType.KFX_REL_INDEXFIELD;
        
        private readonly IReleaseSetupData _setupData;
        private readonly IReleaseData _releaseData;
        private readonly Dictionary<LinkKey, string> _releaseValues = new Dictionary<LinkKey, string>();
        private readonly List<SetupField> _setupFields = new List<SetupField>();
        private readonly IList<string> _setupDataErrors = new List<string>();
        private readonly CultureInfo _culture = CultureInfo.CurrentCulture;
        private readonly List<ImageType> _imageTypes = new List<ImageType>();

        /// <summary>
        /// Gets the Kofax Batch Name.
        /// </summary>
        public string BatchName
        {
            get { return _setupData != null ? string.Empty : _releaseData.BatchName; }
        }

        /// <summary>
        /// Gets the Kofax Batch Class Name.
        /// </summary>
        public string BatchClassName
        {
            get { return _setupData != null ? _setupData.BatchClassName : _releaseData.BatchClassName; }
        }

        /// <summary>
        /// Gets the Kofax Document Class Name.
        /// </summary>
        public string DocumentClassName
        {
            get { return _setupData != null ? _setupData.DocClassName : _releaseData.DocClassName; }
        }

        /// <summary>
        /// Gets or sets the Name.
        /// </summary>
        public string Name
        {
            get { return _setupData != null ? _setupData.Name : _releaseData.Name; }
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
            get { return _setupData != null ? _setupData.ImageFilePath : _releaseData.ImageFilePath; }
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
        public ReadOnlyCollection<string> SetupDataErrors
        {
            get { return new ReadOnlyCollection<string>(_setupDataErrors); }
        }

        /// <summary>
        /// Gets the list of image types.
        /// </summary>
        public ReadOnlyCollection<ImageType> ImageTypes
        {
            get { return new ReadOnlyCollection<ImageType>(_imageTypes); }
        }

        /// <summary>
        /// Gets the list of setup fields.
        /// </summary>
        public ReadOnlyCollection<SetupField> SetupFields
        {
            get { return new ReadOnlyCollection<SetupField>(_setupFields); }
        }

        /// <summary>
        /// Indicates if the current settings are new.
        /// </summary>
        public bool NewSettings { get { return _setupData != null && (_setupData.New != 0); } }
        
        /// <summary>
        /// Initializes a new instance of the Scansation.DocViewerExport.MainSettings class using setup data.
        /// </summary>
        /// <param name="data">Setup data to use as the backing store.</param>
        public MainSettings(IReleaseSetupData data) 
        {
            if (data == null) throw new ArgumentNullException("data");

            _setupData = data;
            LoadSetupSettings();
        }

        /// <summary>
        /// Initializes a new instance of the Tnt.KofaxCapture.A5a6.TntExport.MainSettings class using setup data.
        /// </summary>
        /// <param name="data">Setup data to use as the backing store.</param>
        public MainSettings(IReleaseData data) 
        {
            if (data == null) throw new ArgumentNullException("data");

            _releaseData = data;
            LoadReleaseSettings();
        }

        /// <summary>
        /// Save settings for setup mode.
        /// </summary>
        public void SaveSetupSettings()
        {
            // Save the links.
            AddLinks();

            // We must support PDF output, as it's the only one allowed.
            _setupData.KofaxPDFReleaseScriptEnabled = true;

            // Finish by applying the settings.
            _setupData.Apply();
        }

        /// <summary>
        /// Load settings for setup mode.
        /// </summary>
        private void LoadSetupSettings()
        {
            PopulateSetupFields();
        }

        /// <summary>
        /// Populate the SetupFields collection.
        /// </summary>
        private void PopulateSetupFields()
        {
            // Read the batch, index, and Kofax fields and add them to the list for use in the UI.
            // Add batch fields first.
            for (var i = 1; i <= _setupData.BatchFields.Count; i++)
            {
                // ReSharper disable UseIndexedProperty
                var field = _setupData.BatchFields.get_Item(i);
                // ReSharper restore UseIndexedProperty

                _setupFields.Add(new SetupField
                {
                    FriendlyFieldType = Resources.BatchFieldTypeFriendlyName,
                    KofaxFieldType = KfxLinkSourceType.KFX_REL_BATCHFIELD,
                    FieldName = field.Name
                });
            }

            // Add the index fields.
            for (var i = 1; i <= _setupData.IndexFields.Count; i++)
            {
                // ReSharper disable UseIndexedProperty
                var field = _setupData.IndexFields.get_Item(i);
                // ReSharper restore UseIndexedProperty

                _setupFields.Add(new SetupField
                {
                    FriendlyFieldType = Resources.IndexFieldTypeFriendlyName,
                    KofaxFieldType = KfxLinkSourceType.KFX_REL_INDEXFIELD,
                    FieldName = field.Name
                });
            }

            // Add all Kofax Values.
            var enumerator = ((IEnumerable) _setupData.BatchVariableNames).GetEnumerator();
            enumerator.Reset();

            while (enumerator.MoveNext())
            {
                _setupFields.Add(new SetupField
                {
                    FriendlyFieldType = Resources.KofaxFieldTypeFriendlyName,
                    KofaxFieldType = KfxLinkSourceType.KFX_REL_VARIABLE,
                    FieldName = (string) enumerator.Current
                });
            }

            GC.KeepAlive(enumerator);
        }

        /// <summary>
        /// Add the setup links to the SetupData object of the fields selected by the user.
        /// </summary>
        private void AddLinks()
        {
            _setupData.Links.RemoveAll();
            var batchFieldCount = 0;
            var indexFieldCount = 0;
            var kofaxFieldCount = 0;

            foreach (var setupField in _setupFields)
            {
                string destination;

                switch (setupField.KofaxFieldType)
                {
                    case KfxLinkSourceType.KFX_REL_INDEXFIELD:
                        indexFieldCount++;
                        destination = "I" + indexFieldCount.ToString(CultureInfo.InvariantCulture);
                        break;
                    case KfxLinkSourceType.KFX_REL_VARIABLE:
                        kofaxFieldCount++;
                        destination = "K" + kofaxFieldCount.ToString(CultureInfo.InvariantCulture);
                        break;
                    case KfxLinkSourceType.KFX_REL_BATCHFIELD:
                        batchFieldCount++;
                        destination = "B" + batchFieldCount.ToString(CultureInfo.InvariantCulture);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                _setupData.Links.Add(setupField.FieldName, setupField.KofaxFieldType, destination);
            }
        }

        /// <summary>
        /// Load settings for release mode.
        /// </summary>
        private void LoadReleaseSettings()
        {
            // Read the field mappings.
            foreach (IValue value in _releaseData.Values)
            {
                var linkKey = new LinkKey (value.SourceName, value.SourceType);
                _releaseValues.Add(linkKey, value.Value);
            }
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
            ValidateKofaxSetupData();

            if (string.IsNullOrWhiteSpace(OutputDirectoryPath))
            {
                _setupDataErrors.Add(Resources.OutputDirectoryCannotBeBlank);
            }

            return _setupDataErrors.Count == 0;
        }

        /// <summary>
        /// Validate the setup data stored using the Kofax API.
        /// </summary>
        private void ValidateKofaxSetupData()
        {
            //TODO: Add required fields here?
            var fields = new List<SetupField>();

            foreach (var field in fields.Where(f=> !_setupFields.Contains(f)))
            {
                _setupDataErrors.Add(string.Format(_culture, Resources.MissingRequiredField, field.FriendlyFieldType,
                    field.FieldName));
            }
        }

        /// <summary>
        /// Retrieve the value of the specified field via its mnemonic.
        /// </summary>
        /// <param name="mnemonic">Mnemonic to use to derive a field name and type.</param>
        /// <param name="required">Indicates if the field value must exist.</param>
        /// <returns>Value of the field, or mnemonic if the field was not found tbe usable to derive 
        /// a key from.</returns>
        public string GetMnemonicValue(string mnemonic, bool required = false)
        {
            if (string.IsNullOrEmpty(mnemonic)) return string.Empty;

            var key = LinkKey.FromMnemonic(mnemonic);
            return key != null ? GetFieldValue(key, required) : mnemonic;
        }

        /// <summary>
        /// Retrieve the value of the specified field via its name and type.
        /// </summary>
        /// <typeparam name="T">Type to return.</typeparam>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="fieldType">Type of the field.</param>
        /// <param name="required">Indicates if the field value must exist.</param>
        /// <returns>Value of the field.</returns>
        public T GetFieldValue<T>(string fieldName, KfxLinkSourceType fieldType, bool required = false) where T: struct 
        {
            var value = GetFieldValue(fieldName, fieldType, required);
            if (string.IsNullOrEmpty(value))
            {
                if (required)
                {
                    throw new SettingsException(string.Format(_culture, Resources.CannotConvertValue, value, typeof (T)));
                }
                
                return default(T);
            }

            try
            {
                if (typeof(T) == typeof(int))
                {
                    return (T) (object) int.Parse(value, NumberStyles.Integer);
                }

                if (typeof(T) == typeof(long))
                {
                    return (T)(object)long.Parse(value, NumberStyles.Integer);
                }

                if (typeof(T) == typeof(DateTime))
                {
                    return (T)(object)DateTime.Parse(value, _culture);
                }

                if (typeof(T) == typeof(TimeSpan))
                {
                    return (T)(object)TimeSpan.Parse(value, _culture);
                }
            }
            catch (FormatException ex)
            {
                throw new SettingsException(string.Format(_culture, Resources.CannotConvertValue, value, typeof (T)), ex);
            }

            throw new SettingsException(string.Format(_culture, Resources.UnsupportedGenericType, typeof(T)));    
        }

        /// <summary>
        /// Retrieve the value of the specified field via its name and type.
        /// </summary>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="fieldType">Type of the field.</param>
        /// <param name="required">Indicates if the field value must exist.</param>
        /// <returns>Value of the field.</returns>
        public string GetFieldValue(string fieldName, KfxLinkSourceType fieldType, bool required = false)
        {
            if (string.IsNullOrEmpty(fieldName)) throw new ArgumentNullException("fieldName");

            var linkKey = new LinkKey(fieldName, fieldType);
            return GetFieldValue(linkKey, required);
        }

        /// <summary>
        /// Retrieve the value of the specified field via its LinkKey.
        /// </summary>
        /// <param name="linkKey">LinkKey to use to find the field.</param>
        /// <param name="required">Indicates if the field must exist.</param>
        /// <returns>Value of the field.</returns>
        public string GetFieldValue(LinkKey linkKey, bool required = false)
        {
            if (linkKey == null) throw new ArgumentNullException("linkKey");

            var fieldValue = string.Empty;

            if (_releaseValues.ContainsKey(linkKey))
            {
                fieldValue = _releaseValues[linkKey];
            }

            if (required && string.IsNullOrEmpty(fieldValue))
            {
                throw new SettingsException(string.Format(Resources.MissingRequiredField,
                    GetFieldTypeFriendlyName(linkKey.Type), linkKey.Name));
            }

            return fieldValue;
        }

        /// <summary>
        /// Return the friendly name of the specifie field type.
        /// </summary>
        /// <param name="fieldType">Field type to retunr friendly name of.</param>
        /// <returns>Friendly name of the specified field type.</returns>
        private static string GetFieldTypeFriendlyName(KfxLinkSourceType fieldType)
        {
            switch (fieldType)
            {
                case KfxLinkSourceType.KFX_REL_INDEXFIELD:
                    return Resources.IndexFieldTypeFriendlyName;
                case KfxLinkSourceType.KFX_REL_VARIABLE:
                    return Resources.BatchFieldTypeFriendlyName;
                case KfxLinkSourceType.KFX_REL_BATCHFIELD:
                    return Resources.KofaxFieldTypeFriendlyName;
                default:
                    throw new ArgumentOutOfRangeException("fieldType");
            }
        }
    }
}
