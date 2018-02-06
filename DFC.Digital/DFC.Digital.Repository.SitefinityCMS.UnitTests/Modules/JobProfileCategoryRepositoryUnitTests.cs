using AutoMapper;
using DFC.Digital.AutomationTest.Utilities;
using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using DFC.Digital.Repository.SitefinityCMS.Modules;
using DFC.Digital.Web.Sitefinity.JobProfileModule.Config;
using FakeItEasy;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using Telerik.Sitefinity.Taxonomies.Model;
using Telerik.Sitefinity.Taxonomies.Web;
using Xunit;

namespace DFC.Digital.Repository.SitefinityCMS.UnitTests.Modules
{
    public class JobProfileCategoryRepositoryUnitTests
    {
        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void GetByUrlNameTest(bool isExistingJobCategory)
        {
            //Instantiate
            var jobProfileCategoryRepository = GetTestJobProfileCategoryRepository();

            //this is the one we are going to try and find
            if (isExistingJobCategory)
            {
                //Act
                var expectedTaxon = GetDummyTaxon("categoryTwo");
                var retunedCategory = jobProfileCategoryRepository.GetByUrlName(expectedTaxon.UrlName);

                //Asserts
                retunedCategory.Name.Should().Be(expectedTaxon.Name);
                retunedCategory.Title.Should().Be(expectedTaxon.Title);
                retunedCategory.Description.Should().Be(expectedTaxon.Description);
            }
            else
            {
                //Asserts
                jobProfileCategoryRepository.GetByUrlName("shouldNotFind").Should().Be(null);
            }
        }

        [Fact]
        public void GetJobProfileCategoriesTests()
        {
            //Instantiate
            var jobProfileCategoryRepository = GetTestJobProfileCategoryRepository();

            var retunedCategories = jobProfileCategoryRepository.GetJobProfileCategories();

            //Asset - should get back the same number
            retunedCategories.Should().HaveCount(DummyTaxons().Count());
        }

        [Fact]
        public void GetRelatedJobProfilesTest()
        {
            //Setup the fakes and dummies
            var fakeSearchService = A.Fake<ISearchQueryService<JobProfileIndex>>();
            var fakeTaxonomyManager = A.Fake<ITaxonomyManager>();
            var fakeMapper = A.Fake<IMapper>();

            A.CallTo(() => fakeSearchService.Search("*", null)).WithAnyArguments().Returns(DummySearchResults());

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<JobProfilesAutoMapperProfile>();
            });
            var mapper = config.CreateMapper();

            //Instantiate
            var jobProfileCategoryRepository = new JobProfileCategoryRepository(fakeSearchService, mapper, fakeTaxonomyManager);

            var returnedJobProfiles = jobProfileCategoryRepository.GetRelatedJobProfiles("test");

            var expectedResults = DummyJobProfile.GetDummyJobProfiles();

            //Assert
            //The results from search do not include the SOCCode so ignore this
            returnedJobProfiles.ShouldAllBeEquivalentTo(expectedResults, options => options.Excluding(p => p.SOCCode));
        }

        private SearchResult<JobProfileIndex> DummySearchResults()
        {
            var retSearchResults = new SearchResult<JobProfileIndex>()
            {
                Count = DummyJobProfile.GetDummyJobProfiles().Count(),
                Results = GenerateJobProfileResultItemDummyCollection(DummyJobProfile.GetDummyJobProfiles())
            };
            return retSearchResults;
        }

        private IEnumerable<SearchResultItem<JobProfileIndex>> GenerateJobProfileResultItemDummyCollection(IEnumerable<JobProfile> jobProfiles)
        {
            var iRank = 1;
            foreach (JobProfile p in jobProfiles)
            {
                yield return new SearchResultItem<JobProfileIndex>
                {
                    Rank = iRank++,
                    ResultItem = new JobProfileIndex
                    {
                        IdentityField = p.Title,
                        Title = p.Title,
                        FilterableTitle = p.Title,
                        UrlName = p.UrlName,
                        FilterableAlternativeTitle = p.AlternativeTitle,
                        AlternativeTitle = p.AlternativeTitle?.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(a => a.Trim()),
                        Overview = p.Overview,
                        SalaryRange = p.SalaryRange,
                    }
                };
            }
        }

        private JobProfileCategoryRepository GetTestJobProfileCategoryRepository()
        {
            //Setup the fakes and dummies
            var fakeSearchService = A.Fake<ISearchQueryService<JobProfileIndex>>();
            var fakeTaxonomyManager = A.Fake<ITaxonomyManager>();
            var fakeMapper = A.Fake<IMapper>();

            // Set up calls
            A.CallTo(() => fakeTaxonomyManager.GetTaxa<Taxon>()).Returns(DummyTaxons());

            return new JobProfileCategoryRepository(fakeSearchService, fakeMapper, fakeTaxonomyManager);
        }

        private IQueryable<Taxon> DummyTaxons()
        {
            var t = new List<Taxon>();
            var b = A.Dummy<Taxon>();
            t.Add(GetDummyTaxon("categoryOne"));
            t.Add(GetDummyTaxon("categoryTwo"));
            t.Add(GetDummyTaxon("categoryThree"));
            return t.AsQueryable();
        }

        private Taxon GetDummyTaxon(string name)
        {
            var b = A.Dummy<Taxon>();
            b.Taxonomy.Name = "job-profile-categories";
            b.Name = $"Name-{name}";
            b.UrlName = $"URL-{name}";
            b.Title = $"Title-{name}";
            b.Description = $"Description-{name}";
            b.Parent.Name = string.Empty;
            return b;
        }
    }
}