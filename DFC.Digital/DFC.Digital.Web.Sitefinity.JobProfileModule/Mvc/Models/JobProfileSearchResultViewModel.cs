using System;
using System.Collections.Generic;
using System.Linq;

namespace DFC.Digital.Web.Sitefinity.JobProfileModule.Mvc.Models
{
    /// <summary>
    /// Job Profile Search Results View Model
    /// </summary>
    /// <seealso cref="DFC.Digital.Web.Sitefinity.JobProfileModule.Mvc.Models.JobProfileSearchBoxViewModel" />
    public class JobProfileSearchResultViewModel : JobProfileSearchBoxViewModel
    {
        /// <summary>
        /// Gets or sets the page number.
        /// </summary>
        /// <value>
        /// The page number.
        /// </value>
        public int PageNumber { get; set; } = 1;

        /// <summary>
        /// Gets or sets the next page URL.
        /// </summary>
        /// <value>
        /// The next page URL.
        /// </value>
        public Uri NextPageUrl { get; set; }

        /// <summary>
        /// Gets or sets the next page URL text.
        /// </summary>
        /// <value>
        /// The next page URL text.
        /// </value>
        public string NextPageUrlText { get; set; }

        /// <summary>
        /// Gets or sets the previous page URL.
        /// </summary>
        /// <value>
        /// The previous page URL.
        /// </value>
        public Uri PreviousPageUrl { get; set; }

        /// <summary>
        /// Gets or sets the previous page URL text.
        /// </summary>
        /// <value>
        /// The previous page URL text.
        /// </value>
        public string PreviousPageUrlText { get; set; }

        /// <summary>
        /// Gets a value indicating whether gets or sets a value indicating whether this instance has nex page.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance has nex page; otherwise, <c>false</c>.
        /// </value>
        public bool HasNextPage => TotalPages - PageNumber > 0;

        /// <summary>
        /// Gets a value indicating whether gets or sets a value indicating whether this instance has previous page.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance has previous page; otherwise, <c>false</c>.
        /// </value>
        public bool HasPreviousPage => PageNumber > 1;

        /// <summary>
        /// Gets or sets the count.
        /// </summary>
        /// <value>
        /// The count.
        /// </value>
        public long? Count { get; set; }

        public long? TotalPages { get; set; }

        /// <summary>
        /// Gets or sets the search results.
        /// </summary>
        /// <value>
        /// The search results.
        /// </value>
        public IEnumerable<JobProfileSearchResultItemViewModel> SearchResults { get; set; } = Enumerable.Empty<JobProfileSearchResultItemViewModel>();

        /// <summary>
        /// Gets or sets The base URL for job categories
        /// </summary>
        /// <value>
        /// The JobProfileCategoryPage.
        /// </value>
        public string JobProfileCategoryPage { get; set; }

        public Uri DidYouMeanUrl { get; set; }

        public string DidYouMeanTerm { get; set; }

        public string SalaryBlankText { get; set; }

        public bool ShowSearchedTerm { get; set; }

        public double? Coverage { get; set; }

        public string ComputedSearchTerm { get; set; }

        public long? TotalResultCount { get; set; }
    }
}