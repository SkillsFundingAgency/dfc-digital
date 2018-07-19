namespace DFC.Digital.Service.SkillsFramework.Interface
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Data.Model;

    public interface IBusinessRuleEngine
    {
       Task<IEnumerable<DfcOnetSocMappings>> GetAllSocMappingsAsync();
        Task<IEnumerable<DfcOnetTranslation>> GetAllTranslationsAsync();
        Task<DfcOnetDigitalSkills> GetAllDigitalSkillsAsync(string onetSocCode);
        Task<int> GetDigitalSkillRankAsync(string onetSocCode);
        Task<IEnumerable<DfcOnetAttributesData>> GetBusinessRuleAttributesAsync(string onetSocCode);
    }
}