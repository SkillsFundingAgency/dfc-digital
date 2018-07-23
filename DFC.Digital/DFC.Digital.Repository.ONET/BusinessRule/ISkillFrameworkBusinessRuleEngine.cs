using System.Linq;

namespace DFC.Digital.Repository.ONET
{
    public interface ISkillFrameworkBusinessRuleEngine
    {
        int GetDigitalSkillRank(string onetSocCode);

        IQueryable<OnetAttribute> GetSelectedKnowledge(string onetOccupationalCode);

        IQueryable<OnetAttribute> GetSelectedSkills(string onetOccupationalCode);

        IQueryable<OnetAttribute> GetSelectedAbilities(string onetOccupationalCode);

        IQueryable<OnetAttribute> GetSelectedWorkStyles(string onetOccupationalCode);

        void TrimDuplicateMathsSkillOrKnowledge(IOrderedQueryable<OnetAttribute> result);

        void CombineSimilarAttributes(IOrderedQueryable<OnetAttribute> result);
    }
}