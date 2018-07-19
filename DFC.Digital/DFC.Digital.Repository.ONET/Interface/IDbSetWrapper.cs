using System.Data.Entity.Infrastructure;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace DFC.Digital.Repository.ONET.Interface
{
    public interface IDbSetWrapper<TEntity> : IQueryable<TEntity>, IEnumerable<TEntity>, IQueryable, IEnumerable, IDbAsyncEnumerable<TEntity>
        where TEntity : class
    {
        new IEnumerator<TEntity> GetEnumerator();
    }
}