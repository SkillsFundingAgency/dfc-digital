using DFC.Digital.Core;
using DFC.Digital.Web.Sitefinity.ContactUsModule.Mvc.Controllers;
using FakeItEasy;
using Xunit;

namespace DFC.Digital.Web.Sitefinity.ContactUsModule.UnitTests.Controllers
{

    public class TechnicalControllerTests
    {
        [Fact]
        public void IndexTest()
        {
            var fakeApplicationLogger = A.Fake<IApplicationLogger>();
            var jobProfileAnchorListsController = new TechnicalController(fakeApplicationLogger);
           // var indexMethodCall = jobProfileAnchorListsController.WithCallTo(c => c.Index());
        }
    }
}
