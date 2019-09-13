using DFC.Digital.Web.Sitefinity.JobProfileModule.Mvc.Models;
using System;
using System.Linq;
using TestStack.Seleno.PageObjects;
using TestStack.Seleno.PageObjects.Locators;

namespace DFC.Digital.AcceptanceTest.Infrastructure
{
    public class Homepage : DFCPageWithViewModel<JobProfileSearchBoxViewModel>
    {
        public bool ServiceName => Find.OptionalElement(By.Id("site-header")) != null;

        public T ClickCookiesLink<T>()
            where T : UiComponent, new()
        {
            return NavigateTo<T>(By.CssSelector(".footer-meta-inner a"));
        }

        public T GoToSurvey<T>()
            where T : UiComponent, new()
        {
            return NavigateTo<T>(By.ClassName("survey_action"));
        }

        public T ClickPSFContinueButton<T>()
            where T : UiComponent, new()
        {
            return NavigateTo<T>(By.ClassName("filter-home-button"));
        }

        public T ClickPrivacyLink<T>()
            where T : UiComponent, new()
        {
            return NavigateTo<T>(By.PartialLinkText("Privacy and cookies"));
        }

        public T ClickTermAndCondLink<T>()
            where T : UiComponent, new()
        {
            return NavigateTo<T>(By.PartialLinkText("Terms and conditions"));
        }

        public T ClickInformationSourcesLink<T>()
            where T : UiComponent, new()
        {
            return NavigateTo<T>(By.PartialLinkText("Information sources"));
        }

        public T ClickHelpLink<T>()
            where T : UiComponent, new()
        {
            return NavigateTo<T>(By.LinkText("Help"));
        }

        public T ClickFindACourseLink<T>()
             where T : UiComponent, new()
        {
            return NavigateTo<T>(By.Id("nav-FC"));
        }
    }
}