using DFC.Digital.Core.Utilities;
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
        private readonly ISearchIndexClient indexClient;
        private readonly ISearchServiceClient searchClient;

        #endregion Fields

        #region Ctor

        public AzSearchService(ISearchServiceClient searchClient, ISearchIndexClient indexClient, ISuggesterBuilder suggesterBuilder)
        {
            this.searchClient = searchClient;
            this.indexClient = indexClient;
            this.suggesterBuilder = suggesterBuilder;
        }

        #endregion Ctor

        #region Implementations

        public void EnsureIndex(string indexName)
        {
            Index definition = GetIndexDefenition(indexName);
            searchClient.Indexes.CreateOrUpdate(definition);
        }

        public async Task EnsureIndexAsync(string indexName)
        {
            Index definition = GetIndexDefenition(indexName);
            await searchClient.Indexes.CreateOrUpdateAsync(definition);
        }

        public void DeleteIndex(string indexName)
        {
            if (searchClient.Indexes.Exists(indexName))
            {
                searchClient.Indexes.Delete(indexName);
            }
        }

        public void PopulateIndex(IEnumerable<T> data)
        {
            var batch = IndexBatch.Upload(data);
            var result = indexClient.Documents.Index(batch);

            //fault tolerance - we should look for an alternative library for this
            for (int i = 0; i < 3; i++)
            {
                if (result.Results.Any(r => r.Succeeded == false || r.StatusCode > 400))
                {
                    result = indexClient.Documents.Index(batch);
                }
                else
                {
                    break;
                }
            }
        }

        public async Task PopulateIndexAsync(IEnumerable<T> data)
        {
            var batch = IndexBatch.Upload(data);
            var result = await indexClient.Documents.IndexAsync(batch);

            //fault tolerance - we should look for an alternative library for this
            for (int i = 0; i < 3; i++)
            {
                if (result.Results.Any(r => r.Succeeded == false || r.StatusCode > 400))
                {
                    result = await indexClient.Documents.IndexAsync(batch);
                }
                else
                {
                    break;
                }
            }
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