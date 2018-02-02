using DFC.Digital.Data.Model;
using System.Threading.Tasks;

namespace DFC.Digital.Data.Interfaces
{
    public interface ISearchQueryService<T>
        where T : class
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed", Justification = "This doesn't need to be CLS compliant")]
        SearchResult<T> Search(string searchTerm, SearchProperties properties = null);

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed", Justification = "This doesn't need to be CLS compliant")]
        Task<SearchResult<T>> SearchAsync(string searchTerm, SearchProperties properties = null);

        SuggestionResult<T> GetSuggestion(string partialTerm, SuggestProperties properties);

        Task<SuggestionResult<T>> GetSuggestionAsync(string partialTerm, SuggestProperties properties);
    }
}