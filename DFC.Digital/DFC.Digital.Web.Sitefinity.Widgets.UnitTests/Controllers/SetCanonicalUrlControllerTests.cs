using DFC.Digital.Core;
using DFC.Digital.Data.Interfaces;
using DFC.Digital.Web.Sitefinity.Widgets.Mvc.Controllers;
using FakeItEasy;
using TestStack.FluentMVCTesting;
using Xunit;

namespace DFC.Digital.Web.Sitefinity.Widgets.UnitTests
{
    public class SetCanonicalUrlControllerTests
    {
        private readonly IApplicationLogger fakeApplicationLogger;
        private readonly IWebAppContext fakeWebAppContext;

        public SetCanonicalUrlControllerTests()
        {
            fakeApplicationLogger = A.Fake<IApplicationLogger>(ops => ops.Strict());
            fakeWebAppContext = A.Fake<IWebAppContext>(ops => ops.Strict());
            SetupCalls();
        }

        [Fact]
        public void IndexTest()
        {
            //Assign
            var fakeSetCanonicalUrlController = new SetCanonicalUrlController(fakeApplicationLogger, fakeWebAppContext);

            //Act
            var indexMethodCall = fakeSetCanonicalUrlController.WithCallTo(c => c.Index());

            //Assert
            indexMethodCall.ShouldReturnEmptyResult();
            A.CallTo(() => fakeWebAppContext.SetupCanonicalUrlEventHandler()).MustHaveHappened();
        }

        [Fact]
        public void IndexTestUrlName()
        {
            //Assign
            var fakeSetCanonicalUrlController = new SetCanonicalUrlController(fakeApplicationLogger, fakeWebAppContext);

            //Act
            var indexMethodCall = fakeSetCanonicalUrlController.WithCallTo(c => c.Index("test"));

            //Assert
            indexMethodCall.ShouldReturnEmptyResult();
            A.CallTo(() => fakeWebAppContext.SetupCanonicalUrlEventHandler()).MustHaveHappened();
        }

        private void SetupCalls() => A.CallTo(() => fakeWebAppContext.SetupCanonicalUrlEventHandler()).DoesNothing();
    }
}