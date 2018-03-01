using System.Collections.Generic;
using System.Threading.Tasks;

namespace DFC.Digital.Data.Interfaces
{
    public interface ISearchService<T>
        where T : class
    {
        Task EnsureIndexAsync(string indexName);

        void DeleteIndex(string indexName);

        Task PopulateIndexAsync(IEnumerable<T> data);
    }
}