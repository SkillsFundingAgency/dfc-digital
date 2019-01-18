using DFC.Digital.Data.Model;

namespace DFC.Digital.Data.Interfaces
{
    public interface ISearchManipulator<T>
        where T : class
    {
        SearchResult<T> Reorder(SearchResult<T> searchResult, string searchTerm, SearchProperties searchProperties);

        string BuildSearchExpression(string searchTerm, string cleanedSearchTerm, string partialTermToSearch, SearchProperties properties);
    }
}