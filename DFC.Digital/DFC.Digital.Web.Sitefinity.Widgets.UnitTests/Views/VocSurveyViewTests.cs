using ASP;
using DFC.Digital.Web.Sitefinity.Widgets.Mvc.Models;
using FluentAssertions;
using HtmlAgilityPack;
using RazorGenerator.Testing;
using System.Linq;
using Xunit;

namespace DFC.Digital.Web.Sitefinity.Widgets.UnitTests.Views
{
    public class VocSurveyViewTests
    {
        [Fact]

        //As a Citizen, I want to be able to give feedback on the BETA Service (VOC: Exit Route)
        public void DFC_685_A1_VocSurveyIndex()
        {
            // Arrange
            var indexView = new _MVC_Views_VocSurvey_Index_cshtml();
            var vocSurveyVmDummy =
                GenerateVocSurveyViewModelDummy(nameof(VocSurveyViewModel.AgeLimitText),
                    nameof(VocSurveyViewModel.DontHaveEmailText),
                    nameof(VocSurveyViewModel.EmailSentText),
                    nameof(VocSurveyViewModel.FormIntroText),
                    nameof(VocSurveyViewModel.EmailNotSentText));

            // Act
            var htmlDom = indexView.RenderAsHtml(vocSurveyVmDummy);

            // Assert
            GetViewModelData(htmlDom).ShouldBeEquivalentTo(vocSurveyVmDummy);
        }

        [Theory]
        [InlineData("success message")]
        [InlineData("no success message")]
        [InlineData("")]
        public void DFC_685_A2_VocSurveyResponse(string response)
        {
            // Arrange
            var indexView = new _MVC_Views_VocSurvey_Response_cshtml();
            var emailSubmissionViewModelDummy =
                new EmailSubmissionViewModel
                {
                    ResponseMessage = response
                };

            // Act
            var htmlDom = indexView.RenderAsHtml(emailSubmissionViewModelDummy);

            // Assert
            GetViewErrorMessage(htmlDom).ShouldBeEquivalentTo(response);
        }

        private string GetViewErrorMessage(HtmlDocument htmlDom)
        {
            return htmlDom.DocumentNode.Descendants("h2").FirstOrDefault()?.InnerText;
        }

        private VocSurveyViewModel GetViewModelData(HtmlDocument htmlDom)
        {
            return new VocSurveyViewModel
            {
                EmailNotSentText = GetEmailSentMessage(htmlDom, "js-survey-not-complete"),
                AgeLimitText = GetAgeLimitText(htmlDom),
                DontHaveEmailText = GetDontHaveEmailText(htmlDom),
                EmailSentText = GetEmailSentMessage(htmlDom, "js-survey-complete"),
                FormIntroText = htmlDom.DocumentNode.Descendants("p").FirstOrDefault()?.InnerText
            };
        }

        private string GetEmailSentMessage(HtmlDocument htmlDom, string itemClass)
        {
            var divItem = htmlDom.DocumentNode.Descendants("div").FirstOrDefault(div =>
                div.HasAttributes && div.Attributes["class"].Value.ToLower().Contains(itemClass.ToLower()));

            if (divItem != null)
            {
                return divItem.Descendants("p").FirstOrDefault()?.InnerText;
            }

            return string.Empty;
        }

        private string GetDontHaveEmailText(HtmlDocument htmlDom)
        {
            var anchorTag = htmlDom.DocumentNode.Descendants("a").FirstOrDefault(anchr =>
                anchr.HasAttributes && anchr.Attributes["class"].Value.ToLower().Contains("survey_link"));

            return anchorTag?.InnerText;
        }

        private string GetAgeLimitText(HtmlDocument htmlDom)
        {
            var label = htmlDom.DocumentNode.Descendants("label").FirstOrDefault(lbl =>
                lbl.HasAttributes && lbl.Attributes["class"].Value.ToLower().Contains("survey_label"));

            return label?.Descendants("span").FirstOrDefault()?.InnerText;
        }

        private VocSurveyViewModel GenerateVocSurveyViewModelDummy
        (
            string ageLimitText,
            string dontHaveEmailText,
            string emailSentText,
            string formIntroText,
            string emailNotSentText)
        {
            var result = new VocSurveyViewModel
            {
                AgeLimitText = ageLimitText,
                DontHaveEmailText = dontHaveEmailText,
                EmailSentText = emailSentText,
                FormIntroText = formIntroText,
                EmailNotSentText = emailNotSentText
            };

            return result;
        }
    }
}