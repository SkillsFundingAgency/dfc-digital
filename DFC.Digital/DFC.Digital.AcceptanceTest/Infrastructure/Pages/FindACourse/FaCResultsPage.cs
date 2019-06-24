using OpenQA.Selenium;
using System.Collections.Generic;
using System.Linq;
using TestStack.Seleno.PageObjects;

namespace DFC.Digital.AcceptanceTest.Infrastructure.Pages
{
    public class FaCResultsPage : FaCLandingPage
    {
        public string KeywordSearchText => Find.Element(By.ClassName("govuk-visually-hidden")).Text;

        public string LocationText => Find.Element(By.Id("Location")).GetAttribute("value");

        public string ProviderText => Find.Element(By.Id("Provider")).GetAttribute("value");

        public bool Show16To19CheckBoxSelected => Find.Element(By.Id("Only1619Courses")).Selected;

        public string NoResultsText => Find.Element(By.CssSelector(".govuk-body-s .govuk-body")).Text;

        public int NumberOfCoursesDisplayed => NumberOfCourseResults.Count();

        private List<IWebElement> NumberOfCourseResults => Find.Elements(By.CssSelector(".govuk-heading-m .govuk-link")).ToList();

        private List<IWebElement> FiltersList => Find.Elements(By.ClassName("govuk-radios__input")).ToList();

        public string SelectedCourseText(int courseNo)
        {
            return NumberOfCourseResults[courseNo - 1].Text;
        }

        public T SelectCourse<T>(int courseNo)
            where T : UiComponent, new()
        {
            var findBy = By.ClassName("govuk-heading-m");
            return NavigateTo<T>(NumberOfCourseResults[courseNo - 1].GetAttribute("href"), findBy);
        }

        public void UpdateFilters(string provider, string location)
        {
            EnterProviderName(provider);
            EnterLocation(location);
        }

        public T ApplyFiltersButton<T>()
            where T : UiComponent, new()
        {
            return NavigateTo<T>(By.ClassName("govuk-button"));
        }

        public void SelectCourseFilter(string filter)
        {
            if (!string.IsNullOrWhiteSpace(filter))
            {
                var filteredText = filter.Replace(" ", string.Empty).ToUpper();
                foreach (var button in FiltersList)
                {
                    var buttonText = button.GetAttribute("value").Replace(" ", string.Empty).ToUpper();
                    if (buttonText.Contains(filteredText))
                    {
                        button.Click();
                    }
                }
            }
        }

        public bool IsCourseFilterSelected(string filter)
        {
            var selected = false;

            if (!string.IsNullOrWhiteSpace(filter))
            {
                var filteredText = filter.Replace(" ", string.Empty).ToUpper();
                foreach (var button in FiltersList)
                {
                    var buttonText = button.GetAttribute("value").Replace(" ", string.Empty).ToUpper();
                    if (buttonText.Contains(filteredText))
                    {
                        if (button.Selected == true)
                        {
                            selected = true;
                        }
                        else
                        {
                            selected = false;
                        }
                    }
                }
            }

            return selected;
        }
    }
}
