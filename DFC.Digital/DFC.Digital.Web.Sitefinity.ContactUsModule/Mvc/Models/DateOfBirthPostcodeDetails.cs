using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using DFC.Digital.Web.Core;

namespace DFC.Digital.Web.Sitefinity.ContactUsModule.Mvc.Models
{
    public class DateOfBirthPostcodeDetails : PersonalContactDetails
    {
        [AgeRange(
        13,
        120,
        MinAgeErrorMessage = "You must be over 13 to use this service",
        MaxAgeErrorMessage = "You must be under 120 to use this service",
        InvalidErrorMessage = "Enter a valid date of birth")]
        [Required(ErrorMessage = "Enter a valid date of birth")]
        [Display(Name = "Date of birth")]
        public DateTime? DateOfBirth
        {
            get
            {
                var enGb = new CultureInfo("en-GB");
                var dob = string.Empty;
                if (!string.IsNullOrEmpty(DateOfBirthDay) && !string.IsNullOrEmpty(DateOfBirthMonth) && !string.IsNullOrEmpty(DateOfBirthYear))
                {
                    dob =
                        $"{DateOfBirthDay.PadLeft(2, '0')}/{DateOfBirthMonth.PadLeft(2, '0')}/{DateOfBirthYear.PadLeft(4, '0')}";
                }

                if (string.IsNullOrEmpty(dob) && string.IsNullOrEmpty(DateOfBirthDay) && string.IsNullOrEmpty(DateOfBirthMonth) && string.IsNullOrEmpty(DateOfBirthYear))
                {
                    // DateOfBirth can be null
                    return null;
                }
                else
                {
                    if (DateTime.TryParseExact(dob, "dd/MM/yyyy", enGb, DateTimeStyles.AdjustToUniversal, out var dateOfBirth))
                    {
                        return dateOfBirth;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
        }

        [Display(Name = "Day")]
        [RegularExpression(RegexPatterns.Day, ErrorMessage = "Day must be a number between 1 and 31")]
        public string DateOfBirthDay { get; set; }

        [Display(Name = "Month")]
        [RegularExpression(RegexPatterns.Month, ErrorMessage = "Month must be a number between 1 and 12")]
        public string DateOfBirthMonth { get; set; }

        [Display(Name = "Year")]
        [RegularExpression(RegexPatterns.Numeric, ErrorMessage = "Year must be a number")]
        public string DateOfBirthYear { get; set; }

        [DoubleRegex(
            FirstRegex = RegexPatterns.UKPostCode,
            SecondRegex = RegexPatterns.EnglishOrBFPOPostCode,
            IsAndOperator = true,
            IsRequired = true,
            ErrorMessage = "Postcode must be an English or BFPO postcode")]
        [StringLength(10, ErrorMessage = "Postcode too long (max. 10)")]
        [Display(Name = "Post code")]
        [Required(ErrorMessage = "Enter your postcode")]
        public string Postcode { get; set; }
    }
}