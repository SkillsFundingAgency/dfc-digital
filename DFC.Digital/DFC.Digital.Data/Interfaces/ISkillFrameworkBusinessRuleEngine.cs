using DFC.Digital.Data.Model;
using System.Collections.Generic;
using System.Linq;

namespace DFC.Digital.Data.Interfaces
{
    public interface ISkillFrameworkBusinessRuleEngine
    {
        DigitalSkillsLevel GetDigitalSkillsLevel(int count);

        IQueryable<OnetSkill> GetAllRawOnetSkillsForOccupation(string onetOccupationalCode);

        IEnumerable<OnetSkill> RemoveDFCSuppressions(IEnumerable<OnetSkill> attributes);

        IEnumerable<OnetSkill> RemoveDuplicateAttributes(IEnumerable<OnetSkill> attributes);

        IEnumerable<OnetSkill> MoveBottomLevelAttributesUpOneLevel(IEnumerable<OnetSkill> attributes);

        IEnumerable<OnetSkill> AverageOutscoreScales(IEnumerable<OnetSkill> attributes);

        IEnumerable<OnetSkill> BoostMathsSkills(IEnumerable<OnetSkill> attributes);

        IEnumerable<OnetSkill> CombineSimilarAttributes(IList<OnetSkill> attributes);

        IEnumerable<OnetSkill> SelectFinalAttributes(IEnumerable<OnetSkill> attributes);

        IEnumerable<OnetSkill> AddTitlesToAttributes(IEnumerable<OnetSkill> attributes);
    }
}