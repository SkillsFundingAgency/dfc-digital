﻿using ASP;
using DFC.Digital.Data.Model;
using DFC.Digital.Web.Sitefinity.JobProfileModule.Mvc.Models;
using FluentAssertions;
using FluentAssertions.Common;
using HtmlAgilityPack;
using RazorGenerator.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace DFC.Digital.Web.Sitefinity.JobProfileModule.UnitTests
{
    /// <summary>
    /// Job Profile Section view tests
    /// </summary>
    public class JobProfileSectionViewTests
    {
        /// <summary>
        /// DFC 796 AI job profile section tests
        /// </summary>
        /// <param name="validSection">if set to <c>true</c> [valid section].</param>
        /// <param name="sectionTitle">The section title.</param>
        /// <param name="sectionContent">Content of the section.</param>
        /// <param name="propertyName">Name of the property.</param>
        [Theory]
        [InlineData(true, "How to become", "<p>How to become top to bottom</p>", "propertyName")]
        [InlineData(false, "What to become", "<p>What to become top to bottom</p>", "propertyName")]
        [InlineData(true, "Why to become", "<p>Why to become top to bottom</p>", "propertyName")]

        //As a Citizen, I want to be able to view the Job Profile page
        public void DFC796ScenarioA1ForJobProfileSection(bool validSection, string sectionTitle, string sectionContent, string propertyName)
        {
            // Arrange
            var indexView = new _MVC_Views_JobProfileSection_Index_cshtml();
            var jobProfileSectionViewModel =
                GenerateJobProfileSectionViewModelDummy(validSection, sectionTitle, sectionContent, propertyName);

            // Act
            var htmlDom = indexView.RenderAsHtml(jobProfileSectionViewModel);

            // Assert
            GetH2Heading(htmlDom).Should().BeEquivalentTo(jobProfileSectionViewModel.Title);
            if (validSection)
            {
                CountOfDescendants(htmlDom).Should().Be(4);
            }
            else
            {
                CountOfDescendants(htmlDom).Should().Be(3);
            }
        }

        [Theory]
        [InlineData(true, "Content")]
        [InlineData(false, "")]
        public void Dfc3391RestrictionsOtherRequirementsViewTests(bool validRestrictions, string content)
        {
            // Arrange
            var restrictionsView = new _MVC_Views_JobProfileSection_RestrictionsRequirements_cshtml();
            var restrictionsOtherRequirements = new RestrictionsOtherRequirements
            {
                Restrictions = GetRestrictions(validRestrictions),
                OtherRequirements = content
            };

            // Act
            var htmlDocument = restrictionsView.RenderAsHtml(restrictionsOtherRequirements);

            // Assert
            if (validRestrictions)
            {
                htmlDocument.DocumentNode.Descendants("li").Count().Should().IsSameOrEqualTo(restrictionsOtherRequirements.Restrictions.Count());
            }

            if (string.IsNullOrWhiteSpace(content) && !validRestrictions)
            {
                AssertViewIsEmpty(htmlDocument);
            }

            if (!string.IsNullOrWhiteSpace(content))
            {
                AssertContentExistsInView(content, htmlDocument);
            }
        }

        private static void AssertContentExistsInView(string content, HtmlDocument htmlDocument)
        {
            htmlDocument.DocumentNode.InnerHtml.IndexOf(content, StringComparison.OrdinalIgnoreCase).Should().BeGreaterThan(-1);
        }

        private static void AssertViewIsEmpty(HtmlDocument htmlDocument)
        {
            htmlDocument.DocumentNode.Descendants().Count().Should().Be(0);
        }

        /// <summary>
        /// Gets the h2 heading.
        /// </summary>
        /// <param name="htmlDom">The HTML DOM.</param>
        /// <returns>string</returns>
        private static string GetH2Heading(HtmlDocument htmlDom)
        {
            var h2Element = htmlDom.DocumentNode.Descendants("h2").FirstOrDefault();
            return h2Element?.InnerText;
        }

        /// <summary>
        /// Counts the of descendants.
        /// </summary>
        /// <param name="htmlDom">The HTML DOM.</param>
        /// <returns>int</returns>
        private int CountOfDescendants(HtmlDocument htmlDom)
        {
            return htmlDom.DocumentNode.Element("section").ChildNodes
                .Count(n => n.NodeType == HtmlNodeType.Element);
        }

        private JobProfileSectionViewModel GenerateJobProfileSectionViewModelDummy(
            bool validSection, string sectionTitle, string sectionContent, string propertyName)
        {
            return new JobProfileSectionViewModel
            {
                PropertyValue = validSection
                    ? $"<p>{nameof(JobProfileSectionViewModel.PropertyValue)}</p>"
                    : string.Empty,
                BottomSectionContent = sectionContent,
                TopSectionContent = sectionContent,
                Title = sectionTitle,
                PropertyName = propertyName
            };
        }

        private IEnumerable<Restriction> GetRestrictions(bool validRestrictions)
        {
            return validRestrictions
                ? new List<Restriction>
                {
                    new Restriction { Info = nameof(Restriction.Info), Title = nameof(Restriction.Title) },
                    new Restriction { Info = nameof(Restriction.Info), Title = nameof(Restriction.Title) }
                }
                : new List<Restriction>();
        }

        private bool ContentDisplayedBySectionId(HtmlDocument htmlDocument, string sectionId)
        {
            return !string.IsNullOrWhiteSpace(htmlDocument.DocumentNode.Descendants("section")?.FirstOrDefault(sec => sec.Id.Equals(sectionId))
                ?.InnerHtml);
        }
    }
}