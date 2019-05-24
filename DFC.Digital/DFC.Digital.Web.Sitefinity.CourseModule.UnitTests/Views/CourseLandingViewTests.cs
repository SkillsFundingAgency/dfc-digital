using ASP;
using DFC.Digital.Web.Sitefinity.CourseModule.Mvc.Models;
using FluentAssertions;
using HtmlAgilityPack;
using RazorGenerator.Testing;
using System;
using System.Linq;
using Xunit;

namespace DFC.Digital.Web.Sitefinity.CourseModule.UnitTests
{
    public class CourseLandingViewTests
    {
        [Fact]
        public void Dfc7053CourseLandingViewTests()
        {
            // Arrange
            var courseLandingIndex = new _MVC_Views_CourseLanding_Index_cshtml();
            var courseLandingViewModel = new CourseLandingViewModel
            {
                CourseName = nameof(CourseLandingViewModel.CourseName)
            };

            // Act
            var htmlDocument = courseLandingIndex.RenderAsHtml(courseLandingViewModel);

            // Assert
            this.AssertFormGroupsCounts(htmlDocument, 5);

        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void Dfc7053CourseNameCompulsoryTest(bool modelStateInvalid)
        {
            // Arrange
            var errorMessageView = new _MVC_Views_CourseLanding_Index_cshtml();
            var courseLandingViewModel = new CourseLandingViewModel
            {
                CourseName = nameof(CourseLandingViewModel.CourseName)
            };

            if (modelStateInvalid)
            {
                errorMessageView.ViewData.ModelState.AddModelError(nameof(CourseLandingViewModel.CourseName), nameof(Exception.Message));
            }

            // Act
            var htmlDocument = errorMessageView.RenderAsHtml(courseLandingViewModel);

            // Assert
            if (modelStateInvalid)
            {
                this.AssertErrorDetailOnField(htmlDocument, nameof(Exception.Message));
            }
            else
            {
                AssertErrorFieldIsEmpty(htmlDocument);
            }
        }

        private static void AssertErrorFieldIsEmpty(HtmlDocument htmlDocument)
        {
            htmlDocument.DocumentNode.Descendants("span")
               .Count(span => span.InnerText.Contains("Message")).Should().Be(0);
        }

        private void AssertErrorDetailOnField(HtmlDocument htmlDocument, string errorMessage)
        {
            htmlDocument.DocumentNode.Descendants("span")
                .Count(span => span.InnerText.Contains("Message")).Should().BeGreaterThan(0);
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
    }
}
