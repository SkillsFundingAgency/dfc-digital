using DFC.Digital.Core;
using DFC.Digital.Data.Model;
using DFC.Digital.Service.CourseSearchProvider.CourseSearchServiceApi;
using System;
using System.Collections.Generic;

namespace DFC.Digital.Service.CourseSearchProvider.UnitTests
{
    public class MemberDataHelper
    {
        private const string ApiKey = "apiKey";
        private const string SearchTerm = "maths";

        public static IEnumerable<object[]> GetCourseSearchInputTestsInput()
        {
            yield return new object[]
            {
                new CourseSearchProperties
                {
                    OrderedBy = CourseSearchOrderBy.Distance,
                    Count = 20,
                    Page = 3,
                    Filters = new CourseSearchFilters
                    {
                        SearchTerm = SearchTerm
                    }
                },
                new CourseListInput
                {
                    CourseListRequest = new CourseListRequestStructure
                    {
                        CourseSearchCriteria = new SearchCriteriaStructure
                        {
                            APIKey = ApiKey,
                            SubjectKeyword = SearchTerm
                        },
                        SortBy = SortType.D,
                        RecordsPerPage = "20",
                        PageNo = "3",
                        SortBySpecified = true
                    }
                }
            };

            yield return new object[]
            {
                new CourseSearchProperties
                {
                    OrderedBy = CourseSearchOrderBy.Relevance,
                    Count = 30,
                    Page = 1,
                    Filters = new CourseSearchFilters
                    {
                    SearchTerm = SearchTerm
                }
                },
                new CourseListInput
                {
                    CourseListRequest = new CourseListRequestStructure
                    {
                        CourseSearchCriteria = new SearchCriteriaStructure
                        {
                            APIKey = ApiKey,
                            SubjectKeyword = SearchTerm
                        },
                        SortBy = SortType.A,
                        RecordsPerPage = "30",
                        PageNo = "1",
                        SortBySpecified = true
                    }
                }
            };

            yield return new object[]
            {
                new CourseSearchProperties
                {
                    OrderedBy = CourseSearchOrderBy.StartDate,
                    Count = 40,
                    Page = 2,
                    Filters = new CourseSearchFilters
                    {
                        SearchTerm = SearchTerm
                    }
                },
                new CourseListInput
                {
                    CourseListRequest = new CourseListRequestStructure
                    {
                        CourseSearchCriteria = new SearchCriteriaStructure
                        {
                            APIKey = ApiKey,
                            SubjectKeyword = SearchTerm
                        },
                        SortBy = SortType.S,
                        RecordsPerPage = "40",
                        PageNo = "2",
                        SortBySpecified = true
                    }
                }
            };
        }

        public static IEnumerable<object[]> GetCourseDetailInputTestsInput()
        {
            yield return new object[]
            {
                "2525252",
                new CourseDetailInput
                {
                    APIKey = ApiKey,
                    CourseID = new[] { "2525252" }
                }
            };

            yield return new object[]
            {
                "445545454",
                new CourseDetailInput
                {
                    APIKey = ApiKey,
                    CourseID = new[] { "445545454" }
                }
            };
        }

        public static IEnumerable<object[]> GetTribalAttendanceModesTestInput()
        {
            yield return new object[]
            {
                CourseType.All,
                CourseSearchConstants.AllAttendanceModes()
            };

            yield return new object[]
            {
                CourseType.ClassroomBased,
                CourseSearchConstants.ClassAttendanceModes()
            };

            yield return new object[]
            {
                CourseType.WorkBased,
                CourseSearchConstants.WorkAttendanceModes()
            };

            yield return new object[]
            {
                CourseType.Online,
                 CourseSearchConstants.OnlineAttendanceModes()
            };
        }

        public static IEnumerable<object[]> GetTribalStudyModesTestsInput()
        {
            yield return new object[]
            {
                CourseHours.All,
               null
            };

            yield return new object[]
            {
                CourseHours.Flexible,
               new[] { CourseSearchConstants.FlexibleStudyMode }
            };

            yield return new object[]
            {
                CourseHours.Fulltime,
                new[] { CourseSearchConstants.FulltimeStudyMode }
            };

            yield return new object[]
            {
                CourseHours.PartTime,
                new[] { CourseSearchConstants.PartTimeStudyMode }
            };
        }

        public static IEnumerable<object[]> GetEarliestStartDateTestsInput()
        {
            yield return new object[]
            {
                StartDate.Anytime,
                DateTime.Now,
                null
            };

            yield return new object[]
            {
                StartDate.FromToday,
                DateTime.Now,
                DateTime.Now.ToString(Constants.CourseApiDateFormat)
            };

            yield return new object[]
            {
                StartDate.SelectDateFrom,
                DateTime.Now,
                DateTime.Now.ToString(Constants.CourseApiDateFormat)
            };

            yield return new object[]
            {
                StartDate.SelectDateFrom,
                DateTime.Now.AddDays(70),
                DateTime.Now.AddDays(70).ToString(Constants.CourseApiDateFormat)
            };

            yield return new object[]
            {
                StartDate.SelectDateFrom,
                DateTime.Now.AddYears(5),
                DateTime.Now.AddYears(5).ToString(Constants.CourseApiDateFormat)
            };

            yield return new object[]
            {
                StartDate.SelectDateFrom,
                DateTime.Now.AddYears(2),
                DateTime.Now.AddYears(2).ToString(Constants.CourseApiDateFormat)
            };
            yield return new object[]
            {
                StartDate.SelectDateFrom,
                DateTime.Now.AddYears(-2),
                DateTime.Now.AddYears(-1).ToString(Constants.CourseApiDateFormat)
            };
        }
    }
}
