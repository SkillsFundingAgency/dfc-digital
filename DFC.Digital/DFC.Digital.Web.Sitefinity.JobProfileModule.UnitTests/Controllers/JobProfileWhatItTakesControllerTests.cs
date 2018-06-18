using DFC.Digital.Core;
using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using DFC.Digital.Web.Sitefinity.Core;
using DFC.Digital.Web.Sitefinity.JobProfileModule.Mvc.Controllers;
using DFC.Digital.Web.Sitefinity.JobProfileModule.Mvc.Models;
using FakeItEasy;
using FluentAssertions;
using System.Collections.Generic;
using TestStack.FluentMVCTesting;
using Xunit;

namespace DFC.Digital.Web.Sitefinity.JobProfileModule.UnitTests
{
    /// <summary>
    /// Job Profile Section Controller tests
    /// </summary>
    public class JobProfileWhatItTakesControllerTests
    {
        /// <summary>
        /// Indexes the test.
        /// </summary>
        /// <param name="inContentAuthoringSite">if set to <c>true</c> [in content authoring site].</param>
        /// <param name="isContentPreviewMode">iscontentpreviewmode</param>
        [Theory]
        [InlineData(true, false)]
        [InlineData(false, false)]
        [InlineData(true, true)]
        [InlineData(false, true)]
        public void IndexTest(bool inContentAuthoringSite, bool isContentPreviewMode)
        {
            //Setup the fakes and dummies
            var repositoryFake = A.Fake<IJobProfileRepository>(ops => ops.Strict());
            var loggerFake = A.Fake<IApplicationLogger>();
            var webAppContextFake = A.Fake<IWebAppContext>(ops => ops.Strict());
            var sitefinityPage = A.Fake<ISitefinityPage>(ops => ops.Strict());

            var dummyJobProfile = GetDummyJobPRofile(true);

            // Set up calls
            A.CallTo(() => repositoryFake.GetByUrlName(A<string>._)).Returns(dummyJobProfile);
            A.CallTo(() => repositoryFake.GetByUrlNameForPreview(A<string>._)).Returns(dummyJobProfile);

            A.CallTo(() => webAppContextFake.IsContentAuthoringSite).Returns(inContentAuthoringSite);
            A.CallTo(() => webAppContextFake.IsContentPreviewMode).Returns(isContentPreviewMode);

            A.CallTo(() => sitefinityPage.GetDefaultJobProfileToUse(A<string>._)).ReturnsLazily((string defaultProfile) => defaultProfile);

            //Instantiate & Act
            var jobProfileWhatItTakesController =
                new JobProfileWhatItTakesController(repositoryFake, webAppContextFake, loggerFake, sitefinityPage);

            //Act
            var indexMethodCall = jobProfileWhatItTakesController.WithCallTo(c => c.Index());

            //Assert
            if (inContentAuthoringSite)
            {
                indexMethodCall
                    .ShouldRenderDefaultView()
                    .WithModel<JobProfileWhatItTakesViewModel>(vm =>
                    {
                        vm.Title.Should().BeEquivalentTo(jobProfileWhatItTakesController.MainSectionTitle);
                        vm.TopSectionContent.Should().BeEquivalentTo(jobProfileWhatItTakesController.TopSectionContent);
                        vm.BottomSectionContent.Should().BeEquivalentTo(jobProfileWhatItTakesController.BottomSectionContent);
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
            }
            else
            {
                indexMethodCall.ShouldRedirectTo("\\");
            }
        }

        /// <summary>
        /// Indexes the URL name test.
        /// </summary>
        /// <param name="urlName">Name of the URL.</param>
        /// <param name="useValidJobProfile">if set to <c>true</c> [valid job profile].</param>
        /// <param name="isContentPreviewMode">iscontentpreviewmode</param>
        [Theory]
        [InlineData("Test", true, false)]
        [InlineData("Test", false, false)]
        [InlineData("Test", true, true)]
        [InlineData("Test", false, true)]
        public void IndexUrlNameTest(string urlName, bool useValidJobProfile, bool isContentPreviewMode)
        {
            //Setup the fakes and dummies
            var repositoryFake = A.Fake<IJobProfileRepository>(ops => ops.Strict());
            var loggerFake = A.Fake<IApplicationLogger>();
            var webAppContextFake = A.Fake<IWebAppContext>(ops => ops.Strict());
            var sitefinityPage = A.Fake<ISitefinityPage>(ops => ops.Strict());

            var dummyJobProfile = GetDummyJobPRofile(useValidJobProfile);

            // Set up calls
            A.CallTo(() => repositoryFake.GetByUrlName(A<string>._)).Returns(dummyJobProfile);
            A.CallTo(() => repositoryFake.GetByUrlNameForPreview(A<string>._)).Returns(dummyJobProfile);
            A.CallTo(() => webAppContextFake.IsContentPreviewMode).Returns(isContentPreviewMode);
            A.CallTo(() => sitefinityPage.GetDefaultJobProfileToUse(A<string>._)).ReturnsLazily((string defaultProfile) => defaultProfile);

            //Instantiate & Act
            var jobProfileWhatItTakesController =
                new JobProfileWhatItTakesController(repositoryFake, webAppContextFake, loggerFake, sitefinityPage);

            //Act
            var indexWithUrlNameMethodCall = jobProfileWhatItTakesController.WithCallTo(c => c.Index(urlName));

            if (useValidJobProfile)
            {
                indexWithUrlNameMethodCall
                    .ShouldRenderDefaultView()
                    .WithModel<JobProfileSectionViewModel>(vm =>
                    {
                        vm.Title.Should().BeEquivalentTo(jobProfileWhatItTakesController.MainSectionTitle);
                        vm.TopSectionContent.Should().BeEquivalentTo(jobProfileWhatItTakesController.TopSectionContent);
                        vm.BottomSectionContent.Should().BeEquivalentTo(jobProfileWhatItTakesController.BottomSectionContent);
                    })
                    .AndNoModelErrors();
            }
            else
            {
                indexWithUrlNameMethodCall
                    .ShouldGiveHttpStatus(404);
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

        [Theory]
        [InlineData("Test", true, true)]
        [InlineData("Test", false, false)]
        public void IndexWithCadHowToBecome(string urlName, bool cadReady, bool expected)
        {
            //Setup the fakes and dummies
            var repositoryFake = A.Fake<IJobProfileRepository>(ops => ops.Strict());
            var loggerFake = A.Fake<IApplicationLogger>();
            var webAppContextFake = A.Fake<IWebAppContext>(ops => ops.Strict());
            var sitefinityPage = A.Fake<ISitefinityPage>(ops => ops.Strict());

            var dummyJobProfile = GetDummyJobPRofile(true);
            dummyJobProfile.HowToBecomeData.IsHTBCaDReady = cadReady;

            // Set up calls
            A.CallTo(() => repositoryFake.GetByUrlName(A<string>._)).Returns(dummyJobProfile);
            A.CallTo(() => repositoryFake.GetByUrlNameForPreview(A<string>._)).Returns(dummyJobProfile);
            A.CallTo(() => webAppContextFake.IsContentPreviewMode).Returns(false);
            A.CallTo(() => sitefinityPage.GetDefaultJobProfileToUse(A<string>._)).ReturnsLazily((string defaultProfile) => defaultProfile);

            //Instantiate & Act
            var jobProfileWhatItTakesController =
                new JobProfileWhatItTakesController(repositoryFake, webAppContextFake, loggerFake, sitefinityPage);

            //Act
            var indexWithUrlNameMethodCall = jobProfileWhatItTakesController.WithCallTo(c => c.Index(urlName));

            indexWithUrlNameMethodCall
                .ShouldRenderDefaultView()
                .WithModel<JobProfileWhatItTakesViewModel>(vm =>
                {
                    vm.Title.Should().BeEquivalentTo(jobProfileWhatItTakesController.MainSectionTitle);
                    vm.TopSectionContent.Should().BeEquivalentTo(jobProfileWhatItTakesController.TopSectionContent);
                    vm.BottomSectionContent.Should().BeEquivalentTo(jobProfileWhatItTakesController.BottomSectionContent);
                    vm.IsWhatItTakesCadView.Should().Be(expected);
                    if (expected)
                    {
                        vm.RestrictionsOtherRequirements.SectionIntro.Should()
                            .BeEquivalentTo(jobProfileWhatItTakesController.RestrictionsIntro);
                        vm.RestrictionsOtherRequirements.SectionTitle.Should()
                            .BeEquivalentTo(jobProfileWhatItTakesController.RestrictionsTitle);
                        vm.RestrictionsOtherRequirements.Restrictions.Should()
                            .BeEquivalentTo(dummyJobProfile.Restrictions);
                        vm.RestrictionsOtherRequirements.OtherRequirements.Should()
                            .BeEquivalentTo(dummyJobProfile.OtherRequirements);
                    }
                })
                .AndNoModelErrors();
        }

        private JobProfile GetDummyJobPRofile(bool useValidJobProfile)
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
                       WorkingHoursPatternsAndEnvironment = nameof(JobProfile.WorkingHoursPatternsAndEnvironment),
                       HowToBecomeData = new HowToBecome(),
                       Restrictions = new List<Restriction> { new Restriction { Info = nameof(Restriction.Info), Title = nameof(Restriction.Title) } },
                       OtherRequirements = nameof(JobProfile.OtherRequirements)
                   }
                   : null;
        }
    }
}