﻿using DFC.Digital.Data.Model;

namespace DFC.Digital.Data.Interfaces
{
    public interface ISearchQueryBuilder
    {
        SearchProperties BuildExclusiveExactMatch(string searchTerm, SearchProperties properties);

        SearchProperties BuildExclusivePartialMatch(string searchTerm, SearchProperties properties, int exactMatchCount);

        string BuildContainPartialSearch(string cleanedSearchTerm, SearchProperties properties);

        string BuildExactMatchSearch(string searchTerm);

        string RemoveSpecialCharactersFromTheSearchTerm(string searchTerm, SearchProperties properties);

        string EscapeSpecialCharactersInSearchTerm(string searchTerm, SearchProperties properties);
    }
}