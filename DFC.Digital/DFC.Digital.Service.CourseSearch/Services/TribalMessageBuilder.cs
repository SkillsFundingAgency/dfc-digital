using DFC.Digital.Core;
using DFC.Digital.Data.Model;
using DFC.Digital.Service.CourseSearchProvider.CourseSearchServiceApi;
using System;

namespace DFC.Digital.Service.CourseSearchProvider
{
    public class TribalMessageBuilder : ITribalMessageBuilder
    {
        private readonly ITribalCodesConverter convertTribalCodes;
        private readonly IConfigurationProvider configuration;
        private readonly ICourseBusinessRulesProcessor courseBusinessRules;

        public TribalMessageBuilder(ITribalCodesConverter convertTribalCodes, IConfigurationProvider configuration, ICourseBusinessRulesProcessor courseBusinessRules)
        {
            this.convertTribalCodes = convertTribalCodes;
            this.configuration = configuration;
            this.courseBusinessRules = courseBusinessRules;
        }

        public CourseListInput GetCourseSearchInput(CourseSearchProperties courseSearchProperties)
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
                        SubjectKeyword = courseSearchProperties.Filters.SearchTerm,
                        EarliestStartDate = courseBusinessRules.GetEarliestStartDate(courseSearchProperties.Filters.StartDate, courseSearchProperties.Filters.StartDateFrom),
                        AttendanceModes = convertTribalCodes.GetTribalAttendanceModes(courseSearchProperties.Filters.CourseType),
                        StudyModes = convertTribalCodes.GetTribalStudyModes(courseSearchProperties.Filters.CourseHours),
                        DFE1619Funded = courseSearchProperties.Filters.Only1619Courses ? "Y" : null,
                        ProviderKeyword = courseSearchProperties.Filters.Provider,
                        Distance = courseSearchProperties.Filters.DistanceSpecified ? courseSearchProperties.Filters.Distance : default(float),
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
