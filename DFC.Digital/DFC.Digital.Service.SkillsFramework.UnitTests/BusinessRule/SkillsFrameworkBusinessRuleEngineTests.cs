using Xunit;
using DFC.Digital.Data.Model;
using FakeItEasy;
using DFC.Digital.Repository.ONET.DataModel;
using FluentAssertions;
using DFC.Digital.Service.SkillsFramework;
using DFC.Digital.Data.Interfaces;
using DFC.Digital.Repository.ONET.Query;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Data.Entity.Infrastructure;

namespace DFC.Digital.Repository.ONET.UnitTests
{

    public class SkillsFrameworkBusinessRuleEngineTests
    {

        private const string testONetOccupationCode = "TestCode";

        private readonly ISkillsRepository fakeskillsRepository;
        private readonly IQueryRepository<FrameWorkContent> fakeContentReference;
        private readonly IQueryRepository<FrameWorkSkillCombination> fakeCombinationSkill;
        private readonly IQueryRepository<FrameworkSkillSuppression> fakeFrameworkSkillSuppression;

        private enum KeyLength
        {
            three = 3,
            five = 5,
            seven = 7,
            nine = 9
        }

        public SkillsFrameworkBusinessRuleEngineTests()
        {
            //Setup fakes that will get used by multiple tests
            fakeFrameworkSkillSuppression = A.Fake<IQueryRepository<FrameworkSkillSuppression>>();
            fakeCombinationSkill = A.Fake<IQueryRepository<FrameWorkSkillCombination>>();
            fakeContentReference = A.Fake<IQueryRepository<FrameWorkContent>>();
            fakeskillsRepository = A.Fake<ISkillsRepository>();

        }

        [Theory]
        [InlineData(0, DigitalSkillsLevel.Level4)]
        [InlineData(50, DigitalSkillsLevel.Level4)]
        [InlineData(51, DigitalSkillsLevel.Level3)]
        [InlineData(100, DigitalSkillsLevel.Level3)]
        [InlineData(101, DigitalSkillsLevel.Level2)]
        [InlineData(150, DigitalSkillsLevel.Level2)]
        [InlineData(151, DigitalSkillsLevel.Level1)]
        [InlineData(2000, DigitalSkillsLevel.Level1)]
        public void GetDigitalSkillsLevelTest(int input, DigitalSkillsLevel expectedLevel)
        {
            //Act
            ISkillFrameworkBusinessRuleEngine ruleEngine = new SkillFrameworkBusinessRuleEngine(fakeskillsRepository, fakeFrameworkSkillSuppression, fakeCombinationSkill, fakeContentReference);

            //Assert
            var result = ruleEngine.GetDigitalSkillsLevel(input);
            result.Should().Be(expectedLevel);
        }

        [Fact]
        public void GetAllRawOnetSkillsForOccupationTest()
        {
            //Arrange
            var fakeAbilityDbSet = A.Fake<DbSet<OnetAttribute>>(c => c
                   .Implements(typeof(IQueryable<OnetAttribute>))
                   .Implements(typeof(IDbAsyncEnumerable<OnetAttribute>)))
                   .SetupData(GetTestAttribute(AttributeType.Ability));

            var fakeKowledgeDbSet = A.Fake<DbSet<OnetAttribute>>(c => c
                    .Implements(typeof(IQueryable<OnetAttribute>))
                    .Implements(typeof(IDbAsyncEnumerable<OnetAttribute>)))
                    .SetupData(GetTestAttribute(AttributeType.Knowledge));


            var fakeSkillDbSet = A.Fake<DbSet<OnetAttribute>>(c => c
                    .Implements(typeof(IQueryable<OnetAttribute>))
                    .Implements(typeof(IDbAsyncEnumerable<OnetAttribute>)))
                    .SetupData(GetTestAttribute(AttributeType.Skill));


            var fakeWorkStyleDbSet = A.Fake<DbSet<OnetAttribute>>(c => c
                    .Implements(typeof(IQueryable<OnetAttribute>))
                    .Implements(typeof(IDbAsyncEnumerable<OnetAttribute>)))
                    .SetupData(GetTestAttribute(AttributeType.WorkStyle));


            A.CallTo(() => fakeskillsRepository.GetAbilitiesForONetOccupationCode(testONetOccupationCode)).Returns(fakeAbilityDbSet);
            A.CallTo(() => fakeskillsRepository.GetKowledgeForONetOccupationCode(testONetOccupationCode)).Returns(fakeKowledgeDbSet);
            A.CallTo(() => fakeskillsRepository.GetSkillsForONetOccupationCode(testONetOccupationCode)).Returns(fakeSkillDbSet);
            A.CallTo(() => fakeskillsRepository.GetWorkStylesForONetOccupationCode(testONetOccupationCode)).Returns(fakeWorkStyleDbSet);

            ISkillFrameworkBusinessRuleEngine ruleEngine = new SkillFrameworkBusinessRuleEngine(fakeskillsRepository, fakeFrameworkSkillSuppression, fakeCombinationSkill, fakeContentReference);

            //Act
            var results = ruleEngine.GetAllRawOnetSkillsForOccupation(testONetOccupationCode);

            //Asserts

            //should have one for each type 
            results.Count().Should().Be(4);

            A.CallTo(() => fakeskillsRepository.GetAbilitiesForONetOccupationCode(testONetOccupationCode)).MustHaveHappenedOnceExactly();
            A.CallTo(() => fakeskillsRepository.GetKowledgeForONetOccupationCode(testONetOccupationCode)).MustHaveHappenedOnceExactly();
            A.CallTo(() => fakeskillsRepository.GetSkillsForONetOccupationCode(testONetOccupationCode)).MustHaveHappenedOnceExactly();
            A.CallTo(() => fakeskillsRepository.GetWorkStylesForONetOccupationCode(testONetOccupationCode)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public void AverageOutScoresScalesTest()
        {
            //Setup
            ISkillFrameworkBusinessRuleEngine ruleEngine = new SkillFrameworkBusinessRuleEngine(fakeskillsRepository, fakeFrameworkSkillSuppression, fakeCombinationSkill, fakeContentReference);

            List<OnetAttribute> testAttributeAverageData = new List<OnetAttribute>
            {

                //Abilities should get avearged and workStyle should remain as is
                GetOnetAttribute(AttributeType.Ability, 1, KeyLength.nine),
                GetOnetAttribute(AttributeType.Ability, 1, KeyLength.nine),
                GetOnetAttribute(AttributeType.WorkStyle, 3, KeyLength.nine)
            };

            //make score for first ability 5
            testAttributeAverageData[0].Score = 5;

            //Act
            var results = ruleEngine.AverageOutScoreScales(testAttributeAverageData);

            //Asserts
            //Abititles should have got grouped and hence should have only 2 records.
            results.Count().Should().Be(2);

            //Score for abilities should be (1+5)/2
            results.Where(a => a.Type == AttributeType.Ability).FirstOrDefault().Score.Should().Be(3);
        }

        [Fact]
        public void MoveBottomLevelAttributesUpOneLevelTest()
        {
            //Setup
            ISkillFrameworkBusinessRuleEngine ruleEngine = new SkillFrameworkBusinessRuleEngine(fakeskillsRepository, fakeFrameworkSkillSuppression, fakeCombinationSkill, fakeContentReference);

            List<OnetAttribute> testAttributeMoveLevelsData = new List<OnetAttribute>
            {
                //Abilities should get avearged and workStyle should remain as is
                GetOnetAttribute(AttributeType.Ability, 1, KeyLength.nine),
                GetOnetAttribute(AttributeType.Ability, 1, KeyLength.seven),
                GetOnetAttribute(AttributeType.Ability, 1, KeyLength.five),
                GetOnetAttribute(AttributeType.WorkStyle, 1, KeyLength.five),
            };

            //Act
            var results = ruleEngine.MoveBottomLevelAttributesUpOneLevel(testAttributeMoveLevelsData).ToList();

            //Asserts
            results.Count().Should().Be(testAttributeMoveLevelsData.Count());

            //This is the only thing that should have changed in the list - set it to the expected value
            testAttributeMoveLevelsData[0].Id = (testAttributeMoveLevelsData[0].Id).Substring(0, 7);

            results.Should().BeEquivalentTo(testAttributeMoveLevelsData);
        }

        private List<OnetAttribute> GetTestAttribute(AttributeType type)
        {
            List<OnetAttribute> testData = new List<OnetAttribute>
            {
                new OnetAttribute { Id = $"A1", Type = type}
            };
            return testData;
        }

        private List<OnetAttribute> GetAllTestAttribute()
        {
            List<OnetAttribute> testAttributeDataData = new List<OnetAttribute>();

            for (int ii = 0; ii < 10; ii++)
            {
                //Add duplicates for Abilities, skill, Knowledge to test duplicate for diffrent scales in the DB 
                testAttributeDataData.Add(GetOnetAttribute(AttributeType.Ability, ii, KeyLength.nine));
                testAttributeDataData.Add(GetOnetAttribute(AttributeType.Ability, ii + 1, KeyLength.nine));

                testAttributeDataData.Add(GetOnetAttribute(AttributeType.Knowledge, ii, KeyLength.nine));
                testAttributeDataData.Add(GetOnetAttribute(AttributeType.Knowledge, ii + 1, KeyLength.nine));

                testAttributeDataData.Add(GetOnetAttribute(AttributeType.Skill, ii, KeyLength.nine));
                testAttributeDataData.Add(GetOnetAttribute(AttributeType.Skill, ii + 1, KeyLength.nine));

                //Work styles only have one scale
                testAttributeDataData.Add(GetOnetAttribute(AttributeType.WorkStyle, ii, KeyLength.nine));
            }

            return testAttributeDataData;
        }

        private OnetAttribute GetOnetAttribute(AttributeType type, int id, KeyLength keyLength)
        {
            var keyId = $"{id}-A.B.C.D";
            return new OnetAttribute { Id = $"{keyId.Substring(0, (int) keyLength)}", OnetOccupationalCode = "testONetCode", Score = id, Type = type, Name = $"Name-{type}-{id}" };
        }
    }
}