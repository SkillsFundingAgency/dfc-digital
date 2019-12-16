using DFC.Digital.Data.Model;
using System.Collections.Generic;
using FAC = DFC.FindACourseClient.Models.ExternalInterfaceModels;

namespace DFC.Digital.Web.Sitefinity.CourseModule
{
    public interface ICourseSearchResultsViewModelBullder
    {
        void SetupViewModelPaging(CourseSearchResultsViewModel viewModel, FAC.CourseSearchResult response, string resultsPage, int recordsPerPage);

        OrderByLinks GetOrderByLinks(string resultsPage, FAC.CourseSearchOrderBy orderBy);
    }
}