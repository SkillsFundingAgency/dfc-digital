using AutoMapper;
using DFC.Digital.Core;
using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using DFC.Digital.Web.Sitefinity.Core.Interface;
using DFC.Digital.Web.Sitefinity.JobProfileModule.Config;
using DFC.Digital.Web.Sitefinity.JobProfileModule.Mvc.Controllers;
using DFC.Digital.Web.Sitefinity.JobProfileModule.Mvc.Models;
using FakeItEasy;
using FluentAssertions;
using System.Collections.Generic;
using System.Threading.Tasks;
using TestStack.FluentMVCTesting;
using Xunit;

namespace DFC.Digital.Web.Sitefinity.JobProfileModule.UnitTests.Controllers
{
    /// <summary>
    ///     Job Profile Details Controller tests
    /// </summary>
    public class JobProfileDetailsControllerTests
    {
        private readonly IAsyncHelper asyncHelper = new AsyncHelper();
        private readonly double experiencedSalary = 200;
        private readonly IGovUkNotify govUkNotifyFake = A.Fake<IGovUkNotify>(ops => ops.Strict());
        private readonly IApplicationLogger loggerFake = A.Fake<IApplicationLogger>(ops => ops.Strict());
        private readonly IJobProfileRepository repositoryFake = A.Fake<IJobProfileRepository>(ops => ops.Strict());
        private readonly ISitefinityPage sitefinityPage = A.Fake<ISitefinityPage>(ops => ops.Strict());
        private readonly ISearchQueryService<JobProfileIndex> searchQueryService = A.Fake<ISearchQueryService<JobProfileIndex>>(ops => ops.Strict());
        private readonly double starterSalary = 100;
        private readonly IWebAppContext webAppContextFake = A.Fake<IWebAppContext>();
        private JobProfile dummyJobProfile;
        private MapperConfiguration mapperCfg;

        [Theory]
        [InlineData(true, true)]
        [InlineData(false, true)]
        [InlineData(true, false)]
        [InlineData(false, false)]
        public void IndexTest(bool inContentAuthoringSite, bool isContentPreviewMode)
        {
            //Set up comman call
            SetUpDependeciesAndCall(true, isContentPreviewMode);

            //Set up calls particular to this test case
            A.CallTo(() => webAppContextFake.IsContentAuthoringSite).Returns(inContentAuthoringSite);

            //Instantiate & Act
            using (var jobprofileController = new JobProfileDetailsController(
                webAppContextFake, repositoryFake, loggerFake, sitefinityPage, mapperCfg.CreateMapper(), asyncHelper, searchQueryService))
            {
                //Act
                var indexMethodCall = jobprofileController.WithCallTo(c => c.Index());

                //Assert
                //should get back a default profile for design mode
                if (inContentAuthoringSite)
                {
                    indexMethodCall
                        .ShouldRenderDefaultView()
                        .WithModel<JobProfileDetailsViewModel>(vm =>
                        {
                            vm.SalaryText.ShouldBeEquivalentTo(jobprofileController.SalaryText);
                            vm.HoursText.ShouldBeEquivalentTo(jobprofileController.HoursText);
                            vm.MaxAndMinHoursAreBlankText.ShouldBeEquivalentTo(jobprofileController
                                .MaxAndMinHoursAreBlankText);
                            vm.HoursTimePeriodText.ShouldBeEquivalentTo(jobprofileController.HoursTimePeriodText);
                            vm.AlternativeTitle.ShouldBeEquivalentTo(dummyJobProfile.AlternativeTitle);
                            vm.Overview.ShouldBeEquivalentTo(dummyJobProfile.Overview);
                            vm.Title.ShouldBeEquivalentTo(dummyJobProfile.Title);
                            vm.MaximumHours.ShouldBeEquivalentTo(dummyJobProfile.MaximumHours);
                            vm.MinimumHours.ShouldBeEquivalentTo(dummyJobProfile.MinimumHours);
                            vm.SalaryStarter.ShouldBeEquivalentTo(starterSalary);
                            vm.SalaryExperienced.ShouldBeEquivalentTo(experiencedSalary);
                        })
                        .AndNoModelErrors();

                    AssertActions(isContentPreviewMode);
                }
                else
                {
                    indexMethodCall.ShouldRedirectTo("\\");
                }
            }
        }

        [Theory]
        [InlineData("Test", true, true)]
        [InlineData("Test", false, true)]
        [InlineData("Test", true, false)]
        [InlineData("Test", false, false)]
        public void IndexUrlNameTest(string urlName, bool validJobProfile, bool isContentPreviewMode)
        {
            //Set up comman call
            SetUpDependeciesAndCall(validJobProfile, isContentPreviewMode);

            //Instantiate & Act
            using (var jobprofileController = new JobProfileDetailsController(
                webAppContextFake, repositoryFake, loggerFake, sitefinityPage, mapperCfg.CreateMapper(), asyncHelper, searchQueryService))
            {
                //Act
                var indexWithUrlNameMethodCall = jobprofileController.WithCallTo(c => c.Index(urlName));

                if (validJobProfile)
                {
                    indexWithUrlNameMethodCall
                        .ShouldRenderDefaultView()
                        .WithModel<JobProfileDetailsViewModel>(vm =>
                        {
                            vm.SalaryText.ShouldBeEquivalentTo(jobprofileController.SalaryText);
                            vm.HoursText.ShouldBeEquivalentTo(jobprofileController.HoursText);
                            vm.MaxAndMinHoursAreBlankText.ShouldBeEquivalentTo(jobprofileController
                                .MaxAndMinHoursAreBlankText);
                            vm.HoursTimePeriodText.ShouldBeEquivalentTo(jobprofileController.HoursTimePeriodText);
                            vm.AlternativeTitle.ShouldBeEquivalentTo(dummyJobProfile.AlternativeTitle);
                            vm.SalaryRange.ShouldBeEquivalentTo(dummyJobProfile.SalaryRange);
                            vm.Overview.ShouldBeEquivalentTo(dummyJobProfile.Overview);
                            vm.Title.ShouldBeEquivalentTo(dummyJobProfile.Title);
                            vm.MaximumHours.ShouldBeEquivalentTo(dummyJobProfile.MaximumHours);
                            vm.MinimumHours.ShouldBeEquivalentTo(dummyJobProfile.MinimumHours);
                        })
                        .AndNoModelErrors();
                }
                else
                {
                    indexWithUrlNameMethodCall.ShouldGiveHttpStatus(404);
                }

                if (!isContentPreviewMode)
                {
                    A.CallTo(() => repositoryFake.GetByUrlName(A<string>._)).MustHaveHappened();
                }
                else
                {
                    A.CallTo(() => repositoryFake.GetByUrlNameForPreview(A<string>._)).MustHaveHappened();
                }
            }
        }

        private void SetUpDependeciesAndCall(bool validJobProfile, bool isContentPreviewMode)
        {
            ////Set up comman call
            mapperCfg = new MapperConfiguration(cfg => { cfg.AddProfile<JobProfilesAutoMapperProfile>(); });

            dummyJobProfile = validJobProfile
                ? new JobProfile
                {
                    AlternativeTitle = nameof(JobProfile.AlternativeTitle),
                    SalaryRange = nameof(JobProfile.SalaryRange),
                    Overview = nameof(JobProfile.Overview),
                    Title = nameof(JobProfile.Title),
                    MaximumHours = 40,
                    MinimumHours = 10,
                    UrlName = nameof(JobProfile.UrlName)
                }
                : null;

            var dummyIndex = new JobProfileIndex
            {
                Title = nameof(JobProfileIndex.Title),
                AlternativeTitle = new[] { "alt" },
                SalaryStarter = starterSalary,
                SalaryExperienced = experiencedSalary,
                Overview = "overview",
                UrlName = "dummy-url",
                JobProfileCategoriesWithUrl = new[] { "CatOneURL|Cat One", "CatTwoURL|Cat Two" }
            };
            var resultsCount = 1;
            var dummySearchResult = A.Dummy<SearchResult<JobProfileIndex>>();
            dummySearchResult.Count = resultsCount;
            dummySearchResult.Results = A.CollectionOfDummy<SearchResultItem<JobProfileIndex>>(resultsCount);
            var rawResultItems = new List<SearchResultItem<JobProfileIndex>>
            {
                new SearchResultItem<JobProfileIndex> { ResultItem = dummyIndex }
            };
            dummySearchResult.Results = rawResultItems;

            // Set up calls
            A.CallTo(() => searchQueryService.SearchAsync(A<string>._, A<SearchProperties>._)).Returns(dummySearchResult);
            A.CallTo(() => repositoryFake.GetByUrlName(A<string>._)).Returns(dummyJobProfile);
            A.CallTo(() => repositoryFake.GetByUrlNameForPreview(A<string>._)).Returns(dummyJobProfile);
            A.CallTo(() => webAppContextFake.IsContentPreviewMode).Returns(isContentPreviewMode);
            A.CallTo(() => sitefinityPage.GetDefaultJobProfileToUse(A<string>._))
                .ReturnsLazily((string defaultProfile) => defaultProfile);
            A.CallTo(() => govUkNotifyFake.SubmitEmail(A<string>._, null)).Returns(false);
            A.CallTo(() => webAppContextFake.SetVocCookie(Constants.VocPersonalisationCookieName, A<string>._)).DoesNothing();
            A.CallTo(() => loggerFake.Trace(A<string>._)).DoesNothing();
        }

        private void AssertActions(bool isContentPreviewMode)
        {
            if (!isContentPreviewMode)
            {
                A.CallTo(() => repositoryFake.GetByUrlName(A<string>._)).MustHaveHappened();
                A.CallTo(() => webAppContextFake.IsContentPreviewMode).MustHaveHappened();
                A.CallTo(() => searchQueryService.SearchAsync(A<string>.That.IsEqualTo(dummyJobProfile.Title), A<SearchProperties>._)).MustHaveHappened();
                A.CallTo(() => repositoryFake.GetByUrlNameForPreview(A<string>._)).MustNotHaveHappened();
                A.CallTo(() => sitefinityPage.GetDefaultJobProfileToUse(A<string>._)).MustHaveHappened();
            }
            else
            {
                A.CallTo(() => repositoryFake.GetByUrlNameForPreview(A<string>._)).MustHaveHappened();
                A.CallTo(() => sitefinityPage.GetDefaultJobProfileToUse(A<string>._)).MustHaveHappened();
                A.CallTo(() => webAppContextFake.IsContentPreviewMode).MustHaveHappened();
                A.CallTo(() => searchQueryService.SearchAsync(A<string>.That.IsEqualTo(dummyJobProfile.Title), A<SearchProperties>._)).MustHaveHappened();
                A.CallTo(() => sitefinityPage.GetDefaultJobProfileToUse(A<string>._)).MustHaveHappened();
                A.CallTo(() => repositoryFake.GetByUrlName(A<string>._)).MustNotHaveHappened();
            }
        }
    }
}