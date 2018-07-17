using DFC.Digital.Data.Model;
using Telerik.Sitefinity.DynamicModules.Model;
using Telerik.Sitefinity.Model;

namespace DFC.Digital.Repository.SitefinityCMS.Modules
{
    public class OnetSkillConverter : IDynamicModuleConverter<FrameworkSkill>
    {
        private readonly IDynamicContentExtensions dynamicContentExtensions;

        public OnetSkillConverter(IDynamicContentExtensions dynamicContentExtensions)
        {
            this.dynamicContentExtensions = dynamicContentExtensions;
        }

        public FrameworkSkill ConvertFrom(DynamicContent content)
        {
            return new FrameworkSkill
            {
                Description = dynamicContentExtensions.GetFieldValue<Lstring>(content, nameof(FrameworkSkill.Description)),
                Title = dynamicContentExtensions.GetFieldValue<Lstring>(content, nameof(FrameworkSkill.Title)),
                OnetElementId =
                    dynamicContentExtensions.GetFieldValue<Lstring>(content, nameof(FrameworkSkill.OnetElementId))
            };
        }
    }
}
