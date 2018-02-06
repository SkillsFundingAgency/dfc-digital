using DFC.Digital.Data.Model;

namespace DFC.Digital.Data.Interfaces
{
    public interface IPreSearchFiltersFactory
    {
        IPreSearchFiltersRepository<T> GetRepository<T>()
            where T : PreSearchFilter, new();
    }
}
