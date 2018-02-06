using DFC.Digital.Data.Model;
using Microsoft.Azure.Search.Models;
using System.Collections.Generic;
using System.Linq;

namespace DFC.Digital.Service.AzureSearch.Extensions
{
    public static class SearchExtensions
    {
        public static IEnumerable<SearchResultItem<T>> ToSearchResultItems<T>(this DocumentSearchResult<T> results, SearchProperties properties)
            where T : class
        {
            var beginRank = ((properties.Page - 1) * properties.Count) + properties.ExactMatchCount;

            var resultList = new List<SearchResultItem<T>>();
            foreach (var result in results.Results)
            {
                beginRank++;
                resultList.Add(new SearchResultItem<T>
                {
                    ResultItem = result.Document,
                    Rank = beginRank
                });
            }

            return resultList;
        }

        public static IEnumerable<SuggestionResultItem<T>> ToSuggestResultItems<T>(this DocumentSuggestResult<T> results, SuggestProperties properties)
            where T : class
        {
            return results.Results.Select(r => new SuggestionResultItem<T>
            {
                Index = r.Document,
                MatchedSuggestion = r.Text,
            });
        }
    }
}