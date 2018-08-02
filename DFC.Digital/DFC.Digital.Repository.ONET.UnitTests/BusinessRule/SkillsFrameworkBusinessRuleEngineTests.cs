//code Review:  TK please fix the using statements

using Xunit;
using DFC.Digital.Data.Model;
using FakeItEasy;
using DFC.Digital.Repository.ONET.DataModel;
using AutoMapper;
using FluentAssertions;
using DFC.Digital.Service.SkillsFramework;

namespace DFC.Digital.Repository.ONET.UnitTests
{
    using Core;
    using Data.Interfaces;
    using Mapper;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using ONET.Query;

    [TestClass]
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
            // CodeReview: Tk Place this in the Ctor so your test is cleaner and easier to follow as this not specific for this test
            var mapperConfig = new MapperConfiguration(cfg => cfg.AddProfile(new SkillsFrameworkMapper()));
            var mapper = mapperConfig.CreateMapper();
            // CodeReview: TK : If you are setting these up you should validate they have been called in your Assertions or set these up in the constructor outside the method if they are not used directly
            var fakeFrameworkSkillSuppression = A.Fake<IQueryRepository<FrameWorkSkillSuppression>>();
            var fakeCombinationSkill = A.Fake<IQueryRepository<FrameWorkSkillCombination>>();
            var fakeContentReference = A.Fake<IQueryRepository<FrameWorkContent>>();
            //Act
            ISkillsRepository skillsRepository = new SkillsOueryRepository(new OnetSkillsFramework());
            ISkillFrameworkBusinessRuleEngine ruleEngine = new SkillFrameworkBusinessRuleEngine(mapper, skillsRepository, fakeFrameworkSkillSuppression, fakeCombinationSkill, fakeContentReference);
            //Assert
            var result = ruleEngine.GetDigitalSkillsLevel(input);
            result.Should().Be(expectedLevel);
        }
        [Theory]
        [InlineData("11-1011.00", DigitalSkillsLevel.Level3)]
        public void GetDigitalSkillsLevel(string onetSocCode, DigitalSkillsLevel expectedLevel)
        {
            //Arrange
            // CodeReview: Tk Place this in the Ctor so your test is cleaner and easier to follow as this not specific for this test
            var mapperConfig = new MapperConfiguration(cfg => cfg.AddProfile(new SkillsFrameworkMapper()));
            var mapper = mapperConfig.CreateMapper();
            var fakeLogger = A.Fake<IApplicationLogger>();
            var fakeFrameworkSkillSuppression = A.Fake<IQueryRepository<FrameworkSkillSuppression>>();
            var fakeContentReference = A.Fake<IQueryRepository<FrameWorkContent>>();
            var fakeCombinationSkill = A.Fake<IQueryRepository<FrameWorkSkillCombination>>();
            var skillsRepository = A.Fake<ISkillsRepository>();
            //Act
            IQueryRepository<SocCode> socCodeRepository = new SocMappingsQueryRepository(new OnetSkillsFramework(), mapper);
            IQueryRepository<DigitalSkill> digitalSkillsRepository = new DigitalSkillsQueryRepository(new OnetSkillsFramework(), mapper);
            IQueryRepository<FrameworkSkill> frameWorkRepository = new TranslationQueryRepository(new OnetSkillsFramework(), mapper);
            ISkillFrameworkBusinessRuleEngine ruleEngine = new SkillFrameworkBusinessRuleEngine(mapper, skillsRepository, fakeFrameworkSkillSuppression, fakeCombinationSkill, fakeContentReference);
            ISkillsFrameworkService skillService = new SkillsFrameworkService(fakeLogger, socCodeRepository, digitalSkillsRepository, frameWorkRepository, ruleEngine);
            //Assert
            // CodeReview: Tk: Cannot see where you have faked your repos to get this result. 
            var result = skillService.GetDigitalSkillLevel(onetSocCode);
            result.Should().Be(expectedLevel);
        }
    }
}