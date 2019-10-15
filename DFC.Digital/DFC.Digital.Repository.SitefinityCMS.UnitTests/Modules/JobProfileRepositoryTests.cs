using DFC.Digital.Data.Model;
using FakeItEasy;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Telerik.Sitefinity.DynamicModules.Model;
using Telerik.Sitefinity.GenericContent.Model;
using Telerik.Sitefinity.Model;
using Xunit;

namespace DFC.Digital.Repository.SitefinityCMS.UnitTests
{
    public class JobProfileRepositoryTests
    {
        private readonly IDynamicModuleConverter<JobProfile> fakeJobProfileConverter;
        private readonly IDynamicModuleConverter<JobProfileMessage> fakeJobProfileMessageConverter;
        private readonly IDynamicModuleConverter<JobProfileOverloadForSearch> fakeJobProfileSearchConverter;
        private readonly IDynamicModuleConverter<JobProfileOverloadForWhatItTakes> fakeWitConverter;
        private readonly IDynamicModuleRepository<SocSkillMatrix> fakeSocSkillRepo;
        private readonly IDynamicModuleRepository<JobProfile> fakeRepository;
        private readonly IDynamicContentExtensions fakeDynamicContentExtensions;

        public JobProfileRepositoryTests()
        {
            fakeJobProfileConverter = A.Fake<IDynamicModuleConverter<JobProfile>>();
            fakeJobProfileMessageConverter = A.Fake<IDynamicModuleConverter<JobProfileMessage>>();
            fakeJobProfileSearchConverter = A.Fake<IDynamicModuleConverter<JobProfileOverloadForSearch>>();
            fakeRepository = A.Fake<IDynamicModuleRepository<JobProfile>>();
            fakeWitConverter = A.Fake<IDynamicModuleConverter<JobProfileOverloadForWhatItTakes>>();
            fakeSocSkillRepo = A.Fake<IDynamicModuleRepository<SocSkillMatrix>>();
            fakeDynamicContentExtensions = A.Fake<IDynamicContentExtensions>();
        }

        [Fact]
        public void GetByUrlNameForPreviewTest()
        {
            var dummyJobProfile = A.Dummy<JobProfile>();
            A.CallTo(() => fakeJobProfileConverter.ConvertFrom(A<DynamicContent>._)).Returns(dummyJobProfile);

            var jobProfileRepository = new JobProfileRepository(fakeRepository, fakeJobProfileConverter, fakeDynamicContentExtensions, fakeSocSkillRepo, fakeWitConverter, fakeJobProfileSearchConverter, fakeJobProfileMessageConverter);
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

            var jobProfileRepository = new JobProfileRepository(fakeRepository, fakeJobProfileConverter, fakeDynamicContentExtensions, fakeSocSkillRepo, fakeWitConverter, fakeJobProfileSearchConverter, fakeJobProfileMessageConverter);
            jobProfileRepository.GetByUrlNameForSearchIndex(urlName, publishing);

            A.CallTo(() => fakeRepository.Get(A<Expression<Func<DynamicContent, bool>>>.That.Matches(m => LinqExpressionsTestHelper.IsExpressionEqual(m, item => item.UrlName == urlName && item.Status == (publishing ? ContentLifecycleStatus.Master : ContentLifecycleStatus.Live))))).MustHaveHappened();
        }

        [Fact]
        public void GetContentTypeTest()
        {
            //Assign
            var dummyJobProfile = A.Dummy<JobProfile>();
            A.CallTo(() => fakeJobProfileConverter.ConvertFrom(A<DynamicContent>._)).Returns(dummyJobProfile);
            var jobProfileRepository = new JobProfileRepository(fakeRepository, fakeJobProfileConverter, fakeDynamicContentExtensions, fakeSocSkillRepo, fakeWitConverter, fakeJobProfileSearchConverter, fakeJobProfileMessageConverter);

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
            var jobProfileRepository = new JobProfileRepository(fakeRepository, fakeJobProfileConverter, fakeDynamicContentExtensions, fakeSocSkillRepo, fakeWitConverter, fakeJobProfileSearchConverter, fakeJobProfileMessageConverter);

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

            var jobProfileRepository = new JobProfileRepository(fakeRepository, fakeJobProfileConverter, fakeDynamicContentExtensions, fakeSocSkillRepo, fakeWitConverter, fakeJobProfileSearchConverter, fakeJobProfileMessageConverter);
            jobProfileRepository.GetByUrlName(urlName);

            //A.CallTo(() => fakeRepository.Get(A<Expression<Func<DynamicContent, bool>>>.That.Matches(m => ExpressionEqualityComparer.Instance.Equals()))).MustHaveHappened();
            A.CallTo(() => fakeRepository.Get(A<Expression<Func<DynamicContent, bool>>>.That.Matches(m => LinqExpressionsTestHelper.IsExpressionEqual(m, item => item.UrlName == urlName && item.Status == ContentLifecycleStatus.Live && item.Visible == true)))).MustHaveHappened();
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void GetLiveJobProfilesTest(bool jobProfilesAvailable)
        {
            //Arrange
            var dummyJobProfile = A.Dummy<JobProfileOverloadForWhatItTakes>();

            // Dummies and fakes
            A.CallTo(() => fakeWitConverter.ConvertFrom(A<DynamicContent>._)).Returns(dummyJobProfile);
            A.CallTo(() => fakeRepository.GetMany(A<Expression<Func<DynamicContent, bool>>>._)).Returns(jobProfilesAvailable ? new EnumerableQuery<DynamicContent>(new List<DynamicContent> { new DynamicContent() }) : Enumerable.Empty<DynamicContent>().AsQueryable());
            var jobProfileRepository = new JobProfileRepository(fakeRepository, fakeJobProfileConverter, fakeDynamicContentExtensions, fakeSocSkillRepo, fakeWitConverter, fakeJobProfileSearchConverter, fakeJobProfileMessageConverter);

            // Act
            jobProfileRepository.GetLiveJobProfiles();

            // Assert
            if (jobProfilesAvailable)
            {
                A.CallTo(() => fakeWitConverter.ConvertFrom(A<DynamicContent>._)).MustHaveHappened();
            }
            else
            {
                A.CallTo(() => fakeWitConverter.ConvertFrom(A<DynamicContent>._)).MustNotHaveHappened();
            }

            A.CallTo(() => fakeRepository.GetMany(A<Expression<Func<DynamicContent, bool>>>.That.Matches(m => LinqExpressionsTestHelper.IsExpressionEqual(m, item => item.Status == ContentLifecycleStatus.Live && item.Visible)))).MustHaveHappened();
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void UpdateSocSkillMatricesTest(bool jobProfileAvailable)
        {
            // Arrange
            var urlname = "test-url";
            var digitalSkill = "digiSkill";
            var jobProfile = new JobProfileOverloadForWhatItTakes { UrlName = urlname, DigitalSkillsLevel = digitalSkill };
            var dummyDynamicContent = A.Dummy<DynamicContent>();
            var socSkill = new SocSkillMatrix { Title = "test soc" };
            var socSkills = new List<SocSkillMatrix> { socSkill };

            // Fakes Setup
            A.CallTo(() => fakeRepository.Get(A<Expression<Func<DynamicContent, bool>>>._))
                .Returns(jobProfileAvailable ? dummyDynamicContent : null);
            A.CallTo(() => fakeRepository.GetMaster(dummyDynamicContent)).Returns(dummyDynamicContent);
            A.CallTo(() => fakeRepository.Update(dummyDynamicContent, digitalSkill)).DoesNothing();
            A.CallTo(() => fakeRepository.Commit()).DoesNothing();
            A.CallTo(() =>
                fakeDynamicContentExtensions.SetRelatedFieldValue(dummyDynamicContent, dummyDynamicContent, A<string>._, A<float>._)).DoesNothing();
            A.CallTo(() =>
                fakeDynamicContentExtensions.DeleteRelatedFieldValues(dummyDynamicContent, A<string>._)).DoesNothing();
            A.CallTo(() => fakeRepository.Get(A<Expression<Func<DynamicContent, bool>>>._))
                .Returns(jobProfileAvailable ? dummyDynamicContent : null);
            A.CallTo(() => fakeSocSkillRepo.Get(A<Expression<Func<DynamicContent, bool>>>._))
                .Returns(dummyDynamicContent);

            var jobProfileRepository = new JobProfileRepository(fakeRepository, fakeJobProfileConverter, fakeDynamicContentExtensions, fakeSocSkillRepo, fakeWitConverter, fakeJobProfileSearchConverter, fakeJobProfileMessageConverter);

            // Act
            jobProfileRepository.UpdateSocSkillMatrices(jobProfile, socSkills);

            // Assert
            A.CallTo(() => fakeRepository.Get(A<Expression<Func<DynamicContent, bool>>>.That.Matches(m =>
                    LinqExpressionsTestHelper.IsExpressionEqual(m, item => item.UrlName == jobProfile.UrlName && item.Status == ContentLifecycleStatus.Live &&
                                item.Visible))))
                .MustHaveHappened();

            if (jobProfileAvailable)
            {
                A.CallTo(() => fakeRepository.GetMaster(dummyDynamicContent)).MustHaveHappened();
                A.CallTo(() => fakeRepository.Update(dummyDynamicContent, A<string>._)).MustHaveHappened();
                A.CallTo(() => fakeRepository.Commit()).MustHaveHappened();
                   A.CallTo(() =>
                    fakeDynamicContentExtensions.SetRelatedFieldValue(A<DynamicContent>._, A<DynamicContent>._, "RelatedSkills", A<float>._)).MustHaveHappened();
                A.CallTo(() =>
                        fakeDynamicContentExtensions.DeleteRelatedFieldValues(A<DynamicContent>._, A<string>._))
                    .MustHaveHappened();
                A.CallTo(() => fakeSocSkillRepo.Get(A<Expression<Func<DynamicContent, bool>>>.That.Matches(m =>
                    LinqExpressionsTestHelper.IsExpressionEqual(m, d => d.Status == ContentLifecycleStatus.Master && d.UrlName == socSkill.SfUrlName)))).MustHaveHappened();
            }
            else
            {
                A.CallTo(() => fakeRepository.GetMaster(dummyDynamicContent)).MustNotHaveHappened();
                A.CallTo(() => fakeRepository.Update(dummyDynamicContent, A<string>._)).MustNotHaveHappened();
                A.CallTo(() => fakeRepository.Commit()).MustNotHaveHappened();
                A.CallTo(() => fakeRepository.CheckinTemp(dummyDynamicContent)).MustNotHaveHappened();
                 A.CallTo(() =>
                    fakeDynamicContentExtensions.SetRelatedFieldValue(A<DynamicContent>._, A<DynamicContent>._, A<string>._, A<float>._)).MustNotHaveHappened();
                A.CallTo(() =>
                    fakeDynamicContentExtensions.DeleteRelatedFieldValues(A<DynamicContent>._, A<string>._)).MustNotHaveHappened();
                A.CallTo(() => fakeSocSkillRepo.Get(A<Expression<Func<DynamicContent, bool>>>._))
                    .MustNotHaveHappened();
            }
        }
    }
}