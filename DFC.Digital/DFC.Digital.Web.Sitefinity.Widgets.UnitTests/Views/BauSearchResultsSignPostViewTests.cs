using ASP;
using DFC.Digital.Web.Sitefinity.Widgets.Mvc.Models;
using FluentAssertions;
using HtmlAgilityPack;
using RazorGenerator.Testing;
using System.Linq;
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
            var signPostDummyVm = new BauSearchResultsViewModel
            {
                Content = contentdata
            };

            // Act
            var htmlDom = indexView.RenderAsHtml(signPostDummyVm);

            // Assert
            GetContentData(htmlDom).ShouldBeEquivalentTo(contentdata);
        }

        private static string GetContentData(HtmlDocument htmlDom)
        {
            return htmlDom.DocumentNode?.InnerText;
        }
    }
}