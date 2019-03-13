using DFC.Digital.Data.Model;

namespace DFC.Digital.Data.Interfaces
{
    public interface ISimulateEmailResponses
    {
        SimulateEmailResponse SimulateEmailResponse(string emailAddress);
    }
}
