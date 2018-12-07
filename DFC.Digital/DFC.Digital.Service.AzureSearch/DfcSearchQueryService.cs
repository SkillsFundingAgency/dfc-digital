using DFC.Digital.Core;
using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using Microsoft.Azure.Search;
using System.Linq;
using System.Threading.Tasks;

namespace DFC.Digital.Service.AzureSearch
{
    public class DfcSearchQueryService<T> : AzSearchQueryService<T>, ISearchQueryService<T>
        where T : class
    {
        private ISearchQueryBuilder queryBuilder;

        public DfcSearchQueryService(ISearchIndexClient indexClient, IAzSearchQueryConverter queryConverter, ISearchQueryBuilder queryBuilder, IApplicationLogger applicationLogger) : base(indexClient, queryConverter, applicationLogger)
        {
            this.queryBuilder = queryBuilder;
        }

        public override SearchResult<T> Search(string searchTerm, SearchProperties properties)
        {
            var cleanedSearchTerm = queryBuilder.RemoveSpecialCharactersFromTheSearchTerm(searchTerm, properties);
            var trimmedSearchTerm = queryBuilder.TrimCommonWordsAndSuffixes(cleanedSearchTerm, properties);
            var partialTermToSearch = queryBuilder.BuildContainPartialSearch(trimmedSearchTerm, properties);
            var res = base.Search($"{cleanedSearchTerm} {partialTermToSearch}", properties ?? new SearchProperties());

            return new SearchResult<T>
            {
                Count = res?.Count ?? 0,
                Results = res?.Results ?? Enumerable.Empty<SearchResultItem<T>>(),
                ComputedSearchTerm = res?.ComputedSearchTerm,
                Coverage = res?.Coverage
            };
        }

        public override async Task<SearchResult<T>> SearchAsync(string searchTerm, SearchProperties properties)
        {
            var cleanedSearchTerm = queryBuilder.RemoveSpecialCharactersFromTheSearchTerm(searchTerm, properties);
            var trimmedSearchTerm = queryBuilder.TrimCommonWordsAndSuffixes(cleanedSearchTerm, properties);
            var partialTermToSearch = queryBuilder.BuildContainPartialSearch(trimmedSearchTerm, properties);
            var res = await base.SearchAsync($"{cleanedSearchTerm} {partialTermToSearch}", properties ?? new SearchProperties());

            return new SearchResult<T>
            {
                Count = res?.Count ?? 0,
                Results = res?.Results ?? Enumerable.Empty<SearchResultItem<T>>(),
                ComputedSearchTerm = res?.ComputedSearchTerm,
                Coverage = res?.Coverage
            };
        }
    }
}