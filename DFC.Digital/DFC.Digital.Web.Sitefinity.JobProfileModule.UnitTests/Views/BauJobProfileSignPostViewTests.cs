using ASP;
using DFC.Digital.Web.Sitefinity.JobProfileModule.Mvc.Models;
using FluentAssertions;
using HtmlAgilityPack;
using RazorGenerator.Testing;
using Xunit;

namespace DFC.Digital.Web.Sitefinity.JobProfileModule.UnitTests
{
    public class BauJobProfileSignpostViewTests
    {
        [Theory]
        [InlineData("data")]
        [InlineData("test")]

        // Signposting on Beta JP page to point to specific BAU JP page
        public void DFC2223ScenarioA1ForBAUJobProfileSignpostIndex(string contentdata)
        {
            // Arrange
            var indexView = new _MVC_Views_JobProfileBAUSignposting_Index_cshtml();
            var signPostDummyVm = new BAUJobProfileSignpostViewModel
            {
                SignpostingHtml = contentdata
            };

            // Act
            var htmlDom = indexView.RenderAsHtml(signPostDummyVm);

            // Assert
            GetContentData(htmlDom).Should().BeEquivalentTo(contentdata);
        }

        private string GetContentData(HtmlDocument htmlDom)
        {
            return htmlDom.DocumentNode?.InnerText.Trim();
        }
    }
}