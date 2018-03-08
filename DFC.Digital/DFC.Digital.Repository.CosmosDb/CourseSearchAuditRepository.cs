using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using Microsoft.Azure.Documents;
using System;
using System.Configuration;

namespace DFC.Digital.Repository.CosmosDb
{
    public class CourseSearchAuditRepository : CosmosDbRepository, IAuditRepository
    {
        private readonly Guid correlationId;

        public CourseSearchAuditRepository(IDocumentClient documentClient) : base(documentClient)
        {
            this.correlationId = Guid.NewGuid();
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

        protected override void Initialise()
        {
            Database = ConfigurationManager.AppSettings.Get("DFC.Digital.CourseSearchAudit.Db");
            DocumentCollection = ConfigurationManager.AppSettings.Get("DFC.Digital.CourseSearchAudit.Collection");
        }
    }
 }