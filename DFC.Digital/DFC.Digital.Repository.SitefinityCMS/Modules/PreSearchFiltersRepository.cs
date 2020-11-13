using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using System.Collections.Generic;
using System.Linq;
using Telerik.Sitefinity.DynamicModules.Model;
using Telerik.Sitefinity.GenericContent.Model;

namespace DFC.Digital.Repository.SitefinityCMS.Modules
{
    public class PreSearchFiltersRepository<T> : IPreSearchFiltersRepository<T>
        where T : PreSearchFilter, new()
    {
        #region Fields

        private readonly IDynamicModuleRepository<T> repository;
        private IDynamicModuleConverter<T> converter;

        #endregion Fields

        #region Ctor

        public PreSearchFiltersRepository(IDynamicModuleRepository<T> repository, IDynamicModuleConverter<T> converter)
        {
            this.repository = repository;
            this.converter = converter;
        }

        #endregion Ctor

        #region IJobProfileRepository Implementations

        //IList change to all the way up.
        public IEnumerable<T> GetAllFilters()
        {
            IEnumerable<DynamicContent> filterItems = repository.GetMany(item => item.Visible && item.Status == ContentLifecycleStatus.Live);

            var filters = filterItems?.Select(item => converter.ConvertFrom(item));
            return filters.Where(f => f != null).ToList();
        }

        #endregion IJobProfileRepository Implementations
    }
}