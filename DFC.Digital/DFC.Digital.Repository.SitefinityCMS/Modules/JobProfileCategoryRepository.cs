using AutoMapper;
using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Telerik.Sitefinity.Taxonomies.Web;

namespace DFC.Digital.Repository.SitefinityCMS.Modules
{
    public class JobProfileCategoryRepository : TaxonomyRepository, IJobProfileCategoryRepository
    {
        private const string JobprofileTaxonomyName = "job-profile-categories";

        private readonly ISearchQueryService<JobProfileIndex> jobprofileSearchQueryService;
        private readonly IMapper mapper;

        public JobProfileCategoryRepository(ISearchQueryService<JobProfileIndex> jobProfileSearchQueryService, IMapper mapper, ITaxonomyManager taxonomyManager) : base(taxonomyManager)
        {
            this.jobprofileSearchQueryService = jobProfileSearchQueryService;
            this.mapper = mapper;
        }

        public IQueryable<JobProfileCategory> GetJobProfileCategories()
        {
            return GetMany(category => category.Taxonomy.Name == JobprofileTaxonomyName).Select(category => new JobProfileCategory
            {
                Name = category.Name,
                Title = category.Title,
                Description = category.Description,
                Url = category.UrlName
            });
        }

        public IEnumerable<JobProfileCategory> GetByIds(IList<Guid> categoryIds)
        {
            var categories = GetMany(c => categoryIds.Contains(c.Id) && c.Taxonomy.Name == JobprofileTaxonomyName);
            foreach (var category in categories)
            {
                yield return new JobProfileCategory
                {
                    Name = category.Name,
                    Title = category.Title,
                    Description = category.Description,
                    Url = category.UrlName,
                    Subcategories = GetMany(c => c.Parent.Name == category.Name).Select(q => GetByUrlName(q.UrlName))
                };
            }
        }

        public JobProfileCategory GetByUrlName(string categoryUrlName)
        {
            var category = Get(c => c.UrlName == categoryUrlName && c.Taxonomy.Name == JobprofileTaxonomyName);

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
                Subcategories = GetMany(c => c.Parent.Name == category.Name).Select(q => GetByUrlName(q.UrlName))
            };
        }

        [SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1118:ParameterMustNotSpanMultipleLines", Justification = "Reviewed.")]
        public IEnumerable<JobProfile> GetRelatedJobProfiles(string category)
        {
            var searchResult = jobprofileSearchQueryService.Search(
                    "*",
                    new SearchProperties
                    {
                        UseRawSearchTerm = true,
                        Count = 100,
                        FilterBy = $"{nameof(JobProfileIndex.JobProfileCategories)}/any(c: c eq '{category}')",
                        OrderByFields = new string[] { nameof(JobProfileIndex.Title) },
                    });

            return searchResult.Results.Select(r => mapper.Map<JobProfile>(r.ResultItem));
        }
    }
}