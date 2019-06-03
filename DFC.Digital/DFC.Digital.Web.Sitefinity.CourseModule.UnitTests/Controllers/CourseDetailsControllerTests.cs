using DFC.Digital.Core;
using DFC.Digital.Data.Interfaces;
using DFC.Digital.Web.Sitefinity.CourseModule.Mvc.Controllers;
using FakeItEasy;
using FluentAssertions;
using TestStack.FluentMVCTesting;
using Xunit;

namespace DFC.Digital.Web.Sitefinity.CourseModule.UnitTests
{
    /// <summary>
    /// description CourseDetailsControllerTests.
    /// </summary>
    public class CourseDetailsControllerTests
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
            this.fakeAsyncHelper = A.Fake<IAsyncHelper>();
        }

        [Theory]
        [InlineData("FindAcoursePage", "1", "NoCourseDescriptionMessage", "NoEntryRequirementsAvailableMessage", "NoEquipmentRequiredMessage", "NoAssessmentMethodAvailableMessage", "NoVenueAvailableMessage", "NoOtherDateOrVenueAvailableMessage")]
        public void IndexSetDefaultsTest(string findAcoursePage, string courseId, string noCourseDescriptionMessage, string noEntryRequirementsAvailableMessage, string noEquipmentRequiredMessage, string noAssessmentMethodAvailableMessage, string noVenueAvailableMessage, string noOtherDateOrVenueAvailableMessage)
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
            };

            // Act
            var controllerResult = controller.WithCallTo(contrl => contrl.Index(
                courseId));

            // Assert
            controllerResult.ShouldRenderDefaultView().WithModel<CourseDetailsViewModel>(
                 vm =>
                 {
                     vm.FindACoursePage.Should().BeEquivalentTo(controller.FindAcoursePage);
                 });
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void ValidateReferralUrlTest(bool referralUrlExists)
        {
            var controller = new CourseDetailsController(fakeApplicationLogger, fakeCourseSearchService, fakeAsyncHelper);
            var referralUrl = controller.Request?.UrlReferrer?.PathAndQuery;

            if (referralUrlExists)
            {
                referralUrl = controller.GetBackToResultsUrl();
            }

            // Act
            var controllerResult = controller.WithCallTo(contrl => contrl.Index("1"));

            // Assert
            controllerResult.ShouldRenderDefaultView().WithModel<CourseDetailsViewModel>();
            if (string.IsNullOrWhiteSpace(referralUrl))
            {
                referralUrl.Should().BeEquivalentTo(controller.Request?.UrlReferrer?.PathAndQuery);
            }
            else
            {
                referralUrl.Should().BeEquivalentTo(controller.Request?.UrlReferrer?.PathAndQuery);
            }
        }
    }
}
