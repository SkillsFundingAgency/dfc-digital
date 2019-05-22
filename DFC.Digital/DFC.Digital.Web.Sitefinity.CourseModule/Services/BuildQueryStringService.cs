using DFC.Digital.Data.Model;
using DFC.Digital.Web.Core;
using System.Linq;

namespace DFC.Digital.Web.Sitefinity.CourseModule
{
    public class BuildQueryStringService : IBuildQueryStringService
    {
        public string BuildRedirectPathAndQueryString(string courseSearchResultsPage, string searchTerm, CourseSearchFilters courseSearchFilters)
        {
            var queryParameters = $"{courseSearchResultsPage}?";
            if (!string.IsNullOrWhiteSpace(searchTerm) || !string.IsNullOrWhiteSpace(courseSearchFilters.ProviderKeyword))
            {
                if (!string.IsNullOrWhiteSpace(searchTerm))
                {
                    queryParameters = $"{queryParameters}searchTerm={StringManipulationExtension.GetUrlEncodedString(searchTerm)}";
                }

                if (!string.IsNullOrWhiteSpace(courseSearchFilters.ProviderKeyword))
                {
                    queryParameters = $"{queryParameters}provider={StringManipulationExtension.GetUrlEncodedString(courseSearchFilters.ProviderKeyword)}";
                }

                if (courseSearchFilters.Attendance.Any())
                {
                    queryParameters = $"{queryParameters}&attendance={string.Join(",", courseSearchFilters.Attendance)}";
                }

                if (courseSearchFilters.Only1619Courses)
                {
                    queryParameters = $"{queryParameters}&dfe1619Funded=true";
                }

                if (!string.IsNullOrWhiteSpace(courseSearchFilters.Location))
                {
                    queryParameters = $"{queryParameters}&location={StringManipulationExtension.GetUrlEncodedString(courseSearchFilters.Location)}";
                }

                if (courseSearchFilters.AttendancePattern.Any())
                {
                    queryParameters = $"{queryParameters}&pattern={string.Join(",", courseSearchFilters.AttendancePattern)}";
                }

                queryParameters = $"{queryParameters}&startDate=anytime";

                if (courseSearchFilters.StudyMode.Any())
                {
                    queryParameters = $"{queryParameters}&studymode={string.Join(",", courseSearchFilters.StudyMode)}";
                }

                queryParameters = $"{queryParameters}&page=1";
            }

            return queryParameters;
        }
    }
}