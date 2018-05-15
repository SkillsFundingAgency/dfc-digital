using DFC.Digital.Core;
using DFC.Digital.Data.Interfaces;
using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DFC.Digital.Service.AzureSearch
{
    public class AzSearchService<T> : ISearchService<T>
        where T : class
    {
        #region Fields

        private readonly ISuggesterBuilder suggesterBuilder;
        private readonly ITolerancePolicy policy;
        private readonly ISearchIndexClient indexClient;
        private readonly ISearchServiceClient searchClient;

        #endregion Fields

        #region Ctor

        public AzSearchService(ISearchServiceClient searchClient, ISearchIndexClient indexClient, ISuggesterBuilder suggesterBuilder, ITolerancePolicy policy)
        {
            this.searchClient = searchClient;
            this.indexClient = indexClient;
            this.suggesterBuilder = suggesterBuilder;
            this.policy = policy;
        }

        #endregion Ctor

        #region Implementations

        public bool IndexExists(string indexName)
        {
            return searchClient.Indexes.Exists(indexName);
        }

        public async Task EnsureIndexAsync(string indexName)
        {
            var definition = GetIndexDefenition(indexName);
            await policy.ExecuteAsync(() => searchClient.Indexes.CreateOrUpdateAsync(definition), nameof(AzSearchService<T>), FaultToleranceType.RetryWithCircuitBreaker);
        }

        public void DeleteIndex(string indexName)
        {
            if (searchClient.Indexes.Exists(indexName))
            {
                searchClient.Indexes.Delete(indexName);
            }
        }

        public async Task PopulateIndexAsync(IEnumerable<T> data)
        {
            var batch = IndexBatch.Upload(data);
            await policy.ExecuteAsync(
                () => indexClient.Documents.IndexAsync(batch),
                result => result.Results.Any(r => r.Succeeded == false || r.StatusCode > 400),
                nameof(AzSearchService<T>),
                FaultToleranceType.WaitRetry);
        }

        #endregion Implementations

        #region Private helpers

        private Index GetIndexDefenition(string indexName)
        {
            return new Index
            {
                Name = indexName,
                Fields = FieldBuilder.BuildForType<T>(),
                Suggesters = new List<Suggester>
                {
                    new Suggester
                    {
                        Name = Constants.DefaultSuggesterName,
                        SearchMode = SuggesterSearchMode.AnalyzingInfixMatching,
                        SourceFields = suggesterBuilder.BuildForType<T>(),
                    }
                }
            };
        }

        #endregion Private helpers
    }
}