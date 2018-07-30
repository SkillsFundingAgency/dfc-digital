using DFC.Digital.Core;
using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
//using DFC.Digital.Repository.ONET;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DFC.Digital.Service.SkillsFramework
{
    public class SkillsFrameworkService : ISkillsFrameworkService
    {
        private readonly IApplicationLogger logger;
        private readonly IRepository<SocCode> socRepository;
        private readonly IRepository<DigitalSkill> digitalSkillRepository;
        private readonly IRepository<FrameworkSkill> translationRepository;

        private readonly ISkillFrameworkBusinessRuleEngine skillsBusinessRuleEngine;
        private readonly IRelatedSkillsMappingRepository skillsMappingRepository;

        public SkillsFrameworkService(
            IApplicationLogger logger,
            IRepository<SocCode> socRepository,
            IRepository<DigitalSkill> digitalSkillRepository,
            IRepository<FrameworkSkill> translationRepository,
            ISkillFrameworkBusinessRuleEngine skillsBusinessRuleEngine)
            //IRepository<RelatedSkillMapping> skillsMappingRepository,
        {
            //this.repository = repository;
            this.logger = logger;
            this.socRepository = socRepository;
            this.digitalSkillRepository = digitalSkillRepository;
            this.skillsBusinessRuleEngine = skillsBusinessRuleEngine;
            this.translationRepository = translationRepository;
        }

        #region Implementation of IBusinessRuleEngine

        public IEnumerable<SocCode> GetAllSocMappings()
        {
            return socRepository.GetAll();
        }

        public IEnumerable<FrameworkSkill> GetAllTranslations()
        {
            return translationRepository.GetAll();
        }

        public DigitalSkillsLevel GetDigitalSkillLevel(string onetOccupationalCode)
        {
            var result = digitalSkillRepository.GetById(onetOccupationalCode);
            return result?.Level ?? default;
        }

        public IEnumerable<OnetAttribute> GetRelatedSkillMapping(string onetOccupationalCode)
        {

            //Get All raw attributes linked to occ code from the repository (Skill, knowledge, work styles, ablities)
           var attributes = skillsBusinessRuleEngine.GetAllRawOnetSkillsForOccupation(onetOccupationalCode);

           //attributes =  skillsBusinessRuleEngine.RemoveDFCSuppressions(attributes);


            //Average out the skill thats have LV and LM scales
            //attributes = skillsBusinessRuleEngine.AverageOutScoreScales(attributes);

            //attributes = skillsBusinessRuleEngine.MoveBottomLevelAttributesUpOneLevel(attributes);

            //attributes =  skillsBusinessRuleEngine.RemoveDuplicateAttributes(attributes);

            //attributes =  skillsBusinessRuleEngine.BoostMathsSkills(attributes);

            //attributes =  skillsBusinessRuleEngine.CombineSimilarAttributes(attributes);

            //attributes = skillsBusinessRuleEngine.SelectFinalAttributes(attributes);

            //attributes =  skillsBusinessRuleEngine.AddTitlesToAttributes(attributes);

            return skillsBusinessRuleEngine.AddTitlesToAttributes(attributes);
        }



        #endregion Implementation of IBusinessRuleEngine
    }
}