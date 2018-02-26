using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using DFC.Digital.Web.Sitefinity.Widgets.Mvc.Controllers;
using FakeItEasy;
using FluentAssertions;
using System.Collections.Specialized;
using System.Web;
using System.Web.Mvc;
using Xunit;

namespace DFC.Digital.Web.Sitefinity.Widgets.UnitTests.Controllers
{
    public class SetContentRelatedPageTitleControllerTests
    {
        public enum PageType
        {
            JobProfile,
            Category,
            SearchResults,
            NotInterestedIn
        }

        [Theory]
        [InlineData(1, PageType.JobProfile, "Engineer", true)]
        [InlineData(2, PageType.Category, "Engineering and maintenance", true)]
        [InlineData(3, PageType.SearchResults, "<Proofreader>", true)]
        [InlineData(4, PageType.NotInterestedIn, "SouldRemainAsIs", true)]
        [InlineData(5, PageType.NotInterestedIn, "SouldRemainAsIs", false)]
        [InlineData(6, PageType.JobProfile, "Engineer", false)]
        [InlineData(7, PageType.Category, "Engineering and maintenance", false)]
        [InlineData(8, PageType.SearchResults, "<Proofreader>", false)]
        public void IndexTest(int testIndex, PageType pageType, string expectedPageTitle, bool isViaUrl)
        {
            //Setup the fakes and dummies
            var categoryRepoFake = A.Fake<IJobProfileCategoryRepository>(ops => ops.Strict());
            var jobProfileRepoFake = A.Fake<IJobProfileRepository>(ops => ops.Strict());
            var webAppContextFake = A.Fake<IWebAppContext>(ops => ops.Strict());
            var loggerFake = A.Fake<IApplicationLogger>(ops => ops.Strict());

            // Set up calls
            A.CallTo(() => webAppContextFake.IsCategoryPage).Returns(pageType == PageType.Category);
            A.CallTo(() => webAppContextFake.IsJobProfilePage).Returns(pageType == PageType.JobProfile);
            A.CallTo(() => webAppContextFake.IsSearchResultsPage).Returns(pageType == PageType.SearchResults);
            A.CallTo(() => webAppContextFake.IsContentAuthoringAndNotPreviewMode).Returns(false);
            A.CallTo(() => webAppContextFake.IsContentAuthoringSite).Returns(false);

            // Set up calls
            A.CallTo(() => jobProfileRepoFake.GetByUrlName(A<string>._)).Returns(new JobProfile { Title = expectedPageTitle });
            A.CallTo(() => categoryRepoFake.GetByUrlName(A<string>._)).Returns(new JobProfileCategory { Title = expectedPageTitle });

            if (pageType == PageType.SearchResults)
            {
                A.CallTo(() => webAppContextFake.RequestQueryString).Returns(new NameValueCollection { { "searchTerm", expectedPageTitle } });
            }

            //Instantiate & Act
            var setContentRelatedPageTitleController = new SetContentRelatedPageTitleController(categoryRepoFake, jobProfileRepoFake, webAppContextFake, loggerFake);

            ViewResult indexResult;

            //Act
            if (isViaUrl)
            {
                indexResult = setContentRelatedPageTitleController.Index("fakeURL") as ViewResult;
            }
            else
            {
                indexResult = setContentRelatedPageTitleController.Index() as ViewResult;
            }

            var titleSet = indexResult.ViewData["Title"];

            if (isViaUrl && (pageType == PageType.JobProfile || pageType == PageType.Category))
            {
                titleSet.Should().Be($"{expectedPageTitle} {setContentRelatedPageTitleController.PageTitleSeperator} {setContentRelatedPageTitleController.PageTitleSuffix}");
            }
            else if (!isViaUrl && pageType == PageType.SearchResults)
            {
                titleSet.Should().Be($"{HttpUtility.HtmlEncode(expectedPageTitle)} {setContentRelatedPageTitleController.PageTitleSeperator} Search {setContentRelatedPageTitleController.PageTitleSeperator} {setContentRelatedPageTitleController.PageTitleSuffix}");
            }
            else
            {
                titleSet.Should().Be(null);
            }
        }

        public void SearchTitleIsEncoded()
        {
        }
    }
}