﻿using ASP;
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
        [InlineData(true, "Content")]
        [InlineData(false, "Content")]
        public void Dfc339IndexViewTests(bool cadReady, string content)
        {
            // Arrange
            var restrictionsView = new _MVC_Views_JobProfileWhatItTakes_Index_cshtml();
            var jobProfileWhatItTakesViewModel = new JobProfileWhatItTakesViewModel
            {
                PropertyValue = content,
                IsWhatItTakesCadView = cadReady,
                RestrictionsOtherRequirements = new RestrictionsOtherRequirements
                {
                    Restrictions = GetRestrictions(cadReady),
                    OtherRequirements = content
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

            if (!string.IsNullOrWhiteSpace(content))
            {
                AssertContentExistsInView(content, htmlDocument);
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