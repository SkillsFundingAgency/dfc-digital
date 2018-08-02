using DFC.Digital.Data.Model;
using FakeItEasy;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Telerik.Sitefinity.DynamicModules.Model;
using Telerik.Sitefinity.GenericContent.Model;
using Xunit;

namespace DFC.Digital.Repository.SitefinityCMS.UnitTests
{
    public class JobProfileRepositoryTests
    {
        private readonly IDynamicModuleConverter<JobProfile> fakeJobProfileConverter;
        private readonly IDynamicModuleConverter<ImportJobProfile> importfakeJobProfileConverter;
        private readonly IDynamicModuleRepository<SocSkillMatrix> fakeSocSkillRepo;
        private readonly IDynamicModuleRepository<JobProfile> fakeRepository;
        private readonly IDynamicContentExtensions fakeDynamicContentExtensions;

        public JobProfileRepositoryTests()
        {
            fakeJobProfileConverter = A.Fake<IDynamicModuleConverter<JobProfile>>();
            fakeRepository = A.Fake<IDynamicModuleRepository<JobProfile>>();
            importfakeJobProfileConverter = A.Fake<IDynamicModuleConverter<ImportJobProfile>>();
            fakeSocSkillRepo = A.Fake<IDynamicModuleRepository<SocSkillMatrix>>();
            fakeDynamicContentExtensions = A.Fake<IDynamicContentExtensions>();
        }

        [Fact]
        public void GetByUrlNameForPreviewTest()
        {
            var dummyJobProfile = A.Dummy<JobProfile>();
            A.CallTo(() => fakeJobProfileConverter.ConvertFrom(A<DynamicContent>._)).Returns(dummyJobProfile);

            var jobProfileRepository = new JobProfileRepository(fakeRepository, fakeJobProfileConverter, fakeDynamicContentExtensions, fakeSocSkillRepo, importfakeJobProfileConverter);
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

            var jobProfileRepository = new JobProfileRepository(fakeRepository, fakeJobProfileConverter, fakeDynamicContentExtensions, fakeSocSkillRepo, importfakeJobProfileConverter);
            jobProfileRepository.GetByUrlNameForSearchIndex(urlName, publishing);

            A.CallTo(() => fakeRepository.Get(A<Expression<Func<DynamicContent, bool>>>.That.Matches(m => LinqExpressionsTestHelper.IsExpressionEqual(m, item => item.UrlName == urlName && item.Status == (publishing ? ContentLifecycleStatus.Master : ContentLifecycleStatus.Live))))).MustHaveHappened();
        }

        [Fact]
        public void GetContentTypeTest()
        {
            //Assign
            var dummyJobProfile = A.Dummy<JobProfile>();
            A.CallTo(() => fakeJobProfileConverter.ConvertFrom(A<DynamicContent>._)).Returns(dummyJobProfile);
            var jobProfileRepository = new JobProfileRepository(fakeRepository, fakeJobProfileConverter, fakeDynamicContentExtensions, fakeSocSkillRepo, importfakeJobProfileConverter);

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
            var jobProfileRepository = new JobProfileRepository(fakeRepository, fakeJobProfileConverter, fakeDynamicContentExtensions, fakeSocSkillRepo, importfakeJobProfileConverter);

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

            var jobProfileRepository = new JobProfileRepository(fakeRepository, fakeJobProfileConverter, fakeDynamicContentExtensions, fakeSocSkillRepo, importfakeJobProfileConverter);
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
            var dummyJobProfile = A.Dummy<JobProfile>();

            // Dummies and fakes
            A.CallTo(() => fakeJobProfileConverter.ConvertFrom(A<DynamicContent>._)).Returns(dummyJobProfile);
            A.CallTo(() => fakeRepository.GetMany(A<Expression<Func<DynamicContent, bool>>>._)).Returns(new EnumerableQuery<DynamicContent>(new List<DynamicContent> { jobProfilesAvailable ? new DynamicContent() : null }));
            var jobProfileRepository = new JobProfileRepository(fakeRepository, fakeJobProfileConverter, fakeDynamicContentExtensions, fakeSocSkillRepo, importfakeJobProfileConverter);

            // Act
            jobProfileRepository.GetLiveJobProfiles();

            // Assert

            // Needs investigation as fake convertor not registering call made to it when there is a job profile available.s
            //if (jobProfilesAvailable)
            //{
            //    A.CallTo(() => fakeJobProfileConverter.ConvertFrom(A<DynamicContent>._)).MustHaveHappened();
            //}
            //else
            //{
            //    A.CallTo(() => fakeJobProfileConverter.ConvertFrom(A<DynamicContent>._)).MustNotHaveHappened();
            //}
            A.CallTo(() => fakeRepository.GetMany(A<Expression<Func<DynamicContent, bool>>>.That.Matches(m => LinqExpressionsTestHelper.IsExpressionEqual(m, item => item.Status == ContentLifecycleStatus.Live && item.Visible)))).MustHaveHappened();
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void UpdateDigitalSkillTest(bool jobProfileAvailable)
        {
            // Arrange
            var urlname = "test-url";
            var digitalSkill = "digiSkill";
            var jobProfile = new ImportJobProfile { UrlName = urlname, DigitalSkillsLevel = digitalSkill };
            var dummyDynamicContent = A.Dummy<DynamicContent>();

            // Fakes Setup
            A.CallTo(() => fakeRepository.Get(A<Expression<Func<DynamicContent, bool>>>._))
                .Returns(jobProfileAvailable ? dummyDynamicContent : null);
            A.CallTo(() => fakeRepository.GetMaster(dummyDynamicContent)).Returns(dummyDynamicContent);
            A.CallTo(() => fakeRepository.GetTemp(dummyDynamicContent)).Returns(dummyDynamicContent);
            A.CallTo(() => fakeRepository.CheckinTemp(dummyDynamicContent)).Returns(dummyDynamicContent);
            A.CallTo(() => fakeRepository.Publish(dummyDynamicContent, digitalSkill)).DoesNothing();
            A.CallTo(() => fakeRepository.Commit()).DoesNothing();
            A.CallTo(() => fakeRepository.CheckinTemp(dummyDynamicContent)).Returns(dummyDynamicContent);
            A.CallTo(() =>
                fakeDynamicContentExtensions.SetFieldValue(dummyDynamicContent, nameof(JobProfile.DigitalSkillsLevel), A<string>._)).DoesNothing();
            A.CallTo(() => fakeRepository.Get(A<Expression<Func<DynamicContent, bool>>>._))
                .Returns(jobProfileAvailable ? dummyDynamicContent : null);

            var jobProfileRepository = new JobProfileRepository(fakeRepository, fakeJobProfileConverter, fakeDynamicContentExtensions, fakeSocSkillRepo, importfakeJobProfileConverter);

            // Act
            jobProfileRepository.UpdateDigitalSkill(jobProfile);

            // Assert
            A.CallTo(() => fakeRepository.Get(A<Expression<Func<DynamicContent, bool>>>.That.Matches(m =>
                    LinqExpressionsTestHelper.IsExpressionEqual(m, item => item.UrlName == jobProfile.UrlName && item.Status == ContentLifecycleStatus.Live &&
                                item.Visible))))
                .MustHaveHappened();

            if (jobProfileAvailable)
            {
                A.CallTo(() => fakeRepository.GetMaster(dummyDynamicContent)).MustHaveHappened();
                A.CallTo(() => fakeRepository.GetTemp(dummyDynamicContent)).MustHaveHappened();
                A.CallTo(() => fakeRepository.CheckinTemp(dummyDynamicContent)).MustHaveHappened();
                A.CallTo(() => fakeRepository.Publish(dummyDynamicContent, A<string>._)).MustHaveHappened();
                A.CallTo(() => fakeRepository.Commit()).MustHaveHappened();
                A.CallTo(() => fakeRepository.CheckinTemp(dummyDynamicContent)).MustHaveHappened();
                A.CallTo(() =>
                    fakeDynamicContentExtensions.SetFieldValue(dummyDynamicContent, nameof(JobProfile.DigitalSkillsLevel), A<string>._)).MustHaveHappened();
            }
            else
            {
                A.CallTo(() => fakeRepository.GetMaster(dummyDynamicContent)).MustNotHaveHappened();
                A.CallTo(() => fakeRepository.GetTemp(dummyDynamicContent)).MustNotHaveHappened();
                A.CallTo(() => fakeRepository.CheckinTemp(dummyDynamicContent)).MustNotHaveHappened();
                A.CallTo(() => fakeRepository.Publish(dummyDynamicContent, A<string>._)).MustNotHaveHappened();
                A.CallTo(() => fakeRepository.Commit()).MustNotHaveHappened();
                A.CallTo(() => fakeRepository.CheckinTemp(dummyDynamicContent)).MustNotHaveHappened();
                A.CallTo(() =>
                    fakeDynamicContentExtensions.SetFieldValue(dummyDynamicContent, nameof(JobProfile.DigitalSkillsLevel), A<string>._)).MustNotHaveHappened();
            }
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void UpdateSocSkillMatricesTest(bool jobProfileAvailable)
        {
            // Arrange
            var urlname = "test-url";
            var digitalSkill = "digiSkill";
            var jobProfile = new ImportJobProfile { UrlName = urlname, DigitalSkillsLevel = digitalSkill };
            var dummyDynamicContent = A.Dummy<DynamicContent>();
            var socSkill = new SocSkillMatrix { Title = "test soc" };
            var socSkills = new List<SocSkillMatrix> { socSkill };

            // Fakes Setup
            A.CallTo(() => fakeRepository.Get(A<Expression<Func<DynamicContent, bool>>>._))
                .Returns(jobProfileAvailable ? dummyDynamicContent : null);
            A.CallTo(() => fakeRepository.GetMaster(dummyDynamicContent)).Returns(dummyDynamicContent);
            A.CallTo(() => fakeRepository.Publish(dummyDynamicContent, digitalSkill)).DoesNothing();
            A.CallTo(() => fakeRepository.Commit()).DoesNothing();
            A.CallTo(() =>
                fakeDynamicContentExtensions.SetRelatedFieldValue(dummyDynamicContent, dummyDynamicContent, A<string>._)).DoesNothing();
            A.CallTo(() =>
                fakeDynamicContentExtensions.DeleteRelatedFieldValues(dummyDynamicContent, A<string>._)).DoesNothing();
            A.CallTo(() => fakeRepository.Get(A<Expression<Func<DynamicContent, bool>>>._))
                .Returns(jobProfileAvailable ? dummyDynamicContent : null);
            A.CallTo(() => fakeSocSkillRepo.Get(A<Expression<Func<DynamicContent, bool>>>._))
                .Returns(dummyDynamicContent);

            var jobProfileRepository = new JobProfileRepository(fakeRepository, fakeJobProfileConverter, fakeDynamicContentExtensions, fakeSocSkillRepo, importfakeJobProfileConverter);

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
                A.CallTo(() => fakeRepository.Publish(dummyDynamicContent, A<string>._)).MustHaveHappened();
                A.CallTo(() => fakeRepository.Commit()).MustHaveHappened();
                   A.CallTo(() =>
                    fakeDynamicContentExtensions.SetRelatedFieldValue(A<DynamicContent>._, A<DynamicContent>._, "RelatedSkills")).MustHaveHappened();
                A.CallTo(() =>
                        fakeDynamicContentExtensions.DeleteRelatedFieldValues(A<DynamicContent>._, A<string>._))
                    .MustHaveHappened();
                A.CallTo(() => fakeSocSkillRepo.Get(A<Expression<Func<DynamicContent, bool>>>.That.Matches(m =>
                    LinqExpressionsTestHelper.IsExpressionEqual(m, d => d.Status == ContentLifecycleStatus.Master && d.UrlName == socSkill.SfUrlName)))).MustHaveHappened();
            }
            else
            {
                A.CallTo(() => fakeRepository.GetMaster(dummyDynamicContent)).MustNotHaveHappened();
                A.CallTo(() => fakeRepository.Publish(dummyDynamicContent, A<string>._)).MustNotHaveHappened();
                A.CallTo(() => fakeRepository.Commit()).MustNotHaveHappened();
                A.CallTo(() => fakeRepository.CheckinTemp(dummyDynamicContent)).MustNotHaveHappened();
                 A.CallTo(() =>
                    fakeDynamicContentExtensions.SetRelatedFieldValue(A<DynamicContent>._, A<DynamicContent>._, A<string>._)).MustNotHaveHappened();
                A.CallTo(() =>
                    fakeDynamicContentExtensions.DeleteRelatedFieldValues(A<DynamicContent>._, A<string>._)).MustNotHaveHappened();
                A.CallTo(() => fakeSocSkillRepo.Get(A<Expression<Func<DynamicContent, bool>>>._))
                    .MustNotHaveHappened();
            }
        }
    }
}