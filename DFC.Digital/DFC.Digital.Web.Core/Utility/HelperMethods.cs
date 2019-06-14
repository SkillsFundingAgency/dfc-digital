using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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

        public static string ToConcatenatedString(this IEnumerable<string> source, string delimiter = null)
        {
            return string.Join(delimiter ?? ",", source);
        }

        public static TAttribute GetAttribute<TAttribute>(this Enum enumValue)
            where TAttribute : Attribute
        {
            return enumValue.GetType()
                .GetMember(enumValue.ToString())
                .First()
                .GetCustomAttribute<TAttribute>();
        }
    }
}
