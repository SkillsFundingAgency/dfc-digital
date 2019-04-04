using DFC.Digital.Data.Model;

namespace DFC.Digital.Data.Interfaces
{
    public interface ISimulateEmailResponses
    {
        bool SimulateEmailResponse(string email);

        bool IsThisSimulationRequest(string email);
    }
}
