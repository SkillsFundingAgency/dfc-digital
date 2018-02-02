using System.Collections.Generic;

namespace DFC.Digital.Data.Model
{
    public class SuggestionResult<T>
        where T : class
    {
        public double? Coverage { get; set; }

        public IEnumerable<SuggestionResultItem<T>> Results { get; set; }
    }
}