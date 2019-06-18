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
            string noVenueAvailableMessage,
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
            string contactAdviserSection)
        {
            // Assign
            var controller = new CourseDetailsController(fakeApplicationLogger, fakeCourseSearchService, fakeAsyncHelper)
            {
                FindAcoursePage = findAcoursePage,
                NoCourseDescriptionMessage = noCourseDescriptionMessage,
                NoEntryRequirementsAvailableMessage = noEntryRequirementsAvailableMessage,
                NoEquipmentRequiredMessage = noEquipmentRequiredMessage,
                NoAssessmentMethodAvailableMessage = noAssessmentMethodAvailableMessage,
                NoVenueAvailableMessage = noVenueAvailableMessage,
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
            };

            A.CallTo(() => fakeCourseSearchService.GetCourseDetailsAsync(courseId, oppurtunityId)).Returns(new CourseDetails());

            // Act
            var controllerResult = controller.WithCallTo(contrl => contrl.Index(
                courseId, oppurtunityId, referralPath));

            // Assert
            controllerResult.ShouldRenderDefaultView().WithModel<CourseDetailsViewModel>(
                 vm =>
                 {
                     vm.FindACoursePage.Should().BeEquivalentTo(controller.FindAcoursePage);
                     vm.NoCourseDescriptionMessage.Should().BeEquivalentTo(controller.NoCourseDescriptionMessage);
                     vm.NoEntryRequirementsAvailableMessage.Should().BeEquivalentTo(controller.NoEntryRequirementsAvailableMessage);
                     vm.NoEquipmentRequiredMessage.Should().BeEquivalentTo(controller.NoEquipmentRequiredMessage);
                     vm.NoAssessmentMethodAvailableMessage.Should().BeEquivalentTo(controller.NoAssessmentMethodAvailableMessage);
                     vm.NoVenueAvailableMessage.Should().BeEquivalentTo(controller.NoVenueAvailableMessage);
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
                 });
            if (string.IsNullOrWhiteSpace(courseId))
            {
                A.CallTo(() => fakeCourseSearchService.GetCourseDetailsAsync(courseId, oppurtunityId)).Should().BeNull();
            }

            A.CallTo(() => fakeCourseSearchService.GetCourseDetailsAsync(courseId, oppurtunityId)).MustHaveHappened();
        }
    }
}
