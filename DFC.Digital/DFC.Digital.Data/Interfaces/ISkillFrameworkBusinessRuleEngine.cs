using DFC.Digital.Data.Model;
using System.Collections.Generic;
using System.Linq;

namespace DFC.Digital.Data.Interfaces
{
    public interface ISkillFrameworkBusinessRuleEngine
    {
        DigitalSkillsLevel GetDigitalSkillsLevel(int count);

        IQueryable<OnetAttribute> GetAllRawOnetSkillsForOccupation(string onetOccupationalCode);

        IEnumerable<OnetAttribute> RemoveDFCSuppressions(IEnumerable<OnetAttribute> attributes);

        IEnumerable<OnetAttribute> RemoveDuplicateAttributes(IEnumerable<OnetAttribute> attributes);

        IEnumerable<OnetAttribute> MoveBottomLevelAttributesUpOneLevel(IEnumerable<OnetAttribute> attributes);

        IEnumerable<OnetAttribute> AverageOutScoreScales(IEnumerable<OnetAttribute> attributes);

        IEnumerable<OnetAttribute> BoostMathsSkills(IEnumerable<OnetAttribute> attributes);

        IEnumerable<OnetAttribute> CombineSimilarAttributes(IList<OnetAttribute> attributes);

        IEnumerable<OnetAttribute> SelectFinalAttributes(IEnumerable<OnetAttribute> attributes);

        IEnumerable<OnetAttribute> AddTitlesToAttributes(IEnumerable<OnetAttribute> attributes);
    }
}