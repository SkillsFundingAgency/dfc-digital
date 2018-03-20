using DFC.Digital.Core;
using DFC.Digital.Core.Configuration;
using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using FakeItEasy;
using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace DFC.Digital.Service.AzureSearch.UnitTests
{
    public class DfcSearchQueryServiceTests
    {
        [Fact]
        public async Task SearchAsyncTest()
        {
            //Arrange
            var fakeIndexClient = A.Fake<ISearchIndexClient>(o => o.Strict());
            var fakeQueryBuilder = A.Fake<ISearchQueryBuilder>(o => o.Strict());
            var fakeQueryConverter = A.Fake<IAzSearchQueryConverter>(o => o.Strict());
            var fakeDocumentsOperation = A.Fake<IDocumentsOperations>();
            var dummySearchProperty = A.Dummy<SearchProperties>();
            var dummySearchParameters = A.Dummy<SearchParameters>();
            var dummySearchResult = A.Dummy<Data.Model.SearchResult<JobProfileIndex>>();
            var fakeLogger = A.Fake<IApplicationLogger>();
            var policy = new TolerancePolicy(fakeLogger, new TransientFaultHandlingStrategy(new InMemoryConfigurationProvider()));

            //Configure
            A.CallTo(() => fakeQueryBuilder.RemoveSpecialCharactersFromTheSearchTerm(A<string>._, A<SearchProperties>._)).Returns("cleanedSearchTerm");
            A.CallTo(() => fakeQueryBuilder.BuildContainPartialSearch(A<string>._, A<SearchProperties>._)).Returns("partialTermToSearch");
            A.CallTo(() => fakeQueryConverter.BuildSearchParameters(A<SearchProperties>._)).Returns(dummySearchParameters);
            A.CallTo(() => fakeIndexClient.Documents).Returns(fakeDocumentsOperation);
            A.CallTo(() => fakeQueryConverter.ConvertToSearchResult(A<DocumentSearchResult<JobProfileIndex>>._, A<SearchProperties>._)).Returns(dummySearchResult);

            //Act
            var searchService = new DfcSearchQueryService<JobProfileIndex>(fakeIndexClient, fakeQueryConverter, fakeQueryBuilder, fakeLogger);
            await searchService.SearchAsync("searchTerm", dummySearchProperty);

            //Assert
            A.CallTo(() => fakeQueryBuilder.RemoveSpecialCharactersFromTheSearchTerm(A<string>._, A<SearchProperties>._)).MustHaveHappened();
            A.CallTo(() => fakeQueryBuilder.BuildContainPartialSearch(A<string>._, A<SearchProperties>._)).MustHaveHappened();
            A.CallTo(() => fakeQueryConverter.BuildSearchParameters(A<SearchProperties>._)).MustHaveHappened();
            A.CallTo(() => fakeIndexClient.Documents).MustHaveHappened();
            A.CallTo(() => fakeDocumentsOperation.SearchWithHttpMessagesAsync<JobProfileIndex>(A<string>._, A<SearchParameters>._, A<SearchRequestOptions>._, A<Dictionary<string, List<string>>>._, A<CancellationToken>._)).MustHaveHappened();
            A.CallTo(() => fakeQueryConverter.ConvertToSearchResult(A<DocumentSearchResult<JobProfileIndex>>._, A<SearchProperties>._)).MustHaveHappened();
        }
    }
}