using Xunit;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using FakeItEasy;
using FluentAssertions;
using DFC.Digital.Repository.ONET.DataModel;
using DFC.Digital.Repository.ONET.Query;
using DFC.Digital.Data.Model;

namespace DFC.Digital.Repository.ONET.UnitTests
{
    public class SkillsQueryRepositoryTests
    {
        private const string oNetOccupationCodeForTestJP = "OnetCode1";
        private const string oNetOccupationCodeForAnotherOtherJP = "OnetCode2";

        [Fact]
        public void GetAbilitiesTest()
        {
            //Setup
            var fakeDbContext = A.Fake<OnetSkillsFramework>();
            var repo = new SkillsOueryRepository(fakeDbContext);

            var fakeAbilitiesDbSet = A.Fake<DbSet<ability>>(c => c
                   .Implements(typeof(IQueryable<ability>))
                   .Implements(typeof(IDbAsyncEnumerable<ability>)))
                   .SetupData(GetTestAbilitiesTableData());

            A.CallTo(() => fakeDbContext.abilities).Returns(fakeAbilitiesDbSet);

            //Act
            var results = repo.GetAbilitiesForONetOccupationCode(oNetOccupationCodeForTestJP);

            //Assert
            results.Count().Should().Be(2);

            int index = 0;

            var expectedResults = GetTestAbilitiesTableData().Where(a => a.onetsoc_code == oNetOccupationCodeForTestJP && a.recommend_suppress != "Y" && a.not_relevant != "Y").ToList();

            foreach (var r in results)
            {
                r.Id.Should().Be(expectedResults[index].element_id);
                r.Score.Should().Be(expectedResults[index++].data_value);
                r.OnetOccupationalCode.Should().Be(oNetOccupationCodeForTestJP);
                r.Type.Should().Be(AttributeType.Ability);
            }

        }

        [Fact]
        public void GetKowledgeTest()
        {
            //Setup
            var fakeDbContext = A.Fake<OnetSkillsFramework>();
            var repo = new SkillsOueryRepository(fakeDbContext);

            var fakeKnowledgeDbSet = A.Fake<DbSet<knowledge>>(c => c
                   .Implements(typeof(IQueryable<knowledge>))
                   .Implements(typeof(IDbAsyncEnumerable<knowledge>)))
                   .SetupData(GetTestKnowledgeTableData());

            A.CallTo(() => fakeDbContext.knowledges).Returns((fakeKnowledgeDbSet));

            //Act
            var results = repo.GetKowledgeForONetOccupationCode(oNetOccupationCodeForTestJP);

            //Assert
            results.Count().Should().Be(2);

            int index = 0;

            var expectedResults = GetTestKnowledgeTableData().Where(a => a.onetsoc_code == oNetOccupationCodeForTestJP && a.recommend_suppress != "Y" && a.not_relevant != "Y").ToList();

            foreach (var r in results)
            {
                r.Id.Should().Be(expectedResults[index].element_id);
                r.Score.Should().Be(expectedResults[index++].data_value);
                r.OnetOccupationalCode.Should().Be(oNetOccupationCodeForTestJP);
                r.Type.Should().Be(AttributeType.Knowledge);
            }

        }

        [Fact]
        public void GetSkillsTest()
        {
            //Setup
            var fakeDbContext = A.Fake<OnetSkillsFramework>();
            var repo = new SkillsOueryRepository(fakeDbContext);

            var fakeSkillDbSet = A.Fake<DbSet<skill>>(c => c
                   .Implements(typeof(IQueryable<skill>))
                   .Implements(typeof(IDbAsyncEnumerable<skill>)))
                   .SetupData(GetTestSkillTableData());

            A.CallTo(() => fakeDbContext.skills).Returns((fakeSkillDbSet));

            //Act
            var results = repo.GetSkillsForONetOccupationCode(oNetOccupationCodeForTestJP);

            //Assert
            results.Count().Should().Be(2);

            int index = 0;

            var expectedResults = GetTestSkillTableData().Where(a => a.onetsoc_code == oNetOccupationCodeForTestJP && a.recommend_suppress != "Y" && a.not_relevant != "Y").ToList();

            foreach (var r in results)
            {
                r.Id.Should().Be(expectedResults[index].element_id);
                r.Score.Should().Be(expectedResults[index++].data_value);
                r.OnetOccupationalCode.Should().Be(oNetOccupationCodeForTestJP);
                r.Type.Should().Be(AttributeType.Skill);
            }

        }

        [Fact]
        public void GetWorkStylesTest()
        {
            //Setup
            var fakeDbContext = A.Fake<OnetSkillsFramework>();
            var repo = new SkillsOueryRepository(fakeDbContext);

            var fakeWorkStylesDbSet = A.Fake<DbSet<work_styles>>(c => c
                   .Implements(typeof(IQueryable<work_styles>))
                   .Implements(typeof(IDbAsyncEnumerable<work_styles>)))
                   .SetupData(GetTestWorkStylesTableData());

            A.CallTo(() => fakeDbContext.work_styles).Returns((fakeWorkStylesDbSet));

            //Act
            var results = repo.GetWorkStylesForONetOccupationCode(oNetOccupationCodeForTestJP);

            //Assert
            results.Count().Should().Be(2);

            int index = 0;

            var expectedResults = GetTestWorkStylesTableData().Where(a => a.onetsoc_code == oNetOccupationCodeForTestJP && a.recommend_suppress != "Y").ToList();

            foreach (var r in results)
            {
                r.Id.Should().Be(expectedResults[index].element_id);
                r.Score.Should().Be(expectedResults[index++].data_value);
                r.OnetOccupationalCode.Should().Be(oNetOccupationCodeForTestJP);
                r.Type.Should().Be(AttributeType.WorkStyle);
            }
        }

        private List<ability> GetTestAbilitiesTableData()
        {
            List<ability> testData = new List<ability>
            {
                new ability { element_id = "A1", data_value = 1, onetsoc_code = oNetOccupationCodeForTestJP },
                new ability { element_id = "A2", data_value = 2, onetsoc_code = oNetOccupationCodeForTestJP },
                new ability { element_id = "not_relevant", not_relevant = "Y", onetsoc_code = oNetOccupationCodeForTestJP },
                new ability { element_id = "recommend_suppress", recommend_suppress = "Y", onetsoc_code = oNetOccupationCodeForTestJP },
                new ability { element_id = "A1", data_value = 1, onetsoc_code = oNetOccupationCodeForAnotherOtherJP }
            };

            return testData;
        }
       
        private List<skill> GetTestSkillTableData()
        {
            List<skill> testData = new List<skill>
            {
                new skill { element_id = "A1", data_value = 1, onetsoc_code = oNetOccupationCodeForTestJP },
                new skill { element_id = "A2", data_value = 2, onetsoc_code = oNetOccupationCodeForTestJP },
                new skill { element_id = "not_relevant", not_relevant = "Y", onetsoc_code = oNetOccupationCodeForTestJP },
                new skill { element_id = "recommend_suppress", recommend_suppress = "Y", onetsoc_code = oNetOccupationCodeForTestJP },
                new skill { element_id = "A1", data_value = 1, onetsoc_code = oNetOccupationCodeForAnotherOtherJP }
            };

            return testData;
        }

        private List<knowledge> GetTestKnowledgeTableData()
        {
            List<knowledge> testData = new List<knowledge>
            {
                new knowledge { element_id = "A1", data_value = 1, onetsoc_code = oNetOccupationCodeForTestJP },
                new knowledge { element_id = "A2", data_value = 2, onetsoc_code = oNetOccupationCodeForTestJP },
                new knowledge { element_id = "not_relevant", not_relevant = "Y", onetsoc_code = oNetOccupationCodeForTestJP },
                new knowledge { element_id = "recommend_suppress", recommend_suppress = "Y", onetsoc_code = oNetOccupationCodeForTestJP },
                new knowledge { element_id = "A1", data_value = 1, onetsoc_code = oNetOccupationCodeForAnotherOtherJP }
            };

            return testData;
        }

        private List<work_styles> GetTestWorkStylesTableData()
        {
            List<work_styles> testData = new List<work_styles>
            {
                new work_styles { element_id = "A1", data_value = 1, onetsoc_code = oNetOccupationCodeForTestJP },
                new work_styles { element_id = "A2", data_value = 2, onetsoc_code = oNetOccupationCodeForTestJP },
                new work_styles { element_id = "recommend_suppress", recommend_suppress = "Y", onetsoc_code = oNetOccupationCodeForTestJP },
                new work_styles { element_id = "A1", data_value = 1, onetsoc_code = oNetOccupationCodeForAnotherOtherJP }
            };

            return testData;
        }
    }
}