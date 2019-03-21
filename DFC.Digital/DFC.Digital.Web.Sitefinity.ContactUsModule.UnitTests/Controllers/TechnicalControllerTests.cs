using DFC.Digital.Core;
using FakeItEasy;
using Xunit;

namespace DFC.Digital.Web.Sitefinity.ContactUsModule.UnitTests.Controllers
{

    public class TechnicalControllerTests
    {
        [Fact]
        public void IndexTest()
        {
            //var jobProfileAnchorListsController = new TechnicalController(fakeApplicationLogger);
            // var indexMethodCall = jobProfileAnchorListsController.WithCallTo(c => c.Index());
            var fakeApplicationLogger = A.Fake<IApplicationLogger>();
        }
    }
}
