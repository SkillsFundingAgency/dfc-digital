using ASP;
using DFC.Digital.Data.Model;
using FluentAssertions;
using HtmlAgilityPack;
using RazorGenerator.Testing;
using System;
using System.Linq;
using Xunit;

namespace DFC.Digital.Web.Sitefinity.CourseModule.UnitTests
{
    public class CourseDetailsViewTests
    {
        [Fact]
        public void Dfc7056CourseDetailsIndexViewTests()
        {
            // Arrange
            var courseDetailsIndex = new _MVC_Views_CourseDetails_Index_cshtml();
            var courseDetailsViewModel = new CourseDetailsViewModel();
            courseDetailsViewModel.CourseDetails.Title = nameof(CourseDetailsViewModel.CourseDetails.Title);

            // Act
            var htmlDocument = courseDetailsIndex.RenderAsHtml(courseDetailsViewModel);

            // Assert
            htmlDocument.DocumentNode.Descendants("div")
               .Count(div => div.Attributes["class"].Value.Contains("govuk-grid-row")).Should().Be(1);
        }

        [Fact]
        public void Dfc7056CourseDetailsPartialViewTests()
        {
            // Arrange
            var courseDetailsIndex = new _MVC_Views_CourseDetails_CourseDetails_cshtml();
            var courseDetails = new CourseDetails();
            courseDetails.Title = nameof(CourseDetails.Title);

            // Act
            var htmlDocument = courseDetailsIndex.RenderAsHtml(courseDetails);

            // Assert
            this.AssertTagInnerTextValue(htmlDocument, courseDetails.Title, "h1");
            this.AssertTableCounts(htmlDocument, 2);
            this.AssertH2HeadingCounts(htmlDocument, 6);
        }

        [Theory]
        [InlineData("Cost", true)]
        [InlineData("Cost", false)]
        public void ShowAndHideCostTableRowTest(string propertyName, bool hasValue)
        {
            // Arrange
            var courseDetailsCourseDetails = new _MVC_Views_CourseDetails_CourseDetails_cshtml();
            var courseDetailsViewModel = new CourseDetailsViewModel();
            var courseDetails = new CourseDetails();
            if (!hasValue)
            {
                courseDetails.Cost = null;
            }
            else
            {
                courseDetails.Cost = nameof(CourseDetails.Cost);
            }

            // Act
            var htmlDocument = courseDetailsCourseDetails.RenderAsHtml(courseDetails);

            // Assert
            if (hasValue)
            {
                htmlDocument.DocumentNode.Descendants("th")
               .Any(div => div.Attributes["class"].Value.Contains("govuk-table__header") &&
                div.InnerText.Contains(propertyName)).Should().BeTrue();
            }
            else
            {
                htmlDocument.DocumentNode.Descendants("th")
               .Any(div => div.Attributes["class"].Value.Contains("govuk-table__header") &&
                div.InnerText.Contains(propertyName)).Should().BeFalse();
            }
        }

        [Theory]
        [InlineData("Duration", true)]
        [InlineData("Duration", false)]
        public void ShowAndHideDurationTableRowTest(string propertyName, bool hasValue)
        {
            // Arrange
            var courseDetailsCourseDetails = new _MVC_Views_CourseDetails_CourseDetails_cshtml();
            var courseDetailsViewModel = new CourseDetailsViewModel();
            var courseDetails = new CourseDetails();
            if (!hasValue)
            {
                courseDetails.Duration = null;
            }
            else
            {
                courseDetails.Duration = nameof(CourseDetails.Duration);
            }

            // Act
            var htmlDocument = courseDetailsCourseDetails.RenderAsHtml(courseDetails);

            // Assert
            if (hasValue)
            {
                htmlDocument.DocumentNode.Descendants("th")
               .Any(div => div.Attributes["class"].Value.Contains("govuk-table__header") &&
                div.InnerText.Contains(propertyName)).Should().BeTrue();
            }
            else
            {
                htmlDocument.DocumentNode.Descendants("th")
               .Any(div => div.Attributes["class"].Value.Contains("govuk-table__header") &&
                div.InnerText.Contains(propertyName)).Should().BeFalse();
            }
        }

        [Theory]
        [InlineData("Qualification name", true)]
        [InlineData("Qualification name", false)]
        public void ShowAndHideQualificationNameTableRowTest(string propertyTitle, bool hasValue)
        {
            // Arrange
            var courseDetailsCourseDetails = new _MVC_Views_CourseDetails_CourseDetails_cshtml();
            var courseDetailsViewModel = new CourseDetailsViewModel();
            var courseDetails = new CourseDetails();
            if (!hasValue)
            {
                courseDetails.QualificationName = null;
            }
            else
            {
                courseDetails.QualificationName = nameof(CourseDetails.QualificationName);
            }

            // Act
            var htmlDocument = courseDetailsCourseDetails.RenderAsHtml(courseDetails);

            // Assert
            if (hasValue)
            {
                htmlDocument.DocumentNode.Descendants("th")
               .Any(div => div.Attributes["class"].Value.Contains("govuk-table__header") &&
                div.InnerText.Contains(propertyTitle)).Should().BeTrue();
            }
            else
            {
                htmlDocument.DocumentNode.Descendants("th")
               .Any(div => div.Attributes["class"].Value.Contains("govuk-table__header") &&
                div.InnerText.Contains(propertyTitle)).Should().BeFalse();
            }
        }

        [Theory]
        [InlineData("Qualification level", true)]
        [InlineData("Qualification level", false)]
        public void ShowAndHideQualificationLevelTableRowTest(string propertyTitle, bool hasValue)
        {
            // Arrange
            var courseDetailsCourseDetails = new _MVC_Views_CourseDetails_CourseDetails_cshtml();
            var courseDetailsViewModel = new CourseDetailsViewModel();
            var courseDetails = new CourseDetails();
            if (!hasValue)
            {
                courseDetails.QualificationLevel = null;
            }
            else
            {
                courseDetails.QualificationLevel = nameof(CourseDetails.QualificationLevel);
            }

            // Act
            var htmlDocument = courseDetailsCourseDetails.RenderAsHtml(courseDetails);

            // Assert
            if (hasValue)
            {
                htmlDocument.DocumentNode.Descendants("th")
               .Any(div => div.Attributes["class"].Value.Contains("govuk-table__header") &&
                div.InnerText.Contains(propertyTitle)).Should().BeTrue();
            }
            else
            {
                htmlDocument.DocumentNode.Descendants("th")
               .Any(div => div.Attributes["class"].Value.Contains("govuk-table__header") &&
                div.InnerText.Contains(propertyTitle)).Should().BeFalse();
            }
        }

        [Theory]
        [InlineData("Entry requirements", true)]
        [InlineData("Entry requirements", false)]
        public void ShowAndHideEntryRequirementsTableRowTest(string propertyName, bool hasValue)
        {
            // Arrange
            var courseDetailsCourseDetails = new _MVC_Views_CourseDetails_CourseDetails_cshtml();
            var courseDetailsViewModel = new CourseDetailsViewModel();
            var courseDetails = new CourseDetails();
            if (!hasValue)
            {
                courseDetails.EntryRequirements = null;
            }
            else
            {
                courseDetails.EntryRequirements = nameof(CourseDetails.EntryRequirements);
            }

            // Act
            var htmlDocument = courseDetailsCourseDetails.RenderAsHtml(courseDetails);

            // Assert
            if (hasValue)
            {
                htmlDocument.DocumentNode.Descendants("th")
               .Any(div => div.Attributes["class"].Value.Contains("govuk-table__header") &&
                div.InnerText.Contains(propertyName)).Should().BeTrue();
            }
            else
            {
                htmlDocument.DocumentNode.Descendants("th")
               .Any(div => div.Attributes["class"].Value.Contains("govuk-table__header") &&
                div.InnerText.Contains(propertyName)).Should().BeFalse();
            }
        }

        [Theory]
        [InlineData("Start date", true)]
        [InlineData("Start date", false)]
        public void ShowAndHideStartDateLabelTableRowTest(string propertyName, bool hasValue)
        {
            // Arrange
            var courseDetailsCourseDetails = new _MVC_Views_CourseDetails_CourseDetails_cshtml();
            var courseDetailsViewModel = new CourseDetailsViewModel();
            var courseDetails = new CourseDetails();
            if (!hasValue)
            {
                courseDetails.StartDateLabel = null;
            }
            else
            {
                courseDetails.StartDateLabel = DateTime.Today.ToShortDateString();
            }

            // Act
            var htmlDocument = courseDetailsCourseDetails.RenderAsHtml(courseDetails);

            // Assert
            if (hasValue)
            {
                htmlDocument.DocumentNode.Descendants("th")
               .Any(div => div.Attributes["class"].Value.Contains("govuk-table__header") &&
                div.InnerText.Contains(propertyName)).Should().BeTrue();
            }
            else
            {
                htmlDocument.DocumentNode.Descendants("th")
               .Any(div => div.Attributes["class"].Value.Contains("govuk-table__header") &&
                div.InnerText.Contains(propertyName)).Should().BeFalse();
            }
        }

        [Theory]
        [InlineData("Classroom or online", true)]
        [InlineData("Classroom or online", false)]
        public void ShowAndHideAttendanceModeTableRowTest(string propertyName, bool hasValue)
        {
            // Arrange
            var courseDetailsCourseDetails = new _MVC_Views_CourseDetails_CourseDetails_cshtml();
            var courseDetailsViewModel = new CourseDetailsViewModel();
            var courseDetails = new CourseDetails();
            if (!hasValue)
            {
                courseDetails.AttendanceMode = null;
            }
            else
            {
                courseDetails.AttendanceMode = nameof(CourseDetails.AttendanceMode);
            }

            // Act
            var htmlDocument = courseDetailsCourseDetails.RenderAsHtml(courseDetails);

            // Assert
            if (hasValue)
            {
                htmlDocument.DocumentNode.Descendants("th")
               .Any(div => div.Attributes["class"].Value.Contains("govuk-table__header") &&
                div.InnerText.Contains(propertyName)).Should().BeTrue();
            }
            else
            {
                htmlDocument.DocumentNode.Descendants("th")
               .Any(div => div.Attributes["class"].Value.Contains("govuk-table__header") &&
                div.InnerText.Contains(propertyName)).Should().BeFalse();
            }
        }

        [Theory]
        [InlineData("Day, night or weekend", true)]
        [InlineData("Day, night or weekend", false)]
        public void ShowAndHideAttendancePatternTableRowTest(string propertyName, bool hasValue)
        {
            // Arrange
            var courseDetailsCourseDetails = new _MVC_Views_CourseDetails_CourseDetails_cshtml();
            var courseDetailsViewModel = new CourseDetailsViewModel();
            var courseDetails = new CourseDetails();
            if (!hasValue)
            {
                courseDetails.AttendancePattern = null;
            }
            else
            {
                courseDetails.AttendancePattern = nameof(CourseDetails.AttendancePattern);
            }

            // Act
            var htmlDocument = courseDetailsCourseDetails.RenderAsHtml(courseDetails);

            // Assert
            if (hasValue)
            {
                htmlDocument.DocumentNode.Descendants("th")
               .Any(div => div.Attributes["class"].Value.Contains("govuk-table__header") &&
                div.InnerText.Contains(propertyName)).Should().BeTrue();
            }
            else
            {
                htmlDocument.DocumentNode.Descendants("th")
               .Any(div => div.Attributes["class"].Value.Contains("govuk-table__header") &&
                div.InnerText.Contains(propertyName)).Should().BeFalse();
            }
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("Description")]
        public void ShowCourseDescriptionTest(string propertyValue)
        {
            // Arrange
            var courseDetailsCourseDetails = new _MVC_Views_CourseDetails_CourseDetails_cshtml();
            var courseDetailsViewModel = new CourseDetailsViewModel();
            var courseDetails = new CourseDetails();
            courseDetails.Description = propertyValue;
            courseDetails.NoCourseDescriptionMessage = nameof(CourseDetails.NoCourseDescriptionMessage);

            // Act
            var htmlDocument = courseDetailsCourseDetails.RenderAsHtml(courseDetails);

            // Assert
            if (!string.IsNullOrWhiteSpace(propertyValue))
            {
               htmlDocument.DocumentNode.Descendants("p")
                .Any(div => div.Attributes["class"].Value.Contains("govuk-body") &&
                 div.InnerText.Contains(courseDetails.Description)).Should().BeTrue();
            }
            else
            {
                htmlDocument.DocumentNode.Descendants("p")
                .Any(div => div.Attributes["class"].Value.Contains("govuk-body") &&
                 div.InnerText.Contains(courseDetails.NoCourseDescriptionMessage)).Should().BeTrue();
            }
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("EntryRequirements")]
        public void ShowEntryRequirementsTest(string propertyValue)
        {
            // Arrange
            var courseDetailsCourseDetails = new _MVC_Views_CourseDetails_CourseDetails_cshtml();
            var courseDetailsViewModel = new CourseDetailsViewModel();
            var courseDetails = new CourseDetails();
            courseDetails.EntryRequirements = propertyValue;
            courseDetails.NoEntryRequirementsAvailableMessage = nameof(CourseDetails.NoEntryRequirementsAvailableMessage);

            // Act
            var htmlDocument = courseDetailsCourseDetails.RenderAsHtml(courseDetails);

            // Assert
            if (!string.IsNullOrWhiteSpace(propertyValue))
            {
                htmlDocument.DocumentNode.Descendants("p")
                 .Any(div => div.Attributes["class"].Value.Contains("govuk-body") &&
                  div.InnerText.Contains(courseDetails.EntryRequirements)).Should().BeTrue();
            }
            else
            {
                htmlDocument.DocumentNode.Descendants("p")
                .Any(div => div.Attributes["class"].Value.Contains("govuk-body") &&
                 div.InnerText.Contains(courseDetails.NoEntryRequirementsAvailableMessage)).Should().BeTrue();
            }
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("EquipmentRequired")]
        public void ShowEquipmentRequiredTest(string propertyValue)
        {
            // Arrange
            var courseDetailsCourseDetails = new _MVC_Views_CourseDetails_CourseDetails_cshtml();
            var courseDetailsViewModel = new CourseDetailsViewModel();
            var courseDetails = new CourseDetails();
            courseDetails.EquipmentRequired = propertyValue;
            courseDetails.NoEquipmentRequiredMessage = nameof(CourseDetails.NoEquipmentRequiredMessage);

            // Act
            var htmlDocument = courseDetailsCourseDetails.RenderAsHtml(courseDetails);

            // Assert
            if (!string.IsNullOrWhiteSpace(propertyValue))
            {
                htmlDocument.DocumentNode.Descendants("p")
                 .Any(div => div.Attributes["class"].Value.Contains("govuk-body") &&
                  div.InnerText.Contains(courseDetails.EquipmentRequired)).Should().BeTrue();
            }
            else
            {
                htmlDocument.DocumentNode.Descendants("p")
                .Any(div => div.Attributes["class"].Value.Contains("govuk-body") &&
                 div.InnerText.Contains(courseDetails.NoEquipmentRequiredMessage)).Should().BeTrue();
            }
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("AssessmentMethod")]
        public void ShowAssessmentMethodTest(string propertyValue)
        {
            // Arrange
            var courseDetailsCourseDetails = new _MVC_Views_CourseDetails_CourseDetails_cshtml();
            var courseDetailsViewModel = new CourseDetailsViewModel();
            var courseDetails = new CourseDetails();
            courseDetails.AssessmentMethod = propertyValue;
            courseDetails.NoAssessmentMethodAvailableMessage = nameof(CourseDetails.NoAssessmentMethodAvailableMessage);

            // Act
            var htmlDocument = courseDetailsCourseDetails.RenderAsHtml(courseDetails);

            // Assert
            if (!string.IsNullOrWhiteSpace(propertyValue))
            {
                htmlDocument.DocumentNode.Descendants("p")
                 .Any(div => div.Attributes["class"].Value.Contains("govuk-body") &&
                  div.InnerText.Contains(courseDetails.AssessmentMethod)).Should().BeTrue();
            }
            else
            {
                htmlDocument.DocumentNode.Descendants("p")
                .Any(div => div.Attributes["class"].Value.Contains("govuk-body") &&
                 div.InnerText.Contains(courseDetails.NoAssessmentMethodAvailableMessage)).Should().BeTrue();
            }
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void ShowVenueDetailsTest(bool venueExists)
        {
            // Arrange
            var courseDetailsCourseDetails = new _MVC_Views_CourseDetails_CourseDetails_cshtml();
            var courseDetailsViewModel = new CourseDetailsViewModel();
            var courseDetails = new CourseDetails();
            courseDetails.VenueDetails = new VenueDetails();
            if (venueExists)
            {
                courseDetails.VenueDetails.VenueName = nameof(CourseDetails.VenueDetails.VenueName);
                courseDetails.VenueDetails.Location.AddressLine1 = nameof(CourseDetails.VenueDetails.Location.AddressLine1);
                courseDetails.VenueDetails.Location.AddressLine2 = nameof(CourseDetails.VenueDetails.Location.AddressLine2);
                courseDetails.VenueDetails.Location.County = nameof(CourseDetails.VenueDetails.Location.County);
                courseDetails.VenueDetails.Location.Postcode = nameof(CourseDetails.VenueDetails.Location.Postcode);
            }
            else
            {
                courseDetails.VenueDetails = null;
                courseDetails.NoVenueAvailableMessage = nameof(CourseDetails.NoVenueAvailableMessage);
            }

            // Act
            var htmlDocument = courseDetailsCourseDetails.RenderAsHtml(courseDetails);

            // Assert
            if (venueExists)
            {
                htmlDocument.DocumentNode.Descendants("td")
                 .Any(div => div.Attributes["class"].Value.Contains("govuk-table__cell") &&
                  div.InnerText.Contains(courseDetails.VenueDetails.VenueName)).Should().BeTrue();

                htmlDocument.DocumentNode.Descendants("td")
              .Any(div => div.Attributes["class"].Value.Contains("govuk-table__cell") &&
               div.InnerText.Contains(courseDetails.VenueDetails.Location.AddressLine1)).Should().BeTrue();
                htmlDocument.DocumentNode.Descendants("td")
            .Any(div => div.Attributes["class"].Value.Contains("govuk-table__cell") &&
             div.InnerText.Contains(courseDetails.VenueDetails.Location.AddressLine2)).Should().BeTrue();
                htmlDocument.DocumentNode.Descendants("td")
            .Any(div => div.Attributes["class"].Value.Contains("govuk-table__cell") &&
             div.InnerText.Contains(courseDetails.VenueDetails.Location.Postcode)).Should().BeTrue();
                htmlDocument.DocumentNode.Descendants("td")
            .Any(div => div.Attributes["class"].Value.Contains("govuk-table__cell") &&
             div.InnerText.Contains(courseDetails.VenueDetails.Location.County)).Should().BeTrue();
            }
            else
            {
                htmlDocument.DocumentNode.Descendants("td")
                .Any(div => div.Attributes["class"].Value.Contains("govuk-table__cell") &&
                 div.InnerText.Contains(courseDetails.NoVenueAvailableMessage)).Should().BeTrue();
                htmlDocument.DocumentNode.Descendants("th")
             .Any(div => div.Attributes["class"].Value.Contains("govuk-table__header") &&
              div.InnerText.Contains("Address")).Should().BeFalse();
            }
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void ShowOtherVenuesAndDatesTest(bool otherVenuesExists)
        {
            // Arrange
            var courseDetailsCourseDetails = new _MVC_Views_CourseDetails_OtherDatesAndVenues_cshtml();
            var courseDetailsViewModel = new CourseDetailsViewModel();
            var courseDetails = new CourseDetails();
            courseDetails.VenueDetails = new VenueDetails();
            if (otherVenuesExists)
            {
                courseDetails.OtherDatesAndVenues = new System.Collections.Generic.List<OtherDatesAndVenues>();
                var otherDatesAndVenues = new OtherDatesAndVenues();
                courseDetails.OtherDatesAndVenues.Add(otherDatesAndVenues);
                courseDetails.OtherDatesAndVenues.FirstOrDefault().VenueName = nameof(CourseDetails.OtherDatesAndVenues);
            }
            else
            {
                courseDetails.OtherDatesAndVenues = null;
                courseDetails.NoOtherDateOrVenueAvailableMessage = nameof(CourseDetails.NoOtherDateOrVenueAvailableMessage);
            }

            // Act
            var htmlDocument = courseDetailsCourseDetails.RenderAsHtml(courseDetails);

            // Assert
            if (otherVenuesExists)
            {
                htmlDocument.DocumentNode.Descendants("td")
                 .Any(div => div.Attributes["class"].Value.Contains("govuk-table__cell") &&
                  div.InnerText.Contains(courseDetails.OtherDatesAndVenues.FirstOrDefault().VenueName)).Should().BeTrue();
            }
            else
            {
                htmlDocument.DocumentNode.Descendants("td")
                .Any(div => div.Attributes["class"].Value.Contains("govuk-table__cell") &&
                 div.InnerText.Contains(courseDetails.NoOtherDateOrVenueAvailableMessage)).Should().BeTrue();
            }
        }

        private static void AssertErrorFieldIsEmpty(HtmlDocument htmlDocument)
        {
            htmlDocument.DocumentNode.Descendants("span")
               .Count(span => span.InnerText.Contains("Message")).Should().Be(0);
        }

        private void AssertErrorDetailOnField(HtmlDocument htmlDocument, string errorMessage)
        {
            htmlDocument.DocumentNode.Descendants("span")
                .Count(span => span.InnerText.Contains("Message")).Should().BeGreaterThan(0);
        }

        private void AssertTagInnerTextValue(HtmlDocument htmlDocument, string innerText, string tag)
        {
            htmlDocument.DocumentNode.Descendants(tag)
                .Any(h1 => h1.InnerText.Contains(innerText)).Should().BeTrue();
        }

        private void AssertTableCounts(HtmlDocument htmlDocument, int count)
        {
            htmlDocument.DocumentNode.Descendants("table")
                .Count(div => div.Attributes["class"].Value.Contains("govuk-table k-table")).Should().Be(count);
        }

        private void AssertH2HeadingCounts(HtmlDocument htmlDocument, int count)
        {
            htmlDocument.DocumentNode.Descendants("h2")
                .Count(div => div.Attributes["class"].Value.Contains("govuk-heading-m")).Should().Be(count);
        }

        private void AssertTableRowsCount(HtmlDocument htmlDocument, int remainingRows)
        {
            htmlDocument.DocumentNode.Descendants("tr")
                .Count(div => div.Attributes["class"].Value.Contains("govuk-table__row")).Should().Be(remainingRows);
        }
    }
}
