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

        [Theory]
        [InlineData("test", 5)]
        [InlineData("test", 3)]
        public void GetByParentNameTest(string urlName, int numberOfItemsToReturn)
        {
            //Assign
            var fakeRepo = GetTestJobProfilesRelatedCareersRepository(1, true);

            //Act
            fakeRepo.GetByParentName(urlName, numberOfItemsToReturn);

            // Assert
            A.CallTo(() => fakeDynamicContentExtensions.GetRelatedItems(A<DynamicContent>._, A<string>._, A<int>._))
                .MustHaveHappened();
            A.CallTo(() => fakeRepository.Get(A<Expression<Func<DynamicContent, bool>>>
                .That.Matches(m => LinqExpressionsTestHelper.IsExpressionEqual(m, item => item.UrlName == urlName && item.Status == ContentLifecycleStatus.Live && item.Visible))))
                .MustHaveHappened();
        }

        [Theory]
        [InlineData("test", 5)]
        [InlineData("test", 3)]
        public void GetByParentNameForPreviewTest(string urlName, int numberOfItemsToReturn)
        {
            //Assign
            var fakeRepo = GetTestJobProfilesRelatedCareersRepository(1, true);

            //Act
            fakeRepo.GetByParentNameForPreview(urlName, numberOfItemsToReturn);

            // Assert
            A.CallTo(() => fakeDynamicContentExtensions.GetRelatedItems(A<DynamicContent>._, A<string>._, A<int>._))
                .MustHaveHappened();
            A.CallTo(() => fakeRepository.Get(A<Expression<Func<DynamicContent, bool>>>
                    .That.Matches(m => LinqExpressionsTestHelper.IsExpressionEqual(m, item => item.UrlName == urlName && item.Status == ContentLifecycleStatus.Temp))))
                .MustHaveHappened();
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