using DFC.Digital.Data.Model;
using System;
using Telerik.Sitefinity.DynamicModules.Model;
using Telerik.Sitefinity.Model;

namespace DFC.Digital.Repository.SitefinityCMS.Modules
{
    public class PreSearchFilterOnetSkillConverter : PreSearchFilterConverter<PsfOnetSkill>
    {
        private readonly IDynamicContentExtensions dynamicContentExtensions;

        public PreSearchFilterOnetSkillConverter(IDynamicContentExtensions dynamicContentExtensions) : base(dynamicContentExtensions)
        {
            this.dynamicContentExtensions = dynamicContentExtensions;
        }

        public new PsfOnetSkill ConvertFrom(DynamicContent content) => new PsfOnetSkill
        {
            Title = dynamicContentExtensions.GetFieldValue<Lstring>(content, nameof(PreSearchFilter.Title)),
            Description = dynamicContentExtensions.GetFieldValue<Lstring>(content, nameof(PreSearchFilter.Description)),
            NotApplicable = false,
            Order = 1,
            UrlName = dynamicContentExtensions.GetFieldValue<Lstring>(content, nameof(PreSearchFilter.UrlName)),
            Id = dynamicContentExtensions.GetFieldValue<Guid>(content, nameof(PreSearchFilter.Id)),
        };
    }
}