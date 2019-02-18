using DFC.Digital.Data.Model;

namespace DFC.Digital.Web.Sitefinity.CourseModule
{
    public interface ICourseSearchConverter
    {
        string BuildRedirectPathAndQueryString(string courseSearchResultsPage, TrainingCourseResultsViewModel trainingCourseResultsViewModel, string locationDistanceRegex);

        CourseSearchRequest GetCourseSearchRequest(string searchTerm, int recordsPerPage, string attendance, string studymode, string qualificationLevel, string distance, string dfe1619Funded, int page);
        string GetUrlEncodedString(string input);
        void SetupPaging(TrainingCourseResultsViewModel viewModel, CourseSearchResponse response, string searchTerm, int recordsPerPage, string courseSearchResultsPage);
    }
}