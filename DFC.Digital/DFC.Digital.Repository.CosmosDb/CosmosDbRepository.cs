using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace DFC.Digital.Repository.CosmosDb
{
    public abstract class CosmosDbRepository : IRepository<Audit>
    {
        private readonly IDocumentClient documentClient;

        public CosmosDbRepository(IDocumentClient documentClient)
        {
            Initialise();
            this.documentClient = documentClient;
        }

        public string DocumentCollection { get; set; }

        public string Database { get; set; }

        public void Add(Audit entity)
        {
            documentClient.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri(Database, DocumentCollection), entity);
        }

        public void Delete(Audit entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(Expression<Func<Audit, bool>> where)
        {
            throw new NotImplementedException();
        }

        public Audit Get(Expression<Func<Audit, bool>> where)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Audit> GetAll()
        {
            throw new NotImplementedException();
        }

        public Audit GetById(string id)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Audit> GetMany(Expression<Func<Audit, bool>> where)
        {
            throw new NotImplementedException();
        }

        public void Update(Audit entity)
        {
            throw new NotImplementedException();
        }

        protected abstract void Initialise();
    }
}