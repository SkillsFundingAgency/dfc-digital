using AutoMapper;
using DFC.Digital.Repository.ONET.DataModel;
using System;
using System.Linq;

namespace DFC.Digital.Repository.ONET.BusinessRule
{
    using System.Collections.Generic;
    using Data.Interfaces;
    using Data.Model;

    public class SkillsFrameworkBusinessRuleEngine : ISkillFrameworkBusinessRuleEngine
    {
        private readonly OnetSkillsFramework onetDbContext;
        private readonly IMapper autoMapper;

        public SkillsFrameworkBusinessRuleEngine(OnetSkillsFramework onetDbContext, IMapper autoMapper)
        {
            this.onetDbContext = onetDbContext;
            this.autoMapper = autoMapper;
        }

        public IEnumerable<OnetAttribute> CombineSimilarAttributes(IOrderedQueryable<OnetAttribute> result)
        {
            throw new NotImplementedException();
        }

        public int GetDigitalSkillRank(string onetSocCode)
        {
            throw new NotImplementedException();
        }

        public IQueryable<OnetAttribute> GetSelectedAbilities(string onetOccupationalCode)
        {
            var selectedAbilities = from ability    in onetDbContext.abilities
                                    join cmr        in onetDbContext.content_model_reference
                                        on ability.element_id.Substring(0, 7) equals cmr.element_id
                                    where ability.recommend_suppress != "Y"
                                        && ability.not_relevant != "Y"
                                        && ability.onetsoc_code == onetOccupationalCode
                                    group ability by new
                                    {
                                        cmr.element_name,
                                        cmr.description,
                                        ability.element_id,
                                        ability.onetsoc_code
                                    }
                                    into abilityGroup
                                    select new OnetAttribute
                                    {
                                        OnetOccupationalCode = abilityGroup.Key.onetsoc_code,
                                        Id = abilityGroup.Key.element_id,
                                        Description = abilityGroup.Key.description,
                                        Name = abilityGroup.Key.element_name,
                                        Type = AttributeType.Abilities,
                                        Score = abilityGroup.Sum(x => x.data_value) / 2
                                    };

            return selectedAbilities;
        }

        public IQueryable<OnetAttribute> GetSelectedKnowledge(string onetOccupationalCode)
        {
            var selectedKnowledge = from knwldg in onetDbContext.knowledges
                                    join cmr    in onetDbContext.content_model_reference
                                        on knwldg.element_id.Substring(0, 7) equals cmr.element_id
                                    where knwldg.recommend_suppress != "Y"
                                        && knwldg.not_relevant != "Y"
                                        && knwldg.onetsoc_code == onetOccupationalCode
                                    group knwldg by new
                                    {
                                        cmr.element_name,
                                        cmr.description,
                                        knwldg.element_id,
                                        knwldg.onetsoc_code
                                    }
                                    into knwldgGroup
                                    select new OnetAttribute
                                    {
                                        OnetOccupationalCode = knwldgGroup.Key.onetsoc_code,
                                        Id = knwldgGroup.Key.element_id,
                                        Description = knwldgGroup.Key.description,
                                        Name = knwldgGroup.Key.element_name,
                                        Type = AttributeType.Knowledge,
                                        Score = knwldgGroup.Sum(v => v.data_value) / 2
                                    };

            return selectedKnowledge;
        }

        public IQueryable<OnetAttribute> GetSelectedSkills(string onetOccupationalCode)
        {
            var selectedSkills = from skill in onetDbContext.skills
                                 join cmr   in onetDbContext.content_model_reference
                                    on skill.element_id.Substring(0, 7) equals cmr.element_id
                                 where skill.recommend_suppress != "Y"
                                    && skill.not_relevant != "Y"
                                    && skill.onetsoc_code == onetOccupationalCode
                                 group skill by new
                                 {
                                     skill.onetsoc_code,
                                     cmr.element_name,
                                     cmr.description,
                                     skill.element_id
                                 }
                                 into skillGroup
                                 select new OnetAttribute
                                 {
                                     OnetOccupationalCode = skillGroup.Key.onetsoc_code,
                                     Id = skillGroup.Key.element_id,
                                     Description = skillGroup.Key.description,
                                     Name = skillGroup.Key.element_name,
                                     Type = AttributeType.Skills,
                                     Score = skillGroup.Sum(x => x.data_value) / 2
                                 };

            return selectedSkills;
        }

        public IQueryable<OnetAttribute> GetSelectedWorkStyles(string onetOccupationalCode)
        {
            var selectedAbilities = from workkStyle in onetDbContext.work_styles
                                    join cmr        in onetDbContext.content_model_reference
                                    on workkStyle.element_id.Substring(0, 7) equals cmr.element_id
                                    where workkStyle.recommend_suppress != "Y"
                                        && workkStyle.onetsoc_code == onetOccupationalCode
                                    group workkStyle by new
                                    {
                                        workkStyle.onetsoc_code,
                                        cmr.element_name,
                                        cmr.description,
                                        workkStyle.element_id
                                    }
                                    into workStyleGroup
                                    select new OnetAttribute
                                    {
                                        OnetOccupationalCode = workStyleGroup.Key.onetsoc_code,
                                        Id = workStyleGroup.Key.element_id,
                                        Description = workStyleGroup.Key.description,
                                        Name = workStyleGroup.Key.element_name,
                                        Type = AttributeType.WorkStyles,
                                        Score = workStyleGroup.Sum(x => x.data_value)
                                    };

            return selectedAbilities;
        }

        public void TrimDuplicateMathsSkillOrKnowledge(IOrderedQueryable<OnetAttribute> result)
        {
            throw new NotImplementedException();
        }
    }
}