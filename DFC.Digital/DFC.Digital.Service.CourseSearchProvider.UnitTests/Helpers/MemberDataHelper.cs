using DFC.Digital.Data.Model;
using DFC.Digital.Service.CourseSearchProvider.CourseSearchServiceApi;
using System.Collections.Generic;
using System.Linq;

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
                SearchTerm,
                new CourseSearchProperties
                {
                    OrderedBy = CourseSearchOrderBy.Distance,
                    Count = 20,
                    Page = 3
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
                SearchTerm,
                new CourseSearchProperties
                {
                    OrderedBy = CourseSearchOrderBy.Relevance,
                    Count = 30,
                    Page = 1
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
                SearchTerm,
                new CourseSearchProperties
                {
                    OrderedBy = CourseSearchOrderBy.StartDate,
                    Count = 40,
                    Page = 2
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
                string.Empty,
                CourseSearchConstants.AllAttendanceModes
            };

            yield return new object[]
            {
               "0",
               CourseSearchConstants.AllAttendanceModes
            };

            yield return new object[]
            {
                "1,0",
                CourseSearchConstants.AllAttendanceModes
            };

            yield return new object[]
            {
                "1",
                CourseSearchConstants.ClassAttendanceModes
            };

            yield return new object[]
            {
                "2,3",
                CourseSearchConstants.WorkAttendanceModes.Concat(CourseSearchConstants.DistantAttendanceModes.Concat(CourseSearchConstants.OnlineAttendanceModes))
            };
        }

        public static IEnumerable<object[]> GetTribalStudyModesTestsInput()
        {
            yield return new object[]
            {
                string.Empty,
                null
            };

            yield return new object[]
            {
                "0",
                null
            };

            yield return new object[]
            {
                "0,1",
                null
            };

            yield return new object[]
            {
                "1",
                new[] { CourseSearchConstants.FulltimeStudyMode }
            };

            yield return new object[]
            {
                "2,3",
                new[] { CourseSearchConstants.PartTimeStudyMode, CourseSearchConstants.FlexibleStudyMode }
            };
        }

        public static IEnumerable<object[]> GetTribalQualificationLevelsTestsInput()
        {
            yield return new object[]
            {
                string.Empty,
                CourseSearchConstants.AllQualificationLevels
            };

            yield return new object[]
            {
                "0",
                CourseSearchConstants.AllQualificationLevels
            };

            yield return new object[]
            {
                "1",
                new[] { CourseSearchConstants.EntryLevelQualification }
            };

            yield return new object[]
            {
                "1,3,0",
                CourseSearchConstants.AllQualificationLevels
            };

            yield return new object[]
            {
                "2,3",
                new[] { CourseSearchConstants.Level1Qualification, CourseSearchConstants.Level2Qualification }
            };

            yield return new object[]
            {
                "2,3",
                new[] { CourseSearchConstants.Level1Qualification, CourseSearchConstants.Level2Qualification }
            };

            yield return new object[]
            {
                "3,4,5,6,7,8,9",
                new[] { CourseSearchConstants.Level2Qualification,  CourseSearchConstants.Level3Qualification,  CourseSearchConstants.Level4Qualification,  CourseSearchConstants.Level5Qualification,  CourseSearchConstants.Level6Qualification,  CourseSearchConstants.Level7Qualification,  CourseSearchConstants.Level8Qualification }
            };
        }

        public static IEnumerable<object[]> GetTribalAttendancePatternsTestsInput()
        {
            yield return new object[]
            {
                string.Empty,
                null
            };

            yield return new object[]
            {
                "0",
                null
            };

            yield return new object[]
            {
                "1",
                new[] { CourseSearchConstants.NormalWorkingHoursPattern }
            };

            yield return new object[]
            {
                "2,3",
                new[] { CourseSearchConstants.DayReleaseBlockPattern, CourseSearchConstants.EveningPattern, CourseSearchConstants.WeekendPattern }
            };
        }
    }
}
