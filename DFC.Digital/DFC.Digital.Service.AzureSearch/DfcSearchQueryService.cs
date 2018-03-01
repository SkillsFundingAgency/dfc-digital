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

        public DfcSearchQueryService(ISearchIndexClient indexClient, IAzSearchQueryConverter queryConverter, ISearchQueryBuilder queryBuilder, ITolerancePolicy policy, IApplicationLogger applicationLogger) : base(indexClient, queryConverter, policy, applicationLogger)
        {
            this.queryBuilder = queryBuilder;
        }

        public override SearchResult<T> Search(string searchTerm, SearchProperties properties)
        {
            var cleanedSearchTerm = queryBuilder.RemoveSpecialCharactersFromTheSearchTerm(searchTerm, properties);
            var partialTermToSearch = queryBuilder.BuildContainPartialSearch(cleanedSearchTerm, properties);
            var res = base.Search(partialTermToSearch, properties ?? new SearchProperties());

            return new SearchResult<T>
            {
                Count = res?.Count ?? 0,
                Results = res?.Results ?? Enumerable.Empty<SearchResultItem<T>>(),
            };
        }

        public override async Task<SearchResult<T>> SearchAsync(string searchTerm, SearchProperties properties)
        {
            var cleanedSearchTerm = queryBuilder.RemoveSpecialCharactersFromTheSearchTerm(searchTerm, properties);
            var partialTermToSearch = queryBuilder.BuildContainPartialSearch(cleanedSearchTerm, properties);
            var res = await base.SearchAsync(partialTermToSearch, properties ?? new SearchProperties());

            return new SearchResult<T>
            {
                Count = res?.Count ?? 0,
                Results = res?.Results ?? Enumerable.Empty<SearchResultItem<T>>(),
            };
        }
    }
}