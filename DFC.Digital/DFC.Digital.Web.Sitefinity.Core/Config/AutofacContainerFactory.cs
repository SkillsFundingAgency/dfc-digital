using Autofac;
using System;
using System.Web.Mvc;
using Telerik.Sitefinity.Frontend.Mvc.Infrastructure.Controllers;

namespace DFC.Digital.Web.Sitefinity.Core
{
    public class AutofacContainerFactory : FrontendControllerFactory
    {
        private ILifetimeScope autofacLifetimeScope;

        public AutofacContainerFactory(ILifetimeScope scope)
        {
            this.autofacLifetimeScope = scope;
        }

        protected override IController GetControllerInstance(System.Web.Routing.RequestContext requestContext, Type controllerType)
        {
            if (controllerType == null)
            {
                return null;
            }

            var controller = this.autofacLifetimeScope.ResolveOptional(controllerType) as IController;
            if (controller == null)
            {
                controller = base.GetControllerInstance(requestContext, controllerType);
            }

            return controller;
        }
    }
}