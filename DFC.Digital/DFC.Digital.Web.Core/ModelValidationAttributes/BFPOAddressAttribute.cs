//using System;
//using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Sfa.Careers.Common.Services;

//namespace DFC.Digital.Web.Core
//{
//    public class BFPOAddressAttribute : ConditionalRequiredAttribute
//    {
//        // The property is validated when the other dependent property is NOT BFPO post code (In our case Town)
//        public bool IsNotBfpo { get; set; }

//        // The property is validated when the other dependent property is NOT UK, English or BFPO post code (In our case AlternativePostCode)
//        public bool IsNonEnglishBfpo { get; set; }

//        // The property where the three regexes needed for execution of the logic are stored (In our case HomePostCode)
//        public string DependsOnPropertyForAttribute { get; set; }

//        // The three regexes needed for execution of the logic
//        public string UKPostCodeRegex { get; set; }
//        public string EnglishOrBFPOPostCodeRegex { get; set; }
//        public string BfpoPostCodeRegex { get; set; }

//        public override ValidationResult IsConditionalValid(string property, object value, ValidationContext validationContext)
//        {
//            var dependsOnProp = validationContext.ObjectType.GetProperty(property.Trim());
//            var postcode = dependsOnProp.GetValue(validationContext.ObjectInstance)?.ToString();

//            if (!string.IsNullOrEmpty(UKPostCodeRegex) && !string.IsNullOrEmpty(EnglishOrBFPOPostCodeRegex) && !string.IsNullOrEmpty(BfpoPostCodeRegex) && !string.IsNullOrEmpty(postcode?.Trim()))
//            {
//                if (IsNotBfpo)
//                {
//                    // It applies to Town  
//                    if (!ServiceFunctions.IsValidRegexValue(postcode, BfpoPostCodeRegex) && string.IsNullOrEmpty(value?.ToString()))
//                    {
//                        return new ValidationResult(ErrorMessage);
//                    }
//                } // It applies to AddressLine2 & AddressLine3 (when other dependent property is valid UK BFPO postcode)
//                else if (!IsNonEnglishBfpo && ServiceFunctions.IsValidDoubleRegexValue(postcode, UKPostCodeRegex, BfpoPostCodeRegex, true) && string.IsNullOrEmpty(value?.ToString()))
//                {
//                    return new ValidationResult(ErrorMessage);
//                }

//                // It applies to AlternativePostCode
//                // Here we are evaluating the value of the HomePostCode (postcode), not AlternativePostCode
//                if (IsNonEnglishBfpo && !ServiceFunctions.IsValidDoubleRegexValue(postcode, UKPostCodeRegex, EnglishOrBFPOPostCodeRegex, true) && string.IsNullOrEmpty(value?.ToString()))
//                {
//                    return new ValidationResult(ErrorMessage);
//                }
//            }

//            return null;
//        }
//    }
//}
