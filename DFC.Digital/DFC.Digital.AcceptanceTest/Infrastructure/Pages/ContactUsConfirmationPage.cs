using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestStack.Seleno.PageObjects.Locators;

namespace DFC.Digital.AcceptanceTest.Infrastructure.Pages
{
    public class ContactUsConfirmationPage : DFCPage
    {
        public string ConfirmationText => Find.Element(By.ClassName("govuk-heading-xl")).Text;
    }
}
