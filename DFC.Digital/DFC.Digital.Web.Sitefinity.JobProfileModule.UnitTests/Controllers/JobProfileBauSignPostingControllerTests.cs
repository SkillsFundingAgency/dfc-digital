using AutoMapper;
using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using DFC.Digital.Web.Sitefinity.Core.Interface;
using DFC.Digital.Web.Sitefinity.JobProfileModule.Config;
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

        //The tests above are already testing the behaviour of the widget in diffent modes, content authoring, valid\invalid job profiles
        //This is just to check the signposting logic in the controller
        [Theory]
        [InlineData("BetaJPUrl", false, "")]
        [InlineData("BetaJPUrl", true, "")]
        [InlineData("BetaJPUrl", true, "BAUJPUrl")]
        public void SignPostingUrlTest(string urlName, bool doesNotExistInBau, string overRideBauurl)
        {
            //Set up comman call
            SetUpDependeciesAndCall(true, false);

            dummyJobProfile.BAUSystemOverrideUrl = overRideBauurl;
            dummyJobProfile.DoesNotExistInBAU = doesNotExistInBau;
            dummyJobProfile.UrlName = urlName;

            //Instantiate & Act
            var jobprofileController = new JobProfileBauSignPostingController(webAppContextFake, repositoryFake, loggerFake, sitefinityPage);

            //Act
            var indexWithUrlNameMethodCall = jobprofileController.WithCallTo(c => c.Index(urlName));

            string expectedJpurl;

            if (doesNotExistInBau)
            {
                //Does not exist in BAU point to home
                expectedJpurl = "job-profiles/home";
            }
            else
            {
                if (overRideBauurl == string.Empty)
                {
                    expectedJpurl = $"job-profiles/{urlName}";
                }
                else
                {
                    expectedJpurl = $"job-profiles/{overRideBauurl}";
                }
            }

            //Assert
            indexWithUrlNameMethodCall.ShouldRenderDefaultView()
                .WithModel<BauJpSignPostViewModel>(vm =>
                {
                    vm.DisplaySignPostingToBau.ShouldBeEquivalentTo(true);
                    vm.SignPostingHtml.Should().Contain(expectedJpurl);
                }).AndNoModelErrors();
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
            A.CallTo(() => webAppContextFake.IsContentPreviewMode).Returns(isContentPreviewMode);
            A.CallTo(() => sitefinityPage.GetDefaultJobProfileToUse(A<string>._))
                .ReturnsLazily((string defaultProfile) => defaultProfile);
        }
    }
}
