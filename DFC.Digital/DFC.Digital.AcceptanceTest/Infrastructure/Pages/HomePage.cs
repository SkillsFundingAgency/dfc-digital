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

        public bool HasErrorMessage => Find.OptionalElement(By.ClassName("error")) != null;

        public bool HasJobProfileCategoriesSection => Find.OptionalElement(By.ClassName("homepage-jobcategories")) != null;

        internal bool HasSearchWidget => Find.OptionalElement(By.Id("header-search")) != null;

        internal bool HasSuggestedSearch => Find.OptionalElement(By.ClassName("ui-menu-item-wrapper")) != null;

        internal bool UrlShowsSearchAction => UrlContains("/home/search/");

        public Uri GetJobProfileCategoryUrl(int category)
        {
            return new Uri(Find.Elements(By.CssSelector(".homepage-jobcategories a"))?.ElementAt(category - 1).GetAttribute("href"));
        }

        public T Search<T>(JobProfileSearchBoxViewModel model)
            where T : UiComponent, new()
        {
            Input.Model(model);
            return NavigateTo<T>(By.ClassName("submit"));
        }

        public T Search<T>()
            where T : UiComponent, new()
        {
            return NavigateTo<T>(By.ClassName("submit"));
        }

        public T GoToResult<T>(int index)
            where T : UiComponent, new()
        {
            var findBy = By.CssSelector(".homepage-jobcategories a");
            var results = Find.Elements(findBy).ToList();
            return NavigateTo<T>(results[index - 1].GetAttribute("href"), findBy);
        }

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

        public void SelectSuggestedSearch(int index)
        {
            var list = Find.Elements(By.ClassName("ui-menu-item-wrapper")).ToList();
            list.ElementAt(index - 1).Click();
        }

        internal string GetSuggestedSearchText(int index) => Find.Elements(By.ClassName("ui-menu-item-wrapper")).ElementAt(index - 1).Text;

        internal void EnterSearchText(string text) => Find.OptionalElement(By.Id("SearchTerm")).SendKeys(text); //had to use send keys to simulate typing.

        internal bool HasExploreCategoryText(string expectedText) => Find.Element(By.ClassName("heading-medium"))
            .Text.Equals(expectedText, StringComparison.InvariantCultureIgnoreCase);
    }
}