using System;
using System.Collections.Generic;
using System.Collections.Specialized;
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
        private readonly IWebAppContext fakeWebAppContext;
        private readonly ICachingPolicy fakeCachingPolicy;
        private readonly IQueryable<JobProfileApprenticeshipVacancyReport> fakeList;
        private NameValueCollection query = new NameValueCollection();

        public ApprenticeshipVacancyReportControllerTests()
        {
            fakeReportRepository = A.Fake<IJobProfileReportRepository>(ops => ops.Strict());
            fakeLoggingService = A.Fake<IApplicationLogger>(ops => ops.Strict());
            fakeWebAppContext = A.Fake<IWebAppContext>();
            fakeCachingPolicy = A.Fake<ICachingPolicy>();
            query.Add("ctx", "something");
        }


        [Theory]
        [InlineData(1, 0)]
        [InlineData (2 , 1)]
        [InlineData(2, 2)]
        public void IndexTest(int numberRecords, int numberOfApprenticeship)
        {            
            // Setup
            A.CallTo(() => fakeReportRepository.GetJobProfileApprenticeshipVacancyReport()).Returns(GetDummyReportData(numberRecords, numberOfApprenticeship));
            A.CallTo(() => fakeWebAppContext.RequestQueryString).Returns(query);
            A.CallTo(() => fakeCachingPolicy.Execute(fakeReportRepository.GetJobProfileApprenticeshipVacancyReport, A<CachePolicyType>._, A<string>._, A<string>._)).Returns(GetDummyReportData(numberRecords, numberOfApprenticeship));
            // Assign
            var reportController = new ApprenticeshipVacancyReportController(fakeLoggingService, fakeReportRepository, fakeWebAppContext, fakeCachingPolicy);

            // Act
            var indexMethodCall = reportController.WithCallTo(c => c.Index());

            // Assert
            indexMethodCall
                .ShouldRenderDefaultView()
                .WithModel<JobProfileApprenticeshipVacancyReportViewModel>(vm =>
                {
                    vm.ReportData.Should().HaveCount(numberRecords);
                    int ii = 0;
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

                        r.Frameworks.Should().Be($"DummyFramework_1{ii}-(LARSF_1{ii})|DummyFramework_2{ii}-(LARSF_2{ii})");
                        r.Standards.Should().Be($"DummyStandard_1{ii}-(LARS_1{ii})|DummyStandard_2{ii}-(LARS_2{ii})");
                        ii++;
                    }
                    vm.ExecutionTime.Should().BeGreaterThan(TimeSpan.MinValue);
                })
                .AndNoModelErrors();
            A.CallTo(() => fakeCachingPolicy.Execute(fakeReportRepository.GetJobProfileApprenticeshipVacancyReport, A<CachePolicyType>._, A<string>._, A<string>._)).MustHaveHappened();

        }

        public void IndexRedirectTest(int numberRecords, int numberOfApprenticeship)
        {
            // Setup
            A.CallTo(() => fakeReportRepository.GetJobProfileApprenticeshipVacancyReport()).Returns(GetDummyReportData(numberRecords, numberOfApprenticeship));
            A.CallTo(() => fakeWebAppContext.RequestQueryString).Returns(null);
            A.CallTo(() => fakeWebAppContext.GetCurrentQueryString(A<Dictionary<string, object>>._)).Returns("http://url");

            // Assign
            var reportController = new ApprenticeshipVacancyReportController(fakeLoggingService, fakeReportRepository, fakeWebAppContext, fakeCachingPolicy);

            // Act
            var indexMethodCall = reportController.WithCallTo(c => c.Index());

            // Assert
            indexMethodCall.ShouldRedirectTo("http://url");
            A.CallTo(() => fakeReportRepository.GetJobProfileApprenticeshipVacancyReport()).MustNotHaveHappened();
        }

        private void SetupCalls()
        {
            A.CallTo(() => fakeReportRepository.GetJobProfileApprenticeshipVacancyReport()).Returns(fakeList);
        }

        private IQueryable<JobProfileApprenticeshipVacancyReport> GetDummyReportData(int numberRecords, int numberOfApprenticeship)
        {
            var reportData = new List<JobProfileApprenticeshipVacancyReport>();

            for (int ii = 0; ii < numberRecords; ii++)
            {
                var r = new JobProfileApprenticeshipVacancyReport();
                r.JobProfile = new JobProfileReport() { Title = $"Dummy Profile {ii}", Name = $"profile_name{ii}" };
                r.SocCode = new SocCodeReport() { SOCCode = $"DummySOC {ii}", Description = $"DummySOCDescription {ii}" };
                var frameworks = new List<TaxonReport>
                {
                   new TaxonReport {Title = $"DummyFramework_1{ii}", LarsCode = $"LARSF_1{ii}" },
                   new TaxonReport {Title = $"DummyFramework_2{ii}", LarsCode = $"LARSF_2{ii}" }
                };
                r.SocCode.Frameworks = frameworks.AsQueryable();

                var standards = new List<TaxonReport>
                {
                    new TaxonReport { Title = $"DummyStandard_1{ii}", LarsCode = $"LARS_1{ii}" },
                    new TaxonReport { Title = $"DummyStandard_2{ii}", LarsCode = $"LARS_2{ii}" },
                };
                r.SocCode.Standards = standards.AsQueryable();

                var a = new List<ApprenticeshipVacancyReport>();

                if (numberOfApprenticeship > 0)
                {
                    a.Add(new ApprenticeshipVacancyReport() { Title = $"Apprenticeship One Vacancy {ii}", DateCreated = DateTime.Now.AddDays(-ii) });
                }

                if (numberOfApprenticeship > 1)
                {
                    a.Add(new ApprenticeshipVacancyReport() { Title = $"Apprenticeship Two Vacancy {ii}", DateCreated = DateTime.Now.AddDays(-ii) });
                }

                r.ApprenticeshipVacancies = a;

                reportData.Add(r);
            }
            return reportData.AsQueryable();
        }
    }
}