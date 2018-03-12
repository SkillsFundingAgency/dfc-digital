using DFC.Digital.Core;
using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using DFC.Digital.Web.Sitefinity.Core.Interface;
using DFC.Digital.Web.Sitefinity.JobProfileModule.Mvc.Controllers;
using DFC.Digital.Web.Sitefinity.JobProfileModule.Mvc.Models;
using FakeItEasy;
using FluentAssertions;
using System.Collections.Generic;
using TestStack.FluentMVCTesting;
using Xunit;

namespace DFC.Digital.Web.Sitefinity.JobProfileModule.UnitTests
{
    public class JobProfileRelatedCareersControllerTests
    {
        private const string JobProfileWithRelatedCareers = "JobProfileWithRelatedCareers";
        private const string JobProfileWithOutRelatedCareers = "JobProfileWithOutRelatedCareers";

        [Theory]
        [InlineData(true, 5, true)]
        [InlineData(false, 2, true)]
        [InlineData(true, 5, false)]
        [InlineData(false, 2, false)]
        public void IndexDefaultTest(bool inContentAuthoringSite, int numberLinksToDisplay, bool isContentPreviewMode)
        {
            //Setup the fakes and dummies
            var repositoryFake = A.Fake<IJobProfileRelatedCareersRepository>(ops => ops.Strict());
            var loggerFake = A.Fake<IApplicationLogger>();
            var webAppContextFake = A.Fake<IWebAppContext>(ops => ops.Strict());
            var sitefinityPage = A.Fake<ISitefinityPage>(ops => ops.Strict());

            // Set up calls
            A.CallTo(() => repositoryFake.GetByParentName(A<string>._, numberLinksToDisplay)).Returns(GetTestRelatedCareers(numberLinksToDisplay));
            A.CallTo(() => repositoryFake.GetByParentNameForPreview(A<string>._, numberLinksToDisplay)).Returns(GetTestRelatedCareers(numberLinksToDisplay));

            A.CallTo(() => webAppContextFake.IsContentPreviewMode).Returns(isContentPreviewMode);
            A.CallTo(() => webAppContextFake.IsContentAuthoringSite).Returns(inContentAuthoringSite);

            A.CallTo(() => sitefinityPage.GetDefaultJobProfileToUse(A<string>._)).ReturnsLazily((string defaultProfile) => defaultProfile);

            //Instantiate & Act
            var jobProfileRelatedCareersController = new JobProfileRelatedCareersController(repositoryFake, webAppContextFake, loggerFake, sitefinityPage);

            //Act
            var indexResult = jobProfileRelatedCareersController.WithCallTo(c => c.Index());

            //Assert
            if (inContentAuthoringSite)
            {
                indexResult.ShouldRenderDefaultView().WithModel<JobProfileRelatedCareersModel>(vm =>
                {
                    vm.Title.Should().BeEquivalentTo(jobProfileRelatedCareersController.SectionTitle);
                    vm.RelatedCareers.Should().BeEquivalentTo(GetTestRelatedCareers(numberLinksToDisplay));
                }).AndNoModelErrors();

                if (!isContentPreviewMode)
                {
                    A.CallTo(() => repositoryFake.GetByParentName(A<string>._, numberLinksToDisplay)).MustHaveHappened();
                }
                else
                {
                    A.CallTo(() => repositoryFake.GetByParentNameForPreview(A<string>._, numberLinksToDisplay)).MustHaveHappened();
                }
            }
            else
            {
                indexResult.ShouldRedirectTo("\\");
            }
        }

        [Theory]
        [InlineData(JobProfileWithRelatedCareers, 5, true)]
        [InlineData(JobProfileWithOutRelatedCareers, 5, true)]
        [InlineData(JobProfileWithRelatedCareers, 5, false)]
        [InlineData(JobProfileWithOutRelatedCareers, 5, false)]
        public void IndexWithJobProfileUrlTest(string jobProfileUrl, int numberLinksToDisplay, bool isContentPreviewMode)
        {
            //Setup the fakes and dummies
            var repositoryFake = A.Fake<IJobProfileRelatedCareersRepository>(ops => ops.Strict());
            var loggerFake = A.Fake<IApplicationLogger>();
            var webAppContextFake = A.Fake<IWebAppContext>(ops => ops.Strict());
            var sitefinityPage = A.Fake<ISitefinityPage>(ops => ops.Strict());

            // Set up calls
            A.CallTo(() => webAppContextFake.IsContentPreviewMode).Returns(isContentPreviewMode);

            A.CallTo(() => repositoryFake.GetByParentName(JobProfileWithRelatedCareers, numberLinksToDisplay)).Returns(GetTestRelatedCareers(numberLinksToDisplay));
            A.CallTo(() => repositoryFake.GetByParentName(JobProfileWithOutRelatedCareers, numberLinksToDisplay)).Returns(null);
            A.CallTo(() => repositoryFake.GetByParentNameForPreview(JobProfileWithRelatedCareers, numberLinksToDisplay)).Returns(GetTestRelatedCareers(numberLinksToDisplay));
            A.CallTo(() => repositoryFake.GetByParentNameForPreview(JobProfileWithOutRelatedCareers, numberLinksToDisplay)).Returns(null);

            A.CallTo(() => sitefinityPage.GetDefaultJobProfileToUse(A<string>._)).ReturnsLazily((string defaultProfile) => defaultProfile);

            //Instantiate & Act
            var jobProfileRelatedCareersController = new JobProfileRelatedCareersController(repositoryFake, webAppContextFake, loggerFake, sitefinityPage);

            //Act
            var indexResult = jobProfileRelatedCareersController.WithCallTo(c => c.Index(jobProfileUrl));

            //Assert
            if (jobProfileUrl == JobProfileWithRelatedCareers)
            {
                indexResult.ShouldRenderDefaultView().WithModel<JobProfileRelatedCareersModel>(vm =>
                {
                    vm.Title.Should().BeEquivalentTo(jobProfileRelatedCareersController.SectionTitle);
                    vm.RelatedCareers.Should().BeEquivalentTo(GetTestRelatedCareers(numberLinksToDisplay));
                })
                .AndNoModelErrors();

                if (!isContentPreviewMode)
                {
                    A.CallTo(() => repositoryFake.GetByParentName(A<string>._, numberLinksToDisplay)).MustHaveHappened();
                }
                else
                {
                    A.CallTo(() => repositoryFake.GetByParentNameForPreview(A<string>._, numberLinksToDisplay)).MustHaveHappened();
                }
            }
            else
            {
                indexResult.ShouldReturnEmptyResult();
            }
        }

        private IEnumerable<JobProfileRelatedCareer> GetTestRelatedCareers(int numberToReturn)
        {
            for (int ii = 0; ii < numberToReturn; ii++)
            {
                yield return new JobProfileRelatedCareer() { Title = $"Title {ii}", ProfileLink = $"http://link{ii}" };
            }
        }
    }
}