using System.Web.Mvc;

namespace DFC.Digital.Web.Core
{
    public static class IsDebugExtension
    {
        public static bool IsDebugSymbol(this HtmlHelper htmlHelper)
        {
#if DEBUG
            return true;
#else
            return false;
#endif
        }
    }
}