using DFC.Digital.Data.Attributes;

namespace DFC.Digital.Service.AzureSearch.UnitTests
{
    public abstract class TestSuggestBuilder
    {
        [IsSuggestible]
        public int PropertyOne { get; set; }

        public int PropertyTwo { get; set; }

        [IsSuggestible]
        public int PropertyThree { get; set; }
    }
}