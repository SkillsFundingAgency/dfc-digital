using ASP;
using DFC.Digital.Data.Model;
using DFC.Digital.Web.Sitefinity.Widgets.Mvc.Models;
using FluentAssertions;
using HtmlAgilityPack;
using RazorGenerator.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace DFC.Digital.Web.Sitefinity.Widgets.UnitTests
{
    /// <summary>
    /// DFC  Breadcrumb view tests
    /// </summary>
    public class DfcBreadcrumbViewTests
    {
        private const string HrefNotFound = "HrefNotFound";

        [Fact]
        public void DFC1342A1DfcBreadcrumb()
        {
            // Arrange
            var indexView = new _MVC_Views_DfcBreadcrumb_Index_cshtml();
            var dfcBreadcrumbViewModel = GenerateDfcBreadcrumbViewModelDummy();

            //// Act
            var htmlDom = indexView.RenderAsHtml(dfcBreadcrumbViewModel);

            //// Assert

            //This should be the home page link
            var homeLinkInDom = GetLinkAtPosition(htmlDom, 0);
            homeLinkInDom.Link.Should().BeEquivalentTo(dfcBreadcrumbViewModel.HomepageLink);
            homeLinkInDom.Text.Should().BeEquivalentTo(dfcBreadcrumbViewModel.HomepageText);

            //This should be a standard link
            var firstLinkInDom = GetLinkAtPosition(htmlDom, 1);
            var firstLinkInModel = dfcBreadcrumbViewModel.BreadcrumbLinks.FirstOrDefault();
            firstLinkInDom.Should().BeEquivalentTo(firstLinkInModel);

            //This should be text and the href element missing
            var secondLinkInDom = GetLinkAtPosition(htmlDom, 2);
            var secondLinkInModel = dfcBreadcrumbViewModel.BreadcrumbLinks.Skip(1).FirstOrDefault();
            secondLinkInModel.Link = HrefNotFound;
            secondLinkInDom.Should().BeEquivalentTo(secondLinkInModel);
        }

        [Fact]
        public void Dfc8194ContactUsDfcBreadcrumb()
        {
            // Arrange
            var indexView = new _MVC_Views_DfcBreadcrumb_Index_cshtml();
            var dfcBreadcrumbViewModel = GenerateDfcBreadcrumbViewModelDummy();

            //// Act
            var htmlDom = indexView.RenderAsHtml(dfcBreadcrumbViewModel);

            //// Assert
            // new GDS system tag classes
            AssertTagClassExists(htmlDom, "div", "govuk-breadcrumbs");
            AssertTagClassExists(htmlDom, "ol", "govuk-breadcrumbs__list");
            AssertTagClassExists(htmlDom, "li", "govuk-breadcrumbs__list-item");
            AssertTagClassExists(htmlDom, "a", "govuk-breadcrumbs__link");
        }

        private static void AssertTagClassExists(HtmlDocument htmlDocument, string tag, string tagClass)
        {
            htmlDocument.DocumentNode.Descendants(tag)
                .Any(div => div.Attributes["class"].Value.Contains(tagClass)).Should().BeTrue();
        }

        /// <summary>
        /// Gets the HomePage Text or Link.
        /// </summary>
        /// <param name="htmlDom">The HTML DOM.</param>
        /// <param name="position">the number of li elements to skip before getting link</param>
        /// <returns>Homepage Text or Link</returns>
        private static BreadcrumbLink GetLinkAtPosition(HtmlDocument htmlDom, int position)
        {
            var breadCrumbLink = new BreadcrumbLink();
            var crumbElement = htmlDom.DocumentNode.Descendants("li").Skip(position).FirstOrDefault();
            if (crumbElement != null)
            {
                var anchorElement = crumbElement.Descendants("a").FirstOrDefault();
                if (anchorElement != null)
                {
                    breadCrumbLink.Text = anchorElement.InnerText;
                    breadCrumbLink.Link = anchorElement.GetAttributeValue("href", HrefNotFound);
                }
                else
                {
                    breadCrumbLink.Text = crumbElement.InnerText.Trim();
                    breadCrumbLink.Link = HrefNotFound;
                }
            }

            return breadCrumbLink;
        }

        /// <summary>
        /// Generates the DfcBreadcrumb view model dummy.
        /// </summary>
        /// <returns>Dummy DfcBreadcrumbViewModel</returns>
        private static DfcBreadcrumbViewModel GenerateDfcBreadcrumbViewModelDummy()
        {
            return new DfcBreadcrumbViewModel
            {
                HomepageText = "homepageText",
                HomepageLink = "homepageLink",

                BreadcrumbLinks = new List<BreadcrumbLink>()
                {
                  new BreadcrumbLink { Text = "Text1", Link = "Link1" },
                  new BreadcrumbLink { Text = "Text2" }
                }
            };
        }
    }
}