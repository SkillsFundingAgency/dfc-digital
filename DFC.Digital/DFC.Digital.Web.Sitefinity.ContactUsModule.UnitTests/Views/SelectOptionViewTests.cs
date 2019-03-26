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
    public class SelectOptionViewTests
    {
        [Fact]
        public void Dfc7630SelectOptionViewTests()
        {
            // Arrange
            var selectOptionIndex = new _MVC_Views_SelectOption_Index_cshtml();
            var contactOptionsViewModel = new ContactOptionsViewModel
            {
                Title = nameof(ContactOptionsViewModel.Title)
            };

            // Act
            var htmlDocument = selectOptionIndex.RenderAsHtml(contactOptionsViewModel);

            // Assert
            AssertTagInnerTextValue(htmlDocument, contactOptionsViewModel.Title, "h1");
            AssertFormGroupsCounts(htmlDocument, 2);
            AssertRadioGroupsCounts(htmlDocument, 3);
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
                errorSummaryView.ViewData.ModelState.AddModelError(nameof(ContactOptionsViewModel.ContactOptionType), nameof(Exception.Message));
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
                .Any(h2 => h2.InnerText.Contains(innerText)).Should().BeTrue();
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
