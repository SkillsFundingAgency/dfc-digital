using OpenQA.Selenium;
using TestStack.Seleno.PageObjects;

namespace DFC.Digital.AcceptanceTest.Infrastructure.Pages
{
    public class FaCLandingPage : DFCPage
    {
        public string PageTitle => Find.Element(By.ClassName("govuk-heading-xl")).Text;

        public IWebElement CourseNameField => Find.Element(By.Id("SearchTerm"));

        public bool Course16To19CheckBoxSelected => Find.Element(By.Id("Only1619Courses")).Selected;

        public T SearchCourses<T>()
            where T : UiComponent, new()
        {
            return NavigateTo<T>(By.ClassName("govuk-button"));
        }

        public void EnterCourseName(string courseName)
        {
            if (!string.IsNullOrWhiteSpace(courseName))
            {
                EnterText("SearchTerm", courseName);
            }
        }

        public void EnterProviderName(string provider)
        {
            if (!string.IsNullOrWhiteSpace(provider))
            {
                EnterText("Provider", provider);
            }
        }

        public void EnterLocation(string location)
        {
            if (!string.IsNullOrWhiteSpace(location))
            {
                EnterText("Location", location);
            }
        }

        public void Select1619CheckBox(string yesOrNo)
        {
            if (!string.IsNullOrWhiteSpace(yesOrNo))
            {
                if (yesOrNo.Equals("yes") && Course16To19CheckBoxSelected == false)
                {
                    Find.Element(By.Id("Only1619Courses")).Click();
                }
            }
        }

        public T ApplyFilters<T>(string courseName, string provider, string location, string show16to19)
            where T : UiComponent, new()
        {
            EnterCourseName(courseName);
            EnterProviderName(provider);
            EnterLocation(location);
            Select1619CheckBox(show16to19);
            return NavigateTo<T>(By.ClassName("govuk-button"));
        }
    }
}
