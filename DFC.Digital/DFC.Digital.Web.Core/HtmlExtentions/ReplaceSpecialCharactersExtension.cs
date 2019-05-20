using System;
using System.Configuration;
using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace DFC.Digital.Web.Core
{
    public static class ReplaceSpecialCharactersExtension
    {
        public static string ReplaceSpecialCharacter(string valueWithTokens)
        {
            Regex regex = new Regex("(?:[^a-z0-9 ]|(?<=['\"])s)", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Compiled);
            return regex.Replace(valueWithTokens, string.Empty);
        }
    }
}
