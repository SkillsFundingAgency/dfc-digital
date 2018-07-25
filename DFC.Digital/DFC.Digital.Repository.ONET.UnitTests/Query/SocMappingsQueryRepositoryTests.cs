using Xunit;
using System.Collections.Generic;
using System.Linq;
using FakeItEasy;
using DFC.Digital.Repository.ONET.DataModel;
using AutoMapper;
using DFC.Digital.Repository.ONET.Mapper;
using System.Data.Entity;
using FluentAssertions;
using System.Data.Entity.Infrastructure;

namespace DFC.Digital.Repository.ONET.Query.Tests
{
    using Data.Model;
    using ONET.Tests.Model;
    public class SocMappingsQueryRepositoryTests:HelperOnetDatas
    {
        [Theory]
        [MemberData(nameof(GetAllSocMappingData))]
        public void GetAllTest(List<DFC_SocMappings> setupSocData,List<SocCode> responseData)
        {
            var fakeDbContext = A.Fake<OnetSkillsFramework>();
            var actualMapper = new MapperConfiguration(c => c.AddProfile<SkillsFrameworkMapper>()).CreateMapper();
            var fakeDbSet = A.Fake<DbSet<DFC_SocMappings>>(c => c
                .Implements(typeof(IQueryable<DFC_SocMappings>))
                .Implements(typeof(IDbAsyncEnumerable<DFC_SocMappings>)))
                .SetupData(setupSocData);


            A.CallTo(() => fakeDbContext.DFC_SocMappings).Returns(fakeDbSet);


            var repo = new SocMappingsQueryRepository(fakeDbContext, actualMapper);

            var result = repo.GetAll();
            A.CallTo(() => fakeDbContext.DFC_SocMappings).MustHaveHappened(Repeated.Exactly.Once);
            result.Count().Should().Be(responseData.Count);
            result.Should().BeEquivalentTo(responseData);
            result.Should().Contain(x=>x.SOCCode==setupSocData.First().SocCode);


        }

        [Theory]
        [MemberData(nameof(GetByIdSocMappingData))]
        public void GetByIdTest(List<DFC_SocMappings> setupSocData,SocCode responseData,string socCode)
        {
            var fakeDbContext = A.Fake<OnetSkillsFramework>();
            var actualMapper = new MapperConfiguration(c => c.AddProfile<SkillsFrameworkMapper>()).CreateMapper();
            var fakeDbSet = A.Fake<DbSet<DFC_SocMappings>>(c => c
                    .Implements(typeof(IQueryable<DFC_SocMappings>))
                    .Implements(typeof(IDbAsyncEnumerable<DFC_SocMappings>)))
                .SetupData(setupSocData);


            A.CallTo(() => fakeDbContext.DFC_SocMappings).Returns(fakeDbSet);


            var repo = new SocMappingsQueryRepository(fakeDbContext, actualMapper);

            var result = repo.GetById(socCode);
            A.CallTo(() => fakeDbContext.DFC_SocMappings).MustHaveHappened(Repeated.Exactly.Once);
            result.Should().BeEquivalentTo(responseData);
        }

        [Theory]
        [MemberData(nameof(GetByIdSocMappingData))]
        public void GetTest(List<DFC_SocMappings> setupSocData, SocCode responseData, string socCode)
        {
            var fakeDbContext = A.Fake<OnetSkillsFramework>();
            var actualMapper = new MapperConfiguration(c => c.AddProfile<SkillsFrameworkMapper>()).CreateMapper();
            var fakeDbSet = A.Fake<DbSet<DFC_SocMappings>>(c => c
                    .Implements(typeof(IQueryable<DFC_SocMappings>))
                    .Implements(typeof(IDbAsyncEnumerable<DFC_SocMappings>)))
                .SetupData(setupSocData);


            A.CallTo(() => fakeDbContext.DFC_SocMappings).Returns(fakeDbSet);


            var repo = new SocMappingsQueryRepository(fakeDbContext, actualMapper);

            var result = repo.Get(x=>x.SOCCode==socCode);
            A.CallTo(() => fakeDbContext.DFC_SocMappings).MustHaveHappened(Repeated.Exactly.Once);
            result.Should().BeEquivalentTo(responseData);
        }

        [Theory]
        [MemberData(nameof(GetManySocMappingData))]
        public void GetManyTest(List<DFC_SocMappings> setupData, List<SocCode> responseData, string colData1, string colData2)
        {
            var fakeDbContext = A.Fake<OnetSkillsFramework>();
            var actualMapper = new MapperConfiguration(c => c.AddProfile<SkillsFrameworkMapper>()).CreateMapper();
            var fakeDbSet = A.Fake<DbSet<DFC_SocMappings>>(c => c
                    .Implements(typeof(IQueryable<DFC_SocMappings>))
                    .Implements(typeof(IDbAsyncEnumerable<DFC_SocMappings>)))
                .SetupData(setupData);


            A.CallTo(() => fakeDbContext.DFC_SocMappings).Returns(fakeDbSet);


            var repo = new SocMappingsQueryRepository(fakeDbContext, actualMapper);

            var result = repo.GetMany(x => x.SOCCode == colData1 || x.SOCCode==colData2);
            result.Count().Should().BeGreaterOrEqualTo(2);
            A.CallTo(() => fakeDbContext.DFC_SocMappings).MustHaveHappened(Repeated.Exactly.Once);
            result.Should().BeEquivalentTo(responseData);
        }
    }
}