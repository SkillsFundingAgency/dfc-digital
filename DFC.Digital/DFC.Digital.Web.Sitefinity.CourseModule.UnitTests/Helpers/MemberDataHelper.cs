using DFC.Digital.Data.Model;
using DFC.Digital.Web.Sitefinity.CourseModule.Mvc.Controllers;
using DFC.Digital.Web.Sitefinity.CourseModule.Mvc.Models;
using System;
using System.Collections.Generic;

namespace DFC.Digital.Web.Sitefinity.CourseModule.UnitTests.Helpers
{
    public class MemberDataHelper : AssertionHelper
    {
        private const string SearchTerm = "maths";
        private const string SearchPageUrl = "/courses-search-results";
        private const string PathQuery = "/pathQuery";
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

        private const CourseSearchOrderBy DistanceSort = CourseSearchOrderBy.Distance;
        private const CourseSearchOrderBy StartDateSort = CourseSearchOrderBy.StartDate;
        private const CourseSearchOrderBy RelevanceSort = CourseSearchOrderBy.Relevance;
        private const float ValidDistance = 10F;
        private const float InvalidDistance = default(float);

        private static readonly TrainingCourseResultsViewModel ValidCourseResultsViewModel =
            new TrainingCourseResultsViewModel
            {
                SearchTerm = nameof(TrainingCourseResultsViewModel.SearchTerm)
            };

        private static readonly TrainingCourseResultsViewModel InvalidCourseResultsViewModel =
            new TrainingCourseResultsViewModel
            {
                SearchTerm = string.Empty
            };

        private static readonly CourseSearchResult ValidCourseSearchResponse = new CourseSearchResult
        {
            Courses = GetCourses(2),
            CurrentPage = 1,
            TotalResultCount = 2,
            TotalPages = 1
        };

        private static readonly CourseSearchResult ZeroResultsCourseSearchResponse = new CourseSearchResult
        {
            Courses = new List<Course>()
        };

        private static readonly CourseSearchResult MultiPageResultsCourseSearchResponse = new CourseSearchResult
        {
            Courses = GetCourses(20),
            CurrentPage = 2,
            TotalPages = 3,
            TotalResultCount = 60
        };

        private static readonly CourseSearchResult TwoPageResultsCourseSearchResponse = new CourseSearchResult
        {
            Courses = GetCourses(20),
            CurrentPage = 1,
            TotalPages = 2,
            TotalResultCount = 40
        };

        public static IEnumerable<object[]> Dfc7055SearchResultsViewTestsInput()
        {
            yield return new object[]
            {
                5,
                "page title 1",
                "no courses here"
            };

            yield return new object[]
            {
                5,
                "page title 6",
                "no courses found"
            };

            yield return new object[]
            {
                5,
                "page title 3",
                "no courses at all"
            };
        }

        public static IEnumerable<object[]> Dfc7055CourseDetailsViewTestsInput()
        {
            yield return new object[]
            {
                new Course
                {
                    Title = nameof(Course.Title),
                    CourseId = nameof(Course.CourseId),
                    CourseUrl = $"{SearchPageUrl}/{nameof(Course.CourseId)}",
                    Location = nameof(Course.Location),
                    StartDateLabel = nameof(Course.StartDateLabel),
                    QualificationLevel = QualificationLevel
                },
                "provider name",
                "advanced loan provider:",
                "location:",
                "start date -"
            };

            yield return new object[]
            {
                new Course
                {
                    Title = nameof(Course.Title),
                    CourseId = nameof(Course.CourseId),
                    CourseUrl = $"{SearchPageUrl}/{nameof(Course.CourseId)}",
                    StartDateLabel = nameof(Course.StartDateLabel),
                    QualificationLevel = "unknown"
                },
                "provider name",
                "advanced loan provider:",
                "location:",
                "start date -"
            };

            yield return new object[]
            {
                new Course
                {
                    Title = nameof(Course.Title),
                    CourseId = nameof(Course.CourseId),
                    CourseUrl = $"{SearchPageUrl}/{nameof(Course.CourseId)}",
                    Location = nameof(Course.Location),
                    QualificationLevel = nameof(Course.QualificationLevel)
                },
                "provider name",
                "advanced loan provider:",
                "location:",
                "start date -"
            };
        }

        public static IEnumerable<object[]> Dfc7055PaginationViewTestsInput()
        {
            //bool hasNextPage, bool hasPreviousPage, string nextPageText, string previousPageText, string pathQuery
            yield return new object[]
            {
                true,
                true,
                "3 of 3",
                "1 of 3",
                SearchPageUrl
            };

            yield return new object[]
            {
                false,
                false,
                string.Empty,
                string.Empty,
                SearchPageUrl
            };

            yield return new object[]
            {
                true,
                false,
                "2 of 2",
                string.Empty,
                SearchPageUrl
            };

            yield return new object[]
            {
                false,
                true,
                string.Empty,
                "1 of 2",
                SearchPageUrl
            };
        }

        public static IEnumerable<object[]> GetFilterSelectItemsTestsInput()
        {
            yield return new object[]
            {
                "property1",
                new List<string> { "Show All: 0", "Full Time: 1" },
                "1",
                new List<SelectItem>
                {
                    new SelectItem
                    {
                        Label = "Show All",
                        Value = "0",
                        Checked = string.Empty,
                        Name = "property1",
                        Id = "property10"
                    },
                    new SelectItem
                    {
                        Label = "Full Time",
                        Value = "1",
                        Checked = "checked",
                        Name = "property1",
                        Id = "property11"
                    }
                }
            };

            yield return new object[]
            {
                "property2",
                new List<string> { "All: 0", "Entry Level: 1", "Level 1: 2" },
                "2",
                new List<SelectItem>
                {
                    new SelectItem
                    {
                        Label = "All",
                        Value = "0",
                        Checked = string.Empty,
                        Name = "property2",
                        Id = "property20"
                    },
                    new SelectItem
                    {
                        Label = "Entry Level",
                        Value = "1",
                        Checked = string.Empty,
                        Name = "property2",
                        Id = "property21"
                    },
                    new SelectItem
                    {
                    Label = "Level 1",
                    Value = "2",
                    Checked = "checked",
                    Name = "property2",
                    Id = "property22"
                }
                }
            };
        }

        public static IEnumerable<object[]> GetOrderByLinksTestsInput()
        {
            var sortbyUrl = $"{SearchPageUrl}&sortby=distance";
            yield return new object[]
            {
                sortbyUrl,
                CourseSearchOrderBy.Distance,
                new OrderByLinks
                {
                    CourseSearchSortBy = CourseSearchOrderBy.Distance,
                    OrderByRelevanceUrl = new Uri($"{SearchPageUrl}&sortby=relevance", UriKind.RelativeOrAbsolute),
                    OrderByDistanceUrl = new Uri($"{SearchPageUrl}&sortby=distance", UriKind.RelativeOrAbsolute),
                    OrderByStartDateUrl = new Uri($"{SearchPageUrl}&sortby=startdate", UriKind.RelativeOrAbsolute)
                }
            };
            yield return new object[]
            {
                SearchPageUrl,
                CourseSearchOrderBy.Relevance,
                new OrderByLinks
                {
                    CourseSearchSortBy = CourseSearchOrderBy.Relevance,
                    OrderByRelevanceUrl = new Uri($"{SearchPageUrl}&sortby=relevance", UriKind.RelativeOrAbsolute),
                    OrderByDistanceUrl = new Uri($"{SearchPageUrl}&sortby=distance", UriKind.RelativeOrAbsolute),
                    OrderByStartDateUrl = new Uri($"{SearchPageUrl}&sortby=startdate", UriKind.RelativeOrAbsolute)
                }
            };
            yield return new object[]
            {
                SearchPageUrl,
                CourseSearchOrderBy.StartDate,
                new OrderByLinks
                {
                    CourseSearchSortBy = CourseSearchOrderBy.StartDate,
                    OrderByRelevanceUrl = new Uri($"{SearchPageUrl}&sortby=relevance", UriKind.RelativeOrAbsolute),
                    OrderByDistanceUrl = new Uri($"{SearchPageUrl}&sortby=distance", UriKind.RelativeOrAbsolute),
                    OrderByStartDateUrl = new Uri($"{SearchPageUrl}&sortby=startdate", UriKind.RelativeOrAbsolute)
                }
            };
        }

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

        public static IEnumerable<object[]> SetupPagingTestsInput()
        {
            yield return new object[]
            {
                new TrainingCourseResultsViewModel(),
                ZeroResultsCourseSearchResponse,
                PathQuery,
                20,
                SearchPageUrl,
                new TrainingCourseResultsViewModel()
            };

            yield return new object[]
            {
                new TrainingCourseResultsViewModel(),
                ValidCourseSearchResponse,
                PathQuery,
                20,
                SearchPageUrl,
                new TrainingCourseResultsViewModel
                {
                    CurrentPageNumber = 1,
                    ResultsCount = 2,
                    TotalPagesCount = 1
                }
            };

            yield return new object[]
            {
                new TrainingCourseResultsViewModel(),
                MultiPageResultsCourseSearchResponse,
                PathQuery,
                20,
                SearchPageUrl,
                new TrainingCourseResultsViewModel
                {
                    CurrentPageNumber = 2,
                    ResultsCount = 60,
                    TotalPagesCount = 3,
                    PaginationViewModel = new PaginationViewModel
                    {
                        HasPreviousPage = true,
                        HasNextPage = true,
                        NextPageUrl = new Uri($"{PathQuery}&page=3", UriKind.RelativeOrAbsolute),
                        NextPageUrlText = "3 of 3",
                        PreviousPageUrl = new Uri($"{PathQuery}", UriKind.RelativeOrAbsolute),
                        PreviousPageUrlText = "1 of 3"
                    }
                }
            };

            yield return new object[]
            {
                new TrainingCourseResultsViewModel(),
                TwoPageResultsCourseSearchResponse,
                PathQuery,
                20,
                SearchPageUrl,
                new TrainingCourseResultsViewModel
                {
                    CurrentPageNumber = 1,
                    ResultsCount = 40,
                    TotalPagesCount = 2,
                    PaginationViewModel = new PaginationViewModel
                    {
                        HasPreviousPage = false,
                        HasNextPage = true,
                        NextPageUrl = new Uri($"{PathQuery}&page=2", UriKind.RelativeOrAbsolute),
                        NextPageUrlText = "2 of 2",
                    }
                }
            };
        }

        public static IEnumerable<object[]> GetFilterSelectItemsTestInput()
        {
            yield return new object[]
            {
                new CourseFiltersModel(),
                new Dictionary<string, string>()
            };

            yield return new object[]
            {
                new CourseFiltersModel
                {
                    Location = Location,
                    ProviderKeyword = Provider,
                    AgeSuitabilitySelectedList = GetSelectedItems(),
                    AttendanceSelectedList = GetSelectedItems(),
                    PatternSelectedList = GetSelectedItems(),
                    QualificationSelectedList = GetSelectedItems(),
                    StudyModeSelectedList = GetSelectedItems()
                },
                FilterDictionary()
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

        public static IEnumerable<Course> GetCourses(int count, bool withUrl = false)
        {
            for (var i = 0; i < count; i++)
            {
                yield return new Course
                {
                    Title = nameof(Course.Title),
                    CourseId = nameof(Course.CourseId),
                    CourseUrl = withUrl ? $"{SearchPageUrl}/{nameof(Course.CourseId)}" : string.Empty
                };
            }
        }

        public static IEnumerable<CourseListingViewModel> GetCourseListings(int count, bool withUrl = false)
        {
            for (var i = 0; i < count; i++)
            {
                yield return new CourseListingViewModel
                {
                    Course = new Course
                    {
                        Title = nameof(Course.Title),
                        CourseId = nameof(Course.CourseId),
                        CourseUrl = withUrl ? $"{SearchPageUrl}/{nameof(Course.CourseId)}" : string.Empty
                    }
                };
            }
        }

        private static IDictionary<string, string> FilterDictionary()
        {
            var dictionary = new Dictionary<string, string>
            {
                { "Location:", Location },
                { "Provider:", Provider },
                { "Attendance:", nameof(SelectItem.Label) },
                { "Course type:", nameof(SelectItem.Label) },
                { "Qualification Level(s):", nameof(SelectItem.Label) },
                { "Age Suitability:", nameof(SelectItem.Label) },
                { "Study mode:", nameof(SelectItem.Label) }
            };

            return dictionary;
        }

        private static IEnumerable<SelectItem> GetSelectedItems()
        {
            yield return new SelectItem
            {
                Label = nameof(SelectItem.Label),
                Name = nameof(SelectItem.Name),
                Value = nameof(SelectItem.Value),
                Checked = "checked"
            };
            yield return new SelectItem
            {
                Label = nameof(SelectItem.Label),
                Name = nameof(SelectItem.Name),
                Value = nameof(SelectItem.Value)
            };
        }
    }
}
