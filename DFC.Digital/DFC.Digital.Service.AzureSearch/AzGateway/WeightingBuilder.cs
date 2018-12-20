using DFC.Digital.Data.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DFC.Digital.Service.AzureSearch
{
    public class WeightingBuilder : IWeightingBuilder
    {
        public IDictionary<string, double> BuildForType<T>()
        {
            Type underlyingType = typeof(T);
            var fieldsWithWeightings = underlyingType.GetProperties().Where(p => p.GetCustomAttributes(typeof(AddWeightingAttribute), true)?.Count() > 0);

            var weightings = new Dictionary<string, double>();
            foreach (PropertyInfo p in fieldsWithWeightings)
            {
                var w = p.GetCustomAttribute(typeof(AddWeightingAttribute), true);
                var weightingValue = w.GetType().GetProperty(nameof(AddWeightingAttribute.Weighting)).GetValue(w, null);
                weightings.Add(p.Name, (double)weightingValue);
            }

            return weightings;
        }
    }
}