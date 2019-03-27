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
        private const string CDNLocation = "CDNLocation";
        private const string DummyAssetFile = "DummyAssetFile";
        private const string DummyAssetFilePath = "folder/DummyAssetFile";
        private readonly IConfigurationProvider fakeConfigurationProvider;
        private readonly IHttpClientService<IAssetLocationAndVersion> fakeHTTPClientService;
        private readonly IAsyncHelper fakeAsyncHelper;
        private readonly IWebAppContext fakeWbAppContext;
        private readonly IApplicationLogger fakeApplicationLogger;

        public AssetLocationAndVersionTests()
        {
            fakeConfigurationProvider = A.Fake<IConfigurationProvider>(ops => ops.Strict());
            fakeHTTPClientService = A.Fake<IHttpClientService<IAssetLocationAndVersion>>(ops => ops.Strict());
            fakeWbAppContext = A.Fake<IWebAppContext>(ops => ops.Strict());
            fakeApplicationLogger = A.Fake<IApplicationLogger>(ops => ops.Strict());
            fakeAsyncHelper = new AsyncHelper();
        }

        [Fact]
        public void GetLocationAssetFileAndVersionTest()
        {
            var dummyHttpResponseMessage = new HttpResponseMessage()
            {
                Content = new StringContent("Dummy Content"),
                StatusCode = System.Net.HttpStatusCode.InternalServerError
            };

            var expectedAssetFileLocation = $"{CDNLocation}/{DummyAssetFile}";

            A.CallTo(() => fakeHTTPClientService.GetAsync(A<string>._, A<FaultToleranceType>._)).Returns(dummyHttpResponseMessage);

            var assetLocationAndVersion = new AssetLocationAndVersion(fakeConfigurationProvider, fakeHTTPClientService, fakeAsyncHelper, fakeWbAppContext, fakeApplicationLogger);
            A.CallTo(() => fakeConfigurationProvider.GetConfig<string>(A<string>._)).Returns(CDNLocation);
            var result = assetLocationAndVersion.GetLocationAssetFileAndVersion(DummyAssetFile);

            result.Should().StartWith($"{expectedAssetFileLocation}?{DateTime.Now.ToString("yyyyMMdd")}");
        }

        [Fact]
        public void GetLocationAssetFileAndVersionNoCdnTest()
        {
            var dummyHttpResponseMessage = new HttpResponseMessage()
            {
                Content = new StringContent("Dummy Content"),
                StatusCode = System.Net.HttpStatusCode.InternalServerError
            };

            var expectedAssetFileLocation = "/ResourcePackages/folder/assets/dist/DummyAssetFile?";

            A.CallTo(() => fakeConfigurationProvider.GetConfig<string>(A<string>._)).Returns(null);
            A.CallTo(() => fakeWbAppContext.ServerMapPath(A<string>._)).ReturnsLazily((string file) => file);

            var assetLocationAndVersion = new AssetLocationAndVersion(fakeConfigurationProvider, fakeHTTPClientService, fakeAsyncHelper, fakeWbAppContext, fakeApplicationLogger);
            var result = assetLocationAndVersion.GetLocationAssetFileAndVersion(DummyAssetFilePath);

            result.Should().Be(expectedAssetFileLocation);
        }
    }
}
