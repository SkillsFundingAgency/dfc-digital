using DFC.Digital.Core;
using DFC.Digital.Data.Model;
using FakeItEasy;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace DFC.Digital.Repository.CosmosDb.Tests
{
    public class CourseSearchAuditRepositoryTests
    {
        [Fact]
        public async Task CreateAuditTestAsync()
        {
            //Arrange
            var auditRecord = "audit";
            var fakeDocumentClient = A.Fake<IDocumentClient>();
            var fakeConfigurationProvider = A.Fake<IConfigurationProvider>();
            var repo = new CourseSearchAuditRepository(fakeDocumentClient, fakeConfigurationProvider)
            {
                Database = "db",
                DocumentCollection = "docCollection"
            };

            //Act
            repo.CreateAudit(auditRecord);
            await Task.Delay(10);

            //Assert
            A.CallTo(() => fakeDocumentClient.CreateDocumentAsync(A<Uri>._, A<Audit>.That.Matches(a => a.Data.ToString() == auditRecord), A<RequestOptions>._, A<bool>._, A<CancellationToken>._)).MustHaveHappened();
        }
    }
}