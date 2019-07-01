using DFC.Digital.Data.Model;
using DFC.Digital.Repository.SitefinityCMS.Modules;
using FakeItEasy;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Telerik.Sitefinity.DynamicModules.Model;
using Xunit;

namespace DFC.Digital.Repository.SitefinityCMS.UnitTests
{
    public class StructuredDataInjectionRepositoryTests : MemberDataHelper
    {
        #region Fields
        private readonly IDynamicModuleRepository<StructuredDataInjection> fakeStructuredDataDynamicModuleRepository;
        private readonly IDynamicModuleConverter<StructuredDataInjection> fakeConverter;
        private readonly IDynamicModuleRepository<JobProfile> fakeJobprofileRepository;
        private readonly IDynamicContentExtensions fakeDynamicContentExtensions;
        private readonly StructuredDataInjectionRepository structuredDataInjectionRepository;
        private readonly DynamicContent dummyDynamicContent;

        #endregion Fields

        #region Ctor

        public StructuredDataInjectionRepositoryTests()
        {
            fakeStructuredDataDynamicModuleRepository = A.Fake<IDynamicModuleRepository<StructuredDataInjection>>(ops => ops.Strict());
            fakeConverter = A.Fake<IDynamicModuleConverter<StructuredDataInjection>>(ops => ops.Strict());
            fakeJobprofileRepository = A.Fake<IDynamicModuleRepository<JobProfile>>(ops => ops.Strict());
            fakeDynamicContentExtensions = A.Fake<IDynamicContentExtensions>(ops => ops.Strict());
            structuredDataInjectionRepository = GetStructuredDataInjectionRepository();
            dummyDynamicContent = A.Dummy<DynamicContent>();
            SetupCalls();
        }

        #endregion

        #region Tests

        [Theory]
        [MemberData(nameof(Dfc9493GetByJobProfileUrlNameTestsInput))]
        public void GetByJobProfileUrlNameTests(string urlName, bool validJobProfile, bool validDataInjection)
        {
            //Assign
            //Setup Fakes
            A.CallTo(() => fakeJobprofileRepository.Get(A<Expression<Func<DynamicContent, bool>>>._)).Returns(validJobProfile ? dummyDynamicContent : null);
            A.CallTo(() => fakeDynamicContentExtensions.GetRelatedParentItems(A<DynamicContent>._, A<string>._, A<string>._)).Returns(validDataInjection ? new EnumerableQuery<DynamicContent>(new List<DynamicContent> { dummyDynamicContent }) : Enumerable.Empty<DynamicContent>().AsQueryable());
            A.CallTo(() => fakeConverter.ConvertFrom(A<DynamicContent>._)).Returns(new StructuredDataInjection());

            //Act
            var result = structuredDataInjectionRepository.GetByJobProfileUrlName(urlName);

            //Assert
            A.CallTo(() => fakeJobprofileRepository.Get(A<Expression<Func<DynamicContent, bool>>>._))
                .MustHaveHappenedOnceExactly();
            if (validDataInjection)
            {
                A.CallTo(() => fakeConverter.ConvertFrom(A<DynamicContent>._))
                    .MustHaveHappenedOnceExactly();
            }

            if (!validDataInjection || !validJobProfile)
            {
                result.Should().BeNull();
            }
        }

        [Fact]
        public void GetContentTypeTest()
        {
            //Assign
            //Act
            structuredDataInjectionRepository.GetContentType();

            //Assert
            A.CallTo(() => fakeStructuredDataDynamicModuleRepository.GetContentType())
                .MustHaveHappenedOnceExactly();
        }

        [Fact]
        public void GetProviderNameTest()
        {
            //Assign
            //Act
            structuredDataInjectionRepository.GetProviderName();

            //Assert
            A.CallTo(() => fakeStructuredDataDynamicModuleRepository.GetProviderName())
                .MustHaveHappenedOnceExactly();
        }

        #endregion

        #region Helpers

        private StructuredDataInjectionRepository GetStructuredDataInjectionRepository()
        {
            return new StructuredDataInjectionRepository(fakeDynamicContentExtensions, fakeJobprofileRepository, fakeStructuredDataDynamicModuleRepository, fakeConverter);
        }

        private void SetupCalls()
        {
            A.CallTo(() => fakeStructuredDataDynamicModuleRepository.GetContentType()).Returns(null);
            A.CallTo(() => fakeStructuredDataDynamicModuleRepository.GetProviderName()).Returns("provider");
        }
        #endregion

    }
}