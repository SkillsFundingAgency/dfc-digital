using DFC.Digital.Core;
using DFC.Digital.Data.Interfaces;
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
    public class EmailAuditRepositoryTests
    {
        private readonly IMergeEmailContent<ContactUsRequest> fakeMergeEmailContentService;
        private readonly IConfigurationProvider fakeConfiguration;
        private readonly IDocumentClient fakeDocumentClient;

        public EmailAuditRepositoryTests()
        {
            fakeDocumentClient = A.Fake<IDocumentClient>(ops => ops.Strict());
            fakeConfiguration = A.Fake<IConfigurationProvider>(ops => ops.Strict());
            fakeMergeEmailContentService = A.Fake<IMergeEmailContent<ContactUsRequest>>(ops => ops.Strict());
            SetupCalls();
        }

        [Fact]
        public async Task CreateAuditTest()
        {
            //Arrange
            var fakeContactUsRequest = new ContactUsRequest();
            var fakeEmailTemplate = new EmailTemplate();
            var fakeResponse = new SendEmailResponse();

            var repo = new EmailAuditRepository<ContactUsRequest>(fakeConfiguration, fakeDocumentClient, fakeMergeEmailContentService)
            {
                Database = "db",
                DocumentCollection = "docCollection"
            };

            //Act
            repo.CreateAudit(fakeContactUsRequest, fakeEmailTemplate, fakeResponse);
            await Task.Delay(10);

            //Assert
            A.CallTo(() => fakeDocumentClient.CreateDocumentAsync(A<Uri>._, A<Audit>._, A<RequestOptions>._, A<bool>._, A<CancellationToken>._)).MustHaveHappened();
            A.CallTo(() => fakeMergeEmailContentService.MergeTemplateBodyWithContent(A<ContactUsRequest>._, A<string>._))
                .MustHaveHappened();
        }

        private void SetupCalls()
        {
            A.CallTo(() => fakeDocumentClient.CreateDocumentAsync(A<Uri>._, A<Audit>._, A<RequestOptions>._, A<bool>._, A<CancellationToken>._)).Returns(new ResourceResponse<Document>());
            A.CallTo(() => fakeMergeEmailContentService.MergeTemplateBodyWithContent(A<ContactUsRequest>._, A<string>._))
                .Returns(nameof(IMergeEmailContent<ContactUsRequest>.MergeTemplateBodyWithContent));
        }
    }
}