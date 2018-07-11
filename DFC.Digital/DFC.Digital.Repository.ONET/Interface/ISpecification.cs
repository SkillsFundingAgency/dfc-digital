namespace DFC.Digital.Repository.ONET.Interface
{
    using System.Linq;

    public interface ISpecification<TEntity>
    {
        TEntity SatisfyingEntityFrom(IQueryable<TEntity> query);
        IQueryable<TEntity> SatisfyingEntitiesFrom(IQueryable<TEntity> query);
        IQueryable<TEntity> SatisfyingEntitiesInOrderFrom(IQueryable<TEntity> query, string sortOrder);
    }
}