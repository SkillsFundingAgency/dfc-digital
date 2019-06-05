using Castle.Core.Internal;
using DFC.Digital.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DFC.Digital.Web.Sitefinity.CourseModule
{
    public class CourseSearchViewModelService : ICourseSearchViewModelService
    {
        public void SetupPaging(CourseSearchResultsViewModel viewModel, CourseSearchResult response, string pathQuery, int recordsPerPage, string courseSearchResultsPage)
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
                        $"{pathQuery}&page={viewModel.ResultProperties.Page + 1}", UriKind.RelativeOrAbsolute);
                    viewModel.PaginationViewModel.NextPageText =
                        $"{viewModel.ResultProperties.Page + 1} of {viewModel.ResultProperties.TotalPages}";

                    if (viewModel.ResultProperties.Page > 1)
                    {
                        viewModel.PaginationViewModel.PreviousPageUrl = new Uri(
                            $"{pathQuery}{(viewModel.ResultProperties.Page == 2 ? string.Empty : $"&page={viewModel.ResultProperties.Page - 1}")}",
                            UriKind.RelativeOrAbsolute);
                        viewModel.PaginationViewModel.PreviousPageText =
                            $"{viewModel.ResultProperties.Page - 1} of {viewModel.ResultProperties.TotalPages}";
                    }
                }
            }
        }

        public IEnumerable<SelectItem> GetFilterSelectItems(string propertyName, IEnumerable<string> sourceList, string value)
        {
            var selectList = new List<SelectItem>();

            var sourceItems = sourceList.ToList();
            if (sourceItems.IsNullOrEmpty())
            {
                return selectList;
            }

            var itemList = value?.Split(',');
            foreach (var sourceItem in sourceItems)
            {
                var dataValues = sourceItem.Split(':');
                if (dataValues.Length == 2)
                {
                    selectList.Add(new SelectItem
                    {
                        Checked = itemList != null && itemList.Contains(dataValues[1].Trim()) ? "checked" : string.Empty,
                        Label = dataValues[0].Trim(),
                        Name = propertyName,
                        Id = $"{propertyName}{dataValues[1].Trim()}",
                        Value = dataValues[1].Trim()
                    });
                }
            }

            return selectList;
        }

        public OrderByLinks GetOrderByLinks(string searchUrl, CourseSearchOrderBy courseSearchSortBy)
        {
            if (searchUrl?.IndexOf($"&{nameof(CourseSearchProperties.OrderedBy)}=", StringComparison.InvariantCultureIgnoreCase) > 0)
            {
                searchUrl = searchUrl.Substring(0, searchUrl.IndexOf($"&{nameof(CourseSearchProperties.OrderedBy)}=", StringComparison.InvariantCultureIgnoreCase));
            }

            return new OrderByLinks
            {
                CourseSearchSortBy = courseSearchSortBy,
                OrderByRelevanceUrl = new Uri($"{searchUrl}&{nameof(CourseSearchProperties.OrderedBy)}={nameof(CourseSearchOrderBy.Relevance)}", UriKind.RelativeOrAbsolute),
                OrderByDistanceUrl = new Uri($"{searchUrl}&{nameof(CourseSearchProperties.OrderedBy)}={nameof(CourseSearchOrderBy.Distance)}", UriKind.RelativeOrAbsolute),
                OrderByStartDateUrl = new Uri($"{searchUrl}&{nameof(CourseSearchProperties.OrderedBy)}={nameof(CourseSearchOrderBy.StartDate)}", UriKind.RelativeOrAbsolute)
            };
        }

        public IEnumerable<KeyValuePair<string, string>> GetActiveFilterOptions(CourseFiltersViewModel courseFiltersModel)
        {
            if (courseFiltersModel is null)
            {
                return new Dictionary<string, string>();
            }

            var activeFilters = new Dictionary<string, string>
            {
                [nameof(CourseSearchFilters.Location)] = courseFiltersModel.Location,
                [nameof(CourseSearchFilters.Provider)] = courseFiltersModel.Provider,
                [nameof(CourseSearchFilters.CourseType)] = courseFiltersModel.CourseType != default(CourseType)
                    ? courseFiltersModel.CourseType.ToString()
                    : null,
                [nameof(CourseSearchFilters.CourseHours)] = courseFiltersModel.CourseHours != default(CourseHours)
                    ? courseFiltersModel.CourseHours.ToString()
                    : null,
                [nameof(CourseSearchFilters.StartDate)] = courseFiltersModel.StartDate != default(StartDate)
                    ? courseFiltersModel.StartDate.ToString()
                    : null,
                [nameof(CourseSearchFilters.StartDateFrom)] = courseFiltersModel.StartDateFrom,
                [nameof(CourseSearchFilters.Only1619Courses)] =
                    courseFiltersModel.Only1619Courses ? true.ToString() : null
            };

            return activeFilters.Where(x => !string.IsNullOrWhiteSpace(x.Value));
        }
    }
}