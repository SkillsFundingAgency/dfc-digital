using System.Collections.Generic;
using System.Linq;
using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using FakeItEasy;
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
            A.CallTo(() => fakeImportJobProfileSocCodeRepository.GetLiveSocCodes()).Returns(GetLiveSocs(numberOfSocs));
            A.CallTo(() => fakeSkillsFrameworkService.GetAllSocMappings()).Returns(new List<SocCode>());
            A.CallTo(() => fakeReportAuditRepository.CreateAudit(A<string>._, A<string>._)).DoesNothing();
            A.CallTo(() => fakeImportJobProfileSocCodeRepository.UpdateSocOccupationalCode(A<SocCode>._)).Returns(new RepoActionResult());

            // Act
            skillsImportService.UpdateSocCodesOccupationalCode();

            // Assert
            A.CallTo(() => fakeImportJobProfileSocCodeRepository.GetLiveSocCodes()).MustHaveHappened();
            A.CallTo(() => fakeReportAuditRepository.CreateAudit(A<string>._, A<string>._)).MustHaveHappened();
            A.CallTo(() => fakeSkillsFrameworkService.GetAllSocMappings()).MustHaveHappened();
            A.CallTo(() => fakeImportJobProfileSocCodeRepository.UpdateSocOccupationalCode(A<SocCode>._)).MustHaveHappened(numberOfSocs, Times.OrLess);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(20)]
        [InlineData(11)]
        public void UpdateImportJobProfilesDigitalSkillsTest(int numberOfImportJobProfiles)
        {
            // Arrange
            var skillsImportService = new SkillsFrameworkDataImportService(fakeSkillsFrameworkService, fakeFrameworkSkillRepository, fakeImportJobProfileSocCodeRepository, fakeImportJobProfileRepository, fakeSocSkillMatrixRepository, fakeReportAuditRepository);

            //Dummies and Fakes
            A.CallTo(() => fakeImportJobProfileRepository.GetLiveJobProfiles()).Returns(GetLiveImportJobProfiles(numberOfImportJobProfiles));
            A.CallTo(() => fakeSkillsFrameworkService.GetDigitalSkillLevel(A<string>._)).Returns(DigitalSkillsLevel.Level3);
            A.CallTo(() => fakeReportAuditRepository.CreateAudit(A<string>._, A<string>._)).DoesNothing();
            A.CallTo(() => fakeImportJobProfileRepository.UpdateDigitalSkill(A<ImportJobProfile>._)).Returns(new RepoActionResult());

            // Act
            skillsImportService.UpdateJobProfilesDigitalSkills();

            // Assert
            A.CallTo(() => fakeImportJobProfileRepository.GetLiveJobProfiles()).MustHaveHappened();
            A.CallTo(() => fakeSkillsFrameworkService.GetDigitalSkillLevel(A<string>._)).MustHaveHappened(numberOfImportJobProfiles * 2, Times.Exactly);
            A.CallTo(() => fakeReportAuditRepository.CreateAudit(A<string>._, A<string>._)).MustHaveHappened();
            A.CallTo(() => fakeImportJobProfileRepository.UpdateDigitalSkill(A<ImportJobProfile>._)).MustHaveHappened(numberOfImportJobProfiles * 2, Times.Exactly);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(20)]
        [InlineData(80)]
        public void BuildSocMatrixDataTest(int numberOfSocSkills)
        {
            // Arrange
            var skillsImportService = new SkillsFrameworkDataImportService(fakeSkillsFrameworkService, fakeFrameworkSkillRepository, fakeImportJobProfileSocCodeRepository, fakeImportJobProfileRepository, fakeSocSkillMatrixRepository, fakeReportAuditRepository);

            //Dummies and Fakes
            A.CallTo(() => fakeImportJobProfileSocCodeRepository.GetLiveSocCodes()).Returns(GetLiveSocs(numberOfSocSkills));
            A.CallTo(() => fakeSocSkillMatrixRepository.GetSocSkillMatrices()).Returns(GetSocSkillMatrices(numberOfSocSkills));

            A.CallTo(() => fakeSkillsFrameworkService.GetRelatedSkillMapping(A<string>._)).Returns(new List<OnetAttribute> { new OnetAttribute { OnetOccupationalCode = nameof(OnetAttribute.OnetOccupationalCode), Description = nameof(OccupationOnetSkill.Description) } });
            A.CallTo(() => fakeReportAuditRepository.CreateAudit(A<string>._, A<string>._)).DoesNothing();
            A.CallTo(() => fakeSocSkillMatrixRepository.UpsertSocSkillMatrix(A<SocSkillMatrix>._)).Returns(new RepoActionResult());


            // Act
            skillsImportService.BuildSocMatrixData();


            // Assert
            A.CallTo(() => fakeSocSkillMatrixRepository.GetSocSkillMatrices()).MustHaveHappenedOnceExactly();

            A.CallTo(() => fakeSkillsFrameworkService.GetRelatedSkillMapping(A<string>._)).Returns(new List<OnetAttribute>());
            A.CallTo(() => fakeReportAuditRepository.CreateAudit(A<string>._, A<string>._)).MustHaveHappened();
            A.CallTo(() => fakeSocSkillMatrixRepository.UpsertSocSkillMatrix(A<SocSkillMatrix>._)).MustHaveHappened(50, Times.OrLess);
        }


        [Theory]
        [InlineData(0)]
        [InlineData(20)]
        [InlineData(80)]
        public void UpdateJpSocSkillMatrixTest(int numberOfImportJobProfiles)
        {
            // Arrange
            var skillsImportService = new SkillsFrameworkDataImportService(fakeSkillsFrameworkService, fakeFrameworkSkillRepository, fakeImportJobProfileSocCodeRepository, fakeImportJobProfileRepository, fakeSocSkillMatrixRepository, fakeReportAuditRepository);

            //Dummies and Fakes
            A.CallTo(() => fakeImportJobProfileRepository.GetLiveJobProfiles()).Returns(GetLiveImportJobProfiles(numberOfImportJobProfiles));
            A.CallTo(() => fakeImportJobProfileSocCodeRepository.GetSocSkillMatricesBySocCode(A<string>._)).Returns(new EnumerableQuery<SocSkillMatrix>(GetSocSkillMatrices(5)));
            A.CallTo(() => fakeReportAuditRepository.CreateAudit(A<string>._, A<string>._)).DoesNothing();
            A.CallTo(() => fakeImportJobProfileRepository.UpdateSocSkillMatrices(A<ImportJobProfile>._, A<IEnumerable<SocSkillMatrix>>._)).Returns(new RepoActionResult());

            // Act
            skillsImportService.UpdateJpSocSkillMatrix();

            // Assert
            A.CallTo(() => fakeImportJobProfileRepository.GetLiveJobProfiles()).MustHaveHappened();

            A.CallTo(() => fakeReportAuditRepository.CreateAudit(A<string>._, A<string>._)).MustHaveHappened();
            if (numberOfImportJobProfiles > 0)
            {
                A.CallTo(() => fakeImportJobProfileSocCodeRepository.GetSocSkillMatricesBySocCode(A<string>._))
                    .MustHaveHappened();
            }
            else
            {
                A.CallTo(() => fakeImportJobProfileSocCodeRepository.GetSocSkillMatricesBySocCode(A<string>._))
                    .MustNotHaveHappened();
            }

            A.CallTo(() => fakeImportJobProfileRepository.UpdateSocSkillMatrices(A<ImportJobProfile>._, A<IEnumerable<SocSkillMatrix>>._)).MustHaveHappened(50, Times.OrLess);
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

        private static IEnumerable<ImportJobProfile> GetLiveImportJobProfiles(int count)
        {
            var list = new List<ImportJobProfile>();

            for (var i = 0; i < count; i++)
            {
                list.Add(new ImportJobProfile { Title = nameof(SocCode.Title), SOCCode = nameof(SocCode.SOCCode), ONetOccupationalCode = nameof(SocCode.ONetOccupationalCode), DigitalSkillsLevel = nameof(ImportJobProfile.DigitalSkillsLevel), HasRelatedSocSkillMatrices = true });
            }

            //Some with a null digital level
            for (var i = 0; i < count; i++)
            {
                list.Add(new ImportJobProfile { Title = nameof(SocCode.Title), SOCCode = nameof(SocCode.SOCCode), ONetOccupationalCode = nameof(SocCode.ONetOccupationalCode) });
            }

            //Some with a blank onet occupation code 
            for (var i = 0; i < count; i++)
            {
                list.Add(new ImportJobProfile { Title = nameof(SocCode.Title), SOCCode = nameof(SocCode.SOCCode), ONetOccupationalCode = string.Empty});
            }

            return list;
        }

        private static IEnumerable<ImportJobProfile> GetLiveMatrixImportJobProfiles(int count)
        {
            var list = new List<ImportJobProfile>();

            for (var i = 0; i < count; i++)
            {
                list.Add(new ImportJobProfile { Title = nameof(SocCode.Title), SOCCode = $"{i}-{nameof(SocSkillMatrix.SocCode)}", ONetOccupationalCode = nameof(SocCode.ONetOccupationalCode), DigitalSkillsLevel = nameof(ImportJobProfile.DigitalSkillsLevel) });
            }
            return list;
        }

    }
}