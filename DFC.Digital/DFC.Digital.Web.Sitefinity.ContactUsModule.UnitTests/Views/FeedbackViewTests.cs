using ASP;
using DFC.Digital.Web.Sitefinity.ContactUsModule.Mvc.Models;
using FluentAssertions;
using HtmlAgilityPack;
using RazorGenerator.Testing;
using System;
using System.Linq;
using Xunit;

namespace DFC.Digital.Web.Sitefinity.ContactUsModule.UnitTests
{
    public class FeedbackViewTests
    {
        [Fact]
        public void Dfc7630FeedbackViewTests()
        {
            // Arrange
            var feedbackIndex = new _MVC_Views_Feedback_Index_cshtml();
            var feedbackViewModel = new GeneralFeedbackViewModel
            {
                Title = nameof(GeneralFeedbackViewModel.Title)
            };

            // Act
            var htmlDocument = feedbackIndex.RenderAsHtml(feedbackViewModel);

            // Assert
            AssertTagInnerTextValue(htmlDocument, feedbackViewModel.Title, "h1");
            AssertFormGroupsCounts(htmlDocument, 3);
            AssertRadioGroupsCounts(htmlDocument, 7);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void Dfc7630ErrorSummaryViewTests(bool modelStateInvalid)
        {
            // Arrange
            var errorSummaryView = new _MVC_Views_Shared_ErrorSummary_cshtml();

            if (modelStateInvalid)
            {
                errorSummaryView.ViewData.ModelState.AddModelError(nameof(GeneralFeedbackViewModel.FeedbackQuestionType), nameof(Exception.Message));
            }

            // Act
            var htmlDocument = errorSummaryView.RenderAsHtml();

            // Assert
            if (modelStateInvalid)
            {
                AssertErrorDetailInSummary(htmlDocument, nameof(Exception.Message));
            }
            else
            {
                AssertSummaryListViewIsEmpty(htmlDocument);
            }
        }

        private static void AssertSummaryListViewIsEmpty(HtmlDocument htmlDocument)
        {
            htmlDocument.DocumentNode.Descendants("li").Count().Should().Be(0);
        }

        private void AssertErrorDetailInSummary(HtmlDocument htmlDocument, string errorMessage)
        {
            htmlDocument.DocumentNode.Descendants("h2")
                .Count(h2 => h2.InnerText.Contains("There is a problem")).Should().BeGreaterThan(0);
            htmlDocument.DocumentNode.Descendants("a")
                .Count(a => a.InnerText.Contains(errorMessage)).Should().BeGreaterThan(0);

        }

        private void AssertTagInnerTextValue(HtmlDocument htmlDocument, string innerText, string tag)
        {
            htmlDocument.DocumentNode.Descendants(tag)
                .Any(h1 => h1.InnerText.Contains(innerText)).Should().BeTrue();
        }

        private void AssertFormGroupsCounts(HtmlDocument htmlDocument, int count)
        {
            htmlDocument.DocumentNode.Descendants("div")
                .Count(div => div.Attributes["class"].Value.Contains("govuk-form-group")).Should().Be(count);
        }

        private void AssertRadioGroupsCounts(HtmlDocument htmlDocument, int count)
        {
            htmlDocument.DocumentNode.Descendants("div")
                .Count(div => div.Attributes["class"].Value.Contains("govuk-radios__item")).Should().Be(count);
        }
    }
}
