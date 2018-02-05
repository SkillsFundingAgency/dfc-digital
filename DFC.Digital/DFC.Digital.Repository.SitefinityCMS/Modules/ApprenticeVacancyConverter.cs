using DFC.Digital.Data.Model;
using Telerik.Sitefinity.DynamicModules.Model;
using Telerik.Sitefinity.Model;

namespace DFC.Digital.Repository.SitefinityCMS.Modules
{
    public class ApprenticeVacancyConverter : IDynamicModuleConverter<ApprenticeVacancy>
    {
        public ApprenticeVacancy ConvertFrom(DynamicContent content)
        {
            return new ApprenticeVacancy
            {
                Title = content?.GetValue<Lstring>(nameof(ApprenticeVacancy.Title)),
                Location = content?.GetValue<Lstring>(nameof(ApprenticeVacancy.Location)),
                Url = content?.GetValue<Lstring>(nameof(ApprenticeVacancy.Url)),
                VacancyId = content?.GetValue<Lstring>(nameof(ApprenticeVacancy.VacancyId)),
                WageAmount = content?.GetValue<Lstring>(nameof(ApprenticeVacancy.WageAmount)),
                WageUnitType = content?.GetValue<Lstring>(nameof(ApprenticeVacancy.WageUnitType))
            };
        }
    }
}