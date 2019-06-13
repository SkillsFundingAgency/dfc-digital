using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestStack.Seleno.PageObjects.Locators;

namespace DFC.Digital.AcceptanceTest.Infrastructure.Pages
{
    public class TechnicalIssueFormPage : ContactAnAdviserFormsPage
    {
        public bool TechnicalContactOptions => Find.Element(By.Id("Message")) != null;

        public bool TechnicalFormUserDetails => Find.Element(By.Id("userform")) != null;

        public void CompleteTechnicalInitialForm(string query)
        {
            EnterText("Message", query); //Enters text into field ID 'message' which is the adviser field on the First contact adviser form
        }

        public void CompleteTechnicalSecondForm(string firstName, string lastName, string email, string confEmail, string contact)
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
