using DFC.Digital.Data.Model;
using System.Collections.Generic;

namespace DFC.Digital.Web.Sitefinity.CourseModule
{
    public interface ICourseSearchViewModelService
    {
        void SetupPaging(TrainingCourseResultsViewModel viewModel, CourseSearchResult response, string pathQuery, int recordsPerPage, string courseSearchResultsPage);

        IEnumerable<SelectItem> GetFilterSelectItems(string propertyName, IEnumerable<string> sourceList, string value);

        OrderByLinks GetOrderByLinks(string searchUrl, CourseSearchOrderBy courseSearchSortBy);

        Dictionary<string, string> GetActiveFilterOptions(CourseFiltersModel courseFiltersModel);
    }
}