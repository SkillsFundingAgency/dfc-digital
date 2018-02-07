using ASP;
using DFC.Digital.Web.Sitefinity.JobProfileModule.Mvc.Models;
using FluentAssertions;
using HtmlAgilityPack;
using RazorGenerator.Testing;
using Xunit;

namespace DFC.Digital.Web.Sitefinity.JobProfileModule.UnitTests.Views
{
    public class BauJobProfileSignPostViewTests
    {
        [Theory]
        [InlineData("data")]
        [InlineData("test")]

        // Signposting on Beta JP page to point to specific BAU JP page
        public void DFC_2223_A1_BauJpSignPostIndex(string contentdata)
        {
            // Arrange
            var indexView = new _MVC_Views_JobProfileBauSignPosting_Index_cshtml();
            var signPostDummyVm = new BauJpSignPostViewModel
            {
                SignPostingHtml = contentdata
            };

            // Act
            var htmlDom = indexView.RenderAsHtml(signPostDummyVm);

            // Assert
            GetContentData(htmlDom).ShouldBeEquivalentTo(contentdata);
        }

        private string GetContentData(HtmlDocument htmlDom)
        {
            return htmlDom.DocumentNode?.InnerText.Trim();
        }
    }
}