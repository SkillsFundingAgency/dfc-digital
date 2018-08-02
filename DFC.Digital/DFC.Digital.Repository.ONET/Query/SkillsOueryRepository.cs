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

    public class SkillsOueryRepository : ISkillsRepository
    {
        private readonly OnetSkillsFramework onetDbContext;

        public SkillsOueryRepository(OnetSkillsFramework onetDbContext)
        {
            this.onetDbContext = onetDbContext;
        }

        #region Implementation of ISkillsRepository
        public IQueryable<OnetAttribute> GetAbilitiesForONetOccupationCode(string oNetOccupationCode)
        {
            var attributes = from ability in onetDbContext.abilities
                             where ability.recommend_suppress != "Y"
                                        && ability.not_relevant != "Y"
                                        && ability.onetsoc_code == oNetOccupationCode
                             select new OnetAttribute
                             {
                                 OnetOccupationalCode = ability.onetsoc_code,
                                 Id = ability.element_id,
                                 Type = AttributeType.Ability,
                                 Score = ability.data_value,
                             };
            return attributes;
        }

        public IQueryable<OnetAttribute> GetKowledgeForONetOccupationCode(string oNetOccupationCode)
        {
            var attributes = from knowledge in onetDbContext.knowledges
                             where knowledge.recommend_suppress != "Y"
                                 && knowledge.not_relevant != "Y"
                                 && knowledge.onetsoc_code == oNetOccupationCode

                             select new OnetAttribute
                             {
                                 OnetOccupationalCode = knowledge.onetsoc_code,
                                 Id = knowledge.element_id,
                                 Type = AttributeType.Knowledge,
                                 Score = knowledge.data_value,
                             };
            return attributes;
        }

        public IQueryable<OnetAttribute> GetSkillsForONetOccupationCode(string oNetOccupationCode)
        {
            var attributes = from skill in onetDbContext.skills
                             where skill.recommend_suppress != "Y"
                                        && skill.not_relevant != "Y"
                                        && skill.onetsoc_code == oNetOccupationCode
                             select new OnetAttribute
                             {
                                 OnetOccupationalCode = skill.onetsoc_code,
                                 Id = skill.element_id,
                                 Type = AttributeType.Skill,
                                 Score = skill.data_value,
                             };
            return attributes;
        }

        public IQueryable<OnetAttribute> GetWorkStylesForONetOccupationCode(string oNetOccupationCode)
        {
            var attributes = from workStyle in onetDbContext.work_styles
                             where workStyle.recommend_suppress != "Y"
                                        && workStyle.onetsoc_code == oNetOccupationCode
                             select new OnetAttribute
                             {
                                 OnetOccupationalCode = workStyle.onetsoc_code,
                                 Id = workStyle.element_id,
                                 Type = AttributeType.WorkStyle,
                                 Score = workStyle.data_value,
                             };
            return attributes;
        }
        #endregion
    }
}
