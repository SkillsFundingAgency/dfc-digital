﻿using DFC.Digital.Data.Interfaces;
using FakeItEasy;
using FluentAssertions;
using RichardSzalay.MockHttp;
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
            var fakeHttpClientService = A.Fake<IHttpClientService>(ops => ops.Strict());
            var mockHttp = new MockHttpMessageHandler();

            //Setup Dummies and Mocks
            mockHttp.When("*")
                .Respond(
                "application/json",
                suggestionsReturned ? "{\"_type\": \"SpellCheck\", \"flaggedTokens\": [{\"offset\": 0, \"token\": \"pluse\", \"type\": \"UnknownToken\", \"suggestions\": [{\"suggestion\": \"pulse\", \"score\": 1}]}]}\r\n" : "{\"_type\": \"SpellCheck\", \"flaggedTokens\": []}");

            A.CallTo(() => fakeHttpClientService.GetHttpClient()).Returns(new HttpClient(mockHttp));
            var spellingService = new SpellCheckService(fakeHttpClientService);

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
        public async Task GetServiceStatus(bool suggestionsReturned, ServiceState expectedServiceStatus)
        {
            //Arrange
            var fakeHttpClientService = A.Fake<IHttpClientService>(ops => ops.Strict());
            var mockHttp = new MockHttpMessageHandler();

            //Setup Dummies and Mocks
            mockHttp.When("*")
                .Respond(
                "application/json",
                suggestionsReturned ? "{\"_type\": \"SpellCheck\", \"flaggedTokens\": [{\"offset\": 0, \"token\": \"pluse\", \"type\": \"UnknownToken\", \"suggestions\": [{\"suggestion\": \"pulse\", \"score\": 1}]}]}\r\n" : "{\"_type\": \"SpellCheck\", \"flaggedTokens\": []}");

            A.CallTo(() => fakeHttpClientService.GetHttpClient()).Returns(new HttpClient(mockHttp));

            var spellingService = new SpellCheckService(fakeHttpClientService);

            //Act
            var serviceStatus = await spellingService.GetCurrentStatusAsync();

            //Asserts
            serviceStatus.Status.Should().Be(expectedServiceStatus);
        }

        [Fact]
        public async Task GetServiceStatusException()
        {
            //Arrange
            var fakeHttpClientService = A.Fake<IHttpClientService>(ops => ops.Strict());
            var mockHttp = new MockHttpMessageHandler();

            //Setup Dummies and Mocks
            mockHttp.When("*")
                .Respond(
                "application/json",
                "{\"_type\": \"Cause Exception\"}");

            A.CallTo(() => fakeHttpClientService.GetHttpClient()).Returns(new HttpClient(mockHttp));

            var spellingService = new SpellCheckService(fakeHttpClientService);

            //Act
            var serviceStatus = await spellingService.GetCurrentStatusAsync();

            //Asserts
            serviceStatus.Status.Should().NotBe(ServiceState.Green);
            serviceStatus.Notes.Should().Contain("Exception");
        }
    }
}