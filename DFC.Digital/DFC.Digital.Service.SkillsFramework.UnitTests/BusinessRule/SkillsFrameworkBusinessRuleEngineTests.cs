using Xunit;
using DFC.Digital.Data.Model;
using FakeItEasy;
using DFC.Digital.Repository.ONET.DataModel;
using FluentAssertions;
using DFC.Digital.Service.SkillsFramework;
using DFC.Digital.Data.Interfaces;
using DFC.Digital.Repository.ONET.Query;

namespace DFC.Digital.Repository.ONET.UnitTests
{

    public class SkillsFrameworkBusinessRuleEngineTests
    {
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

            //Act
            ISkillsRepository skillsRepository = new SkillsOueryRepository(new OnetSkillsFramework());
            ISkillFrameworkBusinessRuleEngine ruleEngine = new SkillFrameworkBusinessRuleEngine(skillsRepository, fakeFrameworkSkillSuppression, fakeCombinationSkill, fakeContentReference);

            //Assert
            var result = ruleEngine.GetDigitalSkillsLevel(input);
            result.Should().Be(expectedLevel);
        }
    }
}