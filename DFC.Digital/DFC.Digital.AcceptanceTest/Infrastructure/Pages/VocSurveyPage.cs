using OpenQA.Selenium;
using TestStack.Seleno.PageObjects;

namespace DFC.Digital.AcceptanceTest.Infrastructure
{
    public class VocSurveyPage : DFCPage
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

        public bool IsJPSurveyQuestionDisplayed() => Find.Element(By.Id("ss-question-title-7271726")) != null;
    }
}