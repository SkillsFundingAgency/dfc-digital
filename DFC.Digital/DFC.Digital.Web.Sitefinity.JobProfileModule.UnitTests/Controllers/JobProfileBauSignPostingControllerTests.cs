using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using DFC.Digital.Web.Sitefinity.Core.Interface;
using DFC.Digital.Web.Sitefinity.JobProfileModule.Mvc.Controllers;
using DFC.Digital.Web.Sitefinity.JobProfileModule.Mvc.Models;
using FakeItEasy;
using FluentAssertions;
using TestStack.FluentMVCTesting;
using Xunit;

namespace DFC.Digital.Web.Sitefinity.JobProfileModule.UnitTests.Controllers
{
    public class JobProfileBauSignPostingControllerTests
    {
        private readonly IApplicationLogger loggerFake = A.Fake<IApplicationLogger>(ops => ops.Strict());
        private readonly IJobProfileRepository repositoryFake = A.Fake<IJobProfileRepository>(ops => ops.Strict());
        private readonly ISitefinityPage sitefinityPage = A.Fake<ISitefinityPage>(ops => ops.Strict());
        private readonly IWebAppContext webAppContextFake = A.Fake<IWebAppContext>();
        private JobProfile dummyJobProfile;

        [Theory]
        [InlineData("BetaJPUrl", false, "", false, "job-profiles/BetaJPUrl")]
        [InlineData("BetaJPUrl", true, "", false, "job-profiles/BetaJPUrl")]
        [InlineData("BetaJPUrl", true, "BAUJPUrl", true, "job-profiles/home")]
        [InlineData("BetaJPUrl", false, "BAUJPUrl", true, "job-profiles/BAUJPUrl")]
        public void SignPostingTest(string urlName, bool doesNotExistInBau, string overRideBauUrl, bool isContentAuthoring, string expectedJpurl)
        {
            //Set up comman call
            SetUpDependeciesAndCall(true, isContentAuthoring);

            dummyJobProfile.BAUSystemOverrideUrl = overRideBauUrl;
            dummyJobProfile.DoesNotExistInBAU = doesNotExistInBau;
            dummyJobProfile.UrlName = urlName;

            //Instantiate & Act
            using (var jobprofileController = new JobProfileBauSignPostingController(webAppContextFake, repositoryFake, loggerFake, sitefinityPage))
            {
                //Act
                var indexNameMethodCall = jobprofileController.WithCallTo(c => c.Index());

                //Assert
                if (isContentAuthoring)
                {
                    indexNameMethodCall.ShouldRenderDefaultView()
                        .WithModel<BauJpSignPostViewModel>(vm =>
                        {
                            vm.SignPostingHtml.Should().Contain(expectedJpurl);
                        }).AndNoModelErrors();
                }
                else
                {
                    indexNameMethodCall.ShouldRedirectTo("\\");
                }
            }
        }

        [Theory]
        [InlineData("BetaJPUrl", false, "", "job-profiles/BetaJPUrl")]
        [InlineData("BetaJPUrl", true, "", "job-profiles/home")]
        [InlineData("BetaJPUrl", true, "BAUJPUrl", "job-profiles/home")]
        [InlineData("BetaJPUrl", false, "BAUJPUrl", "job-profiles/BAUJPUrl")]
        public void SignPostingUrlTest(string urlName, bool doesNotExistInBau, string overRideBauUrl, string expectedJpurl)
        {
            //Set up comman call
            SetUpDependeciesAndCall(true, false);

            dummyJobProfile.BAUSystemOverrideUrl = overRideBauUrl;
            dummyJobProfile.DoesNotExistInBAU = doesNotExistInBau;
            dummyJobProfile.UrlName = urlName;

            //Instantiate & Act
            using (var jobprofileController = new JobProfileBauSignPostingController(webAppContextFake, repositoryFake, loggerFake, sitefinityPage))
            {
                //Act
                var indexWithUrlNameMethodCall = jobprofileController.WithCallTo(c => c.Index(urlName));

                //Assert
                indexWithUrlNameMethodCall.ShouldRenderDefaultView()
                    .WithModel<BauJpSignPostViewModel>(vm =>
                    {
                        vm.SignPostingHtml.Should().Contain(expectedJpurl);
                    }).AndNoModelErrors();
            }
        }

        private void SetUpDependeciesAndCall(bool validJobProfile, bool isContentPreviewMode)
        {
            ////Set up comman call
            dummyJobProfile = validJobProfile
                ? new JobProfile
                {
                    AlternativeTitle = nameof(JobProfile.AlternativeTitle),
                    SalaryRange = nameof(JobProfile.SalaryRange),
                    Overview = nameof(JobProfile.Overview),
                    Title = nameof(JobProfile.Title),
                    MaximumHours = 40,
                    MinimumHours = 10
                }
                : null;

            // Set up calls
            A.CallTo(() => repositoryFake.GetByUrlName(A<string>._)).Returns(dummyJobProfile);
            A.CallTo(() => repositoryFake.GetByUrlNameForPreview(A<string>._)).Returns(dummyJobProfile);
            A.CallTo(() => webAppContextFake.IsContentAuthoringSite).Returns(isContentPreviewMode);
            A.CallTo(() => sitefinityPage.GetDefaultJobProfileToUse(A<string>._))
                .ReturnsLazily((string defaultProfile) => defaultProfile);
        }
    }
}
