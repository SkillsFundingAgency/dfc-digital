using FakeItEasy;
using FluentAssertions;
using Xunit;

namespace DFC.Digital.Core.Utilities.Tests
{
    public class EmailSimulateServiceTests
    {
        private readonly IConfigurationProvider fakeConfiguration;

        public EmailSimulateServiceTests()
        {
            fakeConfiguration = A.Fake<IConfigurationProvider>(ops => ops.Strict());
        }

        [Theory]
        [InlineData(Constants.SimulationFailureEmailAddress, false)]
        [InlineData(Constants.SimulationSuccessEmailAddress, true)]
        [InlineData(Constants.SendGridApiKey, false)]
        [InlineData("", false)]
        public void SimulateEmailResponseTest(string emailAddress, bool isSuccessResponse)
        {
            // Assign
            A.CallTo(() => fakeConfiguration.GetConfig<string>(A<string>.That.Matches(x => x.Equals(Constants.SimulationSuccessEmailAddress)))).Returns(Constants.SimulationSuccessEmailAddress);
            A.CallTo(() => fakeConfiguration.GetConfig<string>(A<string>.That.Matches(x => x.Equals(Constants.SimulationFailureEmailAddress)))).Returns(Constants.SimulationFailureEmailAddress);

            // Act
            var simulationServices = new EmailSimulateService(fakeConfiguration);
            var response = simulationServices.SimulateEmailResponse(emailAddress);

            // Assert
            response.Should().Be(isSuccessResponse);
        }

        [Theory]
        [InlineData(Constants.SimulationFailureEmailAddress, true)]
        [InlineData(Constants.SimulationSuccessEmailAddress, true)]
        [InlineData(Constants.SendGridApiKey, false)]
        [InlineData("", false)]
        public void IsThisSimulationRequestTest(string emailAddress, bool isSimulationEmail)
        {
            // Assign
            A.CallTo(() => fakeConfiguration.GetConfig<string>(A<string>.That.Matches(x => x.Equals(Constants.SimulationSuccessEmailAddress)))).Returns(Constants.SimulationSuccessEmailAddress);
            A.CallTo(() => fakeConfiguration.GetConfig<string>(A<string>.That.Matches(x => x.Equals(Constants.SimulationFailureEmailAddress)))).Returns(Constants.SimulationFailureEmailAddress);

            // Act
            var simulationServices = new EmailSimulateService(fakeConfiguration);
            var validSimulationEmail = simulationServices.IsThisSimulationRequest(emailAddress);

            // Assert
            validSimulationEmail.Should().Be(isSimulationEmail);
        }
    }
}