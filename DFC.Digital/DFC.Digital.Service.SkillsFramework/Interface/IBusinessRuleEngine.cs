namespace DFC.Digital.Service.SkillsFramework.Interface
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Data.Model;

    public interface IBusinessRuleEngine
    {
        [Obsolete]
        Task<SkillsFramework> GetSkillsFrameworkForAsync(string socCode);

        Task<IEnumerable<DfcGdsSocMappings>> GetAllSocMappingsAsync();
        Task<IEnumerable<DfcGdsTranslation>> GetAllTranslationsAsync();
        Task<DfcGdsDigitalSkills> GetAllDigitalSkillsAsync(string onetSocCode);
        Task<int> GetDigitalSkillRankAsync(string onetSocCode);
        Task<IEnumerable<DfcGdsAttributesData>> GetBusinessRuleAttributesAsync(string onetSocCode);
    }
}