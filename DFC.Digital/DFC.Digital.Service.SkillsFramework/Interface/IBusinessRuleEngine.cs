namespace DFC.Digital.Service.SkillsFramework.Interface
{
    using Data.Model;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IBusinessRuleEngine
    {
        IEnumerable<SocCode> GetAllSocMappingsAsync();

        Task<IEnumerable<DfcOnetTranslation>> GetAllTranslationsAsync();

        Task<DfcOnetDigitalSkills> GetAllDigitalSkillsAsync(string onetSocCode);

        Task<int> GetDigitalSkillRankAsync(string onetSocCode);

        Task<IEnumerable<DfcOnetAttributesData>> GetBusinessRuleAttributesAsync(string onetSocCode);
    }
}