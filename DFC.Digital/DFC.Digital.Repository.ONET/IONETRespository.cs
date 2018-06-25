namespace DFC.Digital.Repository.ONET
{
    using System;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using Data.Interfaces;
    using Data.Model;
    public interface IOnetRespository<T> :IRepository<SkillsFramework> where T: SkillsFramework
    {
        Task<T> GetWitValuesForAsync(string socCode);
        Task<T> GetWitValuesWithConditionsAsync(Expression<Func<T>> predicate);
    }
}