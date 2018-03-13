﻿using OpenQA.Selenium;
using TestStack.Seleno.PageObjects;

namespace DFC.Digital.AcceptanceTest.Infrastructure
{
    public class BauJpLandingPage : Page
    {
        public bool HasAtoZIndex => Find.Element(By.Id("az-index-wrapper")) != null;
    }
}