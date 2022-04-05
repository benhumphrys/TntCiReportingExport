using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using Kofax.ReleaseLib;
using Tnt.KofaxCapture.TntCiReportingExport.Properties;

namespace Tnt.KofaxCapture.TntCiReportingExport
{
    [ComVisible(true),
    ClassInterface(ClassInterfaceType.None),
    Guid("72934E4D-8A00-4223-A33A-01DE177DF55F"),
    ProgId("TntCiReportingExport.kfxreleasesetup")]
    public class KfxReleaseSetupScript : IKfxReleaseSetupScript, IDisposable
    {
        private bool _disposed;

        /// <summary>
        /// Returns the last error text (useful for unit testing).
        /// </summary>
        public StringBuilder LastErrorText { get; private set; }

        /// <summary>
        /// Gets or set the setup data.
        /// </summary>
        public ReleaseSetupData SetupData { get; set; }

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
            Dispose();

            return KfxReturnValue.KFX_REL_SUCCESS;
        }

        /// <summary>
        /// This method allows the setup script
        /// to respond to various events in the
        /// Administration module.  The script
        /// has the opportunity to make any
        /// necessary changes to the release
        /// settings in the ReleaseSetupData
        /// object or any other external data
        /// source.
        /// </summary>
        /// <param name="actionId"> ID of the event </param>
        /// <param name="data1"> Action parameter 1 </param>
        /// <param name="data2"> Action parameter 2 </param>
        /// <returns>
        /// One of the following:
        ///    KFX_REL_SUCCESS, KFX_REL_ERROR,
        ///    or KFX_REL_UNSUPPORTED
        /// </returns>
        /// <remarks>
        /// Refer to the documentation for a list
        /// of actions and associated parameters.
        /// </remarks>
        public KfxReturnValue ActionEvent(ref KfxActionValue actionId, ref string data1, ref string data2)
        {
            switch (actionId)
            {
                case KfxActionValue.KFX_REL_PUBLISH_CHECK:
                    ConfigureSettings(false);

                    if (LastErrorText != null && LastErrorText.Length > 0)
                    {
                        return KfxReturnValue.KFX_REL_ERROR;
                    }
                    break;

                case KfxActionValue.KFX_REL_INDEXFIELD_INSERT:
                case KfxActionValue.KFX_REL_BATCHFIELD_INSERT:
                case KfxActionValue.KFX_REL_INDEXFIELD_RENAME:
                case KfxActionValue.KFX_REL_BATCHFIELD_RENAME:
                case KfxActionValue.KFX_REL_INDEXFIELD_DELETE:
                case KfxActionValue.KFX_REL_BATCHFIELD_DELETE:
                    ConfigureSettings(false);
                    break;

                // ReSharper disable RedundantCaseLabel
                case KfxActionValue.KFX_REL_UPGRADE:
                case KfxActionValue.KFX_REL_UNDEFINED_ACTION:
                case KfxActionValue.KFX_REL_DOCCLASS_RENAME:
                case KfxActionValue.KFX_REL_BATCHCLASS_RENAME:
                case KfxActionValue.KFX_REL_RELEASESETUP_DELETE:
                case KfxActionValue.KFX_REL_IMPORT:
                case KfxActionValue.KFX_REL_START:
                case KfxActionValue.KFX_REL_END:
                case KfxActionValue.KFX_REL_FOLDERCLASS_INSERT:
                case KfxActionValue.KFX_REL_FOLDERCLASS_RENAME:
                case KfxActionValue.KFX_REL_FOLDERCLASS_DELETE:
                case KfxActionValue.KFX_REL_TABLE_DELETE:
                case KfxActionValue.KFX_REL_TABLE_INSERT:
                case KfxActionValue.KFX_REL_TABLE_RENAME:
                // ReSharper disable RedundantEmptyDefaultSwitchBranch
                default:
                    // No functionality thus far.
                    break;
                // ReSharper restore RedundantEmptyDefaultSwitchBranch
                // ReSharper restore RedundantCaseLabel
            }

            return KfxReturnValue.KFX_REL_SUCCESS;
        }

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
            return KfxReturnValue.KFX_REL_SUCCESS;
        }

        /// <summary>
        /// User interface display point.  This
        /// method is called by the Release Setup
        /// Controller to display the setup form
        /// specific to this script.
        /// </summary>
        /// <returns> Always KFX_REL_SUCCESS </returns>
        /// <remarks>
        /// Called by Release Setup Controller
        /// when the Administration module asks
        /// to run the script and whenever a
        /// Batch Field or Index Field is inserted.
        /// </remarks>
        public KfxReturnValue RunUI()
        {
            ConfigureSettings(true);

            return KfxReturnValue.KFX_REL_SUCCESS;
        }

        /// <summary>
        /// Configure the settings and save them if they are okay.
        /// </summary>
        /// <param name="showDialogs">Determines whether to show dialog boxes.</param>
        internal void ConfigureSettings(bool showDialogs)
        {
            try
            {
                // Read the settings.
                LastErrorText = new StringBuilder();
                var settings = new MainSettings(SetupData);
                var saveSettings = true;

                if (showDialogs)
                {
                    using (var mainForm = new MainForm(settings))
                    {
                        var result = mainForm.ShowDialog();
                        saveSettings = (result == DialogResult.OK);
                    }
                }

                if (!saveSettings) return;

                if (settings.IsSetupDataValid())
                {
                    settings.SaveSetupSettings();
                }
                else
                {
                    LogSetupErrors(settings);
                }
            }
            catch (SettingsException ex)
            {
                SetupData.LogError(6000, 0, 0, ex.ToString(), GetType().Name + "." + Utility.GetCurrentMethod(), 0);
            }
            catch (Exception ex)
            {
                SetupData.LogError(6000, 0, 0, ex.ToString(), GetType().Name + "." + Utility.GetCurrentMethod(), 0);
            }
        }

        /// <summary>
        /// Log the setup errors.
        /// </summary>
        /// <param name="settings">Current settings.</param>
        private void LogSetupErrors(MainSettings settings)
        {
            if (settings == null) throw new ArgumentNullException(nameof(settings));
            LastErrorText.AppendLine();

            foreach (var error in settings.SetupDataErrors)
            {
                LastErrorText.AppendLine(error);
            }

            SetupData.LogError(6000, 0, 0, LastErrorText.ToString(),
                GetType().Name + "." + Utility.GetCurrentMethod(), 0);
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

            // Call the appropriate methods to clean up
            // unmanaged resources here.
            // If disposing is false,
            // only the following code is executed.

            // Record that disposing has been done.
            _disposed = true;
        }

        /// <summary>
        /// This destructor will run only if the Dispose method
        /// does not get called.
        /// </summary>
        ~KfxReleaseSetupScript()
        {
            // Do not re-create Dispose clean-up code; just call Dispose().
            Dispose(false);
        }
    }
}
