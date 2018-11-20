using Autofac;
using Autofac.Integration.Mvc;
using DFC.Digital.Data.Interfaces;
using DFC.Digital.Web.Sitefinity.Core.Interfaces;
using System.ServiceModel;
using System.ServiceModel.Activation;

namespace DFC.Digital.Web.Sitefinity.Core.SitefinityExtensions
{
    [ServiceBehavior(IncludeExceptionDetailInFaults = true, InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Single)]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
    public class RecycleBinService : IRecycleBinService
    {
        private IRecyleBinRepository recyleBin;

        public RecycleBinService()
        {
        }

        public RecycleBinService(IRecyleBinRepository recyleBin)
        {
            this.recyleBin = recyleBin;
        }

        public void ClearAppVacanciesRecycleBin()
        {
            if (recyleBin == null)
            {
                var autofacLifetimeScope = AutofacDependencyResolver.Current.RequestLifetimeScope;
                recyleBin = autofacLifetimeScope.Resolve<IRecyleBinRepository>();
            }

            recyleBin.DeleteVacanciesPermanently();
        }
    }
}