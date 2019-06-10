using DFC.Digital.Data.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace DFC.Digital.Web.Sitefinity.CourseModule
{
    public class QueryStringBuilder : IQueryStringBuilder<CourseSearchFilters>
    {
        private const string StartDateFormat = "yyyy-MM-dd";

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
                [nameof(CourseSearchFilters.CourseType)] = courseSearchFilters.CourseType != CourseType.All ? courseSearchFilters.CourseType.ToString() : null,
                [nameof(CourseSearchFilters.Only1619Courses)] = courseSearchFilters.Only1619Courses ? true.ToString() : null,
                [nameof(CourseSearchFilters.Location)] = courseSearchFilters.Location,
                [nameof(CourseSearchFilters.Distance)] = !courseSearchFilters.Distance.Equals(default(float)) ? courseSearchFilters.Distance.ToString(CultureInfo.InvariantCulture) : null,
                [nameof(CourseSearchFilters.CourseHours)] = courseSearchFilters.CourseHours != CourseHours.All ? courseSearchFilters.CourseHours.ToString() : null,
                [nameof(CourseSearchFilters.StartDate)] = courseSearchFilters.StartDate != StartDate.Anytime ? courseSearchFilters.StartDate.ToString() : null,
                [nameof(CourseSearchFilters.StartDateFrom)] = courseSearchFilters.StartDate == StartDate.SelectDateFrom && !courseSearchFilters.StartDateFrom.Equals(DateTime.MinValue) ? courseSearchFilters.StartDateFrom.ToString(StartDateFormat) : null
            };
            var queryParameters = string.Join("&", parameters.Where(d => !string.IsNullOrEmpty(d.Value)).Select(kvp => $"{kvp.Key}={HttpUtility.UrlEncode(kvp.Value)}"));
            return $"{path}?{queryParameters}";
        }
    }
}