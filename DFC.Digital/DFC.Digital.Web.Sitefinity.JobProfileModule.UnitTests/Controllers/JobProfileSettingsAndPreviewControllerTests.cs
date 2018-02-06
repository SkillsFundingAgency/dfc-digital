using DFC.Digital.Data.Interfaces;
using DFC.Digital.Web.Sitefinity.JobProfileModule.Mvc.Controllers;
using DFC.Digital.Web.Sitefinity.JobProfileModule.Mvc.Models;
using FakeItEasy;
using FluentAssertions;
using TestStack.FluentMVCTesting;
using Xunit;

namespace DFC.Digital.Web.Sitefinity.JobProfileModule.UnitTests.Controllers
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
            var loggerFake = A.Fake<IApplicationLogger>(ops => ops.Strict());
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
                    vm.DefaultJobProfileUrl.ShouldBeEquivalentTo(jobProfileSettingsAndPreviewController.DefaultJobProfileUrlName);
                })
               .AndNoModelErrors();
            }
            else
            {
                indexResult.ShouldReturnEmptyResult();
            }
        }
    }
}