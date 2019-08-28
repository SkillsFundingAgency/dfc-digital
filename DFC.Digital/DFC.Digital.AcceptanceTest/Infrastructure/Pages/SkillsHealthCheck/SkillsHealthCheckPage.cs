using OpenQA.Selenium;
using TestStack.Seleno.PageObjects;

namespace DFC.Digital.AcceptanceTest.Infrastructure.Pages
{
    public class SkillsHealthCheckPage : DFCPage
    {
        public string PageTitle => Find.Element(By.ClassName("heading-xlarge")).Text;

        public string ActionButtonText => Find.Element(By.Name("answerAction")).GetAttribute("value");

        public void AnswerQuestion(int answerValue)
        {
            Find.Element(By.XPath($"//*[@id='skillsForm']/div[1]/fieldset/label[{answerValue}]/input")).Click();
            Find.Element(By.Name("answerAction")).Click();
        }

        public T FinishSkillsHealthCheck<T>(int answerValue)
           where T : UiComponent, new()
        {
            Find.Element(By.XPath($"//*[@id='skillsForm']/div[1]/fieldset/label[{answerValue}]/input")).Click();
            return NavigateTo<T>(By.Name("answerAction"));
        }
    }
}
