using DFC.Digital.Data.Model;
using DFC.Digital.Repository.SitefinityCMS.UnitTests;
using FakeItEasy;
using FluentAssertions;
using System;
using System.Linq;
using System.Linq.Expressions;
using Telerik.Sitefinity.DynamicModules.Model;
using Telerik.Sitefinity.GenericContent.Model;
using Xunit;

namespace DFC.Digital.Repository.SitefinityCMS.Modules.Tests
{
    public class JobProfilesRelatedCareersRepositoryTests
    {
        private IDynamicModuleConverter<JobProfileRelatedCareer> fakeJobProfileRelatedCareerConverter;
        private IDynamicModuleRepository<JobProfile> fakeRepository;
        private IDynamicContentExtensions fakeDynamicContentExtensions;
        private DynamicContent dummyJobProfile = A.Dummy<DynamicContent>();

        public JobProfilesRelatedCareersRepositoryTests()
        {
            fakeJobProfileRelatedCareerConverter = A.Fake<IDynamicModuleConverter<JobProfileRelatedCareer>>();
            fakeRepository = A.Fake<IDynamicModuleRepository<JobProfile>>();
            fakeDynamicContentExtensions = A.Fake<IDynamicContentExtensions>();
        }

        [Fact]
        public void JobProfilesRelatedCareersRepositoryTest()
        {
            //Assign
            //Act
            var result = GetTestJobProfilesRelatedCareersRepository(0);

            //Assert
            result.Should().NotBeNull();
        }

        [Fact]
        public void GetByParentNameTest()
        {
            Assert.True(false, "This test needs an implementation");
        }

        [Fact]
        public void GetByParentNameForPreviewTest()
        {
            Assert.True(false, "This test needs an implementation");
        }

        [Theory]
        [InlineData(true, 3)]
        [InlineData(false, 10)]
        public void GetRelatedCareersTest(bool availableRelatedCareers, int numberOfItemsToReturn)
        {
            //Assign
            var fakeRepo = GetTestJobProfilesRelatedCareersRepository(numberOfItemsToReturn, availableRelatedCareers);

            //Act
            var result = fakeRepo.GetRelatedCareers(dummyJobProfile, numberOfItemsToReturn);

            //Assert
            if (availableRelatedCareers)
            {
                result.Count().Should().Be(numberOfItemsToReturn);
            }
            else
            {
                result.Should().BeNullOrEmpty();
            }

            A.CallTo(() => fakeDynamicContentExtensions.GetRelatedItems(A<DynamicContent>._, A<string>._, A<int>._)).MustHaveHappened();
        }

        private JobProfilesRelatedCareersRepository GetTestJobProfilesRelatedCareersRepository(int numberOfRelatedCareers, bool relatedCareersAvailable = false)
        {
            //Setup the fakes and dummies
            dummyJobProfile.UrlName = nameof(DynamicContent.UrlName);
            A.CallTo(() => fakeRepository.Get(A<Expression<Func<DynamicContent, bool>>>._)).Returns(dummyJobProfile);

            var dummysRelatedCareers = relatedCareersAvailable ? A.CollectionOfDummy<DynamicContent>(numberOfRelatedCareers).AsEnumerable().AsQueryable() : Enumerable.Empty<DynamicContent>().AsQueryable();
            A.CallTo(() => fakeDynamicContentExtensions.GetRelatedItems(A<DynamicContent>._, A<string>._, A<int>._)).Returns(dummysRelatedCareers);

            return new JobProfilesRelatedCareersRepository(fakeJobProfileRelatedCareerConverter, fakeRepository, fakeDynamicContentExtensions);
        }
    }
}