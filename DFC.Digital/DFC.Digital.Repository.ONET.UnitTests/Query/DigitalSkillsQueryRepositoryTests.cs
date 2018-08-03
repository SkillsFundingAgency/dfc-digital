using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using DFC.Digital.Repository.ONET.DataModel;
using FakeItEasy;
using FluentAssertions;
using DFC.Digital.Repository.ONET.Query;
using Xunit;
namespace DFC.Digital.Repository.ONET.UnitTests
{
    public class DigitalSkillsQueryRepositoryTests:HelperOnetDatas
    {
        [Theory]
        [MemberData(nameof(OnetDigitalSkills))]
        public void GetDigitalSkillsLevel(IReadOnlyCollection<tools_and_technology> setupDataToolsAndTechnologies, IReadOnlyCollection<unspsc_reference> setupDataUnspscReferences, string onetSocCode, int numberOfRecords, int applicationCount)
        {
            //Arrange

            var setupTools = new List<tools_and_technology>(numberOfRecords);
            setupTools.AddRange(Enumerable.Repeat(setupDataToolsAndTechnologies.ToList()[0], numberOfRecords));

            var setupUnspscReferences = new List<unspsc_reference>(numberOfRecords);
            setupUnspscReferences.AddRange(Enumerable.Repeat(setupDataUnspscReferences.ToList()[0], numberOfRecords));


            var fakeDbContext = A.Fake<OnetSkillsFramework>();
            var fakeToolsAndTechnologyDbSet = A.Fake<DbSet<tools_and_technology>>(c => c
                    .Implements(typeof(IQueryable<tools_and_technology>))
                    .Implements(typeof(System.Data.Entity.Infrastructure.IDbAsyncEnumerable<tools_and_technology>)))
                .SetupData(setupTools.ToList());

            var fakeUnspcDataSet = A.Fake<DbSet<unspsc_reference>>(c => c
                    .Implements(typeof(IQueryable<unspsc_reference>))
                    .Implements(typeof(System.Data.Entity.Infrastructure.IDbAsyncEnumerable<unspsc_reference>)))
                .SetupData(setupUnspscReferences.ToList());

            //Act
            A.CallTo(() => fakeDbContext.tools_and_technology).Returns(fakeToolsAndTechnologyDbSet);
            A.CallTo(() => fakeDbContext.unspsc_reference).Returns(fakeUnspcDataSet);


            var repo = new DigitalSkillsQueryRepository(fakeDbContext);

            //Assert
            var result = repo.GetById(onetSocCode);
            result.Should().NotBeNull();
            result.ApplicationCount.Should().NotBe(0);
            result.ApplicationCount.Should().Be(applicationCount);
        }

    }
}