using DFC.Digital.Core;
using DFC.Digital.Data.Model;
using DFC.Digital.Service.CourseSearchProvider.CourseSearchServiceApi;
using System;
using System.Configuration;
using System.Linq;

namespace DFC.Digital.Service.CourseSearchProvider
{
    public class BuildTribalMessageService : IBuildTribalMessage
    {
        private readonly IConvertTribalCodes convertTribalCodesService;
        private readonly IConfigurationProvider configuration;

        public BuildTribalMessageService(IConvertTribalCodes convertTribalCodesService, IConfigurationProvider configuration)
        {
            this.convertTribalCodesService = convertTribalCodesService;
            this.configuration = configuration;
        }

        public CourseListInput GetCourseSearchInput(string courseName, CourseSearchProperties courseSearchProperties)
        {
            var apiRequest = new CourseListInput
            {
                CourseListRequest = new CourseListRequestStructure
                {
                    CourseSearchCriteria = new SearchCriteriaStructure
                    {
                        APIKey = configuration.GetConfig<string>(Constants.CourseSearchApiKey),
                        SubjectKeyword = courseName,
                        EarliestStartDate = null,
                        AttendanceModes = convertTribalCodesService.GetTribalAttendanceModes(string.Join(",", courseSearchProperties.Filters.Attendance)),
                        StudyModes = convertTribalCodesService.GetTribalStudyModes(string.Join(",", courseSearchProperties.Filters.StudyMode)),
                        DFE1619Funded = courseSearchProperties.Filters.Only1619Courses ? "Y" : null,
                        AttendancePatterns = convertTribalCodesService.GetTribalAttendancePatterns(string.Join(",", courseSearchProperties.Filters.AttendancePattern)),
                        ProviderKeyword = courseSearchProperties.Filters.ProviderKeyword,
                        Distance = courseSearchProperties.Filters.Distance,
                        DistanceSpecified = courseSearchProperties.Filters.DistanceSpecified,
                        Location = courseSearchProperties.Filters.Location
                    },
                    RecordsPerPage = courseSearchProperties.Count.ToString(),
                    PageNo = courseSearchProperties.Page.ToString(),
                    SortBy = GetSortType(courseSearchProperties.OrderBy),
                    SortBySpecified = true
                }
            };

            return apiRequest;
        }

        public CourseDetailInput GetCourseDetailInput(string courseId)
        {
            return new CourseDetailInput
            {
                APIKey = configuration.GetConfig<string>(Constants.CourseSearchApiKey),
                CourseID = new string[] { courseId }
            };
        }

        private SortType GetSortType(CourseSearchOrderBy courseSearchSortBy)
        {
            switch (courseSearchSortBy)
            {
                case CourseSearchOrderBy.Relevance:
                    return SortType.A;
                case CourseSearchOrderBy.Distance:
                    return SortType.D;
                case CourseSearchOrderBy.StartDate:
                    return SortType.S;
                default:
                    return SortType.A;
            }
        }
    }
}
