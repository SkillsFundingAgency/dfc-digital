using System.Data.Entity;
using DFC.Digital.Repository.ONET.Interface;
namespace DFC.Digital.Repository.ONET.Impl
{
    public class DbSetWrapper<TEntity> : IDbSetWrapper<TEntity> where TEntity : class
    {
        #region Fields

        private readonly IDbSet<TEntity> set;

        #endregion

        #region Constructors and Destructors
        public DbSetWrapper ( IDbSet<TEntity> set )
        {
            this.set = set;
        }

        #endregion
    }
}
