using DFC.Digital.Core;
using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using DFC.Digital.Web.Sitefinity.CourseModule.Mvc.Controllers;
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
    public class CourseDetailsControllerTests
    {
        private readonly IApplicationLogger fakeApplicationLogger;
        private readonly ICourseSearchService fakeCourseSearchService;
        private readonly IAsyncHelper fakeAsyncHelper;
        private readonly IWebAppContext webAppContextFake = A.Fake<IWebAppContext>();

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
        [InlineData("FindAcoursePage", "1", "NoCourseDescriptionMessage", "NoEntryRequirementsAvailableMessage", "NoEquipmentRequiredMessage", "NoAssessmentMethodAvailableMessage", "NoVenueAvailableMessage", "NoOtherDateOrVenueAvailableMessage", "course-search-url")]
        public void IndexSetDefaultsTest(string findAcoursePage, string courseId, string noCourseDescriptionMessage, string noEntryRequirementsAvailableMessage, string noEquipmentRequiredMessage, string noAssessmentMethodAvailableMessage, string noVenueAvailableMessage, string noOtherDateOrVenueAvailableMessage, string referralUrl)
        {
            // Assign
            var controller = new CourseDetailsController(webAppContextFake, fakeApplicationLogger, fakeCourseSearchService, fakeAsyncHelper)
            {
                FindAcoursePage = findAcoursePage,
                NoCourseDescriptionMessage = noCourseDescriptionMessage,
                NoEntryRequirementsAvailableMessage = noEntryRequirementsAvailableMessage,
                NoEquipmentRequiredMessage = noEquipmentRequiredMessage,
                NoAssessmentMethodAvailableMessage = noAssessmentMethodAvailableMessage,
                NoVenueAvailableMessage = noVenueAvailableMessage,
                NoOtherDateOrVenueAvailableMessage = noOtherDateOrVenueAvailableMessage,
            };

            A.CallTo(() => fakeCourseSearchService.GetCourseDetailsAsync(A<string>._, A<string>._)).Returns(new CourseDetails());

            A.CallTo(() => webAppContextFake.RequestQueryString).Returns(new NameValueCollection { { "referralUrl", referralUrl } });

            // Act
            var controllerResult = controller.WithCallTo(contrl => contrl.Index(
                courseId));

            // Assert
            controllerResult.ShouldRenderDefaultView().WithModel<CourseDetailsViewModel>(
                 vm =>
                 {
                     vm.FindACoursePage.Should().BeEquivalentTo(controller.FindAcoursePage);
                 });

            A.CallTo(() => fakeCourseSearchService.GetCourseDetailsAsync(A<string>._, A<string>._)).MustHaveHappened();
        }
    }
}
