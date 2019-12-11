using DFC.FindACourseClient.Models.ExternalInterfaceModels;

namespace DFC.Digital.Web.Sitefinity.CourseModule
{
    public interface ICourseSearchResultsViewModelBullder
    {
        void SetupViewModelPaging(CourseSearchResultsViewModel viewModel, CourseSearchResult response, string resultsPage, int recordsPerPage);

        OrderByLinks GetOrderByLinks(string resultsPage, CourseSearchOrderBy courseSearchOrderBy);
    }
}