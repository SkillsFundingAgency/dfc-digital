
using System;
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
        [InlineData(true, 10)]
        [InlineData(false, 10)]
        public void IndexTest(bool isAdmin, int batchSizeForImport)
        {
            // Assign
            var skillsFrameworkDataImportController = GetSkillsFrameworkDataImportController(isAdmin, batchSizeForImport);

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
        [InlineData(true, 10,"test")]
        [InlineData(false, 10, "test")]
        public void ImportJobProfileTest(bool isAdmin, int batchSizeForImport, string jobProfileSoc)
        {
            // Assign
            var skillsFrameworkDataImportController = GetSkillsFrameworkDataImportController(isAdmin, batchSizeForImport);
            A.CallTo(() => fakeReportAuditRepository.GetAllAuditRecords()).Returns(GetAuditRecords());

            // Act
            var importJobProfileMethodCall = skillsFrameworkDataImportController.WithCallTo(c => c.ImportJobProfile(jobProfileSoc));
            // Assert
            importJobProfileMethodCall
                .ShouldRenderView("ImportResults")
                .WithModel<SkillsFrameworkResultsViewModel>(vm =>
                {
                    vm.FirstParagraph.Should().BeEquivalentTo(skillsFrameworkDataImportController.FirstParagraph);
                    vm.IsAdmin.Should().Be(fakeWebAppContext.IsUserAdministrator);
                    vm.NotAllowedMessage.Should().BeEquivalentTo(skillsFrameworkDataImportController.NotAllowedMessage);
                    vm.PageTitle.Should().BeEquivalentTo(skillsFrameworkDataImportController.PageTitle);
                })
                .AndNoModelErrors();

            A.CallTo(() => fakeWebAppContext.IsUserAdministrator).MustHaveHappened();

            if (isAdmin)
            {
                A.CallTo(() => fakeImportSkillsFrameworkDataService.ImportForSocs(jobProfileSoc)).MustHaveHappened();
            }
            else
            {
                importJobProfileMethodCall
              .ShouldRenderView("ImportResults")
              .WithModel<SkillsFrameworkResultsViewModel>(vm =>
              {    vm.OtherMessage.Should().BeEquivalentTo(skillsFrameworkDataImportController.NotAllowedMessage);  
              }).AndNoModelErrors();
            } 
            A.CallTo(() => fakeReportAuditRepository.GetAllAuditRecords()).MustHaveHappened();
        }

        

        [Theory]
        [InlineData(false, null, 10)]
        [InlineData(false, "", 10)]
        [InlineData(true, " IMPORTSKILLS ", 10)]
        [InlineData(true, " UPDATESOCOCCUPATIONALCODES", 10)]
        [InlineData(true, " UPDATEJPDIGITALSKILLS", 10)]
        [InlineData(true, "buildsocmatrix  ", 10)]
        [InlineData(true, "UPDATEJPSKILLS  ", 10)]
        [InlineData(true, "RESETSOCIMPORTALLSTATUS", 10)]
        [InlineData(true, "resetsocimportstartedstatus", 10)]
        [InlineData(true, "EXPORTNEWONETMAPPINGS", 10)]
        [InlineData(true, "", 10)]
        public void IndexModeTest(bool isAdmin, string mode, int batchSizeForImport)
        {
            // Assign
            A.CallTo(() => fakeReportAuditRepository.GetAllAuditRecords()).Returns(GetAuditRecords());
            A.CallTo(() => fakeImportSkillsFrameworkDataService.ImportFrameworkSkills()).Returns(new FrameworkSkillsImportResponse());
            var skillsFrameworkDataImportController = GetSkillsFrameworkDataImportController(isAdmin, batchSizeForImport);

            // Act
            var indexMethodCall = skillsFrameworkDataImportController.WithCallTo(c => c.Index(mode));

            // Assert
            if (isAdmin)
            {
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

                    case "RESETSOCIMPORTALLSTATUS":
                        A.CallTo(() => fakeImportSkillsFrameworkDataService.ResetAllSocStatus()).MustHaveHappened();

                        break;
                    case "RESETSOCIMPORTSTARTEDSTATUS":
                        A.CallTo(() => fakeImportSkillsFrameworkDataService.ResetStartedSocStatus()).MustHaveHappened();
                        break;
                  
                    case "EXPORTNEWONETMAPPINGS":
                        A.CallTo(() => fakeImportSkillsFrameworkDataService.ExportNewONetMappings()).MustHaveHappened();
                        break;

                    default:
                        A.CallTo(() => fakeImportSkillsFrameworkDataService.UpdateSocCodesOccupationalCode()).MustNotHaveHappened();
                        A.CallTo(() => fakeImportSkillsFrameworkDataService.ImportFrameworkSkills()).MustNotHaveHappened();
                        break;
                }

                A.CallTo(() => fakeWebAppContext.IsUserAdministrator).MustHaveHappened();
                A.CallTo(() => fakeReportAuditRepository.GetAllAuditRecords()).MustHaveHappened();
                A.CallTo(() => fakeImportSkillsFrameworkDataService.GetSocMappingStatus()).MustHaveHappened();
                A.CallTo(() => fakeImportSkillsFrameworkDataService.GetNextBatchOfSOCsToImport(10)).MustHaveHappened();
            }
            else
            {
                indexMethodCall
               .ShouldRenderView("ImportResults")
               .WithModel<SkillsFrameworkResultsViewModel>(vm =>
               {
                   vm.FirstParagraph.Should().BeEquivalentTo(skillsFrameworkDataImportController.FirstParagraph);
                   vm.IsAdmin.Should().Be(fakeWebAppContext.IsUserAdministrator);
                   vm.NotAllowedMessage.Should().BeEquivalentTo(skillsFrameworkDataImportController.NotAllowedMessage);
                   vm.PageTitle.Should().BeEquivalentTo(skillsFrameworkDataImportController.PageTitle);
                   vm.OtherMessage.Should().BeEquivalentTo(skillsFrameworkDataImportController.NotAllowedMessage);
                   vm.AuditRecords.Should().BeEquivalentTo(fakeReportAuditRepository.GetAllAuditRecords());

               }).AndNoModelErrors();
            }

        }

        [Theory]
        [InlineData("Error to test coverage")]
        [InlineData("Error to test coverage number 3")]
        [InlineData("number 2 of the errors to test coverage")]
        public void IndexModeExceptionTest(string errorMessage)
        {
            //set up calls
            var skillsFrameworkDataImportController = GetSkillsFrameworkDataImportController(true, 10);

            A.CallTo(() => fakeReportAuditRepository.GetAllAuditRecords()).Returns(GetAuditRecords());
            A.CallTo(() => fakeImportSkillsFrameworkDataService.ImportFrameworkSkills()).Throws(new Exception(errorMessage));

            //// Act
            var indexMethodCall = skillsFrameworkDataImportController.WithCallTo(c => c.Index("IMPORTSKILLS"));


            //Assert
            indexMethodCall
                .ShouldRenderView("ImportResults")
                .WithModel<SkillsFrameworkResultsViewModel>(
                vm => vm.OtherMessage.Should().Contain(errorMessage)
                );
            A.CallTo(() => fakeReportAuditRepository.GetAllAuditRecords()).MustHaveHappened();

        }

        [Theory]
        [InlineData("Error to test coverage")]
        [InlineData("Error to test coverage number 3")]
        [InlineData("number 2 of the errors to test coverage")]
        public void ImportJobProfileExceptionTest(string errorMessage)
        {
            //set up calls
            var skillsFrameworkDataImportController = GetSkillsFrameworkDataImportController(true, 10);

            A.CallTo(() => fakeReportAuditRepository.GetAllAuditRecords()).Returns(GetAuditRecords());
            A.CallTo(() => fakeImportSkillsFrameworkDataService.ImportForSocs("test")).Throws(new Exception(errorMessage));

            //// Act
            var indexMethodCall = skillsFrameworkDataImportController.WithCallTo(c => c.ImportJobProfile("test"));


            //Assert
            indexMethodCall
                .ShouldRenderView("ImportResults")
                .WithModel<SkillsFrameworkResultsViewModel>(
                    vm => vm.OtherMessage.Should().Contain(errorMessage)
                );
            A.CallTo(() => fakeReportAuditRepository.GetAllAuditRecords()).MustHaveHappened();

        }

        private SkillsFrameworkDataImportController GetSkillsFrameworkDataImportController(bool isAdmin, int batchSizeForImport)
        {
            A.CallTo(() => fakeWebAppContext.IsUserAdministrator).Returns(isAdmin);

            var skillsFrameworkDataImportController = new SkillsFrameworkDataImportController(fakeApplicationLogger,
                fakeImportSkillsFrameworkDataService, fakeReportAuditRepository, fakeWebAppContext)
            {
                FirstParagraph = nameof(SkillsFrameworkDataImportController.FirstParagraph),
                NotAllowedMessage = nameof(SkillsFrameworkDataImportController.NotAllowedMessage),
                PageTitle = nameof(SkillsFrameworkDataImportController.PageTitle),
                BatchSizeForImport = batchSizeForImport,
            };
            return skillsFrameworkDataImportController;
        }


        private void SetupCalls()
        {
            A.CallTo(() => fakeImportSkillsFrameworkDataService.UpdateSocCodesOccupationalCode()).Returns(new UpdateSocOccupationalCodeResponse());
            A.CallTo(() => fakeImportSkillsFrameworkDataService.GetSocMappingStatus()).Returns(new SocMappingStatus());
            A.CallTo(() => fakeImportSkillsFrameworkDataService.GetNextBatchOfSOCsToImport(10)).Returns("test");
            A.CallTo(() => fakeImportSkillsFrameworkDataService.ImportForSocs("test")).Returns(new SkillsServiceResponse());
            A.CallTo(() => fakeImportSkillsFrameworkDataService.ResetAllSocStatus()).DoesNothing();
            A.CallTo(() => fakeImportSkillsFrameworkDataService.ResetStartedSocStatus()).DoesNothing();

        }

        private IDictionary<string, IList<string>> GetAuditRecords()
        {
            var records = new Dictionary<string, IList<string>> { { "key", new List<string> { "test" } } };
            return records;
        }
    }
}