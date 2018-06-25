namespace DFC.Digital.Repository.ONET.Interface
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Interface for a dbset wrapper.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    public interface IDbSetWrapper<TEntity> : IDbSet<TEntity>, IDbAsyncEnumerable<TEntity>
        where TEntity : class
    {
        #region Public Methods and Operators

        /// <summary>
        /// Includes the specified expression.
        /// </summary>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="path">The include expression.</param>
        /// <returns>
        ///     <see cref="IQueryable{TEntity}" />
        /// </returns>
        IQueryable<TEntity> Include<TProperty> ( Expression<Func<TEntity , TProperty>> path );

        /// <summary>
        /// Finds items by key values asynchronously
        /// </summary>
        /// <param name="keyValues">The key values</param>
        /// <returns></returns>
        Task<TEntity> FindAsync ( params Object [ ] keyValues );

        /// <summary>
        /// Finds items by key values asynchronously
        /// </summary>
        /// <param name="cancellationToken">Allows the operation to be cancelled</param>
        /// <param name="keyValues">The key values</param>
        /// <returns></returns>
        Task<TEntity> FindAsync ( CancellationToken cancellationToken , params object [ ] keyValues );

        #endregion
    }
}