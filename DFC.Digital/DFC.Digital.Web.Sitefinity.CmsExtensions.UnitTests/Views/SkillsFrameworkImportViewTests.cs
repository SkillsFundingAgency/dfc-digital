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
        [InlineData(true,10,5,5)]
        [InlineData(false,6000989,10,10)]
        public void Dfc3715SkillsFrameworkImportIndex(bool isAdmin,int awaitingUpdate, int selectedForUpdate,int updateCompleted)
        {
            // Arrange
            var indexView = new _MVC_Views_SkillsFrameworkDataImport_Index_cshtml();
            var skillsFrameworkImportViewModel = new SkillsFrameworkImportViewModel
            {
                PageTitle = nameof(SkillsFrameworkImportViewModel.PageTitle),
                NotAllowedMessage = nameof(SkillsFrameworkImportViewModel.NotAllowedMessage),
                FirstParagraph = nameof(SkillsFrameworkImportViewModel.FirstParagraph),
                SocMappingStatus = new Data.Model.SocMappingStatus { AwaitingUpdate= awaitingUpdate, SelectedForUpdate=selectedForUpdate, UpdateCompleted=updateCompleted},
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
                AssertSocMappingStatusValues(htmlDom, "awaiting-update", $"SOCs awaiting import : {skillsFrameworkImportViewModel.SocMappingStatus.AwaitingUpdate}");
                AssertSocMappingStatusValues(htmlDom, "update-completed", $"SOCs completed : { skillsFrameworkImportViewModel.SocMappingStatus.AwaitingUpdate}");
                AssertSocMappingStatusValues(htmlDom, "selected-for-update", $"SOCs started but not completed : { skillsFrameworkImportViewModel.SocMappingStatus.SelectedForUpdate}");
                AssertNextBatchOfSOCsToImportValues(htmlDom, "next-batch-of-SOCs-to-import", $"{ skillsFrameworkImportViewModel.NextBatchOfSOCsToImport}");
            }
            else
            {
                AssertContentExistsInView(skillsFrameworkImportViewModel.NotAllowedMessage, htmlDom);
                AssertImportDropDownAvailable(htmlDom, false);
            }
        }

        private void AssertSocMappingStatusValues(HtmlDocument htmlDom, string socMappingStatusId, string innerTextValue)
        {
            htmlDom.DocumentNode.Descendants("td")?.FirstOrDefault(td => td.Id == socMappingStatusId).InnerText.Equals(innerTextValue);
        }
        private void AssertNextBatchOfSOCsToImportValues(HtmlDocument htmlDom, string nextBatchOfSOCsToImportInputId, string inputValue)
        {
            htmlDom.DocumentNode.Descendants("input")?.FirstOrDefault(td => td.Id == nextBatchOfSOCsToImportInputId).InnerText.Equals(inputValue);
        }

        [Theory]
        [InlineData(true,"import yy skills","other message",2, 4)]
        [InlineData(true, "update xx skills", "other message", 3, 10)]
        [InlineData(true, "update occupdationl codes", "other message", 3, 8)]
        [InlineData(true, "import random other skills", "other message", 12, 4)]
        
        public void Dfc3715SkillsFrameworkImportImportResults(bool isAdmin, string actionCompleted, string otherMessage, int numberOfKeys,  int numberOfAuditRecords)
        {
            // Arrange
            var indexView = new _MVC_Views_SkillsFrameworkDataImport_ImportResults_cshtml();
            var skillsFrameworkResultsViewModel = new SkillsFrameworkResultsViewModel
            {
                PageTitle = nameof(SkillsFrameworkImportViewModel.PageTitle),
                NotAllowedMessage = nameof(SkillsFrameworkImportViewModel.NotAllowedMessage),
                IsAdmin = isAdmin,
                ActionCompleted = actionCompleted,
                OtherMessage= otherMessage,
                AuditRecords = GetAuditRecords(numberOfKeys, numberOfAuditRecords)
            };

            // Act
            var htmlDom = indexView.RenderAsHtml(skillsFrameworkResultsViewModel);

            // Assert
            AssertContentExistsInView(skillsFrameworkResultsViewModel.PageTitle, htmlDom);
            if (isAdmin)
            {
                AssertContentDoesNotExistsInView(skillsFrameworkResultsViewModel.NotAllowedMessage, htmlDom);
                AssertContentExistsInView(skillsFrameworkResultsViewModel.ActionCompleted, htmlDom);
                AssertContentDoesNotExistsInView(skillsFrameworkResultsViewModel.NotAllowedMessage, htmlDom);
                foreach (var key in skillsFrameworkResultsViewModel.AuditRecords.Keys)
                {
                    AssertContentExistsInView(key, htmlDom);
                }
            }
            if (!String.IsNullOrEmpty(otherMessage))
            {
                AssertOtherMessageValues(htmlDom, skillsFrameworkResultsViewModel.OtherMessage, "other-message-heading", "other-message-paragraph", "Other Messages");
            }
        }
        private void AssertOtherMessageValues(HtmlDocument htmlDom,string otherMessageValue, string otherMessageHeadingId, string otherMessageParagraphId, string otherMessageHeading)
        {
            htmlDom.DocumentNode.Descendants("h3")?.FirstOrDefault(td => td.Id == otherMessageHeadingId).InnerText.Equals(otherMessageHeading);

            htmlDom.DocumentNode.Descendants("p")?.FirstOrDefault(td => td.Id == otherMessageParagraphId).InnerText.Equals(otherMessageValue);
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