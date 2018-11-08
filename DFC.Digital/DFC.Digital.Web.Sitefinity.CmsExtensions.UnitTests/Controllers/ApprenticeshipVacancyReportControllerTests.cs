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
        private readonly IApplicationLogger fakeLoggingService;
        private readonly IQueryable<JobProfileApprenticeshipVacancyReport> fakeList;

        public ApprenticeshipVacancyReportControllerTests()
        {
            fakeReportRepository = A.Fake<IJobProfileReportRepository>(ops => ops.Strict());
            fakeLoggingService = A.Fake<IApplicationLogger>(ops => ops.Strict());
            fakeList =  new List<JobProfileApprenticeshipVacancyReport>().AsQueryable();
            SetupCalls();
        }

        [Fact]
        public void IndexTest()
        {
            // Assign
            var reportController = new ApprenticeshipVacancyReportController(fakeLoggingService, fakeReportRepository);

            // Act
            var indexMethodCall = reportController.WithCallTo(c => c.Index());

            // Assert
            indexMethodCall
                .ShouldRenderDefaultView()
                .WithModel<JobProfileApprenticeshipVacancyReportViewModel>(vm =>
                {
                    vm.ReportData.Should().BeEquivalentTo(fakeList);
                    vm.ExecutionTime.Should().BeGreaterThan(TimeSpan.MinValue);
                })
                .AndNoModelErrors();
            A.CallTo(() => fakeReportRepository.JobProfileApprenticeshipVacancyReport()).MustHaveHappened();

        }

        private void SetupCalls()
        {
            A.CallTo(() => fakeReportRepository.JobProfileApprenticeshipVacancyReport()).Returns(fakeList);
        }
    }
}