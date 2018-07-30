using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using System.Text;
using System.Threading.Tasks;

namespace DFC.Digital.Service.SkillsFramework
{
    public class SkillFrameworkBusinessRuleEngine : ISkillFrameworkBusinessRuleEngine
    {
        private readonly IMapper autoMapper;
        private readonly ISkillsRepository knowledgeRepository;
        private readonly ISkillsRepository abilitiesOueryRepository;
        private readonly ISkillsRepository skillsOueryRepository;
        private readonly ISkillsRepository workStyleRepository;


        public SkillFrameworkBusinessRuleEngine(IMapper autoMapper, ISkillsRepository knowledgeRepository, ISkillsRepository skillsOueryRepository, ISkillsRepository abilitiesOueryRepository,
          ISkillsRepository workStyleRepository)
        {
            this.autoMapper = autoMapper;
            this.knowledgeRepository = knowledgeRepository;
        }


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

            var allSkillForOccupation = knowledgeRepository.GetSkillsForONetOccupationCode(onetOccupationalCode);
            //.Union(businessRuleEngine.GetSkillsForOccupation(onetOccupationalCode))
            //.Union(businessRuleEngine.GetAbilitiesForOccupatio(onetOccupationalCode))
            //.Union(businessRuleEngine.GetWorkStylesForOccupation(onetOccupationalCode))
            //.OrderByDescending(r => r.Score);

            return allSkillForOccupation;
        }

        public DigitalSkillsLevel GetDigitalSkillsLevel(int count)
        {
                return count > 150 ? DigitalSkillsLevel.Level1
                     : count > 100 ? DigitalSkillsLevel.Level2
                     : count > 50 ? DigitalSkillsLevel.Level3
                     : DigitalSkillsLevel.Level4;
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
