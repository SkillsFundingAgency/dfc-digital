//code Review:  TK please fix the using statements (Done- Dinesh)
using Xunit;
using DFC.Digital.Data.Model;
using FakeItEasy;
using DFC.Digital.Repository.ONET.DataModel;
using AutoMapper;
using FluentAssertions;
using DFC.Digital.Service.SkillsFramework;
using DFC.Digital.Data.Interfaces;
using DFC.Digital.Repository.ONET.Mapper;
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
            //Arrange
            var fakeFrameworkSkillSuppression = A.Fake<IQueryRepository<FrameworkSkillSuppression>>();
            var fakeCombinationSkill = A.Fake<IQueryRepository<FrameWorkSkillCombination>>();
            var fakeContentReference = A.Fake<IQueryRepository<FrameWorkContent>>();
            var fakeskillsRepository = A.Fake<ISkillsRepository>();

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
            var fakeFrameworkSkillSuppression = A.Fake<IQueryRepository<FrameworkSkillSuppression>>();
            var fakeCombinationSkill = A.Fake<IQueryRepository<FrameWorkSkillCombination>>();
            var fakeContentReference = A.Fake<IQueryRepository<FrameWorkContent>>();

            var fakeskillsRepository = A.Fake<ISkillsRepository>();

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

        private List<OnetAttribute> GetTestAttribute(AttributeType type)
        {
            List<OnetAttribute> testData = new List<OnetAttribute>
            {
                new OnetAttribute { Id = $"A1", Type = type}
            };
            return testData;
        }

    }
}