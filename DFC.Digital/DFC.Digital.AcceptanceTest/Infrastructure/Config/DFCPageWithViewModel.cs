using DFC.Digital.AcceptanceTest.Infrastructure.Pages;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Linq;
using System.Threading;
using TestStack.Seleno.PageObjects;
using TestStack.Seleno.PageObjects.Actions;

namespace DFC.Digital.AcceptanceTest.Infrastructure
{
    public class DFCPageWithViewModel<TViewModel> : Page<TViewModel>
        where TViewModel : class, new()
    {
        #region Page Elements

        #region private const for survey

        private const string OpenSurvey = "survey_open";
        private const string EmailField = "survey-emailaddress";
        private const string SendButton = "survey_button";

        #endregion private const for survey

        public string PageHeading => Find.Element(By.TagName("h1")).Text;

        #endregion Page Elements

        #region Survey Properties / Methods

        public bool SuccessEmailMessageDisplayed => Find.OptionalElement(By.ClassName("js-survey-complete")) != null;

        internal string BreadcrumbText => Find.Elements(By.ClassName("breadcrumbs")).Last().Text;

        protected new IElementFinder Find => new DfcElementFinder(base.Find, Execute, WaitFor, Browser);

        public TPage ClickFeedbackLink<TPage>()
            where TPage : UiComponent, new()
        {
            return NavigateTo<TPage>(By.CssSelector(".phase-banner p span a"));
        }

        public TPage SelectOnlineSurvey<TPage>()
            where TPage : UiComponent, new()
        {
            return NavigateTo<TPage>(By.ClassName("survey_link"));
        }

        public void EnterEmail(string email) => Find.Element(By.Id(EmailField)).SendKeys(email); //had to use send keys to simulate typing.

        public TPage SubmitEmail<TPage>(string email)
            where TPage : UiComponent, new()
        {
            EnterEmail(email);
            var navPage = NavigateTo<TPage>(By.ClassName(SendButton));
            WaitFor.AjaxCallsToComplete(new TimeSpan(0, 0, 2));
            return navPage;
        }

        #endregion Survey Properties / Methods

        public void AwaitInitialisation()
        {
            WaitFor.AjaxCallsToComplete(new TimeSpan(0, 0, 2));
            while (Browser.IsThisStatusPage())
            {
                Thread.Sleep(TimeSpan.FromSeconds(5));
            }
        }

        public bool UrlContains(string urlFragment)
        {
            return Browser.Url.ToUpperInvariant().Contains(urlFragment?.ToUpperInvariant());
        }

        public TPage ClickExploreCareerBreadcrumb<TPage>()
            where TPage : UiComponent, new()
        {
            return NavigateTo<TPage>(By.PartialLinkText("Home"));
        }

        protected virtual TPage NavigateTo<TPage>(By by, int waitTimeout = 10)
            where TPage : UiComponent, new()
        {
            return Find.NavigateAndWaitForStalenessTo<TPage>(Navigate, Browser, by, waitTimeout);
        }

        protected virtual TPage NavigateTo<TPage>(string url, By checkUsing, int waitTimeout = 10)
    where TPage : UiComponent, new()
        {
            return Find.NavigateAndWaitForStalenessTo<TPage>(Navigate, Browser, url, checkUsing, waitTimeout);
        }
    }
}