using DFC.Digital.Data.Interfaces;

namespace DFC.Digital.Data.Model
{
    public class SearchResultItem<T> : IDigitalDataModel
        where T : class
    {
        public double Rank { get; set; }

        public T ResultItem { get; set; }
    }
}