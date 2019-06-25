using DFC.Digital.Core;
using DFC.Digital.Service.MicroServicesPublishing;
using FakeItEasy;
using System.Net.Http;
using Xunit;

namespace DFC.Digital.Service.MicroServicesPublishing.UnitTests
{
    public class MicroServicesClientProxyTests
    {
        private const string dummyEndpoint = "dummyEndpoint";
        private const string dummyJson = "dummyJson";

        [Fact]
        public void PostDataAsyncTest()
        {
            //Setup
            var fakeHttpClientService = A.Fake<IHttpClientService<IMicroServicesPublishingClientProxy>>(ops => ops.Strict());
            A.CallTo(() => fakeHttpClientService.PostAsync(A<string>._, A<string>._, A<FaultToleranceType>._)).Returns(new HttpResponseMessage());

            //Act
            var microServicesPublishingClientProxy = new MicroServicesPublishingClientProxy(fakeHttpClientService);
            var result = microServicesPublishingClientProxy.PostDataAsync(dummyEndpoint, dummyJson);

            //Asserts
            A.CallTo(() => fakeHttpClientService.PostAsync(dummyEndpoint, dummyJson, A<FaultToleranceType>._)).MustHaveHappenedOnceExactly();
        }
    }
}
