using AutoMapper;
using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace DFC.Digital.Repository.SitefinityCMS.Modules
{
    public class JobProfileCategoryRepository : IJobProfileCategoryRepository
    {
        private const string JobprofileTaxonomyName = "job-profile-categories";

        private readonly ISearchQueryService<JobProfileIndex> jobprofileSearchQueryService;
        private readonly IMapper mapper;
        private readonly IHierarchicalTaxonomyRepository taxonomyRepository;

        public JobProfileCategoryRepository(ISearchQueryService<JobProfileIndex> jobprofileSearchQueryService, IMapper mapper, IHierarchicalTaxonomyRepository taxonomyRepository)
        {
            this.jobprofileSearchQueryService = jobprofileSearchQueryService;
            this.mapper = mapper;
            this.taxonomyRepository = taxonomyRepository;
        }

        [SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1118:ParameterMustNotSpanMultipleLines", Justification = "Reviewed.")]
        public static SearchResult<JobProfileIndex> FilterByCategory(string category, ISearchQueryService<JobProfileIndex> searchQueryService)
        {
            if (searchQueryService == null)
            {
                throw new ArgumentNullException(nameof(searchQueryService));
            }

            return searchQueryService.Search(
                    "*",
                    new SearchProperties
                    {
                        UseRawSearchTerm = true,
                        Count = 1000,
                        FilterBy = $"{nameof(JobProfileIndex.JobProfileCategories)}/any(c: c eq '{category}')",
                        OrderByFields = new string[] { nameof(JobProfileIndex.Title) },
                    });
        }

        public IQueryable<JobProfileCategory> GetJobProfileCategories()
        {
            return taxonomyRepository.GetMany(category => category.Taxonomy.Name == JobprofileTaxonomyName).Select(category => new JobProfileCategory
            {
                Name = category.Name,
                Title = category.Title,
                Description = category.Description,
                Url = category.UrlName
            });
        }

        public IEnumerable<JobProfileCategory> GetByIds(IList<Guid> categoryIds)
        {
            var categories = taxonomyRepository.GetMany(c => categoryIds.Contains(c.Id) && c.Taxonomy.Name == JobprofileTaxonomyName);
            foreach (var category in categories)
            {
                yield return new JobProfileCategory
                {
                    Id = category.Id,
                    Name = category.Name,
                    Title = category.Title,
                    Description = category.Description,
                    Url = category.UrlName,
                    Subcategories = taxonomyRepository.GetMany(c => c.Parent.Name == category.Name).Select(q => GetByUrlName(q.UrlName))
                };
            }
        }

        public JobProfileCategory GetByUrlName(string categoryUrlName)
        {
            var category = taxonomyRepository.Get(c => c.UrlName == categoryUrlName && c.Taxonomy.Name == JobprofileTaxonomyName);

            //if not found via the url return
            if (category == null)
            {
                return null;
            }

            return new JobProfileCategory
            {
                Name = category.Name,
                Title = category.Title,
                Description = category.Description,
                Url = category.UrlName,
                Subcategories = taxonomyRepository.GetMany(c => c.Parent.Name == category.Name).Select(q => GetByUrlName(q.UrlName))
            };
        }

        public IEnumerable<JobProfile> GetRelatedJobProfiles(string category)
        {
            var searchResult = FilterByCategory(category, jobprofileSearchQueryService);
            return searchResult.Results.Select(r => mapper.Map<JobProfile>(r.ResultItem));
        }
    }
}