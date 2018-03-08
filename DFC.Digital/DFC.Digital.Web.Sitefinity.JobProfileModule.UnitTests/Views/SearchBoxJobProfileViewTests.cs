using ASP;
using DFC.Digital.Web.Sitefinity.JobProfileModule.Mvc.Controllers;
using DFC.Digital.Web.Sitefinity.JobProfileModule.Mvc.Models;
using FluentAssertions;
using HtmlAgilityPack;
using RazorGenerator.Testing;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Xunit;

namespace DFC.Digital.Web.Sitefinity.JobProfileModule.View.Tests
{
    public class SearchBoxJobProfileViewTests
    {
        #region Tests

        //As a Citizen, I want to be able to search from JP
        [Fact]
        public void DFC_1330_SearchBoxonJobProfilePage()
        {
            // Arrange
            var indexView = new _MVC_Views_JobProfileSearchBox_JobProfile_cshtml();
            var headerText = nameof(JobProfileSearchBoxController.HeaderText);
            var jobProfileUrl = nameof(JobProfileSearchBoxViewModel.JobProfileUrl);
            var placeholderText = nameof(JobProfileSearchBoxController.PlaceholderText);

            var model = new JobProfileSearchBoxViewModel
            {
                HeaderText = headerText,
                JobProfileUrl = jobProfileUrl,
                PlaceHolderText = placeholderText
            };

            // Act
            var htmlDom = indexView.RenderAsHtml(model);

            // Asserts
            GetHeaderText(htmlDom).Should().Be(headerText);
            GetJobProfileUrlText(htmlDom).Should().Be(jobProfileUrl);
            GetPlaceholderText(htmlDom).Should().Be(placeholderText);
        }

        [Theory]
        [InlineData("test", true)]
        [InlineData("test", false)]
        public void DFC_1494_SearchResultsDidYouMeanTerm(string correctedSearchTerm, bool validSpellcheckResult)
        {
            // Arrange
            var searchView = new _MVC_Views_JobProfileSearchBox_SearchResult_cshtml();

            var model = new JobProfileSearchResultViewModel
            {
                DidYouMeanUrl = validSpellcheckResult ? $"{nameof(JobProfileSearchBoxController.SearchResultsPage)}?searchTerm={HttpUtility.UrlEncode(correctedSearchTerm)}" : string.Empty,
                DidYouMeanTerm = validSpellcheckResult ? correctedSearchTerm : string.Empty,
                SearchResults = new List<JobProfileSearchResultItemViewModel>()
            };

            // Act
            var htmlDom = searchView.RenderAsHtml(model);

            // Asserts
            if (validSpellcheckResult)
            {
                GetDidYouMeanText(htmlDom).Should().Be(model.DidYouMeanTerm);
                GetDidYouMeanUrl(htmlDom).Should().Be(model.DidYouMeanUrl);
            }
            else
            {
                GetDidYouMeanText(htmlDom).Should().BeNullOrEmpty();
            }
        }

        #endregion Tests

        #region Helpers

        private static string GetHeaderText(HtmlDocument htmlDom)
        {
            return htmlDom.DocumentNode.Descendants("h3").FirstOrDefault()?.InnerText.Trim();
        }

        private static string GetPlaceholderText(HtmlDocument htmlDom)
        {
            return htmlDom.DocumentNode.Descendants("label").FirstOrDefault()?.InnerText.Trim();
        }

        private static string GetJobProfileUrlText(HtmlDocument htmlDom)
        {
            return htmlDom.DocumentNode.Descendants("input")
                .FirstOrDefault(input => input.Attributes["type"].Value.Equals("hidden"))?.Attributes["Value"].Value;
        }

        private string GetDidYouMeanUrl(HtmlDocument htmlDom)
        {
            var didYouMeansection = htmlDom.DocumentNode.Descendants("div").ToList()
                .FirstOrDefault(div => div.Attributes["class"].Value.Contains("search-dym"));
            return didYouMeansection?.Descendants("a").FirstOrDefault()?.GetAttributeValue("href", string.Empty);
        }

        private string GetDidYouMeanText(HtmlDocument htmlDom)
        {
            var didYouMeansection = htmlDom.DocumentNode.Descendants("div")
                .FirstOrDefault(div => div.Attributes["class"].Value.Contains("search-dym"));
            return didYouMeansection?.Descendants("a").FirstOrDefault()?.InnerText.Trim();
        }

        #endregion Helpers
    }
}