using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using System;

namespace DFC.Digital.Core.Utilities
{
    public class EmailSimulateService : ISimulateEmailResponses
    {
        private readonly IConfigurationProvider configuration;

        public EmailSimulateService(IConfigurationProvider configuration)
        {
            this.configuration = configuration;
        }

        public string SimulationSuccessEmailAddress => configuration.GetConfig<string>(Constants.SimulationSuccessEmailAddress);

        public string SimulationFailureEmailAddress => configuration.GetConfig<string>(Constants.SimulationFailureEmailAddress);

        public SimulateEmailResponse SimulateEmailResponse(string emailAddress)
        {
            var simulationResponse = new SimulateEmailResponse();
            if (string.IsNullOrWhiteSpace(emailAddress))
            {
                return simulationResponse;
            }

            simulationResponse.SuccessResponse = emailAddress.Equals(SimulationSuccessEmailAddress, StringComparison.InvariantCultureIgnoreCase);
            simulationResponse.ValidSimulationEmail = emailAddress.Equals(SimulationFailureEmailAddress, StringComparison.InvariantCultureIgnoreCase) || emailAddress.Equals(SimulationSuccessEmailAddress, StringComparison.InvariantCultureIgnoreCase);
            return simulationResponse;
        }
    }
}
