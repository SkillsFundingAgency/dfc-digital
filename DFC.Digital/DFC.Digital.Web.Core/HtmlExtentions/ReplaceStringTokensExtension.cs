using System;
using System.Configuration;
using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace DFC.Digital.Web.Core
{
    public static class ReplaceStringTokensExtension
    {
        public static string ReplaceTokens(this HtmlHelper helper, string valueWithTokens)
        {
            return Regex.Replace(valueWithTokens, @"{([^\.]+)\.([^}]+)}", RegexTokenReplacer);
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