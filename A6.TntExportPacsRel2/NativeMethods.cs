using System.Runtime.InteropServices;
using System.Text;

namespace Tnt.KofaxCapture.A6.TntExportPacsRel2
{
    /// <summary>
    /// Defines Win32 API functions and data structures.
    /// </summary>
    internal static class NativeMethods
    {
        /// <summary>
        /// Retrieves a string from the specified section in an initialization file.
        /// </summary>
        /// <param name="lpAppName">The name of the section containing the key name. If this parameter is NULL, 
        /// the GetPrivateProfileString function copies all section names in the file to the supplied buffer.</param>
        /// <param name="lpKeyName">The name of the key whose associated string is to be retrieved. If this 
        /// parameter is NULL, all key names in the section specified by the lpAppName parameter are copied to the 
        /// buffer specified by the lpReturnedString parameter.</param>
        /// <param name="lpDefault">A default string. If the lpKeyName key cannot be found in the initialization 
        /// file, GetPrivateProfileString copies the default string to the lpReturnedString buffer. If this 
        /// parameter is NULL, the default is an empty string, "". Avoid specifying a default string with 
        /// trailing blank characters. The function inserts a null character in the lpReturnedString buffer 
        /// to strip any trailing blanks.</param>
        /// <param name="lpReturnedString">StringBuilder that receives the retrieved string.</param>
        /// <param name="nSize">The size of the buffer pointed to by the lpReturnedString parameter, in 
        /// characters.</param>
        /// <param name="lpFileName">The name of the initialization file. If this parameter does not contain 
        /// a full path to the file, the system searches for the file in the Windows directory.</param>
        /// <returns>The return value is the number of characters copied to the buffer, not including the 
        /// terminating null character.</returns>
        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        public static extern uint GetPrivateProfileString(string lpAppName,
                                                          string lpKeyName,
                                                          string lpDefault,
                                                          StringBuilder lpReturnedString,
                                                          uint nSize,
                                                          string lpFileName);
    }
}
