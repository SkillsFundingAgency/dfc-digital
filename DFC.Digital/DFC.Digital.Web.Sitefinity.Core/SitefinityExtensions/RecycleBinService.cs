using Autofac;
using Autofac.Integration.Mvc;
using DFC.Digital.Data.Interfaces;
using DFC.Digital.Web.Sitefinity.Core.Interfaces;
using System;
using Telerik.Sitefinity.Security.Claims;
using Telerik.Sitefinity.Services;

namespace DFC.Digital.Web.Sitefinity.Core.SitefinityExtensions
{
    public class RecycleBinService : IRecycleBinService
    {
        private IRecyleBinRepository recyleBin;

        public RecycleBinService()
        {
            var autofacLifetimeScope = AutofacDependencyResolver.Current.RequestLifetimeScope;
            recyleBin = autofacLifetimeScope.Resolve<IRecyleBinRepository>();
        }

        public void RecycleBinClearAppVacancies(int itemCount)
        {
            if (!SystemManager.CurrentHttpContext.Request.IsAuthenticated)
            {
                throw new UnauthorizedAccessException("The current user is not allowed access");
            }

            recyleBin.DeleteVacanciesPermanently(itemCount);
        }
    }
}