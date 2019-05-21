using DFC.Digital.Web.Sitefinity.CourseModule.Mvc.Models;

namespace DFC.Digital.Web.Sitefinity.CourseModule
{
    public interface IBuildQueryStringService
    {
        string BuildRedirectPathAndQueryString(string courseSearchResultsPage, TrainingCourseResultsViewModel trainingCourseResultsViewModel, string locationDistanceRegex);

        string BuildSearchRedirectPathAndQueryString(string courseSearchResultsPage, CourseLandingViewModel courseLandingViewModel, string locationDistanceRegex);

        string GetUrlEncodedString(string input);
    }
}
