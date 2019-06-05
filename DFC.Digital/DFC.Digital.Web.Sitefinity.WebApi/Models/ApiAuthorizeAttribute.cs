using System;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using Telerik.Sitefinity.Security;
using Telerik.Sitefinity.Security.Claims;
using Telerik.Sitefinity.Security.Model;
using Telerik.Sitefinity.Utilities.MS.ServiceModel.Web;
using Telerik.Sitefinity.Web.Services;

namespace DFC.Digital.Web.Sitefinity.WebApi
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class ApiAuthorizeAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(HttpActionContext filterContext)
        {
            var user = ClaimsManager.GetCurrentIdentity();

            if (string.IsNullOrWhiteSpace(this.Roles) || !user.IsAuthenticated)
            {
                base.OnAuthorization(filterContext);
            }
            else
            {
                foreach (var role in this.Roles.Split(','))
                {
                    if (user.Roles.Any(r => r.Name == role))
                    {
                        return;
                    }
                }

                this.HandleUnauthorizedRequest(filterContext);
            }
        }

        protected override void HandleUnauthorizedRequest(HttpActionContext actionContext)
        {
               throw new UnauthorizedAccessException();
        }
    }
}