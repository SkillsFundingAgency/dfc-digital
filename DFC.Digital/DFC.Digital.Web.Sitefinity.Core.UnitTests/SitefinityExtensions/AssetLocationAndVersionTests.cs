using DFC.Digital.Core;
using DFC.Digital.Data.Interfaces;
using FakeItEasy;
using FluentAssertions;
using System;
using System.Net.Http;
using Xunit;

namespace DFC.Digital.Web.Sitefinity.Core.UnitTests.SitefinityExtensions
{
    public class AssetLocationAndVersionTests
    {
        private const string ContentMDS = "content-md5";
        private const string CDNLocation = "CDNLocation";
        private const string DummyAssetFile = "DummyAssetFile";
        private readonly IConfigurationProvider fakeConfigurationProvider;
        private readonly IHttpClientService<IAssetLocationAndVersion> fakeHTTPClientService;
        private readonly IAsyncHelper fakeAsyncHelper;

        public AssetLocationAndVersionTests()
        {
            fakeConfigurationProvider = A.Fake<IConfigurationProvider>(ops => ops.Strict());
            fakeHTTPClientService = A.Fake<IHttpClientService<IAssetLocationAndVersion>>(ops => ops.Strict());

            //fakeAsyncHelper = A.Fake<IAsyncHelper>(ops => ops.Strict());
            fakeAsyncHelper = new AsyncHelper();
        }

        [Theory]
        [InlineData(false)]
        public void GetLocationAssetFileAndVersionTest(bool isResponseSuccess)
        {
            var dummyHttpResponseMessage = new HttpResponseMessage()
            {
                Content = new StringContent("Dummy Content"),
                StatusCode = isResponseSuccess ? System.Net.HttpStatusCode.OK : System.Net.HttpStatusCode.InternalServerError
            };

            var expectedAssetFileLocation = $"{CDNLocation}/{DummyAssetFile}";

            A.CallTo(() => fakeHTTPClientService.GetAsync(A<string>._, A<FaultToleranceType>._)).Returns(dummyHttpResponseMessage);

            var assetLocationAndVersion = new AssetLocationAndVersion(fakeConfigurationProvider, fakeHTTPClientService, fakeAsyncHelper);
            A.CallTo(() => fakeConfigurationProvider.GetConfig<string>(A<string>._)).Returns(CDNLocation);
            var result = assetLocationAndVersion.GetLocationAssetFileAndVersion(DummyAssetFile);

            result.Should().StartWith($"{expectedAssetFileLocation}?{DateTime.Now.ToString("yyyyMMdd")}");
        }
    }
}
