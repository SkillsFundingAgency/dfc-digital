using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DFC.Digital.Web.Sitefinity.JobProfileModule.Services
{
    public class JobProfileSearchResultsManipulator : IJobProfileSearchResultsManipulator
    {
        public SearchResult<JobProfileIndex> ReorderForAlterantiveTitle(SearchResult<JobProfileIndex> searchResult, string searchTerm)
        {
            var jobprofileWithSearchTermInAlterantiveTitle = searchResult?.Results.Where(p => p.ResultItem.AlternativeTitle.Any(t => t.Equals(searchTerm, StringComparison.OrdinalIgnoreCase))).FirstOrDefault();

            //The results contain a profile and its not at the top.
            if (jobprofileWithSearchTermInAlterantiveTitle != null && jobprofileWithSearchTermInAlterantiveTitle.Rank != 1)
            {
                jobprofileWithSearchTermInAlterantiveTitle.Rank = 0;
                searchResult.Results = searchResult.Results.OrderBy(r => r.Rank);
            }

            return searchResult;
        }
    }
}