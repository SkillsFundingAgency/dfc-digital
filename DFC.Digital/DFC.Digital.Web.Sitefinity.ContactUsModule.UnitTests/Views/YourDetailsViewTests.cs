using System.Linq;
using ASP;
using DFC.Digital.Web.Sitefinity.ContactUsModule.Mvc.Models;
using FluentAssertions;
using HtmlAgilityPack;
using RazorGenerator.Testing;
using Xunit;

namespace DFC.Digital.Web.Sitefinity.ContactUsModule.UnitTests
{
    public class YourDetailsViewTests
    {
        [Theory]
        [InlineData(ContactOption.ContactAdviser)]
        [InlineData(ContactOption.Feedback)]
        [InlineData(ContactOption.Technical)]
        public void Dfc7630YourDetailsViewTests(ContactOption contactOption)
        {
            // Arrange
            var yourDetailsIndex = new _MVC_Views_YourDetails_Index_cshtml();
            var contactUsViewModel = new ContactUsViewModel
            {
                FirstName = nameof(ContactUsViewModel.FirstName),
                LastName = nameof(ContactUsViewModel.LastName),
                Email = "test@mail.com",
                EmailConfirm = "test@mail.com",
                DobDay = "10",
                DobMonth = "10",
                DobYear = "1970",
                TermsAndConditions = true,
                ContactOption = contactOption
            };

            // Act
            var htmlDocument = yourDetailsIndex.RenderAsHtml(contactUsViewModel);

            // Assert
            if (contactOption == ContactOption.ContactAdviser)
            {
                AssertDobAndPostCodeExistsInView(htmlDocument);
            }
            else
            {
                AssertIsContactableExistsInView(htmlDocument);
            }
        }

        private void AssertIsContactableExistsInView(HtmlDocument htmlDocument)
        {
            htmlDocument.DocumentNode.Descendants("h2")
                .Count(h2 => h2.InnerText.Contains("Do you want us to contact you?")).Should().BeGreaterThan(0);
            htmlDocument.DocumentNode.Descendants("span")
                .Count(h2 => h2.Id.Equals("dob-hint")).Should().Be(0);
            htmlDocument.DocumentNode.Descendants("span")
                .Count(h2 => h2.Id.Equals("postcode-hint")).Should().Be(0);
        }

        private void AssertDobAndPostCodeExistsInView(HtmlDocument htmlDocument)
        {
            htmlDocument.DocumentNode.Descendants("span")
                .Count(h2 => h2.Id.Equals("dob-hint")).Should().BeGreaterThan(0);
            htmlDocument.DocumentNode.Descendants("span")
                .Count(h2 => h2.Id.Equals("postcode-hint")).Should().BeGreaterThan(0);
            htmlDocument.DocumentNode.Descendants("h2")
                .Count(h2 => h2.InnerText.Contains("Do you want us to contact you?")).Should().Be(0);
        }
    }
}
