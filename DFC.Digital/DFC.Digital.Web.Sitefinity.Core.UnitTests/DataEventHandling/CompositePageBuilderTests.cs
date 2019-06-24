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
    public class CompositePageBuilderTests
    {
        private const string DummyUrl = "/DummyUrl";
        private const string DummyContent = "DummyContent";

        private readonly ISitefinityManagerProxy fakeSitefinityManagerProxy;
        private readonly ISitefinityPageNodeProxy fakeSitefinityPageNodeProxy;
        private readonly ISitefinityPageDataProxy fakeSitefinityPageDataProxy;

        public CompositePageBuilderTests()
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
            A.CallTo(() => fakeSitefinityManagerProxy.GetPageNode(A<string>._, A<Type>._, A<Guid>._)).Returns(dummyPageNode);
            A.CallTo(() => fakeSitefinityPageNodeProxy.GetURLName(A<PageNode>._)).Returns(nameof(PageNode.UrlName));
            A.CallTo(() => fakeSitefinityPageNodeProxy.GetLastPublishedDate(A<PageNode>._)).Returns(dummyPublishedDate);

            var dummyPageData = new PageData();
            A.CallTo(() => fakeSitefinityManagerProxy.GetPageData(A<string>._, A<Type>._, A<Guid>._)).Returns(dummyPageData);
            A.CallTo(() => fakeSitefinityPageDataProxy.GetDescription(A<PageData>._)).Returns(nameof(PageData.Description));
            A.CallTo(() => fakeSitefinityPageDataProxy.GetKeywords(A<PageData>._)).Returns(nameof(PageData.Keywords));
            A.CallTo(() => fakeSitefinityPageDataProxy.GetHtmlTitle(A<PageData>._)).Returns(nameof(PageData.HtmlTitle));

            if (hasContentBlock)
            {
                var pageControl = new PageControl() { Caption = Constants.ContentBlock };
                dummyPageData.Controls.Add(pageControl);
            }

            var dummyMvcControllerProxy = new MvcControllerProxy();
            A.CallTo(() => fakeSitefinityManagerProxy.GetControlContent(A<string>._, A<PageControl>._)).Returns(DummyContent);

            //Act
            var compositePageBuilder = new CompositePageBuilder(fakeSitefinityManagerProxy, fakeSitefinityPageDataProxy, fakeSitefinityPageNodeProxy);
            var compositePage = compositePageBuilder.GetCompositePageForPageNode("dummyProvider", typeof(PageNode), A.Dummy<Guid>());

            //Asserts
            compositePage.IncludeInSitemap.Should().Be(isCrawlable);
            compositePage.Name.Should().Be(nameof(PageNode.UrlName));
            compositePage.Title.Should().Be(nameof(PageData.HtmlTitle));
            compositePage.MetaTags.Description.Should().Be(nameof(PageData.Description));
            compositePage.MetaTags.KeyWords.Should().Be(nameof(PageData.Keywords));
            compositePage.LastPublished.Should().Be(dummyPublishedDate);
            compositePage.URLs.ToList().FirstOrDefault().Should().Be(DummyUrl);

            if (hasContentBlock)
            {
                compositePage.Content.ToList().FirstOrDefault().Should().Be(DummyContent);
            }
            else
            {
                compositePage.Content.Should().BeNullOrEmpty();
            }
        }
    }
}
