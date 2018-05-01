using AutoMapper;
using DFC.Digital.Core;
using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DFC.Digital.Service.Import
{
    public class LegacyJobProfileImportService : IContentImportService<JobProfile>
    {
        private readonly IOdataApiService<LegacyJobProfile> legacyOdataApi;

        private readonly IODataQueryBuilder queryBuilder;
        private readonly IImportInputParser<LegacyJobProfile> inputParser;
        private readonly IMapper mapper;
        private readonly IJobProfileImportRepository repository;

        public LegacyJobProfileImportService(
            IOdataApiService<LegacyJobProfile> legacyOdataApi,
            IODataQueryBuilder queryBuilder,
            IImportInputParser<LegacyJobProfile> inputParser,
            IMapper mapper,
            IJobProfileImportRepository repository)
        {
            this.legacyOdataApi = legacyOdataApi;
            this.queryBuilder = queryBuilder;
            this.inputParser = inputParser;
            this.mapper = mapper;
            this.repository = repository;
        }

        public async Task ImportAsync(ImportConfiguration importConfig)
        {
            var profilesToImport = inputParser.Parse(importConfig.Content);

            //https://dev.nationalcareersservice.org.uk/api/testjp/jobprofiledisplays?$orderby=Title&$expand=RelatedCareers(%24select%3DUrlName)&$select=UrlName&$filter=UrlName%20eq%20%27archivist%27%20or%20UrlName%20eq%20%27lj-test-profile%27%20or%20UrlName%20eq%20%27make-up-artist%27%20or%20UrlName%20eq%20%27motor-mechanic%27

            //build filters
            queryBuilder.Expand(nameof(LegacyJobProfile.RelatedCareers), new string[] { nameof(LegacyJobProfile.UrlName) });
            queryBuilder.AddFilter(nameof(LegacyJobProfile.UrlName), profilesToImport.Select(p => p.UrlName), OdataFilterOperator.Or);
            queryBuilder.OrderBy(nameof(LegacyJobProfile.UrlName));

            //get all
            var importedProfiles = await legacyOdataApi.GetAllAsync(queryBuilder.GetUri());
            var profilesToCreate = mapper.Map<IEnumerable<JobProfile>>(importedProfiles);

            //create jp
            var createdProfiles = await repository.ImportAsync(profilesToCreate, importConfig.CanUpdate, importConfig.ShouldBePublished);
            foreach (var profile in importedProfiles)
            {
                await repository.UpdateRelatedCareersAsync(profile.UrlName, profile.RelatedCareers, importConfig.ShouldBePublished);
            }
        }
    }
}