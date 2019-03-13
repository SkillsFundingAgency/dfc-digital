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
        [InlineData(Constants.SimulationFailureEmailAddress, true, false)]
        [InlineData(Constants.SimulationSuccessEmailAddress, true, true)]
        [InlineData(Constants.SendGridApiKey, false, false)]
        [InlineData("", false, false)]
        public void SimulateEmailResponseTest(string emailAddress, bool isSimulationEmail, bool isSuccessResponse)
        {
            // Assign
            A.CallTo(() => fakeConfiguration.GetConfig<string>(A<string>.That.Matches(x => x.Equals(Constants.SimulationSuccessEmailAddress)))).Returns(Constants.SimulationSuccessEmailAddress);
            A.CallTo(() => fakeConfiguration.GetConfig<string>(A<string>.That.Matches(x => x.Equals(Constants.SimulationFailureEmailAddress)))).Returns(Constants.SimulationFailureEmailAddress);

            // Act
            var simulationServices = new EmailSimulateService(fakeConfiguration);
            var response = simulationServices.SimulateEmailResponse(emailAddress);

            // Assert
            response.SuccessResponse.Should().Be(isSuccessResponse);
            response.ValidSimulationEmail.Should().Be(isSimulationEmail);
        }
    }
}