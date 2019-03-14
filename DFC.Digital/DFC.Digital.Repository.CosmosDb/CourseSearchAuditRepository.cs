using DFC.Digital.Core;
using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using Microsoft.Azure.Documents;
using System;

namespace DFC.Digital.Repository.CosmosDb
{
    public class CourseSearchAuditRepository : CosmosDbRepository, IAuditRepository
    {
        private readonly Guid correlationId;
        private readonly IConfigurationProvider configuration;

        public CourseSearchAuditRepository(IDocumentClient documentClient, IConfigurationProvider configuration) : base(documentClient)
        {
            this.correlationId = Guid.NewGuid();
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

        [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
        internal override void Initialise()
        {
            Database = configuration.GetConfig<string>(Constants.CosmosDbName);
            DocumentCollection = configuration.GetConfig<string>(Constants.CourseSearchDocumentCollection);
        }
    }
}