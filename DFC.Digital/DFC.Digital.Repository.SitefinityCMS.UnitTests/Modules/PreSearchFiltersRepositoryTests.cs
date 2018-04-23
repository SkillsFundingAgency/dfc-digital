using DFC.Digital.Data.Model;
using FakeItEasy;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Telerik.Sitefinity.DynamicModules.Model;
using Xunit;

namespace DFC.Digital.Repository.SitefinityCMS.Modules.Tests
{
    public class PreSearchFiltersRepositoryTests
    {
        private readonly IDynamicModuleRepository<PreSearchFilter> fakeRepository;
        private readonly IDynamicModuleConverter<PreSearchFilter> fakeModuleConverter;
        private readonly DynamicContent dynamicContentItem;

        public PreSearchFiltersRepositoryTests()
        {
            fakeRepository = A.Fake<IDynamicModuleRepository<PreSearchFilter>>();
            fakeModuleConverter = A.Fake<IDynamicModuleConverter<PreSearchFilter>>();
            dynamicContentItem = A.Dummy<DynamicContent>();
        }

        [Fact]
        public void GetAllFiltersTest()
        {
           //Assign
           SetupCalls();
           var psfRepo = new PreSearchFiltersRepository<PreSearchFilter>(fakeRepository, fakeModuleConverter);

           //Act
          var result = psfRepo.GetAllFilters();

            //Assert
            A.CallTo(() => fakeRepository.GetMany(A<Expression<Func<DynamicContent, bool>>>._)).MustHaveHappened();
            result.FirstOrDefault()?.Description.Should().Contain(nameof(PreSearchFilter.Description));

            //A.CallTo(() => fakeModuleConverter.ConvertFrom(A<DynamicContent>._)).MustHaveHappened();
        }

        private void SetupCalls()
        {
            A.CallTo(() => fakeRepository.GetMany(A<Expression<Func<DynamicContent, bool>>>._)).Returns(new List<DynamicContent> { dynamicContentItem, dynamicContentItem }.ToList().AsQueryable());
            A.CallTo(() => fakeModuleConverter.ConvertFrom(A<DynamicContent>._)).Returns(new PreSearchFilter { Description = nameof(PreSearchFilter.Description) });
        }
    }
}