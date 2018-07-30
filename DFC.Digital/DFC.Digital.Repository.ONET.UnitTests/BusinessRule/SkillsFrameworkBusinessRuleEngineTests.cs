using Xunit;
using DFC.Digital.Repository.ONET.BusinessRule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DFC.Digital.Data.Model;
using FakeItEasy;
using DFC.Digital.Repository.ONET.DataModel;
using AutoMapper;
using FluentAssertions;
using DFC.Digital.Service.SkillsFramework;

namespace DFC.Digital.Repository.ONET.BusinessRule.Tests
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
            var fakeDbContext = A.Fake<OnetSkillsFramework>();
            var fakeMapper = A.Fake<IMapper>();
           // var ruleEngine = new SkillFrameworkBusinessRuleEngine(fakeDbContext, fakeMapper);

           // var result = ruleEngine.GetDigitalSkillsLevel(input);

          //  result.Should().Be(expectedLevel);
        }
    }
}