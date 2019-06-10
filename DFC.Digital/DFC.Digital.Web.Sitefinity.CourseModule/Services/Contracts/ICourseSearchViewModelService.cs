using DFC.Digital.Data.Model;
using System.Collections.Generic;

namespace DFC.Digital.Web.Sitefinity.CourseModule
{
    public interface ICourseSearchViewModelService
    {
        void SetupViewModelPaging(CourseSearchResultsViewModel viewModel, CourseSearchResult response, string pathQuery, int recordsPerPage);

        OrderByLinks GetOrderByLinks(string searchUrl, CourseSearchOrderBy courseSearchSortBy);
    }
}