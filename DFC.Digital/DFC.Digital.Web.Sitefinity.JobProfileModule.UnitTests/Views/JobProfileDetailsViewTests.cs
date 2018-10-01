using ASP;
using DFC.Digital.Web.Sitefinity.JobProfileModule.Mvc.Models;
using FluentAssertions;
using HtmlAgilityPack;
using RazorGenerator.Testing;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace DFC.Digital.Web.Sitefinity.JobProfileModule.UnitTests
{
    public class JobProfileDetailsViewTests
    {
        private const string HoursTimePeriodTestText = "\r\nper Week\r\n    ";

        [Theory]
        [InlineData("20", "48")]
        [InlineData("20", "")]
        [InlineData("", "48")]
        [InlineData("", "")]

        //As a content author, I want to enter and edit the hours overview in the job profile template
        public void DFC2103ForJobProfileWorkingHours(string minHours, string maxHours)
        {
            // Arrange
            var indexView = new _MVC_Views_JobProfileDetails_Index_cshtml();

            var maxAndMinHoursAreBlankTestText = "Vairable";

            var model = new JobProfileDetailsViewModel
            {
                MinimumHours = minHours,
                MaximumHours = maxHours,
                MaxAndMinHoursAreBlankText = maxAndMinHoursAreBlankTestText
            };

            // Act
            var htmlDom = indexView.RenderAsHtml(model);

            // Asserts
            if (string.IsNullOrWhiteSpace(model.MinimumHours) || string.IsNullOrWhiteSpace(model.MaximumHours))
            {
                GetHoursSummaryText(htmlDom).Should().Be(maxAndMinHoursAreBlankTestText);
            }
            else
            {
                GetHoursSummaryText(htmlDom).Should().Be($"{model.MinimumHours} to {model.MaximumHours}");
            }
        }

        [Theory]
        [InlineData("10000", "30000")]
        [InlineData("10000", "")]
        [InlineData("", "3000")]
        [InlineData("", "")]
        public void DFC2047ForJobProfileSalary(string salaryStarterValue, string salaryExperiencedValue)
        {
            // Arrange
            var indexView = new _MVC_Views_JobProfileDetails_Index_cshtml();

            string salaryBlankText = "Vairable";
            string salaryStarterText = "Starter";
            string salaryExperiencedText = "Experienced";

            double salaryStarter = 0;
            if (double.TryParse(salaryStarterValue, out var salaryStarterGoodValue))
            {
                salaryStarter = salaryStarterGoodValue;
            }

            double salaryExperienced = 0;
            if (double.TryParse(salaryExperiencedValue, out var salaryExperiencedGoodValue))
            {
                salaryExperienced = salaryExperiencedGoodValue;
            }

            var model = new JobProfileDetailsViewModel
            {
                SalaryStarter = salaryStarter,
                SalaryExperienced = salaryExperienced,
                SalaryBlankText = salaryBlankText,
                SalaryStarterText = salaryStarterText,
                SalaryExperiencedText = salaryExperiencedText,
            };

            // Act
            var htmlDom = indexView.RenderAsHtml(model);

            // Asserts
            if (model.SalaryStarter > 0 && model.SalaryExperienced > 0)
            {
                GetSalaryStarter(htmlDom, salaryStarterText).Should().Be(model.SalaryStarter);
                GetSalaryExperienced(htmlDom, salaryExperiencedText).Should().Be(model.SalaryExperienced);
            }
            else
            {
                GetSalaryBlankText(htmlDom).Should().Be(salaryBlankText);
            }
        }

        [Theory]
        [InlineData("Bank holidays", "As customers demand")]
        [InlineData("Freelance / self employed", "")]
        [InlineData("", "Attending events or appointments")]
        [InlineData("", "")]
        public void DFC2086ForJobProfileWorkingPattern(string workingPattern, string workingPatternDetails)
        {
            // Arrange
            var indexView = new _MVC_Views_JobProfileDetails_Index_cshtml();

            var model = new JobProfileDetailsViewModel
            {
                WorkingPattern = workingPattern,
                WorkingPatternDetails = workingPatternDetails,
            };

            // Act
            var htmlDom = indexView.RenderAsHtml(model);

            // Asserts
            if (string.IsNullOrEmpty(model.WorkingPattern))
            {
                // Nothing to display
                GetWorkingPatternText(htmlDom).Should().Be(null);
                GetWorkingPatternDetailText(htmlDom).Should().Be(null);
            }
            else
            {
                if (!string.IsNullOrEmpty(model.WorkingPatternDetails))
                {
                    // Display WorkingPatternDetails
                    GetWorkingPatternDetailText(htmlDom).Should().Be(workingPatternDetails);
                }
                else
                {
                    // No WorkingPatternDetails to be displyed
                    GetWorkingPatternDetailText(htmlDom).Should().Be(null);
                }

                // Display WorkingPattern
                GetWorkingPatternText(htmlDom).Should().Contain(workingPattern);
            }
        }

        [Fact]
        public void DFC3080JobProfileSalaryContextLink()
        {
            var indexView = new _MVC_Views_JobProfileDetails_Index_cshtml();

            var model = new JobProfileDetailsViewModel
            {
                SalaryContextText = "context text",
                SalaryContextLink = "contextlink",
                SalaryStarter = 1000,
                SalaryExperienced = 2000,
                SalaryTextSpan = "Salary Text"
            };

            // Act
            var htmlDom = indexView.RenderAsHtml(model);

            // Asert
            var salaryDiv = htmlDom.GetElementbyId("Salary");

            IEnumerable<HtmlNode> links = null;

            if (salaryDiv != null)
            {
              links = salaryDiv.Descendants("a");
            }

            links.Should().NotBeNull();
            links.Count().Should().Be(1);
            links.First().InnerText.Should().Be(model.SalaryContextText);
        }

        private static string GetHoursSummaryText(HtmlDocument htmlDom)
        {
            var summaryHoursElement = htmlDom.DocumentNode.SelectNodes("//p[contains(@class, 'dfc-code-jphours')]").FirstOrDefault();
            return summaryHoursElement?.InnerText.Replace("\r\n", string.Empty).Trim();
        }

        private static string GetWorkingPatternText(HtmlDocument htmlDom)
        {
            var workingPatternElement = htmlDom.DocumentNode.SelectNodes("//p[contains(@class, 'dfc-code-jpwpattern')]")?.FirstOrDefault();
            return workingPatternElement?.InnerHtml;
        }

        private static string GetWorkingPatternDetailText(HtmlDocument htmlDom)
        {
            var workingPatternDetailElement = htmlDom.DocumentNode.SelectNodes("//span[contains(@class, 'dfc-code-jpwpatterndetail')]")?.FirstOrDefault();
            return workingPatternDetailElement?.InnerHtml;
        }

        private static double GetSalaryStarter(HtmlDocument htmlDom, string salaryStarterText)
        {
            var salaryStarterElement = htmlDom.DocumentNode.SelectNodes("//p[contains(@class, 'dfc-code-jpsstarter')]").FirstOrDefault();

            double salaryStarter = 0;

            if (salaryStarterElement != null)
            {
                if (salaryStarterElement.InnerText.Contains(salaryStarterText))
                {
                    string salaryStarterString = salaryStarterElement.InnerText.Replace(salaryStarterText, string.Empty).Replace("&#163;", string.Empty).Trim();
                    salaryStarterString = salaryStarterString.Replace("to", string.Empty);
                    if (double.TryParse(salaryStarterString, out var salaryStarterGoodValue))
                    {
                        salaryStarter = salaryStarterGoodValue;
                    }

                    return salaryStarter;
                }
            }

            return salaryStarter;
        }

        private static double GetSalaryExperienced(HtmlDocument htmlDom, string salaryExperiencedText)
        {
            var salaryExperiencedElement = htmlDom.DocumentNode.SelectNodes("//p[contains(@class, 'dfc-code-jpsexperienced')]").FirstOrDefault();

            double salaryExperienced = 0;

            if (salaryExperiencedElement != null)
            {
                if (salaryExperiencedElement.InnerText.Contains(salaryExperiencedText))
                {
                    string salaryExperiencedString = salaryExperiencedElement.InnerText.Replace(salaryExperiencedText, string.Empty).Replace("&#163;", string.Empty).Trim();
                    if (double.TryParse(salaryExperiencedString, out var salaryExperiencedGoodValue))
                    {
                        salaryExperienced = salaryExperiencedGoodValue;
                    }

                    return salaryExperienced;
                }
            }

            return salaryExperienced;
        }

        private static string GetSalaryBlankText(HtmlDocument htmlDom)
        {
            var salaryBlankElement = htmlDom.DocumentNode.SelectNodes("//h5[contains(@class, 'dfc-code-jpsalaryblank')]").FirstOrDefault();

            return salaryBlankElement?.InnerText;
        }
    }
}