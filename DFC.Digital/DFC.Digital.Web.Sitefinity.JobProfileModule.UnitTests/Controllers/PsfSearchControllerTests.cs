using AutoMapper;
using DFC.Digital.Core.Utilities;
using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using DFC.Digital.Web.Sitefinity.JobProfileModule.Config;
using DFC.Digital.Web.Sitefinity.JobProfileModule.Mvc.Controllers;
using DFC.Digital.Web.Sitefinity.JobProfileModule.Mvc.Models;
using FakeItEasy;
using FluentAssertions;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using TestStack.FluentMVCTesting;
using Xunit;

namespace DFC.Digital.Web.Sitefinity.JobProfileModule.UnitTests.Controllers
{
    public class PsfSearchControllerTests
    {
        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void IndexTest(bool isContentAuthoringSite)
        {
            var searchServiceFake = A.Fake<ISearchQueryService<JobProfileIndex>>(ops => ops.Strict());
            var loggerFake = A.Fake<IApplicationLogger>(ops => ops.Strict());
            var webAppContextFake = A.Fake<IWebAppContext>(ops => ops.Strict());
            var stateManagerFake = A.Fake<IPreSearchFilterStateManager>(ops => ops.Strict());
            var asyncHelper = new AsyncHelper();
            var defaultJobProfilePage = "/jobprofile-details/";
            var mapperCfg = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<JobProfilesAutoMapperProfile>();
            });
            var buildSearchFilterServiceFake = A.Fake<IBuildSearchFilterService>(ops => ops.Strict());

            // Set up calls
            A.CallTo(() => webAppContextFake.IsContentAuthoringSite).Returns(isContentAuthoringSite);
            var expectedSearchResultsViewModel = Enumerable.Empty<JobProfileSearchResultItemViewModel>();
            var dummySearchResult = A.Dummy<SearchResult<JobProfileIndex>>();
            var expectedTotalMessage = "1 result found";

            var dummyIndex = new JobProfileIndex
            {
                Title = nameof(JobProfileIndex.Title),
                AlternativeTitle = new[] { "alt" },
                SalaryStarter = 10,
                SalaryExperienced = 10,
                Overview = "overview",
                UrlName = "dummy-url",
                JobProfileCategoriesWithUrl = new[] { "CatOneURL|Cat One", "CatTwoURL|Cat Two" }
            };

            dummySearchResult.Count = 1;
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

            A.CallTo(() => searchServiceFake.SearchAsync(A<string>._, A<SearchProperties>._))
                .Returns(dummySearchResult);
            A.CallTo(() => buildSearchFilterServiceFake.BuildPreSearchFilters(A<PreSearchFiltersResultsModel>._, A<Dictionary<string, PreSearchFilterLogicalOperator>>._))
                .Returns(nameof(SearchProperties.FilterBy));
            A.CallTo(() => stateManagerFake.GetPreSearchFilterState()).Returns(new PreSearchFilterState());
            A.CallTo(() => stateManagerFake.GetStateJson()).Returns(string.Empty);
            A.CallTo(() => stateManagerFake.RestoreState(A<string>._)).DoesNothing();
            A.CallTo(() => stateManagerFake.UpdateSectionState(A<PreSearchFilterSection>._)).DoesNothing();

            //Instantiate & Act
            var psfSearchController = new PsfSearchController(searchServiceFake, webAppContextFake, mapperCfg.CreateMapper(), asyncHelper, buildSearchFilterServiceFake, stateManagerFake, loggerFake)
            {
                JobProfileDetailsPage = defaultJobProfilePage
            };

            var indexMethodCall = psfSearchController.WithCallTo(c => c.Index());

            //Assert
            if (!isContentAuthoringSite)
            {
                indexMethodCall.ShouldRedirectTo("\\");
            }
            else
            {
                indexMethodCall
                    .ShouldRenderView("SearchResult")
                    .WithModel<PsfSearchResultsViewModel>(vm =>
                    {
                        vm.MainPageTitle.Should().Be(psfSearchController.MainPageTitle);
                        vm.SecondaryText.ShouldBeEquivalentTo(psfSearchController.SecondaryText);
                        vm.TotalResultsMessage.Should().Be(expectedTotalMessage);
                        vm.SearchResults.Should().NotBeNull();
                        vm.SearchResults.ShouldBeEquivalentTo(expectedSearchResultsViewModel);
                        vm.BackPageUrl.ShouldBeEquivalentTo(psfSearchController.BackPageUrl);
                        vm.BackPageUrlText.ShouldBeEquivalentTo(psfSearchController.BackPageUrlText);
                    })
                    .AndNoModelErrors();

                A.CallTo(() => searchServiceFake.SearchAsync(A<string>._, A<SearchProperties>._)).MustHaveHappened();
                A.CallTo(() => buildSearchFilterServiceFake.BuildPreSearchFilters(A<PreSearchFiltersResultsModel>._, A<Dictionary<string, PreSearchFilterLogicalOperator>>._))
                    .MustHaveHappened();
                A.CallTo(() => stateManagerFake.RestoreState(A<string>._)).MustHaveHappened();
                A.CallTo(() => stateManagerFake.GetPreSearchFilterState()).MustHaveHappened();
                A.CallTo(() => stateManagerFake.GetStateJson()).MustHaveHappened();
            }
        }

        [Theory]
        [InlineData(1, true, true)]
        [InlineData(4, false, true)]
        [InlineData(50, true, false)]
        public void SearchTest(int resultCount, bool notPaging, bool singleSelectValue)
        {
            var searchServiceFake = A.Fake<ISearchQueryService<JobProfileIndex>>(ops => ops.Strict());
            var loggerFake = A.Fake<IApplicationLogger>(ops => ops.Strict());
            var asyncHelper = new AsyncHelper();
            var webAppContextFake = A.Fake<IWebAppContext>(ops => ops.Strict());
            var stateManagerFake = A.Fake<IPreSearchFilterStateManager>(ops => ops.Strict());
            var defaultJobProfilePage = "/jobprofile-details/";
            var mapperCfg = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<JobProfilesAutoMapperProfile>();
            });
            var buildSearchFilterServiceFake = A.Fake<IBuildSearchFilterService>(ops => ops.Strict());

            // Set up calls
            var expectedSearchResultsViewModel = Enumerable.Empty<JobProfileSearchResultItemViewModel>();
            var dummySearchResult = A.Dummy<SearchResult<JobProfileIndex>>();
            var expectedTotalMessage = resultCount == 1 ? "1 result found" : $"{resultCount} results found";

            var dummyIndex = new JobProfileIndex
            {
                Title = nameof(JobProfileIndex.Title),
                AlternativeTitle = new[] { "alt" },
                SalaryStarter = 10,
                SalaryExperienced = 10,
                Overview = "overview",
                UrlName = "dummy-url",
                JobProfileCategoriesWithUrl = new[] { "CatOneURL|Cat One", "CatTwoURL|Cat Two" }
            };

            dummySearchResult.Count = resultCount;
            var endList = new List<SearchResultItem<JobProfileIndex>>(resultCount);
            for (var i = 0; i < resultCount; i++)
            {
                endList.Add(new SearchResultItem<JobProfileIndex>
                {
                    ResultItem = dummyIndex
                });
            }

            dummySearchResult.Results = endList;

            var expectedVmList = new List<JobProfileSearchResultItemViewModel>();
            foreach (var dummyResult in dummySearchResult.Results)
            {
                expectedVmList.Add(
                new JobProfileSearchResultItemViewModel
                {
                    ResultItemTitle = dummyResult.ResultItem.Title,
                    ResultItemAlternativeTitle = string.Join(", ", dummyResult.ResultItem.AlternativeTitle).Trim().TrimEnd(','),
                    ResultItemSalaryRange = string.Format(new CultureInfo("en-GB", false), "{0:C0} to {1:C0}", dummyResult.ResultItem.SalaryStarter, dummyResult.ResultItem.SalaryExperienced),
                    ResultItemOverview = dummyResult.ResultItem.Overview,
                    ResultItemUrlName = $"{defaultJobProfilePage}{dummyResult.ResultItem.UrlName}",
                    Rank = (int)dummyResult.Rank,
                    JobProfileCategoriesWithUrl = dummyResult.ResultItem.JobProfileCategoriesWithUrl
                });
            }

            expectedSearchResultsViewModel = expectedVmList.AsEnumerable();

            A.CallTo(() => searchServiceFake.SearchAsync(A<string>._, A<SearchProperties>._))
                .Returns(dummySearchResult);
            A.CallTo(() => buildSearchFilterServiceFake.BuildPreSearchFilters(A<PreSearchFiltersResultsModel>._, A<Dictionary<string, PreSearchFilterLogicalOperator>>._))
                .Returns(nameof(SearchProperties.FilterBy));
            A.CallTo(() => stateManagerFake.GetPreSearchFilterState()).Returns(new PreSearchFilterState());
            A.CallTo(() => stateManagerFake.GetStateJson()).Returns(string.Empty);
            A.CallTo(() => stateManagerFake.RestoreState(A<string>._)).DoesNothing();
            A.CallTo(() => stateManagerFake.UpdateSectionState(A<PreSearchFilterSection>._)).DoesNothing();

            //Instantiate & Act
            var psfSearchController = new PsfSearchController(searchServiceFake, webAppContextFake, mapperCfg.CreateMapper(), asyncHelper, buildSearchFilterServiceFake, stateManagerFake, loggerFake)
            {
                JobProfileDetailsPage = defaultJobProfilePage
            };

            var searchMethodCall = psfSearchController.WithCallTo(c => c.Search(new PsfModel { Section = new PsfSection { SingleSelectedValue = singleSelectValue ? nameof(PsfSection.SingleSelectedValue) : string.Empty, Options = new List<PsfOption>() } }, 1, notPaging));

            //Assert
            searchMethodCall
                .ShouldRenderView("SearchResult")
                .WithModel<PsfSearchResultsViewModel>(vm =>
                {
                    vm.MainPageTitle.Should().Be(psfSearchController.MainPageTitle);
                    vm.SecondaryText.ShouldBeEquivalentTo(psfSearchController.SecondaryText);
                    vm.TotalResultsMessage.Should().Be(expectedTotalMessage);
                    vm.SearchResults.Should().NotBeNull();
                    vm.SearchResults.ShouldBeEquivalentTo(expectedSearchResultsViewModel);
                    vm.BackPageUrl.ShouldBeEquivalentTo(psfSearchController.BackPageUrl);
                    vm.BackPageUrlText.ShouldBeEquivalentTo(psfSearchController.BackPageUrlText);
                })
                .AndNoModelErrors();

            if (notPaging)
            {
                A.CallTo(() => stateManagerFake.UpdateSectionState(A<PreSearchFilterSection>._)).MustHaveHappened();
            }

            A.CallTo(() => searchServiceFake.SearchAsync(A<string>._, A<SearchProperties>._)).MustHaveHappened();
            A.CallTo(() => buildSearchFilterServiceFake.BuildPreSearchFilters(A<PreSearchFiltersResultsModel>._, A<Dictionary<string, PreSearchFilterLogicalOperator>>._))
                .MustHaveHappened();
            A.CallTo(() => stateManagerFake.RestoreState(A<string>._)).MustHaveHappened();
            A.CallTo(() => stateManagerFake.GetPreSearchFilterState()).MustHaveHappened();
            A.CallTo(() => stateManagerFake.GetStateJson()).MustHaveHappened();
        }

        [Theory]
        [InlineData(1, 1, false, false)]
        [InlineData(12, 2, false, true)]
        [InlineData(50, 3, true, true)]
        public void IndexSearchTest(int resultCount, int pageNumber, bool hasNextPage, bool hasPreviousPage)
        {
            var searchServiceFake = A.Fake<ISearchQueryService<JobProfileIndex>>(ops => ops.Strict());
            var loggerFake = A.Fake<IApplicationLogger>(ops => ops.Strict());
            var asyncHelper = new AsyncHelper();
            var webAppContextFake = A.Fake<IWebAppContext>(ops => ops.Strict());
            var stateManagerFake = A.Fake<IPreSearchFilterStateManager>(ops => ops.Strict());
            var defaultJobProfilePage = "/jobprofile-details/";
            var mapperCfg = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<JobProfilesAutoMapperProfile>();
            });
            var buildSearchFilterServiceFake = A.Fake<IBuildSearchFilterService>(ops => ops.Strict());

            // Set up calls
            var expectedSearchResultsViewModel = Enumerable.Empty<JobProfileSearchResultItemViewModel>();
            var dummySearchResult = A.Dummy<SearchResult<JobProfileIndex>>();
            var expectedTotalMessage = resultCount == 1 ? "1 result found" : $"{resultCount} results found";

            var dummyIndex = new JobProfileIndex
            {
                Title = nameof(JobProfileIndex.Title),
                AlternativeTitle = new[] { "alt" },
                SalaryStarter = 10,
                SalaryExperienced = 10,
                Overview = "overview",
                UrlName = "dummy-url",
                JobProfileCategoriesWithUrl = new[] { "CatOneURL|Cat One", "CatTwoURL|Cat Two" }
            };

            dummySearchResult.Count = resultCount;
            var endList = new List<SearchResultItem<JobProfileIndex>>(resultCount);
            for (var i = 0; i < resultCount; i++)
            {
                endList.Add(new SearchResultItem<JobProfileIndex>
                {
                    ResultItem = dummyIndex
                });
            }

            dummySearchResult.Results = endList;

            var expectedVmList = new List<JobProfileSearchResultItemViewModel>();
            foreach (var dummyResult in dummySearchResult.Results)
            {
                expectedVmList.Add(
                new JobProfileSearchResultItemViewModel
                {
                    ResultItemTitle = dummyResult.ResultItem.Title,
                    ResultItemAlternativeTitle = string.Join(", ", dummyResult.ResultItem.AlternativeTitle).Trim().TrimEnd(','),
                    ResultItemSalaryRange = string.Format(new CultureInfo("en-GB", false), "{0:C0} to {1:C0}", dummyResult.ResultItem.SalaryStarter, dummyResult.ResultItem.SalaryExperienced),
                    ResultItemOverview = dummyResult.ResultItem.Overview,
                    ResultItemUrlName = $"{defaultJobProfilePage}{dummyResult.ResultItem.UrlName}",
                    Rank = (int)dummyResult.Rank,
                    JobProfileCategoriesWithUrl = dummyResult.ResultItem.JobProfileCategoriesWithUrl
                });
            }

            expectedSearchResultsViewModel = expectedVmList.AsEnumerable();

            A.CallTo(() => searchServiceFake.SearchAsync(A<string>._, A<SearchProperties>._)).Returns(dummySearchResult);
            A.CallTo(() => buildSearchFilterServiceFake.BuildPreSearchFilters(A<PreSearchFiltersResultsModel>._, A<Dictionary<string, PreSearchFilterLogicalOperator>>._))
                .Returns(nameof(SearchProperties.FilterBy));
            A.CallTo(() => stateManagerFake.GetPreSearchFilterState()).Returns(new PreSearchFilterState());
            A.CallTo(() => stateManagerFake.GetStateJson()).Returns(string.Empty);
            A.CallTo(() => stateManagerFake.RestoreState(A<string>._)).DoesNothing();
            A.CallTo(() => stateManagerFake.UpdateSectionState(A<PreSearchFilterSection>._)).DoesNothing();

            //Instantiate & Act
            var psfSearchController = new PsfSearchController(searchServiceFake, webAppContextFake, mapperCfg.CreateMapper(), asyncHelper, buildSearchFilterServiceFake, stateManagerFake, loggerFake)
            {
                JobProfileDetailsPage = defaultJobProfilePage
            };

            var searchMethodCall = psfSearchController.WithCallTo(c => c.Index(new PsfModel { Sections = new List<PsfSection>(), Section = new PsfSection { Options = new List<PsfOption>() } }, new PsfSearchResultsViewModel(), pageNumber));

            //Assert
            searchMethodCall
                .ShouldRenderView("SearchResult")
                .WithModel<PsfSearchResultsViewModel>(vm =>
                {
                    vm.MainPageTitle.Should().Be(psfSearchController.MainPageTitle);
                    vm.SecondaryText.ShouldBeEquivalentTo(psfSearchController.SecondaryText);
                    vm.TotalResultsMessage.Should().Be(expectedTotalMessage);
                    vm.SearchResults.Should().NotBeNull();
                    vm.SearchResults.ShouldBeEquivalentTo(expectedSearchResultsViewModel);
                    vm.BackPageUrl.ShouldBeEquivalentTo(psfSearchController.BackPageUrl);
                    vm.BackPageUrlText.ShouldBeEquivalentTo(psfSearchController.BackPageUrlText);
                    vm.HasNexPage.ShouldBeEquivalentTo(hasNextPage);
                    vm.HasPreviousPage.ShouldBeEquivalentTo(hasPreviousPage);
                })
                .AndNoModelErrors();

            A.CallTo(() => searchServiceFake.SearchAsync(A<string>._, A<SearchProperties>._)).MustHaveHappened();
            A.CallTo(() => buildSearchFilterServiceFake.BuildPreSearchFilters(A<PreSearchFiltersResultsModel>._, A<Dictionary<string, PreSearchFilterLogicalOperator>>._))
                .MustHaveHappened();
            A.CallTo(() => stateManagerFake.RestoreState(A<string>._)).MustHaveHappened();
            A.CallTo(() => stateManagerFake.GetPreSearchFilterState()).MustHaveHappened();
            A.CallTo(() => stateManagerFake.GetStateJson()).MustHaveHappened();
        }

        [Theory]
        [InlineData(1)]
        [InlineData(4)]
        [InlineData(50)]
        public void IndexSearchPagingTest(int resultCount)
        {
            var searchServiceFake = A.Fake<ISearchQueryService<JobProfileIndex>>(ops => ops.Strict());
            var loggerFake = A.Fake<IApplicationLogger>(ops => ops.Strict());
            var asyncHelper = new AsyncHelper();
            var webAppContextFake = A.Fake<IWebAppContext>(ops => ops.Strict());
            var defaultJobProfilePage = "/jobprofile-details/";
            var stateManagerFake = A.Fake<IPreSearchFilterStateManager>(ops => ops.Strict());
            var mapperCfg = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<JobProfilesAutoMapperProfile>();
            });
            var buildSearchFilterServiceFake = A.Fake<IBuildSearchFilterService>(ops => ops.Strict());

            // Set up calls
            var expectedSearchResultsViewModel = Enumerable.Empty<JobProfileSearchResultItemViewModel>();
            var dummySearchResult = A.Dummy<SearchResult<JobProfileIndex>>();
            var expectedTotalMessage = resultCount == 1 ? "1 result found" : $"{resultCount} results found";

            var dummyIndex = new JobProfileIndex
            {
                Title = nameof(JobProfileIndex.Title),
                AlternativeTitle = new[] { "alt" },
                SalaryStarter = 10,
                SalaryExperienced = 10,
                Overview = "overview",
                UrlName = "dummy-url",
                JobProfileCategoriesWithUrl = new[] { "CatOneURL|Cat One", "CatTwoURL|Cat Two" }
            };

            dummySearchResult.Count = resultCount;
            var endList = new List<SearchResultItem<JobProfileIndex>>(resultCount);
            for (var i = 0; i < resultCount; i++)
            {
                endList.Add(new SearchResultItem<JobProfileIndex>
                {
                    ResultItem = dummyIndex
                });
            }

            dummySearchResult.Results = endList;

            var expectedVmList = new List<JobProfileSearchResultItemViewModel>();
            foreach (var dummyResult in dummySearchResult.Results)
            {
                expectedVmList.Add(
                new JobProfileSearchResultItemViewModel
                {
                    ResultItemTitle = dummyResult.ResultItem.Title,
                    ResultItemAlternativeTitle = string.Join(", ", dummyResult.ResultItem.AlternativeTitle).Trim().TrimEnd(','),
                    ResultItemSalaryRange = string.Format(new CultureInfo("en-GB", false), "{0:C0} to {1:C0}", dummyResult.ResultItem.SalaryStarter, dummyResult.ResultItem.SalaryExperienced),
                    ResultItemOverview = dummyResult.ResultItem.Overview,
                    ResultItemUrlName = $"{defaultJobProfilePage}{dummyResult.ResultItem.UrlName}",
                    Rank = (int)dummyResult.Rank,
                    JobProfileCategoriesWithUrl = dummyResult.ResultItem.JobProfileCategoriesWithUrl
                });
            }

            expectedSearchResultsViewModel = expectedVmList.AsEnumerable();

            A.CallTo(() => searchServiceFake.SearchAsync(A<string>._, A<SearchProperties>._)).Returns(dummySearchResult);
            A.CallTo(() => buildSearchFilterServiceFake.BuildPreSearchFilters(A<PreSearchFiltersResultsModel>._, A<Dictionary<string, PreSearchFilterLogicalOperator>>._))
                .Returns(nameof(SearchProperties.FilterBy));
            A.CallTo(() => stateManagerFake.GetPreSearchFilterState()).Returns(new PreSearchFilterState());
            A.CallTo(() => stateManagerFake.GetStateJson()).Returns(string.Empty);
            A.CallTo(() => stateManagerFake.RestoreState(A<string>._)).DoesNothing();
            A.CallTo(() => stateManagerFake.UpdateSectionState(A<PreSearchFilterSection>._)).DoesNothing();

            //Instantiate & Act
            var psfSearchController = new PsfSearchController(searchServiceFake, webAppContextFake, mapperCfg.CreateMapper(), asyncHelper, buildSearchFilterServiceFake, stateManagerFake, loggerFake)
            {
                JobProfileDetailsPage = defaultJobProfilePage
            };

            var searchMethodCall = psfSearchController.WithCallTo(c => c.Index(new PsfModel { Section = new PsfSection { Options = new List<PsfOption>() } }, new PsfSearchResultsViewModel(), 1));

            //Assert
            searchMethodCall
                .ShouldRenderView("SearchResult")
                .WithModel<PsfSearchResultsViewModel>(vm =>
                {
                    vm.MainPageTitle.Should().Be(psfSearchController.MainPageTitle);
                    vm.SecondaryText.ShouldBeEquivalentTo(psfSearchController.SecondaryText);
                    vm.TotalResultsMessage.Should().Be(expectedTotalMessage);
                    vm.SearchResults.Should().NotBeNull();
                    vm.SearchResults.ShouldBeEquivalentTo(expectedSearchResultsViewModel);
                    vm.BackPageUrl.ShouldBeEquivalentTo(psfSearchController.BackPageUrl);
                    vm.BackPageUrlText.ShouldBeEquivalentTo(psfSearchController.BackPageUrlText);
                })
                .AndNoModelErrors();

            A.CallTo(() => searchServiceFake.SearchAsync(A<string>._, A<SearchProperties>._)).MustHaveHappened();
            A.CallTo(() => buildSearchFilterServiceFake.BuildPreSearchFilters(A<PreSearchFiltersResultsModel>._, A<Dictionary<string, PreSearchFilterLogicalOperator>>._))
                .MustHaveHappened();
            A.CallTo(() => stateManagerFake.RestoreState(A<string>._)).MustHaveHappened();
            A.CallTo(() => stateManagerFake.GetPreSearchFilterState()).MustHaveHappened();
            A.CallTo(() => stateManagerFake.GetStateJson()).MustHaveHappened();
        }
    }
}