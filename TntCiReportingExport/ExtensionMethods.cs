using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace Tnt.KofaxCapture.TntCiReportingExport
{
    /// <summary>
    /// Defines extension methods.
    /// </summary>
    internal static class ExtensionMethods
    {
        /// <summary>
        /// Returns the specified int as an invariant string.
        /// </summary>
        /// <param name="value">Int to return as an invariant string.</param>
        /// <returns>The int rendered as an invariant string.</returns>
        public static string ToStringInv(this int value)
        {
            return value.ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Cleans the specified string by removing all instances of the chars supplied.
        /// </summary>
        /// <param name="input">String to clean.</param>
        /// <param name="chars">Chars to remove.</param>
        /// <returns>Cleaned string.</returns>
        public static string Clean(this string input, char[] chars)
        {
            if (input == null) return null;
            if (chars == null) throw new ArgumentNullException("chars");

            var output = new StringBuilder();

            foreach (var ch in input.Where(ch => !chars.Contains(ch)))
            {
                output.Append(ch);
            }

            return output.ToString();
        }

        /// <summary>
        /// Cleans the specified string by removing all instances of the chars invalid in a filesystem path.
        /// </summary>
        /// <param name="input">String to clean.</param>
        /// <returns>Cleaned string.</returns>
        public static string CleanInvalidFileNameChars(this string input)
        {
            return input == null ? null : input.Clean(Path.GetInvalidFileNameChars());
        }

        /// <summary>
        /// Performs a case-insensitive replace operation on the input string.
        /// </summary>
        /// <param name="input">String to perform the replace on.</param>
        /// <param name="oldValue">A string to be replaced.</param>
        /// <param name="newValue">A string to replace all occurances of <paramref name="oldValue"/></param>
        /// <returns>Replaced string.</returns>
        public static string ReplaceInsensitive(this string input, string oldValue, string newValue)
        {
            if (input == null) return null;
            if (string.IsNullOrEmpty(oldValue)) throw new ArgumentNullException("oldValue");
            if (newValue == null) newValue = string.Empty;

            var translated = input;
            var startPosition = 0;
            int matchPosition;
            var patternLength = oldValue.Length;

            do
            {
                matchPosition = translated.IndexOf(oldValue, startPosition, StringComparison.OrdinalIgnoreCase);

                if (matchPosition < 0) continue;

                var leadingChars = matchPosition;
                var trailingChars = matchPosition + patternLength;

                translated = translated.Substring(0, leadingChars) +
                             newValue +
                             translated.Substring(trailingChars);

                startPosition = leadingChars + newValue.Length;
            } while (matchPosition >= 0);

            input = translated;
            return input;
        }
    }
}
