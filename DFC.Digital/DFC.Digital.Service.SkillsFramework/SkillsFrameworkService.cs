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
        private readonly ISkillFrameworkBusinessRuleEngine skillsBusinessRuleEngine;
        private readonly IRelatedSkillsMappingRepository skillsMappingRepository;

        //private readonly IRepository<RelatedSkillMapping> skillsMappingRepository;
        private readonly IRepository<WhatItTakesSkill> skillsRepository;

        public SkillsFrameworkService(
            IApplicationLogger logger,
            IRepository<SocCode> socRepository,
            IRepository<DigitalSkill> digitalSkillRepository,
            //ISkillFrameworkBusinessRuleEngine skillsBusinessRuleEngine,
            //IRepository<RelatedSkillMapping> skillsMappingRepository,
            IRelatedSkillsMappingRepository skillsMappingRepository,
            IRepository<WhatItTakesSkill> skillsRepository)
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
            return skillsMappingRepository.GetByONetOccupationalCode(onetOccupationalCode);
        }

        //public DigitalSkill GetDigitalSkills(string onetSocCode)
        //{
        //    return digitalSkillRepository.GetById(onetSocCode);
        //}

        //CodeReview: Who needs this?
        //public async Task<int> GetDigitalSkillRankAsync(string onetSocCode)
        //{
        //    var returnDigitalRank = 0;
        //    try
        //    {
        //        var rankResult = await repository.GetDigitalSkillsRankAsync<int>(onetSocCode);
        //        if (rankResult > Convert.ToInt32(RangeChecker.FirstRange))
        //            returnDigitalRank = 1;
        //        else if ((rankResult > Convert.ToInt32(RangeChecker.SecondRange)) &&
        //            (rankResult < Convert.ToInt32(RangeChecker.FirstRange)))
        //            returnDigitalRank = 2;
        //        else if ((rankResult > Convert.ToInt32(RangeChecker.ThirdRange)) &&
        //            (rankResult < Convert.ToInt32(RangeChecker.SecondRange)))
        //            returnDigitalRank = 3;
        //        else if ((rankResult > Convert.ToInt32(RangeChecker.FourthRange)) &&
        //            (rankResult < Convert.ToInt32(RangeChecker.SecondRange)))
        //            returnDigitalRank = 4;
        //    }
        //    catch (Exception e)
        //    {
        //        logger.ErrorJustLogIt($"GetDigitalSkillRankAsync :{e.Message}", e);
        //        return 0;
        //    }
        //    return returnDigitalRank;
        //}

        //public async Task<IEnumerable<DfcOnetAttributesData>> GetBusinessRuleAttributesAsync(string onetSocCode)
        //{
        //    try
        //    {
        //        return await repository.GetAttributesValuesAsync<DfcOnetAttributesData>(onetSocCode);
        //    }
        //    catch (Exception e)
        //    {
        //        logger.ErrorJustLogIt($"GetBusinessRuleAttributesAsync :{e.Message}", e);
        //        return null;
        //    }
        //}

        #endregion Implementation of IBusinessRuleEngine
    }
}