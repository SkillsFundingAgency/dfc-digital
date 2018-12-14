using AutoMapper;
using DFC.Digital.Core;
using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using DFC.Digital.Web.Sitefinity.Core;
using DFC.Digital.Web.Sitefinity.JobProfileModule.Mvc.Controllers;
using DFC.Digital.Web.Sitefinity.JobProfileModule.Mvc.Models;
using FakeItEasy;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web;
using TestStack.FluentMVCTesting;
using Xunit;

namespace DFC.Digital.Web.Sitefinity.JobProfileModule.UnitTests
{
    /// <summary>
    /// Job Profile Search Box Controller tests
    /// </summary>
    public class JobProfileSearchBoxControllerTests
    {
        public JobProfileSearchBoxControllerTests()
        {
        }

        [Theory]
        [InlineData("Test", SearchWidgetPageMode.SearchResults, 20)]
        [InlineData("Test", SearchWidgetPageMode.SearchResults, 1)]
        [InlineData("Test & Core", SearchWidgetPageMode.SearchResults, 5, 2)]
        public void IndexTestAutomapperAndTotalMessage(string searchTerm, SearchWidgetPageMode mode, int resultsCount, int page = 1)
        {
            //Setup Fakes & dummies
            var queryServiceFake = A.Fake<ISearchQueryService<JobProfileIndex>>(ops => ops.Strict());
            var loggerFake = A.Fake<IApplicationLogger>();
            var webAppContext = A.Fake<IWebAppContext>(ops => ops.Strict());
            var defaultJobProfilePage = "/jobprofile-details/";
            var expectedTotalMessage = "0 results found - try again using a different job title";
            var fakeAsyncHelper = new AsyncHelper();
            var fakeSpellChecker = A.Fake<ISpellcheckService>();
            var mapperCfg = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<JobProfilesAutoMapperProfile>();
            });

            var expectedSearchResultsViewModel = Enumerable.Empty<JobProfileSearchResultItemViewModel>();
            var dummySearchResult = A.Dummy<SearchResult<JobProfileIndex>>();

            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                dummySearchResult.Results = Enumerable.Empty<SearchResultItem<JobProfileIndex>>();
                dummySearchResult.Count = 0;
            }
            else
            {
                var dummyIndex = new JobProfileIndex
                {
                    Title = searchTerm,
                    AlternativeTitle = new[] { "alt" },
                    SalaryStarter = 10,
                    SalaryExperienced = 10,
                    Overview = "overview",
                    UrlName = "dummy-url"
                };

                dummySearchResult.Count = resultsCount;

                expectedTotalMessage = $"{resultsCount} result{(resultsCount == 1 ? string.Empty : "s")} found";

                dummySearchResult.Results = A.CollectionOfDummy<SearchResultItem<JobProfileIndex>>(resultsCount);

                var rawResultItems = new List<SearchResultItem<JobProfileIndex>>();

                for (int i = 0; i < resultsCount; i++)
                {
                    rawResultItems.Add(new SearchResultItem<JobProfileIndex> { ResultItem = dummyIndex });
                }

                dummySearchResult.Results = rawResultItems;

                var expectedSearchResultsRawViewModel = new List<JobProfileSearchResultItemViewModel>();

                foreach (var dummyIndexItem in dummySearchResult.Results)
                {
                    expectedSearchResultsRawViewModel.Add(new JobProfileSearchResultItemViewModel
                    {
                        ResultItemTitle = dummyIndexItem.ResultItem.Title,
                        ResultItemAlternativeTitle = string.Join(", ", dummyIndexItem.ResultItem.AlternativeTitle).Trim().TrimEnd(','),
                        ResultItemSalaryRange = string.Format(new CultureInfo("en-GB", false), "{0:C0} to {1:C0}", dummyIndexItem.ResultItem.SalaryStarter, dummyIndexItem.ResultItem.SalaryExperienced),
                        ResultItemOverview = dummyIndexItem.ResultItem.Overview,
                        ResultItemUrlName = $"{defaultJobProfilePage}{dummyIndexItem.ResultItem.UrlName}",
                        Rank = (int)dummyIndexItem.Rank,
                        JobProfileCategoriesWithUrl = Enumerable.Empty<string>()
                    });
                }

                expectedSearchResultsViewModel = expectedSearchResultsRawViewModel.AsEnumerable();
            }

            //Set-up calls
            A.CallTo(() => queryServiceFake.SearchAsync(A<string>._, A<SearchProperties>._)).Returns(dummySearchResult);

            //Instantiate
            //  var searchController = new JobProfileSearchBoxController(queryServiceFake, webAppContext, mapperCfg.CreateMapper(), loggerFake)
            var searchController = new JobProfileSearchBoxController(queryServiceFake, webAppContext, mapperCfg.CreateMapper(), loggerFake, fakeAsyncHelper, fakeSpellChecker)
            {
                CurrentPageMode = mode,
                JobProfileDetailsPage = defaultJobProfilePage
            };

            //Act
            var searchMethodCall = searchController.WithCallTo(c => c.Index(searchTerm, page));

            //Assert
            searchMethodCall
                .ShouldRenderView("SearchResult")
                .WithModel<JobProfileSearchResultViewModel>(vm =>
                {
                    vm.TotalResultsMessage.Should().Be(expectedTotalMessage);
                    vm.SearchResults.Should().BeEquivalentTo(expectedSearchResultsViewModel);
                })
                .AndNoModelErrors()
                ;
        }

        [Theory]
        [InlineData("Test", SearchWidgetPageMode.SearchResults, 20, 1, 10)]
        [InlineData("Test", SearchWidgetPageMode.SearchResults, 1, 10, 10)]
        [InlineData("Test", SearchWidgetPageMode.SearchResults, 353, 10, 10)]
        [InlineData("Test & Core", SearchWidgetPageMode.SearchResults, 353, 10, 10)]
        public void SearchResultsPaginationViewModelTest(string searchTerm, SearchWidgetPageMode mode, int resultsCount, int page, int pageSize)
        {
            //Setup Fakes & dummies
            var queryServiceFake = A.Fake<ISearchQueryService<JobProfileIndex>>(ops => ops.Strict());
            var loggerFake = A.Fake<IApplicationLogger>();
            var webAppContext = A.Fake<IWebAppContext>(ops => ops.Strict());
            var defaultJobProfilePage = "/jobprofile-details/";
            var defaultSearchResultsPage = "/search-result";
            var defaultJobProfileCateegoryPage = "/job-categories/";
            var expectedTotalMessage = "0 results found - try again using a different job title";
            var placeholderText = "Enter a job title or keywords";
            var salaryBlankText = "Variable";
            var fakeSpellChecker = A.Fake<ISpellcheckService>();
            var fakeAsyncHelper = new AsyncHelper();
            var mapperCfg = new MapperConfiguration(cfg =>
                        {
                            cfg.AddProfile<JobProfilesAutoMapperProfile>();
                        });

            var expectedSearchResultsViewModel = Enumerable.Empty<JobProfileSearchResultItemViewModel>();
            var dummySearchResult = A.Dummy<SearchResult<JobProfileIndex>>();
            var expectedViewModel = new JobProfileSearchResultViewModel
            {
                TotalPages = (resultsCount + pageSize - 1) / pageSize,
                Count = resultsCount,
                SearchTerm = searchTerm,
                PlaceholderText = placeholderText,
                AutoCompleteMinimumCharacters = 2,
                MaximumNumberOfDisplayedSuggestions = 5,
                UseFuzzyAutoCompleteMatching = true,
                JobProfileCategoryPage = defaultJobProfileCateegoryPage,
                SalaryBlankText = salaryBlankText
            };

            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                dummySearchResult.Results = Enumerable.Empty<SearchResultItem<JobProfileIndex>>();
                dummySearchResult.Count = 0;
            }
            else
            {
                var currentPage = page > 0 ? page : 1;
                var dummyIndex = new JobProfileIndex
                {
                    Title = searchTerm,
                    AlternativeTitle = new[] { "alt" },
                    SalaryStarter = 10,
                    SalaryExperienced = 10,
                    Overview = "overview",
                    UrlName = "dummy-url",
                    JobProfileCategoriesWithUrl = new[] { "CatOneURL|Cat One", "CatTwoURL|Cat Two" }
                };

                dummySearchResult.Count = resultsCount;
                expectedTotalMessage = $"{resultsCount} result{(resultsCount == 1 ? string.Empty : "s")} found";
                expectedViewModel.TotalResultsMessage = expectedTotalMessage;
                if (expectedViewModel.TotalPages > 1)
                {
                    expectedViewModel.PageNumber = currentPage;
                    expectedViewModel.NextPageUrl = new Uri($"{defaultSearchResultsPage}?searchTerm={HttpUtility.UrlEncode(searchTerm)}&page={currentPage + 1}", UriKind.RelativeOrAbsolute);
                    expectedViewModel.NextPageUrlText = $"{currentPage + 1} of {expectedViewModel.TotalPages}";
                    if (currentPage > 1)
                    {
                        expectedViewModel.PreviousPageUrl = new Uri($"{defaultSearchResultsPage}?searchTerm={HttpUtility.UrlEncode(searchTerm)}&page={currentPage - 1}", UriKind.RelativeOrAbsolute);
                        expectedViewModel.PreviousPageUrlText = $"{currentPage - 1} of {expectedViewModel.TotalPages}";
                    }
                }

                expectedViewModel.PageNumber = currentPage;
                dummySearchResult.Results = A.CollectionOfDummy<SearchResultItem<JobProfileIndex>>(resultsCount);
                var rawResultItems = new List<SearchResultItem<JobProfileIndex>>();
                for (int i = 0; i < resultsCount; i++)
                {
                    rawResultItems.Add(new SearchResultItem<JobProfileIndex> { ResultItem = dummyIndex });
                }

                dummySearchResult.Results = rawResultItems;
                var expectedSearchResultsRawViewModel = new List<JobProfileSearchResultItemViewModel>();
                foreach (var dummyIndexItem in dummySearchResult.Results)
                {
                    expectedSearchResultsRawViewModel.Add(new JobProfileSearchResultItemViewModel
                    {
                        ResultItemTitle = dummyIndexItem.ResultItem.Title,
                        ResultItemAlternativeTitle = string.Join(", ", dummyIndexItem.ResultItem.AlternativeTitle).Trim().TrimEnd(','),
                        ResultItemSalaryRange = string.Format(new CultureInfo("en-GB", false), "{0:C0} to {1:C0}", dummyIndexItem.ResultItem.SalaryStarter, dummyIndexItem.ResultItem.SalaryExperienced),
                        ResultItemOverview = dummyIndexItem.ResultItem.Overview,
                        ResultItemUrlName = $"{defaultJobProfilePage}{dummyIndexItem.ResultItem.UrlName}",
                        Rank = (int)dummyIndexItem.Rank,
                        JobProfileCategoriesWithUrl = dummyIndexItem.ResultItem.JobProfileCategoriesWithUrl
                    });
                }

                expectedSearchResultsViewModel = expectedSearchResultsRawViewModel.AsEnumerable();
                expectedViewModel.SearchResults = expectedSearchResultsViewModel;
            }

            //Set-up calls
            A.CallTo(() => queryServiceFake.SearchAsync(A<string>._, A<SearchProperties>._)).Returns(dummySearchResult);

            //Instantiate
            var searchController = new JobProfileSearchBoxController(queryServiceFake, webAppContext, mapperCfg.CreateMapper(), loggerFake, fakeAsyncHelper, fakeSpellChecker)
            {
                CurrentPageMode = mode,
                JobProfileDetailsPage = defaultJobProfilePage,
                PageSize = pageSize,
                SearchResultsPage = defaultSearchResultsPage,
                PlaceholderText = placeholderText
            };

            //Act
            var searchMethodCall = searchController.WithCallTo(c => c.Index(searchTerm, page));

            if (mode == SearchWidgetPageMode.SearchResults)
            {
                //Assert
                searchMethodCall
                    .ShouldRenderView("SearchResult")
                    .WithModel<JobProfileSearchResultViewModel>(vm =>
                    {
                        vm.Should().BeEquivalentTo(expectedViewModel);
                    })
                    .AndNoModelErrors()
                    ;

                A.CallTo(() => queryServiceFake.SearchAsync(A<string>._, A<SearchProperties>.That.Matches(m => m.Page == page && m.Count == pageSize))).MustHaveHappened();
                A.CallTo(() => fakeSpellChecker.CheckSpellingAsync(A<string>._)).MustHaveHappened();
            }
        }

        [Theory]
        [InlineData("Test")]
        public void IndexUrlTest(string urlName)
        {
            //Setup Fakes & dummies
            var serviceFake = A.Fake<ISearchQueryService<JobProfileIndex>>(ops => ops.Strict());
            var loggerFake = A.Fake<IApplicationLogger>();
            var webAppContext = A.Fake<IWebAppContext>(ops => ops.Strict());
            var defaultJobProfilePage = "/jobprofile-details/";
            var spellcheckerServiceFake = A.Fake<ISpellcheckService>(ops => ops.Strict());
            var fakeAsyncHelper = new AsyncHelper();
            var mapperCfg = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<JobProfilesAutoMapperProfile>();
            });

            //Instantiate
            var searchController = new JobProfileSearchBoxController(serviceFake, webAppContext, mapperCfg.CreateMapper(), loggerFake, fakeAsyncHelper, spellcheckerServiceFake)
            {
                JobProfileDetailsPage = defaultJobProfilePage
            };

            //Act
            var searchMethodCall = searchController.WithCallTo(c => c.Index(urlName));

            //Assert
            searchMethodCall
                .ShouldRenderView("JobProfile")
                .WithModel<JobProfileSearchBoxViewModel>(vm =>
                {
                    vm.PlaceholderText.Should().NotBeNullOrWhiteSpace();
                    vm.HeaderText.Should().BeEquivalentTo(searchController.HeaderText);
                    vm.JobProfileUrl.OriginalString.Should().BeEquivalentTo(urlName);
                })
                .AndNoModelErrors();
        }

        [Theory]
        [InlineData(null, SearchWidgetPageMode.Landing)]
        [InlineData("", SearchWidgetPageMode.Landing)]
        [InlineData("Test", SearchWidgetPageMode.Landing)]
        [InlineData(null, SearchWidgetPageMode.SearchResults)]
        [InlineData("", SearchWidgetPageMode.SearchResults)]
        [InlineData("Test", SearchWidgetPageMode.SearchResults)]
        public void IndexTest(string searchTerm, SearchWidgetPageMode mode, int page = 1)
        {
            //Setup Fakes & dummies
            var serviceFake = A.Fake<ISearchQueryService<JobProfileIndex>>(ops => ops.Strict());
            var loggerFake = A.Fake<IApplicationLogger>();
            var webAppContext = A.Fake<IWebAppContext>(ops => ops.Strict());
            var defaultJobProfilePage = "/jobprofile-details/";
            var expectedTotalMessage = string.IsNullOrWhiteSpace(searchTerm) ? null : "0 results found - try again using a different job title";
            var spellcheckerServiceFake = A.Fake<ISpellcheckService>();
            var fakeAsyncHelper = new AsyncHelper();

            var mapperCfg = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<JobProfilesAutoMapperProfile>();
            });

            var expectedSearchResultsViewModel = Enumerable.Empty<JobProfileSearchResultItemViewModel>();
            var dummySearchResult = A.Dummy<SearchResult<JobProfileIndex>>();

            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                dummySearchResult.Results = Enumerable.Empty<SearchResultItem<JobProfileIndex>>();
                dummySearchResult.Count = 0;
            }
            else
            {
                var dummyIndex = new JobProfileIndex
                {
                    Title = searchTerm,
                    AlternativeTitle = new[] { "alt" },
                    SalaryStarter = 10,
                    SalaryExperienced = 10,
                    Overview = "overview",
                    UrlName = "dummy-url",
                    JobProfileCategoriesWithUrl = new[] { "CatOneURL|Cat One", "CatTwoURL|Cat Two" }
                };

                dummySearchResult.Count = 1;
                expectedTotalMessage = "1 result found";

                dummySearchResult.Results = A.CollectionOfDummy<SearchResultItem<JobProfileIndex>>(1);
                dummySearchResult.Results.First().ResultItem = dummyIndex;

                expectedSearchResultsViewModel = new List<JobProfileSearchResultItemViewModel>
                {
                    new JobProfileSearchResultItemViewModel
                    {
                        ResultItemTitle = dummyIndex.Title,
                        ResultItemAlternativeTitle = string.Join(", ", dummyIndex.AlternativeTitle).Trim().TrimEnd(','),
                        ResultItemSalaryRange = string.Format(new CultureInfo("en-GB", false), "{0:C0} to {1:C0}", dummyIndex.SalaryStarter, dummyIndex.SalaryExperienced),
                        ResultItemOverview = dummyIndex.Overview,
                        ResultItemUrlName = $"{defaultJobProfilePage}{dummyIndex.UrlName}",
                        Rank = (int)dummySearchResult.Results.First().Rank,
                        JobProfileCategoriesWithUrl = dummyIndex.JobProfileCategoriesWithUrl
                    }
                }.AsEnumerable();
            }

            //Set-up calls
            A.CallTo(() => serviceFake.SearchAsync(A<string>._, A<SearchProperties>._)).Returns(dummySearchResult);

            //Instantiate
            var searchController = new JobProfileSearchBoxController(serviceFake, webAppContext, mapperCfg.CreateMapper(), loggerFake, fakeAsyncHelper, spellcheckerServiceFake)
            {
                CurrentPageMode = mode,
                JobProfileDetailsPage = defaultJobProfilePage
            };

            //Act
            var searchMethodCall = searchController.WithCallTo(c => c.Index(searchTerm, page));

            if (mode == SearchWidgetPageMode.SearchResults)
            {
                //Assert
                searchMethodCall
                    .ShouldRenderView("SearchResult")
                    .WithModel<JobProfileSearchResultViewModel>(vm =>
                    {
                        vm.SearchTerm.Should().BeEquivalentTo(searchTerm);
                        vm.PlaceholderText.Should().NotBeNullOrWhiteSpace();
                        vm.TotalResultsMessage.Should().Be(expectedTotalMessage);
                        vm.SearchResults.Should().NotBeNull();
                        vm.SearchResults.Should().BeEquivalentTo(expectedSearchResultsViewModel);
                    })
                    .AndNoModelErrors()
                    ;

                if (string.IsNullOrWhiteSpace(searchTerm))
                {
                    A.CallTo(() => serviceFake.SearchAsync(A<string>._, A<SearchProperties>._)).MustNotHaveHappened();
                }
                else
                {
                    A.CallTo(() => serviceFake.SearchAsync(A<string>.That.Matches(m => m == searchTerm), A<SearchProperties>._))
                        .MustHaveHappened(Repeated.Exactly.Once);
                }
            }
            else
            {
                searchMethodCall
                    .ShouldRenderDefaultView()
                    .WithModel<JobProfileSearchBoxViewModel>(vm =>
                    {
                        vm.PlaceholderText.Should().NotBeNullOrWhiteSpace();
                    });

                A.CallTo(() => serviceFake.SearchAsync(A<string>._, A<SearchProperties>._)).MustNotHaveHappened();
            }
        }

        [Theory]
        [InlineData(null, SearchWidgetPageMode.Landing, "")]
        [InlineData("", SearchWidgetPageMode.Landing, "")]
        [InlineData("Test", SearchWidgetPageMode.Landing, "")]
        [InlineData(null, SearchWidgetPageMode.SearchResults, "")]
        [InlineData("", SearchWidgetPageMode.SearchResults, "")]
        [InlineData("Test", SearchWidgetPageMode.SearchResults, "")]
        [InlineData("", SearchWidgetPageMode.JobProfile, "test")]
        [InlineData("Test", SearchWidgetPageMode.JobProfile, "tes")]
        public void SearchTest(string searchTerm, SearchWidgetPageMode mode, string jobProfileUrl)
        {
            //Setup Fakes & dummies
            var serviceFake = A.Fake<ISearchQueryService<JobProfileIndex>>(ops => ops.Strict());
            var loggerFake = A.Fake<IApplicationLogger>();
            var spellcheckerServiceFake = A.Fake<ISpellcheckService>(ops => ops.Strict());
            var fakeAsyncHelper = new AsyncHelper();
            var webAppContext = A.Fake<IWebAppContext>(ops => ops.Strict());
            var mapperCfg = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<JobProfilesAutoMapperProfile>();
            });

            A.CallTo(() => webAppContext.IsValidAndFormattedUrl(A<string>._)).Returns(true);

            //Instantiate
            var searchController = new JobProfileSearchBoxController(serviceFake, webAppContext, mapperCfg.CreateMapper(), loggerFake, fakeAsyncHelper, spellcheckerServiceFake)
            {
                CurrentPageMode = mode
            };

            //Act
            var searchMethodCall = searchController.WithCallTo(c => c.Search(searchTerm, jobProfileUrl));

            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                if (mode == SearchWidgetPageMode.Landing)
                {
                    //Assert
                    searchMethodCall
                        .ShouldRenderView("Index") //this is not default for this action
                        .WithModel<JobProfileSearchBoxViewModel>(vm => string.IsNullOrWhiteSpace(vm.PlaceholderText) == false);
                }
                else if (mode == SearchWidgetPageMode.SearchResults)
                {
                    //Assert
                    searchMethodCall
                        .ShouldRenderView("SearchResult")
                        .WithModel<JobProfileSearchResultViewModel>(vm =>
                            string.IsNullOrWhiteSpace(vm.PlaceholderText) == false);
                }
                else
                {
                    //Assert
                    searchMethodCall
                        .ShouldRedirectTo($"{searchController.JobProfileDetailsPage}{jobProfileUrl}");
                }
            }
            else
            {
                //Assert
                searchMethodCall
                    .ShouldRedirectTo($"{searchController.SearchResultsPage}?searchTerm={searchTerm}");
            }

            A.CallTo(() => serviceFake.Search(A<string>._, A<SearchProperties>._)).MustNotHaveHappened();
        }

        [Theory]
        [InlineData("", "")]
        [InlineData("Techer", "Teacher")]
        public void SuggestionsTest(string searchTerm, string expectation)
        {
            //Setup Fakes & dummies
            var serviceFake = A.Fake<ISearchQueryService<JobProfileIndex>>(ops => ops.Strict());
            var loggerFake = A.Fake<IApplicationLogger>();
            var spellcheckerServiceFake = A.Fake<ISpellcheckService>(ops => ops.Strict());
            var fakeAsyncHelper = new AsyncHelper();
            var webAppContext = A.Fake<IWebAppContext>(ops => ops.Strict());
            var mapperCfg = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<JobProfilesAutoMapperProfile>();
            });

            var dummySuggestResult = new SuggestionResult<JobProfileIndex>();
            dummySuggestResult.Results = new List<SuggestionResultItem<JobProfileIndex>>
            {
                new SuggestionResultItem<JobProfileIndex>() { MatchedSuggestion = expectation.ToLower() }
            };

            //Set-up calls
            A.CallTo(() => serviceFake.GetSuggestion(A<string>._, A<SuggestProperties>._)).Returns(dummySuggestResult);

            //Instantiate
            var searchController = new JobProfileSearchBoxController(serviceFake, webAppContext, mapperCfg.CreateMapper(), loggerFake, fakeAsyncHelper, spellcheckerServiceFake);

            var result = searchController.WithCallTo(c => c.Suggestions(searchTerm, 5, true));
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                result.ShouldReturnEmptyResult();
                A.CallTo(() => serviceFake.GetSuggestion(A<string>._, A<SuggestProperties>._)).MustNotHaveHappened();
            }
            else
            {
                result.ShouldReturnJson(json =>
                {
                    var suggestions = json as IEnumerable<Suggestion>;
                    suggestions.First().label.Should().Be(expectation);
                });
                A.CallTo(() => serviceFake.GetSuggestion(A<string>.That.Matches(m => m == searchTerm), A<SuggestProperties>._))
                    .MustHaveHappened(Repeated.Exactly.Once);
            }
        }

        [Theory]
        [InlineData(SearchWidgetPageMode.JobProfile, "ZAP%n%s%n%s%n%s%n%s%n%s%n%s%n%s%n%s%n%s%n%s%n%s%n%s%n%s%n%s%n%s%n%s%n%s%n%s%n%s%n%se", HttpStatusCode.NotFound)]
        [InlineData(SearchWidgetPageMode.JobProfile, "Nurse", HttpStatusCode.Redirect)]
        [InlineData(SearchWidgetPageMode.JobProfile, "<invalid>", HttpStatusCode.NotFound)]
        [InlineData(SearchWidgetPageMode.JobProfile, "inval#d", HttpStatusCode.NotFound)]
        public void ShouldReturnBadRequestIfTheUrlIsinvalidFormatlInRequest(SearchWidgetPageMode mode, string jobProfileUrl, HttpStatusCode expectation)
        {
            //Arrange
            var webAppContext = new WebAppContext();   // OUR SERVICE TYPE
            var searchQuery = A.Fake<ISearchQueryService<JobProfileIndex>>();
            var mapper = A.Fake<IMapper>();
            var applicationLogger = A.Fake<IApplicationLogger>();
            var spellcheckerServiceFake = A.Fake<ISpellcheckService>(ops => ops.Strict());
            var fakeAsyncHelper = new AsyncHelper();

            var controller = new JobProfileSearchBoxController(searchQuery, webAppContext, mapper, applicationLogger, fakeAsyncHelper, spellcheckerServiceFake)
            {
                CurrentPageMode = mode
            };

            switch (expectation)
            {
                case HttpStatusCode.NotFound:
                    controller.WithCallTo(c => c.Search(string.Empty, jobProfileUrl)).ShouldGiveHttpStatus(expectation);
                    break;

                case HttpStatusCode.Redirect:
                default:
                    controller.WithCallTo(c => c.Search(string.Empty, jobProfileUrl)).ShouldRedirectTo($"{controller.JobProfileDetailsPage}{jobProfileUrl}");
                    break;
            }
        }

        // DFC-1494 Did you mean - spell checker service
        [Theory]
        [InlineData("test", false)]
        [InlineData("test", true)]
        public void SpellcheckerServiceTest(string searchTerm, bool validSpellcheckResult)
        {
            //Setup Fakes & dummies
            var serviceFake = A.Fake<ISearchQueryService<JobProfileIndex>>(ops => ops.Strict());
            var loggerFake = A.Fake<IApplicationLogger>();
            var spellcheckerServiceFake = A.Fake<ISpellcheckService>(ops => ops.Strict());
            var fakeAsyncHelper = new AsyncHelper();
            var mapperCfg = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<JobProfilesAutoMapperProfile>();
            });
            var webAppContext = A.Fake<IWebAppContext>(ops => ops.Strict());   // OUR SERVICE TYPE
            var dummySearchResult = A.Dummy<SearchResult<JobProfileIndex>>();
            dummySearchResult.Results = Enumerable.Empty<SearchResultItem<JobProfileIndex>>();
            dummySearchResult.Count = 0;
            var defaultJobProfilePage = nameof(JobProfileSearchBoxController.JobProfileDetailsPage);

            var dummySpellCheckResult = validSpellcheckResult ? new SpellcheckResult
            {
                CorrectedTerm = $"dummy {searchTerm}",
                HasCorrected = true
            }
            : new SpellcheckResult();

            var searchResultsPage = nameof(JobProfileSearchBoxController.SearchResultsPage);
            var correctedTermUrl = $"{searchResultsPage}?searchTerm={HttpUtility.UrlEncode(dummySpellCheckResult.CorrectedTerm)}";

            //Set-up calls
            A.CallTo(() => spellcheckerServiceFake.CheckSpellingAsync(A<string>._)).Returns(dummySpellCheckResult);
            A.CallTo(() => serviceFake.SearchAsync(A<string>._, A<SearchProperties>._)).Returns(dummySearchResult);

            //Instantiate
            var searchController = new JobProfileSearchBoxController(serviceFake, webAppContext, mapperCfg.CreateMapper(), loggerFake, fakeAsyncHelper, spellcheckerServiceFake)
            {
                SearchResultsPage = searchResultsPage,
                CurrentPageMode = SearchWidgetPageMode.SearchResults,
                JobProfileDetailsPage = defaultJobProfilePage
            };

            //Act
            var searchMethodCall = searchController.WithCallTo(c => c.Index(searchTerm, 1));

            if (validSpellcheckResult)
            {
                searchMethodCall
                    .ShouldRenderView("SearchResult")
                    .WithModel<JobProfileSearchResultViewModel>(vm =>
                    {
                        vm.DidYouMeanUrl.OriginalString.Should().BeEquivalentTo(correctedTermUrl);
                        vm.DidYouMeanTerm.Should().NotBeNullOrEmpty();
                    });

                A.CallTo(() => spellcheckerServiceFake.CheckSpellingAsync(A<string>._)).MustHaveHappened();
            }
            else
            {
                searchMethodCall
                    .ShouldRenderView("SearchResult")
                    .WithModel<JobProfileSearchResultViewModel>(vm =>
                    {
                        vm.DidYouMeanUrl?.OriginalString.Should().BeNullOrEmpty();
                        vm.DidYouMeanTerm.Should().BeNullOrEmpty();
                    });
                A.CallTo(() => spellcheckerServiceFake.CheckSpellingAsync(A<string>._)).MustHaveHappened();
            }
        }
    }
}