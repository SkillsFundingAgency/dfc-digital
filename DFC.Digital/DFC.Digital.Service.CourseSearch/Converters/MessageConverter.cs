using DFC.Digital.Core;
using DFC.Digital.Data.Model;
using DFC.Digital.Service.CourseSearchProvider.CourseSearchServiceApi;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace DFC.Digital.Service.CourseSearchProvider
{
    internal static class MessageConverter
    {
        internal static CourseListInput GetCourseListInput(string request)
        {
            var apiRequest = new CourseListInput
            {
                CourseListRequest = new CourseListRequestStructure
                {
                    CourseSearchCriteria = new SearchCriteriaStructure
                    {
                        APIKey = ConfigurationManager.AppSettings[Constants.CourseSearchApiKey],
                        SubjectKeyword = request,
                        EarliestStartDate = DateTime.Now.ToString("yyyy-MM-dd"),
                        AttendanceModes = Convert.ToString(ConfigurationManager.AppSettings[Constants.CourseSearchAttendanceModes])?.Split(',')
                    },
                    RecordsPerPage = ConfigurationManager.AppSettings[Constants.CourseSearchPageSize],
                    PageNo = "1",
                    SortBy = SortType.A,
                    SortBySpecified = true
                }
            };

            return apiRequest;
        }

        internal static IEnumerable<Course> ConvertToCourse(this CourseListOutput apiResult)
        {
            var result = apiResult?.CourseListResponse?.CourseDetails?.Select(c =>
                new Course
                {
                    Title = c.Course.CourseTitle,
                    Location = (c.Opportunity.Item as VenueInfo)?.VenueAddress.Town,
                    ProviderName = c.Provider.ProviderName,
                    StartDate = Convert.ToDateTime(c.Opportunity.StartDate.Item),
                    CourseId = c.Course.CourseID
                });

            return result ?? Enumerable.Empty<Course>();
        }
    }
}