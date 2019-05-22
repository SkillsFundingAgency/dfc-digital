using DFC.Digital.Data.Model;
using DFC.Digital.Web.Core;
using System.Collections.Generic;
using System.Linq;

namespace DFC.Digital.Web.Sitefinity.CourseModule
{
    public class BuildQueryStringService : IBuildQueryStringService
    {
        public string BuildRedirectPathAndQueryString(string courseSearchResultsPage, string searchTerm, CourseSearchFilters courseSearchFilters)
        {
            var parameters = new Dictionary<string, string>();
            if (!string.IsNullOrWhiteSpace(searchTerm) || !string.IsNullOrWhiteSpace(courseSearchFilters.Provider))
            {
                if (!string.IsNullOrWhiteSpace(searchTerm))
                {
                    parameters.Add(nameof(searchTerm), StringManipulationExtension.GetUrlEncodedString(searchTerm));
                }

                if (!string.IsNullOrWhiteSpace(courseSearchFilters.Provider))
                {
                    parameters.Add(nameof(courseSearchFilters.Provider).ToLowerInvariant(), StringManipulationExtension.GetUrlEncodedString(courseSearchFilters.Provider));
                }

                if (courseSearchFilters.Attendance.Any())
                {
                    parameters.Add(nameof(courseSearchFilters.Attendance).ToLowerInvariant(), string.Join(",", courseSearchFilters.Attendance));
                }

                if (courseSearchFilters.Only1619Courses)
                {
                    parameters.Add(nameof(courseSearchFilters.Only1619Courses).ToLowerInvariant(), "true");
                }

                if (!string.IsNullOrWhiteSpace(courseSearchFilters.Location))
                {
                    parameters.Add(nameof(CourseSearchFilters.Location).ToLowerInvariant(), StringManipulationExtension.GetUrlEncodedString(courseSearchFilters.Location));
                }

                if (courseSearchFilters.AttendancePattern.Any())
                {
                    parameters.Add(nameof(CourseSearchFilters.AttendancePattern).ToLowerInvariant(), string.Join(",", courseSearchFilters.AttendancePattern));
                }

                parameters.Add(nameof(CourseSearchFilters.StartDate).ToLowerInvariant(), "anytime");

                if (courseSearchFilters.StudyMode.Any())
                {
                    parameters.Add(nameof(CourseSearchFilters.StudyMode).ToLowerInvariant(), string.Join(",", courseSearchFilters.StudyMode));
                }
            }

            var queryParameters = string.Join("&", parameters.Select(kvp => $"{kvp.Key}={kvp.Value}"));

            return $"{courseSearchResultsPage}?{queryParameters}";
        }
    }
}