using ASP;
using DFC.Digital.Web.Sitefinity.JobProfileModule.Mvc.Models;
using FluentAssertions;
using FluentAssertions.Common;
using HtmlAgilityPack;
using RazorGenerator.Testing;
using System;
using System.Linq;
using Xunit;

namespace DFC.Digital.Web.Sitefinity.JobProfileModule.UnitTests
{
    public class JobProfileWhatYouWillDoViewTests
    {
        [Theory]
        [InlineData(true, "Content")]
        [InlineData(false, "Content")]
        public void WhatYouWillDoIndexViewTests(bool cadReady, string content)
        {
            // Arrange
            var whatYouWillDoView = new _MVC_Views_JobProfileWhatYouWillDo_Index_cshtml();
            var jobProfileWhatYouWillDoViewModel = new JobProfileWhatYouWillDoViewModel
            {
                PropertyValue = content,
            };

            // Act
            var htmlDocument = whatYouWillDoView.RenderAsHtml(jobProfileWhatYouWillDoViewModel);

            // Assert
            if (cadReady)
            {
                htmlDocument.DocumentNode.Should().NotBe(null);
            }

            if (!string.IsNullOrWhiteSpace(content))
            {
                AssertContentExistsInView(content, htmlDocument);
            }
        }

        [Theory]
        [InlineData(true, "Environment", "Location", "Uniform")]
        [InlineData(false, "", "", "")]
        public void WhatYouWillDoWorkingEnvironmentViewTests(bool workingEnvironments, string environment, string location, string uniform)
        {
            // Arrange
            var workingEnvironment = new _MVC_Views_JobProfileWhatYouWillDo_Workingenvironments_cshtml();
            var workingEnvironmentView = new JobProfileWhatYouWillDoViewModel
            {
                EnvironmentTitle = GetEnvironments(workingEnvironments),
                Environment = environment,
                Uniform = uniform,
                Location = location
            };

            // Act
            var htmlDocument = workingEnvironment.RenderAsHtml(workingEnvironmentView);

            // Assert
            if (workingEnvironments)
            {
                htmlDocument.DocumentNode.Should().NotBe(null);
            }

            if (string.IsNullOrWhiteSpace(environment) && string.IsNullOrWhiteSpace(uniform) && string.IsNullOrWhiteSpace(location))
            {
                AssertViewIsEmpty(htmlDocument);
            }

            if (!string.IsNullOrWhiteSpace(environment) && string.IsNullOrWhiteSpace(uniform) && string.IsNullOrWhiteSpace(location))
            {
                AssertContentExistsInView(environment, htmlDocument);
                AssertContentExistsInView(uniform, htmlDocument);
                AssertContentExistsInView(location, htmlDocument);
            }
        }

        private static string GetEnvironments(bool workingEnvironments)
        {
            return workingEnvironments ? "Working Enviroments" : null;
        }

        private static void AssertContentExistsInView(string element,  HtmlDocument htmlDocument)
        {
            htmlDocument.DocumentNode.InnerHtml.IndexOf(element, StringComparison.OrdinalIgnoreCase).Should().BeGreaterThan(-1);
        }

        private static void AssertViewIsEmpty(HtmlDocument htmlDocument)
        {
            htmlDocument.DocumentNode.Descendants().Count().Should().Be(0);
        }
    }
}