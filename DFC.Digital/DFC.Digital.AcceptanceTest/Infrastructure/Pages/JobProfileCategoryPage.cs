using DFC.Digital.Web.Sitefinity.JobProfileModule.Mvc.Models;
using OpenQA.Selenium;
using System.Linq;
using TestStack.Seleno.PageObjects;

namespace DFC.Digital.AcceptanceTest.Infrastructure
{
    public class JobProfileCategoryPage : SitefinityPage<JobProfileByCategoryViewModel>
    {
        public string CategoryTitle => Find.OptionalElement(By.ClassName("heading-xlarge")).Text;

        public bool HasJobProfiles => Find.OptionalElement(By.Id("MainContent_T447555DF001_Col00")) != null;

        public bool HasOtherJobCategoriesSection => Find.OptionalElement(By.Id("MainContent_T447555DF001_Col01")) != null;

        public bool HasSignpostBanner => Find.OptionalElement(By.ClassName("signpost")) != null;

        public string GetJobProfileByIndex(int category)
        {
            return Find.Elements(By.ClassName("dfc-code-search-jpTitle"))?.ElementAt(category - 1).GetAttribute("innerText");
        }

        public bool IsCategoryDisplayedInOtherCategorySection(string category)
        {
            return Find.Element(By.CssSelector(".font-xsmall a")).GetAttribute("innerText").Contains(category);
        }

        public T GoToResult<T>(int index)
            where T : UiComponent, new()
        {
            var results = Find.Elements(By.ClassName("dfc-code-search-jpTitle")).ToList();
            return NavigateTo<T>(results[index - 1].GetAttribute("href"), By.ClassName("dfc-code-search-jpTitle"));
        }

        public T GoToCategory<T>(string category)
            where T : UiComponent, new()
        {
            return NavigateTo<T>(By.PartialLinkText(category));
        }

        public T ClickCategorySignpostBanner<T>()
            where T : UiComponent, new()
        {
            return NavigateTo<T>(By.ClassName("signpost"));
        }

        internal bool ContainsUrlName(string urlname) => UrlContains(urlname);
    }
}