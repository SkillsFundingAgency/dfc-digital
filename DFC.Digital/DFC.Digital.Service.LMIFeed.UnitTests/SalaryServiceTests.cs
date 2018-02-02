using System.Net;
using System.Net.Http;
using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using FakeItEasy;
using FluentAssertions;
using DFC.Digital.Service.LMIFeed.Interfaces;
using Xunit;

namespace DFC.Digital.Service.LMIFeed.UnitTests
{
    using System;
    using Model;

    public class SalaryServiceTests: HelperJobProfileData
    {
        [Theory]
        [MemberData(nameof(JobProfileAsheData))]
        public void GetMedianDeciles_From_AsheFeed(string jobProfileForAsheFeed, string expectedSocCode)
        {
            //Arrange
            var applicationLogger = A.Fake<IApplicationLogger>(ops=>ops.Strict());
            var clientProxy = A.Fake<IAsheHttpClientProxy>(ops => ops.Strict());

            //Act
            var httpResponseMessage = new HttpResponseMessage {StatusCode = HttpStatusCode.OK};
            A.CallTo(() => clientProxy.EstimatePayMdAsync(A<string>._)).Returns(httpResponseMessage);
            A.CallTo(() => applicationLogger.Warn(A<string>._)).DoesNothing();
            A.CallTo(() => applicationLogger.ErrorJustLogIt(A<string>._,A<Exception>._)).DoesNothing();
            ISalaryService lmiFeed = new SalaryService(applicationLogger, clientProxy);

            //Assert
            var response = lmiFeed.GetSalaryBySocAsync(jobProfileForAsheFeed);
            A.CallTo(() => clientProxy.EstimatePayMdAsync(A<string>.That.IsEqualTo(expectedSocCode))).MustHaveHappened();
            response.Result.Should().Be(null);

        }
        [Theory]
        [MemberData(nameof(JobProfileAsheData))]
        public void ThrowsException_GetMedianDeciles_From_AsheFeed(string socCode, string expectedSocCode)
        {
            //Arrange
            var applicationLogger = A.Fake<IApplicationLogger>(ops => ops.Strict());
            var clientProxy = A.Fake<IAsheHttpClientProxy>(ops => ops.Strict());
            var httpResponseMessage = new HttpResponseMessage { StatusCode = HttpStatusCode.OK };

            //Act
            A.CallTo(() => clientProxy.EstimatePayMdAsync(A<string>._)).Throws<Exception>();
            A.CallTo(() => applicationLogger.Warn(A<string>._)).DoesNothing();
            A.CallTo(() => applicationLogger.ErrorJustLogIt(A<string>._,A<Exception>._)).DoesNothing();
            A.CallTo(() => clientProxy.EstimatePayMdAsync(A<string>._)).Returns(httpResponseMessage);

            ISalaryService lmiFeed = new SalaryService(applicationLogger, clientProxy);

            //Assert
            var response = lmiFeed.GetSalaryBySocAsync(socCode);
            A.CallTo(() => applicationLogger.ErrorJustLogIt(A<string>._,A<Exception>._)).MustHaveHappened();
            A.CallTo(() => clientProxy.EstimatePayMdAsync(A<string>.That.IsEqualTo(expectedSocCode))).MustHaveHappened();
            response.Result.Should().Be(null);

        }
    }
}