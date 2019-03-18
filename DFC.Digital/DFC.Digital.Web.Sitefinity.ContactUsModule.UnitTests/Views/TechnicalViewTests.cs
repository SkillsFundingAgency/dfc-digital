namespace DFC.Digital.Web.Sitefinity.ContactUsModule.UnitTests.Views
{
    using System.Linq;
    using ASP;
    using DFC.Digital.Web.Sitefinity.ContactUsModule.Mvc.Models;
    using FluentAssertions;
    using RazorGenerator.Testing;
    using Xunit;

    public class TechnicalViewTests
    {
        [Fact]
        public void TechnicalViewTest()
        {
            // Arrange
            var technicalIndex = new _MVC_Views_Technical_Index_cshtml();
            var contactUsTechnicalViewModel = new TechnicalFeedbackViewModel()
            {
                TechnicalFeedback = new Data.Model.TechnicalFeedback() { Message = "Dummy message" },
                Title = "Dummy Title",
                PageIntroduction = "Dummy Intro",
                PersonalInformation = "Dummy Personal",
                CharacterLimit = "Dummy Limit",
                MessageLabel = "Dummy Message Label"
            };

            // Act
            var htmlDocument = technicalIndex.RenderAsHtml(contactUsTechnicalViewModel);

            //Asserts
            var title = htmlDocument.DocumentNode.SelectNodes("h1").Where(d => d.GetAttributeValue("class", string.Empty).Contains("govuk-heading-xl")).FirstOrDefault();
            title.InnerText.Should().Be(contactUsTechnicalViewModel.Title);

            var pageIntroduction = htmlDocument.DocumentNode.SelectNodes("p").Where(d => d.GetAttributeValue("class", string.Empty).Contains("govuk-body-m")).FirstOrDefault();
            pageIntroduction.InnerText.Should().Be(contactUsTechnicalViewModel.PageIntroduction);

            var characterLimit = htmlDocument.DocumentNode.SelectNodes("//p").Where(d => d.GetAttributeValue("class", string.Empty).Contains("govuk-body-s")).FirstOrDefault();
            characterLimit.InnerText.Should().Be(contactUsTechnicalViewModel.CharacterLimit);

            var label = htmlDocument.DocumentNode.SelectNodes("//label").Where(d => d.GetAttributeValue("class", string.Empty).Contains("govuk-label")).FirstOrDefault();
            label.InnerText.Should().Be(contactUsTechnicalViewModel.MessageLabel);

            var personalInformation = htmlDocument.DocumentNode.SelectNodes("//span").Where(d => d.GetAttributeValue("class", string.Empty).Contains("govuk-hint")).FirstOrDefault();
            personalInformation.InnerText.Should().Be(contactUsTechnicalViewModel.PersonalInformation);

            var message = htmlDocument.DocumentNode.SelectNodes("//textarea").Where(d => d.GetAttributeValue("name", string.Empty).Equals("Message")).FirstOrDefault();
            message.InnerText.Should().Contain(contactUsTechnicalViewModel.TechnicalFeedback.Message);
        }
    }
}
