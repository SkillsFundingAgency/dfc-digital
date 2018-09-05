using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using DFC.Digital.Repository.ONET.DataModel;
using FakeItEasy;
using FluentAssertions;
using DFC.Digital.Repository.ONET.Query;
using Xunit;
using DFC.Digital.Core;

namespace DFC.Digital.Repository.ONET.UnitTests
{
    public class DigitalSkillsQueryRepositoryTests : HelperOnetDatas
    {
        [Fact]
        public void GetDigitalSkillsLevel()
        {
            var testONetCode = "test123";

            //Arrange
            var toolsAndTechnology = new List<tools_and_technology>();

            //This is the one record that should count
            toolsAndTechnology.Add(GetToolsAndTechnologyRecord(testONetCode, Constants.Technology));

            //Not a Technology
            toolsAndTechnology.Add(GetToolsAndTechnologyRecord(testONetCode, "DummyTool"));

            //Not linked to test onet code
            toolsAndTechnology.Add(GetToolsAndTechnologyRecord("DummyCode", Constants.Technology));

            var unspscReferences = new List<unspsc_reference>();
            unspscReferences.Add(GetUnspscReferenceRecord());

            var fakeDbContext = A.Fake<OnetSkillsFramework>();
            var fakeToolsAndTechnologyDbSet = A.Fake<DbSet<tools_and_technology>>(c => c
                    .Implements(typeof(IQueryable<tools_and_technology>))
                    .Implements(typeof(System.Data.Entity.Infrastructure.IDbAsyncEnumerable<tools_and_technology>)))
                .SetupData(toolsAndTechnology.ToList());

            var fakeUnspcDataSet = A.Fake<DbSet<unspsc_reference>>(c => c
                    .Implements(typeof(IQueryable<unspsc_reference>))
                    .Implements(typeof(System.Data.Entity.Infrastructure.IDbAsyncEnumerable<unspsc_reference>)))
                .SetupData(unspscReferences.ToList());

            A.CallTo(() => fakeDbContext.tools_and_technology).Returns(fakeToolsAndTechnologyDbSet);
            A.CallTo(() => fakeDbContext.unspsc_reference).Returns(fakeUnspcDataSet);

            //Act
            var repo = new DigitalSkillsQueryRepository(fakeDbContext);
            var result = repo.GetById(testONetCode);

            //Assert
            result.Should().NotBeNull();
            result.ApplicationCount.Should().Be(1);
        }

        private tools_and_technology GetToolsAndTechnologyRecord(string OnetCode, string toolOrTechnology)
        {
            return new tools_and_technology()
            {
                commodity_code = 123,
                hot_technology = "Y",
                onetsoc_code = OnetCode,
                t2_type = toolOrTechnology,
                t2_example = "DummyExample"
            };
        }

        private  unspsc_reference GetUnspscReferenceRecord()
        {
            return new unspsc_reference()
            {
                commodity_code = 123,
                class_title = "Technology",
                class_code = (decimal)0.2,
                commodity_title = "commoditytitle",
                family_code = (decimal)0.4,
                segment_code = (decimal)0.7,
                segment_title = "title"
            };
        }
    }

}