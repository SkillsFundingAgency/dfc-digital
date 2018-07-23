using System;
using System.Collections.Generic;
using System.Linq;
using ASP;
using FluentAssertions;
using HtmlAgilityPack;
using RazorGenerator.Testing;
using Xunit;

namespace DFC.Digital.Web.Sitefinity.CmsExtensions.UnitTests.Views
{
    public class SkillsFrameworkImportViewTests
    {
        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void Dfc3715SkillsFrameworkImportIndex(bool isAdmin)
        {
            // Arrange
            var indexView = new _MVC_Views_SkillsFrameworkDataImport_Index_cshtml();
            var skillsFrameworkImportViewModel = new SkillsFrameworkImportViewModel
            {
                PageTitle = nameof(SkillsFrameworkImportViewModel.PageTitle),
                NotAllowedMessage = nameof(SkillsFrameworkImportViewModel.NotAllowedMessage),
                FirstParagraph = nameof(SkillsFrameworkImportViewModel.FirstParagraph),
                IsAdmin = isAdmin
            };

            // Act
            var htmlDom = indexView.RenderAsHtml(skillsFrameworkImportViewModel);

            // Assert
            AssertContentExistsInView(skillsFrameworkImportViewModel.PageTitle, htmlDom);
            if (isAdmin)
            {
                AssertContentDoesNotExistsInView(skillsFrameworkImportViewModel.NotAllowedMessage, htmlDom);
                AssertContentExistsInView(skillsFrameworkImportViewModel.FirstParagraph, htmlDom);
                AssertContentDoesNotExistsInView(skillsFrameworkImportViewModel.NotAllowedMessage, htmlDom);
                AssertImportDropDownAvailable(htmlDom, true);
            }
            else
            {
                AssertContentExistsInView(skillsFrameworkImportViewModel.NotAllowedMessage, htmlDom);
                AssertImportDropDownAvailable(htmlDom, false);
            }
        }

        [Theory]
        [InlineData(true, "import yy skills",2, 4)]
        [InlineData(true, "update xx skills", 3, 10)]
        [InlineData(true, "update occupdationl codes", 3, 8)]
        [InlineData(true, "import random other skills", 12, 4)]
        public void Dfc3715SkillsFrameworkImportImportResults(bool isAdmin, string actionCompleted, int numberOfKeys,  int numberOfAuditRecords)
        {
            // Arrange
            var indexView = new _MVC_Views_SkillsFrameworkDataImport_ImportResults_cshtml();
            var skillsFrameworkResultsViewModel = new SkillsFrameworkResultsViewModel
            {
                PageTitle = nameof(SkillsFrameworkImportViewModel.PageTitle),
                NotAllowedMessage = nameof(SkillsFrameworkImportViewModel.NotAllowedMessage),
                FirstParagraph = nameof(SkillsFrameworkImportViewModel.FirstParagraph),
                IsAdmin = isAdmin,
                ActionCompleted = actionCompleted,
                AuditRecords = GetAuditRecords(numberOfKeys, numberOfAuditRecords)
            };

            // Act
            var htmlDom = indexView.RenderAsHtml(skillsFrameworkResultsViewModel);

            // Assert
            AssertContentExistsInView(skillsFrameworkResultsViewModel.PageTitle, htmlDom);
            if (isAdmin)
            {
                AssertContentDoesNotExistsInView(skillsFrameworkResultsViewModel.NotAllowedMessage, htmlDom);
                AssertContentExistsInView(skillsFrameworkResultsViewModel.FirstParagraph, htmlDom);
                AssertContentExistsInView(skillsFrameworkResultsViewModel.ActionCompleted, htmlDom);
                AssertContentDoesNotExistsInView(skillsFrameworkResultsViewModel.NotAllowedMessage, htmlDom);
                AssertImportDropDownAvailable(htmlDom, true);
                foreach (var key in skillsFrameworkResultsViewModel.AuditRecords.Keys)
                {
                    AssertContentExistsInView(key, htmlDom);
                }
            }
            else
            {
                AssertContentExistsInView(skillsFrameworkResultsViewModel.NotAllowedMessage, htmlDom);
                AssertImportDropDownAvailable(htmlDom, false);
            }
        }

        private static void AssertContentExistsInView(string element, HtmlDocument htmlDocument)
        {
            htmlDocument.DocumentNode.InnerHtml.IndexOf(element, StringComparison.OrdinalIgnoreCase).Should().BeGreaterThan(-1);
        }

        private static void AssertContentDoesNotExistsInView(string element, HtmlDocument htmlDocument)
        {
            htmlDocument.DocumentNode.InnerHtml.IndexOf(element, StringComparison.OrdinalIgnoreCase).Should().BeLessOrEqualTo(-1);
        }

        private static void AssertImportDropDownAvailable(HtmlDocument htmlDocument, bool exists)
        {
                htmlDocument.DocumentNode.Descendants("select")?.Any().Should()
                    .Be(exists);
        }

        private static IDictionary<string, IList<string>> GetAuditRecords(int numberofKeys, int numberOfRecords)
        {
            var records = new Dictionary<string, IList<string>>();
            var recordsDetail = new List<string>();

            for (var i = 0; i < numberOfRecords; i++)
            {
                recordsDetail.Add($"details{i}");
            }

            for (var i = 0; i < numberofKeys; i++)
            {
                records.Add($"key{i}", recordsDetail);
            }

            return records;
        }
    }
}