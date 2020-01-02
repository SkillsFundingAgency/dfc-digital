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
        private const string DummyParentTitle = "DummyCanonicalName";

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

        [Fact]
        public void GetMicroServiceEndPointConfigKeyForPageNodeTest()
        {
            //Setup
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
