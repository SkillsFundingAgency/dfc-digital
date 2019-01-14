using Autofac;
using Autofac.Integration.Mvc;
using DFC.Digital.Data.Interfaces;
using DFC.Digital.Web.Sitefinity.Core.Interfaces;
using System;
using Telerik.Sitefinity.Security.Claims;
using Telerik.Sitefinity.Services;

namespace DFC.Digital.Web.Sitefinity.Core.SitefinityExtensions
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class RecycleBinService : IRecycleBinService
    {
        private readonly RecycleBinProcesser recyleBin;

        public RecycleBinService()
        {
            var autofacLifetimeScope = AutofacDependencyResolver.Current.RequestLifetimeScope;
            recyleBin = autofacLifetimeScope.Resolve<RecycleBinProcesser>();
        }

        public void RecycleBinClearAppVacancies(int itemCount)
        {
            recyleBin.RunProcess(itemCount);
        }
    }
}