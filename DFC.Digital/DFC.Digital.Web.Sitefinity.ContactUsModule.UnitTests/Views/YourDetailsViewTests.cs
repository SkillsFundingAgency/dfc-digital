using ASP;
using DFC.Digital.Web.Sitefinity.ContactUsModule.Mvc.Models;
using FluentAssertions;
using HtmlAgilityPack;
using RazorGenerator.Testing;
using System;
using System.Linq;
using Xunit;

namespace DFC.Digital.Web.Sitefinity.ContactUsModule.UnitTests
{
    public class YourDetailsViewTests
    {
        [Fact]
        public void Dfc7630YourDetailsContactAdviserViewTests()
        {
            // Arrange
            var yourDetailsIndex = new _MVC_Views_YourDetails_ContactAdvisor_cshtml();
            var contactUsViewModel = new ContactUsWithDobPostcodeViewModel
            {
                PageIntroduction = nameof(ContactUsWithDobPostcodeViewModel.PageIntroduction),
                PageIntroductionTwo = nameof(ContactUsWithDobPostcodeViewModel.PageIntroduction),
                TermsAndConditionsText = nameof(ContactUsWithDobPostcodeViewModel.TermsAndConditionsText),
                PostcodeHint = nameof(ContactUsWithDobPostcodeViewModel.PostcodeHint),
                DateOfBirthHint = nameof(ContactUsWithDobPostcodeViewModel.DateOfBirthHint),
                PageTitle = nameof(ContactUsWithDobPostcodeViewModel.PageTitle)
            };

            // Act
            var htmlDocument = yourDetailsIndex.RenderAsHtml(contactUsViewModel);

            // Assert
            AssertTagInnerTextValue(htmlDocument, contactUsViewModel.PageTitle, "h1");
            AssertTagInnerTextValue(htmlDocument, contactUsViewModel.PageIntroduction, "p");
            AssertTagInnerTextValue(htmlDocument, contactUsViewModel.PageIntroductionTwo, "p");
            AssertTagInnerTextValue(htmlDocument, contactUsViewModel.DateOfBirthHint, "span");
            AssertTagInnerTextValue(htmlDocument, contactUsViewModel.PostcodeHint, "span");
            AssertTagInnerTextValue(htmlDocument, contactUsViewModel.TermsAndConditionsText, "h3");
            AssertFormGroupsCounts(htmlDocument, 11);
        }

        [Fact]
        public void Dfc7630YourDetailsFeedbackViewTests()
        {
            // Arrange
            var yourDetailsIndex = new _MVC_Views_YourDetails_Feedback_cshtml();
            var contactUsViewModel = new ContactUsWithConsentViewModel
            {
                PageIntroduction = nameof(ContactUsWithConsentViewModel.PageIntroduction),
                DoYouWantUsToContactUsText = nameof(ContactUsWithConsentViewModel.DoYouWantUsToContactUsText),
                TermsAndConditionsText = nameof(ContactUsWithConsentViewModel.TermsAndConditionsText),
                PageTitle = nameof(ContactUsWithConsentViewModel.PageTitle)
            };

            // Act
            var htmlDocument = yourDetailsIndex.RenderAsHtml(contactUsViewModel);

            // Assert
            AssertTagInnerTextValue(htmlDocument, contactUsViewModel.PageTitle, "h1");
            AssertTagInnerTextValue(htmlDocument, contactUsViewModel.PageIntroduction, "p");
            AssertTagInnerTextValue(htmlDocument, contactUsViewModel.DoYouWantUsToContactUsText, "h2");
            AssertTagInnerTextValue(htmlDocument, contactUsViewModel.TermsAndConditionsText, "h3");
            AssertFormGroupsCounts(htmlDocument, 7);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void Dfc7630ErrorSummaryViewTests(bool modelStateInvalid)
        {
            // Arrange
            var errorSummaryView = new _MVC_Views_Shared_ErrorSummary_cshtml();

            if (modelStateInvalid)
            {
                errorSummaryView.ViewData.ModelState.AddModelError(nameof(DateOfBirthPostcodeDetails.Firstname), nameof(Exception.Message));
            }

            // Act
            var htmlDocument = errorSummaryView.RenderAsHtml();

            // Assert
            if (modelStateInvalid)
            {
                AssertErrorDetailInSummary(htmlDocument, nameof(Exception.Message));
            }
            else
            {
                AssertSummaryListViewIsEmpty(htmlDocument);
            }
        }

        private static void AssertSummaryListViewIsEmpty(HtmlDocument htmlDocument)
        {
            htmlDocument.DocumentNode.Descendants("li").Count().Should().Be(0);
        }

        private void AssertErrorDetailInSummary(HtmlDocument htmlDocument, string errorMessage)
        {
            htmlDocument.DocumentNode.Descendants("h2")
                .Count(h2 => h2.InnerText.Contains("There is a problem")).Should().BeGreaterThan(0);
            htmlDocument.DocumentNode.Descendants("a")
                .Count(a => a.InnerText.Contains(errorMessage)).Should().BeGreaterThan(0);
        }

        private void AssertTagInnerTextValue(HtmlDocument htmlDocument, string innerText, string tag)
        {
            htmlDocument.DocumentNode.Descendants(tag)
                .Any(h2 => h2.InnerText.Contains(innerText)).Should().BeTrue();
        }

        private void AssertFormGroupsCounts(HtmlDocument htmlDocument, int count)
        {
            htmlDocument.DocumentNode.Descendants("div")
                .Count(div => div.Attributes["class"].Value.Contains("govuk-form-group")).Should().Be(count);
        }
    }
}
