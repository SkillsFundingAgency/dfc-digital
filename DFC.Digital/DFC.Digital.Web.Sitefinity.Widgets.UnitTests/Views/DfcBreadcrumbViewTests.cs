using ASP;
using DFC.Digital.Web.Sitefinity.Widgets.Mvc.Models;
using FluentAssertions;
using HtmlAgilityPack;
using RazorGenerator.Testing;
using System.Linq;
using Xunit;

namespace DFC.Digital.Web.Sitefinity.Widgets.UnitTests.Views
{
    /// <summary>
    /// DFC  Breadcrumb view tests
    /// </summary>
    public class DfcBreadcrumbViewTests
    {
        /// <summary>
        /// DFC the 1342 A1 DfcBreadcrumb
        /// </summary>
        /// <param name="homePageText">Home Page Text</param>
        /// <param name="homePageLink">Home Page Link</param>
        /// <param name="breadcrumbedPageTitleText">Breadcrumbed PageTitle Text</param>
        [Theory]
        [InlineData("Find a career home", "/", "Administration")]
        [InlineData("Find a career home", "/", "Border force officer")]
        [InlineData("Find a career home", "/", "Search results")]
        [InlineData("Find a career home", "/", "Error")]
        public void DFC_1342_A1_DfcBreadcrumb(string homePageText, string homePageLink, string breadcrumbedPageTitleText)
        {
            // Arrange
            var indexView = new _MVC_Views_DfcBreadcrumb_Index_cshtml();
            var dfcBreadcrumbViewModel = GenerateDfcBreadcrumbViewModelDummy(homePageText, homePageLink, breadcrumbedPageTitleText);

            //// Act
            var htmlDom = indexView.RenderAsHtml(dfcBreadcrumbViewModel);

            //// Assert
            GetHomePageTextOrLink(htmlDom, true).ShouldBeEquivalentTo(homePageText);
            GetHomePageTextOrLink(htmlDom, false).ShouldBeEquivalentTo(homePageLink);
            GetBreadcrumbedPageTitleText(htmlDom).ShouldBeEquivalentTo(breadcrumbedPageTitleText);
        }

        /// <summary>
        /// Gets the HomePage Text or Link.
        /// </summary>
        /// <param name="htmlDom">The HTML DOM.</param>
        /// <param name="isHomePageText">if set to <c>true</c> [is HomePage Text] else <c>false</c> [is HomePage Link].</param>
        /// <returns></returns>
        private static string GetHomePageTextOrLink(HtmlDocument htmlDom, bool isHomePageText)
        {
            var homePageElement = htmlDom.DocumentNode.Descendants("li").FirstOrDefault();
            if (homePageElement != null)
            {
                var liContent = homePageElement.Descendants("a").FirstOrDefault();
                if (liContent != null)
                {
                    if (isHomePageText)
                    {
                        return liContent.InnerText;
                    }
                    else
                    {
                        return liContent.GetAttributeValue("href", string.Empty);
                    }
                }
            }

            return string.Empty;
        }

        /// <summary>
        /// Gets the Breadcrumbed PageTitle Text.
        /// </summary>
        /// <param name="htmlDom">The HTML DOM.</param>
        /// <returns></returns>
        private static string GetBreadcrumbedPageTitleText(HtmlDocument htmlDom)
        {
            var breadcrumbedPageTitleTextElement = htmlDom.DocumentNode.Descendants("li").LastOrDefault();
            if (breadcrumbedPageTitleTextElement != null)
            {
                return breadcrumbedPageTitleTextElement.InnerText;
            }

            return string.Empty;
        }

        /// <summary>
        /// Generates the DfcBreadcrumb view model dummy.
        /// </summary>
        /// <param name="homePageText">Home Page Text</param>
        /// <param name="homePageLink">Home Page Link</param>
        /// <param name="breadcrumbedPageTitleText">Breadcrumbed PageTitle Text</param>
        /// <returns></returns>
        private DfcBreadcrumbViewModel GenerateDfcBreadcrumbViewModelDummy(string homePageText, string homePageLink, string breadcrumbedPageTitleText)
        {
            return new DfcBreadcrumbViewModel
            {
                HomePageText = homePageText,
                HomePageLink = homePageLink,
                BreadcrumbedPageTitleText = breadcrumbedPageTitleText
            };
        }
    }
}