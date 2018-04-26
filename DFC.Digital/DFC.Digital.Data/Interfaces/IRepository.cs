using System;
using System.Linq;
using System.Linq.Expressions;

namespace DFC.Digital.Data.Interfaces
{
    public interface IRepository<T>
        where T : class
    {
        // Marks an entity as new
        void Add(T entity);

        // Marks an entity as modified
        void Update(T entity);

        // Marks an entity to be removed
        void Delete(T entity);

        void Delete(Expression<Func<T, bool>> where);

        // Get an entity by int id
        T GetById(string id);

        // Get an entity using delegate
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", MessageId = "Get", Justification = "Default repository pattern, changing it doesnt make sense.")]
        T Get(Expression<Func<T, bool>> where);

        // Gets all entities of type T
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification = "Default repository pattern, changing it doesnt make sense.")]
        IQueryable<T> GetAll();

        // Gets entities using delegate
        IQueryable<T> GetMany(Expression<Func<T, bool>> where);
    }
}