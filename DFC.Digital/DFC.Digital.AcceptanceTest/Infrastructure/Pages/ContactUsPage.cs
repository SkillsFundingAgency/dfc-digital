using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestStack.Seleno.PageObjects;

namespace DFC.Digital.AcceptanceTest.Infrastructure.Pages
{
    public class ContactUsPage : DFCPage
    {
        public void SelectContactAnAdviser()
        {
            Find.Element(By.Id("ContactOptionType_ContactAdviser")).Click();
        }

        public void SelectGiveFeedback()
        {
            Find.Element(By.Id("ContactOptionType_Feedback")).Click();
        }

        public void SelectReportATechnicalIssue()
        {
            Find.Element(By.Id("ContactOptionType_Technical")).Click();
        }

        public T PressContinue<T>()
            where T : UiComponent, new()
        {
            return NavigateTo<T>(By.ClassName("govuk-button"));
        }

        public T PressSend<T>()
             where T : UiComponent, new()
        {
            return NavigateTo<T>(By.ClassName("govuk-button"));
        }
    }
}
