using System;
using System.Collections.Generic;
using System.Linq;
using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using FakeItEasy;
using FluentAssertions;
using Xunit;

namespace DFC.Digital.Service.SkillsFramework.UnitTests
{
    public class SkillsFrameworkDataImportServiceTests
    {
        private readonly ISkillsFrameworkService fakeSkillsFrameworkService;
        private readonly IFrameworkSkillRepository fakeFrameworkSkillRepository;
        private readonly ISocSkillMatrixRepository fakeSocSkillMatrixRepository;
        private readonly IJobProfileSocCodeRepository fakeImportJobProfileSocCodeRepository;
        private readonly IJobProfileRepository fakeImportJobProfileRepository;
        private readonly IReportAuditRepository fakeReportAuditRepository;

        public SkillsFrameworkDataImportServiceTests()
        {
            fakeSkillsFrameworkService = A.Fake<ISkillsFrameworkService>(ops => ops.Strict());
            fakeFrameworkSkillRepository = A.Fake<IFrameworkSkillRepository>(ops => ops.Strict());
            fakeSocSkillMatrixRepository = A.Fake<ISocSkillMatrixRepository>(ops => ops.Strict());
            fakeImportJobProfileSocCodeRepository = A.Fake<IJobProfileSocCodeRepository>(ops => ops.Strict());
            fakeReportAuditRepository = A.Fake<IReportAuditRepository>(ops => ops.Strict());
            fakeImportJobProfileRepository = A.Fake<IJobProfileRepository>(ops => ops.Strict());
        }

        [Theory]
        [InlineData(0)]
        [InlineData(20)]
        [InlineData(11)]
        public void ImportFrameworkSkillsTest(int numberOfskills)
        {
            // Arrange
            var skillsImportService = new SkillsFrameworkDataImportService(fakeSkillsFrameworkService, fakeFrameworkSkillRepository, fakeImportJobProfileSocCodeRepository, fakeImportJobProfileRepository, fakeSocSkillMatrixRepository, fakeReportAuditRepository);

            //Dummies and Fakes
            A.CallTo(() => fakeSkillsFrameworkService.GetAllTranslations()).Returns(GetFrameworkSkills(numberOfskills));
            A.CallTo(() => fakeReportAuditRepository.CreateAudit(A<string>._, A<string>._)).DoesNothing();
            A.CallTo(() => fakeFrameworkSkillRepository.GetFrameworkSkills()).Returns(Enumerable.Empty<FrameworkSkill>().AsQueryable());
            A.CallTo(() => fakeFrameworkSkillRepository.UpsertFrameworkSkill(A<FrameworkSkill>._)).Returns(new RepoActionResult());

            // Act
            skillsImportService.ImportFrameworkSkills();

            // Assert
            A.CallTo(() => fakeSkillsFrameworkService.GetAllTranslations()).MustHaveHappened();
            A.CallTo(() => fakeReportAuditRepository.CreateAudit(A<string>._, A<string>._)).MustHaveHappened();
            A.CallTo(() => fakeFrameworkSkillRepository.UpsertFrameworkSkill(A<FrameworkSkill>._)).MustHaveHappened(numberOfskills, Times.Exactly);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(11)]
        [InlineData(20)]
        public void UpdateSocCodesOccupationalCodeTest(int numberOfSocs)
        {
            // Arrange
            var skillsImportService = new SkillsFrameworkDataImportService(fakeSkillsFrameworkService, fakeFrameworkSkillRepository, fakeImportJobProfileSocCodeRepository, fakeImportJobProfileRepository, fakeSocSkillMatrixRepository, fakeReportAuditRepository);

            //Dummies and Fakes
            A.CallTo(() => fakeImportJobProfileSocCodeRepository.GetSocCodes()).Returns(GetLiveSocs(numberOfSocs));
            A.CallTo(() => fakeSkillsFrameworkService.GetAllSocMappings()).Returns(new List<SocCode>());
            A.CallTo(() => fakeReportAuditRepository.CreateAudit(A<string>._, A<string>._)).DoesNothing();
            A.CallTo(() => fakeImportJobProfileSocCodeRepository.UpdateSocOccupationalCode(A<SocCode>._)).Returns(new RepoActionResult());

            // Act
            skillsImportService.UpdateSocCodesOccupationalCode();

            // Assert
            A.CallTo(() => fakeImportJobProfileSocCodeRepository.GetSocCodes()).MustHaveHappened();
            A.CallTo(() => fakeReportAuditRepository.CreateAudit(A<string>._, A<string>._)).MustHaveHappened();
            A.CallTo(() => fakeSkillsFrameworkService.GetAllSocMappings()).MustHaveHappened();
            A.CallTo(() => fakeImportJobProfileSocCodeRepository.UpdateSocOccupationalCode(A<SocCode>._)).MustHaveHappened(numberOfSocs * 2, Times.OrLess);
        }

        
        [Theory]
        [InlineData(0)]
        [InlineData(20)]
        public void CreateSocSkillsMatrixRecordsTest(int numberOfSocSkills)
        {
            // Arrange
            var skillsImportService = new SkillsFrameworkDataImportService(fakeSkillsFrameworkService, fakeFrameworkSkillRepository, fakeImportJobProfileSocCodeRepository, fakeImportJobProfileRepository, fakeSocSkillMatrixRepository, fakeReportAuditRepository);

            //Dummies and Fakes
            A.CallTo(() => fakeSkillsFrameworkService.GetRelatedSkillMapping(A<string>._)).Returns(GetRelatedSkill(numberOfSocSkills));
            A.CallTo(() => fakeReportAuditRepository.CreateAudit("SummaryDetails", A<string>._)).DoesNothing();
            A.CallTo(() => fakeReportAuditRepository.CreateAudit("ActionDetails", A<string>._)).DoesNothing();
            A.CallTo(() => fakeReportAuditRepository.CreateAudit("ErrorDetails", A<string>._)).DoesNothing();
            A.CallTo(() => fakeSocSkillMatrixRepository.UpsertSocSkillMatrix(A<SocSkillMatrix>._)).DoesNothing();

            // Act
            var results  = skillsImportService.CreateSocSkillsMatrixRecords(new SocCode {SOCCode = "dummySOC" } );

            // Assert
            A.CallTo(() => fakeSkillsFrameworkService.GetRelatedSkillMapping(A<string>._)).MustHaveHappenedOnceExactly();
            A.CallTo(() => fakeReportAuditRepository.CreateAudit("ActionDetails", A<string>._)).MustHaveHappened((numberOfSocSkills + 1), Times.Exactly);
            A.CallTo(() => fakeSocSkillMatrixRepository.UpsertSocSkillMatrix(A<SocSkillMatrix>._)).MustHaveHappened(numberOfSocSkills, Times.Exactly);

            if (numberOfSocSkills == 0)
            {
                A.CallTo(() => fakeReportAuditRepository.CreateAudit("ErrorDetails", A<string>._)).MustHaveHappened();
            }
        }

        [Fact]
        public void CreateSocSkillsMatrixRecordsTestNullParmeterTest()
        {
            // Arrange
            var skillsImportService = new SkillsFrameworkDataImportService(fakeSkillsFrameworkService, fakeFrameworkSkillRepository, fakeImportJobProfileSocCodeRepository, fakeImportJobProfileRepository, fakeSocSkillMatrixRepository, fakeReportAuditRepository);
            Assert.Throws<ArgumentNullException>(() => skillsImportService.CreateSocSkillsMatrixRecords(null));
        }


        [Fact]
        public void ResetAllSocStatusTest()
        {
            // Arrange
            var skillsImportService = new SkillsFrameworkDataImportService(fakeSkillsFrameworkService, fakeFrameworkSkillRepository, fakeImportJobProfileSocCodeRepository, fakeImportJobProfileRepository, fakeSocSkillMatrixRepository, fakeReportAuditRepository);
            A.CallTo(() => fakeReportAuditRepository.CreateAudit("SummaryDetails", A<string>._)).DoesNothing();
            A.CallTo(() => fakeSkillsFrameworkService.ResetAllSocStatus()).DoesNothing();

            // Act
            skillsImportService.ResetAllSocStatus();
            A.CallTo(() => fakeSkillsFrameworkService.ResetAllSocStatus()).MustHaveHappenedOnceExactly();
            A.CallTo(() => fakeReportAuditRepository.CreateAudit("SummaryDetails", A<string>._)).MustHaveHappened(2, Times.Exactly);
        }

        [Fact]
        public void ResetStartedSocStatusTest()
        {
            // Arrange
            var skillsImportService = new SkillsFrameworkDataImportService(fakeSkillsFrameworkService, fakeFrameworkSkillRepository, fakeImportJobProfileSocCodeRepository, fakeImportJobProfileRepository, fakeSocSkillMatrixRepository, fakeReportAuditRepository);
            A.CallTo(() => fakeReportAuditRepository.CreateAudit("SummaryDetails", A<string>._)).DoesNothing();
            A.CallTo(() => fakeSkillsFrameworkService.ResetStartedSocStatus()).DoesNothing();

            // Act
            skillsImportService.ResetStartedSocStatus();
            A.CallTo(() => fakeSkillsFrameworkService.ResetStartedSocStatus()).MustHaveHappenedOnceExactly();
            A.CallTo(() => fakeReportAuditRepository.CreateAudit("SummaryDetails", A<string>._)).MustHaveHappened(2, Times.Exactly);
        }

        [Fact]
        public void GetSocMappingStatusTest()
        {
            // Arrange
            var dummySocMappingStatus = new SocMappingStatus { AwaitingUpdate = 1, SelectedForUpdate = 2, UpdateCompleted = 3 };
            var skillsImportService = new SkillsFrameworkDataImportService(fakeSkillsFrameworkService, fakeFrameworkSkillRepository, fakeImportJobProfileSocCodeRepository, fakeImportJobProfileRepository, fakeSocSkillMatrixRepository, fakeReportAuditRepository);
            A.CallTo(() => fakeSkillsFrameworkService.GetSocMappingStatus()).Returns(dummySocMappingStatus);

            // Act
            var result = skillsImportService.GetSocMappingStatus();
            A.CallTo(() => fakeSkillsFrameworkService.GetSocMappingStatus()).MustHaveHappenedOnceExactly();
            result.Should().BeEquivalentTo(dummySocMappingStatus);
        }


        private static IEnumerable<FrameworkSkill> GetFrameworkSkills(int count)
        {
            var list = new List<FrameworkSkill>();

            for (var i = 0; i < count; i++)
            {
                list.Add(new FrameworkSkill { Title = nameof(FrameworkSkill.Title), ONetElementId = nameof(FrameworkSkill.ONetElementId) });
            }

            return list;
        }

        private static IEnumerable<OnetAttribute> GetRelatedSkill(int count)
        {
            var list = new List<OnetAttribute>();

            for (var i = 0; i < count; i++)
            {
                list.Add(new OnetAttribute { Name = nameof(OnetAttribute.Name), SocCode = $"{i}-{nameof(OnetAttribute.SocCode)}", OnetOccupationalCode = nameof(OnetAttribute.OnetOccupationalCode), Description = nameof(OccupationOnetSkill.Description) });
            }

            return list;
        }

        private static IQueryable<SocCode> GetLiveSocs(int count)
        {
            var list = new List<SocCode>();

            for (var i = 0; i < count; i++)
            {
                list.Add(new SocCode { Title = nameof(SocCode.Title), SOCCode = nameof(SocCode.SOCCode), ONetOccupationalCode = nameof(SocCode.ONetOccupationalCode) });
            }

            //Some with null ONetOccupationalCode
            for (var i = 0; i < count; i++)
            {
                list.Add(new SocCode { Title = nameof(SocCode.Title), SOCCode = nameof(SocCode.SOCCode) });
            }

            return list.AsQueryable();
        }

        private static IEnumerable<JobProfileOverloadForWhatItTakes> GetLiveImportJobProfiles(int count)
        {
            var list = new List<JobProfileOverloadForWhatItTakes>();

            for (var i = 0; i < count; i++)
            {
                list.Add(new JobProfileOverloadForWhatItTakes { Title = nameof(SocCode.Title), SOCCode = nameof(SocCode.SOCCode), ONetOccupationalCode = nameof(SocCode.ONetOccupationalCode), DigitalSkillsLevel = nameof(JobProfileOverloadForWhatItTakes.DigitalSkillsLevel), HasRelatedSocSkillMatrices = true });
            }

            //Some with a null digital level
            for (var i = 0; i < count; i++)
            {
                list.Add(new JobProfileOverloadForWhatItTakes { Title = nameof(SocCode.Title), SOCCode = nameof(SocCode.SOCCode), ONetOccupationalCode = nameof(SocCode.ONetOccupationalCode) });
            }

            //Some with a blank onet occupation code 
            for (var i = 0; i < count; i++)
            {
                list.Add(new JobProfileOverloadForWhatItTakes { Title = nameof(SocCode.Title), SOCCode = nameof(SocCode.SOCCode), ONetOccupationalCode = string.Empty});
            }

            return list;
        }

        private static IEnumerable<JobProfileOverloadForWhatItTakes> GetLiveMatrixImportJobProfiles(int count)
        {
            var list = new List<JobProfileOverloadForWhatItTakes>();

            for (var i = 0; i < count; i++)
            {
                list.Add(new JobProfileOverloadForWhatItTakes { Title = nameof(SocCode.Title), SOCCode = $"{i}-{nameof(SocSkillMatrix.SocCode)}", ONetOccupationalCode = nameof(SocCode.ONetOccupationalCode), DigitalSkillsLevel = nameof(JobProfileOverloadForWhatItTakes.DigitalSkillsLevel) });
            }
            return list;
        }

    }
}