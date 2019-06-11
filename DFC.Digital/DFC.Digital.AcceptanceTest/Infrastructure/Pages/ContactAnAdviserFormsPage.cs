using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestStack.Seleno.PageObjects.Locators;

namespace DFC.Digital.AcceptanceTest.Infrastructure.Pages
{
    public class ContactAnAdviserFormsPage : DFCPage
    {
        public bool ContactQuestionDisplayed => Find.Element(By.Id("ContactAdviserQuestionType")) != null;

        public bool UserDetailsFormDisplayed => Find.Element(By.Id("userform")) != null;

        public bool ErrorValidationDisplayed => Find.Element(By.Id("error-validation-summary")) != null;

        public string QuestionTypeErrorMessage => Find.Element(By.Id("ContactAdviserQuestionType-error")).Text;

        public string AdviserQueryErrorMessage => Find.Element(By.Id("Message-error")).Text;

        public string DateOfBirthErrorMessage => Find.Element(By.Id("DateOfBirth-error")).Text;

        public void CompleteContactAdviserInitialForm(string option, string query)
        {
            SelectContactOption(option);
            EnterText("Message", query); //Enters text into field ID 'message' which is the adviser field on the First contact adviser form
        }

        public void CompleteSecondForm(string firstName, string lastName, string email, string confEmail, string dob, string postcode)
        {
            EnterText("Firstname", firstName);
            EnterText("Lastname", lastName);
            EnterText("EmailAddress", email);
            EnterText("ConfirmEmailAddress", confEmail);
            EnterDob(dob);
            EnterText("Postcode", postcode);

            var acceptTAndC = Find.Element(By.Id("AcceptTermsAndConditions"));
            if (!acceptTAndC.Selected)
            {
                acceptTAndC.Click();
            }
        }

        public void SelectContactOption(string option)
        {
            var selectedOption = Find.Elements(By.ClassName("govuk-radios__label")).Where(x => x.Text.Contains(option)).First();
            selectedOption.Click();
        }

        public void EnterText(string id, string query)
        {
            Find.Element(By.Id(id)).Clear();
            Find.Element(By.Id(id)).SendKeys(query);
        }

        public void SelectRecontactOption(string contact)
        {
            if (!string.IsNullOrWhiteSpace(contact))
            {
                if (contact.ToLower().Equals("yes"))
                {
                    Find.Element(By.Id("radio-inline-1")).Click();
                }
                else
                {
                    Find.Element(By.Id("radio-inline-2")).Click();
                }
            }
        }

        public bool CorrectErrorMessagesDisplayed()
        {
            List<string> actualErrorList = Find.Elements(By.CssSelector(".govuk-error-summary__list li a")).Select(x => x.Text).ToList();
            List<string> expectedErrorList = new List<string>()
            {
                "Enter your first name", "Enter your last name", "Enter your email address", "Enter a valid date of birth",
                "Enter your postcode", "You must accept our Terms and Conditions"
            };

            if (actualErrorList.All(expectedErrorList.Contains))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void EnterDob(string dob)
        {
            DateTime birthday = DateTime.ParseExact(dob, "dd/M/yyyy", CultureInfo.InvariantCulture);
            EnterText("DateOfBirthDay", birthday.Day.ToString());
            EnterText("DateOfBirthMonth", birthday.Month.ToString());
            EnterText("DateOfBirthYear", birthday.Year.ToString());
        }
    }
}
