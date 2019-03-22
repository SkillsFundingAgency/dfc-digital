using System.Text.RegularExpressions;

namespace DFC.Digital.Web.Core
{
    public static class RegexPatterns
    {
        public const string Postcode = @"^[A-Za-z0-9-.\(\)\/\\\s]*$";
        public const string UKPostCode = @"^([bB][fF][pP][oO]\s{0,1}[0-9]{1,4}|[gG][iI][rR]\s{0,1}0[aA][aA]|[a-pr-uwyzA-PR-UWYZ]([0-9]{1,2}|([a-hk-yA-HK-Y][0-9]|[a-hk-yA-HK-Y][0-9]([0-9]|[abehmnprv-yABEHMNPRV-Y]))|[0-9][a-hjkps-uwA-HJKPS-UW])\s{0,1}[0-9][abd-hjlnp-uw-zABD-HJLNP-UW-Z]{2})$";
        public const string EnglishOrBFPOPostCode = @"^(?!ab|bt|cf|ch5|ch6|ch7|ch8|dd|dg|eh|fk|g[0-9]|gy|hs|im|iv|je|ka|kw|ky|ld|ll|ml|np|pa|ph|sa|sy|td|ze)+.*$";
        public const string BfpoPostCode = @"^([bB][fF][pP][oO]\s{0,1}[0-9]{1,4})$";

        public const string ContactPhone = @"^[+0]{1}[0-9( )-]{10,19}$";
        public const string ContactMobilePhone = @"^((\+44)|(0044)|(0))7([1-5]|[7-9])[0-9]{8}$";

        public const string Name = @"^[A-Za-z'-. `]*$";
        public const string Password = @"^(?i:(?!.*pass.*word|.*\s))(?!^.*\n)(?=.*\W*)(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{8,16}$";
        public const string Email = @"^[A-Za-z0-9_\-\+/{\t\n\r}#$%^\\&\[\]*=()|?'~`! :]+([.][A-Za-z0-9_\-\+/{\t\n\r}#$%^\\&\[\]*=()|?'~`! :]+)*@[A-Za-z0-9\[\]:]+([.-][A-Za-z0-9\[\]:]+)*\.[A-Za-z0-9\[\]:]+([-.][A-Za-z0-9\[\]:]+)*$";
        public const string Addressstring = @"^[A-Za-z0-9'-. `_,:&*#\""\(\)\/\\]*$";
        public const string Day = @"^(0[1-9]|[1-9]|[1-2][0-9]|3[0-1])$";
        public const string Month = @"^(0[1-9]|[1-9]|1[0-2])$";
        public const string Numeric = @"^[0-9]*$";

        public static readonly RegexOptions CommonRegexOptions = RegexOptions.Compiled | RegexOptions.CultureInvariant;
    }
}