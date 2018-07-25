using AutoMapper;
using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using DFC.Digital.Repository.ONET.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DFC.Digital.Repository.ONET.Query
{
    public class SkillsMappingQueryRepository : IRelatedSkillsMappingRepository
    {
        private readonly OnetSkillsFramework onetDbContext;
        private readonly IMapper autoMapper;
        private readonly ISkillFrameworkBusinessRuleEngine businessRuleEngine;

        public SkillsMappingQueryRepository(OnetSkillsFramework onetDbContext, IMapper autoMapper, ISkillFrameworkBusinessRuleEngine businessRuleEngine)
        {
            this.onetDbContext = onetDbContext;
            this.autoMapper = autoMapper;
            this.businessRuleEngine = businessRuleEngine;
        }

        public IEnumerable<RelatedSkillMapping> GetByONetOccupationalCode(string onetOccupationalCode)
        {
            var result = businessRuleEngine.GetSelectedKnowledge(onetOccupationalCode)
                .Union(businessRuleEngine.GetSelectedSkills(onetOccupationalCode))
                .Union(businessRuleEngine.GetSelectedAbilities(onetOccupationalCode))
                .Union(businessRuleEngine.GetSelectedWorkStyles(onetOccupationalCode))
                .OrderByDescending(r => r.Score);

            businessRuleEngine.TrimDuplicateMathsSkillOrKnowledge(result);
            var combinedResult = businessRuleEngine.CombineSimilarAttributes(result);

            return autoMapper.Map<IEnumerable<RelatedSkillMapping>>(combinedResult);
        }
    }
}
