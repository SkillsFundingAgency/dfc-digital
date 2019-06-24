using OpenQA.Selenium;
using TestStack.Seleno.PageObjects;
using TestStack.Seleno.PageObjects.Actions;

namespace DFC.Digital.AcceptanceTest.Infrastructure
{
    public class DFCPage : Page
    {
        protected new IElementFinder Find => new DfcElementFinder(base.Find, Execute, WaitFor, Browser);

        public void EnterText(string id, string query)
        {
            Find.Element(By.Id(id)).Clear();

            //NOTE - Below.SendKeys method will be the new way of interacting with pages / forms / viws rather than using the ViewModels. This is in
            //preparation for moving away from the current style framework and adopting a framework to use pre-defined Selenium Methods
            Find.Element(By.Id(id)).SendKeys(query);
        }

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