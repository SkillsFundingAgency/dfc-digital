using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using System;

namespace DFC.Digital.Core
{
    public class EmailSimulateService : ISimulateEmailResponses
    {
        private readonly IConfigurationProvider configuration;

        public EmailSimulateService(IConfigurationProvider configuration)
        {
            this.configuration = configuration;
        }

        private string SimulationSuccessEmailAddress => configuration.GetConfig<string>(Constants.SimulationSuccessEmailAddress) ?? "simulate-delivered@nationalcareers.service.gov.uk";

        private string SimulationFailureEmailAddress => configuration.GetConfig<string>(Constants.SimulationFailureEmailAddress) ?? "simulate-failed@nationalcareers.service.gov.uk";

        public bool IsThisSimulationRequest(string email)
        {
            return !string.IsNullOrEmpty(email) && (email.Equals(SimulationFailureEmailAddress, StringComparison.InvariantCultureIgnoreCase) || email.Equals(SimulationSuccessEmailAddress, StringComparison.InvariantCultureIgnoreCase));
        }

        public bool SimulateEmailResponse(string email)
        {
            return !string.IsNullOrEmpty(email) && email.Equals(SimulationSuccessEmailAddress, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
