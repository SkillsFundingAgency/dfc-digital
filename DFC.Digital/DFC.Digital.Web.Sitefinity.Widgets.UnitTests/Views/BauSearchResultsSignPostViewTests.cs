using ASP;
using DFC.Digital.Web.Sitefinity.Widgets.Mvc.Models;
using FluentAssertions;
using HtmlAgilityPack;
using RazorGenerator.Testing;
using System.Linq;
using Xunit;

namespace DFC.Digital.Web.Sitefinity.Widgets.UnitTests
{
    public class BauSearchResultsSignpostViewTests
    {
        [Theory]
        [InlineData("data")]
        [InlineData("test")]

        //As a Citizen having searched on the Public Beta site, I want to be signposted to BAU
        public void DFC1764A1BauSearchResultsSignpostIndex(string contentData)
        {
            // Arrange
            var indexView = new _MVC_Views_BauSearchResultsSignPost_Index_cshtml();
            var signPostDummyVm = new BauSearchResultsViewModel
            {
                Content = contentData
            };

            // Act
            var htmlDom = indexView.RenderAsHtml(signPostDummyVm);

            // Assert
            GetContentData(htmlDom).ShouldBeEquivalentTo(contentData);
        }

        private static string GetContentData(HtmlDocument htmlDom)
        {
            return htmlDom.DocumentNode?.InnerText;
        }
    }
}