﻿using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using System;
using System.Linq;

namespace DFC.Digital.Service.AzureSearch
{
    public class JobProfileSearchManipulator : ISearchManipulator<JobProfileIndex>
    {
        public string BuildSearchExpression(string searchTerm, string cleanedSearchTerm, string partialTermToSearch, SearchProperties properties)
        {
            if (searchTerm == null)
            {
                throw new ArgumentNullException(nameof(searchTerm));
            }

            if (cleanedSearchTerm == null)
            {
                throw new ArgumentNullException(nameof(cleanedSearchTerm));
            }

            if (partialTermToSearch == null)
            {
                throw new ArgumentNullException(nameof(partialTermToSearch));
            }

            if (searchTerm == "*")
            {
                return searchTerm;
            }
            else if (properties?.UseRawSearchTerm == true)
            {
                return cleanedSearchTerm;
            }
            else
            {
                return $"{nameof(JobProfileIndex.Title)}:({partialTermToSearch.ToLower()}) {nameof(JobProfileIndex.AlternativeTitle)}:({partialTermToSearch.ToLower()}) {nameof(JobProfileIndex.TitleAsKeyword)}:\"{searchTerm.ToLower()}\" {nameof(JobProfileIndex.AltTitleAsKeywords)}:\"{searchTerm.ToLower()}\" {cleanedSearchTerm}";
            }
        }

        public SearchResult<JobProfileIndex> Reorder(SearchResult<JobProfileIndex> searchResult, string searchTerm, SearchProperties searchProperties)
        {
            if (searchProperties?.Page == 1 && searchResult != null)
            {
                var results = searchResult?.Results?.ToList();
                var promo = results
                    ?.FirstOrDefault(p => p.ResultItem.Title.Equals(searchTerm, StringComparison.OrdinalIgnoreCase));

                if (promo == null)
                {
                    promo = results
                    ?.FirstOrDefault(p =>
                    p.ResultItem.AlternativeTitle.Any(a => a.Equals(searchTerm, StringComparison.OrdinalIgnoreCase)));
                }

                //The results contain a profile and its not at the top.
                if (promo != null && promo.Rank != 1)
                {
                    promo.Rank = 0;
                    searchResult.Results = results.OrderBy(r => r.Rank);
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