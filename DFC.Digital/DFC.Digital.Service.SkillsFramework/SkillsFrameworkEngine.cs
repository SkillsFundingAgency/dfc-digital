using DFC.Digital.Core;
using DFC.Digital.Data.Model;
using DFC.Digital.Repository.ONET;
using DFC.Digital.Service.SkillsFramework.Interface;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DFC.Digital.Service.SkillsFramework
{
    public class SkillsFrameworkEngine : IBusinessRuleEngine
    {
        //CodeReview: Remove the leading underscore, we are not using them, ensure code analysis and style cop are run for this project.
        private readonly IOnetRepository repository;
        private readonly IApplicationLogger logger;

        // Business Rule Engine implemetation
        // Repository will be called with the Expression predicate (Rule Engine)
        public SkillsFrameworkEngine(IOnetRepository repository, IApplicationLogger logger)
        {
            this.repository = repository;
            this.logger = logger;
        }

        #region Implementation of IBusinessRuleEngine

        public async Task<IEnumerable<DfcOnetSocMappings>> GetAllSocMappingsAsync()
        {
            try
            {
                return await repository.GetAllSocMappingsAsync<DfcOnetSocMappings>();
            }
            catch (Exception e)
            {
                logger.ErrorJustLogIt($"GetAllSocMappingsAsync :{e.Message}", e);
                throw;
            }
        }

        public async Task<IEnumerable<DfcOnetTranslation>> GetAllTranslationsAsync()
        {
            try
            {
                return await repository.GetAllTranslationsAsync<DfcOnetTranslation>();
            }
            catch (Exception e)
            {
                logger.Error($"GetAllTranslationsAsync :{e.Message}", e);
                throw;
            }
        }

        public async Task<DfcOnetDigitalSkills> GetAllDigitalSkillsAsync(string onetSocCode)
        {
            try
            {
                return await repository.GetDigitalSkillsAsync<DfcOnetDigitalSkills>(onetSocCode);
            }
            catch (Exception e)
            {
                logger.Error($"GetAllDigitalSkillsAsync :{e.Message}", e);
                throw;
            }
        }

        public async Task<int> GetDigitalSkillRankAsync(string onetSocCode)
        {
            var returnDigitalRank = 0;
            try
            {
                var rankResult = await repository.GetDigitalSkillsRankAsync<int>(onetSocCode);
                if (rankResult > Convert.ToInt32(RangeChecker.FirstRange))
                    returnDigitalRank = 1;
                else if ((rankResult > Convert.ToInt32(RangeChecker.SecondRange)) &&
                    (rankResult < Convert.ToInt32(RangeChecker.FirstRange)))
                    returnDigitalRank = 2;
                else if ((rankResult > Convert.ToInt32(RangeChecker.ThirdRange)) &&
                    (rankResult < Convert.ToInt32(RangeChecker.SecondRange)))
                    returnDigitalRank = 3;
                else if ((rankResult > Convert.ToInt32(RangeChecker.FourthRange)) &&
                    (rankResult < Convert.ToInt32(RangeChecker.SecondRange)))
                    returnDigitalRank = 4;
            }
            catch (Exception e)
            {
                logger.Error($"GetDigitalSkillRankAsync :{e.Message}", e);
                throw;
            }
            return returnDigitalRank;
        }

        public async Task<IEnumerable<DfcOnetAttributesData>> GetBusinessRuleAttributesAsync(string onetSocCode)
        {
            try
            {
                return await repository.GetAttributesValuesAsync<DfcOnetAttributesData>(onetSocCode);
            }
            catch (Exception e)
            {
                logger.Error($"GetBusinessRuleAttributesAsync :{e.Message}", e);
                throw;
            }
        }

        #endregion Implementation of IBusinessRuleEngine
    }
}