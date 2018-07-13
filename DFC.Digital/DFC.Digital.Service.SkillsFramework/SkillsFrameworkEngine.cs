using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DFC.Digital.Service.SkillsFramework.Interface;
using DFC.Digital.Data.Model;
using DFC.Digital.Repository.ONET.Interface;
using DFC.Digital.Core;

namespace DFC.Digital.Service.SkillsFramework
{
    using SkillsFramework = Data.Model.SkillsFramework;

    public class SkillsFrameworkEngine:IBusinessRuleEngine
    {
        private readonly IDfcGdsSkillsFramework _repository;
        private readonly IApplicationLogger _logger;

        // Business Rule Engine implemetation
        // Repository will be called with the Expression predicate (Rule Engine)
        public SkillsFrameworkEngine(IDfcGdsSkillsFramework repository, IApplicationLogger logger )
        {
            _repository = repository;
            _logger = logger;
        }

        #region Implementation of IBusinessRuleEngine
        [Obsolete]
        public Task<SkillsFramework> GetSkillsFrameworkForAsync(string socCode)
        {
            return null;
        }

        public async Task<IEnumerable<DfcGdsSocMappings>> GetAllSocMappingsAsync()
        {
            try
            {
                return await _repository.GetAllSocMappingsAsync<DfcGdsSocMappings>();
            }
            catch (Exception e)
            {
               _logger.Error($"GetAllSocMappingsAsync :{e.Message}",e);
                throw;
            }
        }

        public  async Task<IEnumerable<DfcGdsTranslation>> GetAllTranslationsAsync()
        {
            try
            { 
            return await  _repository.GetAllTranslationsAsync<DfcGdsTranslation>();
            }
            catch(Exception e)
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
            catch(Exception e)
            {
                _logger.Error($"GetAllDigitalSkillsAsync :{e.Message}", e);
                throw;
            }
        }

        public async  Task<int> GetDigitalSkillRankAsync(string onetSocCode)
        {
            try
            {
                return await _repository.GetDigitalSkillsRankAsync<int>(onetSocCode);
            }
            catch(Exception e)
            {
                _logger.Error($"GetDigitalSkillRankAsync :{e.Message}", e);
                throw;
            }
        }

        public async Task<IEnumerable<DfcGdsAttributesData>> GetBusinessRuleAttributesAsync(string onetSocCode)
        {
            try
            {
                return await _repository.GetAttributesValuesAsync<DfcGdsAttributesData>(onetSocCode);
            }
            catch(Exception e)
            {
                _logger.Error($"GetBusinessRuleAttributesAsync :{e.Message}", e);
                throw;
            }
        }

        #endregion
    }
}
