﻿using DFC.Digital.Core;
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

        public static IList<string> GetPageURLs(PageNode pageNode)
        {
            var pageUrls = new List<string>();

            foreach (var pageDataUrl in pageNode?.Urls)
            {
                pageUrls?.Add(pageDataUrl.Url.Split('/').Last()?.ToLower());
            }

            return pageUrls;
        }

        public MicroServicesPublishingPageData GetCompositePublishedPage(Type contentType, Guid itemId, string providerName)
        {
            var pageNode = sitefinityManagerProxy.GetPageNode(contentType, itemId, providerName);
            var pageData = sitefinityManagerProxy.GetPageData(contentType, itemId, providerName);
            return BuildPageData(pageNode, pageData, providerName);
        }

        public MicroServicesPublishingPageData GetCompositePreviewPage(string name)
        {
            var pageData = sitefinityManagerProxy.GetPageDataByName(name);
            var pageNode = pageData.NavigationNode;
            var pageDraft = sitefinityManagerProxy.GetPreViewPageDataByNodeId(pageData.Id);

            return BuildPreViewPageData(pageNode, pageData, pageDraft);
        }

        public string GetPageContentBlocks(Type contentType, Guid itemId, string providerName)
        {
            string contentData = string.Empty;
            var pageData = sitefinityManagerProxy.GetPageData(contentType, itemId, providerName);

            //This bit came from Sitefinity Support.
            var pageDraftControls = pageData.Controls.Where(c => c.Caption == Digital.Core.Constants.ContentBlock);
            foreach (var pageDraftControl in pageDraftControls)
            {
                contentData += sitefinityManagerProxy.GetControlContent(pageDraftControl, providerName);
            }

            return contentData;
        }

        public string GetMicroServiceEndPointConfigKeyForPageNode(Type contentType, Guid itemId, string providerName)
        {
            var pageNode = sitefinityManagerProxy.GetPageNode(contentType, itemId, providerName);

            //If the custom field is not there this will throw a system exception
            //as we dont want to catch unnecessary exceptions this custom page field should be created.
            return sitefinityPageNodeProxy.GetCustomField(pageNode, Digital.Core.Constants.MicroServiceEndPointConfigKey)?.Trim();
        }

        private MicroServicesPublishingPageData BuildPreViewPageData(PageNode pageNode, PageData pageData, PageDraft pageDraft)
        {
            var microServicesPublishingPageData = BuildBasePageData(pageNode, pageData);

            var pageDraftControls = pageDraft.Controls.Where(c => c.Caption == Digital.Core.Constants.ContentBlock);

            foreach (var pageDraftControl in pageDraftControls)
            {
                microServicesPublishingPageData.Content += sitefinityManagerProxy.GetControlContent(pageDraftControl);
            }

            return microServicesPublishingPageData;
        }

        private MicroServicesPublishingPageData BuildPageData(PageNode pageNode, PageData pageData,  string providerName = null)
        {
            var microServicesPublishingPageData = BuildBasePageData(pageNode, pageData);

            var pageDraftControls = pageData.Controls.Where(c => c.Caption == Digital.Core.Constants.ContentBlock);
            foreach (var pageDraftControl in pageDraftControls)
            {
                microServicesPublishingPageData.Content += sitefinityManagerProxy.GetControlContent(pageDraftControl, providerName);
            }

            return microServicesPublishingPageData;
        }

        private MicroServicesPublishingPageData BuildBasePageData(PageNode pageNode, PageData pageData)
        {
            var microServicesPublishingPageData = new MicroServicesPublishingPageData() { CanonicalName = sitefinityPageNodeProxy.GetPageName(pageNode).ToLower() };
            microServicesPublishingPageData.IncludeInSiteMap = pageNode.Crawlable;
            microServicesPublishingPageData.AlternativeNames = GetPageURLs(pageNode);
            microServicesPublishingPageData.LastReviewed = sitefinityPageNodeProxy.GetLastPublishedDate(pageNode);
            microServicesPublishingPageData.Id = pageNode.Id;

            microServicesPublishingPageData.BreadcrumbTitle = sitefinityPageDataProxy.GetTitle(pageData);
            microServicesPublishingPageData.MetaTags = new MetaTags() { Description = sitefinityPageDataProxy.GetDescription(pageData), Keywords = sitefinityPageDataProxy.GetKeywords(pageData), Title = sitefinityPageDataProxy.GetHtmlTitle(pageData) };
            return microServicesPublishingPageData;
        }
    }
}