using DFC.Digital.Core;
using DFC.Digital.Core.Utilities;
using DFC.Digital.Web.Core;
using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace DFC.Digital.Web.Sitefinity.ContactUsModule.Mvc.Models
{
    public class PersonalContactDetails
    {
        [Display(Name = "Title")]
        [Required(ErrorMessage = "Select your title")]
        public string Title { get; set; }

        [StringLength(50, ErrorMessage = "First name too long (max. 50)")]
        [Display(Name = "First name")]
        [Required(ErrorMessage = "Enter your first name")]
        public string Firstname { get; set; }

        [StringLength(50, ErrorMessage = "Last name too long (max. 50)")]
        [Display(Name = "Last name")]
        [Required(ErrorMessage = "Enter your last name")]
        public string Lastname { get; set; }

        [StringLength(1000, ErrorMessage = "Feedback too long (max. 1000)")]
        [Display(Name = "Enter your questions for the advisers in the box below.")]
        [Required(ErrorMessage = "Enter your feedback")]
        public string Feedback { get; set; }

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
                DateTime dateOfBirth = default(DateTime);
                CultureInfo enGb = new CultureInfo("en-GB");
                string dob = string.Empty;
                if (!string.IsNullOrEmpty(DateOfBirthDay) && !string.IsNullOrEmpty(DateOfBirthMonth) && !string.IsNullOrEmpty(DateOfBirthYear))
                {
                    dob = string.Format("{0}/{1}/{2}", DateOfBirthDay.PadLeft(2, '0'), DateOfBirthMonth.PadLeft(2, '0'), DateOfBirthYear.PadLeft(4, '0'));
                }

                if (string.IsNullOrEmpty(dob) && string.IsNullOrEmpty(DateOfBirthDay) && string.IsNullOrEmpty(DateOfBirthMonth) && string.IsNullOrEmpty(DateOfBirthYear))
                {
                    // DateOfBirth can be null
                    return null;
                }
                else
                {
                    if (DateTime.TryParseExact(dob, "dd/MM/yyyy", enGb, DateTimeStyles.AdjustToUniversal, out dateOfBirth))
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

        [RegularExpression(RegexPatterns.Day, ErrorMessage = "Day must be a number between 1 and 31")]
        public string DateOfBirthDay { get; set; }

        [RegularExpression(RegexPatterns.Month, ErrorMessage = "Month must be a number between 1 and 12")]
        public string DateOfBirthMonth { get; set; }

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

        [Display(Name = "Email address")]
        [Required(ErrorMessage = "Enter your email address")]
        [EmailAddress(ErrorMessage = "Enter a valid email address")]
        public string EmailAddress { get; set; }

        [Compare("EmailAddress", ErrorMessage = "Email addresses don't match")]
        [Display(Name = "Confirm email address")]
        public string ConfirmEmailAddress { get; set; }

        [Accept(ErrorMessage = "You must accept our Terms and Conditions")]
        [Display(Name = "I accept the <a href=\"/about-us/terms-and-conditions\">terms and conditions</a> and I am 13 or over")]
        public bool AcceptTermsAndConditions { get; set; }
    }
}