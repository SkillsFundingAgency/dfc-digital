using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestStack.Seleno.PageObjects.Locators;

namespace DFC.Digital.AcceptanceTest.Infrastructure.Pages
{
    public class GiveFeedbackFormPage : ContactAnAdviserFormsPage
    {
        public bool FeedbackContactOptions => Find.Element(By.Id("FeedbackQuestionType")) != null;

        public bool FeedbackFormUserDetails => Find.Element(By.Id("userform")) != null;

        public void CompleteFeedbackInitialForm(string option, string query)
        {
            SelectContactOption(option);
            EnterText("Feedback", query); //Enters text into field ID 'message' which is the adviser field on the First contact adviser form
        }

        public void CompleteFeedbackSecondForm(string firstName, string lastName, string email, string confEmail, string contact)
        {
            EnterText("Firstname", firstName);
            EnterText("Lastname", lastName);
            EnterText("EmailAddress", email);
            EnterText("ConfirmEmailAddress", confEmail);
            SelectRecontactOption(contact);
            Find.Element(By.Id("AcceptTermsAndConditions")).Click();
        }
    }
}
