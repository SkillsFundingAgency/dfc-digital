﻿using DFC.Digital.Core;
using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
//CodeReview; Please remove if not needed
//using DFC.Digital.Repository.ONET;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DFC.Digital.Service.SkillsFramework
{
    public class SkillsFrameworkService : ISkillsFrameworkService
    {
        private readonly IApplicationLogger logger;
        private readonly IQueryRepository<DigitalSkill> digitalSkillRepository;
        private readonly IQueryRepository<FrameworkSkill> translationRepository;
        private readonly ISocMappingRepository socMappingRepository;
        private readonly ISkillFrameworkBusinessRuleEngine skillsBusinessRuleEngine;
   
        public SkillsFrameworkService(
            IApplicationLogger logger,
            IQueryRepository<DigitalSkill> digitalSkillRepository,
            IQueryRepository<FrameworkSkill> translationRepository,
            ISkillFrameworkBusinessRuleEngine skillsBusinessRuleEngine,
            ISocMappingRepository socMappingRepository)
        {
            this.logger = logger;
            this.digitalSkillRepository = digitalSkillRepository;
            this.skillsBusinessRuleEngine = skillsBusinessRuleEngine;
            this.translationRepository = translationRepository;
            this.socMappingRepository = socMappingRepository;
        }

        #region Implementation of ISkillsFrameworkService

        public IEnumerable<SocCode> GetAllSocMappings()
        {
            return socMappingRepository.GetAll();
        }

        public IEnumerable<SocCode> GetNextBatchSocMappingsForUpdate(int batchSize)
        {
            return socMappingRepository.GetSocsAwaitingUpdate().Take(batchSize);
        }

        public IEnumerable<FrameworkSkill> GetAllTranslations()
        {
            return translationRepository.GetAll();
        }

        public DigitalSkillsLevel GetDigitalSkillLevel(string onetOccupationalCode)
        {
            var digitalSkill = digitalSkillRepository.GetById(onetOccupationalCode);
            if (digitalSkill != null)
            {
                var rank = skillsBusinessRuleEngine.GetDigitalSkillsLevel(digitalSkill.ApplicationCount);
                return rank;
            }
            return default;
        }

        public IEnumerable<OnetSkill> GetRelatedSkillMapping(string onetOccupationalCode)
        {
            //Get All raw attributes linked to occ code from the repository (Skill, knowledge, work styles, ablities)
            var rawAttributes = skillsBusinessRuleEngine.GetAllRawOnetSkillsForOccupation(onetOccupationalCode).ToList();

            logger.Trace($"Got {rawAttributes.Count()} raw attributes for occupational code {onetOccupationalCode}");

            //Average out the skill thats have LV and LM scales
            var attributes = skillsBusinessRuleEngine.AverageOutscoreScales(rawAttributes);
            attributes = skillsBusinessRuleEngine.MoveBottomLevelAttributesUpOneLevel(attributes);
            attributes =  skillsBusinessRuleEngine.RemoveDuplicateAttributes(attributes);
            attributes = skillsBusinessRuleEngine.RemoveDFCSuppressions(attributes);
            attributes = skillsBusinessRuleEngine.AddTitlesToAttributes(attributes);
            attributes =  skillsBusinessRuleEngine.BoostMathsSkills(attributes);
            attributes =  skillsBusinessRuleEngine.CombineSimilarAttributes(attributes.ToList());
            attributes =  skillsBusinessRuleEngine.SelectFinalAttributes(attributes);
            logger.Trace($"Returning {attributes.Count()} attributes for occupational code {onetOccupationalCode}");

            return attributes;      
        }

        public void ResetAllSocStatus()
        {
            var allSocCodes = GetAllSocMappings();
            socMappingRepository.SetUpdateStatusForSocs(allSocCodes, SkillsFrameworkUpdateStatus.AwaitingUpdate);
        }

        public void ResetStartedSocStatus()
        {
            var socInStartedStateCodes = socMappingRepository.GetSocsInStartedState();
            socMappingRepository.SetUpdateStatusForSocs(socInStartedStateCodes, SkillsFrameworkUpdateStatus.AwaitingUpdate);
        }

        public void SetSocStatusCompleted(SocCode socCode)
        {
            socMappingRepository.SetUpdateStatusForSocs(new List<SocCode> { socCode }, SkillsFrameworkUpdateStatus.UpdateCompleted);
        }
               
        public void SetSocStatusSelectedForUpdate (SocCode socCode)
        {
            socMappingRepository.SetUpdateStatusForSocs(new List<SocCode> { socCode }, SkillsFrameworkUpdateStatus.SelectedForUpdate);
        }

        public SocMappingStatus GetSocMappingStatus()
        {
            return socMappingRepository.GetSocMappingStatus();
        }

        #endregion Implementation of ISkillsFrameworkService
    }
}