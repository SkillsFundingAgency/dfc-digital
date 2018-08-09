using System;
using System.Linq;
using System.Linq.Expressions;

namespace DFC.Digital.Data.Interfaces
{
    public interface ICommandRepository<T>
        where T : class
    {
        // Marks an entity as new
        void Add(T entity);

        // Marks an entity as modified
        void Update(T entity);

        // Marks an entity to be removed
        void Delete(T entity);

        void Delete(Expression<Func<T, bool>> where);
    }
}