using Autofac;
using AutoMapper;
using DFC.Digital.Core;
using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
        private readonly IApplicationLogger applicationLogger;

        public DfcSearchService()
        {
        }

        public DfcSearchService(ISearchService<JobProfileIndex> searchService, ISearchIndexConfig indexConfig, IJobProfileIndexEnhancer jobProfileIndexEnhancer, IAsyncHelper asyncHelper, IMapper mapper, IApplicationLogger applicationLogger)
        {
            this.applicationLogger = applicationLogger;
            this.searchService = searchService;
            this.index = indexConfig?.Name ?? string.Empty;
            this.jobProfileIndexEnhancer = jobProfileIndexEnhancer;
            this.asyncHelper = asyncHelper;
            this.mapper = mapper;
        }

        public override bool IndexExists(string indexName)
        {
            var activeIndex = string.Empty;
            try
            {
                if (!string.IsNullOrEmpty(indexName) && index.StartsWith(indexName, StringComparison.OrdinalIgnoreCase))
                {
                    activeIndex = index;
                    return searchService.IndexExists(index);
                }
                else
                {
                    activeIndex = indexName;
                    return base.IndexExists(indexName);
                }
            }
            catch (Exception exception)
            {
                applicationLogger.Error($" Method - {MethodBase.GetCurrentMethod().Name} on index {activeIndex} failed with an exception", exception);
            }

            return false;
        }

        public override void DeleteIndex(string indexName)
        {
            var activeIndex = string.Empty;
            try
            {
                if (!string.IsNullOrEmpty(indexName) && index.StartsWith(indexName, StringComparison.OrdinalIgnoreCase))
                {
                    activeIndex = index;
                    searchService?.DeleteIndex(index);
                }
                else
                {
                    activeIndex = indexName;
                    base.DeleteIndex(indexName);
                }
            }
            catch (Exception exception)
            {
                applicationLogger.Error($" Method - {MethodBase.GetCurrentMethod().Name} on index {activeIndex} failed with an exception", exception);
            }
        }

        public override void CreateIndex(string name, IEnumerable<IFieldDefinition> fieldDefinitions)
        {
            var activeIndex = string.Empty;
            try
            {
                if (!string.IsNullOrEmpty(name) && index.StartsWith(name, StringComparison.OrdinalIgnoreCase))
                {
                    //TODO: think about mapping from sitefinity field defenitions to domain model?
                    //Or is it correct to keep them in sync anyway?
                    activeIndex = index;
                    asyncHelper.Synchronise(() => searchService?.EnsureIndexAsync(index));
                }
                else
                {
                    activeIndex = name;
                    base.CreateIndex(name, fieldDefinitions);
                }
            }
            catch (Exception exception)
            {
                applicationLogger.Error($" Method - {MethodBase.GetCurrentMethod().Name} on index {activeIndex} failed with an exception", exception);
            }
        }

        public override void UpdateIndex(string indexName, IEnumerable<IDocument> documents)
        {
            var activeIndex = string.Empty;
            try
            {
                if (!string.IsNullOrEmpty(indexName) && index.StartsWith(indexName, StringComparison.OrdinalIgnoreCase))
                {
                    activeIndex = index;
                    var jpIndexDoc = documents.ConvertToJobProfileIndex(jobProfileIndexEnhancer, asyncHelper);

                    //Requires a deep copy to ensure the enumerable is not executed again on a non-ui thread which sitefinity relies upon!!!
                    var copy = mapper.Map<IEnumerable<JobProfileIndex>>(jpIndexDoc);
                    asyncHelper.Synchronise(() => searchService?.PopulateIndexAsync(copy));
                }
                else
                {
                    activeIndex = indexName;
                    base.UpdateIndex(indexName, documents);
                }
            }
            catch (Exception exception)
            {
                applicationLogger.Error($" Method - {MethodBase.GetCurrentMethod().Name} on index {activeIndex} failed with an exception", exception);
            }
        }

        public override void RemoveDocuments(string indexName, IEnumerable<IDocument> documents)
        {
            var activeIndex = string.Empty;
            try
            {
                var allDocuments = documents as IList<IDocument> ?? documents.ToList();
                if (!string.IsNullOrEmpty(indexName) && index.StartsWith(indexName, StringComparison.OrdinalIgnoreCase))
                {
                    // This has been put here to remove the documents from the index which is created via a web.config key after a runbook entry on slot deployment
                    // "index" is the application configured Index name used for slot deployment
                    activeIndex = index;
                    base.RemoveDocuments(index, allDocuments);
                }
                else
                {
                    activeIndex = indexName;
                    base.RemoveDocuments(indexName, allDocuments);
                }
            }
            catch (Exception exception)
            {
               applicationLogger.Error($" Method - {MethodBase.GetCurrentMethod().Name} on index {activeIndex} failed with an exception", exception);
            }
        }
    }
}