using DFC.Digital.Service.SkillsFramework;
using Xunit;
using System;
using System.Collections.Generic;
using DFC.Digital.Data.Model;
using DFC.Digital.Core;
using FakeItEasy;
using FluentAssertions;
using System.Linq;
using DFC.Digital.Data.Interfaces;
using DFC.Digital.Repository.ONET.Tests.Model;

namespace DFC.Digital.Service.SkillsFramework.Tests
{
    using Repository.ONET;
    using Repository.ONET.DataModel;

    public class SkillsFrameworkTest : HelperOnetDatas
    {
        [Theory]
        [MemberData(nameof(FrameworkTranslationData))]
        public void GetAllTranslationsAsyncTest(List<FrameworkSkill> translatedData)
        {
            // Arrange
            var applicationLogger = A.Fake<IApplicationLogger>();
            var socRepository = A.Fake<IRepository<SocCode>>();
            var skillsMappingRepository = A.Fake<IRelatedSkillsMappingRepository>();
            var skillsRepository = A.Fake<IRepository<FrameworkSkill>>();
            var digitalSkill = A.Fake<IRepository<DigitalSkill>>();
            var skillsBusinessRuleEngine = A.Fake<ISkillFrameworkBusinessRuleEngine>();
            // Act
            A.CallTo(() => skillsRepository.GetAll()).Returns(translatedData.AsQueryable());
            var skillsFrameworkService = new SkillsFrameworkService(applicationLogger,
                socRepository,
                digitalSkill,
                skillsRepository,
                skillsBusinessRuleEngine
                );
            var response = skillsFrameworkService.GetAllTranslations();

            // Assert
            A.CallTo(() => skillsRepository.GetAll()).MustHaveHappened();

            var whatItTakesSkills = response as IList<FrameworkSkill> ?? response.ToList();
            whatItTakesSkills.Should().NotBeNull();
            whatItTakesSkills.Should().BeEquivalentTo(translatedData);
        }

        [Theory]
        [MemberData(nameof(GetAllSocMappingsData))]
        public void GetAllSocMappingsAsyncTest(List<SocCode> responseData)
        {
            // Arrange
            var applicationLogger = A.Fake<IApplicationLogger>();
            var socRepository = A.Fake<IRepository<SocCode>>();
            var skillsMappingRepository = A.Fake<IRelatedSkillsMappingRepository>();
            var skillsRepository = A.Fake<IRepository<FrameworkSkill>>();
            var digitalSkill = A.Fake<IRepository<DigitalSkill>>();
            var skillsBusinessRuleEngine = A.Fake<ISkillFrameworkBusinessRuleEngine>();
            // Act
            A.CallTo(() => socRepository.GetAll()).Returns(responseData.AsQueryable());
            var skillsFrameworkService = new SkillsFrameworkService(applicationLogger,
                socRepository,
                digitalSkill,
                skillsRepository,
                skillsBusinessRuleEngine
               );
            var response = skillsFrameworkService.GetAllSocMappings();

            // Assert
            A.CallTo(() => socRepository.GetAll()).MustHaveHappened();

            var socCodeData = response as IList<SocCode> ?? response.ToList();
            socCodeData.Should().NotBeNull();
            socCodeData.Should().BeEquivalentTo(responseData);
        }

        [Fact()]
        public void GetDigitalSkillLevelTest()
        {
            Assert.True(false, "This test needs an implementation");
        }

        [Fact()]
        public void GetDigitalSkillsLevelTest()
        {
            Assert.True(false, "This test needs an implementation");
        }

        //[Fact]
        //public void GetAllSocMappingsAsyncTestException()
        //{
        //    Arrange
        //    var applicationLogger = A.Fake<IApplicationLogger>();
        //    var onetSkillsRepository = A.Fake<IOnetRepository>();
        //    var businessRuleEngine = A.Fake<IBusinessRuleEngine>();

        //    Act
        //    A.CallTo(() => onetSkillsRepository.GetAllSocMappingsAsync<SocCode>()).ThrowsAsync(new Exception());
        //    A.CallTo(() => applicationLogger.ErrorJustLogIt(A<string>._, A<Exception>._)).DoesNothing();
        //    A.CallTo(() => businessRuleEngine.GetAllTranslationsAsync()).ThrowsAsync(new Exception());

        //    IBusinessRuleEngine ruleEngine = new SkillsFrameworkEngine(onetSkillsRepository, applicationLogger);

        //    Assert
        //    var response = ruleEngine.GetAllSocMappingsAsync();
        //    A.CallTo(() => onetSkillsRepository.GetAllSocMappingsAsync<SocCode>()).ThrowsAsync(new Exception());
        //    A.CallTo(() => businessRuleEngine.GetAllTranslationsAsync()).ThrowsAsync(new Exception());
        //    A.CallTo(() => applicationLogger.ErrorJustLogIt(A<string>._, A<Exception>._)).MustHaveHappened();
        //    response.Result.Should().BeNull();
        //}

        //[Theory]
        //[MemberData(nameof(DigitalSkillsData))]
        //public void GetAllDigitalSkillsAsyncTest(DfcOnetDigitalSkills digitalSkillsData, string onetCode, int digitialSkillsCount)
        //{
        //    Arrange
        //    var applicationLogger = A.Fake<IApplicationLogger>();
        //    var onetSkillsRepository = A.Fake<IOnetRepository>();

        //    Act
        //    A.CallTo(() => onetSkillsRepository.GetDigitalSkillsAsync<DfcOnetDigitalSkills>(onetCode)).Returns(digitalSkillsData);
        //    A.CallTo(() => applicationLogger.ErrorJustLogIt(A<string>._, A<Exception>._)).DoesNothing();
        //    IBusinessRuleEngine ruleEngine = new SkillsFrameworkEngine(onetSkillsRepository, applicationLogger);

        //    Assert
        //    var response = ruleEngine.GetAllDigitalSkillsAsync(onetCode);
        //    A.CallTo(() => onetSkillsRepository.GetDigitalSkillsAsync<DfcOnetDigitalSkills>(A<string>._)).MustHaveHappened();
        //    A.CallTo(() => applicationLogger.ErrorJustLogIt(A<string>._, A<Exception>._)).MustNotHaveHappened();

        //    response.Result.Should().NotBeNull();
        //    response.Result.Should().BeSameAs(digitalSkillsData);
        //    response.Result.DigitalSkillsCount.Should().Be(digitialSkillsCount);
        //}

        //[Theory]
        //[InlineData("11-10011.00", null)]
        //[InlineData("11-2011.01", null)]
        //public void GetAllDigitalSkillsAsyncTestException(string onetCode, DfcOnetDigitalSkills digitalSkillsData)
        //{
        //    Arrange
        //    var applicationLogger = A.Fake<IApplicationLogger>();
        //    var onetSkillsRepository = A.Fake<IOnetRepository>();
        //    var businessRuleEngine = A.Fake<IBusinessRuleEngine>();

        //    Act
        //    A.CallTo(() => onetSkillsRepository.GetDigitalSkillsAsync<DfcOnetDigitalSkills>(onetCode)).ThrowsAsync(new Exception());
        //    A.CallTo(() => applicationLogger.ErrorJustLogIt(A<string>._, A<Exception>._)).DoesNothing();
        //    A.CallTo(() => businessRuleEngine.GetAllDigitalSkillsAsync(onetCode)).ThrowsAsync(new Exception());

        //    IBusinessRuleEngine ruleEngine = new SkillsFrameworkEngine(onetSkillsRepository, applicationLogger);

        //    Assert
        //    var response = ruleEngine.GetAllDigitalSkillsAsync(onetCode);
        //    A.CallTo(() => onetSkillsRepository.GetDigitalSkillsAsync<DfcOnetDigitalSkills>(onetCode)).ThrowsAsync(new Exception());
        //    A.CallTo(() => businessRuleEngine.GetAllDigitalSkillsAsync(onetCode)).ThrowsAsync(new Exception());
        //    A.CallTo(() => applicationLogger.ErrorJustLogIt(A<string>._, A<Exception>._)).MustHaveHappened();
        //    response.Result.Should().Be(digitalSkillsData);
        //}

        //[Theory]
        //[InlineData("11-10011.00", 153, 1)]
        //[InlineData("11-2011.01", 50, 4)]
        //public void GetDigitalSkillRankAsyncTest(string onetCode, int skillsCount, int digitalRank)
        //{
        //    Arrange
        //    var applicationLogger = A.Fake<IApplicationLogger>();
        //    var onetSkillsRepository = A.Fake<IOnetRepository>();

        //    Act
        //    A.CallTo(() => onetSkillsRepository.GetDigitalSkillsRankAsync<int>(onetCode)).Returns(skillsCount);
        //    A.CallTo(() => applicationLogger.ErrorJustLogIt(A<string>._, A<Exception>._)).DoesNothing();
        //    IBusinessRuleEngine ruleEngine = new SkillsFrameworkEngine(onetSkillsRepository, applicationLogger);

        //    Assert
        //    var response = ruleEngine.GetDigitalSkillRankAsync(onetCode);
        //    A.CallTo(() => onetSkillsRepository.GetDigitalSkillsRankAsync<int>(A<string>._)).MustHaveHappened();
        //    A.CallTo(() => applicationLogger.ErrorJustLogIt(A<string>._, A<Exception>._)).MustNotHaveHappened();

        //    response.Result.Should().NotBe(0);
        //    response.Result.Should().Be(digitalRank);
        //}

        //[Theory]
        //[InlineData("11-10011.00", 0)]
        //[InlineData("11-2011.01", 0)]
        //public void GetDigitalSkillRankAsyncTestException(string onetCode, int digitalRank)
        //{
        //    Arrange
        //    var applicationLogger = A.Fake<IApplicationLogger>();
        //    var onetSkillsRepository = A.Fake<IOnetRepository>();
        //    var businessRuleEngine = A.Fake<IBusinessRuleEngine>();

        //    Act
        //    A.CallTo(() => onetSkillsRepository.GetDigitalSkillsRankAsync<int>(onetCode)).ThrowsAsync(new Exception());
        //    A.CallTo(() => businessRuleEngine.GetDigitalSkillRankAsync(onetCode)).ThrowsAsync(new Exception());
        //    A.CallTo(() => applicationLogger.ErrorJustLogIt(A<string>._, A<Exception>._)).DoesNothing();
        //    IBusinessRuleEngine ruleEngine = new SkillsFrameworkEngine(onetSkillsRepository, applicationLogger);

        //    Assert
        //    var response = ruleEngine.GetDigitalSkillRankAsync(onetCode);
        //    A.CallTo(() => onetSkillsRepository.GetDigitalSkillsRankAsync<int>(A<string>._)).MustHaveHappened();
        //    A.CallTo(() => onetSkillsRepository.GetDigitalSkillsRankAsync<int>(A<string>._)).ThrowsAsync(new Exception());
        //    A.CallTo(() => applicationLogger.ErrorJustLogIt(A<string>._, A<Exception>._)).MustHaveHappened();

        //    response.Result.Should().Be(digitalRank);
        //}

        //[Theory]
        //[MemberData(nameof(AttributeDataSet))]
        //public void GetBusinessRuleAttributesAsyncTest(List<DfcOnetAttributesData> onetAttributes, string onetSocCode, int totalCount)
        //{
        //    Arrange
        //    var applicationLogger = A.Fake<IApplicationLogger>();
        //    var onetSkillsRepository = A.Fake<IOnetRepository>();

        //    Act
        //    A.CallTo(() => onetSkillsRepository.GetAttributesValuesAsync<DfcOnetAttributesData>(onetSocCode)).Returns(onetAttributes);
        //    A.CallTo(() => applicationLogger.ErrorJustLogIt(A<string>._, A<Exception>._)).DoesNothing();
        //    IBusinessRuleEngine ruleEngine = new SkillsFrameworkEngine(onetSkillsRepository, applicationLogger);

        //    Assert
        //    var response = ruleEngine.GetBusinessRuleAttributesAsync(onetSocCode);
        //    A.CallTo(() => onetSkillsRepository.GetAttributesValuesAsync<DfcOnetAttributesData>(A<string>._)).MustHaveHappened();
        //    A.CallTo(() => applicationLogger.ErrorJustLogIt(A<string>._, A<Exception>._)).MustNotHaveHappened();

        //    response.Result.Should().NotBeNull();
        //    response.Result.Should().Contain(x => x.Attribute == Attributes.Abilities);
        //    response.Result.Should().Contain(x => x.Attribute == Attributes.Skills);
        //    response.Result.Should().Contain(x => x.Attribute == Attributes.Knowledge);
        //    response.Result.Should().Contain(x => x.Attribute == Attributes.WorkStyles);
        //    response.Result.Count().Should().Be(totalCount);
        //}

        //[Theory]
        //[InlineData("11-10011.00", null)]
        //[InlineData("11-2011.01", null)]
        //public void GetBusinessRuleAttributesAsyncTestException(string onetSocCode, List<DfcOnetAttributesData> onetAttributes)
        //{
        //    Arrange
        //    var applicationLogger = A.Fake<IApplicationLogger>();
        //    var onetSkillsRepository = A.Fake<IOnetRepository>();
        //    var businessRuleEngine = A.Fake<IBusinessRuleEngine>();

        //    Act
        //    A.CallTo(() => onetSkillsRepository.GetAttributesValuesAsync<DfcOnetAttributesData>(onetSocCode)).ThrowsAsync(new Exception());
        //    A.CallTo(() => businessRuleEngine.GetBusinessRuleAttributesAsync(onetSocCode)).ThrowsAsync(new Exception());
        //    A.CallTo(() => applicationLogger.ErrorJustLogIt(A<string>._, A<Exception>._)).DoesNothing();
        //    IBusinessRuleEngine ruleEngine = new SkillsFrameworkEngine(onetSkillsRepository, applicationLogger);

        //    Assert
        //    var response = ruleEngine.GetBusinessRuleAttributesAsync(onetSocCode);
        //    A.CallTo(() => onetSkillsRepository.GetAttributesValuesAsync<DfcOnetAttributesData>(A<string>._)).MustHaveHappened();
        //    A.CallTo(() => onetSkillsRepository.GetAttributesValuesAsync<DfcOnetAttributesData>(A<string>._)).ThrowsAsync(new Exception());
        //    A.CallTo(() => businessRuleEngine.GetBusinessRuleAttributesAsync(onetSocCode)).ThrowsAsync(new Exception());
        //    A.CallTo(() => applicationLogger.ErrorJustLogIt(A<string>._, A<Exception>._)).MustHaveHappened();

        //    response.Result.Should().BeEquivalentTo(onetAttributes);
        //}
    }
}