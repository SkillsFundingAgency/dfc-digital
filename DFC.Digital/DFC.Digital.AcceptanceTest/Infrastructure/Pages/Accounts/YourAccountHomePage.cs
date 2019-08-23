using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestStack.Seleno.PageObjects;

namespace DFC.Digital.AcceptanceTest.Infrastructure.Pages
{
    public class YourAccountHomePage : DFCPage
    {
        public string PageTitle => Find.Element(By.ClassName("heading-large")).Text;
    }
}
