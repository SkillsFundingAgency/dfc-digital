namespace DFC.Digital.Repository.ONET.Interface
{
    using System.Data.Entity.Infrastructure;
    using System.Threading;
    using System.Threading.Tasks;

    public interface IRepositoryInitializer
    {
        void Initialize ( object options = null );
    }

    public interface IDbContext
    {

        IDbSetWrapper<T> Set<T> ( ) where T : class;
        int SaveChanges ( );
        Task<int> SaveChangesAsync ( );
        Task<int> SaveChangesAsync ( CancellationToken cancellationToken );
        DbEntityEntry Entry ( object o );
        DbEntityEntry<TEntity> Entry<TEntity> ( TEntity entity ) where TEntity : class;
        void Dispose ( );
    }
}