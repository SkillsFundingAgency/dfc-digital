using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace DFC.Digital.Web.Core
{
    // ConditionalRequiredAttribute is used to inforce validation logic, which depends on the value of other property
    public class ConditionalRequiredAttribute : ValidationAttribute, IClientValidatable
    {
        public string PropertyName { get; set; }

        public string DependsOn { get; set; }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            // One property could depend on more then one other property
            foreach (var prop in DependsOn.Split(','))
            {
                var result = IsConditionalValid(prop, value, validationContext);
                if (result != null)
                {
                    return result;
                }
            }

            return null;
        }

        public virtual ValidationResult IsConditionalValid(string property, object value, ValidationContext validationContext)
        {
            // We obatain the other property ...
            var dependsOnProp = validationContext.ObjectType.GetProperty(property.Trim());

            // ... and if other property's value is NOT null/empty we have to provide value for the validated property.
            // (It applies directly only for AddressLine1.
            //   Other properties IsConditionalValid gets overriden in BFPOAddressAttribute, which inherites from this ConditionalRequiredAttribute)
            if (!string.IsNullOrEmpty(dependsOnProp.GetValue(validationContext.ObjectInstance)?.ToString()) && string.IsNullOrEmpty(value?.ToString()))
            {
                return new ValidationResult(ErrorMessage);
            }

            return null;
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            //throw new NotImplementedException();
            return Enumerable.Empty<ModelClientValidationRule>();
        }
    }
}