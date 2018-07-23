using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using System.Collections.Generic;
using Telerik.Sitefinity.GenericContent.Model;

namespace DFC.Digital.Repository.SitefinityCMS.Modules
{
    public class JobProfileRelatedSkillsRepository : IJobProfileRelatedSkillsRepository
    {
        #region Fields

        private readonly IDynamicModuleRepository<WhatItTakesSkill> repository;
        private readonly IDynamicModuleConverter<WhatItTakesSkill> converter;

        #endregion Fields

        #region Ctor

        public JobProfileRelatedSkillsRepository(IDynamicModuleRepository<WhatItTakesSkill> repository, IDynamicModuleConverter<WhatItTakesSkill> converter)
        {
            this.repository = repository;
            this.converter = converter;
        }

        #endregion Ctor

        #region JobProfileRelatedSkillsRepository Implementations

        public IEnumerable<WhatItTakesSkill> GetContextualisedSkillsById(IEnumerable<string> skillsIdCollection)
        {
            if (skillsIdCollection != null)
            {
                foreach (var skillUrl in skillsIdCollection)
                {
                    var relatedSkill = converter.ConvertFrom(repository.Get(item => item.UrlName == skillUrl && item.Status == ContentLifecycleStatus.Live && item.Visible == true));
                    if (relatedSkill != null)
                    {
                        yield return relatedSkill;
                    }
                }
            }
        }

        #endregion JobProfileRelatedSkillsRepository Implementations
    }
}