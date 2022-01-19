using System.Web;
using System.Web.Mvc;

namespace DFC.Digital.MigrationTool
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
