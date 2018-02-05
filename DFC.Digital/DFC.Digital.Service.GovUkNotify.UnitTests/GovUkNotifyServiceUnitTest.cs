using DFC.Digital.Service.GovUkNotify;
using System.Collections;
using System.Collections.Generic;
using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using FakeItEasy;
using FluentAssertions;
using Xunit;
using Notify.Exceptions;
using System;
using DFC.Digital.Core.Utilities;

namespace DFC.Digital.Service.GovUkNotify.UnitTests
{
    using System.Linq;

    public class GovUkNotifyServiceUnitTest
    {
        private IApplicationLogger _fakeApplicationLogger = A.Fake<IApplicationLogger>();
        private Base.IGovUkNotifyClientProxy _fakeGovUkNotifyClient = A.Fake<Base.IGovUkNotifyClientProxy>();

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
                A.CallTo(() => _fakeGovUkNotifyClient.SendEmail(A<string>._, A<string>._, A<string>._, A<Dictionary<string, dynamic>>._)).Throws<NotifyClientException>();
            }
            else
            {
                A.CallTo(() => _fakeGovUkNotifyClient.SendEmail(A<string>._, A<string>._, A<string>._, A<Dictionary<string, dynamic>>._)).Returns(emailResponse);
            }

            //Act
            var govUkNotifyService = new GovUkNotifyService(_fakeApplicationLogger, _fakeGovUkNotifyClient);
            var result = govUkNotifyService.SubmitEmail(emailAddress, new VocSurveyPersonalisation());

            //Assertions
            result.Should().Be(expectation);
            A.CallTo(() => _fakeGovUkNotifyClient.SendEmail(A<string>._, A<string>.That.IsEqualTo(emailAddress), A<string>._, A<Dictionary<string, dynamic>>._)).MustHaveHappened();
            if (throwError)
            {
                A.CallTo(() => _fakeApplicationLogger.ErrorJustLogIt(A<string>._, A<Exception>._)).MustHaveHappened();
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
            var govUkNotifyService = new GovUkNotifyService(_fakeApplicationLogger, _fakeGovUkNotifyClient);
            var result = govUkNotifyService.Convert(input);

            // Assert
            result.ShouldAllBeEquivalentTo(expectation);
        }
    }
}
