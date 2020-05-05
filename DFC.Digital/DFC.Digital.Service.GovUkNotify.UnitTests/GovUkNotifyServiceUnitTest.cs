using System.Collections.Generic;
using DFC.Digital.Data.Interfaces;
using DFC.Digital.Core;
using DFC.Digital.Data.Model;
using FakeItEasy;
using FluentAssertions;
using Xunit;
using Notify.Exceptions;
using System;

namespace DFC.Digital.Service.GovUkNotify.UnitTests
{
    using System.Threading.Tasks;

    public class GovUkNotifyServiceUnitTest
    {
        private IApplicationLogger fakeApplicationLogger = A.Fake<IApplicationLogger>();
        private Base.IGovUkNotifyClientProxy fakeGovUkNotifyClient = A.Fake<Base.IGovUkNotifyClientProxy>();

        [Theory]
        [InlineData("1", false, true)]
        [InlineData(null, false, false)]
        [InlineData("error", true, false)]
        public void SubmitEmail(string responseId, bool throwError, bool expectation)
        {
            //Fakes
            string emailAddress = "dumy@email.com";
            var emailResponse = responseId == null ? null : new Notify.Models.Responses.EmailNotificationResponse
            {
                id = responseId
            };

            //Configure calls
            if (throwError)
            {
                A.CallTo(() => fakeGovUkNotifyClient.SendEmail(A<string>._, A<string>._, A<string>._, A<Dictionary<string, dynamic>>._)).Throws<NotifyClientException>();
            }
            else
            {
                A.CallTo(() => fakeGovUkNotifyClient.SendEmail(A<string>._, A<string>._, A<string>._, A<Dictionary<string, dynamic>>._)).Returns(emailResponse);
            }

            //Act
            var govUkNotifyService = new GovUkNotifyService(fakeApplicationLogger, fakeGovUkNotifyClient);
            var result = govUkNotifyService.SubmitEmail(emailAddress, new VocSurveyPersonalisation());

            //Assertions
            result.Should().Be(expectation);
            A.CallTo(() => fakeGovUkNotifyClient.SendEmail(A<string>._, A<string>.That.IsEqualTo(emailAddress), A<string>._, A<Dictionary<string, dynamic>>._)).MustHaveHappened();
            if (throwError)
            {
                A.CallTo(() => fakeApplicationLogger.ErrorJustLogIt(A<string>._, A<Exception>._)).MustHaveHappened();
            }
        }

        [Theory]
        [InlineData(Constants.LastVisitedJobProfileKey, "blah", "blah")]
        [InlineData(Constants.LastVisitedJobProfileKey, null, Constants.Unknown)]
        [InlineData(Constants.LastVisitedJobProfileKey, "", Constants.Unknown)]
        public void ConvertTest(string key, string sourceValue, string expectedValue)
        {
            // Arrange
            var input = new VocSurveyPersonalisation
            {
                Personalisation = new Dictionary<string, string>
                {
                    { key, sourceValue }
                }
            };

            var expectation = new Dictionary<string, dynamic>
            {
                { key, expectedValue }
            };

            // Act
            var govUkNotifyService = new GovUkNotifyService(fakeApplicationLogger, fakeGovUkNotifyClient);
            var result = govUkNotifyService.Convert(input);

            // Assert
            result.Should().BeEquivalentTo(expectation);
        }

        [Theory]
        [InlineData("1", ServiceState.Green)]
        [InlineData(null, ServiceState.Amber)]
        public async Task GetServiceStatusAsync(string responseId, ServiceState expectedServiceStatus)
        {
            //Fakes
            var emailResponse = responseId == null ? null : new Notify.Models.Responses.EmailNotificationResponse
            {
                id = responseId
            };

            A.CallTo(() => fakeGovUkNotifyClient.SendEmail(A<string>._, A<string>._, A<string>._, A<Dictionary<string, dynamic>>._)).Returns(emailResponse);

            //Act
            var govUkNotifyService = new GovUkNotifyService(fakeApplicationLogger, fakeGovUkNotifyClient);
            var serviceStatus = await  govUkNotifyService.GetCurrentStatusAsync();

            //Assert
            serviceStatus.Status.Should().Be(expectedServiceStatus);
        }

        [Fact]
        public async Task GetServiceStatusExceptionAsync()
        {
          
            //Fake set up incorrectly to cause exception
            A.CallTo(() => fakeGovUkNotifyClient.SendEmail(A<string>._, A<string>._, A<string>._, A<Dictionary<string, dynamic>>._)).Throws<NotifyClientException>();
           
            A.CallTo(() => fakeApplicationLogger.LogExceptionWithActivityId(A<string>._, A<Exception>._)).Returns("Exception logged");

            
            //Act
            var govUkNotifyService = new GovUkNotifyService(fakeApplicationLogger, fakeGovUkNotifyClient);
            var serviceStatus = await govUkNotifyService.GetCurrentStatusAsync();
           

            //Asserts
            serviceStatus.Status.Should().NotBe(ServiceState.Green);
            serviceStatus.CheckCorrelationId.Should().Contain("Exception");
          
        }

    }
}
