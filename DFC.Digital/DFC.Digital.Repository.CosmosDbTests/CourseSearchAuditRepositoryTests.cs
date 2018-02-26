using DFC.Digital.Data.Model;
using FakeItEasy;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using System;
using Xunit;

namespace DFC.Digital.Repository.CosmosDb.Tests
{
    public class CourseSearchAuditRepositoryTests
    {
        [Fact]
        public void CreateAuditTest()
        {
            //Arrange
            var auditRecord = "audit";
            var fakeDocumentClient = A.Fake<IDocumentClient>();
            var repo = new CourseSearchAuditRepository(fakeDocumentClient);
            repo.Database = "db";
            repo.DocumentCollection = "docCollection";

            //Act
            repo.CreateAudit(auditRecord);

            //Assert
            A.CallTo(() => fakeDocumentClient.CreateDocumentAsync(A<Uri>._, A<Audit>.That.Matches(a => a.Data.ToString() == auditRecord), A<RequestOptions>._, A<bool>._)).MustHaveHappened();
        }
    }
}