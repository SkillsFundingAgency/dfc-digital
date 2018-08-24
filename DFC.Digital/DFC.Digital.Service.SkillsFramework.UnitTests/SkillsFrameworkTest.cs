using Xunit;
using System.Collections.Generic;
using DFC.Digital.Data.Model;
using DFC.Digital.Core;
using FakeItEasy;
using FluentAssertions;
using System.Linq;
using DFC.Digital.Data.Interfaces;

namespace DFC.Digital.Service.SkillsFramework.UnitTests
{
       
    public class SkillsFrameworkTest : HelperOnetDatas
    {
        private readonly IApplicationLogger fakeApplicationLogger;
        private readonly IRepository<FrameworkSkill> fakeSkillsRepository;
        private readonly IRepository<DigitalSkill> fakeDigitalSkill;
        private readonly ISkillFrameworkBusinessRuleEngine fakeSkillsBusinessRuleEngine;
        private readonly ISocMappingRepository fakeSocMappingRepository;

        public SkillsFrameworkTest()
        {
            fakeApplicationLogger = A.Fake<IApplicationLogger>();
            fakeSkillsRepository = A.Fake<IRepository<FrameworkSkill>>();
            fakeDigitalSkill = A.Fake<IRepository<DigitalSkill>>();
            fakeSkillsBusinessRuleEngine = A.Fake<ISkillFrameworkBusinessRuleEngine>();
            fakeSocMappingRepository = A.Fake<ISocMappingRepository>();
        }

        [Theory]
        [MemberData(nameof(GetAllSocMappingsData))]
        public void GetAllSocMappingsTest(IReadOnlyCollection<SocCode> responseData)
        {
            // Arrange
        // Act
           A.CallTo(() => fakeSocMappingRepository.GetAll()).Returns(responseData.AsQueryable());
           var skillsFrameworkService = new SkillsFrameworkService(fakeApplicationLogger,
                fakeDigitalSkill,
                fakeSkillsRepository,
                fakeSkillsBusinessRuleEngine,
                fakeSocMappingRepository
               );
            var response = skillsFrameworkService.GetAllSocMappings();

            // Assert
            A.CallTo(() => fakeSocMappingRepository.GetAll()).MustHaveHappened();

            var socCodeData = response as IList<SocCode> ?? response.ToList();
            socCodeData.Should().NotBeNull();
            socCodeData.Should().BeEquivalentTo(responseData);
        }
        [Theory]
        [MemberData(nameof(FrameworkTranslationData))]
        public void GetAllTranslationsTest(IReadOnlyCollection<FrameworkSkill> translatedData)
        {
            // Act
            A.CallTo(() => fakeSkillsRepository.GetAll()).Returns(translatedData.AsQueryable());

            var skillsFrameworkService = new SkillsFrameworkService(fakeApplicationLogger,
                fakeDigitalSkill,
                fakeSkillsRepository,
                fakeSkillsBusinessRuleEngine,
                fakeSocMappingRepository
               );

            var response = skillsFrameworkService.GetAllTranslations();

            // Assert
            A.CallTo(() => fakeSkillsRepository.GetAll()).MustHaveHappened();

            var whatItTakesSkills = response as IList<FrameworkSkill> ?? response.ToList();
            whatItTakesSkills.Should().NotBeNull();
            whatItTakesSkills.Should().BeEquivalentTo(translatedData);
        }


        [Fact]
        public void GetRelatedSkillMappingTest()
        {
            // Arrange
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


            var skillsFrameworkService = new SkillsFrameworkService(fakeApplicationLogger,
                      fakeDigitalSkill,
                      fakeSkillsRepository,
                      fakeSkillsBusinessRuleEngine,
                      fakeSocMappingRepository
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

        [Theory]
        [MemberData(nameof(GetAllSocMappingsData))]
        public void GetNextBatchSocMappingsForUpdateTest(IReadOnlyCollection<SocCode> responseData)
        {

            A.CallTo(() => fakeSocMappingRepository.GetSocsAwaitingUpdate()).Returns(responseData.AsQueryable());
            var skillsFrameworkService = new SkillsFrameworkService(fakeApplicationLogger,
                      fakeDigitalSkill,
                      fakeSkillsRepository,
                      fakeSkillsBusinessRuleEngine,
                      fakeSocMappingRepository
                     );
            //act
            var batchSize = 1;
            var result = skillsFrameworkService.GetNextBatchSocMappingsForUpdate(batchSize);
            result.Count().Should().Be(batchSize);

            batchSize = 2;
            result = skillsFrameworkService.GetNextBatchSocMappingsForUpdate(batchSize);
            result.Count().Should().Be(batchSize);
        }

        [Theory]
        [MemberData(nameof(GetAllSocMappingsData))]
        public void ResetAllSocStatusTest(IReadOnlyCollection<SocCode> responseData)
        {            
            A.CallTo(() => fakeSocMappingRepository.GetAll()).Returns(responseData.AsQueryable());
            var skillsFrameworkService = new SkillsFrameworkService(fakeApplicationLogger,
                      fakeDigitalSkill,
                      fakeSkillsRepository,
                      fakeSkillsBusinessRuleEngine,
                      fakeSocMappingRepository
                     );

            A.CallTo(() => fakeSocMappingRepository.SetUpdateStatusForSocs(A<IQueryable<SocCode>>._, SkillsFrameWorkUpdateStatus.AwaitingUpdate)).DoesNothing();
            skillsFrameworkService.ResetAllSocStatus();
            A.CallTo(() => fakeSocMappingRepository.SetUpdateStatusForSocs(A<IQueryable<SocCode>>._, SkillsFrameWorkUpdateStatus.AwaitingUpdate)).MustHaveHappenedOnceExactly();
        }

        [Theory]
        [MemberData(nameof(GetAllSocMappingsData))]
        public void ResetStartedSocStatusTest(IReadOnlyCollection<SocCode> responseData)
        {
            A.CallTo(() => fakeSocMappingRepository.GetSocsInStartedState()).Returns(responseData.AsQueryable());
            var skillsFrameworkService = new SkillsFrameworkService(fakeApplicationLogger,
                      fakeDigitalSkill,
                      fakeSkillsRepository,
                      fakeSkillsBusinessRuleEngine,
                      fakeSocMappingRepository
                     );

            A.CallTo(() => fakeSocMappingRepository.SetUpdateStatusForSocs(A<IQueryable<SocCode>>._, SkillsFrameWorkUpdateStatus.AwaitingUpdate)).DoesNothing();
            skillsFrameworkService.ResetStartedSocStatus();
            A.CallTo(() => fakeSocMappingRepository.SetUpdateStatusForSocs(A<IQueryable<SocCode>>._, SkillsFrameWorkUpdateStatus.AwaitingUpdate)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public void SetSocStatusCompletedTest()
        {
            var skillsFrameworkService = new SkillsFrameworkService(fakeApplicationLogger,
                      fakeDigitalSkill,
                      fakeSkillsRepository,
                      fakeSkillsBusinessRuleEngine,
                      fakeSocMappingRepository
                     );

            A.CallTo(() => fakeSocMappingRepository.SetUpdateStatusForSocs(A<List<SocCode>>._, SkillsFrameWorkUpdateStatus.UpdateCompleted)).DoesNothing();
            skillsFrameworkService.SetSocStatusCompleted(new SocCode {SOCCode="dummySoc"});
            A.CallTo(() => fakeSocMappingRepository.SetUpdateStatusForSocs(A<List<SocCode>>._, SkillsFrameWorkUpdateStatus.UpdateCompleted)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public void SetSocStatusSelectedForUpdateTest()
        {
            var skillsFrameworkService = new SkillsFrameworkService(fakeApplicationLogger,
                      fakeDigitalSkill,
                      fakeSkillsRepository,
                      fakeSkillsBusinessRuleEngine,
                      fakeSocMappingRepository
                     );

            A.CallTo(() => fakeSocMappingRepository.SetUpdateStatusForSocs(A<List<SocCode>>._, SkillsFrameWorkUpdateStatus.SelectedForUpdate)).DoesNothing();
            skillsFrameworkService.SetSocStatusSelectedForUpdate(new SocCode { SOCCode = "dummySoc" });
            A.CallTo(() => fakeSocMappingRepository.SetUpdateStatusForSocs(A<List<SocCode>>._, SkillsFrameWorkUpdateStatus.SelectedForUpdate)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public void GetSocMappingStatusTest()
        {
            var skillsFrameworkService = new SkillsFrameworkService(fakeApplicationLogger,
                      fakeDigitalSkill,
                      fakeSkillsRepository,
                      fakeSkillsBusinessRuleEngine,
                      fakeSocMappingRepository
                     );

            var dummySocMappingStatus = new SocMappingStatus { AwaitingUpdate = 1, SelectedForUpdate = 2, UpdateCompleted = 3 };

            A.CallTo(() => fakeSocMappingRepository.GetSocMappingStatus()).Returns(dummySocMappingStatus);
            var result = skillsFrameworkService.GetSocMappingStatus();
            result.Should().BeEquivalentTo(dummySocMappingStatus);
        }
    }
}