using OpenQA.Selenium;
using TestStack.Seleno.PageObjects;

namespace DFC.Digital.AcceptanceTest.Infrastructure
{
    public class VocSurveyPage : Page
    {
        public bool IsSurveyDisplayed()
        {
            Browser.SwitchTo().Frame(0);

            return Find.Element(By.ClassName("ss-question-title")) != null;
        }

        public void StartSurvey()
        {
            var openSurvey = Find.Element(By.ClassName("survey_action"));
            openSurvey.Click();
        }

        public void CloseSurveyBanner()
        {
            var close = Find.Element(By.ClassName("survey_close"));
            close.Click();
        }
    }
}