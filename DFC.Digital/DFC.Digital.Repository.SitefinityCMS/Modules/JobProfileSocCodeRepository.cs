using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using System.Linq;
using Telerik.Sitefinity.GenericContent.Model;
using Telerik.Sitefinity.Model;

namespace DFC.Digital.Repository.SitefinityCMS.Modules
{
    public class JobProfileSocCodeRepository : IJobProfileSocCodeRepository
    {
        #region Fields

        private readonly IDynamicModuleRepository<SocCode> repository;
        private readonly IDynamicModuleConverter<ApprenticeVacancy> converter;
        private readonly IDynamicContentExtensions dynamicContentExtensions;

        #endregion Fields

        #region Ctor

        public JobProfileSocCodeRepository(IDynamicModuleRepository<SocCode> repository, IDynamicModuleConverter<ApprenticeVacancy> converter, IDynamicContentExtensions dynamicContentExtensions)
        {
            this.repository = repository;
            this.converter = converter;
            this.dynamicContentExtensions = dynamicContentExtensions;
        }

        #endregion Ctor

        #region JobProfileSocCodeRepository Implementations

        public IQueryable<ApprenticeVacancy> GetBySocCode(string socCode)
        {
            var socCodeItem = repository.Get(item => item.Visible && item.Status == ContentLifecycleStatus.Live && item.GetValue<string>(nameof(SocCode.SOCCode)) == socCode);
            var vacancies = dynamicContentExtensions.GetRelatedParentItems(socCodeItem, DynamicTypes.JobProfileApprenticeshipContentType, repository.GetProviderName());

            if (vacancies.Any())
            {
                return vacancies.Select(item => converter.ConvertFrom(item));
            }

            return Enumerable.Empty<ApprenticeVacancy>().AsQueryable();
        }

        public RepoActionResult UpdateSocOccupationalCode(SocCode socCode)
        {
            var socCodeItem = repository.Get(item => item.Visible && item.Status == ContentLifecycleStatus.Live && item.GetValue<string>(nameof(SocCode.SOCCode)) == socCode.SOCCode);

            if (socCodeItem != null)
            {
                var master = repository.GetMaster(socCodeItem);

                var temp = repository.GetTemp(master);

                temp.SetValue(nameof(SocCode.OccupationalCode), socCode.OccupationalCode);

                var updatedMaster = repository.CheckinTemp(temp);

                repository.Update(updatedMaster);
                repository.Commit();
            }

            return new RepoActionResult { Success = true };
        }

        #endregion JobProfileSocCodeRepository Implementations
    }
}