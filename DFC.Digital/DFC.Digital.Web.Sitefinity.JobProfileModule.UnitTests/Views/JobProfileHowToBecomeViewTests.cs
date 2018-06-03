using ASP;
using DFC.Digital.Data.Model;
using DFC.Digital.Web.Sitefinity.JobProfileModule.Mvc.Models;
using FluentAssertions;
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
            var htmlDom = moreInformationView.RenderAsHtml(jobProfileHowToBecomeView);

            // Assert
            if (string.IsNullOrWhiteSpace(careerTips) && string.IsNullOrWhiteSpace(professionalAndIndustryBodies) &&
                string.IsNullOrWhiteSpace(furtherInformation) && !validRegistrations)
            {
                 htmlDom.DocumentNode.Descendants().Count().Should().Be(0);
            }

            if (!string.IsNullOrWhiteSpace(careerTips))
            {
                htmlDom.DocumentNode.InnerHtml.IndexOf(careerTips, StringComparison.OrdinalIgnoreCase).Should().BeGreaterThan(-1);
            }

            if (!string.IsNullOrWhiteSpace(professionalAndIndustryBodies))
            {
                htmlDom.DocumentNode.InnerHtml.IndexOf(professionalAndIndustryBodies, StringComparison.OrdinalIgnoreCase).Should().BeGreaterThan(-1);
            }

            if (!string.IsNullOrWhiteSpace(furtherInformation))
            {
                htmlDom.DocumentNode.InnerHtml.IndexOf(furtherInformation, StringComparison.OrdinalIgnoreCase).Should().BeGreaterThan(-1);
            }

            if (validRegistrations)
            {
                htmlDom.DocumentNode.Descendants("li").Count().Should().BeGreaterThan(0);
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
            var extraInformationView = new _MVC_Views_JobProfileHowToBecome_ExtraInformation_cshtml();
            var jobProfileHowToBecomeViewModel = new JobProfileHowToBecomeViewModel
            {
                HowToBecome = new HowToBecome
                {
                    ExtraInformation = new ExtraInformation
                    {
                        Work = work,
                        DirectApplication = directApplication,
                        OtherRequirements = otherRequirements,
                        OtherRoutes = otherRoutes,
                        Volunteering = volunteering
                    }
                }
            };

            // Act
            var htmlDom = extraInformationView.RenderAsHtml(jobProfileHowToBecomeViewModel);

            // Assert
            if (!string.IsNullOrWhiteSpace(work))
            {
                ContentDisplayedBySectionId(htmlDom, "work").Should().BeTrue();
            }

            if (!string.IsNullOrWhiteSpace(directApplication))
            {
                ContentDisplayedBySectionId(htmlDom, "directapplication").Should().BeTrue();
            }

            if (!string.IsNullOrWhiteSpace(otherRequirements))
            {
                ContentDisplayedBySectionId(htmlDom, "otherrequirements").Should().BeTrue();
            }

            if (!string.IsNullOrWhiteSpace(volunteering))
            {
                ContentDisplayedBySectionId(htmlDom, "volunteering").Should().BeTrue();
            }

            if (!string.IsNullOrWhiteSpace(otherRoutes))
            {
                ContentDisplayedBySectionId(htmlDom, "otherroutes").Should().BeTrue();
            }
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
    }
}
