using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using Microsoft.Azure.Search;
using System.Linq;
using System.Threading.Tasks;

namespace DFC.Digital.Service.AzureSearch
{
    public class DfcSearchQueryService<T> : AzSearchQueryService<T>, ISearchQueryService<T>, IServiceStatus
        where T : class
    {
        private ISearchQueryBuilder queryBuilder;

        public DfcSearchQueryService(ISearchIndexClient indexClient, IAzSearchQueryConverter queryConverter, ISearchQueryBuilder queryBuilder) : base(indexClient, queryConverter)
        {
            this.queryBuilder = queryBuilder;
        }

        #region Implement of IServiceStatus
        private string ServiceName => "Search Service";

        public Task<ServiceStatus> GetCurrentStatusAsync()
        {
            return Task.FromResult(new ServiceStatus { Name = ServiceName, Status = ServiceState.Amber, Notes = string.Empty });
        }

        #endregion

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