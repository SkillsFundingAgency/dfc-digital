namespace DFC.Digital.Repository.ONET.Interface
{
    public interface IDbContext
    {
        IDbSetWrapper<T> Set<T>() where T : class;
        void Dispose();
    }
}