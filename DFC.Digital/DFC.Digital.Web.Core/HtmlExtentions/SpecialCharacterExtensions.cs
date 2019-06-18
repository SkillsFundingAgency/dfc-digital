using System;
using System.Configuration;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace DFC.Digital.Web.Core
{
    public static class SpecialCharacterExtensions
    {
        public static string ReplaceSpecialCharacters(string valueWithSpCharacters)
        {
            Regex regex = new Regex("(?:[^a-z0-9 ]|(?<=['\"])s)", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Compiled);
            if (!string.IsNullOrEmpty(valueWithSpCharacters))
            {
                return regex.Replace(valueWithSpCharacters, string.Empty);
            }

            return valueWithSpCharacters;
        }

        public static string GetUrlEncodedString(string input)
        {
            return !string.IsNullOrWhiteSpace(input) ? HttpUtility.UrlEncode(input) : string.Empty;
        }
    }
}
