using DFC.Digital.Core;
using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using DFC.Digital.Repository.ONET;
using DFC.Digital.Service.SkillsFramework.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DFC.Digital.Service.SkillsFramework
{
    public class SkillsFrameworkEngine : IBusinessRuleEngine
    {
        //CodeReview: Remove the leading underscore, we are not using them, ensure code analysis and style cop are run for this project.
        private readonly IOnetRepository repository;
        private readonly IApplicationLogger logger;
        private readonly IRepository<SocCode> socRepository;
        private readonly IRepository<DigitalSkill> digitalSkillRepository;
        private readonly ISkillFrameworkBusinessRuleEngine skillsBusinessRuleEngine;
        private readonly IRepository<WhatItTakesSkill> skillsRepository;

        // Business Rule Engine implemetation
        // Repository will be called with the Expression predicate (Rule Engine)
        public SkillsFrameworkEngine(
            IOnetRepository repository, 
            IApplicationLogger logger, 
            IRepository<SocCode> socRepository, 
            IRepository<DigitalSkill> digitalSkillRepository,
            ISkillFrameworkBusinessRuleEngine skillsBusinessRuleEngine,
            IRepository<WhatItTakesSkill> skillsRepository)
        {
            this.repository = repository;
            this.logger = logger;
            this.socRepository = socRepository;
            this.digitalSkillRepository = digitalSkillRepository;
            this.skillsBusinessRuleEngine = skillsBusinessRuleEngine;
            this.skillsRepository = skillsRepository;
        }

        #region Implementation of IBusinessRuleEngine

        public IEnumerable<SocCode> GetAllSocMappings()
        {
            return socRepository.GetAll().ToList();
        }

        public IEnumerable<WhatItTakesSkill> GetAllTranslations()
        {
            return skillsRepository.GetAll().ToList();
        }

        public DigitalSkill GetDigitalSkills(string onetSocCode)
        {
            return digitalSkillRepository.GetById(onetSocCode);
        }

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