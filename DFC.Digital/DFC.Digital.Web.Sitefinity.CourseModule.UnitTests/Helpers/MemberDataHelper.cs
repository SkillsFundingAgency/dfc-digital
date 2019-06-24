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

        private static readonly CourseDetailsViewModel CourseDetailsViewModelWithDetailsLabels =
            new CourseDetailsViewModel
            {
                SubjectCategoryLabel = nameof(CourseDetailsViewModel.SubjectCategoryLabel),
                StartDateLabel = nameof(CourseDetailsViewModel.StartDateLabel),
                AdditionalPriceLabel = nameof(CourseDetailsViewModel.AdditionalPriceLabel),
                LanguageOfInstructionLabel = nameof(CourseDetailsViewModel.LanguageOfInstructionLabel),
                AwardingOrganisationLabel = nameof(CourseDetailsViewModel.AwardingOrganisationLabel),
                CourseTypeLabel = nameof(CourseDetailsViewModel.CourseTypeLabel),
                CourseWebpageLinkLabel = nameof(CourseDetailsViewModel.CourseWebpageLinkLabel),
                QualificationLevelLabel = nameof(CourseDetailsViewModel.QualificationLevelLabel),
                QualificationNameLabel = nameof(CourseDetailsViewModel.QualificationNameLabel),
                PriceLabel = nameof(CourseDetailsViewModel.PriceLabel),
                FundingInformationLabel = nameof(CourseDetailsViewModel.FundingInformationLabel),
                AttendancePatternLabel = nameof(CourseDetailsViewModel.AttendancePatternLabel),
                SupportingFacilitiesLabel = nameof(CourseDetailsViewModel.SupportingFacilitiesLabel),
                FundingInformationLink = nameof(CourseDetailsViewModel.FundingInformationLink),
                FundingInformationText = nameof(CourseDetailsViewModel.FundingInformationText)
            };

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

        private static readonly CourseDetailsViewModel ValidCourseDetailsViewModel =
            new CourseDetailsViewModel
            {
                FindACoursePage = nameof(CourseDetailsViewModel.FindACoursePage),
                QualificationDetailsLabel = nameof(CourseDetailsViewModel.QualificationDetailsLabel),
                CourseDescriptionLabel = nameof(CourseDetailsViewModel.CourseDescriptionLabel),

                NoCourseDescriptionMessage = nameof(CourseDetailsViewModel.NoCourseDescriptionMessage),
                EntryRequirementsLabel = nameof(CourseDetailsViewModel.EntryRequirementsLabel),
                NoEntryRequirementsAvailableMessage = nameof(CourseDetailsViewModel.NoEntryRequirementsAvailableMessage),

                EquipmentRequiredLabel = nameof(CourseDetailsViewModel.EquipmentRequiredLabel),
                NoEquipmentRequiredMessage = nameof(CourseDetailsViewModel.NoEquipmentRequiredMessage),
                AssessmentMethodLabel = nameof(CourseDetailsViewModel.AssessmentMethodLabel),

                NoAssessmentMethodAvailableMessage = nameof(CourseDetailsViewModel.NoAssessmentMethodAvailableMessage),
                VenueLabel = nameof(CourseDetailsViewModel.VenueLabel),

                OtherDatesAndVenuesLabel = nameof(CourseDetailsViewModel.OtherDatesAndVenuesLabel),
                NoOtherDateOrVenueAvailableMessage = nameof(CourseDetailsViewModel.NoOtherDateOrVenueAvailableMessage),
                ReferralPath = nameof(CourseDetailsViewModel.ReferralPath),

                ProviderLabel = nameof(CourseDetailsViewModel.ProviderLabel),
                EmployerSatisfactionLabel = nameof(CourseDetailsViewModel.EmployerSatisfactionLabel),
                LearnerSatisfactionLabel = nameof(CourseDetailsViewModel.LearnerSatisfactionLabel),
                ProviderPerformanceLabel = nameof(CourseDetailsViewModel.ProviderPerformanceLabel),
                CourseDetailsPage = nameof(CourseDetailsViewModel.CourseDetailsPage),
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
                   StartDate = StartDate.Anytime,
                   ActiveFiltersMilesText = nameof(CourseFiltersViewModel.ActiveFiltersMilesText),
                   ActiveFiltersOfText = nameof(CourseFiltersViewModel.ActiveFiltersOfText),
                   ActiveFiltersOnly1619CoursesText = nameof(CourseFiltersViewModel.ActiveFiltersOnly1619CoursesText),
                   ActiveFiltersProvidedByText = nameof(CourseFiltersViewModel.ActiveFiltersProvidedByText),
                   ActiveFiltersStartingFromText = nameof(CourseFiltersViewModel.ActiveFiltersStartingFromText),
                   ActiveFiltersSuitableForText = nameof(CourseFiltersViewModel.ActiveFiltersSuitableForText),
                   ActiveFiltersShowingText = nameof(CourseFiltersViewModel.ActiveFiltersShowingText),
                   ActiveFiltersCoursesText = nameof(CourseFiltersViewModel.ActiveFiltersCoursesText),
                   ActiveFiltersWithinText = nameof(CourseFiltersViewModel.ActiveFiltersWithinText),
                   FilterProviderLabel = nameof(CourseFiltersViewModel.FilterProviderLabel),
                   FilterLocationLabel = nameof(CourseFiltersViewModel.FilterLocationLabel),
                   Only1619CoursesSectionText = nameof(CourseFiltersViewModel.Only1619CoursesSectionText)
               }
            };

            yield return new object[]
            {
                new CourseFiltersViewModel
                {
                    ApplyFiltersText = nameof(CourseFiltersViewModel.ApplyFiltersText),
                    CourseHours = CourseHours.Fulltime,
                    CourseType = CourseType.ClassroomBased,
                    Only1619Courses = false,
                    Location = nameof(CourseFiltersViewModel.Location),
                    Provider = nameof(CourseFiltersViewModel.Provider),
                    Distance = 10f,
                    LocationRegex = Constants.CourseSearchLocationRegularExpression,
                    StartDateSectionText = nameof(CourseFiltersViewModel.StartDateSectionText),
                    CourseTypeSectionText = nameof(CourseFiltersViewModel.CourseTypeSectionText),
                    CourseHoursSectionText = nameof(CourseFiltersViewModel.CourseHoursSectionText),
                    StartDate = StartDate.FromToday,
                    ActiveFiltersMilesText = nameof(CourseFiltersViewModel.ActiveFiltersMilesText),
                    ActiveFiltersOfText = nameof(CourseFiltersViewModel.ActiveFiltersOfText),
                    ActiveFiltersOnly1619CoursesText = nameof(CourseFiltersViewModel.ActiveFiltersOnly1619CoursesText),
                    ActiveFiltersProvidedByText = nameof(CourseFiltersViewModel.ActiveFiltersProvidedByText),
                    ActiveFiltersStartingFromText = nameof(CourseFiltersViewModel.ActiveFiltersStartingFromText),
                    ActiveFiltersSuitableForText = nameof(CourseFiltersViewModel.ActiveFiltersSuitableForText),
                    ActiveFiltersShowingText = nameof(CourseFiltersViewModel.ActiveFiltersShowingText),
                    ActiveFiltersCoursesText = nameof(CourseFiltersViewModel.ActiveFiltersCoursesText),
                    ActiveFiltersWithinText = nameof(CourseFiltersViewModel.ActiveFiltersWithinText)
                }
            };

            yield return new object[]
            {
                new CourseFiltersViewModel
                {
                    ApplyFiltersText = nameof(CourseFiltersViewModel.ApplyFiltersText),
                    CourseHours = CourseHours.Fulltime,
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
                    StartDateFrom = DateTime.Now.AddMonths(2),
                    ActiveFiltersMilesText = nameof(CourseFiltersViewModel.ActiveFiltersMilesText),
                    ActiveFiltersOfText = nameof(CourseFiltersViewModel.ActiveFiltersOfText),
                    ActiveFiltersOnly1619CoursesText = nameof(CourseFiltersViewModel.ActiveFiltersOnly1619CoursesText),
                    ActiveFiltersProvidedByText = nameof(CourseFiltersViewModel.ActiveFiltersProvidedByText),
                    ActiveFiltersStartingFromText = nameof(CourseFiltersViewModel.ActiveFiltersStartingFromText),
                    ActiveFiltersSuitableForText = nameof(CourseFiltersViewModel.ActiveFiltersSuitableForText),
                    ActiveFiltersShowingText = nameof(CourseFiltersViewModel.ActiveFiltersShowingText),
                    ActiveFiltersCoursesText = nameof(CourseFiltersViewModel.ActiveFiltersCoursesText),
                    ActiveFiltersWithinText = nameof(CourseFiltersViewModel.ActiveFiltersWithinText)
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
                    CourseLink = $"{SearchPageUrl}/{nameof(Course.CourseId)}/{nameof(PathQuery)}",
                    LocationDetails = new LocationDetails
                    {
                        LocationAddress = nameof(LocationDetails.LocationAddress),
                        Distance = 10f
                    },
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
                    CourseLink = $"{SearchPageUrl}/{nameof(Course.CourseId)}/{nameof(PathQuery)}",
                    StartDateLabel = nameof(Course.StartDateLabel),
                    QualificationLevel = "unknown",
                    LocationDetails = new LocationDetails
                    {
                        LocationAddress = nameof(LocationDetails.LocationAddress)
                    },
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
                    CourseLink = $"{SearchPageUrl}/{nameof(Course.CourseId)}/{nameof(PathQuery)}",
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

        public static IEnumerable<object[]> IndexPostModelTestsInput()
        {
            yield return new object[]
            {
                nameof(CourseSearchResultsController.ResetFilterText),
                nameof(CourseSearchResultsController.PageTitle),
                nameof(CourseSearchResultsController.CourseSearchResultsPage),
                nameof(CourseSearchResultsController.CourseDetailsPage),
                ValidCourseFiltersViewModel
            };
            yield return new object[]
            {
                nameof(CourseSearchResultsController.ResetFilterText),
                nameof(CourseSearchResultsController.PageTitle),
                nameof(CourseSearchResultsController.CourseSearchResultsPage),
                nameof(CourseSearchResultsController.CourseDetailsPage),
                ValidCourseFiltersViewModel
            };
            yield return new object[]
            {
                nameof(CourseSearchResultsController.ResetFilterText),
                nameof(CourseSearchResultsController.PageTitle),
                nameof(CourseSearchResultsController.CourseSearchResultsPage),
                nameof(CourseSearchResultsController.CourseDetailsPage),
                InvalidCourseResultsViewModel
            };
            yield return new object[]
            {
                nameof(CourseSearchResultsController.ResetFilterText),
                nameof(CourseSearchResultsController.PageTitle),
                nameof(CourseSearchResultsController.CourseSearchResultsPage),
                nameof(CourseSearchResultsController.CourseDetailsPage),
                ValidSelectFromCourseFiltersViewModel
            };
            yield return new object[]
            {
                nameof(CourseSearchResultsController.ResetFilterText),
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
                nameof(CourseSearchResultsController.ResetFilterText),
                nameof(CourseSearchResultsController.PageTitle),
                $"/{nameof(CourseSearchResultsController.CourseSearchResultsPage)}",
                nameof(CourseSearchResultsController.CourseDetailsPage),
                CourseSearchOrderBy.Relevance,
                ValidCourseSearchResponse
            };

            yield return new object[]
            {
                ValidSelectFromTodayCourseFiltersViewModel,
                nameof(CourseSearchResultsController.ResetFilterText),
                nameof(CourseSearchResultsController.PageTitle),
                $"/{nameof(CourseSearchResultsController.CourseSearchResultsPage)}",
                nameof(CourseSearchResultsController.CourseDetailsPage),
                CourseSearchOrderBy.Distance,
                ValidCourseSearchResponse
            };

            yield return new object[]
            {
                ValidCourseFiltersViewModel,
                nameof(CourseSearchResultsController.ResetFilterText),
                nameof(CourseSearchResultsController.PageTitle),
                $"/{nameof(CourseSearchResultsController.CourseSearchResultsPage)}",
                nameof(CourseSearchResultsController.CourseDetailsPage),
                CourseSearchOrderBy.StartDate,
                ValidCourseSearchResponse
            };

            yield return new object[]
            {
               ValidCourseFiltersViewModel,
                nameof(CourseSearchResultsController.ResetFilterText),
                nameof(CourseSearchResultsController.PageTitle),
                $"/{nameof(CourseSearchResultsController.CourseSearchResultsPage)}",
                nameof(CourseSearchResultsController.CourseDetailsPage),
                CourseSearchOrderBy.StartDate,
                ZeroResultsCourseSearchResponse
            };

            yield return new object[]
            {
                InvalidCourseResultsViewModel,
                nameof(CourseSearchResultsController.ResetFilterText),
                nameof(CourseSearchResultsController.PageTitle),
                $"/{nameof(CourseSearchResultsController.CourseSearchResultsPage)}",
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
                    CourseLink = withUrl ? $"{SearchPageUrl}/{nameof(Course.CourseId)}/{nameof(PathQuery)}" : string.Empty
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
                        CourseLink = withUrl ? $"{SearchPageUrl}/{nameof(Course.CourseId)}" : string.Empty
                    }
                };
            }
        }

        public static IEnumerable<object[]> CourseDetailsIndexDefaultTestsInput()
        {
            yield return new object[]
            {
                nameof(CourseDetailsViewModel.FindACoursePage),
                nameof(CourseDetailsViewModel.CourseDetails.CourseId),
                nameof(Oppurtunity.OppurtunityId),
                nameof(CourseDetailsViewModel.NoCourseDescriptionMessage),
                nameof(CourseDetailsViewModel.NoEntryRequirementsAvailableMessage),
                nameof(CourseDetailsViewModel.NoEquipmentRequiredMessage),
                nameof(CourseDetailsViewModel.NoAssessmentMethodAvailableMessage),
                nameof(CourseDetailsViewModel.NoOtherDateOrVenueAvailableMessage),
                nameof(CourseDetailsViewModel.CourseDetailsPage),
                nameof(CourseDetailsViewModel.QualificationDetailsLabel),
                nameof(CourseDetailsViewModel.CourseDescriptionLabel),
                nameof(CourseDetailsViewModel.EntryRequirementsLabel),
                nameof(CourseDetailsViewModel.EquipmentRequiredLabel),
                nameof(CourseDetailsViewModel.AssessmentMethodLabel),
                nameof(CourseDetailsViewModel.VenueLabel),
                nameof(CourseDetailsViewModel.OtherDatesAndVenuesLabel),
                nameof(CourseDetailsViewModel.ProviderLabel),
                nameof(CourseDetailsViewModel.EmployerSatisfactionLabel),
                nameof(CourseDetailsViewModel.LearnerSatisfactionLabel),
                nameof(CourseDetailsViewModel.ProviderPerformanceLabel),
                nameof(CourseDetailsViewModel.ReferralPath),
                nameof(CourseDetailsViewModel.ContactAdviserSection),
                nameof(CourseDetailsViewModel.AdditionalPriceLabel),
                nameof(CourseDetailsViewModel.AttendancePatternLabel),
                nameof(CourseDetailsViewModel.AwardingOrganisationLabel),
                nameof(CourseDetailsViewModel.CourseTypeLabel),
                nameof(CourseDetailsViewModel.CourseWebpageLinkLabel),
                nameof(CourseDetailsViewModel.SupportingFacilitiesLabel),
                nameof(CourseDetailsViewModel.FundingInformationLabel),
                nameof(CourseDetailsViewModel.FundingInformationLink),
                nameof(CourseDetailsViewModel.FundingInformationText),
                nameof(CourseDetailsViewModel.LanguageOfInstructionLabel),
                nameof(CourseDetailsViewModel.PriceLabel),
                nameof(CourseDetailsViewModel.QualificationLevelLabel),
                nameof(CourseDetailsViewModel.QualificationNameLabel),
                nameof(CourseDetailsViewModel.StartDateLabel),
                nameof(CourseDetailsViewModel.SubjectCategoryLabel)
            };
        }

        public static IEnumerable<object[]> Dfc9560MissingFieldsTestInput()
        {
            yield return new object[]
            {
               CourseDetailsViewModelWithDetailsLabels,
               new CourseDetails
               {
                   SubjectCategory = nameof(CourseDetails.SubjectCategory),
                   LanguageOfInstruction = nameof(CourseDetails.LanguageOfInstruction),
                   AwardingOrganisation = nameof(CourseDetails.AwardingOrganisation),
                   CourseWebPageLink = nameof(CourseDetails.CourseWebPageLink),
                   AdditionalPrice = nameof(CourseDetails.AdditionalPrice),
                   StudyMode = nameof(CourseDetails.StudyMode),
                   SupportingFacilities = nameof(CourseDetails.SupportingFacilities),
                   AdvancedLearnerLoansOffered = true,
                   QualificationLevel = nameof(CourseDetails.QualificationLevel),
                   QualificationName = nameof(CourseDetails.QualificationName),
                   Cost = nameof(CourseDetails.Cost)
               }
            };

            yield return new object[]
            {
                CourseDetailsViewModelWithDetailsLabels,
                new CourseDetails
                {
                    SubjectCategory = nameof(CourseDetails.SubjectCategory),
                    LanguageOfInstruction = nameof(CourseDetails.LanguageOfInstruction),
                    AwardingOrganisation = nameof(CourseDetails.AwardingOrganisation),
                    CourseWebPageLink = nameof(CourseDetails.CourseWebPageLink),
                    AdditionalPrice = nameof(CourseDetails.AdditionalPrice),
                    StudyMode = nameof(CourseDetails.StudyMode),
                    SupportingFacilities = nameof(CourseDetails.SupportingFacilities),
                    AdvancedLearnerLoansOffered = true
                }
            };

            yield return new object[]
            {
                CourseDetailsViewModelWithDetailsLabels,
                new CourseDetails
                {
                    SubjectCategory = nameof(CourseDetails.SubjectCategory),
                    LanguageOfInstruction = nameof(CourseDetails.LanguageOfInstruction),
                    AwardingOrganisation = nameof(CourseDetails.AwardingOrganisation),
                    CourseWebPageLink = nameof(CourseDetails.CourseWebPageLink)
                }
            };

            yield return new object[]
            {
                CourseDetailsViewModelWithDetailsLabels,
                new CourseDetails
                {
                    AdditionalPrice = nameof(CourseDetails.AdditionalPrice),
                    StudyMode = nameof(CourseDetails.StudyMode),
                    SupportingFacilities = nameof(CourseDetails.SupportingFacilities),
                    AdvancedLearnerLoansOffered = true
                }
            };
        }

        private static IDictionary<string, string> FilterDictionary()
        {
            var dictionary = new Dictionary<string, string>
            {
                { "Location:", Location },
                { "Provider:", Provider },
                { "Attendance:", nameof(SelectItem.Label) },
                { "Course type:", nameof(SelectItem.Label) },
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
