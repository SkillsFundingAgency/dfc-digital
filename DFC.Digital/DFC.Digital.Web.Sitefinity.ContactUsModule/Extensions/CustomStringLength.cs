using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Telerik.Sitefinity.Localization;

namespace DFC.Digital.Web.Sitefinity.ContactUsModule.Extensions
{
    public class CustomStringLength : StringLengthAttribute
    {
        private readonly int maximumLength;

        public CustomStringLength(int maximumLength) : base(maximumLength)
        {
            this.maximumLength = maximumLength;
        }

        public override bool IsValid(object value)
        {
            return base.IsValid(value?.ToString().Replace("\r\n", "\n"));
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var textValue = value?.ToString().Replace("\r\n", "\n");
            return base.IsValid(textValue, validationContext);
        }
    }
}