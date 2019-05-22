using DFC.Digital.Data.Model;
using DFC.Digital.Web.Sitefinity.CourseModule.Mvc.Models;

namespace DFC.Digital.Web.Sitefinity.CourseModule
{
    public interface IBuildQueryStringService
    {
        string BuildRedirectPathAndQueryString(string courseSearchResultsPage, string searchTerm, CourseSearchFilters courseSearchFilters);
    }
}
