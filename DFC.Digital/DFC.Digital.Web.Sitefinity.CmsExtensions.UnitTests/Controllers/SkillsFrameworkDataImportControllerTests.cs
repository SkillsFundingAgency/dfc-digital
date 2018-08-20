using System.Collections.Generic;
using DFC.Digital.Core;
using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using DFC.Digital.Web.Sitefinity.CmsExtensions;
using DFC.Digital.Web.Sitefinity.CmsExtensions.MVC.Controllers;
using FakeItEasy;
using FluentAssertions;
using TestStack.FluentMVCTesting;
using Xunit;

namespace DFC.Digital.Web.Sitefinity.CmsExtensions.UnitTests.Controllers
{
    public class SkillsFrameworkDataImportControllerTests
    {

        private readonly IImportSkillsFrameworkDataService fakeImportSkillsFrameworkDataService;
        private readonly IReportAuditRepository fakeReportAuditRepository;
        private readonly IWebAppContext fakeWebAppContext;
        private readonly IApplicationLogger fakeApplicationLogger;

        public SkillsFrameworkDataImportControllerTests()
        {
            fakeImportSkillsFrameworkDataService = A.Fake<IImportSkillsFrameworkDataService>(ops => ops.Strict());
            fakeReportAuditRepository = A.Fake<IReportAuditRepository>(ops => ops.Strict());
            fakeWebAppContext = A.Fake<IWebAppContext>(ops => ops.Strict());
            fakeApplicationLogger = A.Fake<IApplicationLogger>(ops => ops.Strict());
            SetupCalls();
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void IndexTest(bool isAdmin)
        {
            // Assign
            var skillsFrameworkDataImportController = GetSkillsFrameworkDataImportController(isAdmin);

            // Act
            var indexMethodCall = skillsFrameworkDataImportController.WithCallTo(c => c.Index());

            // Assert
            indexMethodCall
                .ShouldRenderDefaultView()
                .WithModel<SkillsFrameworkImportViewModel>(vm =>
                {
                    vm.FirstParagraph.Should().BeEquivalentTo(skillsFrameworkDataImportController.FirstParagraph);
                    vm.IsAdmin.Should().Be(fakeWebAppContext.IsUserAdministrator);
                    vm.NotAllowedMessage.Should().BeEquivalentTo(skillsFrameworkDataImportController.NotAllowedMessage);
                    vm.PageTitle.Should().BeEquivalentTo(skillsFrameworkDataImportController.PageTitle);
                })
                .AndNoModelErrors();

            A.CallTo(() => fakeWebAppContext.IsUserAdministrator).MustHaveHappened();
            A.CallTo(() => fakeImportSkillsFrameworkDataService.UpdateSocCodesOccupationalCode()).MustNotHaveHappened();
            A.CallTo(() => fakeImportSkillsFrameworkDataService.ImportFrameworkSkills()).MustNotHaveHappened();
            A.CallTo(() => fakeReportAuditRepository.GetAllAuditRecords()).MustNotHaveHappened();
        }

        [Theory]
        [InlineData("IMPORTSKILLS")]
        [InlineData("UPDATESOCOCCUPATIONALCODES")]
        [InlineData("UPDATEJPDIGITALSKILLS")]
        [InlineData("BUILDSOCMATRIX")]
        [InlineData("UPDATEJPSKILLS")]
        [InlineData("")]
        public void IndexModeTest(string mode)
        {
            // Assign
            A.CallTo(() => fakeReportAuditRepository.GetAllAuditRecords()).Returns(GetAuditRecords());
            var skillsFrameworkDataImportController = GetSkillsFrameworkDataImportController(true);

            // Act
            var indexMethodCall = skillsFrameworkDataImportController.WithCallTo(c => c.Index(mode));

            // Assert
            indexMethodCall
                .ShouldRenderView("ImportResults")
                .WithModel<SkillsFrameworkResultsViewModel>(vm =>
                {
                    vm.FirstParagraph.Should().BeEquivalentTo(skillsFrameworkDataImportController.FirstParagraph);
                    vm.IsAdmin.Should().Be(fakeWebAppContext.IsUserAdministrator);
                    vm.NotAllowedMessage.Should().BeEquivalentTo(skillsFrameworkDataImportController.NotAllowedMessage);
                    vm.PageTitle.Should().BeEquivalentTo(skillsFrameworkDataImportController.PageTitle);
                    vm.AuditRecords.Should().BeEquivalentTo(fakeReportAuditRepository.GetAllAuditRecords());
                })
                .AndNoModelErrors();

            switch (mode?.ToUpperInvariant().Trim())
            {
                case "IMPORTSKILLS":
                    A.CallTo(() => fakeImportSkillsFrameworkDataService.ImportFrameworkSkills()).MustHaveHappened();
                    break;
                case "UPDATESOCOCCUPATIONALCODES":
                    A.CallTo(() => fakeImportSkillsFrameworkDataService.UpdateSocCodesOccupationalCode()).MustHaveHappened();
                    break;
                    default:
                        A.CallTo(() => fakeImportSkillsFrameworkDataService.UpdateSocCodesOccupationalCode()).MustNotHaveHappened();
                        A.CallTo(() => fakeImportSkillsFrameworkDataService.ImportFrameworkSkills()).MustNotHaveHappened();
                    break;
            }

            A.CallTo(() => fakeWebAppContext.IsUserAdministrator).MustHaveHappened();
            A.CallTo(() => fakeReportAuditRepository.GetAllAuditRecords()).MustHaveHappened();
        }

        private SkillsFrameworkDataImportController GetSkillsFrameworkDataImportController(bool isAdmin)
        {
            A.CallTo(() => fakeWebAppContext.IsUserAdministrator).Returns(isAdmin);

            var skillsFrameworkDataImportController = new SkillsFrameworkDataImportController(fakeApplicationLogger,
                fakeImportSkillsFrameworkDataService, fakeReportAuditRepository, fakeWebAppContext)
            {
                FirstParagraph = nameof(SkillsFrameworkDataImportController.FirstParagraph),
                NotAllowedMessage = nameof(SkillsFrameworkDataImportController.NotAllowedMessage),
                PageTitle = nameof(SkillsFrameworkDataImportController.PageTitle),
            };
            return skillsFrameworkDataImportController;
        }

        private void SetupCalls()
        {
            A.CallTo(() => fakeImportSkillsFrameworkDataService.UpdateSocCodesOccupationalCode()).Returns(new UpdateSocOccupationalCodeResponse());
            A.CallTo(() => fakeImportSkillsFrameworkDataService.ImportFrameworkSkills()).Returns(new FrameworkSkillsImportResponse());
        }

        private IDictionary<string, IList<string>> GetAuditRecords()
        {
            var records = new Dictionary<string, IList<string>> { { "key", new List<string> { "test" } } };
            return records;
        }
    }
}