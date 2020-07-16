using ASP;
using DFC.Digital.Data.Model;
using DFC.Digital.Web.Sitefinity.CourseModule.UnitTests.Helpers;
using FluentAssertions;
using HtmlAgilityPack;
using RazorGenerator.Testing;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace DFC.Digital.Web.Sitefinity.CourseModule.UnitTests
{
    public class CourseDetailsWorkBasedVenuesTests
    {
        private readonly CourseDetailsViewModel courseDetailsViewModel;
        private readonly _MVC_Views_CourseDetails_CourseDetails_cshtml courseDetailsView;

        public CourseDetailsWorkBasedVenuesTests()
        {
            courseDetailsView = new _MVC_Views_CourseDetails_CourseDetails_cshtml();

            courseDetailsViewModel = new CourseDetailsViewModel
            {
                NationalWorkBasedText = "National",
                CourseDetails =
                {
                    National = true,
                    AttendanceMode = "Work based",
                }
            };
        }

        [Fact]
        public void Dfc11436CourseDetailsWorkBasedNationalViewTest()
        {
            var htmlDocument = courseDetailsView.RenderAsHtml(courseDetailsViewModel);
            htmlDocument.DocumentNode.SelectSingleNode("//*[@id='NationalWorkBasedText']").InnerText.Should().Be(courseDetailsViewModel.NationalWorkBasedText);
        }

        [Fact]
        public void Dfc11436CourseDetailsWorkBasedCourseRegionsViewTest()
        {
            courseDetailsViewModel.CourseDetails.National = false;
            courseDetailsViewModel.CourseDetails.CourseRegions = new List<CourseRegion>() { new CourseRegion() { Region = "Region1", Area = "Area1, Area2" } };

            var htmlDocument = courseDetailsView.RenderAsHtml(courseDetailsViewModel);
            htmlDocument.DocumentNode.SelectSingleNode("//*[@id='NationalWorkBasedText']").Should().BeNull();

            htmlDocument.DocumentNode.Descendants("span")
                 .Any(p => p.Attributes["class"].Value.Contains("govuk-details__summary-text") && p.InnerText.Contains(courseDetailsViewModel.CourseDetails.CourseRegions[0].Region)).Should().BeTrue();

            htmlDocument.DocumentNode.Descendants("div")
               .Any(p => p.Attributes["class"].Value.Contains("govuk-details__text") && p.InnerText.Contains(courseDetailsViewModel.CourseDetails.CourseRegions[0].Area)).Should().BeTrue();
        }
    }
}
