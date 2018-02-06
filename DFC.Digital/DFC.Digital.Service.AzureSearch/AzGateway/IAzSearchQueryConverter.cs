using DFC.Digital.Data.Model;
using Microsoft.Azure.Search.Models;

namespace DFC.Digital.Service.AzureSearch
{
    public interface IAzSearchQueryConverter
    {
        SearchParameters BuildSearchParameters(SearchProperties properties);

        Data.Model.SearchResult<T> ConvertToSearchResult<T>(DocumentSearchResult<T> result, SearchProperties properties)
            where T : class;

        SuggestParameters BuildSuggestParameters(SuggestProperties properties);

        Data.Model.SuggestionResult<T> ConvertToSuggestionResult<T>(DocumentSuggestResult<T> result, SuggestProperties properties)
            where T : class;
    }
}