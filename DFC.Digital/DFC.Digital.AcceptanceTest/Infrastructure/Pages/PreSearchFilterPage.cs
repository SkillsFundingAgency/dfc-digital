using System;
using System.Collections.Generic;
using System.Linq;
using TestStack.Seleno.PageObjects;
using TestStack.Seleno.PageObjects.Locators;

namespace DFC.Digital.AcceptanceTest.Infrastructure
{
    public class PreSearchFilterPage : Page
    {
        private const string SearchJpTitle = "dfc-code-search-jpTitle";

        public bool FilterResultsTitleDisplayed => Find.Element(By.ClassName("filter-results-heading")) != null;

        public string NoResultsMessage => Find.Element(By.ClassName("filter-results-count")).Text;

        public bool JobProfilesAreShown => Find.Element(By.ClassName("dfc-code-search-resultitem")) != null;

        public IEnumerable<OpenQA.Selenium.IWebElement> TagOptions => Find.Elements(By.CssSelector(".filter-list label")).ToList();

        public string SelectedProfileTitle(int index) => Find.Elements(By.ClassName(SearchJpTitle)).ElementAt(index - 1).Text;

        public bool HasCorrectTitle(string title)
        {
            string pageHeader = Find.Element(By.CssSelector(".active h2")).Text;
            return pageHeader.Equals(title, StringComparison.InvariantCultureIgnoreCase);
        }

        public T GoToResult<T>(int index)
             where T : UiComponent, new()
        {
            var results = Find.Elements(By.ClassName(SearchJpTitle)).ToList();
            return Navigate.To<T>(results[index - 1].GetAttribute("href"));
        }

        public T ClickContinue<T>()
            where T : UiComponent, new()
        {
            return Navigate.To<T>(By.Id("filter-continue"));
        }

        public T ClickBack<T>()
            where T : UiComponent, new()
        {
            return Navigate.To<T>(By.ClassName("button-link"));
        }

        public void SelectTags(string tagsToSelect)
        {
            var individualTags = tagsToSelect?.Split(',');
            var tagsInterestedIn = TagOptions.Where(t => individualTags.Any(tg => t.Text.Contains(tg)));

            foreach (var tag in tagsInterestedIn)
            {
                tag.Click();
            }
        }

        public bool IsTagsSelected(string selectedTags)
        {
            var individualTags = selectedTags?.Split(',');
            var hasAnyUnSelectedTags = TagOptions.Where(t => individualTags.Any(tg => t.Text.Contains(tg)))
                .Any(t =>
                {
                    var input = Find.Element(By.Id(t.GetAttribute("for")));
                    return input.Selected == false;
                });

            return !hasAnyUnSelectedTags;
        }
    }
}
