using DFC.Digital.Core;
using DFC.Digital.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using Telerik.Sitefinity.Mvc.Proxy;

namespace DFC.Digital.Web.Sitefinity.Core
{
    public class CompositePageBuilder : ICompositePageBuilder
    {
        private readonly ISitefinityManagerProxy sitefinityManagerProxy;

        public CompositePageBuilder(ISitefinityManagerProxy sitefinityManagerProxy)
        {
            this.sitefinityManagerProxy = sitefinityManagerProxy;
        }

        public CompositePageData GetCompositePageForPageNode(string providerName, Type contentType, Guid itemId)
        {
            var pageNode = sitefinityManagerProxy.GetPageNode(providerName, contentType, itemId);
            var pageData = sitefinityManagerProxy.GetPageData(providerName, contentType, itemId);

            var compositePageData = new CompositePageData() { Name = pageNode.UrlName.Value };
            compositePageData.IncludeInSitemap = pageNode.Crawlable;
            compositePageData.Title = pageData.HtmlTitle.Value;
            compositePageData.MetaTags = new MetaTags() { Description = pageData.Description.Value, KeyWords = pageData.Keywords.Value };
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
                MvcControllerProxy control = sitefinityManagerProxy.LoadControl(providerName, pageDraftControl);
                contentData.Add(control.Settings.Values[Constants.Content]);
            }

            return contentData;
        }
    }
}