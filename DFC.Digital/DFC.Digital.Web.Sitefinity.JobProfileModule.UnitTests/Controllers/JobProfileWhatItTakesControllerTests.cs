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
            var skillsRepositoryFake = A.Fake<IJobProfileRelatedSkillsRepository>(ops => ops.Strict());
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
                new JobProfileWhatItTakesController(repositoryFake, webAppContextFake, loggerFake, sitefinityPage, skillsRepositoryFake);

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
                        vm.JobProfileWhatItTakesSkillsViewModel.TopSectionContent.Should().BeEquivalentTo(jobProfileWhatItTakesController.TopSectionContent);
                        vm.JobProfileWhatItTakesSkillsViewModel.BottomSectionContent.Should().BeEquivalentTo(jobProfileWhatItTakesController.BottomSectionContent);
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
            var skillsRepositoryFake = A.Fake<IJobProfileRelatedSkillsRepository>(ops => ops.Strict());
            var loggerFake = A.Fake<IApplicationLogger>();
            var webAppContextFake = A.Fake<IWebAppContext>(ops => ops.Strict());
            var sitefinityPage = A.Fake<ISitefinityPage>(ops => ops.Strict());

            var dummyJobProfile = GetDummyJobPRofile(useValidJobProfile);

            // Set up calls
            A.CallTo(() => repositoryFake.GetByUrlName(A<string>._)).Returns(dummyJobProfile);
            A.CallTo(() => repositoryFake.GetByUrlNameForPreview(A<string>._)).Returns(dummyJobProfile);
            A.CallTo(() => webAppContextFake.IsContentPreviewMode).Returns(isContentPreviewMode);
            A.CallTo(() => webAppContextFake.IsContentAuthoringSite).Returns(isContentPreviewMode);
            A.CallTo(() => sitefinityPage.GetDefaultJobProfileToUse(A<string>._)).ReturnsLazily((string defaultProfile) => defaultProfile);

            //Instantiate & Act
            var jobProfileWhatItTakesController =
                new JobProfileWhatItTakesController(repositoryFake, webAppContextFake, loggerFake, sitefinityPage, skillsRepositoryFake);

            //Act
            var indexWithUrlNameMethodCall = jobProfileWhatItTakesController.WithCallTo(c => c.Index(urlName));

            if (useValidJobProfile)
            {
                indexWithUrlNameMethodCall
                    .ShouldRenderDefaultView()
                    .WithModel<JobProfileWhatItTakesViewModel>(vm =>
                    {
                        vm.Title.Should().BeEquivalentTo(jobProfileWhatItTakesController.MainSectionTitle);
                        vm.JobProfileWhatItTakesSkillsViewModel.TopSectionContent.Should().BeEquivalentTo(jobProfileWhatItTakesController.TopSectionContent);
                        vm.JobProfileWhatItTakesSkillsViewModel.BottomSectionContent.Should().BeEquivalentTo(jobProfileWhatItTakesController.BottomSectionContent);
                        vm.JobProfileWhatItTakesSkillsViewModel.BottomSectionContent.Should().BeEquivalentTo(jobProfileWhatItTakesController.BottomSectionContent);
                        vm.JobProfileWhatItTakesSkillsViewModel.DigitalSkillsLevel.Should().BeEquivalentTo(nameof(JobProfile.DigitalSkillsLevel));
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
            var skillsRepositoryFake = A.Fake<IJobProfileRelatedSkillsRepository>(ops => ops.Strict());
            var loggerFake = A.Fake<IApplicationLogger>();
            var webAppContextFake = A.Fake<IWebAppContext>(ops => ops.Strict());
            var sitefinityPage = A.Fake<ISitefinityPage>(ops => ops.Strict());

            var dummyJobProfile = GetDummyJobPRofile(true);
            dummyJobProfile.HowToBecomeData.IsHTBCaDReady = cadReady;

            // Set up calls
            A.CallTo(() => repositoryFake.GetByUrlName(A<string>._)).Returns(dummyJobProfile);
            A.CallTo(() => repositoryFake.GetByUrlNameForPreview(A<string>._)).Returns(dummyJobProfile);
            A.CallTo(() => webAppContextFake.IsContentPreviewMode).Returns(false);
            A.CallTo(() => webAppContextFake.IsContentAuthoringSite).Returns(false);
            A.CallTo(() => sitefinityPage.GetDefaultJobProfileToUse(A<string>._)).ReturnsLazily((string defaultProfile) => defaultProfile);

            //Instantiate & Act
            var jobProfileWhatItTakesController =
                new JobProfileWhatItTakesController(repositoryFake, webAppContextFake, loggerFake, sitefinityPage, skillsRepositoryFake);

            //Act
            var indexWithUrlNameMethodCall = jobProfileWhatItTakesController.WithCallTo(c => c.Index(urlName));

            indexWithUrlNameMethodCall
                .ShouldRenderDefaultView()
                .WithModel<JobProfileWhatItTakesViewModel>(vm =>
                {
                    vm.Title.Should().BeEquivalentTo(jobProfileWhatItTakesController.MainSectionTitle);
                    vm.JobProfileWhatItTakesSkillsViewModel.TopSectionContent.Should().BeEquivalentTo(jobProfileWhatItTakesController.TopSectionContent);
                    vm.JobProfileWhatItTakesSkillsViewModel.BottomSectionContent.Should().BeEquivalentTo(jobProfileWhatItTakesController.BottomSectionContent);
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

        [Theory]
        [InlineData(true, true, true)]
        [InlineData(true, false, true)]
        [InlineData(false, true, true)]
        [InlineData(false, false, true)]
        [InlineData(true, true, false)]
        [InlineData(true, false, false)]
        [InlineData(false, true, false)]
        [InlineData(false, false, false)]
        public void IndexWithONetData(bool useONetDataCitizenFacing, bool useONetDataInPreview, bool isContentPreviewMode)
        {
            var repositoryFake = A.Fake<IJobProfileRepository>(ops => ops.Strict());
            var skillsRepositoryFake = A.Fake<IJobProfileRelatedSkillsRepository>(ops => ops.Strict());
            var loggerFake = A.Fake<IApplicationLogger>();
            var webAppContextFake = A.Fake<IWebAppContext>(ops => ops.Strict());
            var sitefinityPage = A.Fake<ISitefinityPage>(ops => ops.Strict());

            var dummyJobProfile = GetDummyJobPRofile(true);

            dummyJobProfile.RelatedSkills = new List<string> { "DummyRelated1", "DummyRelated2" };

            // Set up calls
            A.CallTo(() => repositoryFake.GetByUrlName(A<string>._)).Returns(dummyJobProfile);
            A.CallTo(() => repositoryFake.GetByUrlNameForPreview(A<string>._)).Returns(dummyJobProfile);

            //These two are linked you cannot be in content preview mode with out being in editor mode.
            A.CallTo(() => webAppContextFake.IsContentPreviewMode).Returns(isContentPreviewMode);
            A.CallTo(() => webAppContextFake.IsContentAuthoringSite).Returns(isContentPreviewMode);

            A.CallTo(() => sitefinityPage.GetDefaultJobProfileToUse(A<string>._)).ReturnsLazily((string defaultProfile) => defaultProfile);
            A.CallTo(() => skillsRepositoryFake.GetContextualisedSkillsById(A<IEnumerable<string>>._)).Returns(new List<WhatItTakesSkill> { new WhatItTakesSkill() });

            //Instantiate & Act
            var jobProfileWhatItTakesController =
                new JobProfileWhatItTakesController(repositoryFake, webAppContextFake, loggerFake, sitefinityPage, skillsRepositoryFake);

            jobProfileWhatItTakesController.UseONetDataCitizenFacing = useONetDataCitizenFacing;
            jobProfileWhatItTakesController.UseONetDataInPreview = useONetDataInPreview;
            jobProfileWhatItTakesController.NumberONetSkillsToDisplay = 1;

            //Act
            var indexWithUrlNameMethodCall = jobProfileWhatItTakesController.WithCallTo(c => c.Index("Test"));

            indexWithUrlNameMethodCall
                .ShouldRenderDefaultView()
                .WithModel<JobProfileWhatItTakesViewModel>(vm =>
                {
                    if (isContentPreviewMode)
                    {
                        if (useONetDataInPreview)
                        {
                            vm.JobProfileWhatItTakesSkillsViewModel.UseONetData.Should().BeTrue();
                            vm.JobProfileWhatItTakesSkillsViewModel.WhatItTakesSkills.Should().HaveCount(1);
                        }
                        else
                        {
                            vm.JobProfileWhatItTakesSkillsViewModel.UseONetData.Should().BeFalse();
                            vm.JobProfileWhatItTakesSkillsViewModel.WhatItTakesSkills.Should().BeNullOrEmpty();
                        }
                    }
                    else
                    {
                        if (useONetDataCitizenFacing)
                        {
                            vm.JobProfileWhatItTakesSkillsViewModel.UseONetData.Should().BeTrue();
                            vm.JobProfileWhatItTakesSkillsViewModel.WhatItTakesSkills.Should().HaveCount(1);
                        }
                        else
                        {
                            vm.JobProfileWhatItTakesSkillsViewModel.UseONetData.Should().BeFalse();
                            vm.JobProfileWhatItTakesSkillsViewModel.WhatItTakesSkills.Should().BeNullOrEmpty();
                        }
                    }
                }).AndNoModelErrors();
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
                       DigitalSkillsLevel = nameof(JobProfile.DigitalSkillsLevel),
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
