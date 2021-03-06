﻿using AutoMapper;
using DFC.Digital.AutomationTest.Utilities;
using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using DFC.Digital.Repository.SitefinityCMS.Modules;
using DFC.Digital.Web.Sitefinity.JobProfileModule;
using FakeItEasy;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Telerik.Sitefinity.Taxonomies.Model;
using Xunit;

namespace DFC.Digital.Repository.SitefinityCMS.UnitTests
{
    public class JobProfileCategoryRepositoryUnitTests
    {
        private const string JobprofileTaxonomyName = "job-profile-categories";
        private readonly ISearchQueryService<JobProfileIndex> fakeSearchService;
        private readonly ITaxonomyRepository fakeTaxonomyRepository;
        private readonly IMapper fakeMapper;
        private readonly HierarchicalTaxon dummyTaxon;

        public JobProfileCategoryRepositoryUnitTests()
        {
            fakeSearchService = A.Fake<ISearchQueryService<JobProfileIndex>>();
            fakeMapper = A.Fake<IMapper>();
            dummyTaxon = GetDummyTaxon("categoryTwo");
            fakeTaxonomyRepository = A.Fake<ITaxonomyRepository>();
        }

        [Theory(Skip = "LString throwing a null reference exception")]
        [InlineData(true)]
        [InlineData(false)]
        public void GetByUrlNameTest(bool isExistingJobCategory)
        {
            //Instantiate
            var jobProfileCategoryRepository = GetTestJobProfileCategoryRepository();

            A.CallTo(() => fakeTaxonomyRepository.Get(A<Expression<Func<HierarchicalTaxon, bool>>>._)).Returns(isExistingJobCategory ? dummyTaxon : null);
            A.CallTo(() => fakeTaxonomyRepository.GetMany(A<Expression<Func<HierarchicalTaxon, bool>>>._)).Returns(new EnumerableQuery<HierarchicalTaxon>(new List<HierarchicalTaxon>()));

            string categoryUrlName = dummyTaxon.UrlName;
            var retunedCategory = jobProfileCategoryRepository.GetByUrlName(categoryUrlName);

            //this is the one we are going to try and find
            if (isExistingJobCategory)
            {
                //Asserts
                retunedCategory.Name.Should().Be(dummyTaxon.Name);
                retunedCategory.Title.Should().Be(dummyTaxon.Title);
                retunedCategory.Description.Should().Be(dummyTaxon.Description);
                A.CallTo(() => fakeTaxonomyRepository.GetMany(A<Expression<Func<HierarchicalTaxon, bool>>>._)).MustHaveHappened();
            }
            else
            {
                //Asserts
                retunedCategory.Should().BeNull();
                A.CallTo(() => fakeTaxonomyRepository.GetMany(A<Expression<Func<HierarchicalTaxon, bool>>>._)).MustNotHaveHappened();
            }

            A.CallTo(() => fakeTaxonomyRepository.Get(A<Expression<Func<HierarchicalTaxon, bool>>>.That.Matches(m => LinqExpressionsTestHelper.IsExpressionEqual(m, c => c.UrlName == categoryUrlName && c.Taxonomy.Name == JobprofileTaxonomyName)))).MustHaveHappened();
        }

        [Fact(Skip = "LString throwing a null reference exception")]
        public void GetJobProfileCategoriesTests()
        {
            A.CallTo(() => fakeTaxonomyRepository.GetMany(A<Expression<Func<HierarchicalTaxon, bool>>>._)).Returns(new EnumerableQuery<HierarchicalTaxon>(new List<HierarchicalTaxon> { dummyTaxon }));

            //Instantiate
            var jobProfileCategoryRepository = GetTestJobProfileCategoryRepository();

            var retunedCategories = jobProfileCategoryRepository.GetJobProfileCategories();

            //Asset - should get back the same number
            retunedCategories.Should().HaveCount(1);
            A.CallTo(() => fakeTaxonomyRepository.Get(A<Expression<Func<HierarchicalTaxon, bool>>>._)).MustNotHaveHappened();
            A.CallTo(() => fakeTaxonomyRepository.GetMany(A<Expression<Func<HierarchicalTaxon, bool>>>.That.Matches(m => LinqExpressionsTestHelper.IsExpressionEqual(m, category => category.Taxonomy.Name == JobprofileTaxonomyName)))).MustHaveHappened();
        }

        [Fact(Skip = "LString throwing a null reference exception")]
        public void GetRelatedJobProfilesTest()
        {
            A.CallTo(() => fakeSearchService.Search("*", null)).WithAnyArguments().Returns(DummySearchResults());

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<JobProfilesAutoMapperProfile>();
            });
            var mapper = config.CreateMapper();

            //Instantiate
            var jobProfileCategoryRepository = new JobProfileCategoryRepository(fakeSearchService, mapper, fakeTaxonomyRepository);

            var returnedJobProfiles = jobProfileCategoryRepository.GetRelatedJobProfiles("test");

            var expectedResults = DummyJobProfile.GetDummyJobProfiles();

            //Assert
            //The results from search do not include the SOCCode so ignore this
            returnedJobProfiles.Should().BeEquivalentTo(expectedResults, options => options.Excluding(j => j.SOCCode));
            A.CallTo(() => fakeSearchService.Search("*", A<SearchProperties>.That.Matches(x => x.Count.Equals(1000)))).WithAnyArguments().MustHaveHappened();
            A.CallTo(() => fakeTaxonomyRepository.Get(A<Expression<Func<HierarchicalTaxon, bool>>>._)).MustNotHaveHappened();
            A.CallTo(() => fakeTaxonomyRepository.GetMany(A<Expression<Func<HierarchicalTaxon, bool>>>._)).MustNotHaveHappened();
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
                        UrlName = p.UrlName,
                        AlternativeTitle = p.AlternativeTitle?.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(a => a.Trim()),
                        Overview = p.Overview,
                        MaximumHours = 0,
                        MinimumHours = 0
                    }
                };
            }
        }

        private JobProfileCategoryRepository GetTestJobProfileCategoryRepository()
        {
            // Set up calls
            return new JobProfileCategoryRepository(fakeSearchService, fakeMapper, fakeTaxonomyRepository);
        }

        private IQueryable<Taxon> DummyTaxons()
        {
            var t = new List<HierarchicalTaxon>();
            A.Dummy<HierarchicalTaxon>();
            t.Add(GetDummyTaxon("categoryOne"));
            t.Add(GetDummyTaxon("categoryTwo"));
            t.Add(GetDummyTaxon("categoryThree"));
            return t.AsQueryable();
        }

        private HierarchicalTaxon GetDummyTaxon(string name)
        {
            var b = A.Dummy<HierarchicalTaxon>();
            b.Taxonomy.Name = "job-profile-categories";
            b.Name = $"Name-{name}";
            b.UrlName = $"URL-{name}";
            b.Title = $"Title-{name}";
            b.Description = $"Description-{name}";
            b.Parent = A.Dummy<HierarchicalTaxon>();
            b.Parent.Name = "parentName";
            return b;
        }
    }
}