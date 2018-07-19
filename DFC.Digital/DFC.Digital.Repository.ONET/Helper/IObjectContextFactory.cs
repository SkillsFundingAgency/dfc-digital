using System.Reflection;
namespace DFC.Digital.Repository.ONET.Helper
{
    public interface IObjectContextFactory<out T>
    {
        ConstructorInfo ClassConstructor { get;  }
        T GetContext();
        T GetContext(string connectionString);
    }
}