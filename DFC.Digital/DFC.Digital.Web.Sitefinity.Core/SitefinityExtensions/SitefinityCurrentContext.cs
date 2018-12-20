using DFC.Digital.Data.Model;
using System;
using System.Collections.Generic;
using Telerik.Sitefinity.Modules.Pages;
using Telerik.Sitefinity.Pages.Model;
using Telerik.Sitefinity.Web;

namespace DFC.Digital.Web.Sitefinity.Core
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class SitefinityCurrentContext : ISitefinityCurrentContext
    {
        private PageData currentPage;
        private PageSiteNode currentNode;
        private PageManager pageManager;

        public PageData CurrentPage
        {
            get { return currentPage = currentPage ?? CurrentPageManager.GetPageData(CurrentNode.PageId); }
            set { currentPage = value; }
        }

        public PageSiteNode CurrentNode
        {
            get { return currentNode = currentNode ?? SiteMapBase.GetCurrentNode(); }
            set { currentNode = value; }
        }

        public PageManager CurrentPageManager
        {
            get { return pageManager = pageManager ?? PageManager.GetManager(); }
            set { pageManager = value; }
        }

        public DfcPageSiteNode GetCurrentDfcPageNode()
        {
            return CurrentNode != null ? new DfcPageSiteNode { Title = CurrentNode.Title, Url = new Uri(CurrentNode.Url, UriKind.RelativeOrAbsolute) } : null;
        }

        public IList<BreadCrumbLink> BreadcrumbToParent()
        {
            var breadcrumbLinks = new List<BreadCrumbLink>();
            var pageNode = CurrentNode;
            while (pageNode.ParentNode != null)
            {
                if (pageNode.NodeType == NodeType.Standard && pageNode.Visible)
                {
                    var pageBreadCrumbLink = new BreadCrumbLink
                    {
                        Text = pageNode.Title,
                        Link = pageNode.Url.Replace("~/", string.Empty)
                    };
                    breadcrumbLinks.Add(pageBreadCrumbLink);
                }

                pageNode = pageNode.ParentNode as PageSiteNode;
            }

            return breadcrumbLinks;
        }
    }
}