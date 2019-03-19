using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Web.Mvc;

namespace DFC.Digital.Web.Core
{
    public class DifferAttribute : System.ComponentModel.DataAnnotations.CompareAttribute, IClientValidatable
    {
        // The Name of the other property, which we are comparing against
        public string DifferOtherProperty { get; set; }

        public DifferAttribute(string otherProperty) : base(otherProperty)
        {
            if (otherProperty == null)
            {
                throw new ArgumentNullException("otherProperty");
            }
            DifferOtherProperty = otherProperty;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            PropertyInfo property = validationContext.ObjectType.GetProperty(DifferOtherProperty);
            if (property == null)
            {
                return new ValidationResult("Other (Differ) Property is not set", new[] { nameof(DifferOtherProperty) });
            }

            // The logic of the DifferAttribute is an oposite of Compare attribute.
            // The value of property MUST DIFFER from the value of other property.
            if (object.Equals(value, property.GetValue(validationContext.ObjectInstance, null)))
            {
                return new ValidationResult(ErrorMessage);
            }
            else
            {
                return null;
            }
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule()
            {
                ErrorMessage = String.IsNullOrEmpty(ErrorMessage) ? FormatErrorMessage(metadata.DisplayName) : ErrorMessage,
                ValidationType = "differ",
            };
            var otherproperties = new List<string>
                               {
                                   DifferOtherProperty
                               };
            var errorMessages = new List<string>
                                    {
                                        ErrorMessage
                                    };

            rule.ValidationParameters.Add("otherproperties", otherproperties.ToConcatenatedString(" "));
            rule.ValidationParameters.Add("errormessages", errorMessages.ToConcatenatedString());
            yield return rule;
        }
    }
}