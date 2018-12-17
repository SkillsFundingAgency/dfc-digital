using DFC.Digital.Core;
using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using DFC.Digital.Web.Sitefinity.Core;
using DFC.Digital.Web.Sitefinity.Widgets.Mvc.Controllers;
using DFC.Digital.Web.Sitefinity.Widgets.Mvc.Models;
using FakeItEasy;
using FluentAssertions;
using System;
using TestStack.FluentMVCTesting;
using Xunit;

namespace DFC.Digital.Web.Sitefinity.Widgets.UnitTests
{
    public class DfcBreadcrumbControllerTests
    {
        private IJobProfileCategoryRepository repositoryCategoryFake;
        private IJobProfileRepository repositoryJobProfileFake;
        private ISitefinityCurrentContext sitefinityCurrentContext;
        private IApplicationLogger loggerFake;

        public DfcBreadcrumbControllerTests()
        {
            repositoryCategoryFake = A.Fake<IJobProfileCategoryRepository>(ops => ops.Strict());
            repositoryJobProfileFake = A.Fake<IJobProfileRepository>(ops => ops.Strict());
            sitefinityCurrentContext = A.Fake<ISitefinityCurrentContext>(ops => ops.Strict());
            loggerFake = A.Fake<IApplicationLogger>();
        }

        [Fact]
        public void IndexNoURLTest()
        {


        }

        [Theory]
        [InlineData("~/help/cookies", "Cookies")]
        [InlineData("~/search-results", "Search results")]
        [InlineData("~/alerts", "Error")]
        public void IndexTest(string nodeUrl, string nodeTitle)
        {
            //Setup the fakes and dummies
            var repositoryCategoryFake = A.Fake<IJobProfileCategoryRepository>(ops => ops.Strict());
            var repositoryJobProfileFake = A.Fake<IJobProfileRepository>(ops => ops.Strict());
            var sitefinityCurrentContext = A.Fake<ISitefinityCurrentContext>(ops => ops.Strict());
            var loggerFake = A.Fake<IApplicationLogger>();

            var dummyCategory = A.Dummy<JobProfileCategory>();
            var dummyJobProfile = A.Dummy<JobProfile>();

            var dummyDfcPageSiteNode = A.Dummy<DfcPageSiteNode>();
            dummyDfcPageSiteNode.Title = nodeTitle;
            dummyDfcPageSiteNode.Url = new Uri(nodeUrl, UriKind.RelativeOrAbsolute);

            // Set up calls
            A.CallTo(() => repositoryCategoryFake.GetByUrlName(A<string>._)).Returns(dummyCategory);
            A.CallTo(() => repositoryJobProfileFake.GetByUrlName(A<string>._)).Returns(dummyJobProfile);
            A.CallTo(() => sitefinityCurrentContext.GetCurrentDfcPageNode()).Returns(dummyDfcPageSiteNode);

            //Instantiate & Act
            var dfcBreadcrumbController = new DfcBreadcrumbController(repositoryCategoryFake, repositoryJobProfileFake, sitefinityCurrentContext, loggerFake);

            //Act
            var indexMethodCall = dfcBreadcrumbController.WithCallTo(c => c.Index());

            //Assert
            indexMethodCall
                .ShouldRenderDefaultView()
                .WithModel<DfcBreadcrumbViewModel>(vm =>
                {
                    vm.BreadcrumbLinks.Should().BeEquivalentTo(nodeTitle);
                })
                .AndNoModelErrors();
        }

        [Theory]
        [InlineData("administration", "~/job-categories", "Administration", true)]
        [InlineData("administration", "~/job-categories", "Administration", false)]
        [InlineData("border-force-officer", "~/job-profiles", "Border force officer", true)]
        [InlineData("", "~/search-results", "Search results", true)]
        [InlineData("test", "~/job-categories", "", true)]
        [InlineData("test", "~/job-profiles", "", true)]

        public void IndexUrlNameTest(string urlName, string nodeUrl, string nodeTitle, bool hasDfcPageSiteNode)
        {
            //Setup the fakes and dummies
            var repositoryCategoryFake = A.Fake<IJobProfileCategoryRepository>(ops => ops.Strict());
            var repositoryJobProfileFake = A.Fake<IJobProfileRepository>(ops => ops.Strict());
            var sitefinityCurrentContext = A.Fake<ISitefinityCurrentContext>(ops => ops.Strict());
            var loggerFake = A.Fake<IApplicationLogger>();

            var dummyCategory = A.Dummy<JobProfileCategory>();
            dummyCategory.Title = nodeTitle;
            var dummyJobProfile = A.Dummy<JobProfile>();
            dummyJobProfile.Title = nodeTitle;
            var dummyDfcPageSiteNode = A.Dummy<DfcPageSiteNode>();
            dummyDfcPageSiteNode.Title = nodeTitle;
            dummyDfcPageSiteNode.Url = new Uri(nodeUrl, UriKind.RelativeOrAbsolute);

            // Set up calls
            if (string.IsNullOrEmpty(nodeTitle))
            {
                A.CallTo(() => repositoryCategoryFake.GetByUrlName(A<string>._)).Returns(null);
                A.CallTo(() => repositoryJobProfileFake.GetByUrlName(A<string>._)).Returns(null);
            }
            else
            {
                A.CallTo(() => repositoryCategoryFake.GetByUrlName(A<string>._)).Returns(dummyCategory);
                A.CallTo(() => repositoryJobProfileFake.GetByUrlName(A<string>._)).Returns(dummyJobProfile);
            }

            if (hasDfcPageSiteNode)
            {
                A.CallTo(() => sitefinityCurrentContext.GetCurrentDfcPageNode()).Returns(dummyDfcPageSiteNode);
            }
            else
            {
                A.CallTo(() => sitefinityCurrentContext.GetCurrentDfcPageNode()).Returns(null);
            }

            //Instantiate & Act
            var dfcBreadcrumbController = new DfcBreadcrumbController(repositoryCategoryFake, repositoryJobProfileFake, sitefinityCurrentContext, loggerFake)
            {
                 HomepageLink = nameof(DfcBreadcrumbController.HomepageLink),
                HomepageText = nameof(DfcBreadcrumbController.HomepageText)
            };

            //Act
            var indexMethodCall = dfcBreadcrumbController.WithCallTo(c => c.Index(urlName));

            //Assert
            if (hasDfcPageSiteNode)
            {
                indexMethodCall
                       .ShouldRenderDefaultView()
                       .WithModel<DfcBreadcrumbViewModel>(vm =>
                       {
                           vm.BreadcrumbPageTitleText.Should().BeEquivalentTo(nodeTitle);
                       })
                       .AndNoModelErrors();
            }
            else
            {
                indexMethodCall
                  .ShouldRenderDefaultView()
                  .WithModel<DfcBreadcrumbViewModel>(vm =>
                  {
                      vm.BreadcrumbPageTitleText.Should().BeEquivalentTo("Breadcrumb cannot establish the page node");
                  })
                  .AndNoModelErrors();
            }
        }
    }
}