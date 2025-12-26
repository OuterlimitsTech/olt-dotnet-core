using System.Text.RegularExpressions;

namespace OLT.Constants
{
    public static class OltRegExPatterns
    {
        // Default match timeout to avoid catastrophic backtracking / long-running matches
        private static readonly TimeSpan DefaultMatchTimeout = TimeSpan.FromMilliseconds(750);

        public static readonly Regex Spaces = new Regex(@"\s+", RegexOptions.None, DefaultMatchTimeout); // Looks for Spaces
        public static readonly Regex DigitsOnly = new Regex(@"[^\d]", RegexOptions.None, DefaultMatchTimeout);
        public static readonly Regex DecimalDigitsOnly = new Regex(@"[^\d\.]", RegexOptions.None, DefaultMatchTimeout);
        public static readonly Regex RemoveSpecialCharacters = new Regex("(?:[^a-z0-9 ]|(?<=['\"])s)", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Compiled, DefaultMatchTimeout);
        public static readonly Regex ZipRegex = new Regex("\\d{5}(-?\\d{4})?", RegexOptions.None, DefaultMatchTimeout);
    }
}
