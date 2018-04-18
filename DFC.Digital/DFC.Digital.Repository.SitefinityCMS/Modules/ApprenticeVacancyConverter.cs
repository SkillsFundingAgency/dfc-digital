using DFC.Digital.Data.Model;
using System;
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
                Title = content?.GetValueOrDefault<Lstring>(nameof(ApprenticeVacancy.Title)),
                Location = content?.GetValueOrDefault<Lstring>(nameof(ApprenticeVacancy.Location)),
                URL = content != null ? new Uri(content.GetValueOrDefault<Lstring>(nameof(ApprenticeVacancy.URL)), UriKind.RelativeOrAbsolute) : new Uri(string.Empty),
                VacancyId = content?.GetValueOrDefault<Lstring>(nameof(ApprenticeVacancy.VacancyId)),
                WageAmount = content?.GetValueOrDefault<Lstring>(nameof(ApprenticeVacancy.WageAmount)),
                WageUnitType = content?.GetValueOrDefault<Lstring>(nameof(ApprenticeVacancy.WageUnitType))
            };
        }
    }
}