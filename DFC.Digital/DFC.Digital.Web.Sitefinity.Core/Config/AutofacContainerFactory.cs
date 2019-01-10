using Autofac;
using Autofac.Integration.Mvc;
using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.SessionState;
using Telerik.Sitefinity.Frontend.Mvc.Infrastructure.Controllers;
using Telerik.Sitefinity.Mvc.Proxy;

namespace DFC.Digital.Web.Sitefinity.Core
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class AutofacContainerFactory : FrontendControllerFactory
    {
        public override IController CreateController(RequestContext requestContext, string controllerName)
        {
            return base.CreateController(requestContext, controllerName);
        }

        public override void ReleaseController(IController controller)
        {
            base.ReleaseController(controller);
        }

        public override string ResolveControllerName(MvcProxyBase proxy)
        {
            return base.ResolveControllerName(proxy);
        }

        public override string ResolveControllerName(Type proxyType)
        {
            return base.ResolveControllerName(proxyType);
        }

        public override Type ResolveControllerType(string controllerName)
        {
            var something = base.ResolveControllerType(controllerName);
            return something;
        }

        protected override SessionStateBehavior GetControllerSessionBehavior(RequestContext requestContext, Type controllerType)
        {
            return base.GetControllerSessionBehavior(requestContext, controllerType);
        }

        protected override IController GetControllerInstance(System.Web.Routing.RequestContext requestContext, Type controllerType)
        {
            if (controllerType == null)
            {
                return base.GetControllerInstance(requestContext, controllerType);
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