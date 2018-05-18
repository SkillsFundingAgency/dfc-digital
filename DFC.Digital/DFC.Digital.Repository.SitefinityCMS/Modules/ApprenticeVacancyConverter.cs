using DFC.Digital.Data.Model;
using DFC.Digital.Repository.SitefinityCMS.Extensions;
using System;
using Telerik.Sitefinity.DynamicModules.Model;
using Telerik.Sitefinity.Model;

namespace DFC.Digital.Repository.SitefinityCMS.Modules
{
    public class ApprenticeVacancyConverter : IDynamicModuleConverter<ApprenticeVacancy>
    {
        private readonly IDynamicContentExtensions dynamicContentExtensions;

        public ApprenticeVacancyConverter(IDynamicContentExtensions dynamicContentExtensions)
        {
            this.dynamicContentExtensions = dynamicContentExtensions;
        }

        public ApprenticeVacancy ConvertFrom(DynamicContent content)
        {
            return new ApprenticeVacancy
            {
                Title = dynamicContentExtensions.GetFieldValue<Lstring>(content, nameof(ApprenticeVacancy.Title)),
                Location = dynamicContentExtensions.GetFieldValue<Lstring>(content, nameof(ApprenticeVacancy.Location)),
                URL = content != null ? new Uri(dynamicContentExtensions.GetFieldValue<Lstring>(content, nameof(ApprenticeVacancy.URL)), UriKind.RelativeOrAbsolute) : new Uri(string.Empty),
                VacancyId = dynamicContentExtensions.GetFieldValue<Lstring>(content, nameof(ApprenticeVacancy.VacancyId)),
                WageAmount = dynamicContentExtensions.GetFieldValue<Lstring>(content, nameof(ApprenticeVacancy.WageAmount)),
                WageUnitType = dynamicContentExtensions.GetFieldValue<Lstring>(content, nameof(ApprenticeVacancy.WageUnitType))
            };
        }
    }
}