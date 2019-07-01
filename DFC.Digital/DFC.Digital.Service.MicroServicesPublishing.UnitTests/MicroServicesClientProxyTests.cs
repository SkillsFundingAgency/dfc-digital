using DFC.Digital.Core;
using FakeItEasy;
using System.Net.Http;
using Xunit;

namespace DFC.Digital.Service.MicroServicesPublishing.UnitTests
{
    public class MicroServicesClientProxyTests
    {
        private const string DummyEndpoint = "dummyEndpoint";
        private const string DummyJson = "dummyJson";

        [Fact]
        public async void PostDataAsyncTest()
        {
            //Setup
            var fakeHttpClientService = A.Fake<IHttpClientService<IMicroServicesPublishingClientProxy>>(ops => ops.Strict());
            A.CallTo(() => fakeHttpClientService.PostAsync(A<string>._, A<string>._, A<FaultToleranceType>._)).Returns(new HttpResponseMessage());

            //Act
            var microServicesPublishingClientProxy = new MicroServicesPublishingClientProxy(fakeHttpClientService);
            await microServicesPublishingClientProxy.PostDataAsync(DummyEndpoint, DummyJson);

            //Asserts
            A.CallTo(() => fakeHttpClientService.PostAsync(DummyEndpoint, DummyJson, A<FaultToleranceType>._)).MustHaveHappenedOnceExactly();
        }
    }
}
