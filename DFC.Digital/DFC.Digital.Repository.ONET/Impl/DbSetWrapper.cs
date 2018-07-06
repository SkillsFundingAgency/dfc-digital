using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DFC.Digital.Repository.ONET.Impl
{
    using System.Collections;
    using System.Collections.ObjectModel;
    using System.Data.Entity;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq.Expressions;
    using System.Threading;
    using Interface;
    /// <summary>
    /// Wrapper class for a <see cref="IDbSet{TEntity}" /> to aid with unit testing.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    public class DbSetWrapper<TEntity> : IDbSetWrapper<TEntity>
        where TEntity : class
    {
        #region Fields

        /// <summary>
        /// The internal set reference
        /// </summary>
        private readonly IDbSet<TEntity> _set;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DbSetWrapper{TEntity}" /> class.
        /// </summary>
        /// <param name="set">The set.</param>
        public DbSetWrapper ( IDbSet<TEntity> set )
        {
            this._set = set;

            this.Expression = set.Expression;
            this.ElementType = set.ElementType;
            this.Provider = set.Provider;
            this.Local = set.Local;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the type of the element(s) that are returned when the expression tree associated with this instance of
        /// <see cref="T:System.Linq.IQueryable" /> is executed.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Type" /> that represents the type of the element(s) that are returned when the
        /// expression tree associated with this object is executed.
        /// </returns>
        public Type ElementType { get; private set; }

        /// <summary>
        /// Gets the expression tree that is associated with the instance of <see cref="T:System.Linq.IQueryable" />.
        /// </summary>
        /// <returns>
        /// The <see cref="T:System.Linq.Expressions.Expression" /> that is associated with this instance of
        /// <see cref="T:System.Linq.IQueryable" />.
        /// </returns>
        public Expression Expression { get; private set; }

        /// <summary>
        /// Gets the local collection.
        /// </summary>
        /// <value>
        /// The local collection.
        /// </value>
        public ObservableCollection<TEntity> Local { get; private set; }

        /// <summary>
        /// Gets the query provider that is associated with this data source.
        /// </summary>
        /// <returns>The <see cref="T:System.Linq.IQueryProvider" /> that is associated with this data source.</returns>
        public IQueryProvider Provider { get; private set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Adds the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public TEntity Add ( TEntity entity )
        {
            return this._set.Add ( entity );
        }

        /// <summary>
        /// Attaches the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public TEntity Attach ( TEntity entity )
        {
            return this._set.Attach ( entity );
        }

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <returns></returns>
        public TEntity Create ( )
        {
            return this._set.Create ( );
        }

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <typeparam name="TDerivedEntity">The type of the derived entity.</typeparam>
        /// <returns></returns>
        public TDerivedEntity Create<TDerivedEntity> ( ) where TDerivedEntity : class, TEntity
        {
            return this._set.Create<TDerivedEntity> ( );
        }

        /// <summary>
        /// Finds the specified key values.
        /// </summary>
        /// <param name="keyValues">The key values.</param>
        /// <returns></returns>
        public TEntity Find ( params object [ ] keyValues )
        {
            return this._set.Find ( keyValues );
        }


        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"></see> that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<TEntity> GetEnumerator ( )
        {
            return ( ( IEnumerable<TEntity> )this._set ).GetEnumerator ( );
        }

        /// <summary>
        /// Includes the specified expression.
        /// </summary>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="path">The include expression.</param>
        /// <returns>
        ///     <see cref="IQueryable{TEntity}" />
        /// </returns>
        [ExcludeFromCodeCoverage]
        public IQueryable<TEntity> Include<TProperty> ( Expression<Func<TEntity , TProperty>> path )
        {
            return this._set.Include ( path );
        }

        /// <summary>
        /// Removes the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public TEntity Remove ( TEntity entity )
        {
            return this._set.Remove ( entity );
        }

        #endregion

        #region Explicit Interface Methods

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator ( )
        {
            return this.GetEnumerator ( );
        }

        /// <summary>
        /// Finds the specified key values asynchronously.
        /// </summary>
        /// <param name="keyValues">The key values</param>
        /// <returns></returns>
        public async Task<TEntity> FindAsync ( params object [ ] keyValues )
        {
            //Todo: Review the casting to DbSet
            return await ( ( DbSet<TEntity> )this._set ).FindAsync ( keyValues );
        }

        /// <summary>
        /// Finds items by key values asynchronously
        /// </summary>
        /// <param name="cancellationToken">Allows the operation to be cancelled</param>
        /// <param name="keyValues">The key values</param>
        /// <returns></returns>
        public async Task<TEntity> FindAsync ( CancellationToken cancellationToken , params object [ ] keyValues )
        {
            //Todo: Review the casting to DbSet
            return await ( ( DbSet<TEntity> )this._set ).FindAsync ( cancellationToken , keyValues );

        }

        /// <summary>
        /// Gets the async enumerator.
        /// As we are dealing with an IDbSet rather than a DbSet, we need to cast it and then return the enumerator from DbQuery.
        /// </summary>
        /// <returns></returns>
        public IDbAsyncEnumerator<TEntity> GetAsyncEnumerator ( )
        {
            return ( ( IDbAsyncEnumerable<TEntity> )this._set ).GetAsyncEnumerator ( );
        }

        /// <summary>
        /// Gets the async enumerator
        /// </summary>
        /// <returns></returns>
        IDbAsyncEnumerator IDbAsyncEnumerable.GetAsyncEnumerator ( )
        {
            return this.GetAsyncEnumerator ( );
        }
        #endregion
    }
}
