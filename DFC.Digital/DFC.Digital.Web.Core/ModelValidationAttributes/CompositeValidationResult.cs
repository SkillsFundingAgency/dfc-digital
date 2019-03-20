using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DFC.Digital.Web.Core.ModelValidationAttributes
{
    public class CompositeValidationResult : ValidationResult
    {
        private readonly List<ValidationResult> results = new List<ValidationResult>();

        public CompositeValidationResult(string errorMessage) : base(errorMessage)
        {
        }

        public CompositeValidationResult(string errorMessage, IEnumerable<string> memberNames) : base(errorMessage, memberNames)
        {
        }

        protected CompositeValidationResult(ValidationResult validationResult) : base(validationResult)
        {
        }

        public IEnumerable<ValidationResult> Results
        {
            get
            {
                return results;
            }
        }

        public void AddResult(ValidationResult validationResult)
        {
            results.Add(validationResult);
        }
    }
}