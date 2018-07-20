using DFC.Digital.Data.Interfaces;
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

        [Fact]
        public void SkillsFrameworkDataImportServiceTest()
        {
            Assert.True(false, "This test needs an implementation");
        }

        [Fact]
        public void ImportFrameworkSkillsTest()
        {
            Assert.True(false, "This test needs an implementation");
        }

        [Fact]
        public void UpdateSocCodesOccupationalCodeTest()
        {
            Assert.True(false, "This test needs an implementation");
        }

        [Fact]
        public void UpdateJobProfilesDigitalSkillsTest()
        {
            Assert.True(false, "This test needs an implementation");
        }

        [Fact]
        public void BuildSocMatrixDataTest()
        {
            Assert.True(false, "This test needs an implementation");
        }

        [Fact]
        public void UpdateJpSocSkillMatrixTest()
        {
            Assert.True(false, "This test needs an implementation");
        }
    }
}