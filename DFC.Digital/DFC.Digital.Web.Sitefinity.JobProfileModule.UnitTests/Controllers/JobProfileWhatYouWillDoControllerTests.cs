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
    public class JobProfileWhatYouWillDoControllerTests
    {
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
            var formatContentServiceFake = A.Fake<IFormatContentService>(ops => ops.Strict());

            var dummyJobProfile = GetDummyJobPRofile(true);

            // Set up calls
            A.CallTo(() => repositoryFake.GetByUrlName(A<string>._)).Returns(dummyJobProfile);
            A.CallTo(() => repositoryFake.GetByUrlNameForPreview(A<string>._)).Returns(dummyJobProfile);

            A.CallTo(() => webAppContextFake.IsContentAuthoringSite).Returns(inContentAuthoringSite);
            A.CallTo(() => webAppContextFake.IsContentPreviewMode).Returns(isContentPreviewMode);

            A.CallTo(() => sitefinityPage.GetDefaultJobProfileToUse(A<string>._)).ReturnsLazily((string defaultProfile) => defaultProfile);
            A.CallTo(() => formatContentServiceFake.GetParagraphText(A<string>._, A<IEnumerable<string>>._, A<string>._)).Returns("test");

            //Instantiate & Act
            using (var jobProfileWhatYouWillDoController =
                new JobProfileWhatYouWillDoController(repositoryFake, webAppContextFake, loggerFake, sitefinityPage, formatContentServiceFake))
            {
                //Act
                var indexMethodCall = jobProfileWhatYouWillDoController.WithCallTo(c => c.Index());

                //Assert
                if (inContentAuthoringSite)
                {
                    indexMethodCall
                        .ShouldRenderDefaultView()
                        .WithModel<JobProfileWhatYouWillDoViewModel>(vm =>
                          {
                            vm.Title.Should().BeEquivalentTo(jobProfileWhatYouWillDoController.MainSectionTitle);
                            vm.TopSectionContent.Should().BeEquivalentTo(jobProfileWhatYouWillDoController.TopSectionContent);
                            vm.BottomSectionContent.Should().BeEquivalentTo(jobProfileWhatYouWillDoController.BottomSectionContent);
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
            using (var jobProfileWhatYouWillDoController =
                new JobProfileWhatYouWillDoController(repositoryFake, webAppContextFake, loggerFake, sitefinityPage, formatContentServiceFake))
            {
                //Act
                var indexWithUrlNameMethodCall = jobProfileWhatYouWillDoController.WithCallTo(c => c.Index(urlName));

                if (useValidJobProfile)
                {
                    indexWithUrlNameMethodCall
                        .ShouldRenderDefaultView()
                        .WithModel<JobProfileSectionViewModel>(vm =>
                          {
                            vm.Title.Should().BeEquivalentTo(jobProfileWhatYouWillDoController.MainSectionTitle);
                            vm.TopSectionContent.Should().BeEquivalentTo(jobProfileWhatYouWillDoController.TopSectionContent);
                            vm.BottomSectionContent.Should().BeEquivalentTo(jobProfileWhatYouWillDoController.BottomSectionContent);
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
        }

        [Theory]
        [InlineData("Test", false, false)]
        [InlineData("Test", true, true)]
        public void IndexWithCadWhatYouWillDo(string urlName, bool cadReady, bool expected)
        {
            //Setup the fakes and dummies
            var repositoryFake = A.Fake<IJobProfileRepository>(ops => ops.Strict());
            var loggerFake = A.Fake<IApplicationLogger>();
            var webAppContextFake = A.Fake<IWebAppContext>(ops => ops.Strict());
            var sitefinityPage = A.Fake<ISitefinityPage>(ops => ops.Strict());
            var formatContentServiceFake = A.Fake<IFormatContentService>(ops => ops.Strict());

            var dummyJobProfile = GetDummyJobPRofile(true);
            dummyJobProfile.WhatYouWillDoData.IsCadReady = cadReady;

            // Set up calls
            A.CallTo(() => repositoryFake.GetByUrlName(A<string>._)).Returns(dummyJobProfile);
            A.CallTo(() => repositoryFake.GetByUrlNameForPreview(A<string>._)).Returns(dummyJobProfile);
            A.CallTo(() => webAppContextFake.IsContentPreviewMode).Returns(false);
            A.CallTo(() => webAppContextFake.SetMetaDescription(A<string>._)).DoesNothing();
            A.CallTo(() => sitefinityPage.GetDefaultJobProfileToUse(A<string>._)).ReturnsLazily((string defaultProfile) => defaultProfile);
            A.CallTo(() => formatContentServiceFake.GetParagraphText(A<string>._, A<IEnumerable<string>>._, A<string>._)).Returns("test");

            //Instantiate & Act
            using (var jobProfileWhatYouWillDoController =
                new JobProfileWhatYouWillDoController(repositoryFake, webAppContextFake, loggerFake, sitefinityPage, formatContentServiceFake)
                {
                    TopSectionContent = nameof(JobProfileWhatYouWillDoController.TopSectionContent),
                    BottomSectionContent = nameof(JobProfileWhatYouWillDoController.BottomSectionContent),
                    MainSectionTitle = nameof(JobProfileWhatYouWillDoController.MainSectionTitle),
                    SectionId = nameof(JobProfileWhatYouWillDoController.SectionId),
                    WhatYouWillDoSectionTitle = nameof(JobProfileWhatYouWillDoController.WhatYouWillDoSectionTitle),
                    EnvironmentTitle = nameof(JobProfileWhatYouWillDoController.EnvironmentTitle),
                    IsWYDIntroActive = true
                })
            {
                //Act
                var indexWithUrlNameMethodCall = jobProfileWhatYouWillDoController.WithCallTo(c => c.Index(urlName));
                indexWithUrlNameMethodCall
                    .ShouldRenderDefaultView()
                    .WithModel<JobProfileWhatYouWillDoViewModel>(vm =>
                    {
                        vm.Title.Should().BeEquivalentTo(jobProfileWhatYouWillDoController.MainSectionTitle);
                        vm.TopSectionContent.Should().BeEquivalentTo(jobProfileWhatYouWillDoController.TopSectionContent);
                        vm.BottomSectionContent.Should().BeEquivalentTo(jobProfileWhatYouWillDoController.BottomSectionContent);
                        vm.IsWhatYouWillDoCadView.Should().Be(expected);
                        if (expected)
                        {
                            vm.DailyTasksSectionTitle.Should()
                                .BeEquivalentTo(jobProfileWhatYouWillDoController.WhatYouWillDoSectionTitle);
                        }
                    })
                    .AndNoModelErrors();
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
                       OtherRequirements = nameof(JobProfile.OtherRequirements)
                   }
                   : null;
        }
    }
}