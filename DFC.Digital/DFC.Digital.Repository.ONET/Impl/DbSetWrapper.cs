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

    public class DbSetWrapper<TEntity> : IDbSetWrapper<TEntity> where TEntity : class
    {

        private readonly IDbSet<TEntity> set;
        private readonly Expression expression;
        private readonly Type elementType;
        private readonly IQueryProvider provider;

        public Expression Expression => expression;
        public Type ElementType => elementType;
        public IQueryProvider Provider => provider;

        #region Constructors and Destructors
        public DbSetWrapper ( IDbSet<TEntity> set )
        {
            this.set = set;
            this.expression = set.Expression;
            this.elementType = set.ElementType;
            this.provider = set.Provider;
        }

        #endregion

        #region Implementation of IDbAsyncEnumerable

        public IDbAsyncEnumerator<TEntity> GetAsyncEnumerator()
        {
            return ((IDbAsyncEnumerable<TEntity>)this.set).GetAsyncEnumerator();
        }

        IDbAsyncEnumerator IDbAsyncEnumerable.GetAsyncEnumerator()
        {
            return this.GetAsyncEnumerator();
        }

        #endregion

        #region Implementation of IEnumerable

        public IEnumerator<TEntity> GetEnumerator()
        {
            return ((IEnumerable<TEntity>)this.set).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        #endregion

    }
}
