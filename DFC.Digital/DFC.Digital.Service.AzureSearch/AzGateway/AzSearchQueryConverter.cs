using DFC.Digital.Data.Model;
using DFC.Digital.Service.AzureSearch.Extensions;
using Microsoft.Azure.Search.Models;
using System;

namespace DFC.Digital.Service.AzureSearch.AzGateway
{
    public class AzSearchQueryConverter : IAzSearchQueryConverter
    {
        public SearchParameters BuildSearchParameters(SearchProperties properties)
        {
            if (properties == null)
            {
                throw new ArgumentNullException(nameof(properties));
            }

            return new SearchParameters
            {
                SearchMode = SearchMode.Any,
                IncludeTotalResultCount = true,
                SearchFields = properties.SearchFields,
                Filter = properties.FilterBy,
                Skip = (properties.Page - 1) * properties.Count,
                Top = properties.Count,
                QueryType = QueryType.Full,
                OrderBy = properties.OrderByFields,
            };
        }

        public SuggestParameters BuildSuggestParameters(SuggestProperties properties)
        {
            return new SuggestParameters
            {
                UseFuzzyMatching = properties?.UseFuzzyMatching ?? true,
                Top = properties?.MaxResultCount,
            };
        }

        public Data.Model.SearchResult<T> ConvertToSearchResult<T>(DocumentSearchResult<T> result, SearchProperties properties)
            where T : class
        {
            if (properties == null)
            {
                throw new ArgumentNullException(nameof(properties));
            }

            if (result == null)
            {
                throw new ArgumentNullException(nameof(result));
            }

            return new Data.Model.SearchResult<T>
            {
                Count = result.Count,
                Results = result.ToSearchResultItems(properties)
            };
        }

        public SuggestionResult<T> ConvertToSuggestionResult<T>(DocumentSuggestResult<T> result, SuggestProperties properties)
            where T : class
        {
            if (properties == null)
            {
                throw new ArgumentNullException(nameof(properties));
            }

            if (result == null)
            {
                throw new ArgumentNullException(nameof(result));
            }

            return new Data.Model.SuggestionResult<T>
            {
                Coverage = result.Coverage,
                Results = result.ToSuggestResultItems()
            };
        }
    }
}