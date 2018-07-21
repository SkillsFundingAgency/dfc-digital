using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using DFC.Digital.Data.Model;
using FakeItEasy;
using FluentAssertions;
using Telerik.Sitefinity.DynamicModules.Model;
using Telerik.Sitefinity.GenericContent.Model;
using Xunit;

namespace DFC.Digital.Repository.SitefinityCMS.UnitTests
{
    public class JobProfileRepositoryTests
    {
        private IDynamicModuleConverter<JobProfile> fakeJobProfileConverter;
        private IDynamicModuleRepository<SocSkillMatrix> fakeSocSkillRepo;
        private IDynamicModuleRepository<JobProfile> fakeRepository;
        private IDynamicContentExtensions fakeDynamicContentExtensions;

        public JobProfileRepositoryTests()
        {
            fakeJobProfileConverter = A.Fake<IDynamicModuleConverter<JobProfile>>();
            fakeRepository = A.Fake<IDynamicModuleRepository<JobProfile>>();
            fakeSocSkillRepo = A.Fake<IDynamicModuleRepository<SocSkillMatrix>>();
            fakeDynamicContentExtensions = A.Fake<IDynamicContentExtensions>();
        }

        [Fact]
        public void GetByUrlNameForPreviewTest()
        {
            var dummyJobProfile = A.Dummy<JobProfile>();
            A.CallTo(() => fakeJobProfileConverter.ConvertFrom(A<DynamicContent>._)).Returns(dummyJobProfile);

            var jobProfileRepository = new JobProfileRepository(fakeRepository, fakeJobProfileConverter, fakeDynamicContentExtensions, fakeSocSkillRepo);
            jobProfileRepository.GetByUrlNameForPreview("testURLName");

            A.CallTo(() => fakeRepository.Get(A<Expression<Func<DynamicContent, bool>>>._)).MustHaveHappened();
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void GetByUrlNameForSearchIndexTest(bool publishing)
        {
            var dummyJobProfile = A.Dummy<JobProfile>();
            var urlName = "testURLName";
            A.CallTo(() => fakeJobProfileConverter.ConvertFrom(A<DynamicContent>._)).Returns(dummyJobProfile);

            var jobProfileRepository = new JobProfileRepository(fakeRepository, fakeJobProfileConverter, fakeDynamicContentExtensions, fakeSocSkillRepo);
            jobProfileRepository.GetByUrlNameForSearchIndex(urlName, publishing);

            A.CallTo(() => fakeRepository.Get(A<Expression<Func<DynamicContent, bool>>>.That.Matches(m => LinqExpressionsTestHelper.IsExpressionEqual(m, item => item.UrlName == urlName && item.Status == (publishing ? ContentLifecycleStatus.Master : ContentLifecycleStatus.Live))))).MustHaveHappened();
        }

        [Fact]
        public void GetContentTypeTest()
        {
            //Assign
            var dummyJobProfile = A.Dummy<JobProfile>();
            A.CallTo(() => fakeJobProfileConverter.ConvertFrom(A<DynamicContent>._)).Returns(dummyJobProfile);
            var jobProfileRepository = new JobProfileRepository(fakeRepository, fakeJobProfileConverter, fakeDynamicContentExtensions, fakeSocSkillRepo);

            //Act
            var result = jobProfileRepository.GetContentType();

            //Assert
            result.Should().NotBeNull();
        }

        [Fact]
        public void GetProviderNameTest()
        {
            //Assign
            var dummyJobProfile = A.Dummy<JobProfile>();
            A.CallTo(() => fakeJobProfileConverter.ConvertFrom(A<DynamicContent>._)).Returns(dummyJobProfile);
            var jobProfileRepository = new JobProfileRepository(fakeRepository, fakeJobProfileConverter, fakeDynamicContentExtensions, fakeSocSkillRepo);

            //Act
            var result = jobProfileRepository.GetProviderName();

            //Assert
            result.Should().NotBeNull();
        }

        [Fact]
        public void GetByUrlNameTest()
        {
            var dummyJobProfile = A.Dummy<JobProfile>();
            var urlName = "testURLName";
            A.CallTo(() => fakeJobProfileConverter.ConvertFrom(A<DynamicContent>._)).Returns(dummyJobProfile);

            var jobProfileRepository = new JobProfileRepository(fakeRepository, fakeJobProfileConverter, fakeDynamicContentExtensions, fakeSocSkillRepo);
            jobProfileRepository.GetByUrlName(urlName);

            //A.CallTo(() => fakeRepository.Get(A<Expression<Func<DynamicContent, bool>>>.That.Matches(m => ExpressionEqualityComparer.Instance.Equals()))).MustHaveHappened();
            A.CallTo(() => fakeRepository.Get(A<Expression<Func<DynamicContent, bool>>>.That.Matches(m => LinqExpressionsTestHelper.IsExpressionEqual(m, item => item.UrlName == urlName && item.Status == ContentLifecycleStatus.Live && item.Visible)))).MustHaveHappened();
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void GetLiveJobProfilesTest(bool jobProfilesAvailable)
        {
            var dummyJobProfile = A.Dummy<JobProfile>();
            A.CallTo(() => fakeJobProfileConverter.ConvertFrom(A<DynamicContent>._)).Returns(dummyJobProfile);
            A.CallTo(() => fakeRepository.GetMany(A<Expression<Func<DynamicContent, bool>>>._)).Returns(new EnumerableQuery<DynamicContent>(new List<DynamicContent> { jobProfilesAvailable ? new DynamicContent() : null }));
            var jobProfileRepository = new JobProfileRepository(fakeRepository, fakeJobProfileConverter, fakeDynamicContentExtensions, fakeSocSkillRepo);
            jobProfileRepository.GetLiveJobProfiles();

            A.CallTo(() => fakeRepository.GetMany(A<Expression<Func<DynamicContent, bool>>>.That.Matches(m => LinqExpressionsTestHelper.IsExpressionEqual(m, item => item.Status == ContentLifecycleStatus.Live && item.Visible)))).MustHaveHappened();
        }

        [Fact]
        public void UpdateDigitalSkillTest()
        {
            Assert.True(false, "This test needs an implementation");
        }

        [Fact]
        public void UpdateSocSkillMatricesTest()
        {
            Assert.True(false, "This test needs an implementation");
        }
    }
}