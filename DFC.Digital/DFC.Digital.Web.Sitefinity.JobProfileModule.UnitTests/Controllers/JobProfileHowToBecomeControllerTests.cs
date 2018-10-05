using AutoMapper;
using DFC.Digital.Core;
using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using DFC.Digital.Web.Sitefinity.Core;
using DFC.Digital.Web.Sitefinity.JobProfileModule.Mvc.Controllers;
using DFC.Digital.Web.Sitefinity.JobProfileModule.Mvc.Models;
using FakeItEasy;
using FluentAssertions;
using System.Collections.Generic;
using System.Linq;
using TestStack.FluentMVCTesting;
using Xunit;

namespace DFC.Digital.Web.Sitefinity.JobProfileModule.UnitTests
{
    public class JobProfileHowToBecomeControllerTests
    {
        private IWebAppContext webAppContextFake;
        private IJobProfileRepository jobProfileRepositoryFake;
        private IApplicationLogger applicationLoggerFake;
        private ISitefinityPage sitefinityPageFake;
        private IMapper mapper;

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
            var jobprofilehtbController = new JobProfileHowToBecomeController(webAppContextFake, jobProfileRepositoryFake, applicationLoggerFake, sitefinityPageFake, mapper);
            var indexMethodCall = jobprofilehtbController.WithCallTo(c => c.Index());

            //Assert
            if (inContentAuthoringSite)
            {
                indexMethodCall
                    .ShouldRenderDefaultView()
                    .WithModel<JobProfileHowToBecomeViewModel>(vm =>
                    {
                        AssertViewModelProperties(vm, jobprofilehtbController);
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
        [InlineData("test", true)]
        [InlineData("test", false)]
        public void IndexWithUrlNameTest(string urlName, bool validJobProfile)
        {
            SetupCallsAndFakes(validJobProfile);

            //Act
            var jobprofilehtbController = new JobProfileHowToBecomeController(webAppContextFake, jobProfileRepositoryFake, applicationLoggerFake, sitefinityPageFake, mapper);
            var indexWithUrlNameMethodCall = jobprofilehtbController.WithCallTo(c => c.Index(urlName));

            //Assert
            if (validJobProfile)
            {
                indexWithUrlNameMethodCall
                    .ShouldRenderDefaultView()
                    .WithModel<JobProfileHowToBecomeViewModel>(vm =>
                    {
                        AssertViewModelProperties(vm, jobprofilehtbController);
                    })
                    .AndNoModelErrors();
                    A.CallTo(() => jobProfileRepositoryFake.GetByUrlName(A<string>._)).MustHaveHappened();
            }
            else
            {
                indexWithUrlNameMethodCall.ShouldGiveHttpStatus(404);
            }
        }

        private static void AssertViewModelProperties(JobProfileHowToBecomeViewModel vm, JobProfileHowToBecomeController jobprofilehtbController)
        {
            vm.SubsectionApprenticeship.Should().BeEquivalentTo(jobprofilehtbController.SubsectionApprenticeship);
            vm.MainSectionTitle.Should().BeEquivalentTo(jobprofilehtbController.MainSectionTitle);
            vm.SectionId.Should().BeEquivalentTo(jobprofilehtbController.SectionId);
            vm.SubsectionUniversity.Should().BeEquivalentTo(jobprofilehtbController.SubsectionUniversity);
            vm.SubsectionUniversityMoreInformation.Should().BeEquivalentTo(jobprofilehtbController.SubsectionUniversityMoreInformation);
            vm.SubsectionCollege.Should().BeEquivalentTo(jobprofilehtbController.SubsectionCollege);
            vm.SubsectionCollegeRequirements.Should().BeEquivalentTo(jobprofilehtbController.SubsectionCollegeRequirements);
            vm.SubsectionCollegeMoreInformation.Should().BeEquivalentTo(jobprofilehtbController.SubsectionCollegeMoreInformation);
            vm.SubsectionApprenticeship.Should().BeEquivalentTo(jobprofilehtbController.SubsectionApprenticeship);
            vm.SubsectionApprenticeshipRequirements.Should().BeEquivalentTo(jobprofilehtbController.SubsectionApprenticeshipRequirements);
            vm.SubsectionApprenticeshipMoreInformation.Should().BeEquivalentTo(jobprofilehtbController.SubsectionApprenticeshipMoreInformation);
            vm.SubsectionWork.Should().BeEquivalentTo(jobprofilehtbController.SubsectionWork);
            vm.SubsectionVolunteering.Should().BeEquivalentTo(jobprofilehtbController.SubsectionVolunteering);
            vm.SubsectionDirectApplication.Should().BeEquivalentTo(jobprofilehtbController.SubsectionDirectApplication);
            vm.SubsectionOtherRoutes.Should().BeEquivalentTo(jobprofilehtbController.SubsectionOtherRoutes);
            vm.SubsectionMoreInfo.Should().BeEquivalentTo(jobprofilehtbController.SubsectionMoreInfo);
            vm.SubsectionMoreInfoRegistration.Should().BeEquivalentTo(jobprofilehtbController.SubsectionMoreInfoRegistration);
            vm.SubsectionMoreInfoRegistrationOpeningText.Should().BeEquivalentTo(jobprofilehtbController.SubsectionMoreInfoRegistrationOpeningText);
            vm.SubsectionMoreInfoBodies.Should().BeEquivalentTo(jobprofilehtbController.SubsectionMoreInfoBodies);
            vm.SubsectionMoreInfoTips.Should().BeEquivalentTo(jobprofilehtbController.SubsectionMoreInfoTips);
            vm.SubsectionMoreInfoFurtherInfo.Should().BeEquivalentTo(jobprofilehtbController.SubsectionMoreInfoFurtherInfo);
        }

        private void SetupCallsAndFakes(bool isValidJobProfile, bool inContentAuthoringSite = false, bool isContentPreviewMode = false)
        {
            webAppContextFake = A.Fake<IWebAppContext>(ops => ops.Strict());
            jobProfileRepositoryFake = A.Fake<IJobProfileRepository>(ops => ops.Strict());
            applicationLoggerFake = A.Fake<IApplicationLogger>();
            sitefinityPageFake = A.Fake<ISitefinityPage>(ops => ops.Strict());
            mapper = new MapperConfiguration(c => c.AddProfile<JobProfilesAutoMapperProfile>()).CreateMapper();

            A.CallTo(() => webAppContextFake.IsContentAuthoringSite).Returns(inContentAuthoringSite);
            A.CallTo(() => webAppContextFake.IsContentPreviewMode).Returns(isContentPreviewMode);
            A.CallTo(() => webAppContextFake.MetaDescription(A<string>._)).DoesNothing();
            A.CallTo(() => jobProfileRepositoryFake.GetByUrlName(A<string>._))
                .Returns(GetDummyJobProfile(isValidJobProfile));
            A.CallTo(() => jobProfileRepositoryFake.GetByUrlNameForPreview(A<string>._))
                .Returns(GetDummyJobProfile(isValidJobProfile));
            A.CallTo(() => sitefinityPageFake.GetDefaultJobProfileToUse(A<string>._))
                .Returns(nameof(JobProfile.UrlName));
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
                    WorkingHoursPatternsAndEnvironment = nameof(JobProfile.WorkingHoursPatternsAndEnvironment),
                    HowToBecomeData = new HowToBecome()
                }
                : null;
        }
    }
}