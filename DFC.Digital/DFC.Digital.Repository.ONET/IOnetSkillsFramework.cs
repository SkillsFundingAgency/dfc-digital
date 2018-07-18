using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
namespace DFC.Digital.Repository.ONET
{
    using System.Collections.Generic;

    public interface IOnetSkillsFramework  :IDisposable
    {
        Task<IEnumerable<T>> GetAllTranslationsAsync<T>() where T : DfcGdsOnetEntity;
        Task<IEnumerable<T>> GetAllSocMappingsAsync<T>() where T : DfcGdsOnetEntity;
        Task<IEnumerable<T>> GetAttributesValuesAsync<T>(string socCode);
        Task<T> GetDigitalSkillsRankAsync<T>(string socCode) where T : struct;
        Task<T> GetDigitalSkillsAsync<T>(string socCode) where T : DfcGdsOnetEntity;
      }
}