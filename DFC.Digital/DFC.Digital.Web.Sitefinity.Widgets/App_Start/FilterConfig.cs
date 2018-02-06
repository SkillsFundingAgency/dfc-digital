using System.Web.Mvc;

namespace DFC.Digital.Web.Sitefinity.Widgets
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}