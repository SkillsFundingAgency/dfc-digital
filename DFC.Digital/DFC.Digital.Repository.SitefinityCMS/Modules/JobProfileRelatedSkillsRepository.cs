using DFC.Digital.Core.Logging;
using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.Sitefinity.DynamicModules.Model;
using Telerik.Sitefinity.GenericContent.Model;
using Telerik.Sitefinity.Model;

namespace DFC.Digital.Repository.SitefinityCMS.Modules
{
    public class JobProfileRelatedSkillsRepository : IJobProfileRelatedSkillsRepository
    {
        #region Fields

        private readonly IDynamicModuleRepository<WhatItTakesSkill> repository;
        private readonly IDynamicModuleConverter<WhatItTakesSkill> converter;
        private readonly IDynamicContentExtensions dynamicContentExtensions;

        #endregion Fields

        #region Ctor

        public JobProfileRelatedSkillsRepository(IDynamicModuleRepository<WhatItTakesSkill> repository, IDynamicModuleConverter<WhatItTakesSkill> converter, IDynamicContentExtensions dynamicContentExtensions)
        {
            this.repository = repository;
            this.converter = converter;
            this.dynamicContentExtensions = dynamicContentExtensions;
        }

        #endregion Ctor

        #region JobProfileRelatedSkillsRepository Implementations

       public IEnumerable<WhatItTakesSkill> GetRelatedSkills(IQueryable<string> relatedSkills, int maximumItemsToReturn)
        {
            var skills = new List<WhatItTakesSkill>();

            if (relatedSkills != null)
            {
                foreach (var skillUrl in relatedSkills.Take(maximumItemsToReturn))
                {
                    var relatedSkill = converter.ConvertFrom(repository.Get(item =>
                        item.UrlName == skillUrl && item.Status == ContentLifecycleStatus.Live && item.Visible == true));

                    if (relatedSkill != null)
                    {
                        skills.Add(relatedSkill);
                    }
                }
            }

            return skills;
        }
        #endregion JobProfileRelatedSkillsRepository Implementations
    }
}