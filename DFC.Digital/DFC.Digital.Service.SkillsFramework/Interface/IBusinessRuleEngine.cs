namespace DFC.Digital.Service.SkillsFramework.Interface
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Data.Model;

    public interface IBusinessRuleEngine
    {
        Task<IEnumerable<DfcGdsSocMappings>> GetAllSocMappings();
        Task<IEnumerable<DfcGdsTranslation>> GetAllTranslations();
        Task<DfcGdsDigitalSkills> GetAllDigitalSkills(string onetSocCode);
        Task<int> GetDigitalSkillRank(string onetSocCode);
        Task<IEnumerable<DfcGdsAttributesData>> GetBusinessRuleAttributes(string onetSocCode);
    }
}