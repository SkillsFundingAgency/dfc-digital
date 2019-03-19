using System.Text.RegularExpressions;

namespace DFC.Digital.Web.Core
{
    public static class HelperMethods
    {
        public static bool IsValidRegexValue(string value, string regexPattern)
        {
            var commonRegexOptions = RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase | RegexOptions.Singleline;
            return string.IsNullOrWhiteSpace(value) == false && Regex.IsMatch(value, regexPattern, commonRegexOptions);
        }

        public static bool IsValidDoubleRegexValue(string value, string regexPatternOne, string regexPatternTwo, bool isAndOperator)
        {
            if (isAndOperator)
            {
                return string.IsNullOrWhiteSpace(value) == false && (HelperMethods.IsValidRegexValue(value, regexPatternOne) && HelperMethods.IsValidRegexValue(value, regexPatternTwo));
            }
            else
            {
                return string.IsNullOrWhiteSpace(value) == false && (HelperMethods.IsValidRegexValue(value, regexPatternOne) || HelperMethods.IsValidRegexValue(value, regexPatternTwo));
            }
        }
    }
}
