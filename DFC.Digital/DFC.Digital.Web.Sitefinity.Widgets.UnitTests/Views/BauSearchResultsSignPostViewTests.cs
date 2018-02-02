using ASP;
using DFC.Digital.Web.Sitefinity.Widgets.Mvc.Models;
using FluentAssertions;
using HtmlAgilityPack;
using RazorGenerator.Testing;
using Xunit;

namespace DFC.Digital.Web.Sitefinity.Widgets.UnitTests.Views
{
    public class BauSearchResultsSignPostViewTests
    {
        [Theory]
        [InlineData("data")]
        [InlineData("test")]

        //As a Citizen having searched on the Public Beta site, I want to be signposted to BAU
        public void DFC_1764_A1_BauSearchResultsSignPostIndex(string contentdata)
        {
            // Arrange
            var indexView = new _MVC_Views_BauSearchResultsSignPost_Index_cshtml();
            var signPostDummyVM = new BauSearchResultsViewModel
            {
                Content = contentdata
            };

            // Act
            var htmlDom = indexView.RenderAsHtml(signPostDummyVM);

            // Assert
            GetContentData(htmlDom).ShouldBeEquivalentTo(contentdata);
        }

        private string GetContentData(HtmlDocument htmlDom)
        {
            return htmlDom.DocumentNode?.InnerText;
        }
    }
}