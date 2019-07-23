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

        public MicroServicesPublishingPageBuilderTests()
        {
            fakeSitefinityManagerProxy = A.Fake<ISitefinityManagerProxy>();
            fakeSitefinityPageDataProxy = A.Fake<ISitefinityPageDataProxy>();
            fakeSitefinityPageNodeProxy = A.Fake<ISitefinityPageNodeProxy>();
        }

        [Theory]
        [InlineData(true, true)]
        [InlineData(false, false)]
        public void GetCompositePageForPageNodeTest(bool isCrawlable, bool hasContentBlock)
        {
            //SetUp
            var dummyPageNode = new PageNode();
            dummyPageNode.Crawlable = isCrawlable;
            dummyPageNode.Urls.Add(new PageUrlData() { Url = DummyUrl });
            var dummyPublishedDate = DateTime.Now;
            var dummyGuid = Guid.NewGuid();

            A.CallTo(() => fakeSitefinityManagerProxy.GetPageNode(A<Type>._, A<Guid>._, A<string>._)).Returns(dummyPageNode);
            A.CallTo(() => fakeSitefinityPageNodeProxy.GetPageName(A<PageNode>._)).Returns(nameof(PageNode.UrlName));
            A.CallTo(() => fakeSitefinityPageNodeProxy.GetLastPublishedDate(A<PageNode>._)).Returns(dummyPublishedDate);

            var dummyPageData = new PageData();
            A.CallTo(() => fakeSitefinityManagerProxy.GetPageData(A<Type>._, A<Guid>._, A<string>._)).Returns(dummyPageData);
            A.CallTo(() => fakeSitefinityPageDataProxy.GetDescription(A<PageData>._)).Returns(nameof(PageData.Description));
            A.CallTo(() => fakeSitefinityPageDataProxy.GetKeywords(A<PageData>._)).Returns(nameof(PageData.Keywords));
            A.CallTo(() => fakeSitefinityPageDataProxy.GetHtmlTitle(A<PageData>._)).Returns(nameof(PageData.HtmlTitle));
            A.CallTo(() => fakeSitefinityPageDataProxy.GetTitle(A<PageData>._)).Returns(nameof(PageData.NavigationNode.Title));

            if (hasContentBlock)
            {
                var pageControl = new PageControl() { Caption = Constants.ContentBlock };
                dummyPageData.Controls.Add(pageControl);
            }

            A.CallTo(() => fakeSitefinityManagerProxy.GetControlContent(A<PageControl>._, A<string>._)).Returns(DummyContent);

            //Act
            var microServicesPublishingPageBuilder = new MicroServicesPublishingPageBuilder(fakeSitefinityManagerProxy, fakeSitefinityPageDataProxy, fakeSitefinityPageNodeProxy);
            var microServicesPublishingPageData = microServicesPublishingPageBuilder.GetCompositePublishedPage(typeof(PageNode), dummyGuid, DummyProvider);

            //Asserts
            microServicesPublishingPageData.IncludeInSiteMap.Should().Be(isCrawlable);
            microServicesPublishingPageData.CanonicalName.Should().Be(nameof(PageNode.UrlName).ToLower());
            microServicesPublishingPageData.Id.Should().Be(dummyGuid);
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
    }
}
