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

namespace DFC.Digital.Web.Sitefinity.JobProfileModule.UnitTests
{
    /// <summary>
    /// Job Profile Section view tests
    /// </summary>
    public class JobProfileWhatItTakesViewTests
    {
        [Theory]
        [InlineData(true, "Content")]
        [InlineData(false, "")]
        public void Dfc3391RestrictionsOtherRequirementsViewTests(bool validRestrictions, string content)
        {
            // Arrange
            var restrictionsView = new _MVC_Views_JobProfileWhatItTakes_RestrictionsRequirements_cshtml();
            var restrictionsOtherRequirements = new RestrictionsOtherRequirements
            {
                Restrictions = GetRestrictions(validRestrictions),
                OtherRequirements = content
            };

            // Act
            var htmlDocument = restrictionsView.RenderAsHtml(restrictionsOtherRequirements);

            // Assert
            if (validRestrictions)
            {
                htmlDocument.DocumentNode.Descendants("li").Count().Should().IsSameOrEqualTo(restrictionsOtherRequirements.Restrictions.Count());
            }

            if (string.IsNullOrWhiteSpace(content) && !validRestrictions)
            {
                AssertViewIsEmpty(htmlDocument);
            }

            if (!string.IsNullOrWhiteSpace(content))
            {
                AssertContentExistsInView(content, htmlDocument);
            }
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void Dfc339IndexViewTests(bool cadReady)
        {
            // Arrange
            var restrictionsView = new _MVC_Views_JobProfileWhatItTakes_Index_cshtml();
            var jobProfileWhatItTakesViewModel = new JobProfileWhatItTakesViewModel
            {
                Title = "Dummy Title",
                IsWhatItTakesCadView = cadReady,
                RestrictionsOtherRequirements = new RestrictionsOtherRequirements
                {
                    Restrictions = GetRestrictions(cadReady),
                }
            };

            // Act
            var htmlDocument = restrictionsView.RenderAsHtml(jobProfileWhatItTakesViewModel);

            // Assert
            if (cadReady)
            {
                htmlDocument.DocumentNode.Descendants("li").Count().Should()
                    .IsSameOrEqualTo(jobProfileWhatItTakesViewModel.RestrictionsOtherRequirements.Restrictions.Count());
            }

            htmlDocument.DocumentNode.InnerHtml.IndexOf(jobProfileWhatItTakesViewModel.Title, StringComparison.OrdinalIgnoreCase).Should().BeGreaterThan(-1);
        }

        [Theory]
        [InlineData(true, true)]
        [InlineData(false, false)]
        [InlineData(true, false)]
        [InlineData(false, true)]
        public void Dfc3361SkillsViewFlagsTests(bool useONetCitizenFacing, bool useONetInPreview)
        {
            // Arrange
            int numberSkills = 5;
            var whatItTakesView = new _MVC_Views_JobProfileWhatItTakes_WhatItTakesSkills_cshtml();
            var skillsViewModel = new JobProfileWhatItTakesSkillsViewModel
            {
                WhatItTakesSectionTitle = "Dummy Section Title",
                SkillsSectionIntro = "Dummy Intro",
                WhatItTakesSkills = GetSkills(numberSkills),
                PropertyValue = "Non Onet Skills Text",
                NumberONetSkillsToDisplay = numberSkills,
                UseONetDataCitizenFacing = useONetCitizenFacing,
                UseONetDataInPreview = useONetInPreview
            };

            // Act
            var htmlDocument = whatItTakesView.RenderAsHtml(skillsViewModel);

            //Asserts
            var sectionTitle = htmlDocument.DocumentNode.Descendants("h3").FirstOrDefault();
            sectionTitle.InnerText.Should().BeEquivalentTo(skillsViewModel.WhatItTakesSectionTitle);

            //If using the Onet view
            if (useONetCitizenFacing || useONetInPreview)
            {
                //Non Onet skills should NOT be displayed
                htmlDocument.DocumentNode.InnerHtml.IndexOf(skillsViewModel.PropertyValue).Should().Be(-1);
                htmlDocument.DocumentNode.Descendants("li").Count().Should().IsSameOrEqualTo(skillsViewModel.WhatItTakesSkills.Count());
            }
            else
            {
                //Non Onet skills should be displayed
                htmlDocument.DocumentNode.InnerHtml.IndexOf(skillsViewModel.PropertyValue).Should().BeGreaterThan(-1);
            }
        }

        [Fact]
        public void Dfc3361SkillsViewDigitalSkills()
        {
            // Arrange
            int numberSkills = 5;
            var whatItTakesView = new _MVC_Views_JobProfileWhatItTakes_WhatItTakesSkills_cshtml();
            var skillsViewModel = new JobProfileWhatItTakesSkillsViewModel
            {
                WhatItTakesSkills = GetSkills(numberSkills),
                UseONetDataCitizenFacing = true,
                DigitalSkillsLevel = "Digital skills"
            };

            // Act
            var htmlDocument = whatItTakesView.RenderAsHtml(skillsViewModel);

            //Asserts
            //should have the number of skills plus one for  digital skills
            htmlDocument.DocumentNode.Descendants("li").Count().Should().IsSameOrEqualTo(skillsViewModel.WhatItTakesSkills.Count() + 1);
        }

        private static void AssertContentExistsInView(string content, HtmlDocument htmlDocument)
        {
            htmlDocument.DocumentNode.InnerHtml.IndexOf(content, StringComparison.OrdinalIgnoreCase).Should().BeGreaterThan(-1);
        }

        private static void AssertViewIsEmpty(HtmlDocument htmlDocument)
        {
            htmlDocument.DocumentNode.Descendants().Count().Should().Be(0);
        }

        private List<WhatItTakesSkill> GetSkills(int numberOfSkills)
        {
            var skills = new List<WhatItTakesSkill>();
            for (int ii = 0; ii < numberOfSkills; ii++)
            {
                skills.Add(new WhatItTakesSkill { Title = $"Title-{ii}", Description = $"Description-{ii}" });
            }

            return skills;
        }

        private IEnumerable<Restriction> GetRestrictions(bool validRestrictions)
        {
            return validRestrictions
                ? new List<Restriction>
                {
                    new Restriction { Info = nameof(Restriction.Info), Title = nameof(Restriction.Title) },
                    new Restriction { Info = nameof(Restriction.Info), Title = nameof(Restriction.Title) }
                }
                : new List<Restriction>();
        }
    }
}