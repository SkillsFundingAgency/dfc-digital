using OpenQA.Selenium;
using TestStack.Seleno.PageObjects;

namespace DFC.Digital.AcceptanceTest.Infrastructure
{
    public class CourseDirectoryPage : DFCPage
    {
        public string Heading => Find.Element(By.ClassName("heading-xlarge"))?.Text;
    }
}