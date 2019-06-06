using DFC.Digital.Data.Model;

namespace DFC.Digital.Web.Sitefinity.CourseModule
{
    public interface IBuildQueryStringService
    {
        string BuildRedirectPathAndQueryString(string courseSearchResultsPage, string searchTerm, CourseSearchFilters courseSearchFilters);

        string BuildViewCourseUrlPathAndQueryString(string urlPath, string courseId, string opportunityId, string referralUrl);
    }
}
