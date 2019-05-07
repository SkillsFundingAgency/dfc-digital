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

        public BuildTribalMessageService(IConvertTribalCodes convertTribalCodesService)
        {
            this.convertTribalCodesService = convertTribalCodesService;
        }

        public CourseListInput GetCourseSearchInput(CourseSearchRequest request)
        {
            var apiRequest = new CourseListInput
            {
                CourseListRequest = new CourseListRequestStructure
                {
                    CourseSearchCriteria = new SearchCriteriaStructure
                    {
                        APIKey = ConfigurationManager.AppSettings[Constants.CourseSearchApiKey],
                        SubjectKeyword = request.SearchTerm,
                        EarliestStartDate = null,
                        AttendanceModes = convertTribalCodesService.GetTribalAttendanceModes(request.Attendance),
                        StudyModes = convertTribalCodesService.GetTribalStudyModes(request.StudyMode),
                        DFE1619Funded = !string.IsNullOrWhiteSpace(request.Dfe1619Funded) && request.Dfe1619Funded.Equals("1619") ? "Y" : null,
                        AttendancePatterns = convertTribalCodesService.GetTribalAttendancePatterns(request.AttendancePattern),
                        QualificationLevels = convertTribalCodesService.GetTribalQualificationLevels(request.QualificationLevel),
                        ProviderKeyword = request.ProviderKeyword,
                        Distance = request.Distance,
                        DistanceSpecified = request.DistanceSpecified,
                        Location = request.Location
                    },
                    RecordsPerPage = request.RecordsPerPage.ToString(),
                    PageNo = request.PageNumber.ToString(),
                    SortBy = GetSortType(request.CourseSearchSortBy),
                    SortBySpecified = true
                }
            };

            return apiRequest;
        }

        public CourseDetailInput GetCourseDetailInput(string courseId)
        {
            return new CourseDetailInput
            {
                APIKey = ConfigurationManager.AppSettings[Constants.CourseSearchApiKey],
                CourseID = new string[] { courseId }
            };
        }

        private SortType GetSortType(CourseSearchSortBy courseSearchSortBy)
        {
            switch (courseSearchSortBy)
            {
                case CourseSearchSortBy.Relevance:
                    return SortType.A;
                case CourseSearchSortBy.Distance:
                    return SortType.D;
                case CourseSearchSortBy.StartDate:
                    return SortType.S;
                default:
                    return SortType.A;
            }
        }
    }
}
