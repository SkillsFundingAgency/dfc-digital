using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using DFC.Digital.Web.Sitefinity.Core.Interface;
using DFC.Digital.Web.Sitefinity.JobProfileModule.Mvc.Controllers;
using DFC.Digital.Web.Sitefinity.JobProfileModule.Mvc.Models;
using FakeItEasy;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using TestStack.FluentMVCTesting;
using Xunit;

namespace DFC.Digital.Web.Sitefinity.JobProfileModule.Tests.Controllers
{
    /// <summary>
    /// Job Profile Details Controller tests
    /// </summary>
    public class JobProfileCourseOpportunityControllerTests
    {
        [Theory]
        [InlineData(1, true, false, 2, "coursekeywords")]
        [InlineData(2, true, false, 1, "coursekeywords")]
        [InlineData(3, false, false, 2, "coursekeywords")]
        [InlineData(4, false, false, 2, "coursekeywords")]
        [InlineData(5, true, true, 1, "coursekeywords")]
        [InlineData(6, true, true, 2, "coursekeywords")]
        [InlineData(7, false, true, 1, "coursekeywords")]
        [InlineData(8, false, true, 2, "coursekeywords")]
        [InlineData(8, false, true, 2, "")]
        public void IndexTest(int testIndex, bool inContentAuthoringSite, bool isContentPreviewMode, int maxCourses, string courseKeywords)
        {
            //Setup the fakes and dummies
            var unused = testIndex;
            var repositoryFake = A.Fake<IJobProfileRepository>(ops => ops.Strict());
            var coursesearchFake = A.Fake<ICourseSearchService>(ops => ops.Strict());
            var loggerFake = A.Fake<IApplicationLogger>(ops => ops.Strict());
            var webAppContextFake = A.Fake<IWebAppContext>(ops => ops.Strict());
            var sitefinityPage = A.Fake<ISitefinityPage>(ops => ops.Strict());

            var dummyJobProfile =
                new JobProfile
                {
                    AlternativeTitle = $"dummy {nameof(JobProfile.AlternativeTitle)}",

                    SalaryRange = $"dummy {nameof(JobProfile.SalaryRange)}",
                    Overview = $"dummy {nameof(JobProfile.Overview)}",
                    Title = $"dummy {nameof(JobProfile.Title)}",
                    CourseKeywords = courseKeywords
                };

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

            // Set up calls
            A.CallTo(() => repositoryFake.GetByUrlName(A<string>._)).Returns(dummyJobProfile);

            A.CallTo(() => webAppContextFake.IsContentAuthoringSite).Returns(inContentAuthoringSite);

            A.CallTo(() => webAppContextFake.IsContentPreviewMode).Returns(isContentPreviewMode);
            A.CallTo(() => repositoryFake.GetByUrlNameForPreview(A<string>._)).Returns(dummyJobProfile);
            A.CallTo(() => sitefinityPage.GetDefaultJobProfileToUse(A<string>._)).ReturnsLazily((string defaultProfile) => defaultProfile);

            A.CallTo(() => coursesearchFake.GetCourses(A<string>._)).Returns(dummyCourses);

            //Instantiate & Act
            var jobProfileCourseOpportunityController = new JobProfileCourseOpportunityController(coursesearchFake, webAppContextFake, repositoryFake, loggerFake, sitefinityPage)
            {
                CoursesSectionTitle = nameof(JobProfileCourseOpportunityController.CoursesSectionTitle),
                TrainingCoursesLocationDetails =
                   nameof(JobProfileCourseOpportunityController.TrainingCoursesLocationDetails),
                FindTrainingCoursesLink = nameof(JobProfileCourseOpportunityController.FindTrainingCoursesLink),
                FindTrainingCoursesText = nameof(JobProfileCourseOpportunityController.FindTrainingCoursesText),

                NoTrainingCoursesText = nameof(JobProfileCourseOpportunityController.NoTrainingCoursesText),
                MaxTrainingCoursesMaxCount = maxCourses,

                MainSectionTitle = nameof(JobProfileCourseOpportunityController.MainSectionTitle)
            };

            //Act
            var indexMethodCall = jobProfileCourseOpportunityController.WithCallTo(c => c.Index());

            //Assert
            if (inContentAuthoringSite)
            {
                indexMethodCall
                    .ShouldRenderDefaultView()
                    .WithModel<JobProfileCourseSearchViewModel>(vm =>
                    {
                        vm.FindTrainingCoursesLink.ShouldBeEquivalentTo(jobProfileCourseOpportunityController
                            .FindTrainingCoursesLink);
                        vm.FindTrainingCoursesText.ShouldBeEquivalentTo(jobProfileCourseOpportunityController
                            .FindTrainingCoursesText);
                        vm.CoursesLocationDetails.ShouldAllBeEquivalentTo(jobProfileCourseOpportunityController.TrainingCoursesLocationDetails);
                        vm.NoTrainingCoursesText.ShouldAllBeEquivalentTo(jobProfileCourseOpportunityController.NoTrainingCoursesText);
                        vm.CoursesSectionTitle.ShouldAllBeEquivalentTo(jobProfileCourseOpportunityController.CoursesSectionTitle);
                        vm.Courses.Count().Should()
                            .BeLessOrEqualTo(jobProfileCourseOpportunityController.MaxTrainingCoursesMaxCount);
                    })
                    .AndNoModelErrors();

                if (!isContentPreviewMode)
                {
                    A.CallTo(() => repositoryFake.GetByUrlName(A<string>._)).MustHaveHappened();
                }
                else
                {
                    A.CallTo(() => repositoryFake.GetByUrlNameForPreview(A<string>._)).MustHaveHappened();
                }

                if (!string.IsNullOrEmpty(courseKeywords))
                {
                    A.CallTo(() => coursesearchFake.GetCourses(A<string>._)).MustHaveHappened();
                }
            }
            else
            {
                indexMethodCall.ShouldRedirectTo("\\");
            }
        }

        [Theory]
        [InlineData(1, "Test", true, false, false, "coursekeywords")]
        [InlineData(2, "TestInContentAuth", false, true, false, "coursekeywords")]
        [InlineData(3, "Test", true, false, false, "coursekeywords")]
        [InlineData(4, "Test", false, false, false, "coursekeywords")]
        [InlineData(5, "Test", true, false, true, "coursekeywords")]
        [InlineData(6, "TestInContentAuth", false, true, true, "coursekeywords")]
        [InlineData(7, "Test", true, false, true, "coursekeywords")]
        [InlineData(8, "Test", false, false, true, "coursekeywords")]
        public void IndexUrlNameTest(int testIndex, string urlName, bool useValidJobProfile, bool inContentAuthoringSite, bool isContentPreviewMode, string courseKeywords)
        {
            //Setup the fakes and dummies
            var unused = testIndex;
            var repositoryFake = A.Fake<IJobProfileRepository>(ops => ops.Strict());
            var coursesearchFake = A.Fake<ICourseSearchService>(ops => ops.Strict());
            var loggerFake = A.Fake<IApplicationLogger>(ops => ops.Strict());
            var webAppContextFake = A.Fake<IWebAppContext>(ops => ops.Strict());
            var sitefinityPage = A.Fake<ISitefinityPage>(ops => ops.Strict());

            var dummyJobProfile = useValidJobProfile
                ? new JobProfile
                {
                    AlternativeTitle = $"dummy {nameof(JobProfile.AlternativeTitle)}",
                    SalaryRange = $"dummy {nameof(JobProfile.SalaryRange)}",
                    Overview = $"dummy {nameof(JobProfile.Overview)}",
                    Title = $"dummy {nameof(JobProfile.Title)}",
                    CourseKeywords = courseKeywords
                }
                : null;

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

            // Set up calls
            A.CallTo(() => repositoryFake.GetByUrlName(A<string>._)).Returns(dummyJobProfile);
            A.CallTo(() => webAppContextFake.IsContentAuthoringSite).Returns(inContentAuthoringSite);

            A.CallTo(() => webAppContextFake.IsContentPreviewMode).Returns(isContentPreviewMode);
            A.CallTo(() => repositoryFake.GetByUrlNameForPreview(A<string>._)).Returns(dummyJobProfile);

            A.CallTo(() => coursesearchFake.GetCourses(A<string>._)).Returns(dummyCourses);

            A.CallTo(() => sitefinityPage.GetDefaultJobProfileToUse(A<string>._)).ReturnsLazily((string defaultProfile) => defaultProfile);

            //Instantiate & Act
            var jobProfileCourseOpportunityController = new JobProfileCourseOpportunityController(coursesearchFake, webAppContextFake, repositoryFake, loggerFake, sitefinityPage)
            {
                CoursesSectionTitle = nameof(JobProfileCourseOpportunityController.CoursesSectionTitle),
                TrainingCoursesLocationDetails =
                    nameof(JobProfileCourseOpportunityController.TrainingCoursesLocationDetails),
                FindTrainingCoursesLink = nameof(JobProfileCourseOpportunityController.FindTrainingCoursesLink),
                FindTrainingCoursesText = nameof(JobProfileCourseOpportunityController.FindTrainingCoursesText),
                NoTrainingCoursesText = nameof(JobProfileCourseOpportunityController.NoTrainingCoursesText),
                MaxTrainingCoursesMaxCount = 2
            };

            //Act
            var indexWithUrlNameMethodCall = jobProfileCourseOpportunityController.WithCallTo(c => c.Index(urlName));

            if (useValidJobProfile)
            {
                //Assert
                indexWithUrlNameMethodCall
                    .ShouldRenderDefaultView()
                    .WithModel<JobProfileCourseSearchViewModel>(vm =>
                    {
                        vm.MainSectionTitle.ShouldBeEquivalentTo(jobProfileCourseOpportunityController
                            .MainSectionTitle);

                        vm.FindTrainingCoursesLink.ShouldBeEquivalentTo(jobProfileCourseOpportunityController
                            .FindTrainingCoursesLink);
                        vm.FindTrainingCoursesText.ShouldBeEquivalentTo(jobProfileCourseOpportunityController
                            .FindTrainingCoursesText);
                        vm.CoursesLocationDetails.ShouldAllBeEquivalentTo(jobProfileCourseOpportunityController.TrainingCoursesLocationDetails);
                        vm.NoTrainingCoursesText.ShouldAllBeEquivalentTo(jobProfileCourseOpportunityController.NoTrainingCoursesText);
                        vm.CoursesSectionTitle.ShouldAllBeEquivalentTo(jobProfileCourseOpportunityController.CoursesSectionTitle);
                        vm.Courses.Count().Should()
                            .BeLessOrEqualTo(jobProfileCourseOpportunityController.MaxTrainingCoursesMaxCount);
                    })
                    .AndNoModelErrors();
                if (!string.IsNullOrEmpty(courseKeywords))
                {
                    A.CallTo(() => coursesearchFake.GetCourses(A<string>._)).MustHaveHappened();
                }
            }
            else if (!inContentAuthoringSite)
            {
                indexWithUrlNameMethodCall.ShouldGiveHttpStatus(404);
            }

            if (!isContentPreviewMode)
            {
                A.CallTo(() => repositoryFake.GetByUrlName(A<string>._)).MustHaveHappened();
            }
            else
            {
                A.CallTo(() => repositoryFake.GetByUrlNameForPreview(A<string>._)).MustHaveHappened();
            }
        }
    }
}