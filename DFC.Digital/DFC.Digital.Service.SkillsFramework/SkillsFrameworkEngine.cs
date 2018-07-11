namespace DFC.Digital.Service.SkillsFramework
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Data.Interfaces;
    using Interface;
    using DFC.Digital.Data.Model;
    using Repository.ONET.Interface;


    public class SkillsFrameworkEngine:IBusinessRuleEngine 
    {
        private readonly IDfcGdsSkillsFramework _repository;

        // Business Rule Engine implemetation 
        // Repository will be called with the Expression predicate (Rule Engine)
        public SkillsFrameworkEngine(IDfcGdsSkillsFramework repository )
        {
            _repository = repository;
        }

        #region Implementation of IBusinessRuleEngine

        public async Task<SkillsFramework> GetSkillsFrameworkFor(string socCode)
        {
           var result= _repository.GetAttributesValuesAsync<DfcGdsTranslation>(s=>s.SocCode==socCode).Result.
                Select(skills => new SkillsFramework {SocCode = skills.SocCode}).
                FirstOrDefault();
            return await Task.FromResult ( result).ConfigureAwait ( false );
        }

        #endregion

        #region Implementation of IBusinessRuleEngine

        public Task<IEnumerable<DfcGdsSocMappings>> GetAllSocMappings()
        {
            return _repository.GetAllSocMappingsAsync<DfcGdsSocMappings>();
        }

        public Task<IEnumerable<DfcGdsTranslation>> GetAllTranslations()
        {
            return _repository.GetAllTranslationsAsync<DfcGdsTranslation>();
        }

        public Task<DfcGdsDigitalSkills> GetAllDigitalSkills(string onetSocCode)
        {
            return _repository.GetDigitalSkillsAsync<DfcGdsDigitalSkills>(onetSocCode);
        }

        public Task<int> GetDigitalSkillRank(string onetSocCode)
        {
           return _repository.GetDigitalSkillsRankAsync<int>(onetSocCode);
        }

        public Task<IEnumerable<DfcGdsAttributesData>> GetBusinessRuleAttributes(string onetSocCode)
        {
            return _repository.GetAttributesValuesAsync<DfcGdsAttributesData>(onetSocCode);
        }

        #endregion
    }
}
