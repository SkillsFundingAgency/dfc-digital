using DFC.Digital.Core;
using DFC.Digital.Web.Sitefinity.Widgets.Mvc.Controllers;
using FakeItEasy;
using TestStack.FluentMVCTesting;
using Xunit;

namespace DFC.Digital.Web.Sitefinity.Widgets.UnitTests
{
    public class SetCanonicalUrlControllerTests
    {
        private readonly IApplicationLogger fakeApplicationLogger;

        public SetCanonicalUrlControllerTests()
        {
            fakeApplicationLogger = A.Fake<IApplicationLogger>(ops => ops.Strict());
        }

        [Fact]
        public void IndexTest()
        {
            //Assign
            var fakeSetCanonicalUrlController = new SetCanonicalUrlController(fakeApplicationLogger);

            //Act
            var indexMethodCall = fakeSetCanonicalUrlController.WithCallTo(c => c.Index());

            //Assert
            indexMethodCall.ShouldReturnEmptyResult();
        }

        [Fact]
        public void IndexTestUrlName()
        {
            //Assign
            var fakeSetCanonicalUrlController = new SetCanonicalUrlController(fakeApplicationLogger);

            //Act
            var indexMethodCall = fakeSetCanonicalUrlController.WithCallTo(c => c.Index("test"));

            //Assert
            indexMethodCall.ShouldReturnEmptyResult();
        }
    }
}