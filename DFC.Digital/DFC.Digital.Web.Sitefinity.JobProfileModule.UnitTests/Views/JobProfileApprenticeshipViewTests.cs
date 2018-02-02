using ASP;
using DFC.Digital.Data.Model;
using DFC.Digital.Web.Sitefinity.JobProfileModule.Mvc.Models;
using FluentAssertions;
using RazorGenerator.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace DFC.Digital.Web.Sitefinity.JobProfileModule.Tests.Views
{
    public class JobProfileApprenticeshipViewTests
    {
        //As a Citizen, I want to the see the current available Apprenticeships
        //A1 - Display of Current opportunities section for available apprenticeships
        [Fact]
        public void DFC_114_ApprenticeshipFieldsCorrectTest()
        {
            var index = new _MVC_Views_JobProfileApprenticeships_Index_cshtml();

            var jobProfileApprenticeViewModel = GenerateJobProfileApprenticeshipViewModel();

            var htmlDom = index.RenderAsHtml(jobProfileApprenticeViewModel);

            var sectionText = htmlDom.DocumentNode.SelectNodes("//h2[contains(@class, 'heading-large')]")
                .FirstOrDefault().InnerText;
            var noOfVacancies = htmlDom.DocumentNode.SelectNodes("//h4[contains(@class, 'heading-small')]").Count();
            var noApprenticeshipText =
                htmlDom.DocumentNode.SelectNodes("//div[contains(@class, 'dfc-code-jp-novacancyText')]");

            sectionText.Should().Contain("7. Current Opportunities");
            noOfVacancies.Should().Be(2);
            noApprenticeshipText.Should().BeNull();
        }

        //As a Citizen, I want to the see the current available Apprenticeships
        //Display of Current opportunities section for unavailable apprenticeships
        [Fact]
        public void DFC_114_NoApprenticeshipTextTest()
        {
            var index = new _MVC_Views_JobProfileApprenticeships_Index_cshtml();

            var jobProfileNoApprenticeViewModel = GenerateNoApprenticeshipsViewModel();

            var htmlDom = index.RenderAsHtml(jobProfileNoApprenticeViewModel);

            var bigSectionTitleAndNo = htmlDom.DocumentNode.SelectNodes("//h2[contains(@class, 'heading-large')]")
                .FirstOrDefault().InnerText;
            var noOfVacancies = htmlDom.DocumentNode.SelectNodes("//h4[contains(@class, 'heading-small')]");
            var noApprenticeshipText = htmlDom.DocumentNode
                .SelectNodes("//div[contains(@class, 'dfc-code-jp-novacancyText')]").FirstOrDefault().InnerText;

            bigSectionTitleAndNo.Should().Contain("7. Current Opportunities");
            noOfVacancies.Should().BeNull();
            noApprenticeshipText.Should().Contain("No Apprenticeships available at this time");
        }

        //DFC 114 & 583 - As a citizen, I want to see apprenticeships displayed
        [Fact]
        public void DFC_114_583_SingleApprenticeshipTextTest()
        {
            var index = new _MVC_Views_JobProfileApprenticeships_Index_cshtml();

            var jobProfileSingleApprenticeViewModel = GenerateSingleApprenticeshipsViewModel();

            var htmlDom = index.RenderAsHtml(jobProfileSingleApprenticeViewModel);

            var bigSectionTitleAndNo = htmlDom.DocumentNode.SelectNodes("//h2[contains(@class, 'heading-large')]")
                .FirstOrDefault()?.InnerText;
            var noOfVacancies = htmlDom.DocumentNode.SelectNodes("//h4[contains(@class, 'heading-small')]").Count;
            var noApprenticeshipText =
                htmlDom.DocumentNode.SelectNodes("//div[contains(@class, 'dfc-code-jp-novacancyText')]");

            bigSectionTitleAndNo.Should().Contain(jobProfileSingleApprenticeViewModel.MainSectionTitle);
            noOfVacancies.Should().Be(1);
            noApprenticeshipText.Should().BeNull();
        }

        public JobProfileApprenticeshipViewModel GenerateJobProfileApprenticeshipViewModel()
        {
            return new JobProfileApprenticeshipViewModel
            {
                ApprenticeVacancies = DummyMultipleApprenticeVacancy(),
                ApprenticeshipSectionTitle = "Section Title",
                LocationDetails = "London",
                FindApprenticeshipLink = "http://https://dev-beta.nationalcareersservice.org.uk/",
                FindApprenticeshipText = "Test Apprenticeship Text",
                MainSectionTitle = "7. Current Opportunities",
            };
        }

        public JobProfileApprenticeshipViewModel GenerateJobProfileApprenticeshipTrainingCourseViewModel()
        {
            return new JobProfileApprenticeshipViewModel
            {
                ApprenticeVacancies = DummyMultipleApprenticeVacancy(),
                ApprenticeshipSectionTitle = "Section Title",
                LocationDetails = "London",
                FindApprenticeshipLink = "http://https://dev-beta.nationalcareersservice.org.uk/",
                FindApprenticeshipText = "Test Apprenticeship Text",
                MainSectionTitle = "7. Current Opportunities"
            };
        }

        public JobProfileApprenticeshipViewModel GenerateNoApprenticeshipsViewModel()
        {
            return new JobProfileApprenticeshipViewModel
            {
                NoVacancyText = "No Apprenticeships available at this time",
                MainSectionTitle = "7. Current Opportunities"
            };
        }

        public JobProfileApprenticeshipViewModel GenerateSingleApprenticeshipsViewModel()
        {
            return new JobProfileApprenticeshipViewModel
            {
                ApprenticeVacancies = DummySingleApprenticeVacancy(),
                ApprenticeshipSectionTitle = "Single Apprenticeship",
                LocationDetails = "London",
                FindApprenticeshipLink = "http://https://dev-beta.nationalcareersservice.org.uk/",
                FindApprenticeshipText = "Test Apprenticeship Text",
                MainSectionTitle = "7. Current Opportunities",
            };
        }

        public IEnumerable<ApprenticeVacancy> DummyMultipleApprenticeVacancy()
        {
            yield return new ApprenticeVacancy
            {
                Title = "Test Title",
                URL = "http://test.co.uk",
                WageUnitType = "£",
                WageAmount = "10.00",
                Location = "test",
                VacancyId = "1"
            };
            yield return new ApprenticeVacancy
            {
                Title = "Test Title 2",
                URL = "http://test2.co.uk",
                WageUnitType = "£",
                WageAmount = "10.00",
                Location = "test",
                VacancyId = "1"
            };
        }

        public IEnumerable<ApprenticeVacancy> DummySingleApprenticeVacancy()
        {
            yield return new ApprenticeVacancy
            {
                Title = "Test Title",
                URL = "http://test.co.uk",
                WageUnitType = "£",
                WageAmount = "10.00",
                Location = "test",
                VacancyId = "1"
            };
        }

        private IEnumerable<Course> GetDummyCourses(int courseCount)
        {
            var courses = new List<Course>();
            for (int i = 0; i < courseCount; i++)
            {
                courses.Add(new Course
                {
                    Title = $"dummy {nameof(Course.Title)}",
                    Location = $"dummy {nameof(Course.Location)}",
                    CourseId = $"dummy {nameof(Course.CourseId)}",
                    StartDate = default(DateTime),
                    ProviderName = $"dummy {nameof(Course.ProviderName)}",
                });
            }

            return courses;
        }
    }
}