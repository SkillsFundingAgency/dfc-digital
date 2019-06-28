using System;
using System.Configuration;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace DFC.Digital.Web.Core
{
    public static class StringManipulationExtension
    {
        public static string ReplaceTokens(this HtmlHelper helper, string valueWithTokens)
        {
            return Regex.Replace(valueWithTokens, @"{([^\.]+)\.([^}]+)}", RegexTokenReplacer);
        }

        public static string ReplaceSpecialCharacters(string input, string regexPattern)
        {
            var regex = new Regex(regexPattern, RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Compiled);
            if (!string.IsNullOrEmpty(input))
            {
                return regex.Replace(input, string.Empty);
            }

            return input;
        }

        public static string GetLinkEncodedString(string input)
        {
            return !string.IsNullOrWhiteSpace(input) ? HttpUtility.UrlEncode(input) : string.Empty;
        }

        private static string RegexTokenReplacer(Match match)
        {
            var returnValue = match.ToString();
            switch (match)
            {
                case var resolver when match.Groups.Count > 1 && match.Groups[1].Value.Equals("config", StringComparison.OrdinalIgnoreCase):
                    returnValue = ConfigurationManager.AppSettings[resolver.Groups[2].Value] ?? string.Empty;
                    break;

                default:
                    break;
            }

            return returnValue;
        }
    }
}