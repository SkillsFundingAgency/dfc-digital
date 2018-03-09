using ASP;
using DFC.Digital.Web.Sitefinity.Widgets.Mvc.Models;
using FluentAssertions;
using HtmlAgilityPack;
using RazorGenerator.Testing;
using System.Linq;
using Xunit;

namespace DFC.Digital.Web.Sitefinity.Widgets.UnitTests
{
    /// <summary>
    /// DFC  Breadcrumb view tests
    /// </summary>
    public class DfcBreadcrumbViewTests
    {
        /// <summary>
        /// DFC the 1342 A1 DfcBreadcrumb
        /// </summary>
        /// <param name="homepageText">Home Page Text</param>
        /// <param name="homepageLink">Home Page Link</param>
        /// <param name="breadcrumbPageTitleText">Breadcrumbed PageTitle Text</param>
        [Theory]
        [InlineData("Find a career home", "/", "Administration")]
        [InlineData("Find a career home", "/", "Border force officer")]
        [InlineData("Find a career home", "/", "Search results")]
        [InlineData("Find a career home", "/", "Error")]
        public void DFC1342A1DfcBreadcrumb(string homepageText, string homepageLink, string breadcrumbPageTitleText)
        {
            // Arrange
            var indexView = new _MVC_Views_DfcBreadcrumb_Index_cshtml();
            var dfcBreadcrumbViewModel = GenerateDfcBreadcrumbViewModelDummy(homepageText, homepageLink, breadcrumbPageTitleText);

            //// Act
            var htmlDom = indexView.RenderAsHtml(dfcBreadcrumbViewModel);

            //// Assert
            GetHomepageTextOrLink(htmlDom, true).Should().BeEquivalentTo(homepageText);
            GetHomepageTextOrLink(htmlDom, false).Should().BeEquivalentTo(homepageLink);
            GetBreadcrumbPageTitleText(htmlDom).Should().BeEquivalentTo(breadcrumbPageTitleText);
        }

        /// <summary>
        /// Gets the HomePage Text or Link.
        /// </summary>
        /// <param name="htmlDom">The HTML DOM.</param>
        /// <param name="isHomepageText">if set to <c>true</c> [is HomePage Text] else <c>false</c> [is HomePage Link].</param>
        /// <returns></returns>
        private static string GetHomepageTextOrLink(HtmlDocument htmlDom, bool isHomepageText)
        {
            var homePageElement = htmlDom.DocumentNode.Descendants("li").FirstOrDefault();
            if (homePageElement != null)
            {
                var liContent = homePageElement.Descendants("a").FirstOrDefault();
                if (liContent != null)
                {
                    if (isHomepageText)
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
        private static string GetBreadcrumbPageTitleText(HtmlDocument htmlDom)
        {
            var breadcrumbPageTitleTextElement = htmlDom.DocumentNode.Descendants("li").LastOrDefault();
            if (breadcrumbPageTitleTextElement != null)
            {
                return breadcrumbPageTitleTextElement.InnerText;
            }

            return string.Empty;
        }

        /// <summary>
        /// Generates the DfcBreadcrumb view model dummy.
        /// </summary>
        /// <param name="homepageText">Home Page Text</param>
        /// <param name="homepageLink">Home Page Link</param>
        /// <param name="breadcrumbedpageTitleText">Breadcrumb PageTitle Text</param>
        /// <returns></returns>
        private static DfcBreadcrumbViewModel GenerateDfcBreadcrumbViewModelDummy(string homepageText, string homepageLink, string breadcrumbedpageTitleText)
        {
            return new DfcBreadcrumbViewModel
            {
                HomepageText = homepageText,
                HomepageLink = homepageLink,
                BreadcrumbPageTitleText = breadcrumbedpageTitleText
            };
        }
    }
}