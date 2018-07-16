using DFC.Digital.Core.Logging;
using DFC.Digital.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.Sitefinity.DynamicModules.Model;
using Telerik.Sitefinity.Model;

namespace DFC.Digital.Repository.SitefinityCMS.Modules
{
    public class JobProfileRelatedSkillConverter : IDynamicModuleConverter<WhatItTakesSkill>
    {
        #region Fields

        private const string RelatedSkillsField = "RelatedSkills";
        private const string RelatedSkillField = "RelatedSkill";

        private readonly IDynamicContentExtensions dynamicContentExtensions;

        #endregion Fields

        #region Ctor

        public JobProfileRelatedSkillConverter(IDynamicContentExtensions dynamicContentExtensions)
        {
            this.dynamicContentExtensions = dynamicContentExtensions;
        }

        #endregion Ctor

        public WhatItTakesSkill ConvertFrom(DynamicContent content)
        {
            var whatItTakesSkill = new WhatItTakesSkill();
            var skillDescription = string.Empty;
                try
                {
                    skillDescription = GetSkillDescription(content);
                }
                catch (LoggedException)
                {
                    //This is here in case somebody deletes the related skill in Sitefinity, This is an error
                    //The exception will have been logged and the system needs to continue but not display this skill to the citizen
                }

            if (skillDescription != string.Empty)
            {
                return new WhatItTakesSkill
                {
                    Title = dynamicContentExtensions.GetFieldValue<Lstring>(content, nameof(WhatItTakesSkill.Title)),
                    Description = skillDescription,
                    Contextualised = dynamicContentExtensions.GetFieldValue<Lstring>(content, nameof(WhatItTakesSkill.Contextualised)),
                };
            }
            else
            {
                return null;
            }
        }

        private string GetSkillDescription(DynamicContent relatedSkill)
        {
            var skill = dynamicContentExtensions.GetRelatedItems(relatedSkill, RelatedSkillField).FirstOrDefault();
            if (skill != null)
            {
                return dynamicContentExtensions.GetFieldValue<Lstring>(skill, nameof(WhatItTakesSkill.Description));
            }

            return string.Empty;
        }
    }
}