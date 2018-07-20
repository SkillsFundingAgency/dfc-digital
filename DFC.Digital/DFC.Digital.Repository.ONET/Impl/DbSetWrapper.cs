using System.Data.Entity;
using DFC.Digital.Repository.ONET.Interface;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;

namespace DFC.Digital.Repository.ONET.Impl
{
    using System.Collections.ObjectModel;

    public class DbSetWrapper<TEntity> : IDbSetWrapper<TEntity> where TEntity : class
    {

        private readonly IDbSet<TEntity> Set;


        public Expression Expression
        {
            get; private set;
        }
        public Type ElementType
        {
            get; private set;
        }
        public IQueryProvider Provider
        {
            get; private set;
        }

        public ObservableCollection<TEntity> Local
        {
            get; private set;
        }
        #region Constructors and Destructors
        public DbSetWrapper ( IDbSet<TEntity> set )
        {
            this.Set = set;
            this.Expression = set.Expression;
            this.ElementType = set.ElementType;
            this.Provider = set.Provider;
            this.Local = set.Local;
        }

        #endregion

        #region Implementation of IDbAsyncEnumerable

        public IDbAsyncEnumerator<TEntity> GetAsyncEnumerator()
        {
            return ((IDbAsyncEnumerable<TEntity>)this.Set).GetAsyncEnumerator();
        }

        IDbAsyncEnumerator IDbAsyncEnumerable.GetAsyncEnumerator()
        {
            return this.GetAsyncEnumerator();
        }

        #endregion

        #region Implementation of IEnumerable

        public IEnumerator<TEntity> GetEnumerator()
        {
            return ((IEnumerable<TEntity>)this.Set).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        #endregion

    }
}
