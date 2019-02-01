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
    /// <summary>
    /// Job Profile Details Controller tests
    /// </summary>
    public class JobProfileCourseOpportunityControllerTests
    {
        [Theory]
        [InlineData(true, false, 2, "coursekeywords")]
        [InlineData(true, false, 1, "coursekeywords")]
        [InlineData(false, false, 2, "coursekeywords")]
        [InlineData(true, true, 1, "coursekeywords")]
        [InlineData(true, true, 2, "coursekeywords")]
        [InlineData(false, true, 1, "coursekeywords")]
        [InlineData(false, true, 2, "coursekeywords")]
        [InlineData(false, true, 2, "")]
        public void IndexTest(bool inContentAuthoringSite, bool isContentPreviewMode, int maxCourses, string courseKeywords)
        {
            //Setup the fakes and dummies
            var repositoryFake = A.Fake<IJobProfileRepository>(ops => ops.Strict());
            var coursesearchFake = A.Fake<ICourseSearchService>(ops => ops.Strict());
            var loggerFake = A.Fake<IApplicationLogger>();
            var webAppContextFake = A.Fake<IWebAppContext>(ops => ops.Strict());
            var sitefinityPage = A.Fake<ISitefinityPage>(ops => ops.Strict());

            var dummyJobProfile =
                new JobProfile
                {
                    AlternativeTitle = $"dummy {nameof(JobProfile.AlternativeTitle)}",
                    WidgetContentTitle = $"dummy {nameof(JobProfile.Title)}",
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

            A.CallTo(() => coursesearchFake.GetCoursesAsync(A<string>._)).Returns(dummyCourses);

            //Instantiate & Act
            var jobProfileCourseOpportunityController = new JobProfileCourseOpportunityController(coursesearchFake, new AsyncHelper(), webAppContextFake, repositoryFake, loggerFake, sitefinityPage)
            {
                CoursesSectionTitle = nameof(JobProfileCourseOpportunityController.CoursesSectionTitle),
                TrainingCoursesLocationDetails = nameof(JobProfileCourseOpportunityController.TrainingCoursesLocationDetails),
                TrainingCoursesText = nameof(JobProfileCourseOpportunityController.TrainingCoursesText),

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
                        vm.TrainingCoursesText.Should().BeEquivalentTo(jobProfileCourseOpportunityController
                            .TrainingCoursesText);
                        vm.CoursesLocationDetails.Should().Be(jobProfileCourseOpportunityController.TrainingCoursesLocationDetails);
                        vm.NoTrainingCoursesText.Should().Be(jobProfileCourseOpportunityController.NoTrainingCoursesText);
                        vm.CoursesSectionTitle.Should().Be(jobProfileCourseOpportunityController.CoursesSectionTitle);
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
                    A.CallTo(() => coursesearchFake.GetCoursesAsync(A<string>._)).MustHaveHappened();
                }
            }
            else
            {
                indexMethodCall.ShouldRedirectTo("\\");
            }
        }

        [Theory]
        [InlineData("Test", true, false, false, "coursekeywords")]
        [InlineData("TestInContentAuth", false, true, false, "coursekeywords")]
        [InlineData("Test", false, false, false, "coursekeywords")]
        [InlineData("Test", true, false, true, "coursekeywords")]
        [InlineData("TestInContentAuth", false, true, true, "coursekeywords")]
        [InlineData("Test", false, false, true, "coursekeywords")]
        public void IndexUrlNameTest(string urlName, bool useValidJobProfile, bool inContentAuthoringSite, bool isContentPreviewMode, string courseKeywords)
        {
            //Setup the fakes and dummies
            var repositoryFake = A.Fake<IJobProfileRepository>(ops => ops.Strict());
            var coursesearchFake = A.Fake<ICourseSearchService>(ops => ops.Strict());
            var loggerFake = A.Fake<IApplicationLogger>();
            var webAppContextFake = A.Fake<IWebAppContext>(ops => ops.Strict());
            var sitefinityPage = A.Fake<ISitefinityPage>(ops => ops.Strict());

            var dummyJobProfile = useValidJobProfile
                ? new JobProfile
                {
                    AlternativeTitle = $"dummy {nameof(JobProfile.AlternativeTitle)}",
                    WidgetContentTitle = $"dummy {nameof(JobProfile.Title)}",
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
            A.CallTo(() => webAppContextFake.SetMetaDescription(A<string>._)).DoesNothing();
            A.CallTo(() => webAppContextFake.IsContentPreviewMode).Returns(isContentPreviewMode);
            A.CallTo(() => repositoryFake.GetByUrlNameForPreview(A<string>._)).Returns(dummyJobProfile);

            A.CallTo(() => coursesearchFake.GetCoursesAsync(A<string>._)).Returns(dummyCourses);

            A.CallTo(() => sitefinityPage.GetDefaultJobProfileToUse(A<string>._)).ReturnsLazily((string defaultProfile) => defaultProfile);

            //Instantiate & Act
            var jobProfileCourseOpportunityController = new JobProfileCourseOpportunityController(coursesearchFake, new AsyncHelper(), webAppContextFake, repositoryFake, loggerFake, sitefinityPage)
            {
                CoursesSectionTitle = nameof(JobProfileCourseOpportunityController.CoursesSectionTitle),
                TrainingCoursesLocationDetails =
                    nameof(JobProfileCourseOpportunityController.TrainingCoursesLocationDetails),
                TrainingCoursesText = nameof(JobProfileCourseOpportunityController.TrainingCoursesText),
                NoTrainingCoursesText = nameof(JobProfileCourseOpportunityController.NoTrainingCoursesText),
                MaxTrainingCoursesMaxCount = 2
            };
            var dummyJobProfileCourseSearchViewModel = !useValidJobProfile
             ? new JobProfileCourseSearchViewModel
             {
                 CoursesSectionTitle = $"dummy {nameof(JobProfileCourseSearchViewModel.CoursesSectionTitle)}",
                 TrainingCoursesText = $"dummy {nameof(JobProfileCourseSearchViewModel.TrainingCoursesText)}",
                 CoursesLocationDetails = $"dummy {nameof(JobProfileCourseSearchViewModel.CoursesLocationDetails)}",
                 MainSectionTitle = $"dummy {nameof(JobProfileCourseSearchViewModel.MainSectionTitle)}",
                 NoTrainingCoursesText = $"dummy {nameof(JobProfileCourseSearchViewModel.NoTrainingCoursesText)}"
             }
             : null;

            //Act
            var indexWithUrlNameMethodCall = jobProfileCourseOpportunityController.WithCallTo(c => c.Index(urlName));

            if (inContentAuthoringSite && useValidJobProfile)
            {
                indexWithUrlNameMethodCall
                 .ShouldRenderDefaultView()
                 .WithModel<JobProfileCourseSearchViewModel>(vm =>
                 {
                     vm.CoursesSectionTitle.Should().BeEquivalentTo(dummyJobProfileCourseSearchViewModel.CoursesSectionTitle);
                     vm.TrainingCoursesText.Should().BeEquivalentTo(dummyJobProfileCourseSearchViewModel.TrainingCoursesText);
                     vm.CoursesLocationDetails.Should().BeEquivalentTo(dummyJobProfileCourseSearchViewModel.CoursesLocationDetails);
                     vm.MainSectionTitle.Should().BeEquivalentTo(dummyJobProfileCourseSearchViewModel.MainSectionTitle);
                     vm.NoTrainingCoursesText.Should().BeEquivalentTo(dummyJobProfileCourseSearchViewModel.NoTrainingCoursesText);
                 })
                 .AndNoModelErrors();
            }
            else if (useValidJobProfile)
            {
                //Assert
                indexWithUrlNameMethodCall
                    .ShouldRenderDefaultView()
                    .WithModel<JobProfileCourseSearchViewModel>(vm =>
                    {
                        vm.MainSectionTitle.Should().BeEquivalentTo(jobProfileCourseOpportunityController.MainSectionTitle);
                        vm.TrainingCoursesText.Should().BeEquivalentTo(jobProfileCourseOpportunityController.TrainingCoursesText);
                        vm.CoursesLocationDetails.Should().Be(jobProfileCourseOpportunityController.TrainingCoursesLocationDetails);
                        vm.NoTrainingCoursesText.Should().Be(jobProfileCourseOpportunityController.NoTrainingCoursesText);
                        vm.CoursesSectionTitle.Should().Be(jobProfileCourseOpportunityController.CoursesSectionTitle);
                        vm.Courses.Count().Should().BeLessOrEqualTo(jobProfileCourseOpportunityController.MaxTrainingCoursesMaxCount);
                    })
                    .AndNoModelErrors();
                if (!string.IsNullOrEmpty(courseKeywords))
                {
                    A.CallTo(() => coursesearchFake.GetCoursesAsync(A<string>._)).MustHaveHappened();
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