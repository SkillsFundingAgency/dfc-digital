using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using System.Linq;
using Telerik.Sitefinity.DynamicModules;
using Telerik.Sitefinity.DynamicModules.Model;
using Telerik.Sitefinity.GenericContent.Model;
using Telerik.Sitefinity.Model;
using Telerik.Sitefinity.RelatedData;

namespace DFC.Digital.Repository.SitefinityCMS.Modules
{
    public class JobProfileSocCodeRepository : DynamicModuleRepository, IJobProfileSocCodeRepository
    {
        #region Fields

        private const string JobProfileSocContentType = "Telerik.Sitefinity.DynamicTypes.Model.JobProfile.JobProfileSoc";
        private const string JobProfileApprenticeshipContentType = "Telerik.Sitefinity.DynamicTypes.Model.JobProfile.ApprenticeshipVacancy";
        private const string ModuleName = "Job Profile";

        private IDynamicModuleConverter<ApprenticeVacancy> converter;

        #endregion Fields

        #region Ctor

        public JobProfileSocCodeRepository(IDynamicModuleConverter<ApprenticeVacancy> converter)
            : base(ModuleName, JobProfileSocContentType)
        {
            this.converter = converter;
        }

        #endregion Ctor

        #region JobProfileSocCodeRepository Implementations

        public IQueryable<ApprenticeVacancy> GetBySocCode(string socCode)
        {
            var socCodeItem = Get(item => item.Visible && item.Status == ContentLifecycleStatus.Live && item.GetValue<string>("SOCCode") == socCode);

            if (socCodeItem != null)
            {
                var providerName = DynamicModuleManager.GetDefaultProviderName(ModuleName);
                var vacancies = socCodeItem
                    .GetRelatedParentItems(JobProfileApprenticeshipContentType, providerName)
                    .OfType<DynamicContent>()
                    .Where(d => d.Status == ContentLifecycleStatus.Live && d.Visible);

                if (vacancies.Any())
                {
                    return vacancies.Select(item => converter.ConvertFrom(item));
                }
            }

            return null;
        }

        #endregion JobProfileSocCodeRepository Implementations
    }
}