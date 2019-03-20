using System.ComponentModel.DataAnnotations;

namespace DFC.Digital.Web.Core
{
    public class ValidateUnderlyingType : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            //var results = new List<ValidationResult>();
            //var context = new ValidationContext(value, null, null);
            //return Validator.TryValidateObject(value, context, results, true) && results.Count == 0;
            return false;
        }

        //protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        //{
        //    var results = new List<ValidationResult>();
        //    var context = new ValidationContext(value, null, null);

        //    Validator.TryValidateObject(value, context, results, true);

        //    if (results.Count != 0)
        //    {
        //        var compositeResults = new CompositeValidationResult(string.Format("Validation for {0} failed!", validationContext.DisplayName));
        //        results.ForEach(compositeResults.AddResult);

        //        return compositeResults;
        //    }

        //    return ValidationResult.Success;
        //}
    }
}