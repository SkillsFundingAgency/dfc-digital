using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestStack.Seleno.PageObjects;

namespace DFC.Digital.AcceptanceTest.Infrastructure.Pages
{
    public class YourAccountHomePage : DFCPage
    {
        public string PageTitle => Find.Element(By.ClassName("heading-xlarge")).Text;

        public string PageSectionTitle => Find.Element(By.ClassName("heading-medium")).Text;

        public string SkillsHealthChecksFirstRowFirstColumn => Find.Element(By.XPath("//*[@id='2']/table/tbody/tr/td[1]")).Text;

        public T ClickViewLink<T>()
          where T : UiComponent, new()
        {
            return NavigateTo<T>(By.LinkText("View"));
        }

        public T ClickSignOutLink<T>()
          where T : UiComponent, new()
        {
            return NavigateTo<T>(By.LinkText("Sign out"));
        }

        public T ClickDeleteLink<T>()
          where T : UiComponent, new()
        {
            return NavigateTo<T>(By.LinkText("Delete"));
        }

        public T ClickDeleteButton<T>()
            where T : UiComponent, new()
        {
            return NavigateTo<T>(By.XPath("//input[@value='Delete']"));
        }
    }
}
