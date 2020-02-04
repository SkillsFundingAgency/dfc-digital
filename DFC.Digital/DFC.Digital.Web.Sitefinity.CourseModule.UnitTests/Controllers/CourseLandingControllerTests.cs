using DFC.Digital.Core;
using DFC.Digital.Data.Model;
using DFC.Digital.Web.Sitefinity.CourseModule.Mvc.Controllers;
using FakeItEasy;
using FluentAssertions;
using TestStack.FluentMVCTesting;
using Xunit;

namespace DFC.Digital.Web.Sitefinity.CourseModule.UnitTests
{
    /// <summary>
    /// description CourseLandingControllerTests.
    /// </summary>
    public class CourseLandingControllerTests
    {
        private readonly IApplicationLogger fakeApplicationLogger;
        private readonly IQueryStringBuilder<CourseSearchFilters> fakeBuildQueryStringService;

        public CourseLandingControllerTests()
        {
            fakeApplicationLogger = A.Fake<IApplicationLogger>(ops => ops.Strict());
            fakeBuildQueryStringService = A.Fake<IQueryStringBuilder<CourseSearchFilters>>(ops => ops.Strict());
        }

        [Theory]
        [InlineData("CourseNameHintText", "CourseNameLabel", "ProviderLabel", "ProviderHintText", "LocationLabel", "LocationHintText", "CourseSearchResultsPage", "Find a course")]
        public void IndexSetDefaultsTest(string courseNameHintText, string courseNameLabel, string providerLabel, string providerHintText, string locationLabel, string locationHintText, string courseSearchResultsPage, string buttonText)
        {
            // Assign
            var controller = new CourseLandingController(fakeApplicationLogger, fakeBuildQueryStringService)
            {
                CourseNameHintText = courseNameHintText,
                CourseNameLabel = courseNameLabel,
                LocationLabel = locationLabel,
                ProviderLabel = providerLabel,
                ProviderHintText = providerHintText,
                LocationHintText = locationHintText,
                CourseSearchResultsPage = courseSearchResultsPage,
                SubmitButtonText = buttonText
            };

            // Act
            var controllerResult = controller.WithCallTo(contrl => contrl.Index());

            // Assert
            controllerResult.ShouldRenderDefaultView().WithModel<CourseLandingViewModel>(
                    vm =>
                    {
                        vm.CourseNameHintText.Should().BeEquivalentTo(controller.CourseNameHintText);
                        vm.CourseNameLabel.Should().BeEquivalentTo(controller.CourseNameLabel);
                        vm.LocationLabel.Should().BeEquivalentTo(controller.LocationLabel);
                        vm.ProviderLabel.Should().BeEquivalentTo(controller.ProviderLabel);
                        vm.ProviderNameHintText.Should().BeEquivalentTo(controller.ProviderHintText);
                        vm.LocationHintText.Should().BeEquivalentTo(controller.LocationHintText);
                        vm.SubmitButtonText.Should().BeEquivalentTo(controller.SubmitButtonText);
                    });
        }

        [Theory]
        [InlineData("Maths")]
        [InlineData("")]
        [InlineData("<script />")]
        public void SubmitTests(string courseName)
        {
            // Assign
            var postModel = new CourseLandingViewModel
            {
                SearchTerm = courseName
            };

            var controller = new CourseLandingController(fakeApplicationLogger, fakeBuildQueryStringService);

            A.CallTo(() => fakeBuildQueryStringService.BuildPathAndQueryString(controller.CourseSearchResultsPage, postModel)).Returns(controller.CourseSearchResultsPage);

            // Act
            var controllerResult = controller.WithCallTo(contrl => contrl.Index(postModel));

            // Assert
            controllerResult.ShouldRedirectTo(
                    fakeBuildQueryStringService.BuildPathAndQueryString(controller.CourseSearchResultsPage, postModel));
        }
    }
}
