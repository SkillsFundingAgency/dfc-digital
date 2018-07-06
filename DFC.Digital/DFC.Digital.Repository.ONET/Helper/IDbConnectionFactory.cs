namespace DFC.Digital.Repository.ONET.Helper
{
    using System.Data.Common;

    public interface IDbConnectionFactory
    {
        DbConnection GetConnection();
    }
}