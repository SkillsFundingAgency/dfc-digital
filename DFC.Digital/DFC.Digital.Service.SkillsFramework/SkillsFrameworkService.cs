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
        private readonly IQueryRepository<SocCode> socRepository;

        private readonly IQueryRepository<DigitalSkill> digitalSkillRepository;
        private readonly ISkillFrameworkBusinessRuleEngine skillsBusinessRuleEngine;
        private readonly IRelatedSkillsMappingRepository skillsMappingRepository;

        //private readonly IRepository<RelatedSkillMapping> skillsMappingRepository;
        private readonly IQueryRepository<FrameworkSkill> skillsRepository;

        public SkillsFrameworkService(
            IApplicationLogger logger,
            IQueryRepository<SocCode> socRepository,
            IQueryRepository<DigitalSkill> digitalSkillRepository,
            ISkillFrameworkBusinessRuleEngine skillsBusinessRuleEngine,
            //IRepository<RelatedSkillMapping> skillsMappingRepository,
            IRelatedSkillsMappingRepository skillsMappingRepository,
            IQueryRepository<FrameworkSkill> skillsRepository)
        {
            //this.repository = repository;
            this.logger = logger;
            this.socRepository = socRepository;
            this.digitalSkillRepository = digitalSkillRepository;
            //this.skillsBusinessRuleEngine = skillsBusinessRuleEngine;
            this.skillsMappingRepository = skillsMappingRepository;
            this.skillsRepository = skillsRepository;
        }

        #region Implementation of IBusinessRuleEngine

        public IEnumerable<SocCode> GetAllSocMappings()
        {
            return socRepository.GetAll();
        }

        public IEnumerable<FrameworkSkill> GetAllTranslations()
        {
            return skillsRepository.GetAll();
        }

        public DigitalSkillsLevel GetDigitalSkillLevel(string onetOccupationalCode)
        {
            var result = digitalSkillRepository.GetById(onetOccupationalCode);
            return result?.Level ?? default;
        }

        public IEnumerable<RelatedSkillMapping> GetRelatedSkillMapping(string onetOccupationalCode)
        {

           // //Get All raw attributes linked to occ code from the repository (Skill, knowledge, work styles, ablities)
           //var attributes = skillsBusinessRuleEngine.GetAllRawOnetSkillsForOccupation(onetOccupationalCode);

           // attributes =  skillsBusinessRuleEngine.RemoveDFCSuppressions(attributes.OrderByDescending(x => x.));


           // //Average out the skill thats have LV and LM scales
           // attributes = skillsBusinessRuleEngine.AverageOutScoreScales(attributes);

           // attributes = skillsBusinessRuleEngine.MoveBottomLevelAttributesUpOneLevel(attributes);

           // attributes =  skillsBusinessRuleEngine.RemoveDuplicateAttributes(attributes);

           // attributes =  skillsBusinessRuleEngine.BoostMathsSkills(attributes);

           // attributes =  skillsBusinessRuleEngine.CombineSimilarAttributes(attributes);

           // attributes = skillsBusinessRuleEngine.SelectFinalAttributes(attributes);

           // attributes =  skillsBusinessRuleEngine.AddTitlesToAttributes(attributes);

            return default;
        }



        #endregion Implementation of IBusinessRuleEngine
    }
}