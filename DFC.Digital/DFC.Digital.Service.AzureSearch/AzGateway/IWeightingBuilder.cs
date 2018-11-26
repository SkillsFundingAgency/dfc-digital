using System.Collections.Generic;

namespace DFC.Digital.Service.AzureSearch
{
    public interface IWeightingBuilder
    {
        IDictionary<string, double> BuildForType<T>();
    }
}