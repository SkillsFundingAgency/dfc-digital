﻿using DFC.Digital.Data.Interfaces;
using System.Collections.Generic;

namespace DFC.Digital.Data.Model
{
    public class SearchResult<T> : IDigitalDataModel
        where T : class
    {
        public IEnumerable<SearchResultItem<T>> Results { get; set; }

        public long? Count { get; set; }

        public double? Coverage { get; set; }

        public string ComputedSearchTerm { get; set; }

        public string SearchParametersQueryString { get; set; }
    }
}