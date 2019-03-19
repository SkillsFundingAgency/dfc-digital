using System;
using System.ComponentModel.DataAnnotations;

namespace DFC.Digital.Web.Core
{
    // TelephoneNumberAttribute inherits from DoubleRegexAttribute and it's logic is applied in ExtendedDataAnnotationsModelValidatorProvider,
    // where under condition if ContactPreference set to Phone, DoubleRegexAttribute is enforced to be Required and
    // and the property value is valided against Phone and Mobile Regexes
    public class TelephoneNumberAttribute : DoubleRegexAttribute
    {
        /// <summary>
        /// Gets or Sets the dependent property for this attribute, this is used by the validation method.
        /// </summary>
        public string DependsOn { get; set; }

        public TelephoneNumberAttribute() : base()
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
            base.IsRequired = contactPref == Channel.Phone;

            return base.IsValid(value, validationContext);
        }
    }
}