using System.Web;
using System.Web.Mvc;

namespace DFC.Digital.Web.Sitefinity.ContactAdviserModule
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
