using DFC.Digital.Data.Model;
using System.Collections.Generic;
using System.Linq;

namespace DFC.Digital.Data.Interfaces
{
    public interface ISkillFrameworkBusinessRuleEngine
    {
        int GetDigitalSkillRank(string onetSocCode);

        IQueryable<OnetAttribute> GetSelectedKnowledge(string onetOccupationalCode);

        IQueryable<OnetAttribute> GetSelectedSkills(string onetOccupationalCode);

        IQueryable<OnetAttribute> GetSelectedAbilities(string onetOccupationalCode);

        IQueryable<OnetAttribute> GetSelectedWorkStyles(string onetOccupationalCode);

        void TrimDuplicateMathsSkillOrKnowledge(IOrderedQueryable<OnetAttribute> attributes);

        IEnumerable<OnetAttribute> CombineSimilarAttributes(IOrderedQueryable<OnetAttribute> attributes);
    }
}