using DFC.Digital.Data.Model;
using FakeItEasy;
using FluentAssertions;
using System;
using System.Linq.Expressions;
using Telerik.Sitefinity.DynamicModules.Model;
using Telerik.Sitefinity.GenericContent.Model;
using Xunit;

namespace DFC.Digital.Repository.SitefinityCMS.UnitTests
{
    public class JobProfileRepositoryTests
    {
        private IDynamicModuleConverter<JobProfile> fakeJobProfileConverter;
        private IDynamicModuleRepository<JobProfile> fakeRepository;

        public JobProfileRepositoryTests()
        {
            fakeJobProfileConverter = A.Fake<IDynamicModuleConverter<JobProfile>>();
            fakeRepository = A.Fake<IDynamicModuleRepository<JobProfile>>();
        }

        [Fact]
        public void GetByUrlNameForPreviewTest()
        {
            var dummyJobProfile = A.Dummy<JobProfile>();
            A.CallTo(() => fakeJobProfileConverter.ConvertFrom(A<DynamicContent>._)).Returns(dummyJobProfile);

            var jobProfileRepository = new JobProfileRepository(fakeRepository, fakeJobProfileConverter);
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

            var jobProfileRepository = new JobProfileRepository(fakeRepository, fakeJobProfileConverter);
            jobProfileRepository.GetByUrlNameForSearchIndex(urlName, publishing);

            A.CallTo(() => fakeRepository.Get(A<Expression<Func<DynamicContent, bool>>>.That.Matches(m => LinqExpressionsTestHelper.IsExpressionEqual(m, item => item.UrlName == urlName && item.Status == (publishing ? ContentLifecycleStatus.Master : ContentLifecycleStatus.Live))))).MustHaveHappened();
        }

        [Fact]
        public void GetContentTypeTest()
        {
            //Assign
            var dummyJobProfile = A.Dummy<JobProfile>();
            A.CallTo(() => fakeJobProfileConverter.ConvertFrom(A<DynamicContent>._)).Returns(dummyJobProfile);
            var jobProfileRepository = new JobProfileRepository(fakeRepository, fakeJobProfileConverter);

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
            var jobProfileRepository = new JobProfileRepository(fakeRepository, fakeJobProfileConverter);

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

            var jobProfileRepository = new JobProfileRepository(fakeRepository, fakeJobProfileConverter);
            jobProfileRepository.GetByUrlName(urlName);

            //A.CallTo(() => fakeRepository.Get(A<Expression<Func<DynamicContent, bool>>>.That.Matches(m => ExpressionEqualityComparer.Instance.Equals()))).MustHaveHappened();
            A.CallTo(() => fakeRepository.Get(A<Expression<Func<DynamicContent, bool>>>.That.Matches(m => LinqExpressionsTestHelper.IsExpressionEqual(m, item => item.UrlName == urlName && item.Status == ContentLifecycleStatus.Live && item.Visible == true)))).MustHaveHappened();
        }
    }
}