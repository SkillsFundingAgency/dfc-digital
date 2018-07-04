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
        [InlineData(true, "jpWydData1", "topCnt", "bottomContent", "wydDayToDay", true, "wydDescription", "sectionid")]
        [InlineData(true, "jpWydData2", "topCnt3", "bottomContent", "wydDayToDay", false, "wydDescription", "sectionid2")]
        [InlineData(false, "jpWydData3", "topCnt8", "bottomContent", "wydDayToDay", false, "wydDescription", "sectionid4")]
        public void WhatYouWillDoIndexViewTests(bool cadReady, string jpWydData, string topContent, string bottomContent, string wydDayToDay, bool wydIntroActive, string wydDescription, string sectionId)
        {
            // Arrange
            var whatYouWillDoView = new _MVC_Views_JobProfileWhatYouWillDo_Index_cshtml();
            var jobProfileWhatYouWillDoViewModel = new JobProfileWhatYouWillDoViewModel
            {
                PropertyValue = jpWydData,
                TopSectionContent = topContent,
                BottomSectionContent = bottomContent,
                IsIntroActive = wydIntroActive,
                DailyTasks = wydDayToDay,
                Introduction = wydDescription,
                SectionId = sectionId,
                IsWhatYouWillDoCadView = cadReady
            };

            // Act
            var htmlDocument = whatYouWillDoView.RenderAsHtml(jobProfileWhatYouWillDoViewModel);

            AssertSectionIdDisplayed(sectionId, htmlDocument);

            // Assert
            if (cadReady)
            {
                AssertContentDoesNotExistsInView(bottomContent, htmlDocument);
                AssertContentDoesNotExistsInView(topContent, htmlDocument);
                AssertContentDoesNotExistsInView(jpWydData, htmlDocument);
                if (!string.IsNullOrWhiteSpace(wydDayToDay))
                {
                    AssertContentExistsInView(wydDayToDay, htmlDocument);
                }

                if (!string.IsNullOrWhiteSpace(wydDescription) && wydIntroActive)
                {
                    AssertContentExistsInView(wydDescription, htmlDocument);
                }
                else
                {
                    AssertContentDoesNotExistsInView(wydDescription, htmlDocument);
                }
            }
            else
            {
                AssertContentDoesNotExistsInView(wydDayToDay, htmlDocument);
                AssertContentDoesNotExistsInView(wydDescription, htmlDocument);
                if (!string.IsNullOrWhiteSpace(jpWydData))
                {
                    AssertContentExistsInView(jpWydData, htmlDocument);
                }

                if (!string.IsNullOrWhiteSpace(topContent))
                {
                    AssertContentExistsInView(topContent, htmlDocument);
                }

                if (!string.IsNullOrWhiteSpace(bottomContent))
                {
                    AssertContentExistsInView(bottomContent, htmlDocument);
                }
            }
        }

        [Theory]
        [InlineData("Environment Title", "Environment", "Location", "Uniform")]
        [InlineData("Environment Title", "", "", "")]
        public void WhatYouWillDoWorkingEnvironmentViewTests(string environmentTitle, string environment, string location, string uniform)
        {
            // Arrange
            var workingEnvironment = new _MVC_Views_JobProfileWhatYouWillDo_WorkingEnvironments_cshtml();
            var workingEnvironmentView = new JobProfileWhatYouWillDoViewModel
            {
                EnvironmentTitle = environmentTitle,
                Environment = environment,
                Uniform = uniform,
                Location = location
            };

            // Act
            var htmlDocument = workingEnvironment.RenderAsHtml(workingEnvironmentView);

            // Assert
            if (string.IsNullOrWhiteSpace(environment) && string.IsNullOrWhiteSpace(uniform) && string.IsNullOrWhiteSpace(location))
            {
                AssertViewIsEmpty(htmlDocument);
                AssertContentDoesNotExistsInView(environmentTitle, htmlDocument);
            }

            if (!string.IsNullOrWhiteSpace(environment))
            {
                AssertContentExistsInView(environment, htmlDocument);
            }

            if (!string.IsNullOrWhiteSpace(uniform))
            {
                AssertContentExistsInView(uniform, htmlDocument);
            }

            if (!string.IsNullOrWhiteSpace(location))
            {
                AssertContentExistsInView(location, htmlDocument);
            }
        }

        private static void AssertSectionIdDisplayed(string sectionId, HtmlDocument htmlDocument)
        {
            htmlDocument.DocumentNode.Descendants("section")
                       .FirstOrDefault(section => section.Id.Equals(sectionId)).Should().NotBeNull();
        }

        private static void AssertContentExistsInView(string element,  HtmlDocument htmlDocument)
        {
            htmlDocument.DocumentNode.InnerHtml.IndexOf(element, StringComparison.OrdinalIgnoreCase).Should().BeGreaterThan(-1);
        }

        private static void AssertContentDoesNotExistsInView(string element, HtmlDocument htmlDocument)
        {
            htmlDocument.DocumentNode.InnerHtml.IndexOf(element, StringComparison.OrdinalIgnoreCase).Should().BeLessOrEqualTo(-1);
        }

        private static void AssertViewIsEmpty(HtmlDocument htmlDocument)
        {
            htmlDocument.DocumentNode.Descendants().Count().Should().Be(0);
        }
    }
}