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
                SearchTerm = nameof(CourseLandingViewModel.SearchTerm)
            };

            // Act
            var htmlDocument = courseLandingIndex.RenderAsHtml(courseLandingViewModel);

            // Assert
            this.AssertFormGroupsCounts(htmlDocument, 5);
        }

        private void AssertFormGroupsCounts(HtmlDocument htmlDocument, int count)
        {
            htmlDocument.DocumentNode.Descendants("div")
                .Count(div => div.Attributes["class"].Value.Contains("govuk-form-group")).Should().Be(count);
        }
    }
}
