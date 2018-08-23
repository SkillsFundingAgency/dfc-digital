﻿using Xunit;
using System.Collections.Generic;
using DFC.Digital.Data.Model;
using DFC.Digital.Core;
using FakeItEasy;
using FluentAssertions;
using System.Linq;
using DFC.Digital.Data.Interfaces;

namespace DFC.Digital.Service.SkillsFramework.UnitTests
{
    using System.Data.Entity;
    using Repository.ONET.DataModel;
    using Repository.ONET.Query;

    public class SkillsFrameworkTest : HelperOnetDatas
    {
        [Theory]
        [MemberData(nameof(GetAllSocMappingsData))]
        public void GetAllSocMappingsTest(IReadOnlyCollection<SocCode> responseData)
        {
            // Arrange
            var applicationLogger = A.Fake<IApplicationLogger>();
            var skillsRepository = A.Fake<IRepository<FrameworkSkill>>();
            var digitalSkill = A.Fake<IRepository<DigitalSkill>>();
            var skillsBusinessRuleEngine = A.Fake<ISkillFrameworkBusinessRuleEngine>();
            var ISocMappingRepository = A.Fake<ISocMappingRepository>();
        // Act
        A.CallTo(() => ISocMappingRepository.GetAll()).Returns(responseData.AsQueryable());
            var skillsFrameworkService = new SkillsFrameworkService(applicationLogger,
                digitalSkill,
                skillsRepository,
                skillsBusinessRuleEngine,
                ISocMappingRepository
               );
            var response = skillsFrameworkService.GetAllSocMappings();

            // Assert
            A.CallTo(() => ISocMappingRepository.GetAll()).MustHaveHappened();

            var socCodeData = response as IList<SocCode> ?? response.ToList();
            socCodeData.Should().NotBeNull();
            socCodeData.Should().BeEquivalentTo(responseData);
        }
        [Theory]
        [MemberData(nameof(FrameworkTranslationData))]
        public void GetAllTranslationsTest(IReadOnlyCollection<FrameworkSkill> translatedData)
        {
            // Arrange
            var applicationLogger = A.Fake<IApplicationLogger>();
            var skillsRepository = A.Fake<IRepository<FrameworkSkill>>();
            var digitalSkill = A.Fake<IRepository<DigitalSkill>>();
            var skillsBusinessRuleEngine = A.Fake<ISkillFrameworkBusinessRuleEngine>();
            var ISocMappingRepository = A.Fake<ISocMappingRepository>();
            // Act
            A.CallTo(() => skillsRepository.GetAll()).Returns(translatedData.AsQueryable());
            var skillsFrameworkService = new SkillsFrameworkService(applicationLogger,
                digitalSkill,
                skillsRepository,
                skillsBusinessRuleEngine,
                ISocMappingRepository
                );
            var response = skillsFrameworkService.GetAllTranslations();

            // Assert
            A.CallTo(() => skillsRepository.GetAll()).MustHaveHappened();

            var whatItTakesSkills = response as IList<FrameworkSkill> ?? response.ToList();
            whatItTakesSkills.Should().NotBeNull();
            whatItTakesSkills.Should().BeEquivalentTo(translatedData);
        }


        [Fact]
        public void GetRelatedSkillMappingTest()
        {
            // Arrange
            var applicationLogger = A.Fake<IApplicationLogger>();
            var skillsRepository = A.Fake<IRepository<FrameworkSkill>>();
            var digitalSkill = A.Fake<IRepository<DigitalSkill>>();
            var fakeSkillsBusinessRuleEngine = A.Fake<ISkillFrameworkBusinessRuleEngine>();
            var ISocMappingRepository = A.Fake<ISocMappingRepository>();

            var fakeQuerable = A.Fake<IQueryable<OnetAttribute>>();

            A.CallTo(() => fakeSkillsBusinessRuleEngine.GetAllRawOnetSkillsForOccupation(A<string>._)).Returns(fakeQuerable);
            A.CallTo(() => fakeSkillsBusinessRuleEngine.AverageOutScoreScales(A<IList<OnetAttribute>>._)).Returns(fakeQuerable);
            A.CallTo(() => fakeSkillsBusinessRuleEngine.MoveBottomLevelAttributesUpOneLevel(A<IEnumerable<OnetAttribute>>._)).Returns(fakeQuerable);
            A.CallTo(() => fakeSkillsBusinessRuleEngine.RemoveDuplicateAttributes(A<IEnumerable<OnetAttribute>>._)).Returns(fakeQuerable);
            A.CallTo(() => fakeSkillsBusinessRuleEngine.RemoveDFCSuppressions(A<IEnumerable<OnetAttribute>>._)).Returns(fakeQuerable);
            A.CallTo(() => fakeSkillsBusinessRuleEngine.AddTitlesToAttributes(A<IEnumerable<OnetAttribute>>._)).Returns(fakeQuerable);
            A.CallTo(() => fakeSkillsBusinessRuleEngine.BoostMathsSkills(A<IEnumerable<OnetAttribute>>._)).Returns(fakeQuerable);
            A.CallTo(() => fakeSkillsBusinessRuleEngine.CombineSimilarAttributes(A<IList<OnetAttribute>>._)).Returns(fakeQuerable);
            A.CallTo(() => fakeSkillsBusinessRuleEngine.SelectFinalAttributes(A<IEnumerable<OnetAttribute>>._)).Returns(fakeQuerable);


            var skillsFrameworkService = new SkillsFrameworkService(applicationLogger,
                digitalSkill,
                skillsRepository,
                fakeSkillsBusinessRuleEngine,
                ISocMappingRepository
               );

            //act
            var response = skillsFrameworkService.GetRelatedSkillMapping("dummyCode");

            //asserts
            response.Should().NotBeNull();
            A.CallTo(() => fakeSkillsBusinessRuleEngine.GetAllRawOnetSkillsForOccupation(A<string>._)).MustHaveHappened()
                .Then(A.CallTo(() => fakeSkillsBusinessRuleEngine.AverageOutScoreScales(A<IList<OnetAttribute>>._)).MustHaveHappened())
                .Then(A.CallTo(() => fakeSkillsBusinessRuleEngine.MoveBottomLevelAttributesUpOneLevel(A<IEnumerable<OnetAttribute>>._)).MustHaveHappened())
                .Then(A.CallTo(() => fakeSkillsBusinessRuleEngine.RemoveDuplicateAttributes(A<IEnumerable<OnetAttribute>>._)).MustHaveHappened())
                .Then(A.CallTo(() => fakeSkillsBusinessRuleEngine.RemoveDFCSuppressions(A<IEnumerable<OnetAttribute>>._)).MustHaveHappened())
                .Then(A.CallTo(() => fakeSkillsBusinessRuleEngine.AddTitlesToAttributes(A<IEnumerable<OnetAttribute>>._)).MustHaveHappened())
                .Then(A.CallTo(() => fakeSkillsBusinessRuleEngine.BoostMathsSkills(A<IEnumerable<OnetAttribute>>._)).MustHaveHappened())
                .Then(A.CallTo(() => fakeSkillsBusinessRuleEngine.CombineSimilarAttributes(A<IList<OnetAttribute>>._)).MustHaveHappened())
                .Then(A.CallTo(() => fakeSkillsBusinessRuleEngine.SelectFinalAttributes(A<IEnumerable<OnetAttribute>>._)).MustHaveHappened());
        }
    }
}