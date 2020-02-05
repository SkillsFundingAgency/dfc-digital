using ASP;
using DFC.Digital.Data.Model;
using DFC.Digital.Web.Sitefinity.CourseModule.UnitTests.Helpers;
using FluentAssertions;
using HtmlAgilityPack;
using RazorGenerator.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace DFC.Digital.Web.Sitefinity.CourseModule.UnitTests
{
    public class CourseDetailsViewTests : MemberDataHelper
    {
        [Fact]
        public void Dfc7056CourseDetailsIndexViewTests()
        {
            // Arrange
            var courseDetailsIndex = new _MVC_Views_CourseDetails_Index_cshtml();
            var courseDetailsViewModel = new CourseDetailsViewModel
            {
                CourseDetails =
                {
                    Title = nameof(CourseDetailsViewModel.CourseDetails.Title)
                }
            };

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
            AssertTagInnerTextValue(htmlDocument, model.CourseDetails.Title, "h1");
            this.AssertTableCounts(htmlDocument, 1);
            this.AssertH2HeadingCounts(htmlDocument, 9);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void ShowCourseDescriptionTest(bool hasValue)
        {
            // Arrange
            var courseDetailsCourseDetails = new _MVC_Views_CourseDetails_CourseDetails_cshtml();

            var courseDetailsViewModel = new CourseDetailsViewModel();
            courseDetailsViewModel.CourseDetails = new CourseDetails();
            courseDetailsViewModel.NoCourseDescriptionMessage = nameof(courseDetailsViewModel.NoCourseDescriptionMessage);

            courseDetailsViewModel.CourseDetails.Description = hasValue
                ? nameof(courseDetailsViewModel.CourseDetails.Description)
                : nameof(courseDetailsViewModel.NoCourseDescriptionMessage);

            // Act
            var htmlDocument = courseDetailsCourseDetails.RenderAsHtml(courseDetailsViewModel);

            // Assert
            if (hasValue)
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
        [InlineData(true)]
        [InlineData(false)]
        public void ShowEntryRequirementsTest(bool hasValue)
        {
            // Arrange
            var courseDetailsCourseDetails = new _MVC_Views_CourseDetails_CourseDetails_cshtml();
            var courseDetailsViewModel = new CourseDetailsViewModel();
            courseDetailsViewModel.CourseDetails = new CourseDetails();
            if (hasValue)
            {
                courseDetailsViewModel.CourseDetails.EntryRequirements = nameof(CourseDetailsViewModel.CourseDetails.EntryRequirements);
            }
            else
            {
                courseDetailsViewModel.NoEntryRequirementsAvailableMessage = nameof(CourseDetailsViewModel.NoEntryRequirementsAvailableMessage);
            }

            // Act
            var htmlDocument = courseDetailsCourseDetails.RenderAsHtml(courseDetailsViewModel);

            // Assert
            if (hasValue)
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
        [InlineData(true)]
        [InlineData(false)]
        public void ShowEquipmentRequiredTest(bool hasValue)
        {
            // Arrange
            var courseDetailsCourseDetails = new _MVC_Views_CourseDetails_CourseDetails_cshtml();
            var courseDetailsViewModel = new CourseDetailsViewModel();
            courseDetailsViewModel.CourseDetails = new CourseDetails();
            if (hasValue)
            {
                courseDetailsViewModel.CourseDetails.EquipmentRequired = nameof(CourseDetailsViewModel.CourseDetails.EquipmentRequired);
            }
            else
            {
                courseDetailsViewModel.NoEquipmentRequiredMessage = nameof(CourseDetailsViewModel.NoEquipmentRequiredMessage);
            }

            // Act
            var htmlDocument = courseDetailsCourseDetails.RenderAsHtml(courseDetailsViewModel);

            // Assert
            if (hasValue)
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
        [InlineData(true)]
        [InlineData(false)]
        public void ShowAssessmentMethodTest(bool hasValue)
        {
            // Arrange
            var courseDetailsCourseDetails = new _MVC_Views_CourseDetails_CourseDetails_cshtml();
            var courseDetailsViewModel = new CourseDetailsViewModel();
            courseDetailsViewModel.CourseDetails = new CourseDetails();
            if (hasValue)
            {
                courseDetailsViewModel.CourseDetails.AssessmentMethod = nameof(CourseDetailsViewModel.CourseDetails.AssessmentMethod);
            }
            else
            {
                courseDetailsViewModel.NoAssessmentMethodAvailableMessage = nameof(CourseDetailsViewModel.NoAssessmentMethodAvailableMessage);
            }

            // Act
            var htmlDocument = courseDetailsCourseDetails.RenderAsHtml(courseDetailsViewModel);

            // Assert
            if (hasValue)
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
            courseDetailsViewModel.CourseDetails = new CourseDetails();

            if (venueExists)
            {
                courseDetailsViewModel.CourseDetails.VenueDetails = new Venue
                {
                    VenueName = nameof(CourseDetails.VenueDetails.VenueName),
                    Location = new Address
                    {
                        AddressLine1 = nameof(CourseDetails.VenueDetails.Location.AddressLine1),
                        AddressLine2 = nameof(CourseDetails.VenueDetails.Location.AddressLine2),
                        Town = nameof(CourseDetails.VenueDetails.Location.Town),
                        County = nameof(CourseDetails.VenueDetails.Location.County),
                        Postcode = nameof(CourseDetails.VenueDetails.Location.Postcode)
                    },
                    Website = nameof(CourseDetails.VenueDetails.Website),
                    EmailAddress = nameof(CourseDetails.VenueDetails.EmailAddress),
                    PhoneNumber = nameof(CourseDetails.VenueDetails.PhoneNumber),
                    Facilities = nameof(CourseDetails.VenueDetails.Facilities),
                    Fax = nameof(CourseDetails.VenueDetails.Fax)
                };
            }
            else
            {
                courseDetailsViewModel.CourseDetails.VenueDetails = null;
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
             div.InnerText.Contains(courseDetailsViewModel.CourseDetails.VenueDetails.Location.Town)).Should().BeTrue();
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
                   div.InnerText.Contains(nameof(courseDetailsViewModel.CourseDetails.VenueDetails.VenueName))).Should().BeFalse();
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
        public void OtherVenuesStartdatesTest(bool startDateIsNull)
        {
            //Arrange
            var startDate = DateTime.Now;

            var courseDetailsCourseDetails = new _MVC_Views_CourseDetails_OtherDatesAndVenues_cshtml();
            var courseDetailsViewModel = new CourseDetailsViewModel();
            var courseDetails = new CourseDetails();

            courseDetailsViewModel.CourseDetails.VenueDetails = new Venue();
            courseDetailsViewModel.CourseDetails.Oppurtunities = new System.Collections.Generic.List<Oppurtunity>();
            var otherDatesAndVenues = new Oppurtunity();
            if (!startDateIsNull)
            {
                otherDatesAndVenues.StartDate = startDate;
            }

            courseDetailsViewModel.CourseDetails.Oppurtunities.Add(otherDatesAndVenues);

            // Act
            var htmlDocument = courseDetailsCourseDetails.RenderAsHtml(courseDetailsViewModel);

            var expectedStartDate = $"{string.Format("{0:dd MMMM yyyy}", startDate)}";
            var dateCell = htmlDocument.DocumentNode.SelectNodes("//table[1]/tbody/tr[2]/td[2]").FirstOrDefault().InnerText;

            // Assert
          if (startDateIsNull)
            {
               dateCell.Should().NotContain(expectedStartDate);
            }
            else
            {
                dateCell.Should().Contain(expectedStartDate);
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
                  p.InnerText.Contains(nameof(courseDetailsViewModel.CourseDetails.ProviderDetails.Name))).Should().BeFalse();
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

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void Dfc9563NoOtherDatesTextTests(bool otherDatesAvailable)
        {
            // Arrange
            var otherDatesView = new _MVC_Views_CourseDetails_OtherDatesAndVenues_cshtml();
            var viewModel = new CourseDetailsViewModel
            {
                NoOtherDateOrVenueAvailableMessage = nameof(CourseDetailsViewModel.NoOtherDateOrVenueAvailableMessage),
                CourseDetails =
                {
                    VenueDetails = otherDatesAvailable ? new Venue() : null,
                    Oppurtunities = otherDatesAvailable
                        ? new List<Oppurtunity>
                        {
                            new Oppurtunity
                            {
                                StartDate = DateTime.Now,
                                OppurtunityId = nameof(Oppurtunity.OppurtunityId),
                                VenueName = nameof(Oppurtunity.VenueName), VenueUrl = nameof(Oppurtunity.VenueName)
                            }
                        }
                        : Enumerable.Empty<Oppurtunity>().ToList()
                }
            };

            // Act
            var htmlDocument = otherDatesView.RenderAsHtml(viewModel);

            // Assert
            if (otherDatesAvailable)
            {
                AssertTagInnerTextValueDoesNotExist(htmlDocument, viewModel.NoOtherDateOrVenueAvailableMessage, "td");
            }
            else
            {
                AssertTagInnerTextValue(htmlDocument, viewModel.NoOtherDateOrVenueAvailableMessage, "td");
            }
        }

        [Theory]
        [MemberData(nameof(Dfc9560MissingFieldsTestInput))]
        public void Dfc9560MissingFieldsTest(CourseDetailsViewModel courseDetailsViewModel, CourseDetails courseDetails)
        {
            //Arrange
            courseDetailsViewModel.CourseDetails = courseDetails;
            var detailsView = new _MVC_Views_CourseDetails_CourseDetails_cshtml();

            //Act
            var htmlDocument = detailsView.RenderAsHtml(courseDetailsViewModel);

            //Asserts
            if (!string.IsNullOrWhiteSpace(courseDetails.AttendancePattern))
            {
                AssertTagInnerTextValue(htmlDocument, courseDetailsViewModel.AttendancePatternLabel, "th");
                AssertTagInnerTextValue(htmlDocument, courseDetails.AttendancePattern, "td");
            }

            if (!string.IsNullOrWhiteSpace(courseDetails.AwardingOrganisation))
            {
                AssertTagInnerTextValue(htmlDocument, courseDetailsViewModel.AwardingOrganisationLabel, "th");
                AssertTagInnerTextValue(htmlDocument, courseDetails.AwardingOrganisation, "td");
            }

            if (!string.IsNullOrWhiteSpace(courseDetails.CourseWebpageLink))
            {
                AssertTagInnerTextValue(htmlDocument, courseDetailsViewModel.CourseWebpageLinkLabel, "th");
                AssertElementExistsByAttributeAndValue(htmlDocument, "a", "href", courseDetails.CourseWebpageLink);
            }

            if (!string.IsNullOrWhiteSpace(courseDetails.StudyMode))
            {
                AssertTagInnerTextValue(htmlDocument, courseDetailsViewModel.CourseTypeLabel, "th");
                AssertTagInnerTextValue(htmlDocument, courseDetails.StudyMode, "td");
            }

            if (!string.IsNullOrWhiteSpace(courseDetails.AdditionalPrice))
            {
                AssertTagInnerTextValue(htmlDocument, courseDetailsViewModel.AdditionalPriceLabel, "th");
                AssertTagInnerTextValue(htmlDocument, courseDetails.AdditionalPrice, "td");
            }

            if (!string.IsNullOrWhiteSpace(courseDetails.QualificationName))
            {
                AssertTagInnerTextValue(htmlDocument, courseDetailsViewModel.QualificationNameLabel, "th");
                AssertTagInnerTextValue(htmlDocument, courseDetails.QualificationName, "td");
            }

            if (!string.IsNullOrWhiteSpace(courseDetails.QualificationLevel))
            {
                AssertTagInnerTextValue(htmlDocument, courseDetailsViewModel.QualificationLevelLabel, "th");
                AssertTagInnerTextValue(htmlDocument, courseDetails.QualificationLevel, "td");
            }

            if (!string.IsNullOrWhiteSpace(courseDetails.Cost))
            {
                AssertTagInnerTextValue(htmlDocument, courseDetailsViewModel.PriceLabel, "th");
                AssertTagInnerTextValue(htmlDocument, courseDetails.Cost, "td");
            }
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
    }
}
