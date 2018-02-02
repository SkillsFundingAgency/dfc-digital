using ASP;
using DFC.Digital.Web.Sitefinity.JobProfileModule.Mvc.Models;
using FluentAssertions;
using HtmlAgilityPack;
using RazorGenerator.Testing;
using System.Linq;
using Xunit;

namespace DFC.Digital.Web.Sitefinity.JobProfileModule.Tests.Views
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
        public void DFC_796_A1_JobProfileSection(bool validSection, string sectionTitle, string sectionContent, string propertyName)
        {
            // Arrange
            var indexView = new _MVC_Views_JobProfileSection_Index_cshtml();
            var jobProfileSectionViewModel =
                GenerateJobProfileSectionViewModelDummy(validSection, sectionTitle, sectionContent, propertyName);

            // Act
            var htmlDom = indexView.RenderAsHtml(jobProfileSectionViewModel);

            // Assert
            GetH2Heading(htmlDom).ShouldBeEquivalentTo(jobProfileSectionViewModel.Title);
            if (validSection)
            {
                CountOfDescendants(htmlDom).ShouldBeEquivalentTo(4);
            }
            else
            {
                CountOfDescendants(htmlDom).ShouldBeEquivalentTo(3);
            }
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
                .Where(n => n.NodeType == HtmlNodeType.Element)
                .Count();
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
    }
}