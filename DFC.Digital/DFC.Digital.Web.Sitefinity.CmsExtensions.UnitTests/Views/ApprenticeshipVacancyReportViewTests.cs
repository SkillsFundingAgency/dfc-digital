using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using ASP;
using FakeItEasy;
using FluentAssertions;
using HtmlAgilityPack;
using RazorGenerator.Testing;
using Xunit;


namespace DFC.Digital.Web.Sitefinity.CmsExtensions.UnitTests.Views
{
    public class ApprenticeshipVacancyReportViewTests
    {
        [Fact]
        public void ApprenticeshipVacancyReportViewIndex()
        {
            // Arrange
            var indexView = new _MVC_Views_ApprenticeshipVacancyReport_Index_cshtml();
            var jobProfileApprenticeshipVacancyReportViewModel = new JobProfileApprenticeshipVacancyReportViewModel
            {
                ExecutionTime = new TimeSpan(1000), ReportName = "DummyReportName", ReportData = GetReportData()
            };

            var request = A.Fake<HttpContextBase>();
      
            A.CallTo(() => request.Request.QueryString).Returns(new NameValueCollection());

            // Act
            var htmlDom = indexView.RenderAsHtml(request,jobProfileApprenticeshipVacancyReportViewModel);

            //Assert
            var reportTitle = htmlDom.DocumentNode.SelectNodes("//h3").FirstOrDefault();
            reportTitle.InnerText.Should().Be("Job Profiles and Apprenticeship Vacancy Report");
        }
        

        private IEnumerable<JobProfileApprenticeshipVacancyItemViewModel> GetReportData()
        {
            var dummyReportData = new List<JobProfileApprenticeshipVacancyItemViewModel>
            {
                new JobProfileApprenticeshipVacancyItemViewModel
                {
                    JobProfileTitle = nameof(JobProfileApprenticeshipVacancyItemViewModel.JobProfileTitle),
                    SOC = nameof(JobProfileApprenticeshipVacancyItemViewModel.SOC),
                    SOCDescription = nameof(JobProfileApprenticeshipVacancyItemViewModel.SOCDescription),
                    Frameworks = nameof(JobProfileApprenticeshipVacancyItemViewModel.Frameworks),
                    Standards = nameof(JobProfileApprenticeshipVacancyItemViewModel.Standards),
                    AV1Title = nameof(JobProfileApprenticeshipVacancyItemViewModel.AV1Title),
                    AV1LastModified = nameof(JobProfileApprenticeshipVacancyItemViewModel.AV1LastModified),
                    AV2Title = nameof(JobProfileApprenticeshipVacancyItemViewModel.AV2Title),
                    AV2LastModified = nameof(JobProfileApprenticeshipVacancyItemViewModel.AV2LastModified),
                }
            };

            return dummyReportData.AsEnumerable();
        }
    }
    
}
