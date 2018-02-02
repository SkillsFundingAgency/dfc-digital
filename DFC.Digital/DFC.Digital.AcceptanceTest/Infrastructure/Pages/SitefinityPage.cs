using OpenQA.Selenium;
using System;
using System.Linq;
using System.Threading;
using TestStack.Seleno.PageObjects;
using TestStack.Seleno.PageObjects.Actions;

namespace DFC.Digital.AcceptanceTest.Infrastructure.Pages
{
    public class SitefinityPage<T> : Page<T>
        where T : class, new()
    {
        #region Page Elements
        #region private const for survey
        private const string OpenSurvey = "survey_open";
        private const string EmailField = "EmailAddress";
        private const string SendButton = "survey_button";

        #endregion private const for survey

        public bool IsSurveyBannerDisplayed => Find.OptionalElement(By.ClassName("heading-small")).Displayed;

        public string PageHeading => Find.Element(By.TagName("h1")).Text;

        #endregion Page Elements

        #region Survey Properties / Methods

        public bool SuccessEmailMessageDisplayed => Find.OptionalElement(By.ClassName("js-survey-complete")) != null;

        internal string BreadcrumbText => Find.Elements(By.ClassName("breadcrumbs")).Last().Text;

        protected new IElementFinder Find => new DfcElementFinder(base.Find, Execute);

        public void ClickTakeSurvey() => Find.Element(By.ClassName(OpenSurvey)).Click();

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
            return Navigate.To<TPage>(By.ClassName(SendButton));
        }

        #endregion Survey Properties / Methods
        public void AwaitInitialisation()
        {
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
            return Browser.Url.ToLowerInvariant().Contains(urlFragment.ToLowerInvariant());
        }

        public TPage ClickFindACareerBreadcrumb<TPage>()
            where TPage : UiComponent, new()
        {
            return Navigate.To<TPage>(By.PartialLinkText("Find a career home"));
        }

        private bool IsThisStatusPage()
        {
            try
            {
                return Browser.Url.Contains("/sitefinity/status");
            }
            catch
            {
                return false;
            }
        }
    }
}