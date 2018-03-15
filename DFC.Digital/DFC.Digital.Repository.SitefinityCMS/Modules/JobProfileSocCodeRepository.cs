using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using System;
using System.Linq;
using Telerik.Sitefinity.GenericContent.Model;
using Telerik.Sitefinity.Model;

namespace DFC.Digital.Repository.SitefinityCMS.Modules
{
    public class JobProfileSocCodeRepository : IJobProfileSocCodeRepository
    {
        #region Fields

        private readonly IDynamicModuleRepository<IJobProfileSocCodeRepository> repository;
        private readonly IDynamicModuleRepository<ApprenticeVacancy> avRepository;
        private readonly IDynamicModuleConverter<ApprenticeVacancy> converter;

        #endregion Fields

        #region Ctor

        public JobProfileSocCodeRepository(IDynamicModuleRepository<IJobProfileSocCodeRepository> repository, IDynamicModuleRepository<ApprenticeVacancy> avRepository, IDynamicModuleConverter<ApprenticeVacancy> converter)
        {
            this.repository = repository;
            this.avRepository = avRepository;
            this.converter = converter;
        }

        #endregion Ctor

        #region JobProfileSocCodeRepository Implementations

        public IQueryable<ApprenticeVacancy> GetBySocCode(string socCode)
        {
            var socCodeItem = repository.Get(item => item.Visible && item.Status == ContentLifecycleStatus.Live && item.GetValue<string>("SOCCode") == socCode);
            var avItems = avRepository.GetMany(item => item.Visible && item.Status == ContentLifecycleStatus.Live && item.GetValue<Guid>("SOCCode") == socCodeItem.Id);
            return avItems.Select(av => converter.ConvertFrom(av));
        }

        #endregion JobProfileSocCodeRepository Implementations
    }
}