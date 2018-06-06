using ASP;
using DFC.Digital.Data.Model;
using DFC.Digital.Web.Sitefinity.JobProfileModule.Mvc.Models;
using FluentAssertions;
using FluentAssertions.Common;
using HtmlAgilityPack;
using RazorGenerator.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace DFC.Digital.Web.Sitefinity.JobProfileModule.UnitTests.Views
{
    public class JobProfileHowToBecomeViewTests
    {
        [Theory]
        [InlineData("data", "", "", true)]
        [InlineData("data", "test", "", false)]
        [InlineData("data", "", "test", true)]
        [InlineData("data", "test", "", true)]
        [InlineData("", "", "", false)]
        public void Dfc2857MoreinfomationViewTests(string careerTips, string professionalAndIndustryBodies, string furtherInformation, bool validRegistrations)
        {
            // Arrange
            var moreInformationView = new _MVC_Views_JobProfileHowToBecome_MoreInformation_cshtml();
            var jobProfileHowToBecomeView = new JobProfileHowToBecomeViewModel
            {
                HowToBecome = new HowToBecome
                {
                    FurtherInformation = new MoreInformation
                    {
                        CareerTips = careerTips,
                        ProfessionalAndIndustryBodies = professionalAndIndustryBodies,
                        FurtherInformation = furtherInformation
                    },
                    Registrations = GetRegistrations(validRegistrations)
                }
            };

            // Act
            var htmlDocument = moreInformationView.RenderAsHtml(jobProfileHowToBecomeView);

            // Assert
            if (string.IsNullOrWhiteSpace(careerTips) && string.IsNullOrWhiteSpace(professionalAndIndustryBodies) &&
                string.IsNullOrWhiteSpace(furtherInformation) && !validRegistrations)
            {
                AssertViewIsEmpty(htmlDocument);
            }

            if (!string.IsNullOrWhiteSpace(careerTips))
            {
                AssertContentExistsInView(careerTips, htmlDocument);
            }

            if (!string.IsNullOrWhiteSpace(professionalAndIndustryBodies))
            {
                AssertContentExistsInView(professionalAndIndustryBodies, htmlDocument);
            }

            if (!string.IsNullOrWhiteSpace(furtherInformation))
            {
                AssertContentExistsInView(furtherInformation, htmlDocument);
            }

            if (validRegistrations)
            {
                htmlDocument.DocumentNode.Descendants("li").Count().Should().IsSameOrEqualTo(jobProfileHowToBecomeView.HowToBecome.Registrations.Count());
            }
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void Dfc2857RestrictionsViewTests(bool validRestrictions)
        {
            // Arrange
            var restrictionsView = new _MVC_Views_JobProfileHowToBecome_Restrictions_cshtml();
            var jobProfileHowToBecomeView = new JobProfileHowToBecomeViewModel
            {
                HowToBecome = new HowToBecome
                {
                    Restrictions = GetRestrictions(validRestrictions)
                }
            };

            // Act
            var htmlDocument = restrictionsView.RenderAsHtml(jobProfileHowToBecomeView);

            // Assert
            if (validRestrictions)
            {
                htmlDocument.DocumentNode.Descendants("li").Count().Should().IsSameOrEqualTo(jobProfileHowToBecomeView.HowToBecome.Restrictions.Count());
            }
            else
            {
                ContentDisplayedBySectionId(htmlDocument, "restrictions").Should().BeFalse();
            }
        }

        [Theory]
        [InlineData("test")]
        [InlineData("content")]
        public void Dfc2857IndexViewTests(string content)
        {
            // Arrange
            var indexView = new _MVC_Views_JobProfileHowToBecome_Index_cshtml();
            var jobProfileHowToBecomeView = new JobProfileHowToBecomeViewModel
            {
                HowToBecomeText = content,
                HowToBecome = new HowToBecome()
            };

            // Act
            var htmlDocument = indexView.RenderAsHtml(jobProfileHowToBecomeView);

            // Assert
            AssertContentExistsInView(content, htmlDocument);
        }

        [Theory]
        [InlineData(true, true, RouteEntryType.Apprenticeship, "subjects", "furtherinformation", "routeRequirement")]
        [InlineData(true, true, RouteEntryType.College, "subjects", "furtherinformation", "routeRequirement")]
        [InlineData(true, true, RouteEntryType.University, "subjects", "furtherinformation", "routeRequirement")]
        [InlineData(false, true, RouteEntryType.Apprenticeship, "subjects", "furtherinformation", "routeRequirement")]
        [InlineData(true, false, RouteEntryType.Apprenticeship, "subjects", "furtherinformation", "routeRequirement")]
        [InlineData(false, false, RouteEntryType.Apprenticeship, "", "", "")]
        public void Dfc2857RouteEntryViewTests(bool entryReqs, bool moreInfoLinks, RouteEntryType routeEntryType, string subjects, string furtherinformation, string routeRequirement)
        {
            // Arrange
            var restrictionsView = new _MVC_Views_JobProfileHowToBecome_RouteEntry_cshtml();
            var routeEntryVm = new RouteEntry
            {
               EntryRequirements = GetEntryRequirements(entryReqs),
               MoreInformationLinks = GetInformationLinks(moreInfoLinks),
               RouteName = routeEntryType,
               RouteSubjects = subjects,
               FurtherRouteInformation = furtherinformation,
               RouteRequirement = routeRequirement
            };

            // Act
            var htmlDocument = restrictionsView.RenderAsHtml(routeEntryVm);

            // Assert
            if (string.IsNullOrWhiteSpace(subjects) && string.IsNullOrWhiteSpace(furtherinformation) && string.IsNullOrWhiteSpace(routeRequirement) && !entryReqs && !moreInfoLinks)
            {
                AssertViewIsEmpty(htmlDocument);
            }

            if (!string.IsNullOrWhiteSpace(subjects))
            {
                AssertContentExistsInView(subjects, htmlDocument);
            }

            if (!string.IsNullOrWhiteSpace(furtherinformation))
            {
                AssertContentExistsInView(furtherinformation, htmlDocument);
            }

            if (!string.IsNullOrWhiteSpace(routeRequirement) && entryReqs)
            {
                AssertContentExistsInView(routeRequirement, htmlDocument);
            }

            if (entryReqs)
            {
                GetItemCountByUlClass("list-reqs", htmlDocument).Should().IsSameOrEqualTo(routeEntryVm.MoreInformationLinks.Count());
            }

            // Assert
            if (moreInfoLinks)
            {
                GetItemCountByUlClass("list-link", htmlDocument).Should().IsSameOrEqualTo(routeEntryVm.MoreInformationLinks.Count());
            }
        }

        [Theory]
        [InlineData("data", "", "", "test", "test")]
        [InlineData("data", "test", "", "", "")]
        [InlineData("data", "", "test", "test", "test")]
        [InlineData("data", "test", "", "", "test")]
        [InlineData("", "", "", "", "")]
        public void Dfc2857ExtrainfomationViewTests(string work, string directApplication, string otherRequirements, string otherRoutes, string volunteering)
        {
            // Arrange
            var extraInformationView = new _MVC_Views_JobProfileHowToBecome_FurtherRoutes_cshtml();
            var jobProfileHowToBecomeViewModel = new JobProfileHowToBecomeViewModel
            {
                HowToBecome = new HowToBecome
                {
                    FurtherRoutes = new FurtherRoutes
                    {
                        Work = work,
                        DirectApplication = directApplication,
                        OtherRoutes = otherRoutes,
                        Volunteering = volunteering
                    },
                    OtherRequirements = otherRequirements
                }
            };

            // Act
            var htmlDocument = extraInformationView.RenderAsHtml(jobProfileHowToBecomeViewModel);

            // Assert
            if (!string.IsNullOrWhiteSpace(work))
            {
                ContentDisplayedBySectionId(htmlDocument, "work").Should().BeTrue();
            }

            if (!string.IsNullOrWhiteSpace(directApplication))
            {
                ContentDisplayedBySectionId(htmlDocument, "directapplication").Should().BeTrue();
            }

            if (!string.IsNullOrWhiteSpace(otherRequirements))
            {
                ContentDisplayedBySectionId(htmlDocument, "otherrequirements").Should().BeTrue();
            }

            if (!string.IsNullOrWhiteSpace(volunteering))
            {
                ContentDisplayedBySectionId(htmlDocument, "volunteering").Should().BeTrue();
            }

            if (!string.IsNullOrWhiteSpace(otherRoutes))
            {
                ContentDisplayedBySectionId(htmlDocument, "otherroutes").Should().BeTrue();
            }
        }

        private static void AssertContentExistsInView(string content, HtmlDocument htmlDocument)
        {
            htmlDocument.DocumentNode.InnerHtml.IndexOf(content, StringComparison.OrdinalIgnoreCase).Should().BeGreaterThan(-1);
        }

        private static void AssertViewIsEmpty(HtmlDocument htmlDocument)
        {
            htmlDocument.DocumentNode.Descendants().Count().Should().Be(0);
        }

        private bool ContentDisplayedBySectionId(HtmlDocument htmlDocument, string sectionId)
        {
            return !string.IsNullOrWhiteSpace(htmlDocument.DocumentNode.Descendants("section")?.FirstOrDefault(sec => sec.Id.Equals(sectionId))
                ?.InnerHtml);
        }

        private IEnumerable<Registration> GetRegistrations(bool validRegistrations)
        {
            return validRegistrations
                ? new List<Registration>
                {
                    new Registration { Info = nameof(Registration.Info), Title = nameof(Registration.Title) }
                }
                : new List<Registration>();
        }

        private IEnumerable<Restriction> GetRestrictions(bool validRestrictions)
        {
            return validRestrictions
                ? new List<Restriction>
                {
                    new Restriction { Info = nameof(Restriction.Info), Title = nameof(Restriction.Title) }
                }
                : new List<Restriction>();
        }

        private IEnumerable<MoreInformationLink> GetInformationLinks(bool validInfoLinks)
        {
            return validInfoLinks
                ? new List<MoreInformationLink>
                {
                    new MoreInformationLink { Url = new Uri(nameof(MoreInformationLink.Url), UriKind.RelativeOrAbsolute), Title = nameof(Registration.Title) }
                }
                : new List<MoreInformationLink>();
        }

        private IEnumerable<EntryRequirement> GetEntryRequirements(bool validEntryReqs)
        {
            return validEntryReqs
                ? new List<EntryRequirement>
                {
                    new EntryRequirement { Info = nameof(Restriction.Info), Title = nameof(Restriction.Title) }
                }
                : new List<EntryRequirement>();
        }

        private int GetItemCountByUlClass(string className, HtmlDocument htmlDocument)
        {
            return htmlDocument.DocumentNode.Descendants("ul").FirstOrDefault(ul => ul.Attributes["class"].Value.Equals(className)).Descendants("li").Count();
        }
    }
}
