using OpenQA.Selenium;
using TestStack.Seleno.PageObjects;
using TestStack.Seleno.PageObjects.Actions;

namespace DFC.Digital.AcceptanceTest.Infrastructure
{
    public class DFCPage : Page
    {
        protected new IElementFinder Find => new DfcElementFinder(base.Find, Execute, WaitFor, Browser);

        protected virtual TPage NavigateTo<TPage>(By by, int waitTimeout = 10)
    where TPage : UiComponent, new()
        {
            return Find.NavigateAndWaitForStalenessTo<TPage>(Navigate, Browser, by, waitTimeout);
        }

        protected virtual TPage NavigateTo<TPage>(string url, By checkUsing, int waitTimeout = 10)
    where TPage : UiComponent, new()
        {
            return Find.NavigateAndWaitForStalenessTo<TPage>(Navigate, Browser, url, checkUsing, waitTimeout);
        }
    }
}