using DFC.Digital.Core;
using DFC.Digital.Data.Model;
using DFC.Digital.Service.CourseSearchProvider.CourseSearchServiceApi;
using System;
using System.Configuration;

namespace DFC.Digital.Service.CourseSearchProvider
{
    public class BuildTribalMessageService : IBuildTribalMessage
    {
        private readonly IConvertTribalEnums convertTribalEnums;

        public BuildTribalMessageService(IConvertTribalEnums convertTribalEnums)
        {
            this.convertTribalEnums = convertTribalEnums;
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
                        EarliestStartDate = DateTime.Now.ToString("yyyy-MM-dd"),
                        AttendanceModes = convertTribalEnums.GetTribalAttendanceModes(request.Attendance),
                        StudyModes = convertTribalEnums.GetTribalStudyModes(request.StudyMode),
                        DFE1619Funded = request.Dfe1619Funded.Equals("1619") ? "Y" : null,
                        AttendancePatterns = convertTribalEnums.GetTribalAttendancePatterns(request.AttendancePattern),
                        QualificationLevels = convertTribalEnums.GetTribalQualificationLevels(request.QualificationLevel),
                        ProviderKeyword = request.ProviderKeyword,
                        Distance = request.Distance,
                        DistanceSpecified = request.DistanceSpecified,
                        Location = request.Location
                    },
                    RecordsPerPage = request.RecordsPerPage.ToString(),
                    PageNo = request.PageNumber.ToString(),
                    SortBy = SortType.A,
                    SortBySpecified = true
                }
            };

            return apiRequest;
        }
    }
}
