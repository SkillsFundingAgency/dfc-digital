using DFC.Digital.Core;
using DFC.Digital.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;

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
            return compositePageData;
        }

        public IList<string> GetPageContentBlocks(string providerName, Type contentType, Guid itemId)
        {
            var contentData = new List<string>();
            var pageData = sitefinityManagerProxy.GetPageData(providerName, contentType, itemId);

            //This bit came from Sitefinity Support.
            var pageDraftControls = pageData.Controls.Where(c => c.Caption == Constants.ContentBlock);
            foreach (var pageDraftControl in pageDraftControls)
            {
                contentData.Add(sitefinityManagerProxy.GetControlContent(providerName, pageDraftControl));
            }

            return contentData;
        }
    }
}