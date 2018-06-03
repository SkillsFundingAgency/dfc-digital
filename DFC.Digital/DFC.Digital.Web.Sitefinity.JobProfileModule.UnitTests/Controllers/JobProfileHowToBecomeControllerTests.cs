using DFC.Digital.Core;
using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using DFC.Digital.Web.Sitefinity.Core;
using DFC.Digital.Web.Sitefinity.JobProfileModule.Mvc.Models;
using FakeItEasy;
using FluentAssertions;
using TestStack.FluentMVCTesting;
using Xunit;

namespace DFC.Digital.Web.Sitefinity.JobProfileModule.Mvc.Controllers.Tests
{
    public class JobProfileHowToBecomeControllerTests
    {
        private IWebAppContext webAppContextFake;
        private IJobProfileRepository jobProfileRepositoryFake;
        private IApplicationLogger applicationLoggerFake;
        private ISitefinityPage sitefinityPageFake;

        [Theory]
        [InlineData(true, true, false)]
        [InlineData(true, false, false)]
        [InlineData(false, true, true)]
        [InlineData(false, false, true)]
        public void IndexTest(bool validJobProfile, bool inContentAuthoringSite, bool isContentPreviewMode)
        {
            //Assign
            SetupCallsAndFakes(validJobProfile, inContentAuthoringSite, isContentPreviewMode);

            //Act
            var jobprofilehtbController = new JobProfileHowToBecomeController(webAppContextFake, jobProfileRepositoryFake, applicationLoggerFake, sitefinityPageFake);
            var indexMethodCall = jobprofilehtbController.WithCallTo(c => c.Index());

            //Assert
            if (inContentAuthoringSite)
            {
                indexMethodCall
                    .ShouldRenderDefaultView()
                    .WithModel<JobProfileHowToBecomeViewModel>(vm =>
                    {
                        vm.SubsectionApprenticeship.Should().BeEquivalentTo(jobprofilehtbController.SubsectionApprenticeship);
                        vm.SubsectionApprenticeship.Should().BeEquivalentTo(jobprofilehtbController.SubsectionApprenticeship);
                        vm.SubsectionApprenticeship.Should().BeEquivalentTo(jobprofilehtbController.SubsectionApprenticeship);
                        vm.SubsectionApprenticeship.Should().BeEquivalentTo(jobprofilehtbController.SubsectionApprenticeship);
                        vm.SubsectionApprenticeship.Should().BeEquivalentTo(jobprofilehtbController.SubsectionApprenticeship);
                        vm.SubsectionApprenticeship.Should().BeEquivalentTo(jobprofilehtbController.SubsectionApprenticeship);
                    })
                    .AndNoModelErrors();

                if (!isContentPreviewMode)
                {
                    A.CallTo(() => jobProfileRepositoryFake.GetByUrlName(A<string>._)).MustHaveHappened();
                }
                else
                {
                    A.CallTo(() => jobProfileRepositoryFake.GetByUrlNameForPreview(A<string>._)).MustHaveHappened();
                }
            }
            else
            {
                indexMethodCall.ShouldRedirectTo("\\");
            }
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void IndexWithUrlNameTest(bool validJobProfile)
        {
            SetupCallsAndFakes(validJobProfile);
        }

        private void SetupCallsAndFakes(bool isValidJobProfile, bool inContentAuthoringSite = false, bool isContentPreviewMode = false)
        {
            webAppContextFake = A.Fake<IWebAppContext>();
            jobProfileRepositoryFake = A.Fake<IJobProfileRepository>();
            applicationLoggerFake = A.Fake<IApplicationLogger>();
            sitefinityPageFake = A.Fake<ISitefinityPage>();

            A.CallTo(() => webAppContextFake.IsContentAuthoringSite).Returns(inContentAuthoringSite);
            A.CallTo(() => webAppContextFake.IsContentPreviewMode).Returns(isContentPreviewMode);
            A.CallTo(() => jobProfileRepositoryFake.GetByUrlName(A<string>._))
                .Returns(GetDummyJobProfile(isValidJobProfile));
        }

        private JobProfile GetDummyJobProfile(bool useValidJobProfile)
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
                    WorkingHoursPatternsAndEnvironment = nameof(JobProfile.WorkingHoursPatternsAndEnvironment)
                }
                : null;
        }
    }
}