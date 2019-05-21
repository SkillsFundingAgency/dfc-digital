using DFC.Digital.Core;
using DFC.Digital.Data.Model;
using DFC.Digital.Service.CourseSearchProvider.CourseSearchServiceApi;
using System;
using System.Configuration;

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

        public CourseListInput GetCourseSearchInput(string courseName, CourseSearchProperties courseSearchProperties, CourseSearchFilters courseSearchFilters)
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
                        AttendanceModes = convertTribalCodesService.GetTribalAttendanceModes(courseSearchFilters.Attendance),
                        StudyModes = convertTribalCodesService.GetTribalStudyModes(courseSearchFilters.StudyMode),
                        DFE1619Funded = courseSearchFilters.Only1619Courses ? "Y" : null,
                        AttendancePatterns = convertTribalCodesService.GetTribalAttendancePatterns(courseSearchFilters.AttendancePattern),
                        ProviderKeyword = courseSearchFilters.ProviderKeyword,
                        Distance = courseSearchFilters.Distance,
                        DistanceSpecified = courseSearchFilters.DistanceSpecified,
                        Location = courseSearchFilters.Location
                    },
                    RecordsPerPage = courseSearchProperties.RecordsPerPage.ToString(),
                    PageNo = courseSearchProperties.PageNumber.ToString(),
                    SortBy = GetSortType(courseSearchProperties.CourseSearchSortBy),
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
