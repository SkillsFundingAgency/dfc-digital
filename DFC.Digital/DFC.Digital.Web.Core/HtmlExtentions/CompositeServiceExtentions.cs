using DFC.Digital.Core;
using System.Configuration;
using System.Web.Mvc;

namespace DFC.Digital.Web.Core
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public static class CompositeServiceExtentions
    {
        public static string GetCompositeServiceEndPoint(this HtmlHelper helper)
        {
            return ConfigurationManager.AppSettings[Constants.MicroServiceHelpPreviewEndPoint];
        }
    }
}
