using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace DFC.Digital.Web.Core
{
    public class DoubleRegexAttribute : ValidationAttribute, IClientValidatable
    {
        public DoubleRegexAttribute()
        {
        }

        public DoubleRegexAttribute(string firstRegex, string secondRegex, bool isAndOperator, bool isRequired)
        {
            FirstRegex = firstRegex.Trim();
            SecondRegex = secondRegex.Trim();
            IsAndOperator = isAndOperator;
            IsRequired = isRequired;
        }

        // DoubleRegexAttribute is generic validation against two regexes ...
        public string FirstRegex { get; set; }

        public string SecondRegex { get; set; }

        // ... and can implement AND or OR operators for the two regexes.
        public bool IsAndOperator { get; set; }

        // It also can be applied if Required
        public bool IsRequired { get; set; }

        public override bool IsValid(object value)
        {
            string objectValue = Convert.ToString(value);
            if (!IsRequired && string.IsNullOrEmpty(objectValue))
            {
                return true;
            }
            else
            {
                // The validation is applied only if it is Required with option to use AND or OR operator
                return HelperMethods.IsValidDoubleRegexValue(objectValue, FirstRegex, SecondRegex, IsAndOperator);
            }
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule()
            {
                ErrorMessage = string.IsNullOrEmpty(ErrorMessage) ? FormatErrorMessage(metadata.DisplayName) : ErrorMessage,
                ValidationType = "doubleregex",
            };

            rule.ValidationParameters.Add("firstregex", FirstRegex);
            rule.ValidationParameters.Add("secondregex", SecondRegex);
            rule.ValidationParameters.Add("drerrormessage", ErrorMessage);
            rule.ValidationParameters.Add("isandoperator", IsAndOperator.ToString());
            rule.ValidationParameters.Add("isdrrequired", IsRequired.ToString());
            yield return rule;
        }
    }
}