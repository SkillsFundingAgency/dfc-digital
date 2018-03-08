using DFC.Digital.Data.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DFC.Digital.Service.AzureSearch
{
    public class SuggesterBuilder : ISuggesterBuilder
    {
        public IList<string> BuildForType<T>()
        {
            Type underlyingType = typeof(T);
            var suggestionFields = underlyingType.GetProperties().Where(p => p.GetCustomAttributes(typeof(IsSuggestibleAttribute), true)?.Count() > 0);
            return suggestionFields.Select(s => s.Name).ToList();
        }
    }
}