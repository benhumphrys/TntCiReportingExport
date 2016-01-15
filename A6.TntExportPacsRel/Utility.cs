using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;
using Tnt.KofaxCapture.A6.TntExportPacsRel.Properties;

namespace Tnt.KofaxCapture.A6.TntExportPacsRel
{
    /// <summary>
    /// Holds utility methods.
    /// </summary>
    internal static class Utility
    {
        private const int AssemblyNameElement = 0;
        private const int AssemblyVersionElement = 1;

        /// <summary>
        /// Get details of the specified assembly.
        /// </summary>
        /// <param name="targetAssembly">Assembly to retrieve details for.</param>
        /// <returns>Assembly details.</returns>
        public static AssemblyDetails GetAssemblyDetails(Assembly targetAssembly)
        {
            if (targetAssembly == null) throw new ArgumentNullException("targetAssembly");

            var splitFullName = targetAssembly.FullName.Split(new[] { ',' });
            var name = splitFullName[AssemblyNameElement].Trim();
            var versionNumber = splitFullName[AssemblyVersionElement].Replace(" Version=", string.Empty);
            var fileVersion = Resources.UnknownAssemblyValue;
            var fileVersionObject =
                RuntimeHelpers.GetObjectValue(
                    targetAssembly.GetCustomAttributes(typeof(AssemblyFileVersionAttribute), true).SingleOrDefault());

            var assemblyDetails = new AssemblyDetails
            {
                Name = name,
                Version = versionNumber,
                FileVersion = fileVersion
            };

            if (fileVersionObject == null) return assemblyDetails;

            var fileVersionAttribute = (AssemblyFileVersionAttribute)fileVersionObject;
            assemblyDetails.FileVersion = fileVersionAttribute.Version;
            return assemblyDetails;
        }

        /// <summary>
        /// Delete the specified file.  Ignore IO errors.
        /// </summary>
        /// <param name="filePath">File path to delete.</param>
        public static void DeleteNonCriticalFile(string filePath)
        {
            if (string.IsNullOrEmpty(filePath)) throw new ArgumentNullException("filePath");

            try
            {
                File.Delete(filePath);
            }
            catch (IOException)
            {
                // Ignore errors that occur during deletion as they are non-critical.
            }
        }

        /// <summary>
        /// Get the file path of the specified associated file.
        /// </summary>
        /// <param name="associatedFileName">Associated file name.</param>
        /// <returns>Path to the associated file.</returns>
        /// <remarks>An associated file is one located in the same directory as the executing Assembly.</remarks>
        public static string GetAssociatedFilePath(string associatedFileName)
        {
            if (string.IsNullOrEmpty(associatedFileName)) throw new ArgumentNullException("associatedFileName");

            var assemblyDirectoryPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            if (assemblyDirectoryPath != null)
            {
                return Path.Combine(assemblyDirectoryPath, associatedFileName);
            }

            throw new SettingsException(Resources.CouldNotDetermineDirectoryPathOfAssembly);
        }

        /// <summary>
        /// Retrieve the current method name.
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static string GetCurrentMethod()
        {
            var st = new StackTrace();
            var sf = st.GetFrame(1);

            return sf.GetMethod().Name;
        }

        /// <summary>
        /// Use the system font to allow it to look good on different versions of Windows.
        /// </summary>
        /// <param name="targetControl"></param>
        public static void SetSystemFont(Control targetControl)
        {
            if (targetControl == null) throw new ArgumentNullException("targetControl");

            targetControl.SuspendLayout();
            targetControl.Font = SystemFonts.MessageBoxFont;
            targetControl.ResumeLayout(true);
            targetControl.PerformLayout();
        }

        /// <summary>
        /// Retrieve the path to the Kofax Capture Logs directory.
        /// </summary>
        /// <returns></returns>
        public static string GetLogsDirectory()
        {
            var ascentKey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Kofax Image Products\Ascent Capture\3.0\");

            if (ascentKey == null)
            {
                throw new ExportException(Resources.MissingKofaxRegistryKey);
            }

            var kofaxServerPath = (string)(ascentKey.GetValue("ServerPath"));
            if (kofaxServerPath == null)
            {
                throw new ApplicationException(Resources.MissingServerPathValue);
            }

            var logDirectoryPath = Path.Combine(kofaxServerPath, "Logs");
            if (!Directory.Exists(logDirectoryPath))
            {
                throw new ArgumentException(string.Format(Resources.LogDirectoryDoesNotExist, logDirectoryPath));
            }

            return logDirectoryPath;
        }

        /// <summary>
        /// Retrieve the path to the log file.
        /// </summary>
        /// <returns>Path to log file.</returns>
        /// <remarks>Returns null if the logging is not enabled in the INF file.</remarks>
        public static string GetLogFilePath()
        {
            var filePath = GetAssociatedFilePath("A6.TntExportPacsRel.inf");
            var settingsValue = new StringBuilder(10);
            var result = NativeMethods.GetPrivateProfileString("A5a6.TntExport", "OpenCloseLoggingEnabled",
                bool.FalseString, settingsValue, (uint) settingsValue.Capacity, filePath);

            if (result > 0 &&
                settingsValue.ToString().Equals(bool.TrueString, StringComparison.InvariantCultureIgnoreCase))
            {
                var directoryPath = GetLogsDirectory();
                var processId = Process.GetCurrentProcess().Id;
                return Path.Combine(directoryPath,
                    string.Format("Release_[{0}]_{1}.txt", processId, DateTime.Now.ToString("yyMMdd")));
            }

            return null;
        }
    }
}
