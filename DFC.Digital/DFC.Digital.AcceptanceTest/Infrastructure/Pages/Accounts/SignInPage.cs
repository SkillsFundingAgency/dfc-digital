using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestStack.Seleno.PageObjects;

namespace DFC.Digital.AcceptanceTest.Infrastructure.Pages
{
    public class SignInPage : DFCPage
    {
        public string PageTitle => Find.Element(By.ClassName("heading-large")).Text;

        public T Login<T>(string userName, string password)
         where T : UiComponent, new()
        {
            EnterText("email", userName);
            EnterText("password", password);
            return NavigateTo<T>(By.Id("signin_b01"));
        }
    }
}
