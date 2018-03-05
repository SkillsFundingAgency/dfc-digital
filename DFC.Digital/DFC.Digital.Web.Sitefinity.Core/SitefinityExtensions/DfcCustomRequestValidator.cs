using System.Linq;
using System.Web;
using Telerik.Sitefinity.Security.Claims;

namespace DFC.Digital.Web.Sitefinity.Core
{
    public class DfcCustomRequestValidator : CustomRequestValidator
    {
        // In order to exclude a specific field from validation, add it here in lowercase
        internal static readonly string[] UnrestrictedKeys = new string[] { "searchterm" };

        protected override bool IsValidRequestString(HttpContext context, string value, System.Web.Util.RequestValidationSource requestValidationSource, string collectionKey, out int validationFailureIndex)
        {
            var isValid = base.IsValidRequestString(context, value, requestValidationSource, collectionKey, out validationFailureIndex);

            if (!isValid)
            {
                if (UnrestrictedKeys.Contains(collectionKey.ToLowerInvariant()))
                {
                    return true;
                }
            }

            return isValid;
        }
    }
}