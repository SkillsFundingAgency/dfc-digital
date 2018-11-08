using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using DFC.Digital.Core;
using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using DFC.Digital.Web.Sitefinity.CmsExtensions.MVC.Controllers;
using FakeItEasy;
using FluentAssertions;
using TestStack.FluentMVCTesting;
using Xunit;

namespace DFC.Digital.Web.Sitefinity.CmsExtensions.UnitTests.Controllers
{
    public class ApprenticeshipVacancyReportControllerTests
    {
        private readonly IJobProfileReportRepository fakeReportRepository;
        private readonly IMapper fakeMapper;
        private readonly IApplicationLogger fakeLoggingService;
        private readonly IQueryable<ProfileAndApprenticeshipReport> fakeList;

        public ApprenticeshipVacancyReportControllerTests()
        {
            fakeReportRepository = A.Fake<IJobProfileReportRepository>(ops => ops.Strict());
            fakeMapper = A.Fake<IMapper>(ops => ops.Strict());
            fakeLoggingService = A.Fake<IApplicationLogger>(ops => ops.Strict());
        }

        [Theory]
        [InlineData(1, 0)]
        [InlineData (2 , 1)]
        [InlineData(2, 2)]
        public void IndexTest(int numberRecords, int numberOfApprenticeship)
        {
            // Setup
            A.CallTo(() => fakeReportRepository.GetApprenticeshipVacancyReport()).Returns(GetDummyReportData(numberRecords, numberOfApprenticeship));
            
            // Assign
            var reportController = new ApprenticeshipVacancyReportController(fakeLoggingService, fakeReportRepository, fakeMapper);

            // Act
            var indexMethodCall = reportController.WithCallTo(c => c.Index());

            // Assert
            indexMethodCall
                .ShouldRenderDefaultView()
                .WithModel<JobProfileApprenticeshipVacancyReportViewModel>(vm =>
                {
                    vm.ReportData.Should().HaveCount(numberRecords);
                    foreach (var r in vm.ReportData)
                    {
                        if (numberOfApprenticeship > 0)
                        {
                            r.AV1Title.Should().Contain("One");
                        }
                        if (numberOfApprenticeship > 1)
                        {
                            r.AV2Title.Should().Contain("Two");
                        }
                    }
                    vm.ExecutionTime.Should().BeGreaterThan(TimeSpan.MinValue);
                })
                .AndNoModelErrors();
            A.CallTo(() => fakeReportRepository.GetApprenticeshipVacancyReport()).MustHaveHappened();

        }

        private void SetupCalls()
        {
            A.CallTo(() => fakeReportRepository.GetApprenticeshipVacancyReport()).Returns(fakeList);
        }

        private IQueryable<ProfileAndApprenticeshipReport> GetDummyReportData(int numberRecords, int numberOfApprenticeship)
        {
            var reportData = new List<ProfileAndApprenticeshipReport>();

            for (int ii = 0; ii < numberRecords; ii++)
            {
                var r = new ProfileAndApprenticeshipReport();
                r.JobProfile = new JobProfileReport() { Title = $"Dummy Profile {ii}", Name = $"profile_name{ii}" };
                r.SocCode = new SocCode() { SOCCode = $"DummySOC {ii}", Description = $"DummySOCDescription {ii}" };
                var frameworks = new List<string>();
                frameworks.Add($"DummyFramework{ii}");
                r.Frameworks = frameworks.AsQueryable();

                var standards = new List<string>();
                standards.Add($"DummyStandards{ii}");
                r.Standards = standards.AsQueryable();

                var a = new List<ApprenticeshipVacancyReport>();

                if (numberOfApprenticeship > 0)
                {
                    a.Add(new ApprenticeshipVacancyReport() { Title = $"Apprenticeship One Vacancy {ii}", DateCreated = DateTime.Now.AddDays(-ii) });
                }

                if (numberOfApprenticeship > 1)
                {
                    a.Add(new ApprenticeshipVacancyReport() { Title = $"Apprenticeship Two Vacancy {ii}", DateCreated = DateTime.Now.AddDays(-ii) });
                }

                r.ApprenticeVacancies = a;

                reportData.Add(r);
            }
            return reportData.AsQueryable();
        }
    }
}