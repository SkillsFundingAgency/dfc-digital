using DFC.Digital.Core;
using DFC.Digital.Data.Model;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace DFC.Digital.Web.Sitefinity.CourseModule
{
    public class QueryStringBuilder : IQueryStringBuilder<CourseSearchFilters>
    {
        public string BuildPathAndQueryString(string path, CourseSearchFilters queryParameters)
        {
            if (queryParameters is null)
            {
                return path;
            }

            var parameters = new Dictionary<string, string>
            {
                [nameof(CourseSearchFilters.SearchTerm)] = queryParameters.SearchTerm,
                [nameof(CourseSearchFilters.Provider)] = queryParameters.Provider,
                [nameof(CourseSearchFilters.CourseType)] = queryParameters.CourseType != CourseType.All ? queryParameters.CourseType.ToString() : null,
                [nameof(CourseSearchFilters.Only1619Courses)] = queryParameters.Only1619Courses ? true.ToString() : null,
                [nameof(CourseSearchFilters.Location)] = queryParameters.Location,
                [nameof(CourseSearchFilters.Distance)] = queryParameters.Distance.ToString(CultureInfo.InvariantCulture),
                [nameof(CourseSearchFilters.CourseHours)] = queryParameters.CourseHours != CourseHours.All ? queryParameters.CourseHours.ToString() : null,
                [nameof(CourseSearchFilters.StartDate)] = queryParameters.StartDate != StartDate.Anytime ? queryParameters.StartDate.ToString() : null,
                [nameof(CourseSearchFilters.StartDateFrom)] = queryParameters.IsValidStartDateFrom ? queryParameters.StartDateFrom.ToString(Constants.CourseSearchQueryStringStartDateFormat) : null
            };
            var selectedParameters = string.Join("&", parameters.Where(d => !string.IsNullOrEmpty(d.Value)).Select(kvp => $"{kvp.Key}={HttpUtility.UrlEncode(kvp.Value)}"));
            return $"{path}?{selectedParameters}";
        }
    }
}