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
        private readonly IDynamicModuleConverter<SocCode> socCodeConverter;
        private readonly IDynamicContentExtensions dynamicContentExtensions;

        #endregion Fields

        #region Ctor

        public JobProfileSocCodeRepository(IDynamicModuleRepository<SocCode> repository, IDynamicModuleConverter<ApprenticeVacancy> converter, IDynamicModuleConverter<SocCode> socCodeConverter, IDynamicContentExtensions dynamicContentExtensions)
        {
            this.repository = repository;
            this.converter = converter;
            this.dynamicContentExtensions = dynamicContentExtensions;
            this.socCodeConverter = socCodeConverter;
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
            var socCodeItem = repository.Get(item => item.Visible && item.Status == ContentLifecycleStatus.Live && item.UrlName == socCode.UrlName);

            if (socCodeItem != null)
            {
                var master = repository.GetMaster(socCodeItem);

                var temp = repository.GetTemp(master);

                temp.SetValue(nameof(SocCode.ONetOccupationalCode), socCode.ONetOccupationalCode);

                var updatedMaster = repository.CheckinTemp(temp);

                repository.Update(updatedMaster);
                repository.Commit();
            }

            return new RepoActionResult { Success = true };
        }

        public IQueryable<SocCode> GetLiveSocCodes()
        {
            var socCodes = repository.GetMany(item => item.Visible && item.Status == ContentLifecycleStatus.Live);

            if (socCodes.Any())
            {
                return socCodes.Select(item => socCodeConverter.ConvertFrom(item));
            }

            return Enumerable.Empty<SocCode>().AsQueryable();
        }

        #endregion JobProfileSocCodeRepository Implementations
    }
}