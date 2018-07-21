using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using DFC.Digital.Service.SkillsFramework.Services.Contracts;
using FakeItEasy;
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
            A.CallTo(() => fakeSkillsFrameworkService.GetOnetSkills()).Returns(GetFrameworkSkills(numberOfskills));
            A.CallTo(() => fakeReportAuditRepository.CreateAudit(A<string>._, A<string>._)).DoesNothing();
            A.CallTo(() => fakeFrameworkSkillRepository.GetFrameworkSkills()).Returns(Enumerable.Empty<FrameworkSkill>().AsQueryable());
            A.CallTo(() => fakeFrameworkSkillRepository.UpsertFrameworkSkill(A<FrameworkSkill>._)).Returns(new RepoActionResult());

            // Act
            skillsImportService.ImportFrameworkSkills();

            // Assert
            A.CallTo(() => fakeSkillsFrameworkService.GetOnetSkills()).MustHaveHappened();
            A.CallTo(() => fakeReportAuditRepository.CreateAudit(A<string>._, A<string>._)).MustHaveHappened();
            A.CallTo(() => fakeFrameworkSkillRepository.UpsertFrameworkSkill(A<FrameworkSkill>._)).MustHaveHappened(numberOfskills, Times.Exactly);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(20)]
        [InlineData(11)]
        public void UpdateSocCodesOccupationalCodeTest(int numberOfSocs)
        {
            // Arrange
            var skillsImportService = new SkillsFrameworkDataImportService(fakeSkillsFrameworkService, fakeFrameworkSkillRepository, fakeJobProfileSocCodeRepository, fakeJobProfileRepository, fakeSocSkillMatrixRepository, fakeReportAuditRepository);

            //Dummies and Fakes
            A.CallTo(() => fakeJobProfileSocCodeRepository.GetLiveSocCodes()).Returns(GetLiveSocs(numberOfSocs));
            A.CallTo(() => fakeSkillsFrameworkService.GetSocOccupationalCodeMappings()).Returns(new Dictionary<string, string> { { "SOCCode", "OccupationalCode" } });
            A.CallTo(() => fakeReportAuditRepository.CreateAudit(A<string>._, A<string>._)).DoesNothing();
            A.CallTo(() => fakeJobProfileSocCodeRepository.UpdateSocOccupationalCode(A<SocCode>._)).Returns(new RepoActionResult());

            // Act
            skillsImportService.UpdateSocCodesOccupationalCode();

            // Assert
            A.CallTo(() => fakeJobProfileSocCodeRepository.GetLiveSocCodes()).MustHaveHappened();
            A.CallTo(() => fakeReportAuditRepository.CreateAudit(A<string>._, A<string>._)).MustHaveHappened();
            A.CallTo(() => fakeSkillsFrameworkService.GetSocOccupationalCodeMappings()).MustHaveHappened();
            A.CallTo(() => fakeJobProfileSocCodeRepository.UpdateSocOccupationalCode(A<SocCode>._)).MustHaveHappened(numberOfSocs, Times.Exactly);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(20)]
        [InlineData(11)]
        public void UpdateJobProfilesDigitalSkillsTest(int numberOfJobProfiles)
        {
            // Arrange
            var skillsImportService = new SkillsFrameworkDataImportService(fakeSkillsFrameworkService, fakeFrameworkSkillRepository, fakeJobProfileSocCodeRepository, fakeJobProfileRepository, fakeSocSkillMatrixRepository, fakeReportAuditRepository);

            //Dummies and Fakes
            A.CallTo(() => fakeJobProfileRepository.GetLiveJobProfiles()).Returns(GetLiveJobProfiles(numberOfJobProfiles));
            A.CallTo(() => fakeSkillsFrameworkService.GetDigitalSkillLevel(A<string>._)).Returns(1);
            A.CallTo(() => fakeReportAuditRepository.CreateAudit(A<string>._, A<string>._)).DoesNothing();
            A.CallTo(() => fakeJobProfileRepository.UpdateDigitalSkill(A<JobProfile>._)).Returns(new RepoActionResult());

            // Act
            skillsImportService.UpdateJobProfilesDigitalSkills();

            // Assert
            A.CallTo(() => fakeJobProfileRepository.GetLiveJobProfiles()).MustHaveHappened();
            A.CallTo(() => fakeSkillsFrameworkService.GetDigitalSkillLevel(A<string>._)).MustHaveHappened(numberOfJobProfiles, Times.Exactly);
            A.CallTo(() => fakeReportAuditRepository.CreateAudit(A<string>._, A<string>._)).MustHaveHappened();
            A.CallTo(() => fakeJobProfileRepository.UpdateDigitalSkill(A<JobProfile>._)).MustHaveHappened(numberOfJobProfiles, Times.Exactly);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(20)]
        [InlineData(80)]
        public void BuildSocMatrixDataTest(int numberOfSocSkills)
        {
            // Arrange
            var skillsImportService = new SkillsFrameworkDataImportService(fakeSkillsFrameworkService, fakeFrameworkSkillRepository, fakeJobProfileSocCodeRepository, fakeJobProfileRepository, fakeSocSkillMatrixRepository, fakeReportAuditRepository);

            //Dummies and Fakes
            A.CallTo(() => fakeSocSkillMatrixRepository.GetSocSkillMatrices()).Returns(GetSocSkillMatrices(numberOfSocSkills));
            A.CallTo(() => fakeJobProfileRepository.GetLiveJobProfiles()).Returns(GetLiveMatrixJobProfiles(numberOfSocSkills*2));
            A.CallTo(() => fakeSkillsFrameworkService.GetOccupationalCodeSkills(A<string>._)).Returns(new List<OccupationOnetSkill> { new OccupationOnetSkill {Title = nameof(OccupationOnetSkill.Title), OnetRank = 1, Description = nameof(OccupationOnetSkill.Description)}});
            A.CallTo(() => fakeReportAuditRepository.CreateAudit(A<string>._, A<string>._)).DoesNothing();
            A.CallTo(() => fakeSocSkillMatrixRepository.UpsertSocSkillMatrix(A<SocSkillMatrix>._)).Returns(new RepoActionResult());

            // Act
            skillsImportService.BuildSocMatrixData();

            // Assert
            A.CallTo(() => fakeSocSkillMatrixRepository.GetSocSkillMatrices()).MustHaveHappenedOnceExactly();
            A.CallTo(() => fakeJobProfileRepository.GetLiveJobProfiles()).MustHaveHappenedOnceExactly();
            A.CallTo(() => fakeSkillsFrameworkService.GetOccupationalCodeSkills(A<string>._)).Returns(new List<OccupationOnetSkill>());
            A.CallTo(() => fakeReportAuditRepository.CreateAudit(A<string>._, A<string>._)).MustHaveHappened();
            A.CallTo(() => fakeSocSkillMatrixRepository.UpsertSocSkillMatrix(A<SocSkillMatrix>._)).MustHaveHappened(50, Times.OrLess);
        }


        [Theory]
        [InlineData(0)]
        [InlineData(20)]
        [InlineData(80)]
        public void UpdateJpSocSkillMatrixTest(int numberOfJobProfiles)
        {
            // Arrange
            var skillsImportService = new SkillsFrameworkDataImportService(fakeSkillsFrameworkService, fakeFrameworkSkillRepository, fakeJobProfileSocCodeRepository, fakeJobProfileRepository, fakeSocSkillMatrixRepository, fakeReportAuditRepository);

            //Dummies and Fakes
            A.CallTo(() => fakeJobProfileRepository.GetLiveJobProfiles()).Returns(GetLiveJobProfiles(numberOfJobProfiles));
            A.CallTo(() => fakeJobProfileSocCodeRepository.GetSocSkillMatricesBySocCode(A<string>._)).Returns( new EnumerableQuery<SocSkillMatrix>( GetSocSkillMatrices(5)));
            A.CallTo(() => fakeReportAuditRepository.CreateAudit(A<string>._, A<string>._)).DoesNothing();
            A.CallTo(() => fakeJobProfileRepository.UpdateSocSkillMatrices(A<JobProfile>._, A<IEnumerable<SocSkillMatrix>>._)).Returns(new RepoActionResult());

            // Act
            skillsImportService.UpdateJpSocSkillMatrix();

            // Assert
            A.CallTo(() => fakeJobProfileRepository.GetLiveJobProfiles()).MustHaveHappened();
            
            A.CallTo(() => fakeReportAuditRepository.CreateAudit(A<string>._, A<string>._)).MustHaveHappened();
            if (numberOfJobProfiles > 0)
            {
                A.CallTo(() => fakeJobProfileSocCodeRepository.GetSocSkillMatricesBySocCode(A<string>._))
                    .MustHaveHappened();
            }
            else
            {
                A.CallTo(() => fakeJobProfileSocCodeRepository.GetSocSkillMatricesBySocCode(A<string>._))
                    .MustNotHaveHappened();
            }

            A.CallTo(() => fakeJobProfileRepository.UpdateSocSkillMatrices(A<JobProfile>._, A<IEnumerable<SocSkillMatrix>>._)).MustHaveHappened(50, Times.OrLess);
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

        private static IEnumerable<SocSkillMatrix> GetSocSkillMatrices(int count)
        {
            var list = new List<SocSkillMatrix>();

            for (var i = 0; i < count; i++)
            {
                list.Add(new SocSkillMatrix { Title = nameof(SocSkillMatrix.Title), SocCode = $"{i}-{nameof(SocSkillMatrix.SocCode)}" });
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

            for (var i = 0; i < count; i++)
            {
                list.Add(new SocCode { Title = nameof(SocCode.Title), SOCCode = nameof(SocCode.SOCCode) });
            }

            return list.AsQueryable();
        }

        private static IEnumerable<JobProfile> GetLiveJobProfiles(int count)
        {
            var list = new List<JobProfile>();

            for (var i = 0; i < count; i++)
            {
                list.Add(new JobProfile { Title = nameof(SocCode.Title), SOCCode = nameof(SocCode.SOCCode), ONetOccupationalCode = nameof(SocCode.ONetOccupationalCode), DigitalSkillsLevel = nameof(JobProfile.DigitalSkillsLevel), HasRelatedSocSkillMatrices = true});
            }

            for (var i = 0; i < count; i++)
            {
                list.Add(new JobProfile { Title = nameof(SocCode.Title), SOCCode = nameof(SocCode.SOCCode), ONetOccupationalCode = nameof(SocCode.ONetOccupationalCode) });
            }

            return list;
        }

        private static IEnumerable<JobProfile> GetLiveMatrixJobProfiles(int count)
        {
            var list = new List<JobProfile>();

            for (var i = 0; i < count; i++)
            {
                list.Add(new JobProfile { Title = nameof(SocCode.Title), SOCCode = $"{i}-{nameof(SocSkillMatrix.SocCode)}", ONetOccupationalCode = nameof(SocCode.ONetOccupationalCode), DigitalSkillsLevel = nameof(JobProfile.DigitalSkillsLevel) });
            }
            return list;
        }

    }
}