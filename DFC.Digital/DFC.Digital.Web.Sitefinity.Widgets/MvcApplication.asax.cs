﻿using System.Web.Mvc;

namespace DFC.Digital.Web.Sitefinity.Widgets
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected static void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
        }
    }
}