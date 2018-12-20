using DFC.Digital.Data.Model;

namespace DFC.Digital.Data.Interfaces
{
    public interface ISearchResultsManipulator<T>
        where T : class
    {
        SearchResult<T> Reorder(SearchResult<T> searchResult, string searchTerm, SearchProperties searchProperties);
    }
}