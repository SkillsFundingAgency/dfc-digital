using DFC.Digital.Data.Model;
using FakeItEasy;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using System;
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
            var repo = new CourseSearchAuditRepository(fakeDocumentClient);
            repo.Database = "db";
            repo.DocumentCollection = "docCollection";

            //Act
            repo.CreateAudit(auditRecord);
            await Task.Delay(10);

            //Assert
            A.CallTo(() => fakeDocumentClient.CreateDocumentAsync(A<Uri>._, A<Audit>.That.Matches(a => a.Data.ToString() == auditRecord), A<RequestOptions>._, A<bool>._)).MustHaveHappened();
        }
    }
}