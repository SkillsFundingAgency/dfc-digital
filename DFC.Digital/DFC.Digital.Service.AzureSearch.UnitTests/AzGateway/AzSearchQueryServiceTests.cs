using DFC.Digital.AutomationTest.Utilities;
using DFC.Digital.Core;
using DFC.Digital.Core.Configuration;
using DFC.Digital.Data.Model;
using FakeItEasy;
using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;
using Microsoft.Rest.Azure;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace DFC.Digital.Service.AzureSearch.UnitTests
{
    public class AzSearchQueryServiceTests
    {
        [Fact]
        public async Task SearchTestAsync()
        {
            //Arrange
            var fakeIndexClient = A.Fake<ISearchIndexClient>(o => o.Strict());
            var fakeQueryConverter = A.Fake<IAzSearchQueryConverter>(o => o.Strict());
            var fakeDocumentsOperation = A.Fake<IDocumentsOperations>();
            var dummySearchProperty = A.Dummy<SearchProperties>();
            var dummySearchParameters = A.Dummy<SearchParameters>();
            var dummySearchResult = A.Dummy<Data.Model.SearchResult<JobProfileIndex>>();
            var fakeLogger = A.Fake<IApplicationLogger>();

            //Configure
            A.CallTo(() => fakeQueryConverter.BuildSearchParameters(A<SearchProperties>._)).Returns(dummySearchParameters);
            A.CallTo(() => fakeIndexClient.Documents).Returns(fakeDocumentsOperation);
            A.CallTo(() => fakeQueryConverter.ConvertToSearchResult(A<DocumentSearchResult<JobProfileIndex>>._, A<SearchProperties>._)).Returns(dummySearchResult);

            //Act
            var searchService = new AzSearchQueryService<JobProfileIndex>(fakeIndexClient, fakeQueryConverter, fakeLogger);
            await searchService.SearchAsync("searchTerm", dummySearchProperty);

            //Assert
            A.CallTo(() => fakeQueryConverter.BuildSearchParameters(A<SearchProperties>._)).MustHaveHappened();
            A.CallTo(() => fakeIndexClient.Documents).MustHaveHappened();
            A.CallTo(() => fakeDocumentsOperation.SearchWithHttpMessagesAsync<JobProfileIndex>(A<string>._, A<SearchParameters>._, A<SearchRequestOptions>._, A<Dictionary<string, List<string>>>._, A<CancellationToken>._)).MustHaveHappened();
            A.CallTo(() => fakeQueryConverter.ConvertToSearchResult(A<DocumentSearchResult<JobProfileIndex>>._, A<SearchProperties>._)).MustHaveHappened();
        }

        [Fact]
        public void GetSuggestionTest()
        {
            var fakeDocuments = A.Fake<IDocumentsOperations>();
            var fakeIndexClient = A.Fake<ISearchIndexClient>();
            var fakeQueryConverter = A.Fake<IAzSearchQueryConverter>();
            var fakeLogger = A.Fake<IApplicationLogger>(ops => ops.Strict());
            var suggestParameters = new SuggestParameters { UseFuzzyMatching = true, Top = null };
            var azResponse = new AzureOperationResponse<DocumentSuggestResult<JobProfileIndex>>
            {
                Body = new DocumentSuggestResult<JobProfileIndex>
                {
                    Coverage = 1,
                    Results = new List<SuggestResult<JobProfileIndex>>
                    {
                        new SuggestResult<JobProfileIndex>
                        {
                            Document = DummyJobProfileIndex.GenerateJobProfileIndexDummy("one"),
                            Text = "one",
                        },
                        new SuggestResult<JobProfileIndex>
                        {
                            Document = DummyJobProfileIndex.GenerateJobProfileIndexDummy("two"),
                            Text = "two",
                        }
                    }
                }
            };

            A.CallTo(() => fakeQueryConverter.BuildSuggestParameters(A<SuggestProperties>._)).Returns(suggestParameters);
            A.CallTo(() => fakeDocuments.SuggestWithHttpMessagesAsync<JobProfileIndex>(A<string>._, A<string>._, A<SuggestParameters>._, A<SearchRequestOptions>._, A<Dictionary<string, List<string>>>._, A<CancellationToken>._))
                .Returns(azResponse);
            A.CallTo(() => fakeIndexClient.Documents).Returns(fakeDocuments);

            // Act
            var searchService = new AzSearchQueryService<JobProfileIndex>(fakeIndexClient, fakeQueryConverter, fakeLogger);
            searchService.GetSuggestion("searchTerm", new SuggestProperties { MaxResultCount = 20, UseFuzzyMatching = true });

            A.CallTo(() => fakeQueryConverter.BuildSuggestParameters(A<SuggestProperties>._)).MustHaveHappened();
            A.CallTo(() => fakeIndexClient.Documents).MustHaveHappened();
            A.CallTo(() => fakeDocuments.SuggestWithHttpMessagesAsync<JobProfileIndex>(A<string>._, A<string>._, A<SuggestParameters>._, A<SearchRequestOptions>._, A<Dictionary<string, List<string>>>._, A<CancellationToken>._)).MustHaveHappened();
            A.CallTo(() => fakeQueryConverter.ConvertToSuggestionResult(A<DocumentSuggestResult<JobProfileIndex>>._, A<SuggestProperties>._)).MustHaveHappened();
        }

        [Fact]
        public async Task GetSuggestionAsyncTest()
        {
            var fakeDocuments = A.Fake<IDocumentsOperations>();
            var fakeIndexClient = A.Fake<ISearchIndexClient>();
            var fakeQueryConverter = A.Fake<IAzSearchQueryConverter>();
            var fakeLogger = A.Fake<IApplicationLogger>(ops => ops.Strict());
            var suggestParameters = new SuggestParameters { UseFuzzyMatching = true, Top = null };
            var azResponse = new AzureOperationResponse<DocumentSuggestResult<JobProfileIndex>>
            {
                Body = new DocumentSuggestResult<JobProfileIndex>
                {
                    Coverage = 1,
                    Results = new List<SuggestResult<JobProfileIndex>>
                    {
                        new SuggestResult<JobProfileIndex>
                        {
                            Document = DummyJobProfileIndex.GenerateJobProfileIndexDummy("one"),
                            Text = "one",
                        },
                        new SuggestResult<JobProfileIndex>
                        {
                            Document = DummyJobProfileIndex.GenerateJobProfileIndexDummy("two"),
                            Text = "two",
                        }
                    }
                }
            };

            A.CallTo(() => fakeQueryConverter.BuildSuggestParameters(A<SuggestProperties>._)).Returns(suggestParameters);
            A.CallTo(() => fakeDocuments.SuggestWithHttpMessagesAsync<JobProfileIndex>(A<string>._, A<string>._, A<SuggestParameters>._, A<SearchRequestOptions>._, A<Dictionary<string, List<string>>>._, A<CancellationToken>._))
                .Returns(azResponse);
            A.CallTo(() => fakeIndexClient.Documents).Returns(fakeDocuments);

            // Act
            var searchService = new AzSearchQueryService<JobProfileIndex>(fakeIndexClient, fakeQueryConverter, fakeLogger);
            await searchService.GetSuggestionAsync("searchTerm", new SuggestProperties { MaxResultCount = 20, UseFuzzyMatching = true });

            A.CallTo(() => fakeQueryConverter.BuildSuggestParameters(A<SuggestProperties>._)).MustHaveHappened();
            A.CallTo(() => fakeIndexClient.Documents).MustHaveHappened();
            A.CallTo(() => fakeDocuments.SuggestWithHttpMessagesAsync<JobProfileIndex>(A<string>._, A<string>._, A<SuggestParameters>._, A<SearchRequestOptions>._, A<Dictionary<string, List<string>>>._, A<CancellationToken>._)).MustHaveHappened();
            A.CallTo(() => fakeQueryConverter.ConvertToSuggestionResult(A<DocumentSuggestResult<JobProfileIndex>>._, A<SuggestProperties>._)).MustHaveHappened();
        }
    }
}