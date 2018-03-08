using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using System.Collections.Generic;
using System.Linq;
using Telerik.Sitefinity.DynamicModules.Model;
using Telerik.Sitefinity.GenericContent.Model;

namespace DFC.Digital.Repository.SitefinityCMS.Modules
{
    internal class PreSearchFiltersRepository<T> : DynamicModuleRepository, IPreSearchFiltersRepository<T>
        where T : PreSearchFilter, new()
    {
        #region Fields

        private const string InterestContentType = "Telerik.Sitefinity.DynamicTypes.Model.PreSearchFilters.Interest";
        private const string EnablersContentType = "Telerik.Sitefinity.DynamicTypes.Model.PreSearchFilters.Enabler";
        private const string EducationContentType = "Telerik.Sitefinity.DynamicTypes.Model.PreSearchFilters.EntryQualification";
        private const string TrainingContentType = "Telerik.Sitefinity.DynamicTypes.Model.PreSearchFilters.TrainingRoute";
        private const string JobAreaType = "Telerik.Sitefinity.DynamicTypes.Model.PreSearchFilters.JobArea";
        private const string CareerFocusType = "Telerik.Sitefinity.DynamicTypes.Model.PreSearchFilters.CareerFocus";
        private const string PreferredTaskTypeType = "Telerik.Sitefinity.DynamicTypes.Model.PreSearchFilters.PreferredTaskType";
        private const string ModuleName = "Pre Search Filters";

        private IDynamicModuleConverter<T> converter;

        #endregion Fields

        #region Ctor

        public PreSearchFiltersRepository(IDynamicModuleConverter<T> converter)
            : base(ModuleName)
        {
            this.converter = converter;
        }

        #endregion Ctor

        #region IJobProfileRepository Implementations

        public IEnumerable<T> GetAllFilters()
        {
            IEnumerable<DynamicContent> filterItems = GetMany(item => item.Visible && item.Status == ContentLifecycleStatus.Live);

            if (filterItems?.Any() == true)
            {
                return filterItems.Select(item => converter.ConvertFrom(item));
            }

            return Enumerable.Empty<T>();
        }

        protected override void Initialise()
        {
            if (typeof(T) == typeof(PsfInterest))
            {
                SetContentType(InterestContentType);
            }
            else if (typeof(T) == typeof(PsfEnabler))
            {
                SetContentType(EnablersContentType);
            }
            else if (typeof(T) == typeof(PsfEntryQualification))
            {
                SetContentType(EducationContentType);
            }
            else if (typeof(T) == typeof(PsfTrainingRoute))
            {
                SetContentType(TrainingContentType);
            }
            else if (typeof(T) == typeof(PsfJobArea))
            {
                SetContentType(JobAreaType);
            }
            else if (typeof(T) == typeof(PsfCareerFocus))
            {
                SetContentType(CareerFocusType);
            }
            else if (typeof(T) == typeof(PsfPreferredTaskType))
            {
                SetContentType(PreferredTaskTypeType);
            }
        }

        #endregion IJobProfileRepository Implementations
    }
}