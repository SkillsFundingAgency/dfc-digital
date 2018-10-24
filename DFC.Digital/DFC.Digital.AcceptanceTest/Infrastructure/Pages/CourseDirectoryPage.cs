using OpenQA.Selenium;
using TestStack.Seleno.PageObjects;

namespace DFC.Digital.AcceptanceTest.Infrastructure
{
    public class CourseDirectoryPage : Page
    {
        public string Heading => Find.Element(By.ClassName("heading-xlarge"))?.Text.Trim();
    }
}