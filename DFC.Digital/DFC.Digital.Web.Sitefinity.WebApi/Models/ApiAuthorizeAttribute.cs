﻿using System;
using System.Web.Http;
using System.Web.Http.Controllers;
using Telerik.Sitefinity.Web.Services;

namespace DFC.Digital.Web.Sitefinity.WebApi
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class ApiAuthorizeAttribute : AuthorizeAttribute
    {
        protected override void HandleUnauthorizedRequest(HttpActionContext actionContext)
        {
            base.HandleUnauthorizedRequest(actionContext);
            ServiceUtility.RequestBackendUserAuthentication();
        }
    }
}