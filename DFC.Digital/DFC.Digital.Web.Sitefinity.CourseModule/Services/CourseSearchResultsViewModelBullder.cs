using Castle.Core.Internal;
using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DFC.Digital.Web.Sitefinity.CourseModule
{
    public class CourseSearchResultsViewModelBullder : ICourseSearchResultsViewModelBullder
    {
        private readonly IWebAppContext context;

        public CourseSearchResultsViewModelBullder(IWebAppContext context)
        {
            this.context = context;
        }

        public void SetupViewModelPaging(CourseSearchResultsViewModel viewModel, CourseSearchResult response, string resultsPage, int recordsPerPage)
        {
            if (viewModel != null && response != null)
            {
                viewModel.Count = recordsPerPage;
                viewModel.Page = response.ResultProperties.Page;
                viewModel.ResultProperties = response.ResultProperties;

                if (viewModel.ResultProperties.TotalPages > 1 && viewModel.ResultProperties.TotalPages >= viewModel.ResultProperties.Page)
                {
                    var partialQueryString = context.GetQueryStringExcluding(new[] { $"{nameof(CourseSearchProperties.Page)}" });
                    viewModel.PaginationViewModel.HasPreviousPage = viewModel.ResultProperties.Page > 1;
                    viewModel.PaginationViewModel.HasNextPage = viewModel.ResultProperties.Page < viewModel.ResultProperties.TotalPages;
                    viewModel.PaginationViewModel.NextPageUrl = new Uri($"{resultsPage}?{partialQueryString}&{nameof(CourseSearchProperties.Page)}={viewModel.ResultProperties.Page + 1}", UriKind.RelativeOrAbsolute);
                    viewModel.PaginationViewModel.NextPageText = $"{viewModel.ResultProperties.Page + 1} of {viewModel.ResultProperties.TotalPages}";

                    if (viewModel.ResultProperties.Page > 1)
                    {
                        viewModel.PaginationViewModel.PreviousPageUrl = new Uri($"{resultsPage}?{partialQueryString}&{nameof(CourseSearchProperties.Page)}={viewModel.ResultProperties.Page - 1}", UriKind.RelativeOrAbsolute);
                        viewModel.PaginationViewModel.PreviousPageText = $"{viewModel.ResultProperties.Page - 1} of {viewModel.ResultProperties.TotalPages}";
                    }
                }
            }
        }

        public OrderByLinks GetOrderByLinks(string resultsPage, CourseSearchOrderBy orderBy)
        {
            var partialQueryString = context.GetQueryStringExcluding(new[] { $"{nameof(CourseSearchProperties.OrderedBy)}" });
            return new OrderByLinks
            {
                OrderBy = orderBy,
                OrderByRelevanceUrl = new Uri($"{resultsPage}?{partialQueryString.ToString()}&{nameof(CourseSearchProperties.OrderedBy)}={nameof(CourseSearchOrderBy.Relevance)}", UriKind.RelativeOrAbsolute),
                OrderByDistanceUrl = new Uri($"{resultsPage}?{partialQueryString.ToString()}&{nameof(CourseSearchProperties.OrderedBy)}={nameof(CourseSearchOrderBy.Distance)}", UriKind.RelativeOrAbsolute),
                OrderByStartDateUrl = new Uri($"{resultsPage}?{partialQueryString.ToString()}&{nameof(CourseSearchProperties.OrderedBy)}={nameof(CourseSearchOrderBy.StartDate)}", UriKind.RelativeOrAbsolute)
            };
        }
    }
}