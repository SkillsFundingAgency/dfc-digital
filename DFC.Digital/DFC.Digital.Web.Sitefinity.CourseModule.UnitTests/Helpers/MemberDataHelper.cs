using DFC.Digital.Core;
using DFC.Digital.Data.Model;
using DFC.Digital.Web.Sitefinity.CourseModule.Mvc.Controllers;
using System;
using System.Collections.Generic;

namespace DFC.Digital.Web.Sitefinity.CourseModule.UnitTests.Helpers
{
    public class MemberDataHelper : AssertionHelper
    {
        private const string SearchTerm = "maths";
        private const string SearchPageUrl = "/courses-search-results";
        private const string PathQuery = "/pathQuery";
        private const string QualificationLevel = "qualificationLevel";
        private const bool Only1619Courses = true;
        private const string Location = "leeds";
        private const string LocationPostCode = "cv12wt";
        private const string Provider = "ucla";
        private const string StartDateFormat = "yyyy-MM-dd";
        private const float ValidDistance = 10F;

        private static readonly CourseFiltersViewModel ValidCourseFiltersViewModel =
            new CourseFiltersViewModel
            {
                SearchTerm = nameof(CourseSearchFilters.SearchTerm),
            };

        private static readonly CourseFiltersViewModel ValidSelectFromCourseFiltersViewModel =
            new CourseFiltersViewModel
            {
                SearchTerm = nameof(CourseSearchFilters.SearchTerm),
                StartDateDay = DateTime.Now.Day.ToString(),
                StartDate = StartDate.SelectDateFrom,
                StartDateMonth = DateTime.Now.AddMonths(2).Month.ToString(),
                StartDateYear = DateTime.Now.Year.ToString(),
                StartDateFrom = DateTime.Now.AddDays(20)
            };

        private static readonly CourseFiltersViewModel ValidSelectFromTodayCourseFiltersViewModel =
            new CourseFiltersViewModel
            {
                SearchTerm = nameof(CourseSearchFilters.SearchTerm),
                StartDate = StartDate.FromToday,
                CourseType = CourseType.ClassroomBased,
                CourseHours = CourseHours.All,
                Only1619Courses = true
            };

        private static readonly CourseFiltersViewModel InvalidCourseResultsViewModel =
            new CourseFiltersViewModel
            {
                SearchTerm = string.Empty
            };

        private static readonly CourseSearchResult ValidCourseSearchResponse = new CourseSearchResult
        {
            Courses = GetCourses(2),
            ResultProperties = new CourseSearchResultProperties
            {
                Page = 1,
                TotalPages = 1,
                TotalResultCount = 2
            }
        };

        private static readonly CourseSearchResult ZeroResultsCourseSearchResponse = new CourseSearchResult
        {
            Courses = new List<Course>()
        };

        private static readonly CourseSearchResult MultiPageResultsCourseSearchResponse = new CourseSearchResult
        {
            Courses = GetCourses(20),
            ResultProperties = new CourseSearchResultProperties
            {
                Page = 2,
                TotalPages = 3,
                TotalResultCount = 60
            }
        };

        private static readonly CourseSearchResult TwoPageResultsCourseSearchResponse = new CourseSearchResult
        {
            Courses = GetCourses(20),
            ResultProperties = new CourseSearchResultProperties
            {
                Page = 1,
                TotalPages = 2,
                TotalResultCount = 40
            }
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
                0,
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

        public static IEnumerable<object[]> Dfc9208CourseFiltersViewModelViewTestsInput()
        {
            yield return new object[]
            {
               new CourseFiltersViewModel
               {
                   ApplyFiltersText = nameof(CourseFiltersViewModel.ApplyFiltersText),
                   CourseHours = CourseHours.All,
                   CourseType = CourseType.All,
                   Only1619Courses = false,
                   Location = LocationPostCode,
                   Provider = nameof(CourseFiltersViewModel.Provider),
                   Distance = 10f,
                   LocationRegex = Constants.CourseSearchLocationRegularExpression,
                   StartDateSectionText = nameof(CourseFiltersViewModel.StartDateSectionText),
                   CourseTypeSectionText = nameof(CourseFiltersViewModel.CourseTypeSectionText),
                   CourseHoursSectionText = nameof(CourseFiltersViewModel.CourseHoursSectionText),
                   StartDate = StartDate.Anytime
               }
            };

            yield return new object[]
            {
                new CourseFiltersViewModel
                {
                    ApplyFiltersText = nameof(CourseFiltersViewModel.ApplyFiltersText),
                    CourseHours = CourseHours.FullTime,
                    CourseType = CourseType.ClassroomBased,
                    Only1619Courses = false,
                    Location = nameof(CourseFiltersViewModel.Location),
                    Provider = nameof(CourseFiltersViewModel.Provider),
                    Distance = 10f,
                    LocationRegex = Constants.CourseSearchLocationRegularExpression,
                    StartDateSectionText = nameof(CourseFiltersViewModel.StartDateSectionText),
                    CourseTypeSectionText = nameof(CourseFiltersViewModel.CourseTypeSectionText),
                    CourseHoursSectionText = nameof(CourseFiltersViewModel.CourseHoursSectionText),
                    StartDate = StartDate.FromToday
                }
            };

            yield return new object[]
            {
                new CourseFiltersViewModel
                {
                    ApplyFiltersText = nameof(CourseFiltersViewModel.ApplyFiltersText),
                    CourseHours = CourseHours.FullTime,
                    CourseType = CourseType.ClassroomBased,
                    Only1619Courses = false,
                    Location = LocationPostCode,
                    LocationRegex = Constants.CourseSearchLocationRegularExpression,
                    Provider = nameof(CourseFiltersViewModel.Provider),
                    Distance = 10f,
                    StartDateSectionText = nameof(CourseFiltersViewModel.StartDateSectionText),
                    CourseTypeSectionText = nameof(CourseFiltersViewModel.CourseTypeSectionText),
                    CourseHoursSectionText = nameof(CourseFiltersViewModel.CourseHoursSectionText),
                    StartDate = StartDate.SelectDateFrom,
                    StartDateFrom = DateTime.Now.AddMonths(2)
                }
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

        public static IEnumerable<object[]> GetOrderByLinksTestsInput()
        {
            var sortbyUrl = $"{SearchPageUrl}&{nameof(CourseSearchProperties.OrderedBy)}=Distance";
            yield return new object[]
            {
                sortbyUrl,
                CourseSearchOrderBy.Distance,
                new OrderByLinks
                {
                    OrderBy = CourseSearchOrderBy.Distance,
                    OrderByRelevanceUrl = new Uri($"{SearchPageUrl}&{nameof(CourseSearchProperties.OrderedBy)}={nameof(CourseSearchOrderBy.Relevance)}", UriKind.RelativeOrAbsolute),
                    OrderByDistanceUrl = new Uri($"{SearchPageUrl}&{nameof(CourseSearchProperties.OrderedBy)}={nameof(CourseSearchOrderBy.Distance)}", UriKind.RelativeOrAbsolute),
                    OrderByStartDateUrl = new Uri($"{SearchPageUrl}&{nameof(CourseSearchProperties.OrderedBy)}={nameof(CourseSearchOrderBy.StartDate)}", UriKind.RelativeOrAbsolute)
                }
            };
            yield return new object[]
            {
                SearchPageUrl,
                CourseSearchOrderBy.Relevance,
                new OrderByLinks
                {
                    OrderBy = CourseSearchOrderBy.Relevance,
                    OrderByRelevanceUrl = new Uri($"{SearchPageUrl}&{nameof(CourseSearchProperties.OrderedBy)}={nameof(CourseSearchOrderBy.Relevance)}", UriKind.RelativeOrAbsolute),
                    OrderByDistanceUrl = new Uri($"{SearchPageUrl}&{nameof(CourseSearchProperties.OrderedBy)}={nameof(CourseSearchOrderBy.Distance)}", UriKind.RelativeOrAbsolute),
                    OrderByStartDateUrl = new Uri($"{SearchPageUrl}&{nameof(CourseSearchProperties.OrderedBy)}={nameof(CourseSearchOrderBy.StartDate)}", UriKind.RelativeOrAbsolute)
                }
            };
            yield return new object[]
            {
                SearchPageUrl,
                CourseSearchOrderBy.StartDate,
                new OrderByLinks
                {
                    OrderBy = CourseSearchOrderBy.StartDate,
                    OrderByRelevanceUrl = new Uri($"{SearchPageUrl}&{nameof(CourseSearchProperties.OrderedBy)}={nameof(CourseSearchOrderBy.Relevance)}", UriKind.RelativeOrAbsolute),
                    OrderByDistanceUrl = new Uri($"{SearchPageUrl}&{nameof(CourseSearchProperties.OrderedBy)}={nameof(CourseSearchOrderBy.Distance)}", UriKind.RelativeOrAbsolute),
                    OrderByStartDateUrl = new Uri($"{SearchPageUrl}&{nameof(CourseSearchProperties.OrderedBy)}={nameof(CourseSearchOrderBy.StartDate)}", UriKind.RelativeOrAbsolute)
                }
            };
        }

        public static IEnumerable<object[]> IndexPostTestsInput()
        {
            yield return new object[]
            {
                nameof(CourseSearchResultsController.FilterCourseByText),
                nameof(CourseSearchResultsController.PageTitle),
                nameof(CourseSearchResultsController.CourseSearchResultsPage),
                nameof(CourseSearchResultsController.CourseDetailsPage),
                ValidCourseFiltersViewModel
            };
            yield return new object[]
            {
                nameof(CourseSearchResultsController.FilterCourseByText),
                nameof(CourseSearchResultsController.PageTitle),
                nameof(CourseSearchResultsController.CourseSearchResultsPage),
                nameof(CourseSearchResultsController.CourseDetailsPage),
                ValidCourseFiltersViewModel
            };
            yield return new object[]
            {
                nameof(CourseSearchResultsController.FilterCourseByText),
                nameof(CourseSearchResultsController.PageTitle),
                nameof(CourseSearchResultsController.CourseSearchResultsPage),
                nameof(CourseSearchResultsController.CourseDetailsPage),
                InvalidCourseResultsViewModel
            };
            yield return new object[]
            {
                nameof(CourseSearchResultsController.FilterCourseByText),
                nameof(CourseSearchResultsController.PageTitle),
                nameof(CourseSearchResultsController.CourseSearchResultsPage),
                nameof(CourseSearchResultsController.CourseDetailsPage),
                ValidSelectFromCourseFiltersViewModel
            };
            yield return new object[]
            {
                nameof(CourseSearchResultsController.FilterCourseByText),
                nameof(CourseSearchResultsController.PageTitle),
                nameof(CourseSearchResultsController.CourseSearchResultsPage),
                nameof(CourseSearchResultsController.CourseDetailsPage),
                ValidSelectFromTodayCourseFiltersViewModel
            };
        }

        public static IEnumerable<object[]> IndexTestsInput()
        {
            yield return new object[]
            {
               ValidSelectFromCourseFiltersViewModel,
                nameof(CourseSearchResultsController.FilterCourseByText),
                nameof(CourseSearchResultsController.PageTitle),
                nameof(CourseSearchResultsController.CourseSearchResultsPage),
                nameof(CourseSearchResultsController.CourseDetailsPage),
                CourseSearchOrderBy.Relevance,
                ValidCourseSearchResponse
            };

            yield return new object[]
            {
                ValidSelectFromTodayCourseFiltersViewModel,
                nameof(CourseSearchResultsController.FilterCourseByText),
                nameof(CourseSearchResultsController.PageTitle),
                nameof(CourseSearchResultsController.CourseSearchResultsPage),
                nameof(CourseSearchResultsController.CourseDetailsPage),
                CourseSearchOrderBy.Distance,
                ValidCourseSearchResponse
            };

            yield return new object[]
            {
                ValidCourseFiltersViewModel,
                nameof(CourseSearchResultsController.FilterCourseByText),
                nameof(CourseSearchResultsController.PageTitle),
                nameof(CourseSearchResultsController.CourseSearchResultsPage),
                nameof(CourseSearchResultsController.CourseDetailsPage),
                CourseSearchOrderBy.StartDate,
                ValidCourseSearchResponse
            };

            yield return new object[]
            {
               ValidCourseFiltersViewModel,
                nameof(CourseSearchResultsController.FilterCourseByText),
                nameof(CourseSearchResultsController.PageTitle),
                nameof(CourseSearchResultsController.CourseSearchResultsPage),
                nameof(CourseSearchResultsController.CourseDetailsPage),
                CourseSearchOrderBy.StartDate,
                ZeroResultsCourseSearchResponse
            };

            yield return new object[]
            {
                InvalidCourseResultsViewModel,
                nameof(CourseSearchResultsController.FilterCourseByText),
                nameof(CourseSearchResultsController.PageTitle),
                nameof(CourseSearchResultsController.CourseSearchResultsPage),
                nameof(CourseSearchResultsController.CourseDetailsPage),
                CourseSearchOrderBy.Distance,
                ZeroResultsCourseSearchResponse
            };
        }

        public static IEnumerable<object[]> SetupPagingTestsInput()
        {
            yield return new object[]
            {
                new CourseSearchResultsViewModel(),
                ZeroResultsCourseSearchResponse,
                PathQuery,
                20,
                new CourseSearchResultsViewModel()
            };

            yield return new object[]
            {
                new CourseSearchResultsViewModel(),
                ValidCourseSearchResponse,
                PathQuery,
                20,
                new CourseSearchResultsViewModel
                {
                    Page = 1,
                    ResultProperties = new CourseSearchResultProperties
                    {
                        TotalResultCount = 2,
                        TotalPages = 1
                    }
                }
            };

            yield return new object[]
            {
                new CourseSearchResultsViewModel(),
                MultiPageResultsCourseSearchResponse,
                PathQuery,
                20,
                new CourseSearchResultsViewModel
                {
                    Page = 2,
                    ResultProperties = new CourseSearchResultProperties
                    {
                        TotalResultCount = 60,
                        TotalPages = 3
                    },
                    PaginationViewModel = new PaginationViewModel
                    {
                        HasPreviousPage = true,
                        HasNextPage = true,
                        NextPageUrl = new Uri($"{PathQuery}&Page=3", UriKind.RelativeOrAbsolute),
                        NextPageText = "3 of 3",
                        PreviousPageUrl = new Uri($"{PathQuery}", UriKind.RelativeOrAbsolute),
                        PreviousPageText = "1 of 3"
                    }
                }
            };

            yield return new object[]
            {
                new CourseSearchResultsViewModel(),
                TwoPageResultsCourseSearchResponse,
                PathQuery,
                20,
                new CourseSearchResultsViewModel
                {
                    Page = 1,
                    ResultProperties = new CourseSearchResultProperties
                    {
                        TotalResultCount = 40,
                        TotalPages = 2
                    },
                    PaginationViewModel = new PaginationViewModel
                    {
                        HasPreviousPage = false,
                        HasNextPage = true,
                        NextPageUrl = new Uri($"{PathQuery}&Page=2", UriKind.RelativeOrAbsolute),
                        NextPageText = "2 of 2",
                    }
                }
            };
        }

        public static IEnumerable<object[]> BuildSearchRedirectPathAndQueryStringTestsInput()
        {
            yield return new object[]
            {
                "/courses-search-results",
                new CourseLandingViewModel(),
                "/courses-search-results?"
            };

            yield return new object[]
            {
                "/courses-search-results",
                new CourseLandingViewModel
                {
                    SearchTerm = SearchTerm,
                    Provider = Provider
                },
                "/courses-search-results?searchTerm=maths&provider=ucla"
            };

            yield return new object[]
            {
                "/courses-search-results",
                new CourseLandingViewModel
                {
                    Provider = Provider
                },
                "/courses-search-results?provider=ucla"
            };

            yield return new object[]
            {
                "/courses-search-results",
                new CourseLandingViewModel
                {
                    SearchTerm = SearchTerm
                },
                "/courses-search-results?searchTerm=maths"
            };

            yield return new object[]
            {
                "/courses-search-results",
                new CourseLandingViewModel
                {
                    SearchTerm = SearchTerm,
                    Location = Location,
                    Only1619Courses = Only1619Courses
                },
                "/courses-search-results?searchTerm=maths&only1619courses=true&location=leeds"
            };

            yield return new object[]
            {
                "/courses-search-results",
                new CourseLandingViewModel
                {
                    SearchTerm = SearchTerm,
                    Location = LocationPostCode,
                    Only1619Courses = Only1619Courses
                },
                "/courses-search-results?searchTerm=maths&only1619courses=true&location=cv12wt"
            };
        }

        public static IEnumerable<object[]> BuildRedirectPathAndQueryStringTestsInput()
        {
            yield return new object[]
            {
                "/courses-search-results",
                new CourseSearchResultsViewModel(),
                "/courses-search-results?"
            };

            yield return new object[]
            {
                "/courses-search-results",
                new CourseSearchResultsViewModel
                {
                    CourseFiltersModel = new CourseFiltersViewModel
                    {
                        SearchTerm = SearchTerm,
                        Provider = Provider
                    }
                },
                "/courses-search-results?searchTerm=maths&provider=ucla"
            };

            yield return new object[]
            {
                "/courses-search-results",
                new CourseSearchResultsViewModel
                {
                    CourseFiltersModel = new CourseFiltersViewModel
                    {
                        Provider = Provider
                    }
                },
                "/courses-search-results?provider=ucla"
            };

            yield return new object[]
            {
                "/courses-search-results",
                new CourseSearchResultsViewModel
                {
                   CourseFiltersModel = new CourseFiltersViewModel
                   {
                       SearchTerm = SearchTerm
                   }
                },
                "/courses-search-results?searchTerm=maths"
            };

            yield return new object[]
            {
                "/courses-search-results",
                new CourseSearchResultsViewModel
                {
                    CourseFiltersModel = new CourseFiltersViewModel
                    {
                        SearchTerm = SearchTerm,
                        Only1619Courses = Only1619Courses,
                        Location = Location,
                        LocationRegex = Constants.CourseSearchLocationRegularExpression,
                        Distance = ValidDistance,
                        StartDate = StartDate.Anytime
                    }
                },
                "/courses-search-results?searchTerm=maths&only1619courses=true&location=leeds"
            };

            yield return new object[]
            {
                "/courses-search-results",
                new CourseSearchResultsViewModel
                {
                    CourseFiltersModel = new CourseFiltersViewModel
                    {
                        SearchTerm = SearchTerm,
                        Only1619Courses = Only1619Courses,
                        Location = Location,
                        LocationRegex = Constants.CourseSearchLocationRegularExpression,
                        StartDate = StartDate.FromToday
                    }
                },
                "/courses-search-results?searchTerm=maths&only1619courses=true&location=leeds&StartDate=FromToday"
            };

            yield return new object[]
            {
                "/courses-search-results",
                new CourseSearchResultsViewModel
                {
                    CourseFiltersModel = new CourseFiltersViewModel
                    {
                        SearchTerm = SearchTerm,
                        Only1619Courses = Only1619Courses,
                        Location = LocationPostCode,
                        LocationRegex = Constants.CourseSearchLocationRegularExpression,
                        Distance = ValidDistance,
                        StartDate = StartDate.FromToday
                    }
                },
                "/courses-search-results?searchTerm=maths&only1619courses=true&location=cv12wt&Distance=10&StartDate=FromToday"
            };

            yield return new object[]
            {
                "/courses-search-results",
                new CourseSearchResultsViewModel
                {
                    CourseFiltersModel = new CourseFiltersViewModel
                    {
                        SearchTerm = SearchTerm,
                        Only1619Courses = Only1619Courses,
                        Location = Location,
                        LocationRegex = Constants.CourseSearchLocationRegularExpression,
                        Distance = ValidDistance,
                        StartDate = StartDate.SelectDateFrom,
                        StartDateFrom = DateTime.Now.AddDays(30)
                    }
                },
                $"/courses-search-results?searchTerm=maths&only1619courses=true&location=leeds&StartDate=SelectDateFrom&StartDateFrom={DateTime.Now.AddDays(30).ToString(StartDateFormat)}"
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
    }
}
