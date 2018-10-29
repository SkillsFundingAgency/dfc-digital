using DFC.Digital.SitefinityTest.HelperExtenstions;
using DFC.Digital.SitefinityTest.Utilities;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DFC.Digital.SitefinityTest.Pages
{
    public class GenericPage : BasePage
    {
        public GenericPage(IWebDriver webDriver) : base(webDriver)
        {

        }

        public By ProfileHeader => By.ClassName("sfUserHeader");
        public By LogOutButton => By.LinkText("Logout");

        public ExploreCareerPage LogOut()
        {
            PageHelper.ClickElement(ProfileHeader);
            PageHelper.ClickElement(LogOutButton);
            return new ExploreCareerPage(_driver);
        }
    }
}
