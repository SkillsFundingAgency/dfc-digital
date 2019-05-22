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
            viewModel.Count = recordsPerPage;
            viewModel.Page = response.ResultProperties.Page;
            viewModel.ResultProperties = response.ResultProperties;

            if (viewModel.ResultProperties.TotalPages > 1 && viewModel.ResultProperties.TotalPages >= viewModel.ResultProperties.Page)
            {
                viewModel.PaginationViewModel.HasPreviousPage = viewModel.ResultProperties.Page > 1;
                viewModel.PaginationViewModel.HasNextPage = viewModel.ResultProperties.Page < viewModel.ResultProperties.TotalPages;
                viewModel.PaginationViewModel.NextPageUrl = new Uri($"{pathQuery}&page={viewModel.ResultProperties.Page + 1}", UriKind.RelativeOrAbsolute);
                viewModel.PaginationViewModel.NextPageUrlText = $"{viewModel.ResultProperties.Page + 1} of {viewModel.ResultProperties.TotalPages}";

                if (viewModel.ResultProperties.Page > 1)
                {
                    viewModel.PaginationViewModel.PreviousPageUrl = new Uri($"{pathQuery}{(viewModel.ResultProperties.Page == 2 ? string.Empty : $"&page={viewModel.ResultProperties.Page - 1}")}", UriKind.RelativeOrAbsolute);
                    viewModel.PaginationViewModel.PreviousPageUrlText = $"{viewModel.ResultProperties.Page - 1} of {viewModel.ResultProperties.TotalPages}";
                }
            }
        }

        public IEnumerable<SelectItem> GetFilterSelectItems(string propertyName, IEnumerable<string> sourceList, string value)
        {
            var selectList = new List<SelectItem>();
            var itemList = value?.Split(',');
            foreach (var sourceItem in sourceList)
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
            if (searchUrl?.ToLowerInvariant().IndexOf("&sortby=", StringComparison.InvariantCultureIgnoreCase) > 0)
            {
                searchUrl = searchUrl.Substring(0, searchUrl.ToLowerInvariant().IndexOf("&sortby=", StringComparison.InvariantCultureIgnoreCase));
            }

            return new OrderByLinks
            {
                CourseSearchSortBy = courseSearchSortBy,
                OrderByRelevanceUrl = new Uri($"{searchUrl}&sortby=relevance", UriKind.RelativeOrAbsolute),
                OrderByDistanceUrl = new Uri($"{searchUrl}&sortby=distance", UriKind.RelativeOrAbsolute),
                OrderByStartDateUrl = new Uri($"{searchUrl}&sortby=startdate", UriKind.RelativeOrAbsolute)
            };
        }

        public Dictionary<string, string> GetActiveFilterOptions(CourseFiltersViewModel courseFiltersModel)
        {
            var activeFilters = new Dictionary<string, string>();

            if (!string.IsNullOrWhiteSpace(courseFiltersModel.Location))
            {
                activeFilters.Add("Location:", courseFiltersModel.Location);
            }

            if (!string.IsNullOrWhiteSpace(courseFiltersModel.Provider))
            {
                activeFilters.Add("Provider:", courseFiltersModel.Provider);
            }

            if (courseFiltersModel.AttendanceSelectedList.Any(x => !string.IsNullOrWhiteSpace(x.Checked)))
            {
                activeFilters.Add("Attendance:", string.Join(", ", courseFiltersModel.AttendanceSelectedList.Where(x => !string.IsNullOrWhiteSpace(x.Checked)).Select(lbl => lbl.Label)));
            }

            if (courseFiltersModel.PatternSelectedList.Any(x => !string.IsNullOrWhiteSpace(x.Checked)))
            {
                activeFilters.Add("Course type:", string.Join(", ", courseFiltersModel.PatternSelectedList.Where(x => !string.IsNullOrWhiteSpace(x.Checked)).Select(lbl => lbl.Label)));
            }

            if (courseFiltersModel.AgeSuitabilitySelectedList.Any(x => !string.IsNullOrWhiteSpace(x.Checked)))
            {
                activeFilters.Add("Age Suitability:", string.Join(", ", courseFiltersModel.AgeSuitabilitySelectedList.Where(x => !string.IsNullOrWhiteSpace(x.Checked)).Select(lbl => lbl.Label)));
            }

            if (courseFiltersModel.StudyModeSelectedList.Any(x => !string.IsNullOrWhiteSpace(x.Checked)))
            {
                activeFilters.Add("Study mode:", string.Join(", ", courseFiltersModel.StudyModeSelectedList.Where(x => !string.IsNullOrWhiteSpace(x.Checked)).Select(lbl => lbl.Label)));
            }

            return activeFilters;
        }
    }
}