using Owin;
using System;
using Telerik.Sitefinity.Owin;
using Telerik.Sitefinity.Utilities.TypeConverters;

namespace DFC.Digital.Web.Sitefinity
{
    public class Startup
    {
        private const string SitefinityOpenIdConnectWebApiAuthenticationMiddleware = "Telerik.Sitefinity.Authentication.Owin.OpenId.SitefinityOpenIdConnectWebApiAuthenticationMiddleware";

        /// <summary>
        /// <see cref="Telerik.Sitefinity.Authentication.Owin.OpenId.SitefinityOpenIdConnectWebApiAuthenticationMiddleware"/> type
        /// </summary>
        private static readonly Type SitefinityOpenIdConnectMiddlewareType = TypeResolutionService.ResolveType(SitefinityOpenIdConnectWebApiAuthenticationMiddleware);

        /// <summary>
        /// Configurations the specified application.
        /// </summary>
        /// <param name="app">The application.</param>
        /// <exception cref="ArgumentNullException">app</exception>
        public void Configuration(IAppBuilder app)
        {
            if (app == null)
            {
                throw new ArgumentNullException("app");
            }

            // Register default Sitefinity middlewares in the pipeline
            app.UseSitefinityMiddleware();

            app.Map("/dfcapi", (IAppBuilder innerApp) =>
            {
                Type type = SitefinityOpenIdConnectMiddlewareType;
                object[] objArray = new object[] { innerApp, null };
                objArray[1] = new string[] { "openid" };
                innerApp.Use(type, objArray);
            });
        }
    }
}