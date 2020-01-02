using DFC.Digital.Core;
using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using DFC.Digital.Web.Sitefinity.CourseModule.Mvc.Controllers;
using DFC.Digital.Web.Sitefinity.CourseModule.UnitTests.Helpers;
using FakeItEasy;
using FluentAssertions;
using System.Collections.Specialized;
using TestStack.FluentMVCTesting;
using Xunit;

namespace DFC.Digital.Web.Sitefinity.CourseModule.UnitTests
{
    /// <summary>
    /// description CourseDetailsControllerTests.
    /// </summary>
    public class CourseDetailsControllerTests : MemberDataHelper
    {
        private readonly IApplicationLogger fakeApplicationLogger;
        private readonly ICourseSearchService fakeCourseSearchService;
        private readonly IAsyncHelper fakeAsyncHelper;

        /// <summary>
        /// Initializes a new instance of the <see cref="CourseDetailsControllerTests"/> class.
        /// </summary>
        public CourseDetailsControllerTests()
        {
            this.fakeApplicationLogger = A.Fake<IApplicationLogger>(ops => ops.Strict());
            this.fakeCourseSearchService = A.Fake<ICourseSearchService>();
            this.fakeAsyncHelper = new AsyncHelper();
        }

        [Theory]
        [MemberData(nameof(CourseDetailsIndexDefaultTestsInput))]
        public void IndexSetDefaultsTest(
            string findAcoursePage,
            string courseId,
            string oppurtunityId,
            string noCourseDescriptionMessage,
            string noEntryRequirementsAvailableMessage,
            string noEquipmentRequiredMessage,
            string noAssessmentMethodAvailableMessage,
            string noOtherDateOrVenueAvailableMessage,
            string courseDetailsPage,
            string qualificationDetailsLabel,
            string courseDescriptionLabel,
            string entryRequirementsLabel,
            string equipmentRequiredLabel,
            string assessmentMethodLabel,
            string venueLabel,
            string otherDatesAndVenuesLabel,
            string providerLabel,
            string employerSatisfactionLabel,
            string learnerSatisfactionLabel,
            string providerPerformanceLabel,
            string referralPath,
            string contactAdviserSection,
            string costDescriptionLabel,
            string attendancePatternLabel,
            string awardingOrganisationLabel,
            string courseTypeLabel,
            string courseWebpageLinkLabel,
            string qualificationLevelLabel,
            string qualificationNameLabel,
            string startDateLabel,
            string subjectCategoryLabel,
            bool hasCourseId)
        {
            // Assign
            var controller = new CourseDetailsController(fakeApplicationLogger, fakeCourseSearchService, fakeAsyncHelper)
            {
                FindAcoursePage = findAcoursePage,
                NoCourseDescriptionMessage = noCourseDescriptionMessage,
                NoEntryRequirementsAvailableMessage = noEntryRequirementsAvailableMessage,
                NoEquipmentRequiredMessage = noEquipmentRequiredMessage,
                NoAssessmentMethodAvailableMessage = noAssessmentMethodAvailableMessage,
                NoOtherDateOrVenueAvailableMessage = noOtherDateOrVenueAvailableMessage,
                CourseDetailsPage = courseDetailsPage,
                QualificationDetailsLabel = qualificationDetailsLabel,
                CourseDescriptionLabel = courseDescriptionLabel,
                EntryRequirementsLabel = entryRequirementsLabel,
                EquipmentRequiredLabel = equipmentRequiredLabel,
                AssessmentMethodLabel = assessmentMethodLabel,
                VenueLabel = venueLabel,
                OtherDatesAndVenuesLabel = otherDatesAndVenuesLabel,
                ProviderLabel = providerLabel,
                EmployerSatisfactionLabel = employerSatisfactionLabel,
                LearnerSatisfactionLabel = learnerSatisfactionLabel,
                ProviderPerformanceLabel = providerPerformanceLabel,
                ContactAdviserSection = contactAdviserSection,
                AttendancePatternLabel = attendancePatternLabel,
                AwardingOrganisationLabel = awardingOrganisationLabel,
                CostDescriptionLabel = costDescriptionLabel,
                CourseTypeLabel = courseTypeLabel,
                CourseWebpageLinkLabel = courseWebpageLinkLabel,
                QualificationLevelLabel = qualificationLevelLabel,
                QualificationNameLabel = qualificationNameLabel,
                StartDateLabel = startDateLabel,
                SubjectCategoryLabel = subjectCategoryLabel
            };
            if (hasCourseId)
            {
                A.CallTo(() => fakeCourseSearchService.GetCourseDetailsAsync(courseId, oppurtunityId)).Returns(new CourseDetails());
            }
            else
            {
                A.CallTo(() => fakeCourseSearchService.GetCourseDetailsAsync(courseId, oppurtunityId)).Returns<CourseDetails>(null);
            }

            // Act
            var controllerResult = controller.WithCallTo(contrl => contrl.Index(
                courseId, oppurtunityId, referralPath));

            // Assert
            if (hasCourseId)
            {
                controllerResult.ShouldRenderDefaultView().WithModel<CourseDetailsViewModel>(
                vm =>
                {
                    vm.FindACoursePage.Should().BeEquivalentTo(controller.FindAcoursePage);
                    vm.NoCourseDescriptionMessage.Should().BeEquivalentTo(controller.NoCourseDescriptionMessage);
                    vm.NoEntryRequirementsAvailableMessage.Should().BeEquivalentTo(controller.NoEntryRequirementsAvailableMessage);
                    vm.NoEquipmentRequiredMessage.Should().BeEquivalentTo(controller.NoEquipmentRequiredMessage);
                    vm.NoAssessmentMethodAvailableMessage.Should().BeEquivalentTo(controller.NoAssessmentMethodAvailableMessage);
                    vm.NoOtherDateOrVenueAvailableMessage.Should().BeEquivalentTo(controller.NoOtherDateOrVenueAvailableMessage);
                    vm.CourseDetailsPage.Should().BeEquivalentTo(controller.CourseDetailsPage);
                    vm.QualificationDetailsLabel.Should().BeEquivalentTo(controller.QualificationDetailsLabel);
                    vm.CourseDescriptionLabel.Should().BeEquivalentTo(controller.CourseDescriptionLabel);
                    vm.EntryRequirementsLabel.Should().BeEquivalentTo(controller.EntryRequirementsLabel);
                    vm.EquipmentRequiredLabel.Should().BeEquivalentTo(controller.EquipmentRequiredLabel);
                    vm.AssessmentMethodLabel.Should().BeEquivalentTo(controller.AssessmentMethodLabel);
                    vm.VenueLabel.Should().BeEquivalentTo(controller.VenueLabel);
                    vm.OtherDatesAndVenuesLabel.Should().BeEquivalentTo(controller.OtherDatesAndVenuesLabel);
                    vm.ProviderLabel.Should().BeEquivalentTo(controller.ProviderLabel);
                    vm.EmployerSatisfactionLabel.Should().BeEquivalentTo(controller.EmployerSatisfactionLabel);
                    vm.LearnerSatisfactionLabel.Should().BeEquivalentTo(controller.LearnerSatisfactionLabel);
                    vm.ProviderPerformanceLabel.Should().BeEquivalentTo(controller.ProviderPerformanceLabel);
                    vm.CostDescriptionLabel.Should().BeEquivalentTo(controller.CostDescriptionLabel);
                    vm.AttendancePatternLabel.Should().BeEquivalentTo(controller.AttendancePatternLabel);
                    vm.AwardingOrganisationLabel.Should().BeEquivalentTo(controller.AwardingOrganisationLabel);
                    vm.CourseTypeLabel.Should().BeEquivalentTo(controller.CourseTypeLabel);
                    vm.CourseWebpageLinkLabel.Should().BeEquivalentTo(controller.CourseWebpageLinkLabel);
                    vm.QualificationLevelLabel.Should().BeEquivalentTo(controller.QualificationLevelLabel);
                    vm.QualificationNameLabel.Should().BeEquivalentTo(controller.QualificationNameLabel);
                    vm.StartDateLabel.Should().BeEquivalentTo(controller.StartDateLabel);
                    vm.SubjectCategoryLabel.Should().BeEquivalentTo(controller.SubjectCategoryLabel);
                });

                A.CallTo(() => fakeCourseSearchService.GetCourseDetailsAsync(courseId, oppurtunityId)).MustHaveHappened();
            }
            else
            {
                controllerResult.ShouldGiveHttpStatus(404);
            }
        }
    }
}
