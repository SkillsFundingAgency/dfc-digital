using DFC.Digital.AutomationTest.Utilities;
using DFC.Digital.Core;
using DFC.Digital.Core.Configuration;
using DFC.Digital.Data.Model;
using FakeItEasy;
using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;
using Microsoft.Rest.Azure;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace DFC.Digital.Service.AzureSearch.UnitTests
{
    public class AzSearchServiceTests
    {
        //Fakes
        private readonly ISuggesterBuilder fakeSuggesterBuilder = A.Fake<ISuggesterBuilder>();

        private readonly ISearchIndexClient fakeIndexClient = A.Fake<ISearchIndexClient>();
        private readonly ISearchServiceClient fakeSearchClient = A.Fake<ISearchServiceClient>();
        private readonly IIndexesOperations fakeIndexes = A.Fake<IIndexesOperations>();
        private readonly IDocumentsOperations fakeDocuments = A.Fake<IDocumentsOperations>();
        private IApplicationLogger fakeLogger = A.Fake<IApplicationLogger>();
        private ITolerancePolicy policy;
        private TransientFaultHandlingStrategy strategy = new TransientFaultHandlingStrategy(new InMemoryConfigurationProvider());

        public AzSearchServiceTests()
        {
            policy = new TolerancePolicy(fakeLogger, strategy);
        }

        [Fact]
        public async Task EnsureIndexTestAsync()
        {
            //Arrange or configure
            A.CallTo(() => fakeSearchClient.Indexes).Returns(fakeIndexes);

            var azSearchService = new AzSearchService<JobProfileIndex>(fakeSearchClient, fakeIndexClient, fakeSuggesterBuilder, policy);
            await azSearchService.EnsureIndexAsync("test");

            A.CallTo(() => fakeSuggesterBuilder.BuildForType<JobProfileIndex>()).MustHaveHappened();
            A.CallTo(() => fakeSearchClient.Indexes).MustHaveHappened();
            A.CallTo(() => fakeIndexes.CreateOrUpdateWithHttpMessagesAsync(A<Index>.That.Matches(i => i.Name.Equals("test") && i.Suggesters.Count == 1 && i.Suggesters.First().Name == Constants.DefaultSuggesterName), A<bool?>._, A<SearchRequestOptions>._, A<AccessCondition>._, A<Dictionary<string, List<string>>>._, A<CancellationToken>._)).MustHaveHappened();
        }

        [Fact]
        public void DeleteIndexTest()
        {
            //Arrange or configure
            var azOpResponse = new AzureOperationResponse<bool>
            {
                Body = true
            };
            A.CallTo(() => fakeSearchClient.Indexes).Returns(fakeIndexes);
            A.CallTo(() => fakeIndexes.ExistsWithHttpMessagesAsync(
                A<string>._, A<SearchRequestOptions>._, A<Dictionary<string, List<string>>>._, A<CancellationToken>._))
                .Returns(azOpResponse);

            var azSearchService = new AzSearchService<JobProfileIndex>(fakeSearchClient, fakeIndexClient, fakeSuggesterBuilder, policy);
            azSearchService.DeleteIndex("test");

            A.CallTo(() => fakeSearchClient.Indexes).MustHaveHappened();
            A.CallTo(() => fakeIndexes.ExistsWithHttpMessagesAsync(
                A<string>.That.IsEqualTo("test"), A<SearchRequestOptions>._, A<Dictionary<string, List<string>>>._, A<CancellationToken>._)).MustHaveHappened();

            A.CallTo(() => fakeIndexes.DeleteWithHttpMessagesAsync(
                A<string>.That.IsEqualTo("test"), A<SearchRequestOptions>._, A<AccessCondition>._, A<Dictionary<string, List<string>>>._, A<CancellationToken>._)).MustHaveHappened();
        }

        [Theory]
        [InlineData(201)]
        [InlineData(500)]
        public async Task PopulateIndexAsyncTest(int statusCode)
        {
            //Arrange or configure
            var dummyCollectionOfData = DummyJobProfileIndex.GenerateJobProfileIndexDummyCollection("test", 5);
            A.Dummy<IndexBatch<JobProfileIndex>>();
            var azOpResponse = new AzureOperationResponse<DocumentIndexResult>
            {
                Body = new DocumentIndexResult(new List<IndexingResult>
                {
                    new IndexingResult(statusCode: statusCode, succeeded: statusCode < 400)
                })
            };
            A.CallTo(() => fakeIndexClient.Documents).Returns(fakeDocuments);
            A.CallTo(() => fakeDocuments.IndexWithHttpMessagesAsync(
                A<IndexBatch<JobProfileIndex>>._, A<SearchRequestOptions>._, A<Dictionary<string, List<string>>>._, A<CancellationToken>._))
                .Returns(azOpResponse);

            var azSearchService = new AzSearchService<JobProfileIndex>(fakeSearchClient, fakeIndexClient, fakeSuggesterBuilder, policy);
            await azSearchService.PopulateIndexAsync(dummyCollectionOfData);

            A.CallTo(() => fakeIndexClient.Documents).MustHaveHappened();

            A.CallTo(() => fakeDocuments.IndexWithHttpMessagesAsync(
                    A<IndexBatch<JobProfileIndex>>._,
                    A<SearchRequestOptions>._,
                    A<Dictionary<string, List<string>>>._,
                    A<CancellationToken>._))
                .MustHaveHappened(statusCode > 400 ? Repeated.Exactly.Times(strategy.Retry + 1) : Repeated.Exactly.Once);
        }
    }
}