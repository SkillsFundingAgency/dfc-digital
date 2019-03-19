using Sfa.Careers.Common.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Web.Mvc;
using Sfa.Careers.Common.Extensions;

namespace DFC.Digital.Web.Core
{
    // HomePostCodeAttribute is not Validation attribute and is used only for storage of the three regexes
    public class HomePostCodeAttribute : Attribute
    {
        public string UKPostCodeRegex { get; set; }
        public string EnglishOrBFPOPostCodeRegex { get; set; }
        public string BfpoPostCodeRegex { get; set; }

        public HomePostCodeAttribute() { }

        public HomePostCodeAttribute(string ukPostCodeRegex, string englishOrBFPOPostCodeRegex, string bfpoPostCodeRegex)
        {
            UKPostCodeRegex = ukPostCodeRegex;
            EnglishOrBFPOPostCodeRegex = englishOrBFPOPostCodeRegex;
            BfpoPostCodeRegex = bfpoPostCodeRegex;
        }
    }
}