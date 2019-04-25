using ASP;
using DFC.Digital.Web.Sitefinity.ContactUsModule.Mvc.Models;
using FluentAssertions;
using RazorGenerator.Testing;
using System.Linq;
using Xunit;

namespace DFC.Digital.Web.Sitefinity.ContactUsModule.UnitTests
{
    public class TechnicalViewTests
    {
        [Fact]
        public void TechnicalViewTest()
        {
            // Arrange objects
            var technicalIndex = new _MVC_Views_Technical_Index_cshtml();
            var technicalFeedbackViewModel = new TechnicalFeedbackViewModel()
            {
                Message = "Dummy message",
                Title = "Dummy Title",
                PageIntroduction = "Dummy Intro",
                PersonalInformation = "Dummy Personal",
                CharacterLimit = "Dummy Limit",
                MessageLabel = "Dummy Message Label"
            };

            // Act
            var htmlDocument = technicalIndex.RenderAsHtml(technicalFeedbackViewModel);

            //Asserts
            var title = htmlDocument.DocumentNode.SelectNodes("h1").Where(d => d.GetAttributeValue("class", string.Empty).Contains("govuk-heading-xl")).FirstOrDefault();
            title.InnerText.Should().Be(technicalFeedbackViewModel.Title);

            var pageIntroduction = htmlDocument.DocumentNode.SelectNodes("p").Where(d => d.GetAttributeValue("class", string.Empty).Contains("govuk-body-m")).FirstOrDefault();
            pageIntroduction.InnerText.Should().Be(technicalFeedbackViewModel.PageIntroduction);

            var characterLimit = htmlDocument.DocumentNode.SelectNodes("//span").Where(d => d.GetAttributeValue("class", string.Empty).Contains("govuk-character-count__message")).FirstOrDefault();
            characterLimit.InnerText.Should().Be(technicalFeedbackViewModel.CharacterLimit);

            var label = htmlDocument.DocumentNode.SelectNodes("//label").Where(d => d.GetAttributeValue("class", string.Empty).Contains("govuk-label")).FirstOrDefault();
            label.InnerText.Should().Be(technicalFeedbackViewModel.MessageLabel);

            var personalInformation = htmlDocument.DocumentNode.SelectNodes("//span").Where(d => d.GetAttributeValue("class", string.Empty).Contains("govuk-hint")).FirstOrDefault();
            personalInformation.InnerText.Should().Be(technicalFeedbackViewModel.PersonalInformation);

            var message = htmlDocument.DocumentNode.SelectNodes("//textarea").Where(d => d.GetAttributeValue("name", string.Empty).Equals("Message")).FirstOrDefault();
            message.InnerText.Should().Contain(technicalFeedbackViewModel.Message);
        }
    }
}