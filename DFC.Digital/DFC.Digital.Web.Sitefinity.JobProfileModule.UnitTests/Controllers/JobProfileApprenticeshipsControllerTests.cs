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
    ///     Job Profile Details Controller tests
    /// </summary>
    public class JobProfileApprenticeshipsControllerTests
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "unused", Justification = "Used in debuuging tests as this number is dumped as part of the parameters - hence no need to copare them all to find the exact instance")]
        [Theory]
        [InlineData(1, true, "testtrue", false, 2)]
        [InlineData(2, true, "", false, 2)]
        [InlineData(3, false, "", false, 1)]
        [InlineData(4, false, "testfalse", false, 2)]
        [InlineData(5, true, "testtrue", true, 1)]
        [InlineData(6, true, "", true, 2)]
        [InlineData(7, false, "", true, 2)]
        [InlineData(8, false, "testfalse", true, 1)]
        public void IndexTest(int testIndex, bool inContentAuthoringSite, string socCode, bool isContentPreviewMode, int maxApp)
        {
            //Setup the fakes and dummies
            var unused = testIndex;
            var repositoryFake = A.Fake<IJobProfileRepository>(ops => ops.Strict());
            var socRepositoryFake = A.Fake<IJobProfileSocCodeRepository>(ops => ops.Strict());
            var loggerFake = A.Fake<IApplicationLogger>();
            var webAppContextFake = A.Fake<IWebAppContext>(ops => ops.Strict());
            var sitefinityPage = A.Fake<ISitefinityPage>(ops => ops.Strict());

            var dummyJobProfile =
                new JobProfile
                {
                    AlternativeTitle = $"dummy {nameof(JobProfile.AlternativeTitle)}",

                    SalaryRange = $"dummy {nameof(JobProfile.SalaryRange)}",
                    Overview = $"dummy {nameof(JobProfile.Overview)}",
                    Title = $"dummy {nameof(JobProfile.Title)}",
                    SOCCode = socCode
                };

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
            A.CallTo(() => socRepositoryFake.GetApprenticeVacanciesBySocCode(A<string>._)).Returns(dummyApprenticeships);
            A.CallTo(() => webAppContextFake.IsContentAuthoringSite).Returns(inContentAuthoringSite);

            A.CallTo(() => webAppContextFake.IsContentPreviewMode).Returns(isContentPreviewMode);
            A.CallTo(() => repositoryFake.GetByUrlNameForPreview(A<string>._)).Returns(dummyJobProfile);
            A.CallTo(() => sitefinityPage.GetDefaultJobProfileToUse(A<string>._))
                .ReturnsLazily((string defaultProfile) => defaultProfile);

            //Instantiate & Act
            var jobProfileApprenticeshipsController = new JobProfileApprenticeshipsController(repositoryFake, webAppContextFake, socRepositoryFake, loggerFake, sitefinityPage)
            {
                ApprenticeshipText = nameof(JobProfileApprenticeshipsController.ApprenticeshipText),
                NoVacancyText = nameof(JobProfileApprenticeshipsController.NoVacancyText),
                ApprenticeshipLocationDetails =
                    nameof(JobProfileApprenticeshipsController.ApprenticeshipLocationDetails),
                ApprenticeshipSectionTitle = nameof(JobProfileApprenticeshipsController.ApprenticeshipSectionTitle),
                MainSectionTitle = nameof(JobProfileApprenticeshipsController.MainSectionTitle),
                MaxApprenticeshipCount = maxApp
            };

            //Act
            var indexMethodCall = jobProfileApprenticeshipsController.WithCallTo(c => c.Index());

            //Assert
            if (inContentAuthoringSite)
            {
                indexMethodCall
                    .ShouldRenderDefaultView()
                    .WithModel<JobProfileApprenticeshipViewModel>(vm =>
                    {
                        vm.MainSectionTitle.Should().BeEquivalentTo(jobProfileApprenticeshipsController
                            .MainSectionTitle);
                        vm.ApprenticeshipText.Should().BeEquivalentTo(jobProfileApprenticeshipsController
                            .ApprenticeshipText);
                        vm.NoVacancyText.Should().BeEquivalentTo(jobProfileApprenticeshipsController
                           .NoVacancyText);
                        vm.WageTitle.Should().Be(jobProfileApprenticeshipsController
                           .ApprenticeshipWageTitle);
                        vm.LocationDetails.Should().Be(jobProfileApprenticeshipsController
                            .ApprenticeshipLocationDetails);
                        vm.NoVacancyText.Should().Be(jobProfileApprenticeshipsController.NoVacancyText);
                        vm.ApprenticeshipSectionTitle.Should().Be(jobProfileApprenticeshipsController
                            .ApprenticeshipSectionTitle);
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

                if (!string.IsNullOrWhiteSpace(socCode) && !isContentPreviewMode)
                {
                    indexMethodCall
                        .ShouldRenderDefaultView()
                        .WithModel<JobProfileApprenticeshipViewModel>(vm =>
                        {
                            vm.ApprenticeVacancies.Count().Should()
                                .BeLessOrEqualTo(jobProfileApprenticeshipsController.MaxApprenticeshipCount);
                            vm.ApprenticeVacancies.Should().BeEquivalentTo(dummyApprenticeships);
                        });
                    A.CallTo(() => socRepositoryFake.GetApprenticeVacanciesBySocCode(A<string>._)).MustHaveHappened();
                }
            }
            else
            {
                indexMethodCall.ShouldRedirectTo("\\");
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "unused", Justification = "Used in debuuging tests as this number is dumped as part of the parameters - hence no need to copare them all to find the exact instance")]
        [Theory]
        [InlineData(1, "Test", true, "test", false, false)]
        [InlineData(2, "TestInContentAuth", false, "", true, false)]
        [InlineData(3, "Test", true, "", false, false)]
        [InlineData(4, "Test", false, "test", false, false)]
        [InlineData(5, "Test", true, "test", false, true)]
        [InlineData(6, "TestInContentAuth", false, "", true, true)]
        [InlineData(7, "Test", true, "", false, true)]
        [InlineData(8, "Test", false, "test", false, true)]
        public void IndexUrlNameTest(int testIndex, string urlName, bool useValidJobProfile, string socCode, bool inContentAuthoringSite, bool isContentPreviewMode)
        {
            //Setup the fakes and dummies
            var unused = testIndex;
            var repositoryFake = A.Fake<IJobProfileRepository>(ops => ops.Strict());
            var socRepositoryFake = A.Fake<IJobProfileSocCodeRepository>(ops => ops.Strict());
            var loggerFake = A.Fake<IApplicationLogger>();
            var webAppContextFake = A.Fake<IWebAppContext>(ops => ops.Strict());
            var sitefinityPage = A.Fake<ISitefinityPage>(ops => ops.Strict());

            var dummyJobProfileApprenticeshipViewModel = !useValidJobProfile
                ? new JobProfileApprenticeshipViewModel
                {
                    ApprenticeVacancies = new List<ApprenticeVacancy>(),
                    ApprenticeshipSectionTitle = $"dummy {nameof(JobProfileApprenticeshipViewModel.ApprenticeshipSectionTitle)}",
                    LocationDetails = $"dummy {nameof(JobProfileApprenticeshipViewModel.LocationDetails)}",
                    ApprenticeshipText = $"dummy {nameof(JobProfileApprenticeshipViewModel.ApprenticeshipText)}",
                    MainSectionTitle = $"dummy {nameof(JobProfileApprenticeshipViewModel.MainSectionTitle)}",
                    NoVacancyText = $"dummy {nameof(JobProfileApprenticeshipViewModel.NoVacancyText)}"
                }
                : null;

            var dummyJobProfile = useValidJobProfile
                ? new JobProfile
                {
                    AlternativeTitle = $"dummy {nameof(JobProfile.AlternativeTitle)}",

                    SalaryRange = $"dummy {nameof(JobProfile.SalaryRange)}",
                    Overview = $"dummy {nameof(JobProfile.Overview)}",
                    Title = $"dummy {nameof(JobProfile.Title)}",
                    SOCCode = socCode
                }
                : null;

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
            A.CallTo(() => socRepositoryFake.GetApprenticeVacanciesBySocCode(A<string>._)).Returns(dummyApprenticeships);
            A.CallTo(() => webAppContextFake.IsContentAuthoringSite).Returns(inContentAuthoringSite);
            A.CallTo(() => webAppContextFake.SetMetaDescription(A<string>._)).DoesNothing();
            A.CallTo(() => webAppContextFake.IsContentPreviewMode).Returns(isContentPreviewMode);
            A.CallTo(() => repositoryFake.GetByUrlNameForPreview(A<string>._)).Returns(dummyJobProfile);
            A.CallTo(() => sitefinityPage.GetDefaultJobProfileToUse(A<string>._))
                .ReturnsLazily((string defaultProfile) => defaultProfile);

            //Instantiate & Act
            var jobProfileApprenticeshipsController = new JobProfileApprenticeshipsController(repositoryFake, webAppContextFake, socRepositoryFake, loggerFake, sitefinityPage)
            {
                ApprenticeshipText = nameof(JobProfileApprenticeshipsController.ApprenticeshipText),
                NoVacancyText = nameof(JobProfileApprenticeshipsController.NoVacancyText),
                ApprenticeshipLocationDetails = nameof(JobProfileApprenticeshipsController.ApprenticeshipLocationDetails),
                ApprenticeshipSectionTitle = nameof(JobProfileApprenticeshipsController.ApprenticeshipSectionTitle),
                MainSectionTitle = nameof(JobProfileApprenticeshipsController.MainSectionTitle),
                DefaultJobProfileUrlName = nameof(JobProfileApprenticeshipsController.DefaultJobProfileUrlName),
                MaxApprenticeshipCount = 2
            };

            //Act
            var indexWithUrlNameMethodCall = jobProfileApprenticeshipsController.WithCallTo(c => c.Index(urlName));
            if (inContentAuthoringSite && useValidJobProfile)
            {
                indexWithUrlNameMethodCall
                 .ShouldRenderDefaultView()
                 .WithModel<JobProfileApprenticeshipViewModel>(vm =>
                 {
                     vm.ApprenticeVacancies.Should().BeEquivalentTo(dummyApprenticeships);
                     vm.ApprenticeshipSectionTitle.Should().BeEquivalentTo(dummyJobProfileApprenticeshipViewModel.ApprenticeshipSectionTitle);
                     vm.WageTitle.Should().Be(jobProfileApprenticeshipsController.ApprenticeshipWageTitle);
                     vm.LocationDetails.Should().BeEquivalentTo(dummyJobProfileApprenticeshipViewModel.LocationDetails);
                     vm.MainSectionTitle.Should().BeEquivalentTo(dummyJobProfileApprenticeshipViewModel.MainSectionTitle);
                     vm.ApprenticeshipText.Should().BeEquivalentTo(dummyJobProfileApprenticeshipViewModel.ApprenticeshipText);
                     vm.NoVacancyText.Should().BeEquivalentTo(dummyJobProfileApprenticeshipViewModel.NoVacancyText);
                 })
                 .AndNoModelErrors();
            }
            else if (useValidJobProfile)
            {
                //Assert
                indexWithUrlNameMethodCall
                    .ShouldRenderDefaultView()
                    .WithModel<JobProfileApprenticeshipViewModel>(vm =>
                    {
                        vm.MainSectionTitle.Should().BeEquivalentTo(jobProfileApprenticeshipsController
                            .MainSectionTitle);
                        vm.ApprenticeshipText.Should().BeEquivalentTo(jobProfileApprenticeshipsController
                            .ApprenticeshipText);
                        vm.WageTitle.Should().Be(jobProfileApprenticeshipsController.ApprenticeshipWageTitle);
                        vm.LocationDetails.Should().Be(jobProfileApprenticeshipsController
                            .ApprenticeshipLocationDetails);
                        vm.NoVacancyText.Should().Be(jobProfileApprenticeshipsController.NoVacancyText);
                        vm.ApprenticeshipSectionTitle.Should().Be(jobProfileApprenticeshipsController
                            .ApprenticeshipSectionTitle);
                    })
                    .AndNoModelErrors();

                if (!string.IsNullOrWhiteSpace(socCode))
                {
                    indexWithUrlNameMethodCall
                        .ShouldRenderDefaultView()
                        .WithModel<JobProfileApprenticeshipViewModel>(vm =>
                        {
                            vm.ApprenticeVacancies.Count().Should()
                                .BeLessOrEqualTo(jobProfileApprenticeshipsController.MaxApprenticeshipCount);
                        });
                }
            }
            else if (!inContentAuthoringSite)
            {
                indexWithUrlNameMethodCall.ShouldGiveHttpStatus(404);
            }

            if (!string.IsNullOrWhiteSpace(socCode) && useValidJobProfile)
            {
                indexWithUrlNameMethodCall
                    .ShouldRenderDefaultView()
                    .WithModel<JobProfileApprenticeshipViewModel>(vm =>
                    {
                        vm.ApprenticeVacancies.Should().BeEquivalentTo(dummyApprenticeships);
                    })
                    .AndNoModelErrors();
                A.CallTo(() => socRepositoryFake.GetApprenticeVacanciesBySocCode(A<string>._)).MustHaveHappened();
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