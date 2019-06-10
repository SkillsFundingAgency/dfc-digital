using Castle.Core.Internal;
using DFC.Digital.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DFC.Digital.Web.Sitefinity.CourseModule
{
    public class CourseSearchResultsViewModelBullder : ICourseSearchResultsViewModelBullder
    {
        public void SetupViewModelPaging(CourseSearchResultsViewModel viewModel, CourseSearchResult response, string pathQuery, int recordsPerPage)
        {
            if (viewModel != null && response != null)
            {
                viewModel.Count = recordsPerPage;
                viewModel.Page = response.ResultProperties.Page;
                viewModel.ResultProperties = response.ResultProperties;

                if (viewModel.ResultProperties.TotalPages > 1 &&
                    viewModel.ResultProperties.TotalPages >= viewModel.ResultProperties.Page)
                {
                    viewModel.PaginationViewModel.HasPreviousPage = viewModel.ResultProperties.Page > 1;
                    viewModel.PaginationViewModel.HasNextPage =
                        viewModel.ResultProperties.Page < viewModel.ResultProperties.TotalPages;
                    viewModel.PaginationViewModel.NextPageUrl = new Uri(
                        $"{pathQuery}&{nameof(CourseSearchProperties.Page)}={viewModel.ResultProperties.Page + 1}", UriKind.RelativeOrAbsolute);
                    viewModel.PaginationViewModel.NextPageText =
                        $"{viewModel.ResultProperties.Page + 1} of {viewModel.ResultProperties.TotalPages}";

                    if (viewModel.ResultProperties.Page > 1)
                    {
                        viewModel.PaginationViewModel.PreviousPageUrl = new Uri(
                            $"{pathQuery}{(viewModel.ResultProperties.Page == 2 ? string.Empty : $"&{nameof(CourseSearchProperties.Page)}={viewModel.ResultProperties.Page - 1}")}",
                            UriKind.RelativeOrAbsolute);
                        viewModel.PaginationViewModel.PreviousPageText =
                            $"{viewModel.ResultProperties.Page - 1} of {viewModel.ResultProperties.TotalPages}";
                    }
                }
            }
        }

        public OrderByLinks GetOrderByLinks(string pathAndQuery, CourseSearchOrderBy orderBy)
        {
            if (pathAndQuery?.IndexOf($"&{nameof(CourseSearchProperties.OrderedBy)}=", StringComparison.InvariantCultureIgnoreCase) > 0)
            {
                pathAndQuery = pathAndQuery.Substring(0, pathAndQuery.IndexOf($"&{nameof(CourseSearchProperties.OrderedBy)}=", StringComparison.InvariantCultureIgnoreCase));
            }

            return new OrderByLinks
            {
                OrderBy = orderBy,
                OrderByRelevanceUrl = new Uri($"{pathAndQuery}&{nameof(CourseSearchProperties.OrderedBy)}={nameof(CourseSearchOrderBy.Relevance)}", UriKind.RelativeOrAbsolute),
                OrderByDistanceUrl = new Uri($"{pathAndQuery}&{nameof(CourseSearchProperties.OrderedBy)}={nameof(CourseSearchOrderBy.Distance)}", UriKind.RelativeOrAbsolute),
                OrderByStartDateUrl = new Uri($"{pathAndQuery}&{nameof(CourseSearchProperties.OrderedBy)}={nameof(CourseSearchOrderBy.StartDate)}", UriKind.RelativeOrAbsolute)
            };
        }
    }
}