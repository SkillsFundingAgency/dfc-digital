using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using System;
using System.Linq;

namespace DFC.Digital.Web.Sitefinity.JobProfileModule.Services
{
    public class JobProfileSearchResultsManipulator : ISearchResultsManipulator<JobProfileIndex>
    {
        public SearchResult<JobProfileIndex> Reorder(SearchResult<JobProfileIndex> searchResult, string searchTerm, SearchProperties searchProperties)
        {
            if (searchProperties.Page == 1)
            {
                var promo = searchResult
                    ?.Results
                    ?.FirstOrDefault(p =>
                    p.ResultItem.AlternativeTitle.Any(a => a.Equals(searchTerm, StringComparison.OrdinalIgnoreCase))
                    || p.ResultItem.Title.Equals(searchTerm, StringComparison.OrdinalIgnoreCase));

                //The results contain a profile and its not at the top.
                if (promo != null && promo.Rank != 1)
                {
                    promo.Rank = 0;
                    searchResult.Results = searchResult.Results.OrderBy(r => r.Rank);
                }
            }

            searchResult = searchResult ?? new SearchResult<JobProfileIndex>();
            if (searchResult.Results is null)
            {
                searchResult.Results = Enumerable.Empty<SearchResultItem<JobProfileIndex>>();
            }

            return searchResult;
        }
    }
}