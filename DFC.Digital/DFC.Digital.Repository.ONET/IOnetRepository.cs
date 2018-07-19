using System;
using System.Threading.Tasks;
using DFC.Digital.Data.Model;
using System.Collections.Generic;
namespace DFC.Digital.Repository.ONET
{
    //CodeReview: if it is a repo, then change this to IOnetRepo
    public interface IOnetRepository  :IDisposable
    {
        Task<IEnumerable<T>> GetAllTranslationsAsync<T>() where T : DfcGdsOnetEntity;
        Task<IEnumerable<T>> GetAllSocMappingsAsync<T>() where T : DfcGdsOnetEntity;
        Task<IEnumerable<T>> GetAttributesValuesAsync<T>(string socCode);
        Task<T> GetDigitalSkillsRankAsync<T>(string socCode) where T : struct;
        Task<T> GetDigitalSkillsAsync<T>(string socCode) where T : DfcGdsOnetEntity;
      }
}