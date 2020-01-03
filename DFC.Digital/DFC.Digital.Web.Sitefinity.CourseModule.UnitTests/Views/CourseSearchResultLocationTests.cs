using ASP;
using DFC.Digital.Data.Model;
using FluentAssertions;
using HtmlAgilityPack;
using RazorGenerator.Testing;
using System.Linq;
using Xunit;

namespace DFC.Digital.Web.Sitefinity.CourseModule.UnitTests
{
    public class CourseSearchResultLocationTests
    {
        private readonly _MVC_Views_CourseSearchResults_CourseDetail_cshtml courseDetailsView;
        private readonly CourseListingViewModel courseListingViewModel;

        public CourseSearchResultLocationTests()
        {
            courseDetailsView = new _MVC_Views_CourseSearchResults_CourseDetail_cshtml();
            courseListingViewModel = new CourseListingViewModel
            {
                LocationLabel = "LocationLabel",
                ProviderLabel = "ProviderLabel",
                AdvancedLoanProviderLabel = "AdvancedLoanProviderLabel",
                StartDateLabel = "StartDateLabel",
                Course = new Course()
            };
        }

        [Fact]
        public void CourseDetailsViewNullLocationDetails()
        {
            // Act
            var htmlDom = courseDetailsView.RenderAsHtml(courseListingViewModel);

            // Assert
            htmlDom.DocumentNode.InnerText.Should().NotContain(courseListingViewModel.LocationLabel);
        }

        [Fact]
        public void CourseDetailsViewNullLocationAddress()
        {
            //Arrange
            courseListingViewModel.Course.LocationDetails = new LocationDetails();

            // Act
            var htmlDom = courseDetailsView.RenderAsHtml(courseListingViewModel);

            // Assert
            htmlDom.DocumentNode.InnerText.Should().NotContain(courseListingViewModel.LocationLabel);
        }

        [Fact]
        public void CourseDetailsViewVaildLocationAddress()
        {
            //Arrange
            courseListingViewModel.Course.LocationDetails = new LocationDetails() { LocationAddress = "LocationAddress" };

            // Act
            var htmlDom = courseDetailsView.RenderAsHtml(courseListingViewModel);

            // Assert
            htmlDom.DocumentNode.InnerText.Should().Contain(courseListingViewModel.Course.LocationDetails.LocationAddress);
        }
    }
}
