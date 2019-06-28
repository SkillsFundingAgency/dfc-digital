using DFC.Digital.Core;
using DFC.Digital.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using Telerik.Sitefinity;
using Telerik.Sitefinity.Model;
using Telerik.Sitefinity.Pages.Model;

namespace DFC.Digital.Web.Sitefinity.Core
{
    public class MicroServicesPublishingPageBuilder : ICompositePageBuilder
    {
        private readonly ISitefinityManagerProxy sitefinityManagerProxy;
        private readonly ISitefinityPageNodeProxy sitefinityPageNodeProxy;
        private readonly ISitefinityPageDataProxy sitefinityPageDataProxy;

        public MicroServicesPublishingPageBuilder(ISitefinityManagerProxy sitefinityManagerProxy, ISitefinityPageDataProxy sitefinityPageDataProxy, ISitefinityPageNodeProxy sitefinityPageNodeProxy)
        {
            this.sitefinityManagerProxy = sitefinityManagerProxy;
            this.sitefinityPageDataProxy = sitefinityPageDataProxy;
            this.sitefinityPageNodeProxy = sitefinityPageNodeProxy;
        }

        public MicroServicesPublishingPageData GetCompositePageForPageNode(string providerName, Type contentType, Guid itemId)
        {
            var pageNode = sitefinityManagerProxy.GetPageNode(providerName, contentType, itemId);
            var pageData = sitefinityManagerProxy.GetPageData(providerName, contentType, itemId);

            var microServicesPublishingPageData = new MicroServicesPublishingPageData() { Name = sitefinityPageNodeProxy.GetPageName(pageNode), Id = itemId };
            microServicesPublishingPageData.IncludeInSitemap = pageNode.Crawlable;
            microServicesPublishingPageData.PageTitle = sitefinityPageDataProxy.GetTitle(pageData);
            microServicesPublishingPageData.MetaTags = new MetaTags() { Description = sitefinityPageDataProxy.GetDescription(pageData), KeyWords = sitefinityPageDataProxy.GetKeywords(pageData), Title = sitefinityPageDataProxy.GetHtmlTitle(pageData) };
            microServicesPublishingPageData.Content = GetPageContentBlocks(providerName, contentType, itemId);
            microServicesPublishingPageData.URLs = GetPageURLs(pageNode);
            microServicesPublishingPageData.LastPublished = sitefinityPageNodeProxy.GetLastPublishedDate(pageNode);

            return microServicesPublishingPageData;
        }

        public IList<string> GetPageContentBlocks(string providerName, Type contentType, Guid itemId)
        {
            var contentData = new List<string>();
            var pageData = sitefinityManagerProxy.GetPageData(providerName, contentType, itemId);

            //This bit came from Sitefinity Support.
            var pageDraftControls = pageData.Controls.Where(c => c.Caption == Digital.Core.Constants.ContentBlock);
            foreach (var pageDraftControl in pageDraftControls)
            {
                contentData.Add(sitefinityManagerProxy.GetControlContent(providerName, pageDraftControl));
            }

            return contentData;
        }

        public IList<string> GetPageURLs(PageNode pageNode)
        {
            var pageUrls = new List<string>();
            foreach (var pageDataUrl in pageNode?.Urls)
            {
                pageUrls?.Add(pageDataUrl.Url);
            }

            return pageUrls;
        }

        public string GetMicroServiceEndPointConfigKeyForPageNode(string providerName, Type contentType, Guid itemId)
        {
            var pageNode = sitefinityManagerProxy.GetPageNode(providerName, contentType, itemId);

            //If the custom field is not there this will throw a system exception
            //as we dont want to catch unnecessary exceptions this custom page field should be created.
            return sitefinityPageNodeProxy.GetCustomField(pageNode, Digital.Core.Constants.MicroServiceEndPointConfigKey)?.Trim();
        }
    }
}