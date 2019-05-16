using DFC.Digital.Data.Model;
using DFC.Digital.Web.Sitefinity.CourseModule.Mvc.Controllers;
using DFC.Digital.Web.Sitefinity.CourseModule.Mvc.Models;
using System.Collections.Generic;

namespace DFC.Digital.Web.Sitefinity.CourseModule.UnitTests.Helpers
{
    public class HelperSearchResultsData
    {
        private const string SearchTerm = "maths";
        private const int PageNumber = 1;
        private const int RecordsPerPage = 20;
        private const string Attendance = "attendance";
        private const string StudyMode = "studyMode";
        private const string QualificationLevel = "qualificationLevel";
        private const string ValidDistanceInput = "10";
        private const string InValidDistanceInput = "Distance";
        private const string Dfe1619Funded = "1619";
        private const string Pattern = "pattern";
        private const string Location = "leeds";
        private const string LocationPostCode = "cv12wt";
        private const string Provider = "ucla";
        private const string DistanceSortBy = "distance";
        private const string StartDateSortBy = "startDate";
        private const string RelevanceSortBy = "relevance";
        private const string LocationRegex =
            @"([Gg][Ii][Rr] 0[Aa]{2})|((([A-Za-z][0-9]{1,2})|(([A-Za-z][A-Ha-hJ-Yj-y][0-9]{1,2})|(([A-Za-z][0-9][A-Za-z])|([A-Za-z][A-Ha-hJ-Yj-y][0-9][A-Za-z]?))))\s?[0-9][A-Za-z]{2})";

        private const CourseSearchSortBy DistanceSort = CourseSearchSortBy.Distance;
        private const CourseSearchSortBy StartDateSort = CourseSearchSortBy.StartDate;
        private const CourseSearchSortBy RelevanceSort = CourseSearchSortBy.Relevance;
        private const float ValidDistance = 10F;
        private const float InvalidDistance = default(float);

        private static readonly TrainingCourseResultsViewModel ValidCourseResultsViewModel = new TrainingCourseResultsViewModel
        {
            SearchTerm = nameof(TrainingCourseResultsViewModel.SearchTerm)
        };

        private static readonly TrainingCourseResultsViewModel InvalidCourseResultsViewModel = new TrainingCourseResultsViewModel
        {
            SearchTerm = string.Empty
        };

        private static readonly CourseSearchResponse ValidCourseSearchResponse = new CourseSearchResponse
        {
            Courses = new List<Course> { new Course { Title = nameof(Course.Title) } }
        };

        private static readonly CourseSearchResponse ZeroResultsCourseSearchResponse = new CourseSearchResponse
        {
            Courses = new List<Course>()
        };

        public static IEnumerable<object[]> IndexPostTestsInput()
        {
            yield return new object[]
            {
                nameof(TrainingCoursesController.FilterCourseByText),
                nameof(TrainingCoursesController.PageTitle),
                nameof(TrainingCoursesController.CourseSearchResultsPage),
                nameof(TrainingCoursesController.CourseDetailsPage),
                ValidCourseResultsViewModel
            };
            yield return new object[]
            {
                nameof(TrainingCoursesController.FilterCourseByText),
                nameof(TrainingCoursesController.PageTitle),
                nameof(TrainingCoursesController.CourseSearchResultsPage),
                nameof(TrainingCoursesController.CourseDetailsPage),
                ValidCourseResultsViewModel
            };
            yield return new object[]
            {
                nameof(TrainingCoursesController.FilterCourseByText),
                nameof(TrainingCoursesController.PageTitle),
                nameof(TrainingCoursesController.CourseSearchResultsPage),
                nameof(TrainingCoursesController.CourseDetailsPage),
                InvalidCourseResultsViewModel
            };
        }

        public static IEnumerable<object[]> IndexTestsInput()
        {
            yield return new object[]
            {
                nameof(TrainingCourseResultsViewModel.SearchTerm),
                nameof(TrainingCoursesController.FilterCourseByText),
                nameof(TrainingCoursesController.PageTitle),
                nameof(TrainingCoursesController.CourseSearchResultsPage),
                nameof(TrainingCoursesController.CourseDetailsPage),
                ValidCourseSearchResponse
            };

            yield return new object[]
            {
                nameof(TrainingCourseResultsViewModel.SearchTerm),
                nameof(TrainingCoursesController.FilterCourseByText),
                nameof(TrainingCoursesController.PageTitle),
                nameof(TrainingCoursesController.CourseSearchResultsPage),
                nameof(TrainingCoursesController.CourseDetailsPage),
                ZeroResultsCourseSearchResponse
            };

            yield return new object[]
            {
                null,
                nameof(TrainingCoursesController.FilterCourseByText),
                nameof(TrainingCoursesController.PageTitle),
                nameof(TrainingCoursesController.CourseSearchResultsPage),
                nameof(TrainingCoursesController.CourseDetailsPage),
                ZeroResultsCourseSearchResponse
            };
        }

        public static IEnumerable<object[]> GetUrlEncodedStringTestsInput()
        {
            yield return new object[]
            {
                "/courses-search-results?searchTerm=math<script>",
                "%2fcourses-search-results%3fsearchTerm%3dmath%3cscript%3e"
            };

            yield return new object[]
            {
                "/courses-search-results?searchTerm=language&provider=london",
                "%2fcourses-search-results%3fsearchTerm%3dlanguage%26provider%3dlondon"
            };

            yield return new object[]
            {
                "courses-search-results?searchTerm=language&provider=london&dfe1619Funded=1619&location=leeds&startDate=anytime&studymode=1&page=1",
                "courses-search-results%3fsearchTerm%3dlanguage%26provider%3dlondon%26dfe1619Funded%3d1619%26location%3dleeds%26StartDate%3dAnytime%26studymode%3d1%26page%3d1"
            };

            yield return new object[]
            {
                "pageurl/itemid",
                "pageurl%2fitemid"
            };

            yield return new object[]
            {
                null,
                string.Empty
            };
        }

        public static IEnumerable<object[]> BuildSearchRedirectPathAndQueryStringTestsInput()
        {
            yield return new object[]
            {
                "/courses-search-results",
                new CourseLandingViewModel(),
               LocationRegex,
                "/courses-search-results?"
            };

            yield return new object[]
            {
                "/courses-search-results",
                new CourseLandingViewModel
                {
                    SearchTerm = SearchTerm,
                    ProviderKeyword = Provider
                },
                LocationRegex,
                "/courses-search-results?searchTerm=maths&provider=ucla&startDate=anytime"
            };

            yield return new object[]
            {
                "/courses-search-results",
                new CourseLandingViewModel
                {
                    ProviderKeyword = Provider
                },
                LocationRegex,
                "/courses-search-results?provider=ucla&startDate=anytime"
            };

            yield return new object[]
            {
                "/courses-search-results",
                new CourseLandingViewModel
                {
                    SearchTerm = SearchTerm
                },
                LocationRegex,
                "/courses-search-results?searchTerm=maths&startDate=anytime"
            };

            yield return new object[]
            {
                "/courses-search-results",
                new CourseLandingViewModel
                {
                    SearchTerm = SearchTerm,
                        QualificationLevel = nameof(CourseLandingViewModel.QualificationLevel),
                        Location = Location,
                        Distance = "10",
                        Dfe1619Funded = Dfe1619Funded
                },
                LocationRegex,
                "/courses-search-results?searchTerm=maths&qualificationlevel=QualificationLevel&location=leeds&dfe1619Funded=1619&startDate=anytime"
            };

            yield return new object[]
            {
                "/courses-search-results",
                new CourseLandingViewModel
                {
                    SearchTerm = SearchTerm,
                    QualificationLevel = nameof(CourseLandingViewModel.QualificationLevel),
                    Location = LocationPostCode,
                    Distance = "10",
                    Dfe1619Funded = Dfe1619Funded
                },
                LocationRegex,
                "/courses-search-results?searchTerm=maths&qualificationlevel=QualificationLevel&location=cv12wt&dfe1619Funded=1619&startDate=anytime&distance=10"
            };
        }

        public static IEnumerable<object[]> BuildRedirectPathAndQueryStringTestsInput()
        {
            yield return new object[]
            {
                "/courses-search-results",
                new TrainingCourseResultsViewModel(),
               LocationRegex,
                "/courses-search-results?"
            };

            yield return new object[]
            {
                "/courses-search-results",
                new TrainingCourseResultsViewModel
                {
                    SearchTerm = SearchTerm,
                    CourseFiltersModel = new CourseFiltersModel
                    {
                        ProviderKeyword = Provider
                    }
                },
                LocationRegex,
                "/courses-search-results?searchTerm=maths&provider=ucla&startDate=anytime&page=1"
            };

            yield return new object[]
            {
                "/courses-search-results",
                new TrainingCourseResultsViewModel
                {
                    CourseFiltersModel = new CourseFiltersModel
                    {
                        ProviderKeyword = Provider
                    }
                },
                LocationRegex,
                "/courses-search-results?provider=ucla&startDate=anytime&page=1"
            };

            yield return new object[]
            {
                "/courses-search-results",
                new TrainingCourseResultsViewModel
                {
                    SearchTerm = SearchTerm
                },
                LocationRegex,
                "/courses-search-results?searchTerm=maths&startDate=anytime&page=1"
            };

            yield return new object[]
            {
                "/courses-search-results",
                new TrainingCourseResultsViewModel
                {
                    SearchTerm = SearchTerm,
                    CourseFiltersModel = new CourseFiltersModel
                    {
                        AttendanceMode = new List<string> { "attendancemode1", "attendancemode2" },
                        QualificationLevel = new List<string> { "qual1", "qual2" },
                        AgeSuitability = "1619",
                        Location = Location,
                        AttendancePattern = new List<string> { "pattern", "pattern2" },
                        StudyMode = new List<string> { "studymode", "studymode1" },
                        Distance = "10"
                    }
                },
                LocationRegex,
                "/courses-search-results?searchTerm=maths&attendance=attendancemode1,attendancemode2&qualificationlevel=qual1,qual2&dfe1619Funded=1619&location=leeds&pattern=pattern,pattern2&startDate=anytime&studymode=studymode,studymode1&page=1"
            };

            yield return new object[]
            {
                "/courses-search-results",
                new TrainingCourseResultsViewModel
                {
                    SearchTerm = SearchTerm,
                    CourseFiltersModel = new CourseFiltersModel
                    {
                        AttendanceMode = new List<string> { "attendancemode1", "attendancemode2" },
                        QualificationLevel = new List<string> { "qual1", "qual2" },
                        AgeSuitability = "1619",
                        Location = LocationPostCode,
                        AttendancePattern = new List<string> { "pattern", "pattern2" },
                        StudyMode = new List<string> { "studymode", "studymode1" },
                        Distance = "10"
                    }
                },
                LocationRegex,
                "/courses-search-results?searchTerm=maths&attendance=attendancemode1,attendancemode2&qualificationlevel=qual1,qual2&dfe1619Funded=1619&location=cv12wt&pattern=pattern,pattern2&startDate=anytime&distance=10&studymode=studymode,studymode1&page=1"
            };
        }

        public static IEnumerable<object[]> GetCourseSearchRequestTestsInput()
        {
            yield return new object[]
            {
                SearchTerm,
                RecordsPerPage,
                Attendance,
                StudyMode,
                QualificationLevel,
                ValidDistanceInput,
                Dfe1619Funded,
                Pattern,
                Location,
                DistanceSortBy,
                Provider,
                PageNumber,
                new CourseSearchRequest
                {
                    SearchTerm = SearchTerm,
                    RecordsPerPage = RecordsPerPage,
                    PageNumber = PageNumber,
                    Attendance = Attendance,
                    StudyMode = StudyMode,
                    QualificationLevel = QualificationLevel,
                    Dfe1619Funded = Dfe1619Funded,
                    Distance = ValidDistance,
                    AttendancePattern = Pattern,
                    Location = Location,
                    CourseSearchSortBy = DistanceSort,
                    ProviderKeyword = Provider
                }
            };

            yield return new object[]
            {
                SearchTerm,
                RecordsPerPage,
                Attendance,
                StudyMode,
                QualificationLevel,
                InValidDistanceInput,
                Dfe1619Funded,
                Pattern,
                Location,
                RelevanceSortBy,
                Provider,
                PageNumber,
                new CourseSearchRequest
                {
                    SearchTerm = SearchTerm,
                    RecordsPerPage = RecordsPerPage,
                    PageNumber = PageNumber,
                    Attendance = Attendance,
                    StudyMode = StudyMode,
                    QualificationLevel = QualificationLevel,
                    Dfe1619Funded = Dfe1619Funded,
                    Distance = InvalidDistance,
                    AttendancePattern = Pattern,
                    Location = Location,
                    CourseSearchSortBy = RelevanceSort,
                    ProviderKeyword = Provider
                }
            };

            yield return new object[]
            {
                SearchTerm,
                RecordsPerPage,
                Attendance,
                StudyMode,
                QualificationLevel,
                InValidDistanceInput,
                Dfe1619Funded,
                Pattern,
                Location,
                string.Empty,
                Provider,
                PageNumber,
                new CourseSearchRequest
                {
                    SearchTerm = SearchTerm,
                    RecordsPerPage = RecordsPerPage,
                    PageNumber = PageNumber,
                    Attendance = Attendance,
                    StudyMode = StudyMode,
                    QualificationLevel = QualificationLevel,
                    Dfe1619Funded = Dfe1619Funded,
                    Distance = InvalidDistance,
                    AttendancePattern = Pattern,
                    Location = Location,
                    CourseSearchSortBy = RelevanceSort,
                    ProviderKeyword = Provider
                }
            };

            yield return new object[]
            {
                SearchTerm,
                RecordsPerPage,
                Attendance,
                StudyMode,
                QualificationLevel,
                InValidDistanceInput,
                Dfe1619Funded,
                Pattern,
                Location,
                StartDateSortBy,
                Provider,
                PageNumber,
                new CourseSearchRequest
                {
                    SearchTerm = SearchTerm,
                    RecordsPerPage = RecordsPerPage,
                    PageNumber = PageNumber,
                    Attendance = Attendance,
                    StudyMode = StudyMode,
                    QualificationLevel = QualificationLevel,
                    Dfe1619Funded = Dfe1619Funded,
                    Distance = InvalidDistance,
                    AttendancePattern = Pattern,
                    Location = Location,
                    CourseSearchSortBy = StartDateSort,
                    ProviderKeyword = Provider
                }
            };
        }
    }
}
