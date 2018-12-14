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
        private readonly ISearchQueryBuilder queryBuilder;
        private readonly ISearchResultsManipulator<T> searchResultsManipulator;

        public DfcSearchQueryService(
            ISearchIndexClient indexClient,
            IAzSearchQueryConverter queryConverter,
            ISearchQueryBuilder queryBuilder,
            ISearchResultsManipulator<T> searchResultsManipulator,
            IApplicationLogger applicationLogger)
            : base(indexClient, queryConverter, applicationLogger)
        {
            this.queryBuilder = queryBuilder;
            this.searchResultsManipulator = searchResultsManipulator;
        }

        public override SearchResult<T> Search(string searchTerm, SearchProperties properties)
        {
            var cleanedSearchTerm = queryBuilder.RemoveSpecialCharactersFromTheSearchTerm(searchTerm, properties);
            var partialTermToSearch = queryBuilder.BuildContainPartialSearch(cleanedSearchTerm, properties);
            var res = base.Search(partialTermToSearch, properties ?? new SearchProperties());
            var orderedResult = searchResultsManipulator.Reorder(res, searchTerm, properties);

            return orderedResult;
        }

        public override async Task<SearchResult<T>> SearchAsync(string searchTerm, SearchProperties properties)
        {
            var cleanedSearchTerm = queryBuilder.RemoveSpecialCharactersFromTheSearchTerm(searchTerm, properties);
            var partialTermToSearch = queryBuilder.BuildContainPartialSearch(cleanedSearchTerm, properties);
            var res = await base.SearchAsync(partialTermToSearch, properties ?? new SearchProperties());
            var orderedResult = searchResultsManipulator.Reorder(res, searchTerm, properties);

            return orderedResult;
        }
    }
}