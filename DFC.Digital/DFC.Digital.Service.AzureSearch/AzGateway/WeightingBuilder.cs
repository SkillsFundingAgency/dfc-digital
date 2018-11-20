using DFC.Digital.Data.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DFC.Digital.Service.AzureSearch
{
    public class WeightingBuilder : IWeightingBuilder
    {
        IDictionary<string, double> IWeightingBuilder.BuildForType<T>()
        {
            //Dictionary<string, double> weightings = new Dictionary<string, double>();
            Type underlyingType = typeof(T);

            var fieldsWithWeightings = underlyingType.GetProperties().Where(p => p.GetCustomAttributes(typeof(WeightingAttribute), true)?.Count() > 0);

            var weightings = new Dictionary<string, double>();
            foreach (PropertyInfo p in fieldsWithWeightings)
            {
                var w = p.GetCustomAttribute(typeof(WeightingAttribute), true);
                var weightingValue = w.GetType().GetProperty("Weighting").GetValue(w, null);
                weightings.Add(p.Name, (double)weightingValue);
            }

            return weightings;
        }
    }
}