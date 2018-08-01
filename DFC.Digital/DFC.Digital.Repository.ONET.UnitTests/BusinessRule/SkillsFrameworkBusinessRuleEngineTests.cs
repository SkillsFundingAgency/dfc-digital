using Xunit;
using DFC.Digital.Data.Model;
using FakeItEasy;
using DFC.Digital.Repository.ONET.DataModel;
using AutoMapper;
using FluentAssertions;
using DFC.Digital.Service.SkillsFramework;

namespace DFC.Digital.Repository.ONET.UnitTests.BusinessRule
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

            var mapperConfig = new MapperConfiguration(cfg => cfg.AddProfile(new SkillsFrameworkMapper()));
            var mapper = mapperConfig.CreateMapper();
            var fakeframeworkSkill = A.Fake<IQueryRepository<FrameworkSkill>>();
            var fakeCombinationSkill = A.Fake<IQueryRepository<FrameWorkSkillCombination>>();

             ISkillsRepository skillsRepository = new KnowledgeOueryRepository(new OnetSkillsFramework());
            ISkillFrameworkBusinessRuleEngine ruleEngine = new SkillFrameworkBusinessRuleEngine(mapper, skillsRepository, skillsRepository, skillsRepository, skillsRepository, fakeframeworkSkill, fakeCombinationSkill);

            var result = ruleEngine.GetDigitalSkillsLevel(input);

            result.Should().Be(expectedLevel);
        }
        [Theory]
        [InlineData("11-1011.00", DigitalSkillsLevel.Level3)]
        public void GetDigitalSkillsLevel(string onetSocCode, DigitalSkillsLevel expectedLevel)
        {
            var mapperConfig = new MapperConfiguration(cfg => cfg.AddProfile(new SkillsFrameworkMapper()));
            var mapper = mapperConfig.CreateMapper();
            var fakeLogger = A.Fake<IApplicationLogger>();
            var fakeframeworkSkill = A.Fake<IQueryRepository<FrameworkSkill>>();
            var fakeContentReference = A.Fake<IQueryRepository<FrameworkSkill>>();
            var fakeCombinationSkill = A.Fake<IQueryRepository<FrameWorkSkillCombination>>();

            IQueryRepository<SocCode> socCodeRepository = new SocMappingsQueryRepository(new OnetSkillsFramework(), mapper);
            IQueryRepository<DigitalSkill> digitalSkillsRepository = new DigitalSkillsQueryRepository(new OnetSkillsFramework(), mapper);
            IQueryRepository<FrameworkSkill> frameWorkRepository = new TranslationQueryRepository(new OnetSkillsFramework(), mapper);
            ISkillsRepository skillsRepository = new KnowledgeOueryRepository(new OnetSkillsFramework());
            ISkillFrameworkBusinessRuleEngine ruleEngine = new SkillFrameworkBusinessRuleEngine(mapper, skillsRepository, skillsRepository, skillsRepository, skillsRepository, fakeframeworkSkill, fakeCombinationSkill, fakeContentReference);
            ISkillsFrameworkService skillService = new SkillsFrameworkService(fakeLogger, socCodeRepository, digitalSkillsRepository, frameWorkRepository, ruleEngine);
            var result= skillService.GetDigitalSkillLevel(onetSocCode);
            result.Should().Be(expectedLevel);
        }
    }
}