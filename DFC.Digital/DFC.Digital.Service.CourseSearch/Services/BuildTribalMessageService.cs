using DFC.Digital.Core;
using DFC.Digital.Data.Model;
using DFC.Digital.Service.CourseSearchProvider.CourseSearchServiceApi;
using System;
using System.Collections.Generic;
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
            if (courseSearchProperties == null)
            {
                throw new ArgumentNullException(nameof(courseSearchProperties));
            }

            var apiRequest = new CourseListInput
            {
                CourseListRequest = new CourseListRequestStructure
                {
                    CourseSearchCriteria = new SearchCriteriaStructure
                    {
                        APIKey = configuration.GetConfig<string>(Constants.CourseSearchApiKey),
                        SubjectKeyword = courseName,
                        EarliestStartDate = null,
                        AttendanceModes = convertTribalCodesService.GetTribalAttendanceModes(string.Join(",",  new List<string>())),
                        StudyModes = convertTribalCodesService.GetTribalStudyModes(string.Join(",", new List<string>())),
                        DFE1619Funded = courseSearchProperties.Filters.Only1619Courses ? "Y" : null,
                        AttendancePatterns = convertTribalCodesService.GetTribalAttendancePatterns(string.Join(",", new List<string>())),
                        ProviderKeyword = courseSearchProperties.Filters.Provider,
                        Distance = courseSearchProperties.Filters.Distance,
                        DistanceSpecified = courseSearchProperties.Filters.DistanceSpecified,
                        Location = courseSearchProperties.Filters.Location
                    },
                    RecordsPerPage = courseSearchProperties.Count.ToString(),
                    PageNo = courseSearchProperties.Page.ToString(),
                    SortBy = GetSortType(courseSearchProperties.OrderedBy),
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

        private static SortType GetSortType(CourseSearchOrderBy courseSearchSortBy)
        {
            switch (courseSearchSortBy)
            {
                case CourseSearchOrderBy.Distance:
                    return SortType.D;
                case CourseSearchOrderBy.StartDate:
                    return SortType.S;
                default:
                case CourseSearchOrderBy.Relevance:
                    return SortType.A;
            }
        }
    }
}
