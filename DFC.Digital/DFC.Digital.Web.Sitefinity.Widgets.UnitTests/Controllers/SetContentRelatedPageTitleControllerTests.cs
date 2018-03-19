using DFC.Digital.Core;
using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using DFC.Digital.Web.Sitefinity.Widgets.Mvc.Controllers;
using FakeItEasy;
using FluentAssertions;
using System.Collections.Specialized;
using System.Web;
using System.Web.Mvc;
using Xunit;

namespace DFC.Digital.Web.Sitefinity.Widgets.UnitTests
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
        [InlineData(PageType.JobProfile, "Engineer", true)]
        [InlineData(PageType.Category, "Engineering and maintenance", true)]
        [InlineData(PageType.SearchResults, "<Proofreader>", true)]
        [InlineData(PageType.NotInterestedIn, "SouldRemainAsIs", true)]
        [InlineData(PageType.NotInterestedIn, "SouldRemainAsIs", false)]
        [InlineData(PageType.JobProfile, "Engineer", false)]
        [InlineData(PageType.Category, "Engineering and maintenance", false)]
        [InlineData(PageType.SearchResults, "<Proofreader>", false)]
        public void IndexTest(PageType pageType, string expectedPageTitle, bool isViaUrl)
        {
            //Setup the fakes and dummies
            var categoryRepoFake = A.Fake<IJobProfileCategoryRepository>(ops => ops.Strict());
            var jobProfileRepoFake = A.Fake<IJobProfileRepository>(ops => ops.Strict());
            var webAppContextFake = A.Fake<IWebAppContext>(ops => ops.Strict());
            var loggerFake = A.Fake<IApplicationLogger>();

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
                titleSet.Should().Be($"{expectedPageTitle} {setContentRelatedPageTitleController.PageTitleSeparator} {setContentRelatedPageTitleController.PageTitleSuffix}");
            }
            else if (!isViaUrl && pageType == PageType.SearchResults)
            {
                titleSet.Should().Be($"{HttpUtility.HtmlEncode(expectedPageTitle)} {setContentRelatedPageTitleController.PageTitleSeparator} Search {setContentRelatedPageTitleController.PageTitleSeparator} {setContentRelatedPageTitleController.PageTitleSuffix}");
            }
            else
            {
                titleSet.Should().Be(null);
            }
        }
    }
}