﻿using DFC.Digital.Core;
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
        private readonly IApplicationLogger fakeLogger;

        public EmailAuditRepositoryTests()
        {
            fakeDocumentClient = A.Fake<IDocumentClient>(ops => ops.Strict());
            fakeConfiguration = A.Fake<IConfigurationProvider>(ops => ops.Strict());
            fakeMergeEmailContentService = A.Fake<IMergeEmailContent<ContactUsRequest>>(ops => ops.Strict());
            fakeLogger = A.Fake<IApplicationLogger>(ops => ops.Strict());
            SetupCalls();
        }

        [Fact]
        public async Task CreateAuditTest()
        {
            //Arrange
            var dummyContactUsRequest = A.Dummy<ContactUsRequest>();
            var dummyEmailTemplate = A.Dummy<EmailTemplate>();
            var dummyResponse = A.Dummy<SendEmailResponse>();

            var repo = GetEmailRepo();

            //Act
            repo.CreateAudit(dummyContactUsRequest, dummyEmailTemplate, dummyResponse);
            await Task.Delay(10);

            //Assert
            A.CallTo(() => fakeDocumentClient.CreateDocumentAsync(A<Uri>._, A<Audit>._, A<RequestOptions>._, A<bool>._, A<CancellationToken>._)).MustHaveHappened();
            A.CallTo(() => fakeMergeEmailContentService.MergeTemplateBodyWithContent(A<ContactUsRequest>._, A<string>._))
                .MustHaveHappened();
        }

        [Fact]
        public async Task CreateAuditNullInputTest()
        {
            //Arrange
            var repo = GetEmailRepo();

            //Act
            repo.CreateAudit(null, null, null);
            await Task.Delay(10);

            //Assert
            A.CallTo(() => fakeDocumentClient.CreateDocumentAsync(A<Uri>._, A<Audit>._, A<RequestOptions>._, A<bool>._, A<CancellationToken>._)).MustHaveHappened();
            A.CallTo(() => fakeMergeEmailContentService.MergeTemplateBodyWithContent(A<ContactUsRequest>._, A<string>._))
                .MustHaveHappened();
        }

        [Fact]
        public async Task CreateAuditExceptionTest()
        {
            //Arrange
            var dummyContactUsRequest = A.Dummy<ContactUsRequest>();
            var dummyEmailTemplate = A.Dummy<EmailTemplate>();
            var dummyResponse = A.Dummy<SendEmailResponse>();
            var fakeErrorMergeEmailContentService = A.Fake<IMergeEmailContent<ContactUsRequest>>();
            var repo = new EmailAuditRepository<ContactUsRequest>(fakeConfiguration, fakeDocumentClient, fakeErrorMergeEmailContentService, fakeLogger)
            {
                Database = "db",
                DocumentCollection = "docCollection"
            };

            A.CallTo(() => fakeErrorMergeEmailContentService.MergeTemplateBodyWithContent(A<ContactUsRequest>._, A<string>._)).Throws<NullReferenceException>();
            A.CallTo(() => fakeLogger.ErrorJustLogIt(A<string>._, A<Exception>._)).DoesNothing();

            //Act
            repo.CreateAudit(dummyContactUsRequest, dummyEmailTemplate, dummyResponse);
            await Task.Delay(10);

            //Assert
            A.CallTo(() => fakeDocumentClient.CreateDocumentAsync(A<Uri>._, A<Audit>.That.Matches(m => ((EmailAuditRecord<ContactUsRequest>)m.Data).Exception is NullReferenceException), A<RequestOptions>._, A<bool>._, A<CancellationToken>._)).MustHaveHappened();
            A.CallTo(() => fakeLogger.ErrorJustLogIt(A<string>._, A<Exception>._)).MustHaveHappened();
        }

        [Fact]
        public void InitialiseTest()
        {
            //Arrange
            A.CallTo(() => fakeConfiguration.GetConfig<string>(Constants.CosmosDbName)).Returns(Constants.CosmosDbName);
            A.CallTo(() => fakeConfiguration.GetConfig<string>(Constants.EmailDocumentCollection)).Returns(Constants.EmailDocumentCollection);

            //Act
            var repo = GetEmailRepo();
            repo.Initialise();

            //Assert
            A.CallTo(() => fakeConfiguration.GetConfig<string>(Constants.CosmosDbName)).MustHaveHappened();
            A.CallTo(() => fakeConfiguration.GetConfig<string>(Constants.EmailDocumentCollection)).MustHaveHappened();
        }

        private void SetupCalls()
        {
            A.CallTo(() => fakeDocumentClient.CreateDocumentAsync(A<Uri>._, A<Audit>._, A<RequestOptions>._, A<bool>._, A<CancellationToken>._)).Returns(new ResourceResponse<Document>());
            A.CallTo(() => fakeMergeEmailContentService.MergeTemplateBodyWithContent(A<ContactUsRequest>._, A<string>._))
                .Returns(nameof(IMergeEmailContent<ContactUsRequest>.MergeTemplateBodyWithContent));
        }

        private EmailAuditRepository<ContactUsRequest> GetEmailRepo()
        {
            return new EmailAuditRepository<ContactUsRequest>(fakeConfiguration, fakeDocumentClient, fakeMergeEmailContentService, fakeLogger)
            {
                Database = "db",
                DocumentCollection = "docCollection"
            };
        }
    }
}