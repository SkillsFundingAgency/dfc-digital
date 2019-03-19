using Sfa.Careers.Common.Enums;
using Sfa.Careers.Common.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Web.Mvc;
using Sfa.Careers.Common.Extensions;
using Sfa.Careers.Common.Models.AccountDetails;
using System.Linq;

namespace DFC.Digital.Web.Core
{
    public class MobilePhoneAttribute : ValidationAttribute, IClientValidatable
    {
        /// <summary>
        /// Gets or Sets the mobile phone regular expression used for validation
        /// </summary>
        public string MobilePhoneRegex { get; set; }

        /// <summary>
        /// Gets or Sets the dependent property for conditional validation
        /// </summary>
        public string DependsOn { get; set; }

        public MobilePhoneAttribute()
        {
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            // We have to get the selection value of the property on which the logic is dependent (ContactPreference)
            var propertyContactPreference = validationContext.ObjectType.GetProperty(this.DependsOn);
            if (propertyContactPreference == null)
            {
                throw new MissingMemberException("Missing member DependsOn - Should be set to contact preference.");
            }

            var contactPref = (Channel)propertyContactPreference.GetValue(validationContext.ObjectInstance);

            // Get all properties, where MobilePhoneAttribute is applied
            var propertyTelephoneNumbers = validationContext.ObjectType.GetProperties().Where(p => p.CustomAttributes.Any(a => a.AttributeType == typeof(MobilePhoneAttribute)));
            if (propertyTelephoneNumbers == null || propertyTelephoneNumbers.Count() == 0)
            {
                throw new InvalidOperationException("Cannot find properties referencing MobilePnoneAttribute - Expected telephone and alternative telephone.");
            }

            if (contactPref == Channel.Text) // The logic is applied only if selection of the ContactPreference is Text
            {
                // If ANY of the validated properties is Mobile number validation rule is satisfied. If not ErrorMessage is returned as validation result
                bool hasMobileNumber = propertyTelephoneNumbers.Any(p => ServiceFunctions.IsValidRegexValue(p.GetValue(validationContext.ObjectInstance)?.ToString(), MobilePhoneRegex));
                if (!hasMobileNumber)
                {
                    return new ValidationResult(ErrorMessage);
                }
            }

            return null;
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule()
            {
                ErrorMessage = String.IsNullOrEmpty(ErrorMessage) ? FormatErrorMessage(metadata.DisplayName) : ErrorMessage,
                ValidationType = "contactpreference",
            };

            rule.ValidationParameters.Add("mobilephoneregex", MobilePhoneRegex);
            rule.ValidationParameters.Add("errormessage", ErrorMessage);

            yield return rule;
        }
    }
}