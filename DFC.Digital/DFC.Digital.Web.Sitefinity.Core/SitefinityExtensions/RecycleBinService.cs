using Autofac;
using Autofac.Integration.Mvc;
using DFC.Digital.Web.Sitefinity.Core.Interfaces;
using System.Diagnostics.CodeAnalysis;

namespace DFC.Digital.Web.Sitefinity.Core.SitefinityExtensions
{
    [ExcludeFromCodeCoverage]
    public class RecycleBinService : IRecycleBinService
    {
        private readonly RecycleBinProcesser recyleBinProcessor;

        public RecycleBinService()
        {
            var autofacLifetimeScope = AutofacDependencyResolver.Current.RequestLifetimeScope;
            recyleBinProcessor = autofacLifetimeScope.Resolve<RecycleBinProcesser>();
        }

        public bool RecycleBinClearAppVacancies(int itemCount)
        {
            return recyleBinProcessor.RunProcess(itemCount);
        }
    }
}