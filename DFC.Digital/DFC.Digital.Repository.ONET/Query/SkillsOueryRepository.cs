using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using DFC.Digital.Repository.ONET.DataModel;
using System.Linq;

namespace DFC.Digital.Repository.ONET
{

    public class SkillsOueryRepository : ISkillsRepository
    {
        private readonly OnetSkillsFramework onetDbContext;

        public SkillsOueryRepository(OnetSkillsFramework onetDbContext)
        {
            this.onetDbContext = onetDbContext;
        }

        #region Implementation of ISkillsRepository
        public IQueryable<OnetSkill> GetAbilitiesForONetOccupationCode(string oNetOccupationCode)
        {
            var attributes = from ability in onetDbContext.abilities
                             where ability.recommend_suppress != "Y"
                                        && ability.not_relevant != "Y"
                                        && ability.onetsoc_code == oNetOccupationCode
                             select new OnetSkill
                             {
                                 OnetOccupationalCode = ability.onetsoc_code,
                                 Id = ability.element_id,
                                 Category = CategoryType.Ability,
                                 Score = ability.data_value,
                             };
            return attributes;
        }

        public IQueryable<OnetSkill> GetKowledgeForONetOccupationCode(string oNetOccupationCode)
        {
            var attributes = from knowledge in onetDbContext.knowledges
                             where knowledge.recommend_suppress != "Y"
                                 && knowledge.not_relevant != "Y"
                                 && knowledge.onetsoc_code == oNetOccupationCode

                             select new OnetSkill
                             {
                                 OnetOccupationalCode = knowledge.onetsoc_code,
                                 Id = knowledge.element_id,
                                 Category = CategoryType.Knowledge,
                                 Score = knowledge.data_value,
                             };
            return attributes;
        }

        public IQueryable<OnetSkill> GetSkillsForONetOccupationCode(string oNetOccupationCode)
        {
            var attributes = from skill in onetDbContext.skills
                             where skill.recommend_suppress != "Y"
                                        && skill.not_relevant != "Y"
                                        && skill.onetsoc_code == oNetOccupationCode
                             select new OnetSkill
                             {
                                 OnetOccupationalCode = skill.onetsoc_code,
                                 Id = skill.element_id,
                                 Category = CategoryType.Skill,
                                 Score = skill.data_value,
                             };
            return attributes;
        }

        public IQueryable<OnetSkill> GetWorkStylesForONetOccupationCode(string oNetOccupationCode)
        {
            var attributes = from workStyle in onetDbContext.work_styles
                             where workStyle.recommend_suppress != "Y"
                                        && workStyle.onetsoc_code == oNetOccupationCode
                             select new OnetSkill
                             {
                                 OnetOccupationalCode = workStyle.onetsoc_code,
                                 Id = workStyle.element_id,
                                 Category = CategoryType.WorkStyle,
                                 Score = workStyle.data_value,
                             };
            return attributes;
        }
        #endregion
    }
}
