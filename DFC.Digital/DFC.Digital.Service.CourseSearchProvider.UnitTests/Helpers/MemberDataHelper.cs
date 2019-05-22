using DFC.Digital.Data.Model;
using DFC.Digital.Service.CourseSearchProvider.CourseSearchServiceApi;
using System.Collections.Generic;
using System.Linq;

namespace DFC.Digital.Service.CourseSearchProvider.UnitTests
{
    public class MemberDataHelper
    {
        private const string NormalWorkingHoursPattern = "AP1";
        private const string DayReleaseBlockPattern = "AP2";
        private const string EveningPattern = "AP3";
        private const string WeekendPattern = "AP5";
        private const string FullTimeStudyMode = "SM1";
        private const string PartTimeStudyMode = "SM2";
        private const string FlexibleStudyMode = "SM4";
        private const string AllQualificationLevels = "LV0,LV1,LV2,LV3,LV4,LV5,LV6,LV7,LV8,LV9,LVNA";
        private const string EntryLevelQualification = "LV0";
        private const string Level1Qualification = "LV1";
        private const string Level2Qualification = "LV2";
        private const string Level3Qualification = "LV3";
        private const string Level4Qualification = "LV4";
        private const string Level5Qualification = "LV5";
        private const string Level6Qualification = "LV6";
        private const string Level7Qualification = "LV7";
        private const string Level8Qualification = "LV8";
        private const string AllAttendanceModes = "AM1,AM2,AM3,AM4,AM5,AM6,AM7,AM8,AM9";
        private const string ClassAttendanceModes = "AM1,AM2,AM4,AM9";
        private const string OnlineAttendanceModes = "AM4,AM7,AM8,AM9";
        private const string DistantAttendanceModes = "AM4,AM5,AM6,AM9";
        private const string WorkAttendanceModes = "AM3,AM4,AM6,AM7,AM9";
        private const string ApiKey = "apiKey";
        private const string SearchTerm = "maths";

        public static IEnumerable<object[]> GetCourseSearchInputTestsInput()
        {
            yield return new object[]
            {
                SearchTerm,
                new CourseSearchProperties
                {
                    OrderBy = CourseSearchOrderBy.Distance,
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
                    OrderBy = CourseSearchOrderBy.Relevance,
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
                    OrderBy = CourseSearchOrderBy.StartDate,
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
                AllAttendanceModes.Split(',')
            };

            yield return new object[]
            {
               "0",
               AllAttendanceModes.Split(',')
            };

            yield return new object[]
            {
                "1,0",
                AllAttendanceModes.Split(',')
            };

            yield return new object[]
            {
                "1",
                ClassAttendanceModes.Split(',')
            };

            yield return new object[]
            {
                "2,3",
                WorkAttendanceModes.Split(',').Concat(DistantAttendanceModes.Split(',')).Concat(OnlineAttendanceModes.Split(','))
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
                new[] { FullTimeStudyMode }
            };

            yield return new object[]
            {
                "2,3",
                new[] { PartTimeStudyMode, FlexibleStudyMode }
            };
        }

        public static IEnumerable<object[]> GetTribalQualificationLevelsTestsInput()
        {
            yield return new object[]
            {
                string.Empty,
                AllQualificationLevels.Split(',')
            };

            yield return new object[]
            {
                "0",
                AllQualificationLevels.Split(',')
            };

            yield return new object[]
            {
                "1",
                new[] { EntryLevelQualification }
            };

            yield return new object[]
            {
                "2,3",
                new[] { Level1Qualification, Level2Qualification }
            };

            yield return new object[]
            {
                "2,3",
                new[] { Level1Qualification, Level2Qualification }
            };

            yield return new object[]
            {
                "3,4,5,6,7,8,9",
                new[] { Level2Qualification, Level3Qualification, Level4Qualification, Level5Qualification, Level6Qualification, Level7Qualification, Level8Qualification }
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
                new[] { NormalWorkingHoursPattern }
            };

            yield return new object[]
            {
                "2,3",
                new[] { DayReleaseBlockPattern, EveningPattern, WeekendPattern }
            };
        }
    }
}
