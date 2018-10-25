using OpenQA.Selenium;
using System;
using System.Linq;
using System.Threading;
using TestStack.Seleno.PageObjects;
using TestStack.Seleno.PageObjects.Actions;

namespace DFC.Digital.AcceptanceTest.Infrastructure
{
    public class SitefinityPage<T> : Page<T>
        where T : class, new()
    {
        #region Page Elements
        #region private const for survey

        private const string OpenSurvey = "survey_open";
        private const string EmailField = "survey-emailaddress";
        private const string SendButton = "survey_button";

        #endregion private const for survey

        public bool IsSurveyBannerDisplayed => Find.OptionalElement(By.ClassName("heading-small")).Displayed;

        public string PageHeading => Find.Element(By.TagName("h1")).Text;

        #endregion Page Elements

        #region Survey Properties / Methods

        public bool SuccessEmailMessageDisplayed => Find.OptionalElement(By.ClassName("js-survey-complete")) != null;

        internal string BreadcrumbText => Find.Elements(By.ClassName("breadcrumbs")).Last().Text;

        protected new IElementFinder Find => new DfcElementFinder(base.Find, Execute);

        public TPage ClickTakeSurvey<TPage>()
            where TPage : UiComponent, new()
        {
            return Navigate.To<TPage>(By.ClassName(OpenSurvey));
        }

        public TPage SelectOnlineSurvey<TPage>()
            where TPage : UiComponent, new()
        {
            return Navigate.To<TPage>(By.ClassName("survey_link"));
        }

        public void EnterEmail(string email) => Find.Element(By.Id(EmailField)).SendKeys(email);

        public TPage SubmitEmail<TPage>(string email)
            where TPage : UiComponent, new()
        {
            EnterEmail(email);
            var navPage = Navigate.To<TPage>(By.ClassName(SendButton));
            WaitFor.AjaxCallsToComplete(new TimeSpan(0, 0, 2));
            return navPage;
        }

        #endregion Survey Properties / Methods
        public void AwaitInitialisation()
        {
            WaitFor.AjaxCallsToComplete(new TimeSpan(0, 0, 2));
            while (IsThisStatusPage())
            {
                Thread.Sleep(TimeSpan.FromSeconds(5));
            }
        }

        public void CloseSurvey()
        {
            var close = Find.Element(By.ClassName("survey_close"));
            close.Click();
        }

        public bool UrlContains(string urlFragment)
        {
            return Browser.Url.ToUpperInvariant().Contains(urlFragment?.ToUpperInvariant());
        }

        public TPage ClickExploreCareerBreadcrumb<TPage>()
            where TPage : UiComponent, new()
        {
            return Navigate.To<TPage>(By.PartialLinkText("Explore careers home"));
        }

        private bool IsThisStatusPage()
        {
            try
            {
                return Browser.Url.Contains("/health/status");
            }
            catch (ArgumentNullException)
            {
                return false;
            }
        }
    }
}