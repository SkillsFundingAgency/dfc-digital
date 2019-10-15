using OpenQA.Selenium;
using TestStack.Seleno.PageObjects;

namespace DFC.Digital.AcceptanceTest.Infrastructure
{
    public class VocSurveyPage : DFCPage
    {
        public bool IsSurveyDisplayed()
        {
            Browser.SwitchTo().Window(Browser.WindowHandles[1]);

            return Find.Element(By.ClassName("mrQuestionText")) != null;
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

        public void CloseSurveyWindow()
        {
            Browser.Close();
            Browser.SwitchTo().Window(Browser.WindowHandles[0]);
        }

        public bool IsJPSurveyQuestionDisplayed() => Find.Element(By.ClassName("ss-question-title")) != null;
    }
}