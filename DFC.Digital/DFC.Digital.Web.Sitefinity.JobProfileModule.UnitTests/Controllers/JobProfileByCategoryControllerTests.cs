using DFC.Digital.Core;
using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using DFC.Digital.Web.Sitefinity.JobProfileModule.Mvc.Controllers;
using DFC.Digital.Web.Sitefinity.JobProfileModule.Mvc.Models;
using FakeItEasy;
using FluentAssertions;
using TestStack.FluentMVCTesting;
using Xunit;

namespace DFC.Digital.Web.Sitefinity.JobProfileModule.Tests.Controllers
{
    public class JobProfileByCategoryControllerTests
    {
        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void IndexTest(bool inContentAuthoringSite)
        {
            //Setup the fakes and dummies
            var repositoryFake = A.Fake<IJobProfileCategoryRepository>(ops => ops.Strict());
            var loggerFake = A.Fake<IApplicationLogger>();
            var webAppContextFake = A.Fake<IWebAppContext>(ops => ops.Strict());

            var dummyJobProfileCategory = A.Dummy<JobProfileCategory>();
            var dummyRelatedJobProfiles = A.CollectionOfDummy<JobProfile>(5);

            // Set up calls
            A.CallTo(() => webAppContextFake.IsContentAuthoringSite).Returns(inContentAuthoringSite);
            A.CallTo(() => repositoryFake.GetByUrlName(A<string>._)).Returns(dummyJobProfileCategory);
            A.CallTo(() => repositoryFake.GetRelatedJobProfiles(A<string>._)).Returns(dummyRelatedJobProfiles);

            //Instantiate & Act
            var jobProfilesByCategoryController = new JobProfilesByCategoryController(repositoryFake, webAppContextFake, loggerFake);

            //Act
            var indexMethodCall = jobProfilesByCategoryController.WithCallTo(c => c.Index());

            //Assert
            if (inContentAuthoringSite)
            {
                indexMethodCall
                    .ShouldRenderDefaultView()
                    .WithModel<JobProfileByCategoryViewModel>(vm =>
                    {
                        vm.Title.ShouldBeEquivalentTo(dummyJobProfileCategory.Title);
                        vm.Description.ShouldBeEquivalentTo(dummyJobProfileCategory.Description);
                        vm.JobProfiles.ShouldBeEquivalentTo(dummyRelatedJobProfiles);
                    })
                    .AndNoModelErrors();

                A.CallTo(() => repositoryFake.GetRelatedJobProfiles(A<string>._)).MustHaveHappened();
            }
            else
            {
                indexMethodCall.ShouldRedirectTo("\\");
            }
        }

        [Theory]
        [InlineData("Test", true)]
        [InlineData("Test", false)]
        public void IndexUrlNameTest(string urlName, bool validJobCategory)
        {
            //Setup the fakes and dummies
            var repositoryFake = A.Fake<IJobProfileCategoryRepository>(ops => ops.Strict());
            var webAppContextFake = A.Fake<IWebAppContext>(ops => ops.Strict());
            var loggerFake = A.Fake<IApplicationLogger>();

            var dummyJobProfileCategory = A.Dummy<JobProfileCategory>();
            var dummyRelatedJobProfiles = A.CollectionOfDummy<JobProfile>(5);

            // Set up calls
            A.CallTo(() => repositoryFake.GetByUrlName(A<string>._)).Returns(validJobCategory ? dummyJobProfileCategory : null);

            A.CallTo(() => repositoryFake.GetRelatedJobProfiles(A<string>._)).Returns(dummyRelatedJobProfiles);

            //Instantiate & Act
            var jobprofileController = new JobProfilesByCategoryController(repositoryFake, webAppContextFake, loggerFake);

            //Act
            var indexWithUrlNameMethodCall = jobprofileController.WithCallTo(c => c.Index(urlName));

            //Assert
            A.CallTo(() => repositoryFake.GetByUrlName(A<string>._)).MustHaveHappened();

            if (validJobCategory)
            {
                indexWithUrlNameMethodCall
                    .ShouldRenderDefaultView()
                    .WithModel<JobProfileByCategoryViewModel>(vm =>
                    {
                        vm.Title.ShouldBeEquivalentTo(dummyJobProfileCategory.Title);
                        vm.Description.ShouldBeEquivalentTo(dummyJobProfileCategory.Description);
                        vm.JobProfiles.ShouldBeEquivalentTo(dummyRelatedJobProfiles);
                    })
                    .AndNoModelErrors();

                A.CallTo(() => repositoryFake.GetRelatedJobProfiles(A<string>._)).MustHaveHappened();
            }
            else
            {
                indexWithUrlNameMethodCall.ShouldGiveHttpStatus(404);
            }
        }
    }
}