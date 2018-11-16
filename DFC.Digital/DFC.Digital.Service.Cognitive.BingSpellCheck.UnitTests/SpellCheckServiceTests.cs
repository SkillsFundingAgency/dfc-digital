using DFC.Digital.Core;
using DFC.Digital.Core.Configuration;
using DFC.Digital.Data.Interfaces;
using FakeItEasy;
using FluentAssertions;
using System;
using System.Net.Http;
using System.Text;

//using RichardSzalay.MockHttp;
using System.Threading.Tasks;
using Xunit;

namespace DFC.Digital.Service.Cognitive.BingSpellCheck.UnitTests
{
    public class SpellCheckServiceTests
    {
        [Theory]
        [InlineData("test", true, true)]
        [InlineData("test", false, true)]
        [InlineData("test", true, false)]
        [InlineData("test", false, false)]
        public async Task CheckSpellingAsyncTest(string term, bool suggestionsReturned, bool isResponseSuccess)
        {
            //Arrange
            var fakeHttpClientService = A.Fake<IHttpClientService<ISpellcheckService>>(ops => ops.Strict());
            var fakeLogger = A.Fake<IApplicationLogger>();
            var policy = new TolerancePolicy(fakeLogger, new TransientFaultHandlingStrategy(new InMemoryConfigurationProvider()));

            var json = suggestionsReturned ? "{\"_type\": \"SpellCheck\", \"flaggedTokens\": [{\"offset\": 0, \"token\": \"pluse\", \"type\": \"UnknownToken\", \"suggestions\": [{\"suggestion\": \"pulse\", \"score\": 1}]}]}\r\n" : "{\"_type\": \"SpellCheck\", \"flaggedTokens\": []}";
            var fakeHttpResponseMessage = new HttpResponseMessage
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json"),
                StatusCode = isResponseSuccess ? System.Net.HttpStatusCode.OK : System.Net.HttpStatusCode.ServiceUnavailable
            };

            A.CallTo(() => fakeHttpClientService.GetAsync(A<string>._, A<FaultToleranceType>._)).Returns(fakeHttpResponseMessage);
            A.CallTo(() => fakeHttpClientService.AddHeader(Constants.OcpApimSubscriptionKey, A<string>._)).Returns(true);

            //Act
            var spellingService = new SpellCheckService(fakeHttpClientService, fakeLogger);
            var result = await spellingService.CheckSpellingAsync(term);

            //Assert
            if (isResponseSuccess && suggestionsReturned)
            {
                result.HasCorrected.Should().BeTrue();
            }
            else
            {
                result.HasCorrected.Should().BeFalse();
            }
        }

        [Theory]
        [InlineData(true, ServiceState.Green, true)]
        [InlineData(false, ServiceState.Amber, true)]
        [InlineData(true, ServiceState.Red, false)]
        [InlineData(false, ServiceState.Red, false)]
        public async Task GetServiceStatusAsync(bool suggestionsReturned, ServiceState expectedServiceStatus, bool isResponseSuccess)
        {
            //Arrange
            var fakeHttpClientService = A.Fake<IHttpClientService<ISpellcheckService>>(ops => ops.Strict());
            var fakeLogger = A.Fake<IApplicationLogger>();
            var policy = new TolerancePolicy(fakeLogger, new TransientFaultHandlingStrategy(new InMemoryConfigurationProvider()));

            var json = suggestionsReturned ? "{\"_type\": \"SpellCheck\", \"flaggedTokens\": [{\"offset\": 0, \"token\": \"pluse\", \"type\": \"UnknownToken\", \"suggestions\": [{\"suggestion\": \"pulse\", \"score\": 1}]}]}\r\n" : "{\"_type\": \"SpellCheck\", \"flaggedTokens\": []}";
            var fakeHttpResponseMessage = new HttpResponseMessage
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json"),
                StatusCode = isResponseSuccess ? System.Net.HttpStatusCode.OK : System.Net.HttpStatusCode.ServiceUnavailable,
                ReasonPhrase = nameof(GetServiceStatusAsync)
            };

            //Setup Dummies and Mocks
            A.CallTo(() => fakeHttpClientService.GetAsync(A<string>._, A<FaultToleranceType>._)).Returns(fakeHttpResponseMessage);
            A.CallTo(() => fakeHttpClientService.AddHeader(Constants.OcpApimSubscriptionKey, A<string>._)).Returns(true);

            //Act
            var spellingService = new SpellCheckService(fakeHttpClientService, fakeLogger);
            var serviceStatus = await spellingService.GetCurrentStatusAsync();

            //Asserts
            serviceStatus.Status.Should().Be(expectedServiceStatus);
        }

        [Fact]
        public async Task GetServiceStatusExceptionAsync()
        {
            //Arrange
            var fakeHttpClientService = A.Fake<IHttpClientService<ISpellcheckService>>(ops => ops.Strict());
            var fakeLogger = A.Fake<IApplicationLogger>(ops => ops.Strict());
            var policy = new TolerancePolicy(fakeLogger, new TransientFaultHandlingStrategy(new InMemoryConfigurationProvider()));
            var json = "{\"_type\": \"Cause Exception\"}";
            var fakeHttpResponseMessage = new HttpResponseMessage
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json"),
                StatusCode = System.Net.HttpStatusCode.ServiceUnavailable,
                ReasonPhrase = nameof(GetServiceStatusAsync)
            };

            A.CallTo(() => fakeHttpClientService.GetAsync(A<string>._, A<FaultToleranceType>._)).Returns(fakeHttpResponseMessage);
            A.CallTo(() => fakeHttpClientService.AddHeader(Constants.OcpApimSubscriptionKey, A<string>._)).Returns(true);
            A.CallTo(() => fakeLogger.LogExceptionWithActivityId(A<string>._, A<Exception>._)).Returns("Exception logged");

            //Act
            var spellingService = new SpellCheckService(fakeHttpClientService, fakeLogger);
            var serviceStatus = await spellingService.GetCurrentStatusAsync();

            //Asserts
            serviceStatus.Status.Should().NotBe(ServiceState.Green);
            serviceStatus.Notes.Should().Contain("Exception logged");
            A.CallTo(() => fakeLogger.LogExceptionWithActivityId(A<string>._, A<Exception>._)).MustHaveHappened();
        }
    }
}