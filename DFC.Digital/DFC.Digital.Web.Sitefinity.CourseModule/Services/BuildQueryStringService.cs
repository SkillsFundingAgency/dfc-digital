using DFC.Digital.Data.Model;
using DFC.Digital.Web.Core;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DFC.Digital.Web.Sitefinity.CourseModule
{
    public class BuildQueryStringService : IQueryStringBuilder<CourseSearchFilters>
    {
        public string BuildPathAndQueryString(string path, CourseSearchFilters courseSearchFilters)
        {
            if (courseSearchFilters is null)
            {
                return path;
            }

            var parameters = new Dictionary<string, string>
            {
                [nameof(CourseSearchFilters.SearchTerm)] = courseSearchFilters.SearchTerm,
                [nameof(CourseSearchFilters.Provider)] = courseSearchFilters.Provider,
                [nameof(CourseSearchFilters.CourseType)] = courseSearchFilters.CourseType != default(CourseType) ? courseSearchFilters.CourseType.ToString() : null,
                [nameof(CourseSearchFilters.Only1619Courses)] = courseSearchFilters.Only1619Courses ? true.ToString() : null,
                [nameof(CourseSearchFilters.Location)] = courseSearchFilters.Location,
                [nameof(CourseSearchFilters.CourseHours)] = courseSearchFilters.CourseHours != default(CourseHours) ? courseSearchFilters.CourseHours.ToString() : null,
                [nameof(CourseSearchFilters.StartDate)] = courseSearchFilters.StartDate != default(StartDate) ? courseSearchFilters.StartDate.ToString() : null,
                [nameof(CourseSearchFilters.StartDateFrom)] = courseSearchFilters.StartDateFrom
            };
            var queryParameters = string.Join("&", parameters.Where(d => !string.IsNullOrEmpty(d.Value)).Select(kvp => $"{kvp.Key}={HttpUtility.UrlEncode(kvp.Value)}"));
            return $"{path}?{queryParameters}";
        }
    }
}