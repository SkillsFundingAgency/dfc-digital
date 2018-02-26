using AutoMapper;
using DFC.Digital.Core.Utilities;
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
        private readonly decimal experiencedSalary = 200;
        private readonly IGovUkNotify govUkNotifyFake = A.Fake<IGovUkNotify>(ops => ops.Strict());
        private readonly IApplicationLogger loggerFake = A.Fake<IApplicationLogger>(ops => ops.Strict());
        private readonly IJobProfileRepository repositoryFake = A.Fake<IJobProfileRepository>(ops => ops.Strict());
        private readonly ISalaryCalculator salaryCalculator = A.Fake<ISalaryCalculator>(ops => ops.Strict());
        private readonly ISalaryService salaryService = A.Fake<ISalaryService>(ops => ops.Strict());
        private readonly ISitefinityPage sitefinityPage = A.Fake<ISitefinityPage>(ops => ops.Strict());
        private readonly decimal starterSalary = 100;
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
                webAppContextFake, repositoryFake, loggerFake, sitefinityPage, mapperCfg.CreateMapper(), salaryService, salaryCalculator, asyncHelper))
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
                        vm.WorkingHoursDetails.ShouldBeEquivalentTo(dummyJobProfile.WorkingHoursDetails);
                        vm.WorkingPattern.ShouldBeEquivalentTo(dummyJobProfile.WorkingPattern);
                        vm.WorkingPatternDetails.ShouldBeEquivalentTo(dummyJobProfile.WorkingPatternDetails);
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
                webAppContextFake, repositoryFake, loggerFake, sitefinityPage, mapperCfg.CreateMapper(), salaryService, salaryCalculator, asyncHelper))
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
                        vm.WorkingHoursDetails.ShouldBeEquivalentTo(dummyJobProfile.WorkingHoursDetails);
                        vm.WorkingPattern.ShouldBeEquivalentTo(dummyJobProfile.WorkingPattern);
                        vm.WorkingPatternDetails.ShouldBeEquivalentTo(dummyJobProfile.WorkingPatternDetails);
                        vm.SalaryStarter.ShouldBeEquivalentTo(starterSalary);
                        vm.SalaryExperienced.ShouldBeEquivalentTo(experiencedSalary);
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
                    WorkingHoursDetails = nameof(JobProfile.WorkingHoursDetails),
                    WorkingPattern = nameof(JobProfile.WorkingPattern),
                    WorkingPatternDetails = nameof(JobProfile.WorkingPatternDetails)
                }
                : null;

            var dummySalary = new JobProfileSalary
            {
                Deciles = new Dictionary<int, decimal>
                {
                    { 10, 100 },
                    { 90, 200 }
                }
            };

            // Set up calls
            A.CallTo(() => repositoryFake.GetByUrlName(A<string>._)).Returns(dummyJobProfile);
            A.CallTo(() => repositoryFake.GetByUrlNameForPreview(A<string>._)).Returns(dummyJobProfile);
            A.CallTo(() => webAppContextFake.IsContentPreviewMode).Returns(isContentPreviewMode);
            A.CallTo(() => sitefinityPage.GetDefaultJobProfileToUse(A<string>._))
                .ReturnsLazily((string defaultProfile) => defaultProfile);
            A.CallTo(() => govUkNotifyFake.SubmitEmail(A<string>._, null)).Returns(false);
            A.CallTo(() => salaryService.GetSalaryBySocAsync(A<string>._)).Returns(Task.FromResult(dummySalary));
            A.CallTo(() => salaryCalculator.GetStarterSalary(A<JobProfileSalary>._)).Returns(starterSalary);
            A.CallTo(() => salaryCalculator.GetExperiencedSalary(A<JobProfileSalary>._)).Returns(experiencedSalary);
            A.CallTo(() => webAppContextFake.SetVocCookie(Constants.VocPersonalisationCookieName, A<string>._)).DoesNothing();
        }

        private void AssertActions(bool isContentPreviewMode)
        {
            if (!isContentPreviewMode)
            {
                A.CallTo(() => repositoryFake.GetByUrlName(A<string>._)).MustHaveHappened();
                A.CallTo(() => webAppContextFake.IsContentPreviewMode).MustHaveHappened();
                A.CallTo(() => salaryService.GetSalaryBySocAsync(A<string>.That.IsEqualTo(dummyJobProfile.SOCCode))).MustHaveHappened();
                A.CallTo(() => salaryCalculator.GetStarterSalary(A<JobProfileSalary>._)).MustHaveHappened();
                A.CallTo(() => salaryCalculator.GetExperiencedSalary(A<JobProfileSalary>._)).MustHaveHappened();
                A.CallTo(() => repositoryFake.GetByUrlNameForPreview(A<string>._)).MustNotHaveHappened();
                A.CallTo(() => sitefinityPage.GetDefaultJobProfileToUse(A<string>._)).MustHaveHappened();
            }
            else
            {
                A.CallTo(() => repositoryFake.GetByUrlNameForPreview(A<string>._)).MustHaveHappened();
                A.CallTo(() => sitefinityPage.GetDefaultJobProfileToUse(A<string>._)).MustHaveHappened();
                A.CallTo(() => webAppContextFake.IsContentPreviewMode).MustHaveHappened();
                A.CallTo(() => salaryService.GetSalaryBySocAsync(A<string>.That.IsEqualTo(dummyJobProfile.SOCCode))).MustHaveHappened();
                A.CallTo(() => salaryCalculator.GetStarterSalary(A<JobProfileSalary>._)).MustHaveHappened();
                A.CallTo(() => salaryCalculator.GetExperiencedSalary(A<JobProfileSalary>._)).MustHaveHappened();
                A.CallTo(() => sitefinityPage.GetDefaultJobProfileToUse(A<string>._)).MustHaveHappened();
                A.CallTo(() => repositoryFake.GetByUrlName(A<string>._)).MustNotHaveHappened();
            }
        }
    }
}