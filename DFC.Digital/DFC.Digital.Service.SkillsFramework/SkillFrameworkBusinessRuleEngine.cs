using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DFC.Digital.Service.SkillsFramework
{
    public class OnetBusinessRulesEngine : ISkillFrameworkBusinessRuleEngine
    {
        public IEnumerable<OnetAttribute> AddTitlesToAttributes(IEnumerable<OnetAttribute> attributes)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<OnetAttribute> AverageOutScoreScales(IEnumerable<OnetAttribute> attributes)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<OnetAttribute> BoostMathsSkills(IEnumerable<OnetAttribute> attributes)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<OnetAttribute> CombineSimilarAttributes(IEnumerable<OnetAttribute> attributes)
        {
            throw new NotImplementedException();
        }

        public IQueryable<OnetAttribute> GetAllRawOnetSkillsForOccupation(string onetOccupationalCode)
        {
            throw new NotImplementedException();
        }

        public DigitalSkillsLevel GetDigitalSkillsLevel(int count)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<OnetAttribute> MoveBottomLevelAttributesUpOneLevel(IEnumerable<OnetAttribute> attributes)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<OnetAttribute> RemoveDFCSuppressions(IOrderedQueryable<OnetAttribute> attributes)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<OnetAttribute> RemoveDuplicateAttributes(IEnumerable<OnetAttribute> attributes)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<OnetAttribute> SelectFinalAttributes(IEnumerable<OnetAttribute> attributes)
        {
            throw new NotImplementedException();
        }
    }
}
