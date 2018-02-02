using DFC.Digital.Data.Model;
using System.Collections.Generic;

namespace DFC.Digital.Data.Interfaces
{
    public interface IPreSearchFiltersRepository<T>
        where T : PreSearchFilter, new()
    {
        IEnumerable<T> GetAllFilters();
    }
}