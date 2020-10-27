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
            return new PsfOnetSkill
            {
                Title = title,
                Description = dynamicContentExtensions.GetFieldValue<Lstring>(content, nameof(PreSearchFilter.Description)),
                NotApplicable = title.Equals("None", StringComparison.OrdinalIgnoreCase) || title.Equals("Skip", StringComparison.OrdinalIgnoreCase),
                Order = title.Equals("Skip", StringComparison.OrdinalIgnoreCase) ? 100 : title.Equals("None", StringComparison.OrdinalIgnoreCase) ? 1 : 2,
                UrlName = dynamicContentExtensions.GetFieldValue<Lstring>(content, nameof(PreSearchFilter.UrlName)),
                Id = dynamicContentExtensions.GetFieldValue<Guid>(content, nameof(PreSearchFilter.Id)),
            };
        }
    }
}