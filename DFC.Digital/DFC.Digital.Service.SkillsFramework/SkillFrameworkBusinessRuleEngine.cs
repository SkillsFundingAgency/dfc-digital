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
        private readonly ISkillsRepository knowledgeQueryRepository;
        private readonly ISkillsRepository abilitiesOueryRepository;
        private readonly ISkillsRepository skillsOueryRepository;
        private readonly ISkillsRepository workStyleRepository;

        private readonly IQueryRepository<FrameworkSkill> suppressionsQueryRepository;
        private readonly IQueryRepository<FrameWorkSkillCombination> combinationsQueryRepository;

        public SkillFrameworkBusinessRuleEngine(IMapper autoMapper, ISkillsRepository knowledgeQueryRepository, ISkillsRepository skillsOueryRepository, ISkillsRepository abilitiesOueryRepository,
          ISkillsRepository workStyleRepository, IQueryRepository<FrameworkSkill> suppressionsQueryRepository, IQueryRepository<FrameWorkSkillCombination> combinationsQueryRepository)
        {
            this.autoMapper = autoMapper;
            this.knowledgeQueryRepository = knowledgeQueryRepository;
            this.abilitiesOueryRepository = abilitiesOueryRepository;
            this.skillsOueryRepository = skillsOueryRepository;
            this.workStyleRepository = workStyleRepository;
            this.combinationsQueryRepository = combinationsQueryRepository;            
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

            var allSkillForOccupation = knowledgeQueryRepository.GetSkillsForONetOccupationCode(onetOccupationalCode)
                .Union(abilitiesOueryRepository.GetSkillsForONetOccupationCode(onetOccupationalCode));
            //.Union(businessRuleEngine.GetAbilitiesForOccupatio(onetOccupationalCode))
            //.Union(businessRuleEngine.GetWorkStylesForOccupation(onetOccupationalCode))
            //.OrderByDescending(r => r.Score);

            return allSkillForOccupation;
        }

        public DigitalSkillsLevel GetDigitalSkillsLevel(int count)
        {
                var rankValue= count > 150 ? DigitalSkillsLevel.Level1
                     : count > 100 ? DigitalSkillsLevel.Level2
                     : count > 50 ? DigitalSkillsLevel.Level3
                     : DigitalSkillsLevel.Level4;
            return rankValue;
        }

        public IEnumerable<OnetAttribute> MoveBottomLevelAttributesUpOneLevel(IEnumerable<OnetAttribute> attributes)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<OnetAttribute> RemoveDFCSuppressions(IEnumerable<OnetAttribute> attributes)
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
