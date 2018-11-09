using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestStack.Seleno.PageObjects;
using TestStack.Seleno.PageObjects.Locators;

namespace DFC.Digital.AcceptanceTest.Infrastructure.Pages
{
    public class HealthStatusPage : Page
    {
        public string ServiceTitle => Find.OptionalElement(By.ClassName("heading-medium")).Text;

        public bool ListOfServices => Find.OptionalElement(By.ClassName("list-service")) != null;
    }
}
