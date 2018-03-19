using DFC.Digital.Core;
using DFC.Digital.Core.Configuration;
using DFC.Digital.Data.Interfaces;
using FakeItEasy;
using FluentAssertions;
using RichardSzalay.MockHttp;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace DFC.Digital.Service.Cognitive.BingSpellCheck.UnitTests
{
    public class SpellCheckServiceTests
    {
        [Theory]
        [InlineData("test", true)]
        [InlineData("test", false)]
        public async Task CheckSpellingAsyncTest(string term, bool suggestionsReturned)
        {
            //Arrange
            var fakeHttpClientService = A.Fake<IHttpClientService<ISpellcheckService>>(ops => ops.Strict());
            var fakeLogger = A.Fake<IApplicationLogger>();
            var policy = new TolerancePolicy(fakeLogger, new TransientFaultHandlingStrategy(new InMemoryConfigurationProvider()));
            var mockHttp = new MockHttpMessageHandler();

            //Setup Dummies and Mocks
            mockHttp.When("*")
                .Respond(
                "application/json",
                suggestionsReturned ? "{\"_type\": \"SpellCheck\", \"flaggedTokens\": [{\"offset\": 0, \"token\": \"pluse\", \"type\": \"UnknownToken\", \"suggestions\": [{\"suggestion\": \"pulse\", \"score\": 1}]}]}\r\n" : "{\"_type\": \"SpellCheck\", \"flaggedTokens\": []}");

            A.CallTo(() => fakeHttpClientService.GetAsync(A<string>._, A<FaultToleranceType>._)).Returns(new HttpClient(mockHttp).GetAsync("http://mockurl"));
            A.CallTo(() => fakeHttpClientService.AddHeader(Constants.OcpApimSubscriptionKey, A<string>._)).Returns(true);

            var spellingService = new SpellCheckService(fakeHttpClientService, fakeLogger);

            //Act
            var result = await spellingService.CheckSpellingAsync(term);

            //Assert
            if (suggestionsReturned)
            {
                result.HasCorrected.Should().BeTrue();
            }
            else
            {
                result.HasCorrected.Should().BeFalse();
            }
        }

        [Theory]
        [InlineData(true, ServiceState.Green)]
        [InlineData(false, ServiceState.Amber)]
        public async Task GetServiceStatusAsync(bool suggestionsReturned, ServiceState expectedServiceStatus)
        {
            //Arrange
            var fakeHttpClientService = A.Fake<IHttpClientService<ISpellcheckService>>(ops => ops.Strict());
            var fakeLogger = A.Fake<IApplicationLogger>(ops => ops.Strict());
            var mockHttp = new MockHttpMessageHandler();
            var policy = new TolerancePolicy(fakeLogger, new TransientFaultHandlingStrategy(new InMemoryConfigurationProvider()));

            //Setup Dummies and Mocks
            mockHttp.When("*")
                .Respond(
                "application/json",
                suggestionsReturned ? "{\"_type\": \"SpellCheck\", \"flaggedTokens\": [{\"offset\": 0, \"token\": \"pluse\", \"type\": \"UnknownToken\", \"suggestions\": [{\"suggestion\": \"pulse\", \"score\": 1}]}]}\r\n" : "{\"_type\": \"SpellCheck\", \"flaggedTokens\": []}");

            A.CallTo(() => fakeHttpClientService.GetAsync(A<string>._, A<FaultToleranceType>._)).Returns(new HttpClient(mockHttp).GetAsync("http://mockurl"));
            A.CallTo(() => fakeHttpClientService.AddHeader(Constants.OcpApimSubscriptionKey, A<string>._)).Returns(true);

            var spellingService = new SpellCheckService(fakeHttpClientService, fakeLogger);

            //Act
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
            var mockHttp = new MockHttpMessageHandler();
            var policy = new TolerancePolicy(fakeLogger, new TransientFaultHandlingStrategy(new InMemoryConfigurationProvider()));

            //Setup Dummies and Mocks
            mockHttp.When("*")
                .Respond(
                "application/json",
                "{\"_type\": \"Cause Exception\"}");

            A.CallTo(() => fakeHttpClientService.GetAsync(A<string>._, A<FaultToleranceType>._)).Returns(new HttpClient(mockHttp).GetAsync("http://mockurl"));
            A.CallTo(() => fakeHttpClientService.AddHeader(Constants.OcpApimSubscriptionKey, A<string>._)).Returns(true);

            A.CallTo(() => fakeLogger.LogExceptionWithActivityId(A<string>._, A<Exception>._)).Returns("Exception logged");

            var spellingService = new SpellCheckService(fakeHttpClientService, fakeLogger);

            //Act
            var serviceStatus = await spellingService.GetCurrentStatusAsync();

            //Asserts
            serviceStatus.Status.Should().NotBe(ServiceState.Green);
            serviceStatus.Notes.Should().Contain("Exception logged");
            A.CallTo(() => fakeLogger.LogExceptionWithActivityId(A<string>._, A<Exception>._)).MustHaveHappened();
        }
    }
}