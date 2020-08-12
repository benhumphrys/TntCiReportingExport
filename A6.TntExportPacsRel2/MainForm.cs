using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Windows.Forms;
using Tnt.KofaxCapture.A6.TntExportPacsRel.Properties;

namespace Tnt.KofaxCapture.A6.TntExportPacsRel
{
    /// <summary>
    /// Main Form for the setup UI.
    /// </summary>
    internal sealed partial class MainForm : Form
    {
        private bool _dirty;
        private readonly MainSettings _settings;
        private static bool _closedChildDialog;
        private readonly CultureInfo _culture = CultureInfo.CurrentCulture;
        private readonly AssemblyDetails _assemblyDetails;

        /// <summary>
        /// Indicates that the settings have changed.
        /// </summary>
        private bool Dirty
        {
            get { return _dirty; }
            set
            {
                _dirty = value;
                ApplyButton.Enabled = _dirty;
            }
        }

        /// <summary>
        /// Indicates whether the settings are okay.
        /// </summary>
        public bool SettingsOk { get; set; }

        /// <summary>
        /// Initializes a new instance of the Tnt.KofaxCapture.A5a6.TntExport.MainFormClass.
        /// </summary>
        /// <param name="settings">Main settings.</param>
        public MainForm(MainSettings settings) : this()
        {
            if (settings == null) throw new ArgumentNullException("settings");
            _settings = settings;
            _assemblyDetails = Utility.GetAssemblyDetails(Assembly.GetExecutingAssembly());
        }

        /// <summary>
        /// Initializes a new instance of the Tnt.KofaxCapture.A5a6.TntExport.MainFormClass.
        /// </summary>
        private MainForm()
        {
            InitializeComponent();
            Utility.SetSystemFont(this);
        }

        /// <summary>
        /// Invoked when the form loads.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_Load(object sender, EventArgs e)
        {
            LoadResourceStrings();
            LoadSettings();
            WireUpEvents();
        }

        /// <summary>
        /// Localise the UI.
        /// </summary>
        private void LoadResourceStrings()
        {
            Text = Resources.AppName;
            BatchClassLabel.Text = Resources.BatchClassLabelText;
            DocClassLabel.Text = Resources.DocClassLabelText;
            NameLabel.Text = Resources.NameLabelText;

            OutputDirectoryLabel.Text = Resources.OutputDirectoryLabelText;
            BrowseButton.Text = Resources.BrowseButtonText;

            VersionLabel.Text = string.Format(Resources.VersionLabelText, _assemblyDetails.FileVersion);
            OkButton.Text = Resources.OkButtonText;
            CloseButton.Text = Resources.CloseButton;
            ApplyButton.Text = Resources.ApplyButtonText;
        }

        /// <summary>
        /// Read the settings from the setup data.
        /// </summary>
        private void LoadSettings()
        {
            if (_settings.NewSettings)
            {
                Dirty = true;
            }

            BatchClassValueLabel.Text = _settings.BatchClassName;
            DocClassValueLabel.Text = _settings.DocumentClassName;
            NameTextBox.Text = _settings.Name;
            OutputDirectoryTextBox.Text = _settings.OutputDirectoryPath;
        }

        /// <summary>
        /// Wire-up the events for the form.
        /// </summary>
        private void WireUpEvents()
        {
            NameTextBox.Validated += (o, args) =>
            {
                Dirty = true;
                _settings.Name = NameTextBox.Text;
            };

            OutputDirectoryTextBox.TextChanged += (o, args) =>
            {
                Dirty = true;
                _settings.OutputDirectoryPath = OutputDirectoryTextBox.Text;
            };

            OutputDirectoryTextBox.Validated += (o, args) =>
            {
                if (_settings.OutputDirectoryPath == OutputDirectoryTextBox.Text) return;
                Dirty = true;
                _settings.OutputDirectoryPath = OutputDirectoryTextBox.Text;
            };

            BrowseButton.Click += (o, args) =>
            {
                Dirty = true;
                var dialogResult = OutputDirectoryFolderBrowserDialog.ShowDialog(this);

                if (dialogResult != DialogResult.OK) return;
                OutputDirectoryTextBox.Text = OutputDirectoryFolderBrowserDialog.SelectedPath;
                _settings.OutputDirectoryPath = OutputDirectoryFolderBrowserDialog.SelectedPath;
            };

            Closing += MainSetupForm_Closing;
            ApplyButton.Click += ApplyButton_Click;
        }

        /// <summary>
        /// Occurs when the form begins the process of closing.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void MainSetupForm_Closing(object sender, CancelEventArgs e)
        {
            if (_closedChildDialog)
            {
                // Don't close the form if it was triggered by a child form closing.
                _closedChildDialog = false;
                e.Cancel = true;
                return;
            }

            var trySave = DetermineTrySaveSettings(e);
            if (!Dirty || !trySave) return;

            if (_settings.IsSetupDataValid())
            {
                DialogResult = DialogResult.OK;
                return;
            }

            DisplaySettingsErrors();
            e.Cancel = true;
        }

        /// <summary>
        /// Display a dialog box with the specified error lines.
        /// </summary>
        private void DisplaySettingsErrors()
        {
            // Generate the list of errors.
            var allErrors = new List<string>(_settings.SetupDataErrors);
            
            MessageBox.Show(this,
                string.Format(_culture, Resources.InvalidSettings, string.Join("\n", allErrors.ToArray())),
                Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>
        /// Determine whether to try saving the current settings.
        /// </summary>
        /// <param name="e">Event args.</param>
        /// <returns>True if we should try to save the current settings.</returns>
        private bool DetermineTrySaveSettings(CancelEventArgs e)
        {
            var trySave = false;

            switch (DialogResult)
            {
                case DialogResult.OK:
                    trySave = true;
                    break;

                case DialogResult.Cancel:
                    if (Dirty)
                    {
                        var confirmResult = ConfirmSaveSettings();

                        switch (confirmResult)
                        {
                            case DialogResult.Yes:
                                trySave = true;
                                break;
                            case DialogResult.Cancel:
                                e.Cancel = true;
                                break;
                        }
                    }

                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
            return trySave;
        }

        /// <summary>
        /// Confirm if the user wants to save settings after clicking Cancel.
        /// </summary>
        /// <returns></returns>
        private static DialogResult ConfirmSaveSettings()
        {
            return MessageBox.Show(Resources.SettingsChangedSave,
                                   Resources.AppName,
                                   MessageBoxButtons.YesNoCancel,
                                   MessageBoxIcon.Question,
                                   MessageBoxDefaultButton.Button1, 0);
        }

        /// <summary>
        /// Check the settings and if they are okay, save them.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ApplyButton_Click(object sender, EventArgs e)
        {
            if (_settings.IsSetupDataValid())
            {
                _settings.SaveSetupSettings();
                Dirty = false;
                return;
            }

            DisplaySettingsErrors();
        }
    }
}
