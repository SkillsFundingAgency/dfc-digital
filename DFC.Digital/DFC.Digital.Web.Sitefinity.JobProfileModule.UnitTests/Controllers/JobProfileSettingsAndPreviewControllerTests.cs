using DFC.Digital.Core;
using DFC.Digital.Data.Interfaces;
using DFC.Digital.Web.Sitefinity.JobProfileModule.Mvc.Controllers;
using DFC.Digital.Web.Sitefinity.JobProfileModule.Mvc.Models;
using FakeItEasy;
using FluentAssertions;
using TestStack.FluentMVCTesting;
using Xunit;

namespace DFC.Digital.Web.Sitefinity.JobProfileModule.UnitTests
{
    public class JobProfileSettingsAndPreviewControllerTests
    {
        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void IndexDefaultTest(bool inContentAuthoringSiteAndNotPreview)
        {
            //Setup the fakes and dummies
            var repositoryFake = A.Fake<IJobProfileRepository>(ops => ops.Strict());
            var loggerFake = A.Fake<IApplicationLogger>();
            var webAppContextFake = A.Fake<IWebAppContext>(ops => ops.Strict());

            // Set up calls
            A.CallTo(() => webAppContextFake.IsContentAuthoringAndNotPreviewMode).Returns(inContentAuthoringSiteAndNotPreview);

            //Instantiate & Act
            var jobProfileSettingsAndPreviewController = new JobProfileSettingsAndPreviewController(repositoryFake, webAppContextFake, loggerFake);

            //Act
            var indexResult = jobProfileSettingsAndPreviewController.WithCallTo(c => c.Index());

            //Assert
            if (inContentAuthoringSiteAndNotPreview)
            {
                indexResult.ShouldRenderDefaultView().WithModel<JobProfileSettingsAndPreviewModel>(vm =>
                {
                    vm.DefaultJobProfileUrl.Should().BeEquivalentTo(jobProfileSettingsAndPreviewController.DefaultJobProfileUrlName);
                })
               .AndNoModelErrors();
            }
            else
            {
                indexResult.ShouldReturnEmptyResult();
            }
        }

        [Theory]
        [InlineData("something", false, true)]
        [InlineData("something", true, false)]
        [InlineData("", false, false)]
        public void IndexDefaultUrlTest(string urlName, bool isContentAuthoringSite, bool expectation)
        {
            //Setup the fakes and dummies
            var repositoryFake = A.Fake<IJobProfileRepository>(ops => ops.Strict());
            var loggerFake = A.Fake<IApplicationLogger>();
            var webAppContextFake = A.Fake<IWebAppContext>(ops => ops.Strict());

            // Set up calls
            A.CallTo(() => webAppContextFake.IsContentAuthoringSite).Returns(isContentAuthoringSite);
            A.CallTo(() => webAppContextFake.SetVocCookie(Constants.VocPersonalisationCookieName, A<string>._)).DoesNothing();

            //Instantiate & Act
            var jobProfileSettingsAndPreviewController = new JobProfileSettingsAndPreviewController(repositoryFake, webAppContextFake, loggerFake);

            //Act
            jobProfileSettingsAndPreviewController.WithCallTo(c => c.Index(urlName));

            //Assert
            if (expectation)
            {
                A.CallTo(() => webAppContextFake.SetVocCookie(Constants.VocPersonalisationCookieName, A<string>.That.IsEqualTo(urlName))).MustHaveHappened();
            }
            else
            {
                A.CallTo(() => webAppContextFake.SetVocCookie(Constants.VocPersonalisationCookieName, A<string>._)).MustNotHaveHappened();
            }
        }

        [Theory]
        [InlineData("something", true)]
        [InlineData("", false)]
        public void SetVocCookieTest(string urlName, bool expectation)
        {
            //Setup the fakes and dummies
            var repositoryFake = A.Fake<IJobProfileRepository>(ops => ops.Strict());
            var loggerFake = A.Fake<IApplicationLogger>();
            var webAppContextFake = A.Fake<IWebAppContext>(ops => ops.Strict());

            // Set up calls
            A.CallTo(() => webAppContextFake.SetVocCookie(Constants.VocPersonalisationCookieName, A<string>._)).DoesNothing();

            //Instantiate & Act
            var jobProfileSettingsAndPreviewController = new JobProfileSettingsAndPreviewController(repositoryFake, webAppContextFake, loggerFake);

            //Act
            jobProfileSettingsAndPreviewController.WithCallTo(c => c.SetVocCookie(urlName));

            //Assert
            if (expectation)
            {
                A.CallTo(() => webAppContextFake.SetVocCookie(Constants.VocPersonalisationCookieName, A<string>.That.IsEqualTo(urlName))).MustNotHaveHappened();
            }
            else
            {
                A.CallTo(() => webAppContextFake.SetVocCookie(Constants.VocPersonalisationCookieName, A<string>._)).MustNotHaveHappened();
            }
        }
    }
}