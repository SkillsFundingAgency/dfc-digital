using DFC.Digital.Core;
using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using Microsoft.Azure.Documents;
using Newtonsoft.Json;
using System;

namespace DFC.Digital.Repository.CosmosDb
{
    public class EmailAuditRepository<T> : CosmosDbRepository, IAuditNoncitizenEmailRepository<T>
        where T : class
    {
        private readonly Guid correlationId;
        private readonly IMergeEmailContent<T> mergeEmailContentService;
        private readonly IApplicationLogger logger;
        private readonly IConfigurationProvider configuration;

        public EmailAuditRepository(
            IConfigurationProvider configuration,
            IDocumentClient documentClient,
            IMergeEmailContent<T> mergeEmailContentService,
            IApplicationLogger logger) : base(documentClient)
        {
            this.correlationId = Guid.NewGuid();
            this.mergeEmailContentService = mergeEmailContentService;
            this.logger = logger;
            this.configuration = configuration;
        }

        public void CreateAudit(T emailRequest, EmailTemplate emailTemplate, SendEmailResponse response)
        {
            var safeRequestSerialized = JsonConvert.SerializeObject(emailRequest);
            var safeRequest = JsonConvert.DeserializeObject<T>(safeRequestSerialized);

            try
            {
                var emailContent = mergeEmailContentService.MergeTemplateBodyWithContent(safeRequest, emailTemplate?.Body);
                Add(new Audit
                {
                    CorrelationId = correlationId,
                    Data = new EmailAuditRecord<T>
                    {
                        Request = safeRequest,
                        EmailContent = emailContent,
                        SendEmailResponse = response,
                        EmailTemplate = emailTemplate
                    },
                    Timestamp = DateTime.Now
                });
            }
            catch (Exception exception)
            {
                logger.ErrorJustLogIt($"Failed to audit non-citizen email", exception);
                Add(new Audit
                {
                    CorrelationId = correlationId,
                    Data = new EmailAuditRecord<T>
                    {
                        Request = safeRequest,
                        Exception = exception,
                        SendEmailResponse = response,
                        EmailTemplate = emailTemplate
                    },
                    Timestamp = DateTime.Now
                });
            }
        }

        internal override void Initialise()
        {
            Database = configuration.GetConfig<string>(Constants.CosmosDbName);
            DocumentCollection = configuration.GetConfig<string>(Constants.EmailDocumentCollection);
        }
    }
}
