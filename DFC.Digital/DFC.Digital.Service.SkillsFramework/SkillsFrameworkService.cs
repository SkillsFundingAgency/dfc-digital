﻿using DFC.Digital.Core;
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
        private readonly IQueryRepository<FrameWorkSkill> translationRepository;

        private readonly ISkillFrameworkBusinessRuleEngine skillsBusinessRuleEngine;
   
        public SkillsFrameworkService(
            IApplicationLogger logger,
            IQueryRepository<SocCode> socRepository,
            IQueryRepository<DigitalSkill> digitalSkillRepository,
            IQueryRepository<FrameWorkSkill> translationRepository,
            ISkillFrameworkBusinessRuleEngine skillsBusinessRuleEngine)
        {
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

        public IEnumerable<FrameWorkSkill> GetAllTranslations()
        {
            return translationRepository.GetAll();
        }

        public DigitalSkillsLevel GetDigitalSkillLevel(string onetOccupationalCode)
        {
            var digitalSkill = digitalSkillRepository.GetById(onetOccupationalCode);
            var rank= skillsBusinessRuleEngine.GetDigitalSkillsLevel(digitalSkill.ApplicationCount);
            return rank;
        }

        public IEnumerable<OnetAttribute> GetRelatedSkillMapping(string onetOccupationalCode)
        {

            //Get All raw attributes linked to occ code from the repository (Skill, knowledge, work styles, ablities)
            var rawAttributes = skillsBusinessRuleEngine.GetAllRawOnetSkillsForOccupation(onetOccupationalCode).ToList(); 

           
            //Average out the skill thats have LV and LM scales
            var attributes = skillsBusinessRuleEngine.AverageOutScoreScales(rawAttributes);

            attributes = skillsBusinessRuleEngine.MoveBottomLevelAttributesUpOneLevel(attributes);

            attributes =  skillsBusinessRuleEngine.RemoveDuplicateAttributes(attributes);

            attributes = skillsBusinessRuleEngine.RemoveDFCSuppressions(attributes);

            attributes = skillsBusinessRuleEngine.AddTitlesToAttributes(attributes);

            attributes =  skillsBusinessRuleEngine.BoostMathsSkills(attributes);

            attributes =  skillsBusinessRuleEngine.CombineSimilarAttributes(attributes);

            attributes =  skillsBusinessRuleEngine.SelectFinalAttributes(attributes).ToList();

            return attributes;
      
        }

        #endregion Implementation of IBusinessRuleEngine
    }
}