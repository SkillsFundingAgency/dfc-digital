using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace DFC.Digital.Web.Core
{
    public class AcceptAttribute : ValidationAttribute, IClientValidatable
    {
        public AcceptAttribute()
        {
        }

        public override bool IsValid(object value)
        {
            // AcceptAttribute is applied only to Boolean Properties and allow only values set to true
            if (value == null)
            {
                return false;
            }

            if (value.GetType() != typeof(bool))
            {
                throw new InvalidOperationException("Can only be used on boolean properties.");
            }

            return (bool)value == true;
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            yield return new ModelClientValidationRule
            {
                ErrorMessage = string.IsNullOrEmpty(ErrorMessage) ? FormatErrorMessage(metadata.DisplayName) : ErrorMessage,
                ValidationType = "enforcetrue"
            };
        }
    }
}