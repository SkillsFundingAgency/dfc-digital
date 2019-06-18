using DFC.Digital.Core;
using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using DFC.Digital.Service.CompositeUI;
using FakeItEasy;
using FluentAssertions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DFC.Digital.Service.CompositeUIPublishing.UnitTests
{
    public class CompositeUIPublishingTests
    {
        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task PostPageDataAsync(bool expectedResponse)
        {
            //Arrange
            var applicationLogger = A.Fake<IApplicationLogger>(ops => ops.Strict());
            var clientProxy = A.Fake<ICompositeClientProxy>(ops => ops.Strict());

            HttpStatusCode expectedHttpStatusCode;
            if (expectedResponse)
            {
                expectedHttpStatusCode = HttpStatusCode.OK;
            }
            else
            {
                expectedHttpStatusCode = HttpStatusCode.BadRequest;
            }

            var httpResponseMessage = new HttpResponseMessage(expectedHttpStatusCode);

            A.CallTo(() => clientProxy.PostDataAsync(A<string>._)).Returns(httpResponseMessage);
            A.CallTo(() => applicationLogger.Trace(A<string>._)).DoesNothing();
            A.CallTo(() => applicationLogger.Info(A<string>._)).DoesNothing();

            //Act
            var compositeUIService = new CompositeUIService(applicationLogger, clientProxy);
            var result = await compositeUIService.PostPageDataAsync(new CompositePageData());

            //Assert
            result.Should().Be(expectedResponse);
        }


        [Theory]
        [InlineData(true, ServiceState.Green)]
        [InlineData(false, ServiceState.Red)]
        public async Task GetServiceStatusAsync(bool returnValidHttpStatusCode, ServiceState expectedServiceStatus)
        {
            //Arrange
            var applicationLogger = A.Fake<IApplicationLogger>(ops => ops.Strict());
            var clientProxy = A.Fake<ICompositeClientProxy>(ops => ops.Strict());

            HttpStatusCode expectedHttpStatusCode;
            if (returnValidHttpStatusCode)
            {
                expectedHttpStatusCode = HttpStatusCode.OK;
            }
            else
            {
                expectedHttpStatusCode = HttpStatusCode.BadRequest;
            }

            var httpResponseMessage = new HttpResponseMessage(expectedHttpStatusCode);

            A.CallTo(() => clientProxy.PostDataAsync(A<string>._)).Returns(httpResponseMessage);
            A.CallTo(() => applicationLogger.Warn(A<string>._)).DoesNothing();
            A.CallTo(() => applicationLogger.ErrorJustLogIt(A<string>._, A<Exception>._)).DoesNothing();

            //Act
            var compositeUIService = new CompositeUIService(applicationLogger, clientProxy);
            var serviceStatus = await compositeUIService.GetCurrentStatusAsync();

            //Assert
            serviceStatus.Status.Should().Be(expectedServiceStatus);
        }

        [Fact]
        public async Task GetServiceStatusExceptionAsync()
        {
            //Arrange
            var applicationLogger = A.Fake<IApplicationLogger>(ops => ops.Strict());
            var clientProxy = A.Fake<ICompositeClientProxy>(ops => ops.Strict());

            //add no content to cause an exception
            var httpResponseMessage = new HttpResponseMessage();

            A.CallTo(() => clientProxy.PostDataAsync(A<string>._)).Throws(new HttpRequestException() );
            A.CallTo(() => applicationLogger.Warn(A<string>._)).DoesNothing();
            A.CallTo(() => applicationLogger.LogExceptionWithActivityId(A<string>._, A<Exception>._)).Returns("Exception logged");

            //Act
            var compositeUIService = new CompositeUIService(applicationLogger, clientProxy);
            var serviceStatus = await compositeUIService.GetCurrentStatusAsync();

            //Asserts
            serviceStatus.Status.Should().NotBe(ServiceState.Green);
            serviceStatus.Notes.Should().Contain("Exception");
            A.CallTo(() => applicationLogger.LogExceptionWithActivityId(A<string>._, A<Exception>._)).MustHaveHappened();
        }
    }
}
