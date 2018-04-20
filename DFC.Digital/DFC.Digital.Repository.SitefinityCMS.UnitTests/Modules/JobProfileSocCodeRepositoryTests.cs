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
    public class JobProfileSocCodeRepositoryTests
    {
        private IDynamicModuleConverter<ApprenticeVacancy> fakeJobProfileSocConverter;
        private IDynamicModuleRepository<SocCode> fakeRepository;
        private IDynamicContentExtensions fakeDynamicContentExtensions;

        public JobProfileSocCodeRepositoryTests()
        {
            fakeJobProfileSocConverter = A.Fake<IDynamicModuleConverter<ApprenticeVacancy>>();
            fakeRepository = A.Fake<IDynamicModuleRepository<SocCode>>();
            fakeDynamicContentExtensions = A.Fake<IDynamicContentExtensions>();
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
        public void GetBySocCodeTest(bool validSoc)
        {
            //Assign
            var fakeRepo = GetTestJobProfileSocCodeRepository(validSoc);

            //Act
           var result = fakeRepo.GetBySocCode(nameof(JobProfileSocCodeRepositoryTest));

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

            A.CallTo(() => fakeRepository.Get(A<Expression<Func<DynamicContent, bool>>>._)).MustHaveHappened();
        }

        private JobProfileSocCodeRepository GetTestJobProfileSocCodeRepository(bool validSoc = false)
        {
            //Setup the fakes and dummies
            var dummySocCode = A.Dummy<DynamicContent>();
            var dummyAppVacancy = A.Dummy<ApprenticeVacancy>();

            var dummyVacancies = validSoc ? A.CollectionOfDummy<DynamicContent>(2).AsEnumerable().AsQueryable() : Enumerable.Empty<DynamicContent>().AsQueryable();
            A.CallTo(() => fakeDynamicContentExtensions.GetRelatedParentItems(A<DynamicContent>._, A<string>._, A<string>._)).Returns(dummyVacancies);

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

            return new JobProfileSocCodeRepository(fakeRepository, fakeJobProfileSocConverter, fakeDynamicContentExtensions);
        }
    }
}