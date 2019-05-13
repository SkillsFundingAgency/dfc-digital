using DFC.Digital.Core;
using DFC.Digital.Web.Sitefinity.CourseModule.Mvc.Controllers;
using DFC.Digital.Web.Sitefinity.CourseModule.Mvc.Models;
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
        private readonly ICourseSearchConverter fakeCourseSearchConverter;

        /// <summary>
        /// Initializes a new instance of the <see cref="CourseLandingControllerTests"/> class.
        /// </summary>
        public CourseLandingControllerTests()
        {
            this.fakeApplicationLogger = A.Fake<IApplicationLogger>(ops => ops.Strict());
            this.fakeCourseSearchConverter = A.Fake<ICourseSearchConverter>();
        }

        /// <summary>
        /// Tests Index Controller with IndexSetDefaultsTest <see cref="CourseLandingController"/> controller class.
        /// </summary>
        [Theory]
        [InlineData("CourseNameHintText", "CourseNameLabel", "ProviderLabel", "LocationLabel", "LocationHintText", "CourseSearchResultsPage", "Dfe1619FundedText")]
        public void IndexSetDefaultsTest(string courseNameHintText, string courseNameLabel, string providerLabel, string locationLabel, string locationHintText, string courseSearchResultsPage, string dfe1619FundedText)
        {
            // Assign
            var controller = new CourseLandingController(fakeApplicationLogger, fakeCourseSearchConverter)
            {
                CourseNameHintText = courseNameHintText,
                CourseNameLabel = courseNameLabel,
                LocationLabel = locationLabel,
                ProviderLabel = providerLabel,
                LocationHintText = locationHintText,
                CourseSearchResultsPage = courseSearchResultsPage,
                Dfe1619FundedText = dfe1619FundedText,
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
                        vm.LocationHintText.Should().BeEquivalentTo(controller.LocationHintText);
                        vm.Dfe1619FundedText.Should().BeEquivalentTo(controller.Dfe1619FundedText);
                    });
        }

        /// <summary>
        /// Tests Submit functionality with SubmitTests <see cref="CourseLandingController"/> controller class.
        /// </summary>
        /// 
        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("Maths")]
        public void SubmitTests(string searchTerm)
        {
            // Assign
            var postModel = new CourseLandingViewModel();

            var controller = new CourseLandingController(this.fakeApplicationLogger, this.fakeCourseSearchConverter);

            // Act
            var controllerResult = controller.WithCallTo(contrl => contrl.Index(postModel));

            // Assert
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                controller.ShouldRedirectTo(
                    this.fakeCourseSearchConverter.BuildSearchRedirectPathAndQueryString(controller.CourseSearchResultsPage, postModel, controller.LocationRegex));
            }
            else
            {
                controllerResult.ShouldRenderDefaultView().WithModel<CourseLandingViewModel>(
                   vm =>
                   {
                       vm.CourseNameHintText.Should().BeEquivalentTo(controller.CourseNameHintText);
                       vm.CourseNameLabel.Should().BeEquivalentTo(controller.CourseNameLabel);
                       vm.LocationLabel.Should().BeEquivalentTo(controller.LocationLabel);
                       vm.ProviderLabel.Should().BeEquivalentTo(controller.ProviderLabel);
                       vm.LocationHintText.Should().BeEquivalentTo(controller.LocationHintText);
                       vm.Dfe1619FundedText.Should().BeEquivalentTo(controller.Dfe1619FundedText);
                   });
            }
        }
    }
}
