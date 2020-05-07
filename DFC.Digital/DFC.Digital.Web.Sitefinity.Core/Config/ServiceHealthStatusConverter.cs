using AutoMapper;
using DFC.Digital.Data.Interfaces;

namespace DFC.Digital.Web.Sitefinity.Core
{
    internal class ServiceHealthStatusConverter : IValueConverter<ServiceState, string>
    {
        public string Convert(ServiceState state, ResolutionContext context)
        {
            switch (state)
            {
                case ServiceState.Green:
                    return "Available";

                case ServiceState.Amber:
                    return "Degraded";

                case ServiceState.Red:
                default:
                    return "Unavailable";
            }
        }
    }
}
