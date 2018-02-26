using Autofac;
using Autofac.Integration.Mvc;
using System;
using System.Web;
using System.Web.Mvc;
using Telerik.Sitefinity.Frontend.Mvc.Infrastructure.Controllers;

namespace DFC.Digital.Web.Sitefinity.Core
{
    public class AutofacContainerFactory : FrontendControllerFactory
    {
        protected override IController GetControllerInstance(System.Web.Routing.RequestContext requestContext, Type controllerType)
        {
            if (controllerType == null)
            {
                return null;
            }

            var autofacLifetimeScope = AutofacDependencyResolver.Current.RequestLifetimeScope;
            var controller = autofacLifetimeScope.ResolveOptional(controllerType) as IController;
            if (controller == null)
            {
                controller = base.GetControllerInstance(requestContext, controllerType);
            }
            else
            {
                HttpContext.Current.Items["dfc-controller"] = controllerType.Name;
            }

            return controller;
        }
    }
}