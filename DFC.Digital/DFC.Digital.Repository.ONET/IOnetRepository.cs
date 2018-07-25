using DFC.Digital.Data.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DFC.Digital.Repository.ONET
{
    //CodeReview: if it is a repo, then change this to IOnetRepo
    public interface IOnetRepository
    {
        Task<IEnumerable<T>> GetAllTranslationsAsync<T>() where T : OnetEntity;

        Task<IEnumerable<T>> GetAllSocMappingsAsync<T>() where T : OnetEntity;

        Task<IEnumerable<T>> GetAttributesValuesAsync<T>(string socCode);

        Task<T> GetDigitalSkillsRankAsync<T>(string socCode) where T : struct;

        Task<T> GetDigitalSkillsAsync<T>(string socCode) where T : OnetEntity;
    }
}