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
        private readonly IOnetSkillsFramework _repository;
        private readonly IApplicationLogger _logger;

        // Business Rule Engine implemetation
        // Repository will be called with the Expression predicate (Rule Engine)
        public SkillsFrameworkEngine(IOnetSkillsFramework repository, IApplicationLogger logger)
        {
            _repository = repository;
            _logger = logger;
        }

        #region Implementation of IBusinessRuleEngine

        public async Task<IEnumerable<DfcGdsSocMappings>> GetAllSocMappingsAsync()
        {
            try
            {
                return await _repository.GetAllSocMappingsAsync<DfcGdsSocMappings>();
            }
            catch (Exception e)
            {
                _logger.ErrorJustLogIt($"GetAllSocMappingsAsync :{e.Message}", e);
                throw;
            }
        }

        public async Task<IEnumerable<DfcGdsTranslation>> GetAllTranslationsAsync()
        {
            try
            {
                return await _repository.GetAllTranslationsAsync<DfcGdsTranslation>();
            }
            catch (Exception e)
            {
                _logger.Error($"GetAllTranslationsAsync :{e.Message}", e);
                throw;
            }
        }

        public async Task<DfcGdsDigitalSkills> GetAllDigitalSkillsAsync(string onetSocCode)
        {
            try
            {
                return await _repository.GetDigitalSkillsAsync<DfcGdsDigitalSkills>(onetSocCode);
            }
            catch (Exception e)
            {
                _logger.Error($"GetAllDigitalSkillsAsync :{e.Message}", e);
                throw;
            }
        }

        public async Task<int> GetDigitalSkillRankAsync(string onetSocCode)
        {
            var returnDigitalRank = 0;
            try
            {
                var rankResult = await _repository.GetDigitalSkillsRankAsync<int>(onetSocCode);
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
                _logger.Error($"GetDigitalSkillRankAsync :{e.Message}", e);
                throw;
            }
            return returnDigitalRank;
        }

        public async Task<IEnumerable<DfcGdsAttributesData>> GetBusinessRuleAttributesAsync(string onetSocCode)
        {
            try
            {
                return await _repository.GetAttributesValuesAsync<DfcGdsAttributesData>(onetSocCode);
            }
            catch (Exception e)
            {
                _logger.Error($"GetBusinessRuleAttributesAsync :{e.Message}", e);
                throw;
            }
        }

        #endregion Implementation of IBusinessRuleEngine
    }
}