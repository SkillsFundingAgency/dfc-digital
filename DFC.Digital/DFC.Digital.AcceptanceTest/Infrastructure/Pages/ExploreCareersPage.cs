using DFC.Digital.Web.Sitefinity.JobProfileModule.Mvc.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using TestStack.Seleno.PageObjects;
using TestStack.Seleno.PageObjects.Locators;

namespace DFC.Digital.AcceptanceTest.Infrastructure.Pages
{
    public class ExploreCareersPage : DFCPageWithViewModel<JobProfileSearchBoxViewModel>
    {
        public string PageHeader => Find.Element(By.Id("site-header")).Text;

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
