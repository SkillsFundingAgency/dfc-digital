using DFC.Digital.Core;
using FakeItEasy;
using FluentAssertions;
using System;
using System.Linq;
using Telerik.Sitefinity.Mvc.Proxy;
using Telerik.Sitefinity.Pages.Model;
using Xunit;

namespace DFC.Digital.Web.Sitefinity.Core.UnitTests
{
    public class MicroServicesPublishingPageBuilderTests
    {
        private const string DummyUrl = "~/DummyUrl";
        private const string DummyContent = "DummyContent";
        private const string DummyProvider = "DummyProvider";

        private readonly ISitefinityManagerProxy fakeSitefinityManagerProxy;
        private readonly ISitefinityPageNodeProxy fakeSitefinityPageNodeProxy;
        private readonly ISitefinityPageDataProxy fakeSitefinityPageDataProxy;

        private readonly PageNode dummyPageNode;
        private readonly PageData dummyPageData;
        private readonly DateTime dummyPublishedDate;

        public MicroServicesPublishingPageBuilderTests()
        {
            fakeSitefinityManagerProxy = A.Fake<ISitefinityManagerProxy>();
            fakeSitefinityPageDataProxy = A.Fake<ISitefinityPageDataProxy>();
            fakeSitefinityPageNodeProxy = A.Fake<ISitefinityPageNodeProxy>();

            dummyPublishedDate = DateTime.Now;
            dummyPageNode = new PageNode();
            dummyPageData = new PageData();
        }

        [Theory]
        [InlineData(true, true)]
        [InlineData(false, false)]
        public void GetCompositePublishedPageTest(bool isCrawlable, bool hasContentBlock)
        {
            SetUpData(isCrawlable);

            if (hasContentBlock)
            {
                var pageControl = new PageControl() { Caption = Constants.ContentBlock };
                dummyPageData.Controls.Add(pageControl);
            }

            A.CallTo(() => fakeSitefinityManagerProxy.GetControlContent(A<PageControl>._, A<string>._)).Returns(DummyContent);
            A.CallTo(() => fakeSitefinityManagerProxy.GetPageData(A<Type>._, A<Guid>._, A<string>._)).Returns(dummyPageData);

            //Act
            var microServicesPublishingPageBuilder = new MicroServicesPublishingPageBuilder(fakeSitefinityManagerProxy, fakeSitefinityPageDataProxy, fakeSitefinityPageNodeProxy);
            var microServicesPublishingPageData = microServicesPublishingPageBuilder.GetCompositePublishedPage(typeof(PageNode), dummyPageNode.Id, DummyProvider);

            //Asserts
            microServicesPublishingPageData.IncludeInSiteMap.Should().Be(isCrawlable);
            microServicesPublishingPageData.CanonicalName.Should().Be(nameof(PageNode.UrlName).ToLower());
            microServicesPublishingPageData.Id.Should().Be(dummyPageNode.Id);
            microServicesPublishingPageData.BreadcrumbTitle.Should().Be(nameof(PageData.NavigationNode.Title));
            microServicesPublishingPageData.MetaTags.Description.Should().Be(nameof(PageData.Description));
            microServicesPublishingPageData.MetaTags.Keywords.Should().Be(nameof(PageData.Keywords));
            microServicesPublishingPageData.MetaTags.Title.Should().Be(nameof(PageData.HtmlTitle));
            microServicesPublishingPageData.LastReviewed.Should().Be(dummyPublishedDate);
            microServicesPublishingPageData.AlternativeNames.ToList().FirstOrDefault().Should().Be(DummyUrl.Split('/').Last().ToLower());

            if (hasContentBlock)
            {
                microServicesPublishingPageData.Content.Should().Be(DummyContent);
            }
            else
            {
                microServicesPublishingPageData.Content.Should().BeNullOrEmpty();
            }
        }

        [Theory]
        [InlineData(true, true)]
        [InlineData(false, false)]
        public void GetCompositePreviewPageTest(bool isCrawlable, bool hasContentBlock)
        {
            SetUpData(isCrawlable);

            var dummyPageDraft = new PageDraft();

            if (hasContentBlock)
            {
                var pageDraftControl = new PageDraftControl() { Caption = Constants.ContentBlock };
                dummyPageDraft.Controls.Add(pageDraftControl);
            }

            A.CallTo(() => fakeSitefinityManagerProxy.GetPreViewPageDataByNodeId(A<Guid>._)).Returns(dummyPageDraft);
            A.CallTo(() => fakeSitefinityManagerProxy.GetControlContent(A<PageDraftControl>._)).Returns(DummyContent);
            A.CallTo(() => fakeSitefinityManagerProxy.GetPageDataByName(A<string>._)).Returns(dummyPageData);

            //Act
            var microServicesPublishingPageBuilder = new MicroServicesPublishingPageBuilder(fakeSitefinityManagerProxy, fakeSitefinityPageDataProxy, fakeSitefinityPageNodeProxy);
            var microServicesPublishingPageData = microServicesPublishingPageBuilder.GetCompositePreviewPage(nameof(PageNode.UrlName));

            //Asserts
            microServicesPublishingPageData.IncludeInSiteMap.Should().Be(isCrawlable);
            microServicesPublishingPageData.CanonicalName.Should().Be(nameof(PageNode.UrlName).ToLower());
            microServicesPublishingPageData.Id.Should().Be(dummyPageNode.Id);
            microServicesPublishingPageData.BreadcrumbTitle.Should().Be(nameof(PageData.NavigationNode.Title));
            microServicesPublishingPageData.MetaTags.Description.Should().Be(nameof(PageData.Description));
            microServicesPublishingPageData.MetaTags.Keywords.Should().Be(nameof(PageData.Keywords));
            microServicesPublishingPageData.MetaTags.Title.Should().Be(nameof(PageData.HtmlTitle));
            microServicesPublishingPageData.LastReviewed.Should().Be(dummyPublishedDate);
            microServicesPublishingPageData.AlternativeNames.ToList().FirstOrDefault().Should().Be(DummyUrl.Split('/').Last().ToLower());

            if (hasContentBlock)
            {
                microServicesPublishingPageData.Content.Should().Be(DummyContent);
            }
            else
            {
                microServicesPublishingPageData.Content.Should().BeNullOrEmpty();
            }
        }

        [Fact]
        public void GetCompositePreviewPageDoesNotExsitTest()
        {
            SetUpData(false);
            A.CallTo(() => fakeSitefinityManagerProxy.GetPageDataByName(A<string>._)).Returns(null);

            //Act
            var microServicesPublishingPageBuilder = new MicroServicesPublishingPageBuilder(fakeSitefinityManagerProxy, fakeSitefinityPageDataProxy, fakeSitefinityPageNodeProxy);
            var microServicesPublishingPageData = microServicesPublishingPageBuilder.GetCompositePreviewPage(nameof(PageNode.UrlName));

            //Asserts
            microServicesPublishingPageData.Should().BeNull();
         }

        [Fact]
        public void GetMicroServiceEndPointConfigKeyForPageNodeTest()
        {
            //Setup
            var dummyPageNode = new PageNode();
            var dummyKeyName = "dummyKeyName   ";
            var dummyGuid = Guid.NewGuid();

            A.CallTo(() => fakeSitefinityManagerProxy.GetPageNode(A<Type>._, A<Guid>._, A<string>._)).Returns(dummyPageNode);
            A.CallTo(() => fakeSitefinityPageNodeProxy.GetCustomField(A<PageNode>._, A<string>._)).Returns(dummyKeyName);

            //Act
            var microServicesPublishingPageBuilder = new MicroServicesPublishingPageBuilder(fakeSitefinityManagerProxy, fakeSitefinityPageDataProxy, fakeSitefinityPageNodeProxy);
            var configKeyName = microServicesPublishingPageBuilder.GetMicroServiceEndPointConfigKeyForPageNode(typeof(PageNode), dummyGuid, DummyProvider);

            //Asserts
            configKeyName.Should().Be(dummyKeyName.Trim());
        }

        private void SetUpData(bool isCrawlable)
        {
            //SetUp
            var dummyGuid = Guid.NewGuid();
            dummyPageNode.Id = dummyGuid;

            dummyPageNode.Crawlable = isCrawlable;
            dummyPageNode.Urls.Add(new PageUrlData() { Url = DummyUrl });
            dummyPageData.NavigationNode = dummyPageNode;

            A.CallTo(() => fakeSitefinityManagerProxy.GetPageNode(A<Type>._, A<Guid>._, A<string>._)).Returns(dummyPageNode);
            A.CallTo(() => fakeSitefinityPageNodeProxy.GetPageName(A<PageNode>._)).Returns(nameof(PageNode.UrlName));
            A.CallTo(() => fakeSitefinityPageNodeProxy.GetLastPublishedDate(A<PageNode>._)).Returns(dummyPublishedDate);

            A.CallTo(() => fakeSitefinityManagerProxy.GetPageData(A<Type>._, A<Guid>._, A<string>._)).Returns(dummyPageData);
            A.CallTo(() => fakeSitefinityManagerProxy.GetPageDataByName(A<string>._)).Returns(dummyPageData);
            A.CallTo(() => fakeSitefinityPageDataProxy.GetDescription(A<PageData>._)).Returns(nameof(PageData.Description));
            A.CallTo(() => fakeSitefinityPageDataProxy.GetKeywords(A<PageData>._)).Returns(nameof(PageData.Keywords));
            A.CallTo(() => fakeSitefinityPageDataProxy.GetHtmlTitle(A<PageData>._)).Returns(nameof(PageData.HtmlTitle));
            A.CallTo(() => fakeSitefinityPageDataProxy.GetTitle(A<PageData>._)).Returns(nameof(PageData.NavigationNode.Title));
        }
    }
}
