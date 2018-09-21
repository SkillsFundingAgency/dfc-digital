using Xunit;
using DFC.Digital.Data.Model;
using FakeItEasy;
using DFC.Digital.Repository.ONET.DataModel;
using FluentAssertions;
using DFC.Digital.Service.SkillsFramework;
using DFC.Digital.Data.Interfaces;
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
        private readonly IQueryRepository<FrameworkContent> fakeContentReference;
        private readonly IQueryRepository<FrameworkSkillCombination> fakeCombinationSkill;
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
            fakeCombinationSkill = A.Fake<IQueryRepository<FrameworkSkillCombination>>();
            fakeContentReference = A.Fake<IQueryRepository<FrameworkContent>>();
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
            var fakeAbilityDbSet = A.Fake<DbSet<OnetSkill>>(c => c
                   .Implements(typeof(IQueryable<OnetSkill>))
                   .Implements(typeof(IDbAsyncEnumerable<OnetSkill>)))
                   .SetupData(GetTestAttribute(CategoryType.Ability));

            var fakeKowledgeDbSet = A.Fake<DbSet<OnetSkill>>(c => c
                    .Implements(typeof(IQueryable<OnetSkill>))
                    .Implements(typeof(IDbAsyncEnumerable<OnetSkill>)))
                    .SetupData(GetTestAttribute(CategoryType.Knowledge));


            var fakeSkillDbSet = A.Fake<DbSet<OnetSkill>>(c => c
                    .Implements(typeof(IQueryable<OnetSkill>))
                    .Implements(typeof(IDbAsyncEnumerable<OnetSkill>)))
                    .SetupData(GetTestAttribute(CategoryType.Skill));


            var fakeWorkStyleDbSet = A.Fake<DbSet<OnetSkill>>(c => c
                    .Implements(typeof(IQueryable<OnetSkill>))
                    .Implements(typeof(IDbAsyncEnumerable<OnetSkill>)))
                    .SetupData(GetTestAttribute(CategoryType.WorkStyle));


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
        public void AverageOutscoresScalesTest()
        {
            //Setup
            ISkillFrameworkBusinessRuleEngine ruleEngine = new SkillFrameworkBusinessRuleEngine(fakeskillsRepository, fakeFrameworkSkillSuppression, fakeCombinationSkill, fakeContentReference);

            List<OnetSkill> testAttributeAverageData = new List<OnetSkill>
            {

                //Abilities should get avearged and workStyle should remain as is
                GetOnetAttribute(CategoryType.Ability, 1, KeyLength.nine),
                GetOnetAttribute(CategoryType.Ability, 1, KeyLength.nine),
                GetOnetAttribute(CategoryType.WorkStyle, 3, KeyLength.nine)
            };

            //make score for first ability 5
            testAttributeAverageData[0].Score = 5;

            //Act
            var results = ruleEngine.AverageOutscoreScales(testAttributeAverageData);

            //Asserts
            //Abititles should have got grouped and hence should have only 2 records.
            results.Count().Should().Be(2);

            //Score for abilities should be (1+5)/2
            results.Where(a => a.Category == CategoryType.Ability).FirstOrDefault().Score.Should().Be(3);
        }

        [Fact]
        public void MoveBottomLevelAttributesUpOneLevelTest()
        {
            //Setup
            ISkillFrameworkBusinessRuleEngine ruleEngine = new SkillFrameworkBusinessRuleEngine(fakeskillsRepository, fakeFrameworkSkillSuppression, fakeCombinationSkill, fakeContentReference);

            List<OnetSkill> testAttributeMoveLevelsData = new List<OnetSkill>
            {
                //Abilities should get avearged and workStyle should remain as is
                GetOnetAttribute(CategoryType.Ability, 1, KeyLength.nine),
                GetOnetAttribute(CategoryType.Ability, 1, KeyLength.seven),
                GetOnetAttribute(CategoryType.Ability, 1, KeyLength.five),
                GetOnetAttribute(CategoryType.WorkStyle, 1, KeyLength.five),
            };

            //Act
            var results = ruleEngine.MoveBottomLevelAttributesUpOneLevel(testAttributeMoveLevelsData);

            //Asserts
         
            //This is the only thing that should have changed in the list - set it to the expected value
            testAttributeMoveLevelsData[0].Id = (testAttributeMoveLevelsData[0].Id).Substring(0, 7);

            results.Should().BeEquivalentTo(testAttributeMoveLevelsData);
        }

        [Fact]
        public void RemoveDuplicateAttributesTest()
        {
            //Setup
            ISkillFrameworkBusinessRuleEngine ruleEngine = new SkillFrameworkBusinessRuleEngine(fakeskillsRepository, fakeFrameworkSkillSuppression, fakeCombinationSkill, fakeContentReference);

            List<OnetSkill> testAttributeDuplicatesData = new List<OnetSkill>
            {
                GetOnetAttribute(CategoryType.Ability, 1, KeyLength.seven),
                GetOnetAttribute(CategoryType.Ability, 1, KeyLength.seven),
                GetOnetAttribute(CategoryType.WorkStyle, 1, KeyLength.five),
            };

            //make the score for the first dupicate ability higher
            testAttributeDuplicatesData[0].Score = 5;

            //Act
            var results = ruleEngine.RemoveDuplicateAttributes(testAttributeDuplicatesData);

            //Asserts

            //The lower ranking ability should have been removed, so remove it from our test data
            var expectedResults = testAttributeDuplicatesData.FindAll(a => a.Category != CategoryType.Ability || a.Score != 1 ).ToList();

            results.Should().BeEquivalentTo(expectedResults);
        }

        [Fact]
        public void RemoveDFCSuppressionsTest()
        {
            //Setup
            List<OnetSkill> testSuppressionsData = new List<OnetSkill>
            {
                GetOnetAttribute(CategoryType.Ability, 1, KeyLength.seven),
                GetOnetAttribute(CategoryType.Ability, 1, KeyLength.five),
                GetOnetAttribute(CategoryType.WorkStyle, 1, KeyLength.five),
            };

            var indexToSuppress = 1;


            //set the second ability in our test data for suppression 
            var fakeSuppressionsDbSet = A.Fake<DbSet<FrameworkSkillSuppression>>(c => c
                  .Implements(typeof(IQueryable<FrameworkSkillSuppression>))
                  .Implements(typeof(IDbAsyncEnumerable<FrameworkSkillSuppression>)))
                  .SetupData(new List<FrameworkSkillSuppression> { new FrameworkSkillSuppression { ONetElementId = testSuppressionsData[indexToSuppress].Id } }).AsQueryable();

            var fakeSuppressions = A.Fake<IQueryRepository<FrameworkSkillSuppression>>();
            A.CallTo(() => fakeSuppressions.GetAll()).Returns(fakeSuppressionsDbSet);


            ISkillFrameworkBusinessRuleEngine ruleEngine = new SkillFrameworkBusinessRuleEngine(fakeskillsRepository, fakeSuppressions, fakeCombinationSkill, fakeContentReference);

            //Act
            var results = ruleEngine.RemoveDFCSuppressions(testSuppressionsData).ToList();

            //Asserts
            //The second abbility should get suppressioned , so remove it from our test data
            var expectedResults = testSuppressionsData.FindAll(a => a.Id != testSuppressionsData[indexToSuppress].Id).ToList();

            results.Should().BeEquivalentTo(expectedResults);
        }


        [Fact]
        public void AddTitlesTest()
        {
            //Setup
            List<OnetSkill> testTitlesData = new List<OnetSkill>
            {
                GetOnetAttribute(CategoryType.Ability, 1, KeyLength.seven),
                GetOnetAttribute(CategoryType.Ability, 1, KeyLength.five)
            };

            var fakeContentDbSet = A.Fake<DbSet<FrameworkContent>>(c => c
                  .Implements(typeof(IQueryable<FrameworkContent>))
                  .Implements(typeof(IDbAsyncEnumerable<FrameworkContent>)))
                  .SetupData(new List<FrameworkContent> {
                      new FrameworkContent { ONetElementId = testTitlesData[0].Id, Title = "Title 1" },
                      new FrameworkContent { ONetElementId = testTitlesData[1].Id, Title = "Title 2" }
                  }).AsQueryable();

            var fakeContent = A.Fake<IQueryRepository<FrameworkContent>>();
            A.CallTo(() => fakeContent.GetAll()).Returns(fakeContentDbSet);


            ISkillFrameworkBusinessRuleEngine ruleEngine = new SkillFrameworkBusinessRuleEngine(fakeskillsRepository, fakeFrameworkSkillSuppression, fakeCombinationSkill, fakeContent);

            //Act
            var results = ruleEngine.AddTitlesToAttributes(testTitlesData);

            //Asserts
            //update our original list with the expected titles
            testTitlesData[0].Name = "Title 1";
            testTitlesData[1].Name = "Title 2";

            //Results should remain the same as original except for updated name.
            results.Should().BeEquivalentTo(testTitlesData);
        }

        [Fact]
        public void BoostMathsSkillsTest()
        {
            //Setup

            const string maths = "Mathematics";

            List<OnetSkill> testMathsData = new List<OnetSkill>
            {
                GetOnetAttribute(CategoryType.Skill, 1, KeyLength.seven),
                GetOnetAttribute(CategoryType.Knowledge, 5, KeyLength.five),
                GetOnetAttribute(CategoryType.WorkStyle, 5, KeyLength.three)
            };

            //Set the skill and Knowledge items to be maths
            testMathsData[0].Name = maths;
            testMathsData[1].Name = maths;


            ISkillFrameworkBusinessRuleEngine ruleEngine = new SkillFrameworkBusinessRuleEngine(fakeskillsRepository, fakeFrameworkSkillSuppression, fakeCombinationSkill, fakeContentReference);

            //Act
            var results = ruleEngine.BoostMathsSkills(testMathsData).ToList();

            //Asserts
            //The first maths for skills should get removed and the second one for knowledge should have its score boosted by 10% 
            testMathsData[1].Score = testMathsData[1].Score * 1.1m;
            var expectedResults = testMathsData.FindAll(a => a.Category != CategoryType.Skill);

            results.Should().BeEquivalentTo(expectedResults);
        }


        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void BoostMathsSkillsNoSkillsKnowledgeTest(bool mathsAvailable)
        {
            //Setup

            const string maths = "Mathematics";

            List<OnetSkill> testMathsData = new List<OnetSkill>
            {
                GetOnetAttribute(CategoryType.Skill, 10, KeyLength.seven),
                GetOnetAttribute(CategoryType.Knowledge, 5, KeyLength.five),
                GetOnetAttribute(CategoryType.WorkStyle, 5, KeyLength.three)
            };

            //Set the skill and Knowledge items to be maths
            if (mathsAvailable)
            {
                testMathsData[0].Name = maths;
                testMathsData[1].Name = maths;
            }


            ISkillFrameworkBusinessRuleEngine ruleEngine = new SkillFrameworkBusinessRuleEngine(fakeskillsRepository, fakeFrameworkSkillSuppression, fakeCombinationSkill, fakeContentReference);

            //Act
            var results = ruleEngine.BoostMathsSkills(testMathsData).ToList();

            //Asserts
            if (mathsAvailable)
            {
                //The first maths for skills should get removed and the second one for knowledge should have its score boosted by 10% 
                testMathsData[0].Score = testMathsData[0].Score * 1.1m;
                var expectedResults = testMathsData.FindAll(a => a.Category != CategoryType.Knowledge);

                results.Should().BeEquivalentTo(expectedResults);
            }
       
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void CombineSimilarAttributesTest(bool shouldGetCombination)
        {
            //Setup
            //This return 10 each of each of the 4 attributes total 40  items with a max score of 10 
            var testAttributeData = GetAllTestAttribute();

            var testCombination = new FrameworkSkillCombination { OnetElementOneId = "C1", OnetElementTwoId = "C2", CombinedElementId = "CombinedId1", Title = "CombinedTitle1" };
            var combinationsData = new List<FrameworkSkillCombination> {testCombination};

            var scoreForCombination = shouldGetCombination ? 9 : 2;

            //Add records that we know will get combined to the attribute list
            testAttributeData.Add(new OnetSkill { Category = CategoryType.Ability, OnetOccupationalCode = "testONetCode", Id = testCombination.OnetElementOneId, Score = scoreForCombination });
            testAttributeData.Add(new OnetSkill { Category = CategoryType.Skill,   OnetOccupationalCode = "testONetCode", Id = testCombination.OnetElementTwoId, Score = scoreForCombination-1 });


            var fakeCombinationDbSet = A.Fake<DbSet<FrameworkSkillCombination>>(c => c
                .Implements(typeof(IQueryable<FrameworkSkillCombination>))
                .Implements(typeof(IDbAsyncEnumerable<FrameworkSkillCombination>)))
                .SetupData(combinationsData).AsQueryable();

            var fakeCombination = A.Fake<IQueryRepository<FrameworkSkillCombination>>();
            A.CallTo(() => fakeCombination.GetAll()).Returns(fakeCombinationDbSet);

            ISkillFrameworkBusinessRuleEngine ruleEngine = new SkillFrameworkBusinessRuleEngine(fakeskillsRepository, fakeFrameworkSkillSuppression, fakeCombination, fakeContentReference);


            //Act
            var results = ruleEngine.CombineSimilarAttributes(testAttributeData);

            //Asserts
          
            if (shouldGetCombination)
            {
                var expectedResults = GetAllTestAttribute();

                //Add in expected combination
                expectedResults.Add(new OnetSkill { Category = CategoryType.Combination, Name = testCombination.Title, OnetOccupationalCode = "testONetCode", Id = testCombination.CombinedElementId, Score = scoreForCombination });
                //Everything else should reamin as is expect for the combination, if there is one
                results.Should().BeEquivalentTo(expectedResults);
            }
            else
            {
                //should be the same as the data that went in as there is no combination
                results.Should().BeEquivalentTo(testAttributeData);
            }
        }

        [Fact]
        public void SelectFinalAttributesTest()
        {
            //Setup
            ISkillFrameworkBusinessRuleEngine ruleEngine = new SkillFrameworkBusinessRuleEngine(fakeskillsRepository, fakeFrameworkSkillSuppression, fakeCombinationSkill, fakeContentReference);

            var testAttributeData = GetAllTestAttribute();

            //Add in some combinations, one with a high rank and one with a low
            testAttributeData.Add(new OnetSkill { Category = CategoryType.Combination, OnetOccupationalCode = "testONetCode", Id = "C1", Score = 9 });
            testAttributeData.Add(new OnetSkill { Category = CategoryType.Combination, OnetOccupationalCode = "testONetCode", Id = "C2", Score = 1 });


            //Act
            var results = ruleEngine.SelectFinalAttributes(testAttributeData);

            //Asserts
            results.Count().Should().Be(20);
            results.Should().BeInDescendingOrder(a => a.Score);
            results.FirstOrDefault(a => a.Id == "C1").Should().NotBeNull();
        }


        private List<OnetSkill> GetTestAttribute(CategoryType type)
        {
            List<OnetSkill> testData = new List<OnetSkill>
            {
                new OnetSkill { Id = $"A1", Category = type}
            };
            return testData;
        }

        private List<OnetSkill> GetAllTestAttribute()
        {
            List<OnetSkill> testAttributeDataData = new List<OnetSkill>();

            for (int ii = 0; ii < 10; ii++)
            {
                testAttributeDataData.Add(GetOnetAttribute(CategoryType.Ability, ii, KeyLength.seven));
      
                testAttributeDataData.Add(GetOnetAttribute(CategoryType.Knowledge, ii, KeyLength.five));
      
                testAttributeDataData.Add(GetOnetAttribute(CategoryType.Skill, ii, KeyLength.seven));
      
                testAttributeDataData.Add(GetOnetAttribute(CategoryType.WorkStyle, ii, KeyLength.five));
            }

            return testAttributeDataData;
        }

        private OnetSkill GetOnetAttribute(CategoryType type, int id, KeyLength keyLength)
        {
            var keyId = $"{id}-A.B.C.D";
            return new OnetSkill { Id = $"{keyId.Substring(0, (int) keyLength)}", OnetOccupationalCode = "testONetCode", Score = id, Category = type, Name = $"Name-{type}-{id}" };
        }
    }
}