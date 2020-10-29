using DFC.Digital.Data.Model;
using System;
using Telerik.Sitefinity.DynamicModules.Model;
using Telerik.Sitefinity.Model;

namespace DFC.Digital.Repository.SitefinityCMS.Modules
{
    public class PreSearchFilterOnetSkillConverter : IDynamicModuleConverter<PsfOnetSkill>
    {
        private readonly IDynamicContentExtensions dynamicContentExtensions;

        public PreSearchFilterOnetSkillConverter(IDynamicContentExtensions dynamicContentExtensions)
        {
            this.dynamicContentExtensions = dynamicContentExtensions;
        }

        public PsfOnetSkill ConvertFrom(DynamicContent content)
        {
            var title = dynamicContentExtensions.GetFieldValue<Lstring>(content, nameof(PreSearchFilter.Title));
            var psfTitle = dynamicContentExtensions.GetFieldValue<Lstring>(content, "PSFLabel");
            var psfDescription = dynamicContentExtensions.GetFieldValue<Lstring>(content, $"PSF{nameof(PreSearchFilter.Description)}");
            return new PsfOnetSkill
            {
                Title = string.IsNullOrEmpty(psfTitle) ? title : psfTitle,
                Description = !string.IsNullOrEmpty(psfDescription) ? psfDescription : dynamicContentExtensions.GetFieldValue<Lstring>(content, nameof(PreSearchFilter.Description)),
                NotApplicable = dynamicContentExtensions.GetFieldValue<bool>(content, $"PSF{nameof(PreSearchFilter.NotApplicable)}"),
                Order = dynamicContentExtensions.GetFieldValue<decimal?>(content, $"PSF{nameof(PreSearchFilter.Order)}") ?? 0,
                UrlName = dynamicContentExtensions.GetFieldValue<Lstring>(content, nameof(PreSearchFilter.UrlName)),
                Id = dynamicContentExtensions.GetFieldValue<Guid>(content, nameof(PreSearchFilter.Id)),
            };
        }
    }
}