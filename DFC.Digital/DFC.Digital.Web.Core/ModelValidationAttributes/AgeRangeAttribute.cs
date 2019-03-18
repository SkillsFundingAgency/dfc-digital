using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Reflection;
using System.Web.Mvc;

namespace DFC.Digital.Web.Core
{
    public class AgeRangeAttribute : ValidationAttribute, IClientValidatable
    {
        public AgeRangeAttribute(int minAge, int maxAge)
        {
            MinAge = minAge;
            MaxAge = maxAge;
        }

        public int MinAge { get; set; }

        public int MaxAge { get; set; }

        public string MinAgeErrorMessage { get; set; }

        public string MaxAgeErrorMessage { get; set; }

        public string InvalidErrorMessage { get; set; }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(System.Web.Mvc.ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule()
            {
                ErrorMessage = String.IsNullOrEmpty(ErrorMessage) ? FormatErrorMessage(metadata.DisplayName) : ErrorMessage,
                ValidationType = "agerange",
            };
            var dates = new List<string>
                               {
                                   MinAge.ToString(),
                                   MaxAge.ToString()
                               };
            var errorMessages = new List<string>
                                    {
                                        MinAgeErrorMessage,
                                        MaxAgeErrorMessage,
                                        InvalidErrorMessage
                                    };

            rule.ValidationParameters.Add("dates", dates.ToConcatenatedString(" "));
            rule.ValidationParameters.Add("errormessages", errorMessages.ToConcatenatedString());
            yield return rule;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            PropertyInfo propertyDateOfBirthDay = validationContext.ObjectType.GetProperty("DateOfBirthDay");
            PropertyInfo propertyDateOfBirthMonth = validationContext.ObjectType.GetProperty("DateOfBirthMonth");
            PropertyInfo propertyDateOfBirthYear = validationContext.ObjectType.GetProperty("DateOfBirthYear");

            if (propertyDateOfBirthDay == null)
            {
                return new ValidationResult("DateOfBirthDay Property is not set");
            }
            else if (propertyDateOfBirthMonth == null)
            {
                return new ValidationResult("DateOfBirthMonth Property is not set");
            }
            else if (propertyDateOfBirthYear == null)
            {
                return new ValidationResult("DateOfBirthYear Property is not set");
            }
            else
            {
                string dateOfBirthDay = propertyDateOfBirthDay.GetValue(validationContext.ObjectInstance, null) != null ? propertyDateOfBirthDay.GetValue(validationContext.ObjectInstance, null).ToString() : string.Empty;
                string dateOfBirthMonth = propertyDateOfBirthMonth.GetValue(validationContext.ObjectInstance, null) != null ? propertyDateOfBirthMonth.GetValue(validationContext.ObjectInstance, null).ToString() : string.Empty;
                string dateOfBirthYear = propertyDateOfBirthYear.GetValue(validationContext.ObjectInstance, null) != null ? propertyDateOfBirthYear.GetValue(validationContext.ObjectInstance, null).ToString() : string.Empty;

                // The DateOfBirthDay is concatinated value of three fields for Day, Month and Year
                DateTime dateOfBirth = default(DateTime);
                CultureInfo enGb = new CultureInfo("en-GB");
                string dob = string.Empty;
                if (!string.IsNullOrEmpty(dateOfBirthDay) && !string.IsNullOrEmpty(dateOfBirthMonth) && !string.IsNullOrEmpty(dateOfBirthYear))
                {
                    dob = string.Format("{0}/{1}/{2}", dateOfBirthDay.PadLeft(2, '0'), dateOfBirthMonth.PadLeft(2, '0'), dateOfBirthYear.PadLeft(4, '0'));
                }

                // AgeRangeAttribute is not Required value and having no value is allowed
                // If we want to make it required, we can just add RequiredAttribute to the same property
                if (string.IsNullOrEmpty(dob) && string.IsNullOrEmpty(dateOfBirthDay) && string.IsNullOrEmpty(dateOfBirthMonth) && string.IsNullOrEmpty(dateOfBirthYear))
                {
                    // AgeRange is NOT required
                    return null;
                }
                else
                {
                    // If we have a value for DateOfBirth we are assesing, whether value is valid and if it is whether is above the MinAge and below MaxAge
                    if (DateTime.TryParseExact(dob, "dd/MM/yyyy", enGb, DateTimeStyles.AdjustToUniversal, out dateOfBirth))
                    {
                        //valid dateOfBirth;
                        if (dateOfBirth.AddYears(MinAge).Date > DateTime.UtcNow.Date)
                        {
                            // but younger than min age
                            return new ValidationResult(MinAgeErrorMessage);
                        }
                        else if (dateOfBirth.AddYears(MaxAge).Date < DateTime.UtcNow.Date)
                        {
                            // but older than max age
                            return new ValidationResult(MaxAgeErrorMessage);
                        }
                        else
                        {
                            // correct age range
                            return null;
                        }
                    }
                    else
                    {
                        // NOT valid dateOfBirth;
                        return new ValidationResult(InvalidErrorMessage);
                    }
                }
            }
        }
    }
}