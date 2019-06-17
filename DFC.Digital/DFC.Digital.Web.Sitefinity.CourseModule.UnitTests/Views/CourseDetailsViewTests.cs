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
            var model = new CourseDetailsViewModel();
           model.CourseDetails.Title = nameof(CourseDetails.Title);

            // Act
            var htmlDocument = courseDetailsIndex.RenderAsHtml(model);

            // Assert
            this.AssertTagInnerTextValue(htmlDocument, model.CourseDetails.Title, "h1");
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
                courseDetailsViewModel.CourseDetails.Cost = null;
            }
            else
            {
                courseDetailsViewModel.CourseDetails.Cost = nameof(CourseDetails.Cost);
            }

            // Act
            var htmlDocument = courseDetailsCourseDetails.RenderAsHtml(courseDetailsViewModel);

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
                courseDetailsViewModel.CourseDetails.Duration = null;
            }
            else
            {
                courseDetailsViewModel.CourseDetails.Duration = nameof(CourseDetails.Duration);
            }

            // Act
            var htmlDocument = courseDetailsCourseDetails.RenderAsHtml(courseDetailsViewModel);

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
                courseDetailsViewModel.CourseDetails.QualificationName = null;
            }
            else
            {
                courseDetailsViewModel.CourseDetails.QualificationName = nameof(CourseDetails.QualificationName);
            }

            // Act
            var htmlDocument = courseDetailsCourseDetails.RenderAsHtml(courseDetailsViewModel);

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
                courseDetailsViewModel.CourseDetails.QualificationLevel = null;
            }
            else
            {
                courseDetailsViewModel.CourseDetails.QualificationLevel = nameof(CourseDetails.QualificationLevel);
            }

            // Act
            var htmlDocument = courseDetailsCourseDetails.RenderAsHtml(courseDetailsViewModel);

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
                courseDetailsViewModel.CourseDetails.EntryRequirements = null;
            }
            else
            {
                courseDetailsViewModel.CourseDetails.EntryRequirements = nameof(CourseDetails.EntryRequirements);
            }

            // Act
            var htmlDocument = courseDetailsCourseDetails.RenderAsHtml(courseDetailsViewModel);

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
                courseDetailsViewModel.CourseDetails.StartDateLabel = null;
            }
            else
            {
                courseDetailsViewModel.CourseDetails.StartDateLabel = DateTime.Today.ToShortDateString();
            }

            // Act
            var htmlDocument = courseDetailsCourseDetails.RenderAsHtml(courseDetailsViewModel);

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
                courseDetailsViewModel.CourseDetails.AttendanceMode = null;
            }
            else
            {
                courseDetailsViewModel.CourseDetails.AttendanceMode = nameof(CourseDetails.AttendanceMode);
            }

            // Act
            var htmlDocument = courseDetailsCourseDetails.RenderAsHtml(courseDetailsViewModel);

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
                courseDetailsViewModel.CourseDetails.AttendancePattern = null;
            }
            else
            {
                courseDetailsViewModel.CourseDetails.AttendancePattern = nameof(CourseDetails.AttendancePattern);
            }

            // Act
            var htmlDocument = courseDetailsCourseDetails.RenderAsHtml(courseDetailsViewModel);

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
            courseDetailsViewModel.CourseDetails.Description = propertyValue;
            courseDetailsViewModel.NoCourseDescriptionMessage = nameof(CourseDetailsViewModel.NoCourseDescriptionMessage);

            // Act
            var htmlDocument = courseDetailsCourseDetails.RenderAsHtml(courseDetailsViewModel);

            // Assert
            if (!string.IsNullOrWhiteSpace(propertyValue))
            {
                htmlDocument.DocumentNode.Descendants("p")
                 .Any(div => div.Attributes["class"].Value.Contains("govuk-body") &&
                  div.InnerText.Contains(courseDetailsViewModel.CourseDetails.Description)).Should().BeTrue();
            }
            else
            {
                htmlDocument.DocumentNode.Descendants("p")
                .Any(div => div.Attributes["class"].Value.Contains("govuk-body") &&
                 div.InnerText.Contains(courseDetailsViewModel.NoCourseDescriptionMessage)).Should().BeTrue();
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
            courseDetailsViewModel.CourseDetails.EntryRequirements = propertyValue;
            courseDetailsViewModel.NoEntryRequirementsAvailableMessage = nameof(CourseDetailsViewModel.NoEntryRequirementsAvailableMessage);

            // Act
            var htmlDocument = courseDetailsCourseDetails.RenderAsHtml(courseDetailsViewModel);

            // Assert
            if (!string.IsNullOrWhiteSpace(propertyValue))
            {
                htmlDocument.DocumentNode.Descendants("p")
                 .Any(div => div.Attributes["class"].Value.Contains("govuk-body") &&
                  div.InnerText.Contains(courseDetailsViewModel.CourseDetails.EntryRequirements)).Should().BeTrue();
            }
            else
            {
                htmlDocument.DocumentNode.Descendants("p")
                .Any(div => div.Attributes["class"].Value.Contains("govuk-body") &&
                 div.InnerText.Contains(courseDetailsViewModel.NoEntryRequirementsAvailableMessage)).Should().BeTrue();
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
            courseDetailsViewModel.CourseDetails.EquipmentRequired = propertyValue;
            courseDetailsViewModel.NoEquipmentRequiredMessage = nameof(CourseDetailsViewModel.NoEquipmentRequiredMessage);

            // Act
            var htmlDocument = courseDetailsCourseDetails.RenderAsHtml(courseDetailsViewModel);

            // Assert
            if (!string.IsNullOrWhiteSpace(propertyValue))
            {
                htmlDocument.DocumentNode.Descendants("p")
                 .Any(div => div.Attributes["class"].Value.Contains("govuk-body") &&
                  div.InnerText.Contains(courseDetailsViewModel.CourseDetails.EquipmentRequired)).Should().BeTrue();
            }
            else
            {
                htmlDocument.DocumentNode.Descendants("p")
                .Any(div => div.Attributes["class"].Value.Contains("govuk-body") &&
                 div.InnerText.Contains(courseDetailsViewModel.NoEquipmentRequiredMessage)).Should().BeTrue();
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
            courseDetailsViewModel.CourseDetails.AssessmentMethod = propertyValue;
            courseDetailsViewModel.NoAssessmentMethodAvailableMessage = nameof(CourseDetailsViewModel.NoAssessmentMethodAvailableMessage);

            // Act
            var htmlDocument = courseDetailsCourseDetails.RenderAsHtml(courseDetailsViewModel);

            // Assert
            if (!string.IsNullOrWhiteSpace(propertyValue))
            {
                htmlDocument.DocumentNode.Descendants("p")
                 .Any(div => div.Attributes["class"].Value.Contains("govuk-body") &&
                  div.InnerText.Contains(courseDetailsViewModel.CourseDetails.AssessmentMethod)).Should().BeTrue();
            }
            else
            {
                htmlDocument.DocumentNode.Descendants("p")
                .Any(div => div.Attributes["class"].Value.Contains("govuk-body") &&
                 div.InnerText.Contains(courseDetailsViewModel.NoAssessmentMethodAvailableMessage)).Should().BeTrue();
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
            courseDetailsViewModel.CourseDetails.VenueDetails = new Venue();
            if (venueExists)
            {
                courseDetailsViewModel.CourseDetails.VenueDetails.VenueName = nameof(CourseDetails.VenueDetails.VenueName);
                courseDetailsViewModel.CourseDetails.VenueDetails.Location.AddressLine1 = nameof(CourseDetails.VenueDetails.Location.AddressLine1);
                courseDetailsViewModel.CourseDetails.VenueDetails.Location.AddressLine2 = nameof(CourseDetails.VenueDetails.Location.AddressLine2);
                courseDetailsViewModel.CourseDetails.VenueDetails.Location.County = nameof(CourseDetails.VenueDetails.Location.County);
                courseDetailsViewModel.CourseDetails.VenueDetails.Location.Postcode = nameof(CourseDetails.VenueDetails.Location.Postcode);
                courseDetailsViewModel.CourseDetails.VenueDetails.Website = nameof(CourseDetails.VenueDetails.Website);
                courseDetailsViewModel.CourseDetails.VenueDetails.EmailAddress = nameof(CourseDetails.VenueDetails.EmailAddress);
                courseDetailsViewModel.CourseDetails.VenueDetails.PhoneNumber = nameof(CourseDetails.VenueDetails.PhoneNumber);
                courseDetailsViewModel.CourseDetails.VenueDetails.Fax = nameof(CourseDetails.VenueDetails.Fax);
            }
            else
            {
                courseDetailsViewModel.CourseDetails.VenueDetails = null;
                courseDetailsViewModel.NoVenueAvailableMessage = nameof(CourseDetailsViewModel.NoVenueAvailableMessage);
            }

            // Act
            var htmlDocument = courseDetailsCourseDetails.RenderAsHtml(courseDetailsViewModel);

            // Assert
            if (venueExists)
            {
                htmlDocument.DocumentNode.Descendants("td")
                 .Any(div => div.Attributes["class"].Value.Contains("govuk-table__cell") &&
                  div.InnerText.Contains(courseDetailsViewModel.CourseDetails.VenueDetails.VenueName)).Should().BeTrue();

                htmlDocument.DocumentNode.Descendants("td")
              .Any(div => div.Attributes["class"].Value.Contains("govuk-table__cell") &&
               div.InnerText.Contains(courseDetailsViewModel.CourseDetails.VenueDetails.Location.AddressLine1)).Should().BeTrue();
                htmlDocument.DocumentNode.Descendants("td")
            .Any(div => div.Attributes["class"].Value.Contains("govuk-table__cell") &&
             div.InnerText.Contains(courseDetailsViewModel.CourseDetails.VenueDetails.Location.AddressLine2)).Should().BeTrue();
                htmlDocument.DocumentNode.Descendants("td")
            .Any(div => div.Attributes["class"].Value.Contains("govuk-table__cell") &&
             div.InnerText.Contains(courseDetailsViewModel.CourseDetails.VenueDetails.Location.Postcode)).Should().BeTrue();
                htmlDocument.DocumentNode.Descendants("td")
            .Any(div => div.Attributes["class"].Value.Contains("govuk-table__cell") &&
             div.InnerText.Contains(courseDetailsViewModel.CourseDetails.VenueDetails.Location.County)).Should().BeTrue();
            }
            else
            {
                htmlDocument.DocumentNode.Descendants("td")
                .Any(div => div.Attributes["class"].Value.Contains("govuk-table__cell") &&
                 div.InnerText.Contains(courseDetailsViewModel.NoVenueAvailableMessage)).Should().BeTrue();
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
            courseDetailsViewModel.CourseDetails.VenueDetails = new Venue();
            if (otherVenuesExists)
            {
                courseDetailsViewModel.CourseDetails.Oppurtunities = new System.Collections.Generic.List<Oppurtunity>();
                var otherDatesAndVenues = new Oppurtunity();
                courseDetailsViewModel.CourseDetails.Oppurtunities.Add(otherDatesAndVenues);
                courseDetailsViewModel.CourseDetails.Oppurtunities.FirstOrDefault().VenueName = nameof(CourseDetails.Oppurtunities);
            }
            else
            {
                courseDetailsViewModel.CourseDetails.Oppurtunities = null;
                courseDetailsViewModel.NoOtherDateOrVenueAvailableMessage = nameof(CourseDetailsViewModel.NoOtherDateOrVenueAvailableMessage);
            }

            // Act
            var htmlDocument = courseDetailsCourseDetails.RenderAsHtml(courseDetailsViewModel);

            // Assert
            if (otherVenuesExists)
            {
                htmlDocument.DocumentNode.Descendants("td")
                 .Any(div => div.Attributes["class"].Value.Contains("govuk-table__cell") &&
                  div.InnerText.Contains(courseDetailsViewModel.CourseDetails.Oppurtunities.FirstOrDefault().VenueName)).Should().BeTrue();
            }
            else
            {
                htmlDocument.DocumentNode.Descendants("td")
                .Any(div => div.Attributes["class"].Value.Contains("govuk-table__cell") &&
                 div.InnerText.Contains(courseDetailsViewModel.NoOtherDateOrVenueAvailableMessage)).Should().BeTrue();
            }
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void ShowProviderDetailsNameTest(bool providerDetailsExist)
        {
            // Arrange
            var courseDetailsProviderDetails = new _MVC_Views_CourseDetails_Provider_cshtml();
            var providerDetails = new ProviderDetails();
            var courseDetailsViewModel = new CourseDetailsViewModel();
            courseDetailsViewModel.CourseDetails.ProviderDetails = providerDetails;
            if (providerDetailsExist)
            {
                courseDetailsViewModel.CourseDetails.ProviderDetails.Name = nameof(CourseDetails.ProviderDetails.Name);
            }
            else
            {
                courseDetailsViewModel.CourseDetails.ProviderDetails.Name = null;
            }

            // Act
            var htmlDocument = courseDetailsProviderDetails.RenderAsHtml(courseDetailsViewModel);

            // Assert
            if (providerDetailsExist)
            {
                htmlDocument.DocumentNode.Descendants("p")
                 .Any(p => p.Attributes["class"].Value.Contains("govuk-body-lead") &&
                  p.InnerText.Contains(courseDetailsViewModel.CourseDetails.ProviderDetails.Name)).Should().BeTrue();
            }
            else
            {
                htmlDocument.DocumentNode.Descendants("p")
                 .Any(p => p.Attributes["class"].Value.Contains("govuk-body-lead") &&
                  p.InnerText.Contains(courseDetailsViewModel.CourseDetails.ProviderDetails.Name)).Should().BeFalse();
            }
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void ShowProviderDetailsEmailAddressTest(bool providerDetailsExist)
        {
            // Arrange
            var courseDetailsProviderDetails = new _MVC_Views_CourseDetails_Provider_cshtml();
            var providerDetails = new ProviderDetails();
            var courseDetailsViewModel = new CourseDetailsViewModel();
            courseDetailsViewModel.CourseDetails.ProviderDetails = providerDetails;
            if (providerDetailsExist)
            {
                courseDetailsViewModel.CourseDetails.ProviderDetails.EmailAddress = nameof(CourseDetails.ProviderDetails.EmailAddress);
            }

            // Act
            var htmlDocument = courseDetailsProviderDetails.RenderAsHtml(courseDetailsViewModel);

            // Assert
            if (providerDetailsExist)
            {
                htmlDocument.DocumentNode.Descendants("a")
                 .Any(a => a.Attributes["class"].Value.Contains("govuk-link") &&
                  a.InnerText.Contains(courseDetailsViewModel.CourseDetails.ProviderDetails.EmailAddress)).Should().BeTrue();
            }
            else
            {
                htmlDocument.DocumentNode.Descendants("a")
                 .Any(a => a.Attributes["class"].Value.Contains("govuk-link") &&
                  a.InnerText.Contains(courseDetailsViewModel.CourseDetails.ProviderDetails.EmailAddress)).Should().BeFalse();
            }
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void ShowProviderDetailsWebsiteTest(bool providerDetailsExist)
        {
            // Arrange
            var courseDetailsProviderDetails = new _MVC_Views_CourseDetails_Provider_cshtml();
            var providerDetails = new ProviderDetails();
            var courseDetailsViewModel = new CourseDetailsViewModel();
            courseDetailsViewModel.CourseDetails.ProviderDetails = providerDetails;
            if (providerDetailsExist)
            {
                courseDetailsViewModel.CourseDetails.ProviderDetails.Website = nameof(CourseDetails.ProviderDetails.Website);
            }
            else
            {
                courseDetailsViewModel.CourseDetails.ProviderDetails.Website = null;
            }

            // Act
            var htmlDocument = courseDetailsProviderDetails.RenderAsHtml(courseDetailsViewModel);

            // Assert
            if (providerDetailsExist)
            {
                htmlDocument.DocumentNode.Descendants("li")
                 .Any(li => li.InnerText.Contains(courseDetailsViewModel.CourseDetails.ProviderDetails.Website)).Should().BeTrue();
            }
            else
            {
                htmlDocument.DocumentNode.Descendants("li")
                 .Any(li => li.InnerText.Contains(courseDetailsViewModel.CourseDetails.ProviderDetails.Website)).Should().BeFalse();
            }
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void ShowProviderDetailsPhoneNumberTest(bool providerDetailsExist)
        {
            // Arrange
            var courseDetailsProviderDetails = new _MVC_Views_CourseDetails_Provider_cshtml();
            var providerDetails = new ProviderDetails();
            var courseDetailsViewModel = new CourseDetailsViewModel();
            courseDetailsViewModel.CourseDetails.ProviderDetails = providerDetails;
            if (providerDetailsExist)
            {
                courseDetailsViewModel.CourseDetails.ProviderDetails.PhoneNumber = nameof(CourseDetails.ProviderDetails.PhoneNumber);
            }
            else
            {
                courseDetailsViewModel.CourseDetails.ProviderDetails.PhoneNumber = null;
            }

            // Act
            var htmlDocument = courseDetailsProviderDetails.RenderAsHtml(courseDetailsViewModel);

            // Assert
            if (providerDetailsExist)
            {
                htmlDocument.DocumentNode.Descendants("li")
                 .Any(li => li.InnerText.Contains(courseDetailsViewModel.CourseDetails.ProviderDetails.PhoneNumber)).Should().BeTrue();
            }
            else
            {
                htmlDocument.DocumentNode.Descendants("li")
                 .Any(li => li.InnerText.Contains(courseDetailsViewModel.CourseDetails.ProviderDetails.PhoneNumber)).Should().BeFalse();
            }
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void ShowHideBackToResultsTest(bool referralUrlExists)
        {
            // Arrange
            var courseDetailsHtml = new _MVC_Views_CourseDetails_CourseDetails_cshtml();
            var courseDetailsViewModel = new CourseDetailsViewModel();
            var courseDetails = new CourseDetails();
            if (referralUrlExists)
            {
                courseDetailsViewModel.ReferralPath = nameof(CourseDetailsViewModel.ReferralPath);
            }
            else
            {
                courseDetailsViewModel.ReferralPath = null;
            }

            // Act
            var htmlDocument = courseDetailsHtml.RenderAsHtml(courseDetailsViewModel);

            // Assert
            if (referralUrlExists)
            {
                htmlDocument.DocumentNode.Descendants("a")
                 .Any(a => a.Attributes["class"].Value.Contains("govuk-back-link") &&
                  a.Attributes["href"].Value.Contains(courseDetailsViewModel.ReferralPath)).Should().BeTrue();
            }
            else
            {
                htmlDocument.DocumentNode.Descendants("a")
                .Any(a => a.Attributes["class"].Value.Contains("govuk-back-link") &&
                a.Attributes["href"].Value.Contains(courseDetailsViewModel.ReferralPath)).Should().BeFalse();
            }
        }

        [Theory]
        [InlineData(true, true)]
        [InlineData(false, false)]
        [InlineData(true, false)]
        [InlineData(false, true)]
        public void ShowHideLearnerAndEmployerSatisfationTest(bool learnerSatisfactionSpecified, bool employeeSatisfactionSpecified)
        {
            // Arrange
            var courseDetailsProviderDetails = new _MVC_Views_CourseDetails_Provider_cshtml();
            var courseDetailsViewModel = new CourseDetailsViewModel();
            var providerDetails = new ProviderDetails
            {
                LearnerSatisfactionSpecified = learnerSatisfactionSpecified,
                EmployerSatisfactionSpecified = employeeSatisfactionSpecified
            };

            providerDetails.Name = nameof(courseDetailsViewModel.CourseDetails.ProviderDetails.Name);
            courseDetailsViewModel.CourseDetails.ProviderDetails = providerDetails;
            var dummyLearnerSatisfaction = 1.0;
            var dummyEmployeeSatisfaction = 1.0;

            if (learnerSatisfactionSpecified && employeeSatisfactionSpecified)
            {
                courseDetailsViewModel.CourseDetails.ProviderDetails.LearnerSatisfaction = dummyLearnerSatisfaction;
                courseDetailsViewModel.CourseDetails.ProviderDetails.EmployerSatisfaction = dummyEmployeeSatisfaction;
            }
            else if (learnerSatisfactionSpecified && !employeeSatisfactionSpecified)
            {
                courseDetailsViewModel.CourseDetails.ProviderDetails.LearnerSatisfaction = dummyLearnerSatisfaction;
            }
            else if (!learnerSatisfactionSpecified && employeeSatisfactionSpecified)
            {
                courseDetailsViewModel.CourseDetails.ProviderDetails.EmployerSatisfaction = dummyEmployeeSatisfaction;
            }

            // Act
            var htmlDocument = courseDetailsProviderDetails.RenderAsHtml(courseDetailsViewModel);

            // Assert
            if (learnerSatisfactionSpecified && employeeSatisfactionSpecified)
            {
                htmlDocument.DocumentNode.Descendants("p")
                 .Any(p => p.Attributes["class"].Value.Contains("govuk-body govuk-!-font-size-48 govuk-!-font-weight-bold govuk-!-margin-bottom-2") &&
                  p.InnerText.Contains(courseDetailsViewModel.CourseDetails.ProviderDetails.LearnerSatisfaction.ToString())).Should().BeTrue();

                htmlDocument.DocumentNode.Descendants("p")
                .Any(p => p.Attributes["class"].Value.Contains("govuk-body govuk-!-font-size-48 govuk-!-font-weight-bold govuk-!-margin-bottom-2") &&
                 p.InnerText.Contains(courseDetailsViewModel.CourseDetails.ProviderDetails.EmployerSatisfaction.ToString())).Should().BeTrue();
            }
            else if (learnerSatisfactionSpecified && !employeeSatisfactionSpecified)
            {
                htmlDocument.DocumentNode.Descendants("p")
                 .Any(p => p.Attributes["class"].Value.Contains("govuk-body govuk-!-font-size-48 govuk-!-font-weight-bold govuk-!-margin-bottom-2") &&
                  p.InnerText.Contains(courseDetailsViewModel.CourseDetails.ProviderDetails.LearnerSatisfaction.ToString())).Should().BeTrue();

                htmlDocument.DocumentNode.Descendants("p")
                .Any(p => p.Attributes["class"].Value.Contains("govuk-body govuk-!-font-size-48 govuk-!-font-weight-bold govuk-!-margin-bottom-2") &&
                 p.InnerText.Contains(courseDetailsViewModel.CourseDetails.ProviderDetails.EmployerSatisfaction.ToString())).Should().BeFalse();
            }
            else if (!learnerSatisfactionSpecified && employeeSatisfactionSpecified)
            {
                htmlDocument.DocumentNode.Descendants("p")
                .Any(p => p.Attributes["class"].Value.Contains("govuk-body govuk-!-font-size-48 govuk-!-font-weight-bold govuk-!-margin-bottom-2") &&
                 p.InnerText.Contains(courseDetailsViewModel.CourseDetails.ProviderDetails.LearnerSatisfaction.ToString())).Should().BeFalse();

                htmlDocument.DocumentNode.Descendants("p")
                .Any(p => p.Attributes["class"].Value.Contains("govuk-body govuk-!-font-size-48 govuk-!-font-weight-bold govuk-!-margin-bottom-2") &&
                 p.InnerText.Contains(courseDetailsViewModel.CourseDetails.ProviderDetails.EmployerSatisfaction.ToString())).Should().BeTrue();
            }
            else if (!learnerSatisfactionSpecified && !employeeSatisfactionSpecified)
            {
                htmlDocument.DocumentNode.Descendants("p")
                .Any(p => p.Attributes["class"].Value.Contains("govuk-body govuk-!-font-size-48 govuk-!-font-weight-bold govuk-!-margin-bottom-2") &&
                 p.InnerText.Contains(courseDetailsViewModel.CourseDetails.ProviderDetails.LearnerSatisfaction.ToString())).Should().BeFalse();

                htmlDocument.DocumentNode.Descendants("p")
                .Any(p => p.Attributes["class"].Value.Contains("govuk-body govuk-!-font-size-48 govuk-!-font-weight-bold govuk-!-margin-bottom-2") &&
                 p.InnerText.Contains(courseDetailsViewModel.CourseDetails.ProviderDetails.EmployerSatisfaction.ToString())).Should().BeFalse();
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