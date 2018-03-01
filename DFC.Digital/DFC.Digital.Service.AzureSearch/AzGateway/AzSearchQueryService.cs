using DFC.Digital.Core;
using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;
using System.Threading.Tasks;

namespace DFC.Digital.Service.AzureSearch
{
    public class AzSearchQueryService<T> : ISearchQueryService<T>
        where T : class
    {
        #region Fields

        private readonly ITolerancePolicy policy;
        private ISearchIndexClient indexClient;
        private IAzSearchQueryConverter queryConverter;

        #endregion Fields

        #region ctor

        public AzSearchQueryService(ISearchIndexClient indexClient, IAzSearchQueryConverter queryConverter, ITolerancePolicy policy)
        {
            this.indexClient = indexClient;
            this.queryConverter = queryConverter;
            this.policy = policy;
        }

        #endregion ctor

        #region Implementation

        public virtual Data.Model.SearchResult<T> Search(string searchTerm, SearchProperties properties)
        {
            SearchParameters searchParam = queryConverter.BuildSearchParameters(properties);
            var result = policy.Execute(() => indexClient.Documents.Search<T>(searchTerm, searchParam), nameof(AzSearchQueryService<T>), FaultToleranceType.Timeout);
            return queryConverter.ConvertToSearchResult<T>(result, properties);
        }

        public virtual async Task<Data.Model.SearchResult<T>> SearchAsync(string searchTerm, SearchProperties properties)
        {
            SearchParameters searchParam = queryConverter.BuildSearchParameters(properties);
            var result = await policy.ExecuteAsync(() => indexClient.Documents.SearchAsync<T>(searchTerm, searchParam), nameof(AzSearchQueryService<T>), FaultToleranceType.Timeout);
            return queryConverter.ConvertToSearchResult<T>(result, properties);
        }

        public virtual SuggestionResult<T> GetSuggestion(string partialTerm, SuggestProperties properties)
        {
            SuggestParameters suggestParameters = queryConverter.BuildSuggestParameters(properties);
            var result = indexClient.Documents.Suggest<T>(partialTerm, Constants.DefaultSuggesterName, suggestParameters);
            return queryConverter.ConvertToSuggestionResult<T>(result, properties);
        }

        public async Task<SuggestionResult<T>> GetSuggestionAsync(string partialTerm, SuggestProperties properties)
        {
            SuggestParameters suggestParameters = queryConverter.BuildSuggestParameters(properties);
            var result = await indexClient.Documents.SuggestAsync<T>(partialTerm, Constants.DefaultSuggesterName, suggestParameters);
            return queryConverter.ConvertToSuggestionResult<T>(result, properties);
        }

        #endregion Implementation
    }
}