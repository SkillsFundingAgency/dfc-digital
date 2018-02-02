using System.Collections.Generic;
using System.Threading.Tasks;

namespace DFC.Digital.Data.Interfaces
{
    public interface ISearchService<T>
        where T : class
    {
        void EnsureIndex(string indexName);

        Task EnsureIndexAsync(string indexName);

        void DeleteIndex(string indexName);

        void PopulateIndex(IEnumerable<T> data);

        Task PopulateIndexAsync(IEnumerable<T> data);
    }
}