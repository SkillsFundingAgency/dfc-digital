using Xunit;
using DFC.Digital.Repository.ONET.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FakeItEasy;
using DFC.Digital.Repository.ONET.DataModel;
using AutoMapper;
using DFC.Digital.Repository.ONET.Mapper;
using System.Data.Entity;
using FluentAssertions;

namespace DFC.Digital.Repository.ONET.Query.Tests
{
    public class SocMappingsQueryRepositoryTests
    {
        [Fact()]
        public void GetAllTest()
        {
            OnetSkillsFramework fakeDbContext = A.Fake<OnetSkillsFramework>();
            var actualMapper = new MapperConfiguration(c => c.AddProfile<SkillsFrameworkMapper>()).CreateMapper();
            var fakeDbSet = A.Fake<DbSet<DFC_SocMappings>>();

            fakeDbSet.SetupData(new List<DFC_SocMappings> { new DFC_SocMappings { SocCode = "1234" } });

            A.CallTo(() => fakeDbContext.DFC_SocMappings).Returns(fakeDbSet);


            var repo = new SocMappingsQueryRepository(fakeDbContext, actualMapper);

            var result = repo.GetAll();

            result.Count().Should().Be(1);
            result.First().SOCCode.Should().Be("1234");

            A.CallTo(() => fakeDbContext.DFC_SocMappings).MustHaveHappened(Repeated.Exactly.Once);
        }
    }
}