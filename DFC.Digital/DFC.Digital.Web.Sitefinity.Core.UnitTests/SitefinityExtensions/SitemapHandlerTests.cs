using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using FakeItEasy;
using FluentAssertions;
using System.Collections.Generic;
using System.Linq;
using Telerik.Sitefinity.SitemapGenerator.Data;
using Xunit;

namespace DFC.Digital.Web.Sitefinity.Core.Tests
{
    public class SitemapHandlerTests
    {
        [Theory]
        [InlineData(true, 9, 10)]
        [InlineData(false, 5,  5)]
        public void ManipulateSitemapTest(bool validCategory, int numberOfItems,  int expectedCount)
        {
            //Setup the fakes and dummies
            var fakeRepository = A.Fake<IJobProfileCategoryRepository>();
            var siteMapList = GetEntries(validCategory, numberOfItems);
            A.CallTo(() => fakeRepository.GetJobProfileCategories()).Returns(new EnumerableQuery<JobProfileCategory>(new List<JobProfileCategory> { new JobProfileCategory { Url = nameof(JobProfileCategory.Url) } }));

            //Instantiate & Act
            var sitemapHandler = new SitemapHandler(fakeRepository);

            //Act
            var result = sitemapHandler.ManipulateSitemap(siteMapList);

            //Assert
            result.Count().Should().Be(expectedCount);
            result.Any(x => x.Location.ToUpperInvariant().EndsWith("/JOB-CATEGORIES") || x.Location.ToUpperInvariant().EndsWith("/JOB-PROFILES")).Should().BeFalse();

            if (validCategory)
            {
                A.CallTo(() => fakeRepository.GetJobProfileCategories()).MustHaveHappenedOnceExactly();
                result.Any(x => x.Location.Equals($"/job-categories/{nameof(JobProfileCategory.Url)}")).Should().BeTrue();
            }
            else
            {
                A.CallTo(() => fakeRepository.GetJobProfileCategories()).MustNotHaveHappened();
            }
        }

        private List<SitemapEntry> GetEntries(bool includeCategoryEntry, int numberOfItems)
        {
            var list = new List<SitemapEntry>();

            for (int i = 0; i < numberOfItems; i++)
            {
                list.Add(new SitemapEntry { Location = nameof(SitemapEntry.Location) });
            }

            list.Add(new SitemapEntry { Location = "/job-profiles" });

            if (includeCategoryEntry)
            {
                list.Add(new SitemapEntry { Location = "/job-categories" });
            }

            return list;
        }
    }
}