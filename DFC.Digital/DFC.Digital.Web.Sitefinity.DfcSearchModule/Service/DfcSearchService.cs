using Autofac;
using AutoMapper;
using DFC.Digital.Core;
using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using System;
using System.Collections.Generic;
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
        private ISearchService<JobProfileIndex> searchService;
        private ISearchIndexConfig indexConfig;
        private IJobProfileIndexEnhancer jobProfileIndexEnhancer;

        public DfcSearchService()
        {
            if (ObjectFactory.Container.IsRegistered<ILifetimeScope>())
            {
                var autofacContainer = ObjectFactory.Container.Resolve<ILifetimeScope>();
                searchService = autofacContainer.Resolve<ISearchService<JobProfileIndex>>();
                indexConfig = autofacContainer.Resolve<ISearchIndexConfig>();
                jobProfileIndexEnhancer = autofacContainer.Resolve<IJobProfileIndexEnhancer>();
                asyncHelper = autofacContainer.Resolve<IAsyncHelper>();
                mapper = new MapperConfiguration(c => c.CreateMap<JobProfileIndex, JobProfileIndex>()).CreateMapper();
            }
        }

        public DfcSearchService(ISearchService<JobProfileIndex> searchService, ISearchIndexConfig indexConfig, IJobProfileIndexEnhancer jobProfileIndexEnhancer, IAsyncHelper asyncHelper, IMapper mapper)
        {
            this.searchService = searchService;
            this.indexConfig = indexConfig;
            this.jobProfileIndexEnhancer = jobProfileIndexEnhancer;
            this.asyncHelper = asyncHelper;
            this.mapper = mapper;
        }

        public override void CreateIndex(string name, IEnumerable<IFieldDefinition> fieldDefinitions)
        {
            if (!string.IsNullOrEmpty(name) && name.Equals(indexConfig?.Name, StringComparison.OrdinalIgnoreCase))
            {
                //TODO: think about mapping from sitefinity field defenitions to domain model?
                //Or is it correct to keep them in sync anyway?
                asyncHelper.Synchronise(() => this.searchService?.EnsureIndexAsync(name));
            }
            else
            {
                base.CreateIndex(name, fieldDefinitions);
            }
        }

        public override void UpdateIndex(string indexName, IEnumerable<IDocument> documents)
        {
            if (!string.IsNullOrEmpty(indexName) && indexName.Equals(indexConfig?.Name, StringComparison.OrdinalIgnoreCase))
            {
                var jpIndexDoc = documents.ConvertToJobProfileIndex(jobProfileIndexEnhancer, asyncHelper);

                //Requires a deep copy to ensure the enumerable is not executed again on a non-ui thread which sitefinity relies upon!!!
                var copy = mapper.Map<IEnumerable<JobProfileIndex>>(jpIndexDoc);
                asyncHelper.Synchronise(() => this.searchService?.PopulateIndexAsync(copy));
            }
            else
            {
                base.UpdateIndex(indexName, documents);
            }
        }
    }
}