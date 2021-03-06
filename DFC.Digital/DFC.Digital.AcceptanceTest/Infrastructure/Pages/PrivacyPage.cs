﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestStack.Seleno.PageObjects;
using TestStack.Seleno.PageObjects.Locators;

namespace DFC.Digital.AcceptanceTest.Infrastructure.Pages
{
    public class PrivacyPage : DFCPage
    {
        public string PageTitle => Find.OptionalElement(By.ClassName("govuk-heading-xl")).Text;
    }
}
