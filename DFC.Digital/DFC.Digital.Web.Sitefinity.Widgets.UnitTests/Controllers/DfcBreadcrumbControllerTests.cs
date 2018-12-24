using DFC.Digital.Core;
using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using DFC.Digital.Web.Sitefinity.Core;
using DFC.Digital.Web.Sitefinity.Widgets.Mvc.Controllers;
using DFC.Digital.Web.Sitefinity.Widgets.Mvc.Models;
using FakeItEasy;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public enum SegmentType
        {
            JobProfile,
            Category,
            Alert
        }

        [Theory]
        [InlineData(SegmentType.Category, "Category Title", "Category Title")]
        [InlineData(SegmentType.Category, null, "Category Page Title")]
        [InlineData(SegmentType.JobProfile, "JobProfile Title", "JobProfile Title")]
        [InlineData(SegmentType.JobProfile, null, "JobProfile Page Title")]
        [InlineData(SegmentType.Alert, null, "Title from repo")]
        public void IndexCustomPagesTest(SegmentType segmentType, string repoTitle, string expectedLinkTitle)
        {
            //Setup
            var dummyDfcPageSiteNode = A.Dummy<DfcPageSiteNode>();
            dummyDfcPageSiteNode.Title = expectedLinkTitle;
            A.CallTo(() => sitefinityCurrentContext.GetCurrentDfcPageNode()).Returns(dummyDfcPageSiteNode);

            //Instantiate
            var dfcBreadcrumbController = new DfcBreadcrumbController(repositoryCategoryFake, repositoryJobProfileFake, sitefinityCurrentContext, loggerFake);

            switch (segmentType)
            {
                case SegmentType.Category:
                        dummyDfcPageSiteNode.Url = new Uri($"/abcd/{dfcBreadcrumbController.JobCategoriesPathSegment}", UriKind.RelativeOrAbsolute);
                        A.CallTo(() => repositoryCategoryFake.GetByUrlName(A<string>._)).Returns(repoTitle == null ? null : new JobProfileCategory() { Title = repoTitle });
                break;
                case SegmentType.JobProfile:
                    dummyDfcPageSiteNode.Url = new Uri($"/abcd/{dfcBreadcrumbController.JobProfilesPathSegment}", UriKind.RelativeOrAbsolute);
                    A.CallTo(() => repositoryJobProfileFake.GetByUrlName(A<string>._)).Returns(repoTitle == null ? null : new JobProfile() { Title = repoTitle });
                break;
                case SegmentType.Alert:
                    dummyDfcPageSiteNode.Url = new Uri($"/abcd/{dfcBreadcrumbController.AlertsPathSegment}", UriKind.RelativeOrAbsolute);
                    dfcBreadcrumbController.AlertsPageText = expectedLinkTitle;
                break;
            }

            // Act
            var indexMethodCall = dfcBreadcrumbController.WithCallTo(c => c.Index("dummyURLName"));

            //Assert
            indexMethodCall
                .ShouldRenderDefaultView()
                .WithModel<DfcBreadcrumbViewModel>(vm =>
                {
                    vm.HomepageText.Should().BeEquivalentTo(dfcBreadcrumbController.HomepageText);
                    vm.HomepageLink.Should().BeEquivalentTo(dfcBreadcrumbController.HomepageLink);
                    vm.BreadcrumbLinks.FirstOrDefault().Text.Should().BeEquivalentTo(expectedLinkTitle);
                    vm.BreadcrumbLinks.FirstOrDefault().Link.Should().BeEquivalentTo(null);
                })
                .AndNoModelErrors();
        }

        [Fact]
        public void IndexPageHierarchyTest()
        {
            //Setup
            var dummyDfcPageSiteNode = A.Dummy<DfcPageSiteNode>();

            dummyDfcPageSiteNode.Url = new Uri($"/a/b/c", UriKind.RelativeOrAbsolute);

            A.CallTo(() => sitefinityCurrentContext.GetCurrentDfcPageNode()).Returns(dummyDfcPageSiteNode);

            var breadcrumbToParent = new List<BreadcrumbLink>()
            {
                  new BreadcrumbLink { Text = "Text2", Link = "Link2" },
                  new BreadcrumbLink { Text = "Text1", Link = "Link1" }
            };

            A.CallTo(() => sitefinityCurrentContext.BreadcrumbToParent()).Returns(breadcrumbToParent);

            //Instantiate
            var dfcBreadcrumbController = new DfcBreadcrumbController(repositoryCategoryFake, repositoryJobProfileFake, sitefinityCurrentContext, loggerFake);

            //Act
            var indexMethodCall = dfcBreadcrumbController.WithCallTo(c => c.Index("dummyURLName"));

            //Assert
            indexMethodCall
                .ShouldRenderDefaultView()
                .WithModel<DfcBreadcrumbViewModel>(vm =>
                {
                    vm.HomepageText.Should().BeEquivalentTo(dfcBreadcrumbController.HomepageText);
                    vm.HomepageLink.Should().BeEquivalentTo(dfcBreadcrumbController.HomepageLink);

                    //Links should have been reversed
                    vm.BreadcrumbLinks.FirstOrDefault().Text.Should().BeEquivalentTo("Text1");
                    vm.BreadcrumbLinks.FirstOrDefault().Link.Should().BeEquivalentTo("Link1");

                    //Last one should not have a link
                    vm.BreadcrumbLinks.Skip(1).FirstOrDefault().Text.Should().BeEquivalentTo("Text2");
                    vm.BreadcrumbLinks.Skip(1).FirstOrDefault().Link.Should().BeEquivalentTo(null);
                })
                .AndNoModelErrors();
        }

        [Fact]
        public void IndexPageNullTest()
        {
            //Setup
            A.CallTo(() => sitefinityCurrentContext.GetCurrentDfcPageNode()).Returns(null);

            //Instantiate
            var dfcBreadcrumbController = new DfcBreadcrumbController(repositoryCategoryFake, repositoryJobProfileFake, sitefinityCurrentContext, loggerFake);

            // Act
            var indexMethodCall = dfcBreadcrumbController.WithCallTo(c => c.Index("dummyURLName"));

            //Assert
            indexMethodCall
                .ShouldRenderDefaultView()
                .WithModel<DfcBreadcrumbViewModel>(vm =>
                {
                    vm.HomepageText.Should().BeEquivalentTo(dfcBreadcrumbController.HomepageText);
                    vm.HomepageLink.Should().BeEquivalentTo(dfcBreadcrumbController.HomepageLink);
                    vm.BreadcrumbLinks.FirstOrDefault().Text.Should().BeEquivalentTo(null);
                    vm.BreadcrumbLinks.FirstOrDefault().Link.Should().BeEquivalentTo(null);
                })
                .AndNoModelErrors();
        }
    }
}