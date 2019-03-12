using DFC.Digital.Core;
using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using Microsoft.Azure.Documents;
using Newtonsoft.Json;
using System;

namespace DFC.Digital.Repository.CosmosDb
{
    public class EmailAuditRepository : CosmosDbRepository, IAuditEmailRepository
    {
        private readonly Guid correlationId;
        private readonly IMergeEmailContent<ContactAdvisorRequest> mergeEmailContentService;
        private readonly IConfigurationProvider configuration;

        public EmailAuditRepository(
            IConfigurationProvider configuration,
            IDocumentClient documentClient,
            IMergeEmailContent<ContactAdvisorRequest> mergeEmailContentService) : base(documentClient)
        {
            this.correlationId = Guid.NewGuid();
            this.mergeEmailContentService = mergeEmailContentService;
            this.configuration = configuration;
        }

        public void CreateAudit(object record)
        {
            Add(new Audit
            {
                CorrelationId = correlationId,
                Data = record,
                Timestamp = DateTime.Now
            });
        }

        public void AuditContactAdvisorEmailData(ContactAdvisorRequest emailRequest, EmailTemplate emailTemplate, SendEmailResponse response)
        {
            try
            {
                var safeRequestSerialized = JsonConvert.SerializeObject(emailRequest);

                var safeRequest = JsonConvert.DeserializeObject<ContactAdvisorRequest>(safeRequestSerialized);

                var emailContent =
                    mergeEmailContentService.MergeTemplateBodyWithContentWithHtml(safeRequest, emailTemplate.Body);

                var record = new EmailAuditRecord
                {
                    ContactAdvisorRequest = safeRequest,
                    EmailContent = emailContent,
                    SendEmailResponse = response,
                    EmailTemplate = emailTemplate
                };

                var json = JsonConvert.SerializeObject(record);

                Add(new Audit
                {
                    CorrelationId = correlationId,
                    Data = record,
                    Timestamp = DateTime.Now
                });
            }
            catch (Exception exception)
            {
                Add(new Audit
                {
                    CorrelationId = correlationId,
                    Data = new EmailAuditRecord
                    {
                        ContactAdvisorRequest = emailRequest,
                        Exception = exception,
                        SendEmailResponse = response
                    },
                    Timestamp = DateTime.Now
                });
            }
        }

        [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
        internal override void Initialise()
        {
            Database = configuration.GetConfig<string>(Constants.CosmosDbName);
            DocumentCollection = configuration.GetConfig<string>(Constants.EmailDocumentCollection);
        }
    }
}
