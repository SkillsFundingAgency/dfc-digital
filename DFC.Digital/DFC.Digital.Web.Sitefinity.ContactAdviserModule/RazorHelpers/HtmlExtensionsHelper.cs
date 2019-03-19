using System.Web.Mvc;

namespace DFC.Digital.Web.Sitefinity.ContactUsModule
{
    public static class HtmlExtensionsHelper
    {
        public static string GetErrorClass(this HtmlHelper htmlHelper, string propertyName, ModelStateDictionary modelState)
        {
            if (modelState.IsValid == false)
            {
                return modelState.ContainsKey(propertyName) && modelState[propertyName].Errors.Count > 0 ? "govuk-form-group--error" : string.Empty;
            }

            return string.Empty;
        }

    }
}