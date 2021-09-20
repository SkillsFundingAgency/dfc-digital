using DFC.Digital.Data.Model;
using DFC.Digital.Repository.SitefinityCMS.UnitTests;
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

namespace DFC.Digital.Repository.SitefinityCMS.Modules.Tests
{
    public class JobProfileSocCodeRepositoryTests
    {
        private IDynamicModuleConverter<ApprenticeVacancy> fakeJobProfileSocConverter;
        private IDynamicModuleConverter<SocCode> fakeSocConverter;
        private IDynamicModuleConverter<JobProfileOverloadForWhatItTakes> fakeConverterLight;
        private IDynamicModuleRepository<SocCode> fakeRepository;
        private IDynamicModuleRepository<JobProfile> fakeJpRepository;
        private IDynamicContentExtensions fakeDynamicContentExtensions;

        public JobProfileSocCodeRepositoryTests()
        {
            fakeJobProfileSocConverter = A.Fake<IDynamicModuleConverter<ApprenticeVacancy>>();
            fakeRepository = A.Fake<IDynamicModuleRepository<SocCode>>();
            fakeJpRepository = A.Fake<IDynamicModuleRepository<JobProfile>>();
            fakeDynamicContentExtensions = A.Fake<IDynamicContentExtensions>();
            fakeSocConverter = A.Fake<IDynamicModuleConverter<SocCode>>();
            fakeConverterLight = A.Fake<IDynamicModuleConverter<JobProfileOverloadForWhatItTakes>>();
        }

        [Fact]
        public void JobProfileSocCodeRepositoryTest()
        {
            //Assign
            //Act
            var fakeRepo = GetTestJobProfileSocCodeRepository();

            //Assert
            fakeRepo.Should().NotBeNull();
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void GetApprenticeVacanciesBySocCodeTest(bool validSoc)
        {
            //Assign
            var fakeRepo = GetTestJobProfileSocCodeRepository(validSoc);
            var socCode = nameof(JobProfileSocCodeRepositoryTest);

            //Act
           var result = fakeRepo.GetApprenticeVacanciesBySocCode(socCode);

            //Assert
            if (validSoc)
            {
                result.Should().NotBeEmpty();
            }
            else
            {
                result.Should().BeEmpty();
            }

            A.CallTo(() => fakeDynamicContentExtensions.GetRelatedParentItems(A<DynamicContent>._, A<string>._, A<string>._)).MustHaveHappened();

            A.CallTo(() => fakeRepository.Get(A<Expression<Func<DynamicContent, bool>>>.That.Matches(m => LinqExpressionsTestHelper.IsExpressionEqual(m, item => item.Visible && item.Status == ContentLifecycleStatus.Live && item.GetValue<string>(nameof(SocCode.SOCCode)) == socCode)))).MustHaveHappened();
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void GetBySocCodeTest(bool validSoc)
        {
            //Assign
            var fakeRepo = GetTestJobProfileSocCodeRepository(validSoc);
            var socCode = nameof(JobProfileSocCodeRepositoryTest);

            //Act
            fakeRepo.GetBySocCode(socCode);

            //Assert
            if (validSoc)
            {
                A.CallTo(() => fakeSocConverter.ConvertFrom(A<DynamicContent>._)).MustHaveHappened();
            }
            else
            {
                A.CallTo(() => fakeSocConverter.ConvertFrom(A<DynamicContent>._)).MustNotHaveHappened();
            }

            A.CallTo(() => fakeRepository.Get(A<Expression<Func<DynamicContent, bool>>>.That.Matches(m => LinqExpressionsTestHelper.IsExpressionEqual(m, item => item.Visible && item.Status == ContentLifecycleStatus.Live && item.GetValue<string>(nameof(SocCode.SOCCode)) == socCode)))).MustHaveHappened();
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void GetSocCodesTest(bool validSoc)
        {
            //Assign
            var fakeRepo = GetTestJobProfileSocCodeRepository();
            var dummyDynamicContent = A.Dummy<DynamicContent>();

            // Setup Fakes
            A.CallTo(() => fakeRepository.GetMany(A<Expression<Func<DynamicContent, bool>>>._)).Returns(validSoc
                ? new EnumerableQuery<DynamicContent>(new List<DynamicContent> { dummyDynamicContent })
                : Enumerable.Empty<DynamicContent>().AsQueryable());

            //Act
            fakeRepo.GetSocCodes();

            //Assert
            A.CallTo(() => fakeRepository.GetMany(A<Expression<Func<DynamicContent, bool>>>.That.Matches(m => LinqExpressionsTestHelper.IsExpressionEqual(m, item => item.Visible && item.Status == ContentLifecycleStatus.Live)))).MustHaveHappened();
        }

        [Theory]
        [InlineData(false, false)]
        [InlineData(true, true)]
        [InlineData(true, false)]
        public void GetLiveJobProfilesBySocCodeTest(bool validSoc, bool jobsAvailable)
        {
            //Assign
            var fakeRepo = GetTestJobProfileSocCodeRepository(validSoc);

            A.CallTo(() => fakeJpRepository.IsCheckedOut(A<DynamicContent>._)).Returns(false);
            var dummyJobProfiles = jobsAvailable ? A.CollectionOfDummy<DynamicContent>(2).AsEnumerable().AsQueryable() : Enumerable.Empty<DynamicContent>().AsQueryable();
            A.CallTo(() => fakeDynamicContentExtensions.GetRelatedParentItems(A<DynamicContent>._, A<string>._, A<string>._)).Returns(dummyJobProfiles);
            var socCode = nameof(JobProfileSocCodeRepositoryTest);

            //Act
            fakeRepo.GetLiveJobProfilesBySocCode(socCode);

            //Assert
            if (validSoc)
            {
                A.CallTo(() => fakeDynamicContentExtensions.GetRelatedParentItems(A<DynamicContent>._, A<string>._, A<string>._)).MustHaveHappened();

                if (jobsAvailable)
                {
                    A.CallTo(() => fakeJpRepository.IsCheckedOut(A<DynamicContent>._)).MustHaveHappened();
                    A.CallTo(() => fakeConverterLight.ConvertFrom(A<DynamicContent>._)).MustHaveHappened();
                }
                else
                {
                    A.CallTo(() => fakeJpRepository.IsCheckedOut(A<DynamicContent>._)).MustNotHaveHappened();
                    A.CallTo(() => fakeConverterLight.ConvertFrom(A<DynamicContent>._)).MustNotHaveHappened();
                }
            }
            else
            {
                A.CallTo(() => fakeDynamicContentExtensions.GetRelatedParentItems(A<DynamicContent>._, A<string>._, A<string>._)).MustNotHaveHappened();
            }

            A.CallTo(() => fakeRepository.Get(A<Expression<Func<DynamicContent, bool>>>.That.Matches(m => LinqExpressionsTestHelper.IsExpressionEqual(m, item => item.Status == ContentLifecycleStatus.Live && item.GetValue<string>(nameof(SocCode.SOCCode)) == socCode)))).MustHaveHappened();
        }

        [Theory(Skip = "LString throwing a null reference exception")]
        [InlineData(false)]
        [InlineData(true)]
        public void GetLiveJobProfilesBySocCodeLockedTest(bool locked)
        {
            //Assign
            var fakeRepo = GetTestJobProfileSocCodeRepository(true);
            A.CallTo(() => fakeJpRepository.IsCheckedOut(A<DynamicContent>._)).Returns(locked);
            var dummyJobProfiles = A.CollectionOfDummy<DynamicContent>(2).AsEnumerable().AsQueryable();
            A.CallTo(() => fakeDynamicContentExtensions.GetRelatedParentItems(A<DynamicContent>._, A<string>._, A<string>._)).Returns(dummyJobProfiles);
            var socCode = nameof(JobProfileSocCodeRepositoryTest);

            //Act
            fakeRepo.GetLiveJobProfilesBySocCode(socCode);

            //Assert
            if (locked)
            {
                A.CallTo(() => fakeConverterLight.ConvertFrom(A<DynamicContent>._)).MustNotHaveHappened();
            }
            else
            {
                A.CallTo(() => fakeConverterLight.ConvertFrom(A<DynamicContent>._)).MustHaveHappened();
            }

            A.CallTo(() => fakeRepository.Get(A<Expression<Func<DynamicContent, bool>>>.That.Matches(m => LinqExpressionsTestHelper.IsExpressionEqual(m, item => item.Status == ContentLifecycleStatus.Live && item.GetValue<string>(nameof(SocCode.SOCCode)) == socCode)))).MustHaveHappened();
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void UpdateSocOccupationalCodeTest(bool validSoc)
        {
            //Assign
            var fakeRepo = GetTestJobProfileSocCodeRepository();
            var socCode = new SocCode();
            var dummyDynamicContent = A.Dummy<DynamicContent>();

            // Setup Fakes
            if (validSoc)
            {
                A.CallTo(() => fakeRepository.Get(A<Expression<Func<DynamicContent, bool>>>._)).Returns(dummyDynamicContent);
            }
            else
            {
                A.CallTo(() => fakeRepository.Get(A<Expression<Func<DynamicContent, bool>>>._)).Returns(null);
            }

            A.CallTo(() => fakeRepository.GetMaster(dummyDynamicContent)).Returns(dummyDynamicContent);
            A.CallTo(() => fakeRepository.GetTemp(dummyDynamicContent)).Returns(dummyDynamicContent);
            A.CallTo(() => fakeRepository.CheckinTemp(dummyDynamicContent)).Returns(dummyDynamicContent);
            A.CallTo(() => fakeRepository.Publish(dummyDynamicContent, A<string>._)).DoesNothing();
            A.CallTo(() => fakeRepository.Commit()).DoesNothing();
            A.CallTo(() => fakeRepository.CheckinTemp(dummyDynamicContent)).Returns(dummyDynamicContent);
            A.CallTo(() =>
                fakeDynamicContentExtensions.SetFieldValue(dummyDynamicContent, nameof(socCode.ONetOccupationalCode), A<string>._)).DoesNothing();

            //Act
            fakeRepo.UpdateSocOccupationalCode(socCode);

            //Assert
            if (validSoc)
            {
                A.CallTo(() => fakeRepository.GetMaster(dummyDynamicContent)).MustHaveHappened();
                A.CallTo(() => fakeRepository.GetTemp(dummyDynamicContent)).MustHaveHappened();
                A.CallTo(() => fakeRepository.CheckinTemp(dummyDynamicContent)).MustHaveHappened();
                A.CallTo(() => fakeRepository.Publish(dummyDynamicContent, A<string>._)).MustHaveHappened();
                A.CallTo(() => fakeRepository.Commit()).MustHaveHappened();
                A.CallTo(() => fakeRepository.CheckinTemp(dummyDynamicContent)).MustHaveHappened();
                A.CallTo(() =>
                    fakeDynamicContentExtensions.SetFieldValue(dummyDynamicContent, nameof(socCode.ONetOccupationalCode), A<string>._)).MustHaveHappened();
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
                    fakeDynamicContentExtensions.SetFieldValue(dummyDynamicContent, nameof(socCode.ONetOccupationalCode), A<string>._)).MustNotHaveHappened();
            }

            A.CallTo(() => fakeRepository.Get(A<Expression<Func<DynamicContent, bool>>>.That.Matches(m => LinqExpressionsTestHelper.IsExpressionEqual(m, item => item.Visible && item.Status == ContentLifecycleStatus.Live && item.UrlName == socCode.UrlName)))).MustHaveHappened();
        }

        private JobProfileSocCodeRepository GetTestJobProfileSocCodeRepository(bool validSoc = false)
        {
            //Setup the fakes and dummies
            var dummySocCode = A.Dummy<DynamicContent>();
            var dummyAppVacancy = A.Dummy<ApprenticeVacancy>();
            var witJp = new JobProfileOverloadForWhatItTakes();

            var dummyVacancies = validSoc ? A.CollectionOfDummy<DynamicContent>(2).AsEnumerable().AsQueryable() : Enumerable.Empty<DynamicContent>().AsQueryable();
            A.CallTo(() => fakeDynamicContentExtensions.GetFieldValue<Lstring>(A<DynamicContent>._, A<string>._)).Returns("test");
            A.CallTo(() => fakeDynamicContentExtensions.GetRelatedParentItems(A<DynamicContent>._, A<string>._, A<string>._)).Returns(dummyVacancies);

            A.CallTo(() => fakeConverterLight.ConvertFrom(A<DynamicContent>._)).Returns(witJp);

            if (validSoc)
            {
                A.CallTo(() => fakeRepository.Get(A<Expression<Func<DynamicContent, bool>>>._)).Returns(dummySocCode);
                A.CallTo(() => fakeJobProfileSocConverter.ConvertFrom(dummySocCode)).Returns(dummyAppVacancy);
            }
            else
            {
                A.CallTo(() => fakeRepository.Get(A<Expression<Func<DynamicContent, bool>>>._)).Returns(null);
                A.CallTo(() => fakeJobProfileSocConverter.ConvertFrom(dummySocCode)).Returns(null);
            }

            return new JobProfileSocCodeRepository(fakeRepository, fakeJobProfileSocConverter, fakeSocConverter, fakeDynamicContentExtensions, fakeConverterLight, fakeJpRepository);
        }
    }
}