using Autofac;
using AutoMapper;
using DFC.Digital.Core;
using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using Telerik.Microsoft.Practices.Unity;
using Telerik.Sitefinity.Abstractions;
using Telerik.Sitefinity.Services.Search;
using Telerik.Sitefinity.Services.Search.Data;

namespace DFC.Digital.Web.Sitefinity.DfcSearchModule
{
    public class DfcSearchService : AzureSearchService
    {
        public const string Name = "DfcSearchService";
        private readonly IMapper mapper;
        private readonly IAsyncHelper asyncHelper;
        private readonly ISearchService<JobProfileIndex> searchService;
        private readonly string index;
        private readonly IJobProfileIndexEnhancer jobProfileIndexEnhancer;

        public DfcSearchService()
        {
        }

        public DfcSearchService(ISearchService<JobProfileIndex> searchService, ISearchIndexConfig indexConfig, IJobProfileIndexEnhancer jobProfileIndexEnhancer, IAsyncHelper asyncHelper, IMapper mapper)
        {
            this.searchService = searchService;
            this.index = indexConfig?.Name ?? string.Empty;
            this.jobProfileIndexEnhancer = jobProfileIndexEnhancer;
            this.asyncHelper = asyncHelper;
            this.mapper = mapper;
        }

        public override bool IndexExists(string indexName)
        {
            if (!string.IsNullOrEmpty(indexName) && index.StartsWith(indexName, StringComparison.OrdinalIgnoreCase))
            {
                return searchService.IndexExists(index);
            }
            else
            {
                return base.IndexExists(indexName);
            }
        }

        public override void DeleteIndex(string indexName)
        {
            if (!string.IsNullOrEmpty(indexName) && index.StartsWith(indexName, StringComparison.OrdinalIgnoreCase))
            {
                searchService?.DeleteIndex(index);
            }
            else
            {
                base.DeleteIndex(indexName);
            }
        }

        public override void CreateIndex(string name, IEnumerable<IFieldDefinition> fieldDefinitions)
        {
            if (!string.IsNullOrEmpty(name) && index.StartsWith(name, StringComparison.OrdinalIgnoreCase))
            {
                //TODO: think about mapping from sitefinity field defenitions to domain model?
                //Or is it correct to keep them in sync anyway?
                asyncHelper.Synchronise(() => searchService?.EnsureIndexAsync(index));
            }
            else
            {
                base.CreateIndex(name, fieldDefinitions);
            }
        }

        public override void UpdateIndex(string indexName, IEnumerable<IDocument> documents)
        {
            if (!string.IsNullOrEmpty(indexName) && index.StartsWith(indexName, StringComparison.OrdinalIgnoreCase))
            {
                var jpIndexDoc = documents.ConvertToJobProfileIndex(jobProfileIndexEnhancer, asyncHelper);

                //Requires a deep copy to ensure the enumerable is not executed again on a non-ui thread which sitefinity relies upon!!!
                var copy = mapper.Map<IEnumerable<JobProfileIndex>>(jpIndexDoc);
                asyncHelper.Synchronise(() => searchService?.PopulateIndexAsync(copy));
            }
            else
            {
                base.UpdateIndex(indexName, documents);
            }
        }

        public override void RemoveDocuments(string indexName, IEnumerable<IDocument> documents)
        {
            var allDocuments = documents as IList<IDocument> ?? documents.ToList();

            // This has been put here to remove the documents from the index which is created via a web.config key after a runbook entry on slot deployment
            // "index" is the application configured Index name used for slot deployment
            base.RemoveDocuments(index, allDocuments);
            base.RemoveDocuments(indexName, allDocuments);
        }
    }
}