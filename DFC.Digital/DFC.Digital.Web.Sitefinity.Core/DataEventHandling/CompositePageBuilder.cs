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
    public class CompositePageBuilder : ICompositePageBuilder
    {
        private readonly ISitefinityManagerProxy sitefinityManagerProxy;
        private readonly ISitefinityPageNodeProxy sitefinityPageNodeProxy;
        private readonly ISitefinityPageDataProxy sitefinityPageDataProxy;

        public CompositePageBuilder(ISitefinityManagerProxy sitefinityManagerProxy, ISitefinityPageDataProxy sitefinityPageDataProxy, ISitefinityPageNodeProxy sitefinityPageNodeProxy)
        {
            this.sitefinityManagerProxy = sitefinityManagerProxy;
            this.sitefinityPageDataProxy = sitefinityPageDataProxy;
            this.sitefinityPageNodeProxy = sitefinityPageNodeProxy;
        }

        public CompositePageData GetCompositePageForPageNode(string providerName, Type contentType, Guid itemId)
        {
            var pageNode = sitefinityManagerProxy.GetPageNode(providerName, contentType, itemId);
            var pageData = sitefinityManagerProxy.GetPageData(providerName, contentType, itemId);

            var compositePageData = new CompositePageData() { Name = sitefinityPageNodeProxy.GetURLName(pageNode) };
            compositePageData.IncludeInSitemap = pageNode.Crawlable;
            compositePageData.Title = sitefinityPageDataProxy.GetHtmlTitle(pageData);
            compositePageData.MetaTags = new MetaTags() { Description = sitefinityPageDataProxy.GetDescription(pageData), KeyWords = sitefinityPageDataProxy.GetKeywords(pageData) };
            compositePageData.Content = GetPageContentBlocks(providerName, contentType, itemId);
            compositePageData.URLs = GetPageURLs(pageNode);
            compositePageData.LastPublished = sitefinityPageNodeProxy.GetLastPublishedDate(pageNode);

            return compositePageData;
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
            foreach (var pageDataUrl in pageNode.Urls)
            {
                pageUrls.Add(pageDataUrl.Url);
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