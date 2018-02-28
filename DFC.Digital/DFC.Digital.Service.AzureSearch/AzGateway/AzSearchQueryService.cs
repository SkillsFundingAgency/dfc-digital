using DFC.Digital.Core.Utilities;
using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;
using System;
using System.Threading.Tasks;

namespace DFC.Digital.Service.AzureSearch
{
    public class AzSearchQueryService<T> : ISearchQueryService<T>, IServiceStatus
        where T : class
    {
        #region Fields

        private ISearchIndexClient indexClient;
        private IAzSearchQueryConverter queryConverter;

        #endregion Fields

        #region ctor

        public AzSearchQueryService(ISearchIndexClient indexClient, IAzSearchQueryConverter queryConverter)
        {
            this.indexClient = indexClient;
            this.queryConverter = queryConverter;
        }

        #endregion ctor

        #region Implementation

        #region Implement of IServiceStatus
        private string ServiceName => "Search Service";

        public async Task<ServiceStatus> GetCurrentStatusAsync()
        {
            var serviceStatus = new ServiceStatus { Name = ServiceName, Status = ServiceState.Red, Notes = string.Empty };

            var searchTerm = "*";
            serviceStatus.CheckParametersUsed = $"Search term - {searchTerm}";

            try
            {
                SearchParameters searchParam = new SearchParameters() { Top = 5 };
                var result = await indexClient.Documents.SearchAsync<T>(searchTerm, searchParam);

                //The call worked ok
                serviceStatus.Status = ServiceState.Amber;
                serviceStatus.Notes = "Success search with 0 results";

                if (result.Results.Count > 0)
                {
                    serviceStatus.Status = ServiceState.Green;
                    serviceStatus.Notes = string.Empty;
                }
            }
            catch (Exception ex)
            {
                serviceStatus.Notes = $"Exception: {ex.Message}";
            }

            return serviceStatus;
        }

        #endregion

        public virtual Data.Model.SearchResult<T> Search(string searchTerm, SearchProperties properties)
        {
            SearchParameters searchParam = queryConverter.BuildSearchParameters(properties);
            var result = indexClient.Documents.Search<T>(searchTerm, searchParam);
            return queryConverter.ConvertToSearchResult<T>(result, properties);
        }

        public virtual async Task<Data.Model.SearchResult<T>> SearchAsync(string searchTerm, SearchProperties properties)
        {
            SearchParameters searchParam = queryConverter.BuildSearchParameters(properties);
            var result = await indexClient.Documents.SearchAsync<T>(searchTerm, searchParam);
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