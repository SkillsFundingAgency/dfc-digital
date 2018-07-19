using Xunit;
using System;
using System.Collections.Generic;
using DFC.Digital.Data.Model;
using DFC.Digital.Core;
using FakeItEasy;
using FluentAssertions;
using DFC.Digital.Service.SkillsFramework.Interface;
using DFC.Digital.Repository.ONET;
using DFC.Digital.Service.SkillsFramework.UnitTests.Model;
namespace DFC.Digital.Service.SkillsFramework.Tests
{
    public class SkillsFrameworkTest:HelperOnetDatas
    {
        [Theory]
        [MemberData(nameof(TranslationData))]
        public void GetAllTranslationsAsyncTest(List<DfcGdsTranslation> translatedData)
        {
            //Arrange
            var applicationLogger = A.Fake<IApplicationLogger>();
            var onetSkillsRepository = A.Fake<IOnetRepository>();

            //Act
            A.CallTo(() => onetSkillsRepository.GetAllTranslationsAsync<DfcGdsTranslation>()).Returns(translatedData);
            A.CallTo(() => applicationLogger.ErrorJustLogIt(A<string>._, A<Exception>._)).DoesNothing();
            IBusinessRuleEngine ruleEngine = new SkillsFrameworkEngine(onetSkillsRepository, applicationLogger);

            //Assert
            var response = ruleEngine.GetAllTranslationsAsync();
            A.CallTo(() => onetSkillsRepository.GetAllTranslationsAsync<DfcGdsTranslation>()).MustHaveHappened();
            response.Result.Should().BeSameAs(translatedData);
        }

        [Theory]
        [MemberData(nameof(SocMappings))]
        public void GetAllSocMappingsAsyncTest(List<DfcGdsSocMappings> socMappings)
        {
            //Arrange
            var applicationLogger = A.Fake<IApplicationLogger>();
            var onetSkillsRepository = A.Fake<IOnetRepository>();

            //Act
            A.CallTo(() => onetSkillsRepository.GetAllSocMappingsAsync<DfcGdsSocMappings>()).Returns(socMappings);
            A.CallTo(() => applicationLogger.ErrorJustLogIt(A<string>._, A<Exception>._)).DoesNothing();
            IBusinessRuleEngine ruleEngine =new SkillsFrameworkEngine(onetSkillsRepository,applicationLogger);

            //Assert
            var response = ruleEngine.GetAllSocMappingsAsync();
            A.CallTo(() => onetSkillsRepository.GetAllSocMappingsAsync<DfcGdsSocMappings>()).MustHaveHappened();
            response.Result.Should().BeSameAs(socMappings);

        }

        [Theory]
        [MemberData(nameof(DigitalSkillsData))]
        public void GetAllDigitalSkillsAsyncTest(DfcGdsDigitalSkills digitalSkillsData,string onetCode,int digitialSkillsCount)
        {
            //Arrange
            var applicationLogger = A.Fake<IApplicationLogger>();
            var onetSkillsRepository = A.Fake<IOnetRepository>();

            //Act
            A.CallTo(() => onetSkillsRepository.GetDigitalSkillsAsync<DfcGdsDigitalSkills>(onetCode)).Returns(digitalSkillsData);
            A.CallTo(() => applicationLogger.ErrorJustLogIt(A<string>._, A<Exception>._)).DoesNothing();
            IBusinessRuleEngine ruleEngine = new SkillsFrameworkEngine(onetSkillsRepository, applicationLogger);

            //Assert
            var response = ruleEngine.GetAllDigitalSkillsAsync(onetCode);
            A.CallTo(() => onetSkillsRepository.GetDigitalSkillsAsync<DfcGdsDigitalSkills>(A<string>._)).MustHaveHappened();
            response.Result.Should().BeSameAs(digitalSkillsData);
            response.Result.DigitalSkillsCount.Should().Be(digitialSkillsCount);
        }

        [Theory]
        [InlineData("11-10011.00",153,1)]
        [InlineData("11-2011.01",50,4)]
        public void GetDigitalSkillRankAsyncTest(string onetCode,int skillsCount,int digitalRank)
        {
            //Arrange
            var applicationLogger = A.Fake<IApplicationLogger>();
            var onetSkillsRepository = A.Fake<IOnetRepository>();

            //Act
            A.CallTo(() => onetSkillsRepository.GetDigitalSkillsRankAsync<int>(onetCode)).Returns(skillsCount);
            A.CallTo(() => applicationLogger.ErrorJustLogIt(A<string>._, A<Exception>._)).DoesNothing();
            IBusinessRuleEngine ruleEngine = new SkillsFrameworkEngine(onetSkillsRepository, applicationLogger);

            //Assert
            var response = ruleEngine.GetDigitalSkillRankAsync(onetCode);
            A.CallTo(() => onetSkillsRepository.GetDigitalSkillsRankAsync<int>(A<string>._)).MustHaveHappened();
            response.Result.Should().Be(digitalRank);
        }

        [Fact()]
        public void GetBusinessRuleAttributesAsyncTest()
        {
            Xunit.Assert.True(false, "This test needs an implementation");
        }
    }
}

