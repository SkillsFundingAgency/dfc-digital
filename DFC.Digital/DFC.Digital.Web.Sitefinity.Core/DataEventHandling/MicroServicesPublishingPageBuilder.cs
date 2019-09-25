using DFC.Digital.Core;
using DFC.Digital.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using Telerik.Sitefinity;
using Telerik.Sitefinity.DynamicModules.Model;
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

        public MicroServicesPublishingPageData GetPublishedPage(Type contentType, Guid itemId, string providerName)
        {
            var pageNode = sitefinityManagerProxy.GetPageNode(contentType, itemId, providerName);
            var pageData = sitefinityManagerProxy.GetPageData(contentType, itemId, providerName);
            return BuildPageData(pageNode, pageData, providerName);
        }

        public MicroServicesPublishingPageData GetPublishedDynamicContent(Type contentType, Guid itemId, string providerName)
        {
            var pageNode = sitefinityManagerProxy.GetDynamicContentTypeNode(contentType, itemId, providerName);

            //var pageData = sitefinityManagerProxy.GetPageData(contentType, itemId, providerName);
            return BuildBaseDynamicContentData(pageNode);
        }

        public MicroServicesPublishingPageData GetPreviewPage(string name)
        {
            var pageData = sitefinityManagerProxy.GetPageDataByName(name);
            if (pageData is null)
            {
                return null;
            }

            var pageNode = pageData.NavigationNode;
            var pageDraft = sitefinityManagerProxy.GetPreviewPageDataById(pageData.Id);

            return BuildPreViewPageData(pageNode, pageData, pageDraft);
        }

        public string GetMicroServiceEndPointConfigKeyForPageNode(Type contentType, Guid itemId, string providerName)
        {
            var pageNode = sitefinityManagerProxy.GetPageNode(contentType, itemId, providerName);

            //If the custom field is not there this will throw a system exception
            //as we dont want to catch unnecessary exceptions this custom page field should be created.
            return sitefinityPageNodeProxy.GetCustomField(pageNode, Digital.Core.Constants.MicroServiceEndPointConfigKey)?.Trim();
        }

        public string GetMicroServiceEndPointConfigKeyForDynamicContentNode(Type contentType, Guid itemId, string providerName)
        {
            var dynamicContentNode = sitefinityManagerProxy.GetDynamicContentTypeNode(contentType, itemId, providerName);

            //If the custom field is not there this will throw a system exception
            //as we dont want to catch unnecessary exceptions this custom page field should be created.
            return sitefinityPageNodeProxy.GetCustomContentField(dynamicContentNode, Digital.Core.Constants.MicroServiceEndPointConfigKey)?.Trim();
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

        private MicroServicesPublishingPageData BuildPageData(PageNode pageNode, PageData pageData, string providerName = null)
        {
            var microServicesPublishingPageData = BuildBasePageData(pageNode, pageData);

            var pageControls = pageData.Controls.Where(c => c.Caption == Digital.Core.Constants.ContentBlock);
            foreach (var pageControl in pageControls)
            {
                microServicesPublishingPageData.Content += sitefinityManagerProxy.GetControlContent(pageControl, providerName);
            }

            return microServicesPublishingPageData;
        }

        private MicroServicesPublishingPageData BuildBasePageData(PageNode pageNode, PageData pageData)
        {
            return new MicroServicesPublishingPageData()
            {
                CanonicalName = sitefinityPageNodeProxy.GetPageName(pageNode).ToLower(),
                IncludeInSiteMap = pageNode.Crawlable,
                AlternativeNames = GetPageURLs(pageNode),
                LastReviewed = sitefinityPageNodeProxy.GetLastPublishedDate(pageNode),
                Id = pageNode.Id,
                BreadcrumbTitle = sitefinityPageDataProxy.GetTitle(pageData),
                MetaTags = new MetaTags() { Description = sitefinityPageDataProxy.GetDescription(pageData), Keywords = sitefinityPageDataProxy.GetKeywords(pageData), Title = sitefinityPageDataProxy.GetHtmlTitle(pageData) }
            };
        }

        private MicroServicesPublishingPageData BuildBaseDynamicContentData(DynamicContent contentNode)
        {
            return new MicroServicesPublishingPageData()
            {
                Id = contentNode.Id
            };
        }
    }
}