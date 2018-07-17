using DFC.Digital.Data.Model;
using Telerik.Sitefinity.DynamicModules.Model;
using Telerik.Sitefinity.Model;

namespace DFC.Digital.Repository.SitefinityCMS.Modules
{
    public class OnetSkillConverter : IDynamicModuleConverter<OnetSkill>
    {
        private readonly IDynamicContentExtensions dynamicContentExtensions;

        public OnetSkillConverter(IDynamicContentExtensions dynamicContentExtensions)
        {
            this.dynamicContentExtensions = dynamicContentExtensions;
        }

        public OnetSkill ConvertFrom(DynamicContent content)
        {
            return new OnetSkill
            {
                Description = dynamicContentExtensions.GetFieldValue<Lstring>(content, nameof(OnetSkill.Description)),
                Title = dynamicContentExtensions.GetFieldValue<Lstring>(content, nameof(OnetSkill.Title)),
                OnetElementId =
                    dynamicContentExtensions.GetFieldValue<Lstring>(content, nameof(OnetSkill.OnetElementId))
            };
        }
    }
}
