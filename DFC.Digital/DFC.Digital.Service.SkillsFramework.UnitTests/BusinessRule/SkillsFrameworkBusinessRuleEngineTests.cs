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
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Linq;
    using Core;
    using Data.Interfaces;
    using Mapper;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using ONET.Query;

    public class SkillsFrameworkBusinessRuleEngineTests:HelperOnetDatas
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
        [MemberData(nameof(OnetDigitalSkills))]
        public void GetDigitalSkillsLevel(IReadOnlyCollection<tools_and_technology> setupDataToolsAndTechnologies,IReadOnlyCollection<unspsc_reference> setupDataUnspscReferences,string onetSocCode,int numberOfRecords,int applicationCount)
        {
            //Arrange

            var setupTools = new List<tools_and_technology>(numberOfRecords);
            setupTools.AddRange(Enumerable.Repeat(setupDataToolsAndTechnologies.ToList()[0], numberOfRecords));

            var setupUnspscReferences = new List<unspsc_reference>(numberOfRecords);
            setupUnspscReferences.AddRange(Enumerable.Repeat(setupDataUnspscReferences.ToList()[0], numberOfRecords));

            var mapperConfig = new MapperConfiguration(cfg => cfg.AddProfile(new SkillsFrameworkMapper()));
            var mapper = mapperConfig.CreateMapper();
            var fakeLogger = A.Fake<IApplicationLogger>();

            var fakeDbContext = A.Fake<OnetSkillsFramework>();
            var fakeToolsAndTechnologyDbSet = A.Fake<DbSet<tools_and_technology>>(c => c
                    .Implements(typeof(IQueryable<tools_and_technology>))
                    .Implements(typeof(IDbAsyncEnumerable<tools_and_technology>)))
                .SetupData(setupTools.ToList());
            var fakeUnspcDataSet = A.Fake<DbSet<unspsc_reference>>(c => c
                    .Implements(typeof(IQueryable<unspsc_reference>))
                    .Implements(typeof(IDbAsyncEnumerable<unspsc_reference>)))
                .SetupData(setupUnspscReferences.ToList());

            //Act
            A.CallTo(() => fakeDbContext.tools_and_technology).Returns(fakeToolsAndTechnologyDbSet);
            A.CallTo(() => fakeDbContext.unspsc_reference).Returns(fakeUnspcDataSet);


            var repo = new DigitalSkillsQueryRepository(fakeDbContext, mapper);

            //Assert
            var result= repo.GetById(onetSocCode);
            result.Should().NotBeNull();
            result.ApplicationCount.Should().NotBe(0);
            result.ApplicationCount.Should().Be(applicationCount);
        }
    }
}