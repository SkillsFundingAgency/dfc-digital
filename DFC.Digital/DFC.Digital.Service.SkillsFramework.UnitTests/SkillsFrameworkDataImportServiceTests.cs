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
        private readonly IJobProfileSocCodeRepository fakeJobProfileSocCodeRepository;
        private readonly IJobProfileRepository fakeJobProfileRepository;
        private readonly IReportAuditRepository fakeReportAuditRepository;

        public SkillsFrameworkDataImportServiceTests()
        {
            fakeSkillsFrameworkService = A.Fake<ISkillsFrameworkService>(ops => ops.Strict());
            fakeFrameworkSkillRepository = A.Fake<IFrameworkSkillRepository>(ops => ops.Strict());
            fakeSocSkillMatrixRepository = A.Fake<ISocSkillMatrixRepository>(ops => ops.Strict());
            fakeJobProfileSocCodeRepository = A.Fake<IJobProfileSocCodeRepository>(ops => ops.Strict());
            fakeReportAuditRepository = A.Fake<IReportAuditRepository>(ops => ops.Strict());
            fakeJobProfileRepository = A.Fake<IJobProfileRepository>(ops => ops.Strict());

            A.CallTo(() => fakeReportAuditRepository.CreateAudit("SummaryDetails", A<string>._)).DoesNothing();
            A.CallTo(() => fakeReportAuditRepository.CreateAudit("ActionDetails", A<string>._)).DoesNothing();
            A.CallTo(() => fakeReportAuditRepository.CreateAudit("ErrorDetails", A<string>._)).DoesNothing();

        }

        [Theory]
        [InlineData(0)]
        [InlineData(20)]
        [InlineData(11)]
        public void ImportFrameworkSkillsTest(int numberOfskills)
        {
            // Arrange
            var skillsImportService = new SkillsFrameworkDataImportService(fakeSkillsFrameworkService, fakeFrameworkSkillRepository, fakeJobProfileSocCodeRepository, fakeJobProfileRepository, fakeSocSkillMatrixRepository, fakeReportAuditRepository);

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
            var skillsImportService = new SkillsFrameworkDataImportService(fakeSkillsFrameworkService, fakeFrameworkSkillRepository, fakeJobProfileSocCodeRepository, fakeJobProfileRepository, fakeSocSkillMatrixRepository, fakeReportAuditRepository);

            //Dummies and Fakes
            A.CallTo(() => fakeJobProfileSocCodeRepository.GetSocCodes()).Returns(GetSOCs(numberOfSocs));
            A.CallTo(() => fakeSkillsFrameworkService.GetAllSocMappings()).Returns(new List<SocCode>());
            A.CallTo(() => fakeReportAuditRepository.CreateAudit(A<string>._, A<string>._)).DoesNothing();
            A.CallTo(() => fakeJobProfileSocCodeRepository.UpdateSocOccupationalCode(A<SocCode>._)).Returns(new RepoActionResult());

            // Act
            skillsImportService.UpdateSocCodesOccupationalCode();

            // Assert
            A.CallTo(() => fakeJobProfileSocCodeRepository.GetSocCodes()).MustHaveHappened();
            A.CallTo(() => fakeReportAuditRepository.CreateAudit(A<string>._, A<string>._)).MustHaveHappened();
            A.CallTo(() => fakeSkillsFrameworkService.GetAllSocMappings()).MustHaveHappened();
            A.CallTo(() => fakeJobProfileSocCodeRepository.UpdateSocOccupationalCode(A<SocCode>._)).MustHaveHappened(numberOfSocs * 2, Times.OrLess);
        }

        [Fact]
        public void UpdateSocCodesOccupationalCodeNullTest()
        {
            // Arrange
            var skillsImportService = new SkillsFrameworkDataImportService(fakeSkillsFrameworkService, fakeFrameworkSkillRepository, fakeJobProfileSocCodeRepository, fakeJobProfileRepository, fakeSocSkillMatrixRepository, fakeReportAuditRepository);

            var SocCodesFromOnet = new List<SocCode>();
            SocCodesFromOnet.Add(new SocCode { SOCCode = "1", ONetOccupationalCode = null });
            SocCodesFromOnet.Add(new SocCode { SOCCode = "3", ONetOccupationalCode = "" });

            var SocCodesFromSitefinity = new List<SocCode>();
            SocCodesFromSitefinity.Add(new SocCode { SOCCode = "1", ONetOccupationalCode = "" });   //Null gets translated to ""
            SocCodesFromSitefinity.Add(new SocCode { SOCCode = "3", ONetOccupationalCode = "" });

            //Dummies and Fakes
            A.CallTo(() => fakeJobProfileSocCodeRepository.GetSocCodes()).Returns(SocCodesFromSitefinity.AsQueryable());
            A.CallTo(() => fakeSkillsFrameworkService.GetAllSocMappings()).Returns(SocCodesFromOnet);
            A.CallTo(() => fakeReportAuditRepository.CreateAudit(A<string>._, A<string>._)).DoesNothing();
            A.CallTo(() => fakeJobProfileSocCodeRepository.UpdateSocOccupationalCode(A<SocCode>._)).Returns(new RepoActionResult());

            // Act
            skillsImportService.UpdateSocCodesOccupationalCode();

            // Assert
            A.CallTo(() => fakeJobProfileSocCodeRepository.GetSocCodes()).MustHaveHappened();
            A.CallTo(() => fakeSkillsFrameworkService.GetAllSocMappings()).MustHaveHappened();
            A.CallTo(() => fakeJobProfileSocCodeRepository.UpdateSocOccupationalCode(A<SocCode>._)).MustNotHaveHappened();
        }

        [Theory]
        [InlineData(0)]
        [InlineData(20)]
        public void CreateSocSkillsMatrixRecordsTest(int numberOfSocSkills)
        {
            // Arrange
            var skillsImportService = new SkillsFrameworkDataImportService(fakeSkillsFrameworkService, fakeFrameworkSkillRepository, fakeJobProfileSocCodeRepository, fakeJobProfileRepository, fakeSocSkillMatrixRepository, fakeReportAuditRepository);

            //Dummies and Fakes
            A.CallTo(() => fakeSkillsFrameworkService.GetRelatedSkillMapping(A<string>._)).Returns(GetRelatedSkill(numberOfSocSkills));
            A.CallTo(() => fakeSocSkillMatrixRepository.UpsertSocSkillMatrix(A<SocSkillMatrix>._)).DoesNothing();

            // Act
            skillsImportService.CreateSocSkillsMatrixRecords(new SocCode {SOCCode = "dummySOC" } );

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
            var skillsImportService = new SkillsFrameworkDataImportService(fakeSkillsFrameworkService, fakeFrameworkSkillRepository, fakeJobProfileSocCodeRepository, fakeJobProfileRepository, fakeSocSkillMatrixRepository, fakeReportAuditRepository);
            Assert.Throws<ArgumentNullException>(() => skillsImportService.CreateSocSkillsMatrixRecords(null));
        }


        [Fact]
        public void ResetAllSocStatusTest()
        {
            // Arrange
            var skillsImportService = new SkillsFrameworkDataImportService(fakeSkillsFrameworkService, fakeFrameworkSkillRepository, fakeJobProfileSocCodeRepository, fakeJobProfileRepository, fakeSocSkillMatrixRepository, fakeReportAuditRepository);
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
            var skillsImportService = new SkillsFrameworkDataImportService(fakeSkillsFrameworkService, fakeFrameworkSkillRepository, fakeJobProfileSocCodeRepository, fakeJobProfileRepository, fakeSocSkillMatrixRepository, fakeReportAuditRepository);
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
            var skillsImportService = new SkillsFrameworkDataImportService(fakeSkillsFrameworkService, fakeFrameworkSkillRepository, fakeJobProfileSocCodeRepository, fakeJobProfileRepository, fakeSocSkillMatrixRepository, fakeReportAuditRepository);
            A.CallTo(() => fakeSkillsFrameworkService.GetSocMappingStatus()).Returns(dummySocMappingStatus);

            // Act
            var result = skillsImportService.GetSocMappingStatus();
            A.CallTo(() => fakeSkillsFrameworkService.GetSocMappingStatus()).MustHaveHappenedOnceExactly();
            result.Should().BeEquivalentTo(dummySocMappingStatus);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(20)]
        public void GetNextBatchOfSOCsToImportTests(int batchSize)
        {
            //Arrange
            var dummySocs = GetSOCs(batchSize);
            var skillsImportService = new SkillsFrameworkDataImportService(fakeSkillsFrameworkService, fakeFrameworkSkillRepository, fakeJobProfileSocCodeRepository, fakeJobProfileRepository, fakeSocSkillMatrixRepository, fakeReportAuditRepository);
            A.CallTo(() => fakeSkillsFrameworkService.GetNextBatchSocMappingsForUpdate(batchSize)).Returns(dummySocs);

            // Act
            var result = skillsImportService.GetNextBatchOfSOCsToImport(batchSize);
            A.CallTo(() => fakeSkillsFrameworkService.GetNextBatchSocMappingsForUpdate(batchSize)).MustHaveHappenedOnceExactly();

            result.Should().BeEquivalentTo(string.Join(",", dummySocs.ToList().Select(s => s.SOCCode)));
        }

        [Fact]
        public void ImportForSingleSocNoSocTest()
        {
            var dummySoc = GetSOCs(1).FirstOrDefault();
      
            //Arrange
            A.CallTo(() => fakeSkillsFrameworkService.SetSocStatusCompleted(A<SocCode>._)).DoesNothing();
            A.CallTo(() => fakeJobProfileSocCodeRepository.GetBySocCode(A<string>._)).Returns(null);
            var skillsImportService = new SkillsFrameworkDataImportService(fakeSkillsFrameworkService, fakeFrameworkSkillRepository, fakeJobProfileSocCodeRepository, fakeJobProfileRepository, fakeSocSkillMatrixRepository, fakeReportAuditRepository);

            //Act
            skillsImportService.ImportForSingleSoc(dummySoc.SOCCode);

            //asserts
            A.CallTo(() => fakeReportAuditRepository.CreateAudit("ErrorDetails", A<string>._)).MustHaveHappened();
            A.CallTo(() => fakeSkillsFrameworkService.SetSocStatusCompleted(A<SocCode>._)).MustHaveHappenedOnceExactly();
            A.CallTo(() => fakeReportAuditRepository.CreateAudit("ActionDetails", A<string>._)).MustHaveHappened();
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        public void ImportForSingleSocTest(int numberOfJobProfiles)
        {
            var numberOfRelatedSkills = 2;
            var dummySoc = GetSOCs(1).FirstOrDefault();
            var dummyJobProfiles = GetLiveImportJobProfiles(numberOfJobProfiles);


            A.CallTo(() => fakeSkillsFrameworkService.SetSocStatusCompleted(A<SocCode>._)).DoesNothing();
            A.CallTo(() => fakeSkillsFrameworkService.SetSocStatusSelectedForUpdate(A<SocCode>._)).DoesNothing();
            A.CallTo(() => fakeSocSkillMatrixRepository.UpsertSocSkillMatrix(A<SocSkillMatrix>._)).DoesNothing();

            A.CallTo(() => fakeJobProfileSocCodeRepository.GetBySocCode(A<string>._)).Returns(dummySoc);
            A.CallTo(() => fakeJobProfileSocCodeRepository.GetLiveJobProfilesBySocCode(dummySoc.SOCCode)).Returns(dummyJobProfiles);
            A.CallTo(() => fakeSkillsFrameworkService.GetDigitalSkillLevel(A<string>._)).Returns(DigitalSkillsLevel.Level3);
            A.CallTo(() => fakeSkillsFrameworkService.GetRelatedSkillMapping(A<string>._)).Returns(GetRelatedSkill(numberOfRelatedSkills));
            A.CallTo(() => fakeJobProfileRepository.UpdateSocSkillMatrices(A<JobProfileOverloadForWhatItTakes>._, A<IEnumerable<SocSkillMatrix>>._)).Returns(new RepoActionResult { Success = true });

            var skillsImportService = new SkillsFrameworkDataImportService(fakeSkillsFrameworkService, fakeFrameworkSkillRepository, fakeJobProfileSocCodeRepository, fakeJobProfileRepository, fakeSocSkillMatrixRepository, fakeReportAuditRepository);

            //Act
            skillsImportService.ImportForSingleSoc(dummySoc.SOCCode);

            if (numberOfJobProfiles <= 0)
            {
                A.CallTo(() => fakeReportAuditRepository.CreateAudit("ErrorDetails", A<string>._)).MustHaveHappened();
            }
            else
            {
                A.CallTo(() => fakeReportAuditRepository.CreateAudit("ErrorDetails", A<string>._)).MustNotHaveHappened();
            }

            A.CallTo(() => fakeJobProfileRepository.UpdateSocSkillMatrices(A<JobProfileOverloadForWhatItTakes>._, A<IEnumerable<SocSkillMatrix>>._)).MustHaveHappened(numberOfJobProfiles, Times.Exactly);
            A.CallTo(() => fakeReportAuditRepository.CreateAudit("ActionDetails", A<string>._)).MustHaveHappened(4, Times.OrMore);

        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void ImportForSingleSocJobProfileLockedTest(bool lockedJp)
        {
            var numberOfRelatedSkills = 2;
            var dummySoc = GetSOCs(1).FirstOrDefault();
            var dummyJobProfiles = GetLiveImportJobProfiles(1, lockedJp);


            A.CallTo(() => fakeSkillsFrameworkService.SetSocStatusCompleted(A<SocCode>._)).DoesNothing();
            A.CallTo(() => fakeSkillsFrameworkService.SetSocStatusSelectedForUpdate(A<SocCode>._)).DoesNothing();
            A.CallTo(() => fakeSocSkillMatrixRepository.UpsertSocSkillMatrix(A<SocSkillMatrix>._)).DoesNothing();

            A.CallTo(() => fakeJobProfileSocCodeRepository.GetBySocCode(A<string>._)).Returns(dummySoc);
            A.CallTo(() => fakeJobProfileSocCodeRepository.GetLiveJobProfilesBySocCode(dummySoc.SOCCode)).Returns(dummyJobProfiles);
            A.CallTo(() => fakeSkillsFrameworkService.GetDigitalSkillLevel(A<string>._)).Returns(DigitalSkillsLevel.Level3);
            A.CallTo(() => fakeSkillsFrameworkService.GetRelatedSkillMapping(A<string>._)).Returns(GetRelatedSkill(numberOfRelatedSkills));
            A.CallTo(() => fakeJobProfileRepository.UpdateSocSkillMatrices(A<JobProfileOverloadForWhatItTakes>._, A<IEnumerable<SocSkillMatrix>>._)).Returns(new RepoActionResult { Success = true });

            var skillsImportService = new SkillsFrameworkDataImportService(fakeSkillsFrameworkService, fakeFrameworkSkillRepository, fakeJobProfileSocCodeRepository, fakeJobProfileRepository, fakeSocSkillMatrixRepository, fakeReportAuditRepository);

            //Act
            skillsImportService.ImportForSingleSoc(dummySoc.SOCCode);

            if (lockedJp)
            {
                A.CallTo(() => fakeJobProfileRepository.UpdateSocSkillMatrices(A<JobProfileOverloadForWhatItTakes>._, A<IEnumerable<SocSkillMatrix>>._)).MustNotHaveHappened();
                A.CallTo(() => fakeReportAuditRepository.CreateAudit("SummaryDetails", A<string>.That.Contains("could not be updated as it is locked or already in a Draft status with Soc code"))).MustHaveHappened();
            }
            else
            {
                A.CallTo(() => fakeJobProfileRepository.UpdateSocSkillMatrices(A<JobProfileOverloadForWhatItTakes>._, A<IEnumerable<SocSkillMatrix>>._)).MustHaveHappened(dummyJobProfiles.Count(), Times.Exactly);
                A.CallTo(() => fakeReportAuditRepository.CreateAudit("SummaryDetails", A<string>.That.Contains("could not be updated as it is locked or already in a Draft status with Soc code"))).MustNotHaveHappened();
            }

              A.CallTo(() => fakeReportAuditRepository.CreateAudit("ActionDetails", A<string>._)).MustHaveHappened(4, Times.OrMore);

        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public void ImportForSocsTest(int numberOfJobProfiles)
        {
            var numberOfRelatedSkills = 2;
            var dummySoc = GetSOCs(1).FirstOrDefault();
            var dummyJobProfiles = GetLiveImportJobProfiles(numberOfJobProfiles);

            var dummySocMappingStatus = new SocMappingStatus { AwaitingUpdate = 1, SelectedForUpdate = 2, UpdateCompleted = 3 };
            A.CallTo(() => fakeSkillsFrameworkService.GetSocMappingStatus()).Returns(dummySocMappingStatus);
            A.CallTo(() => fakeSkillsFrameworkService.SetSocStatusCompleted(A<SocCode>._)).DoesNothing();
            A.CallTo(() => fakeSkillsFrameworkService.SetSocStatusSelectedForUpdate(A<SocCode>._)).DoesNothing();
            A.CallTo(() => fakeSocSkillMatrixRepository.UpsertSocSkillMatrix(A<SocSkillMatrix>._)).DoesNothing();
            A.CallTo(() => fakeJobProfileSocCodeRepository.GetBySocCode(A<string>._)).Returns(dummySoc);
            A.CallTo(() => fakeJobProfileSocCodeRepository.GetLiveJobProfilesBySocCode(dummySoc.SOCCode)).Returns(dummyJobProfiles);
            A.CallTo(() => fakeSkillsFrameworkService.GetDigitalSkillLevel(A<string>._)).Returns(DigitalSkillsLevel.Level3);
            A.CallTo(() => fakeSkillsFrameworkService.GetRelatedSkillMapping(A<string>._)).Returns(GetRelatedSkill(numberOfRelatedSkills));
            A.CallTo(() => fakeJobProfileRepository.UpdateSocSkillMatrices(A<JobProfileOverloadForWhatItTakes>._, A<IEnumerable<SocSkillMatrix>>._)).Returns(new RepoActionResult { Success = true });
            var skillsImportService = new SkillsFrameworkDataImportService(fakeSkillsFrameworkService, fakeFrameworkSkillRepository, fakeJobProfileSocCodeRepository, fakeJobProfileRepository, fakeSocSkillMatrixRepository, fakeReportAuditRepository);

            //Act
            skillsImportService.ImportForSocs(string.Join(",", dummyJobProfiles.ToList().Select(s => s.SOCCode)));
            A.CallTo(() => fakeSkillsFrameworkService.GetSocMappingStatus()).MustHaveHappened(2, Times.Exactly);
            A.CallTo(() => fakeSkillsFrameworkService.SetSocStatusCompleted(A<SocCode>._)).MustHaveHappened(numberOfJobProfiles, Times.Exactly);
        }

        [Fact]
        public void ImportForSocsParmeterNullTest()
        {
            // Arrange
            var skillsImportService = new SkillsFrameworkDataImportService(fakeSkillsFrameworkService, fakeFrameworkSkillRepository, fakeJobProfileSocCodeRepository, fakeJobProfileRepository, fakeSocSkillMatrixRepository, fakeReportAuditRepository);
            Assert.Throws<ArgumentNullException>(() => skillsImportService.ImportForSocs(null));
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

        private static IEnumerable<OnetSkill> GetRelatedSkill(int count)
        {
            var list = new List<OnetSkill>();

            for (var i = 0; i < count; i++)
            {
                list.Add(new OnetSkill { Name = nameof(OnetSkill.Name), SocCode = $"{i}-{nameof(OnetSkill.SocCode)}", OnetOccupationalCode = nameof(OnetSkill.OnetOccupationalCode), Description = nameof(OccupationOnetSkill.Description) });
            }

            return list;
        }

        private static IQueryable<SocCode> GetSOCs(int count)
        {
            var list = new List<SocCode>();

            for (var i = 0; i < count; i++)
            {
                list.Add(new SocCode { Description = nameof(SocCode.Description), SOCCode = $"A-{nameof(SocCode.SOCCode) + i}", ONetOccupationalCode = nameof(SocCode.ONetOccupationalCode) });
            }

            //Some with null ONetOccupationalCode
            for (var i = 0; i < count; i++)
            {
                list.Add(new SocCode { Description = nameof(SocCode.Description), SOCCode = $"B-{nameof(SocCode.SOCCode) + i}" });
            }

            return list.AsQueryable();
        }

        private static IEnumerable<JobProfileOverloadForWhatItTakes> GetLiveImportJobProfiles(int count, bool locked = false)
        {
            var list = new List<JobProfileOverloadForWhatItTakes>();

            for (var i = 0; i < count; i++)
            {
                list.Add(new JobProfileOverloadForWhatItTakes { Title = nameof(SocCode.Description), SOCCode = nameof(SocCode.SOCCode), ONetOccupationalCode = nameof(SocCode.ONetOccupationalCode), DigitalSkillsLevel = nameof(JobProfileOverloadForWhatItTakes.DigitalSkillsLevel), HasRelatedSocSkillMatrices = true, Locked = locked});
            }
            return list;
        }
    }
}