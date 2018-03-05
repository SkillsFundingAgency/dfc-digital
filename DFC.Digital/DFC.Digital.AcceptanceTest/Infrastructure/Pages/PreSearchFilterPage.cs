using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestStack.Seleno.PageObjects;
using TestStack.Seleno.PageObjects.Locators;

namespace DFC.Digital.AcceptanceTest.Infrastructure.Pages
{
    public class PreSearchFilterPage : Page
    {
        private const string SearchJpTitle = "dfc-code-search-jpTitle";

        public bool FilterResultsTitleDisplayed => Find.Element(By.ClassName("filter-results-heading")) != null;

        public string NoResultsMessage => Find.Element(By.ClassName("filter-results-count")).Text;

        public bool JobProfilesAreShown => Find.Element(By.ClassName("dfc-code-search-resultitem")) != null;

        public string SelectedProfileTitle(int index) => Find.Elements(By.ClassName(SearchJpTitle)).ElementAt(index - 1).Text;

        public bool HasCorrectTitle(string title)
        {
            string pageHeader = Find.Element(By.CssSelector(".active h2")).Text;
            if (pageHeader.Equals(title))
            {
                return true;
            }
            else
            {
                return false;
            }
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
            List<string> individualTags = tagsToSelect.Split(',').ToList<string>();
            var tagOptions = Find.Elements(By.CssSelector(".filter-list label")).ToList();
            var tagsInterestedIn = tagOptions.Where(t => individualTags.Any(tg => t.Text.Contains(tg)));

            foreach (var tag in tagsInterestedIn)
            {
                tag.Click();
                break;
            }
        }

        public bool IsTagsSelected(string selectedTags)
        {
            List<string> individualTags = selectedTags.Split(',').ToList<string>();
            var tagOptions = Find.Elements(By.CssSelector(".filter-list label")).ToList();
            bool selected = true;

            var tagsInterestedIn = tagOptions.Where(t => individualTags.Any(tg => t.Text.Contains(tg)));

            foreach (var tag in tagsInterestedIn)
            {
                var checkboxID = tag.GetAttribute("for");
                var checkboxElement = Find.Element(By.Id(checkboxID));

                bool isSelected = checkboxElement.Selected;

                if (isSelected == false)
                {
                    selected = false;
                }

                break;
            }

            return selected;
        }
    }
}
