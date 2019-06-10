using DFC.Digital.AcceptanceTest.Infrastructure;
using DFC.Digital.AcceptanceTest.Infrastructure.Pages;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechTalk.SpecFlow;

namespace DFC.Digital.AcceptanceTest.AcceptanceCriteria.Steps
{
    [Binding]
    public class ContactUsSteps : BaseStep
    {
        public ContactUsSteps(BrowserStackSelenoHost browserStackSelenoHost, ScenarioContext scenarioContext) : base(browserStackSelenoHost, scenarioContext)
        {
        }

        #region Givens
        [Given(@"I navigate to the Contact us select option page")]
        public void GivenINavigateToTheContactUsSelectOptionPage()
        {
            NavigateToContactUsPage<ContactUsPage>();
        }

        #endregion

        #region Whens
        [When(@"I select the '(.*)' option")]
        public void WhenISelectTheContactOption(string contactOption)
        {
            var contactUs = GetNavigatedPage<ContactUsPage>();
            switch (contactOption.ToLower())
            {
                case "contact-adviser":
                    contactUs.SelectContactAnAdviser();
                    break;
                case "give-feedback":
                    contactUs.SelectGiveFeedback();
                    break;
                case "technical-issue":
                    contactUs.SelectReportATechnicalIssue();
                    break;
            }
        }

        [When(@"I press continue")]
        public void WhenIPressContinue()
        {
            GetNavigatedPage<ContactUsPage>()?.PressContinue<ContactAnAdviserFormsPage>()
                .SaveTo(ScenarioContext);
        }

        [When(@"I press continue on feecback form")]
        public void WhenIPressContinueOnFeecbackForm()
        {
            GetNavigatedPage<ContactUsPage>()?.PressContinue<GiveFeedbackFormPage>()
                 .SaveTo(ScenarioContext);
        }

        [When(@"I press continue on tecnical form")]
        public void WhenIPressContinueOnTecnicalForm()
        {
            GetNavigatedPage<ContactUsPage>()?.PressContinue<TechnicalIssueFormPage>()
                .SaveTo(ScenarioContext);
        }

        [When(@"I press send")]
        public void WhenIPressSend()
        {
            GetNavigatedPage<ContactUsPage>()?.PressSend<ContactUsConfirmationPage>()
                .SaveTo(ScenarioContext);
        }

        [When(@"I complete the first form with (.*) option and (.*) query")]
        public void WhenICompleteTheFirstFormWithOptionAndQuery(string option, string query)
        {
            GetNavigatedPage<ContactAnAdviserFormsPage>()?.CompleteContactAdviserInitialForm(option, query);
        }

        [When(@"I complete the details form with the details (.*), (.*), (.*), (.*), (.*), (.*)")]
        public void WhenICompleteTheDetailsFormWithTheDetails(string firstName, string lastName, string email, string confEmail, string dob, string postcode)
        {
            GetNavigatedPage<ContactAnAdviserFormsPage>()?.CompleteSecondForm(firstName, lastName, email, confEmail, dob, postcode);
        }

        [When(@"I complete the first feedback form with (.*) option and (.*) query")]
        public void WhenICompleteTheFirstFeedbackFormWithFundingOptionAndAutomatedTestQuery(string option, string query)
        {
            GetNavigatedPage<GiveFeedbackFormPage>()?.CompleteFeedbackInitialForm(option, query);
        }

        [When(@"I complete the first technical form with (.*) query")]
        public void WhenICompleteTheFirstFeedbackFormWithQuery(string query)
        {
            GetNavigatedPage<TechnicalIssueFormPage>()?.CompleteTechnicalInitialForm(query);
        }

        [When(@"I complete the give feedback details form with the details (.*), (.*), (.*), (.*), (.*)")]
        public void WhenICompleteTheGiveFeedbackDetailsFormWithTheDetails(string firstName, string lastName, string email, string confEmail, string contact)
        {
            GetNavigatedPage<GiveFeedbackFormPage>()?.CompleteFeedbackSecondForm(firstName, lastName, email, confEmail, contact);
        }

        [When(@"I complete the give technical details form with the details (.*), (.*), (.*), (.*), (.*)")]
        public void WhenICompleteTheGiveTechnicalDetailsFormWithTheDetails(string firstName, string lastName, string email, string confEmail, string contact)
        {
            GetNavigatedPage<TechnicalIssueFormPage>()?.CompleteTechnicalSecondForm(firstName, lastName, email, confEmail, contact);
        }

        #endregion

        #region Thens
        [Then(@"I am redirected to the first '(.*)' contact form")]
        public void ThenIAmRedirectedToTheFirstContactForm(string contactOption)
        {
            switch (contactOption.ToLower())
            {
                case "adviser":
                    var adviserForm = GetNavigatedPage<ContactAnAdviserFormsPage>();
                    adviserForm.ContactQuestionDisplayed.Should().BeTrue();
                    break;
                case "feedback":
                    var feedbackForm = GetNavigatedPage<GiveFeedbackFormPage>();
                    feedbackForm.FeedbackContactOptions.Should().BeTrue();
                    break;
                case "technical":
                    var technicalForm = GetNavigatedPage<TechnicalIssueFormPage>();
                    technicalForm.TechnicalContactOptions.Should().BeTrue();
                    break;
                default:
                    throw new Exception("Contact Form does not exist");
            }
        }

        [Then(@"I am redirected to the second '(.*)' contact form")]
        public void ThenIAmRedirectedToTheSecondContactForm(string contactOption)
        {
            switch (contactOption.ToLower())
            {
                case "adviser":
                    var adviserForm = GetNavigatedPage<ContactAnAdviserFormsPage>();
                    adviserForm.UserDetailsFormDisplayed.Should().BeTrue();
                    break;
                case "feedback":
                    var feedbackForm = GetNavigatedPage<GiveFeedbackFormPage>();
                    feedbackForm.FeedbackFormUserDetails.Should().BeTrue();
                    break;
                case "technical":
                    var technicalForm = GetNavigatedPage<TechnicalIssueFormPage>();
                    technicalForm.TechnicalFormUserDetails.Should().BeTrue();
                    break;
                default:
                    throw new Exception("Contact Form does not exist");
            }
        }

        [Then(@"I am redirected to the confirmation page")]
        public void ThenIAmRedirectedToTheConfirmationPage()
        {
            GetNavigatedPage<ContactUsConfirmationPage>()?.ConfirmationText.Should().Contain("Thank you for contacting us");
        }
        #endregion
    }
}
