using AutoMapper;
using DFC.Digital.Core;
using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using DFC.Digital.Web.Sitefinity.Core;
using DFC.Digital.Web.Sitefinity.JobProfileModule.Mvc.Controllers;
using DFC.Digital.Web.Sitefinity.JobProfileModule.Mvc.Models;
using FakeItEasy;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using TestStack.FluentMVCTesting;
using Xunit;

namespace DFC.Digital.Web.Sitefinity.JobProfileModule.UnitTests
{
    public class BaseJobProfileWidgetControllerTests
    {
        [Theory]
        [InlineData("Plumber", "plumber")]
        [InlineData("Colon Hydrotherapist", "colon hydrotherapist")]
        public void DynamicSectionTitleWithLowerCaseTest(string title, string expected)
        {
            //Setup the fakes and dummies
            var repositoryFake = A.Fake<IJobProfileRepository>(ops => ops.Strict());
            var socCodeRepositoryFake = A.Fake<IJobProfileSocCodeRepository>(ops => ops.Strict());
            var coursesearchFake = A.Fake<ICourseSearchService>(ops => ops.Strict());
            var loggerFake = A.Fake<IApplicationLogger>();
            var webAppContextFake = A.Fake<IWebAppContext>(ops => ops.Strict());
            var sitefinityPage = A.Fake<ISitefinityPage>(ops => ops.Strict());
            var dummyJobProfile = GetDummyJobPRofile(true);
            dummyJobProfile.Title = title;

            // Set up calls
            A.CallTo(() => repositoryFake.GetByUrlName(A<string>._)).Returns(dummyJobProfile);
            A.CallTo(() => repositoryFake.GetByUrlNameForPreview(A<string>._)).Returns(dummyJobProfile);
            A.CallTo(() => sitefinityPage.GetDefaultJobProfileToUse(A<string>._)).ReturnsLazily((string defaultProfile) => defaultProfile);
            A.CallTo(() => webAppContextFake.IsContentAuthoringSite).Returns(true);
            A.CallTo(() => webAppContextFake.IsContentPreviewMode).Returns(true);

            //Instantiate & Act
            using (var jobProfileChangeTitleCaseController = new TestBaseJobProfileWidgetController(webAppContextFake, repositoryFake, loggerFake, sitefinityPage))
            {
                //Act
                var result = jobProfileChangeTitleCaseController.ChangeWordCase(title);

                //Assert
                result.Should().BeEquivalentTo(expected);
            }
        }

        [Theory]
        [InlineData("UX Designer", "UX designer")]
        [InlineData("English as a foreign language (EFL) teacher", "english as a foreign language (EFL) teacher")]
        public void DynamicSectionTitleWithAcronym(string title, string expected)
        {
            //Setup the fakes and dummies
            var repositoryFake = A.Fake<IJobProfileRepository>(ops => ops.Strict());
            var socCodeRepositoryFake = A.Fake<IJobProfileSocCodeRepository>(ops => ops.Strict());
            var coursesearchFake = A.Fake<ICourseSearchService>(ops => ops.Strict());
            var loggerFake = A.Fake<IApplicationLogger>();
            var webAppContextFake = A.Fake<IWebAppContext>(ops => ops.Strict());
            var sitefinityPage = A.Fake<ISitefinityPage>(ops => ops.Strict());
            var dummyJobProfile = GetDummyJobPRofile(true);
            dummyJobProfile.Title = title;

            // Set up calls
            A.CallTo(() => repositoryFake.GetByUrlName(A<string>._)).Returns(dummyJobProfile);
            A.CallTo(() => repositoryFake.GetByUrlNameForPreview(A<string>._)).Returns(dummyJobProfile);
            A.CallTo(() => sitefinityPage.GetDefaultJobProfileToUse(A<string>._)).ReturnsLazily((string defaultProfile) => defaultProfile);
            A.CallTo(() => webAppContextFake.IsContentAuthoringSite).Returns(true);
            A.CallTo(() => webAppContextFake.IsContentPreviewMode).Returns(true);

            //Instantiate & Act
            using (var jobProfileCheckForAcronymController = new TestBaseJobProfileWidgetController(webAppContextFake, repositoryFake, loggerFake, sitefinityPage))
            {
                //Act
                var result = jobProfileCheckForAcronymController.CheckForAcronym(title);

                //Assert
                result.Should().BeEquivalentTo(expected);
            }
        }

        [Theory]
        [InlineData("Her", true)]
        [InlineData("Royal", true)]
        [InlineData("Teacher", false)]

        public void DynamicSectionTitleCheckForSpecialConditionTest(string title, bool expected)
        {
            //Setup the fakes and dummies
            var repositoryFake = A.Fake<IJobProfileRepository>(ops => ops.Strict());
            var socCodeRepositoryFake = A.Fake<IJobProfileSocCodeRepository>(ops => ops.Strict());
            var coursesearchFake = A.Fake<ICourseSearchService>(ops => ops.Strict());
            var loggerFake = A.Fake<IApplicationLogger>();
            var webAppContextFake = A.Fake<IWebAppContext>(ops => ops.Strict());
            var sitefinityPage = A.Fake<ISitefinityPage>(ops => ops.Strict());
            var dummyJobProfile = GetDummyJobPRofile(true);
            dummyJobProfile.Title = title;

            // Set up calls
            A.CallTo(() => repositoryFake.GetByUrlName(A<string>._)).Returns(dummyJobProfile);
            A.CallTo(() => repositoryFake.GetByUrlNameForPreview(A<string>._)).Returns(dummyJobProfile);
            A.CallTo(() => sitefinityPage.GetDefaultJobProfileToUse(A<string>._)).ReturnsLazily((string defaultProfile) => defaultProfile);
            A.CallTo(() => webAppContextFake.IsContentAuthoringSite).Returns(true);
            A.CallTo(() => webAppContextFake.IsContentPreviewMode).Returns(true);

            //Instantiate & Act
            using (var jobProfileCheckForAcronymController = new TestBaseJobProfileWidgetController(webAppContextFake, repositoryFake, loggerFake, sitefinityPage))
            {
                //Act
                var result = jobProfileCheckForAcronymController.SpecialConditionWords(title);

                //Assert
                result.Should().Be(expected);
            }
        }

        [Theory]
        [InlineData("No Prefix", "test", "test")]
        [InlineData("Prefix with a", "test", "a test")]
        [InlineData("Prefix with an", "test", "an test")]
        [InlineData("test", "test", "a test")]
        [InlineData("test", "etest", "an etest")]
        [InlineData("No Title", "test", "")]
        public void GetDynamicTitleTest(string dynamicTitlePrefix, string title, string expected)
        {
            //Setup the fakes and dummies
            var repositoryFake = A.Fake<IJobProfileRepository>(ops => ops.Strict());
            var loggerFake = A.Fake<IApplicationLogger>();
            var webAppContextFake = A.Fake<IWebAppContext>(ops => ops.Strict());
            var sitefinityPage = A.Fake<ISitefinityPage>(ops => ops.Strict());
            var dummyJobProfile = GetDummyJobPRofile(true);
            dummyJobProfile.DynamicTitlePrefix = dynamicTitlePrefix;
            dummyJobProfile.Title = title;

            // Set up calls
            //A.CallTo(() => repositoryFake.GetByUrlName(A<string>._)).Returns(dummyJobProfile);
            A.CallTo(() => repositoryFake.GetByUrlNameForPreview(A<string>._)).Returns(dummyJobProfile);
            A.CallTo(() => sitefinityPage.GetDefaultJobProfileToUse(A<string>._)).ReturnsLazily((string defaultProfile) => defaultProfile);
            A.CallTo(() => webAppContextFake.IsContentAuthoringSite).Returns(true);
            A.CallTo(() => webAppContextFake.IsContentPreviewMode).Returns(true);

            //Instantiate & Act
            using (var jobProfileHowToBecomeController = new TestBaseJobProfileWidgetController(webAppContextFake, repositoryFake, loggerFake, sitefinityPage))
            {
                //Act
                var result = jobProfileHowToBecomeController.GetDynamicTitle(false);

                //Assert
                result.Should().BeEquivalentTo(expected);
            }
        }

        [Theory]
        [InlineData("No Title", "test", "a test")]
        [InlineData("No Title", "etest", "an etest")]
        public void DynamicSectionTitleForCoursesAndApprenticeshipsTest(string htbPrefix, string title, string expected)
        {
            //Setup the fakes and dummies
            var repositoryFake = A.Fake<IJobProfileRepository>(ops => ops.Strict());
            var socCodeRepositoryFake = A.Fake<IJobProfileSocCodeRepository>(ops => ops.Strict());
            var coursesearchFake = A.Fake<ICourseSearchService>(ops => ops.Strict());
            var loggerFake = A.Fake<IApplicationLogger>();
            var webAppContextFake = A.Fake<IWebAppContext>(ops => ops.Strict());
            var sitefinityPage = A.Fake<ISitefinityPage>(ops => ops.Strict());
            var dummyJobProfile = GetDummyJobPRofile(true);
            dummyJobProfile.DynamicTitlePrefix = htbPrefix;
            dummyJobProfile.Title = title;

            var dummyCourses = new EnumerableQuery<Course>(new List<Course>
            {
                new Course
                {
                    Title = $"dummy {nameof(Course.Title)}",
                    Location = $"dummy {nameof(Course.Location)}",
                    CourseId = $"dummy {nameof(Course.CourseId)}",
                    StartDate = default(DateTime),
                    ProviderName = $"dummy {nameof(Course.ProviderName)}"
                },
                new Course
                {
                    Title = $"dummy {nameof(Course.Title)}",
                    Location = $"dummy {nameof(Course.Location)}",
                    CourseId = $"dummy {nameof(Course.CourseId)}",
                    StartDate = default(DateTime),
                    ProviderName = $"dummy {nameof(Course.ProviderName)}"
                }
            });

            var dummyApprenticeships = new EnumerableQuery<ApprenticeVacancy>(new List<ApprenticeVacancy>
            {
                new ApprenticeVacancy
                {
                    Title = $"dummy {nameof(ApprenticeVacancy.Title)}",
                    Location = $"dummy {nameof(ApprenticeVacancy.Location)}",
                    URL = new Uri($"/dummy{nameof(ApprenticeVacancy.URL)}", UriKind.RelativeOrAbsolute),
                    VacancyId = $"dummy {nameof(ApprenticeVacancy.VacancyId)}",
                    WageAmount = "£3",
                    WageUnitType = $"dummy {nameof(ApprenticeVacancy.WageUnitType)}"
                },
                new ApprenticeVacancy
                {
                    Title = $"dummy {nameof(ApprenticeVacancy.Title)}",
                    Location = $"dummy {nameof(ApprenticeVacancy.Location)}",
                    URL = new Uri($"/dummy{nameof(ApprenticeVacancy.URL)}", UriKind.RelativeOrAbsolute),
                    VacancyId = $"dummy {nameof(ApprenticeVacancy.VacancyId)}",
                    WageAmount = "£3",
                    WageUnitType = $"dummy {nameof(ApprenticeVacancy.WageUnitType)}"
                }
            });

            // Set up calls
            A.CallTo(() => repositoryFake.GetByUrlName(A<string>._)).Returns(dummyJobProfile);
            A.CallTo(() => repositoryFake.GetByUrlNameForPreview(A<string>._)).Returns(dummyJobProfile);
            A.CallTo(() => socCodeRepositoryFake.GetApprenticeVacanciesBySocCode(A<string>._)).Returns(dummyApprenticeships);
            A.CallTo(() => coursesearchFake.GetCoursesAsync(A<string>._)).Returns(dummyCourses);
            A.CallTo(() => sitefinityPage.GetDefaultJobProfileToUse(A<string>._)).ReturnsLazily((string defaultProfile) => defaultProfile);
            A.CallTo(() => webAppContextFake.IsContentAuthoringSite).Returns(true);
            A.CallTo(() => webAppContextFake.IsContentPreviewMode).Returns(true);

            //Instantiate & Act
            using (var jobProfileApprenticeshipsAndCoursesController = new TestBaseJobProfileWidgetController(webAppContextFake, repositoryFake, loggerFake, sitefinityPage))
            {
                //Act
                var result = jobProfileApprenticeshipsAndCoursesController.GetDynamicTitle(true);

                //Assert
                result.Should().BeEquivalentTo(expected);
            }
        }

        [Theory]
        [InlineData(true, true)]
        [InlineData(false, true)]
        [InlineData(false, false)]
        public void BaseIndexTest(bool inContentAuthoringSite, bool isContentPreviewMode)
        {
            //Setup the fakes and dummies
            var repositoryFake = A.Fake<IJobProfileRepository>(ops => ops.Strict());
            var loggerFake = A.Fake<IApplicationLogger>();
            var webAppContextFake = A.Fake<IWebAppContext>(ops => ops.Strict());
            var sitefinityPage = A.Fake<ISitefinityPage>(ops => ops.Strict());
            var formatContentServiceFake = A.Fake<IFormatContentService>(ops => ops.Strict());
            var dummyJobProfile = GetDummyJobPRofile(true);

            // Set up calls
            A.CallTo(() => repositoryFake.GetByUrlName(A<string>._)).Returns(dummyJobProfile);
            A.CallTo(() => repositoryFake.GetByUrlNameForPreview(A<string>._)).Returns(dummyJobProfile);
            A.CallTo(() => webAppContextFake.IsContentAuthoringSite).Returns(inContentAuthoringSite);
            A.CallTo(() => webAppContextFake.IsContentPreviewMode).Returns(isContentPreviewMode);
            A.CallTo(() => formatContentServiceFake.GetParagraphText(A<string>._, A<IEnumerable<string>>._, A<string>._)).Returns("test");
            A.CallTo(() => sitefinityPage.GetDefaultJobProfileToUse(A<string>._)).ReturnsLazily((string defaultProfile) => defaultProfile);

            //Instantiate & Act
            using (var baseJobprofileController = new JobProfileWhatYouWillDoController(repositoryFake, webAppContextFake, loggerFake, sitefinityPage, formatContentServiceFake))
            {
                //Act
                var indexMethodCall = baseJobprofileController.WithCallTo(c => c.BaseIndex());

                //Assert
                A.CallTo(() => webAppContextFake.IsContentAuthoringSite).MustHaveHappened();

                if (inContentAuthoringSite)
                {
                    A.CallTo(() => sitefinityPage.GetDefaultJobProfileToUse(A<string>._)).MustHaveHappened();
                }
                else
                {
                    indexMethodCall.ShouldRedirectTo("\\");
                }
            }
        }

        [Theory]
        [InlineData("Test", true, false)]
        [InlineData("Test", false, false)]
        [InlineData("Test", true, true)]
        [InlineData("Test", false, true)]
        public void BaseIndexUrlNameTest(string urlName, bool useValidJobProfile, bool isContentPreviewMode)
        {
            //Setup the fakes and dummies
            var repositoryFake = A.Fake<IJobProfileRepository>(ops => ops.Strict());
            var loggerFake = A.Fake<IApplicationLogger>();
            var webAppContextFake = A.Fake<IWebAppContext>(ops => ops.Strict());
            var sitefinityPage = A.Fake<ISitefinityPage>(ops => ops.Strict());
            var formatContentServiceFake = A.Fake<IFormatContentService>(ops => ops.Strict());

            var dummyJobProfile = GetDummyJobPRofile(useValidJobProfile);

            // Set up calls
            A.CallTo(() => repositoryFake.GetByUrlName(A<string>._)).Returns(dummyJobProfile);
            A.CallTo(() => repositoryFake.GetByUrlNameForPreview(A<string>._)).Returns(dummyJobProfile);
            A.CallTo(() => webAppContextFake.IsContentPreviewMode).Returns(isContentPreviewMode);
            A.CallTo(() => webAppContextFake.SetMetaDescription(A<string>._)).DoesNothing();
            A.CallTo(() => sitefinityPage.GetDefaultJobProfileToUse(A<string>._)).ReturnsLazily((string defaultProfile) => defaultProfile);
            A.CallTo(() => formatContentServiceFake.GetParagraphText(A<string>._, A<IEnumerable<string>>._, A<string>._)).Returns("test");

            //Instantiate & Act
            using (var baseJobprofileController = new JobProfileWhatYouWillDoController(repositoryFake, webAppContextFake, loggerFake, sitefinityPage, formatContentServiceFake))
            {
                //Act
                var indexWithUrlNameMethodCall = baseJobprofileController.WithCallTo(c => c.BaseIndex(urlName));
                A.CallTo(() => webAppContextFake.IsContentPreviewMode).MustHaveHappened();
                if (isContentPreviewMode)
                {
                    A.CallTo(() => repositoryFake.GetByUrlNameForPreview(A<string>._)).MustHaveHappened();
                    A.CallTo(() => repositoryFake.GetByUrlName(A<string>._)).MustNotHaveHappened();
                }
                else
                {
                    A.CallTo(() => repositoryFake.GetByUrlName(A<string>._)).MustHaveHappened();
                    A.CallTo(() => repositoryFake.GetByUrlNameForPreview(A<string>._)).MustNotHaveHappened();
                }

                if (useValidJobProfile)
                {
                    A.CallTo(() => webAppContextFake.SetMetaDescription(A<string>._)).MustHaveHappened();
                }
                else
                {
                    A.CallTo(() => webAppContextFake.SetMetaDescription(A<string>._)).MustNotHaveHappened();
                    indexWithUrlNameMethodCall.ShouldGiveHttpStatus(404);
                }
            }
        }

        private static JobProfile GetDummyJobPRofile(bool useValidJobProfile)
        {
            return useValidJobProfile ?
                   new JobProfile
                   {
                       AlternativeTitle = nameof(JobProfile.AlternativeTitle),
                       SalaryRange = nameof(JobProfile.SalaryRange),
                       Overview = nameof(JobProfile.Overview),
                       Title = nameof(JobProfile.Title),
                       SOCCode = nameof(JobProfile.SOCCode),
                       CareerPathAndProgression = nameof(JobProfile.CareerPathAndProgression),
                       HowToBecome = nameof(JobProfile.HowToBecome),
                       Salary = nameof(JobProfile.Salary),
                       Skills = nameof(JobProfile.Skills),
                       WhatYouWillDo = nameof(JobProfile.WhatYouWillDo),
                       WhatYouWillDoData = new WhatYouWillDo { Locations = new List<string> { "Office and Client Site" }, Uniforms = new List<string> { "Casual / Smart Casual / Business Dress" }, Environments = new List<string> { "Friendly / Business / Secured" }, IsCadReady = true },
                       WorkingHoursPatternsAndEnvironment = nameof(JobProfile.WorkingHoursPatternsAndEnvironment),
                       HowToBecomeData = new HowToBecome(),
                       Restrictions = new List<Restriction> { new Restriction { Info = nameof(Restriction.Info), Title = nameof(Restriction.Title) } },
                       OtherRequirements = nameof(JobProfile.OtherRequirements),
                       DynamicTitlePrefix = nameof(JobProfile.DynamicTitlePrefix)
                   }
                   : null;
        }
    }
}