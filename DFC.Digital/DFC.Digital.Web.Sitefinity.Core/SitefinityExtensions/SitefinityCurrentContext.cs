using System;
using DFC.Digital.Data.Model;
using Telerik.Sitefinity.Modules.Pages;
using Telerik.Sitefinity.Pages.Model;
using Telerik.Sitefinity.Web;

namespace DFC.Digital.Web.Sitefinity.Core
{
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
            return CurrentNode != null ? new DfcPageSiteNode { Title = CurrentNode.Title, Url = new Uri(CurrentNode.Url) } : null;
        }
    }
}