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

namespace DFC.Digital.Web.Sitefinity.JobProfileModule.Tests.Views
{
    /// <summary>
    /// Job Profile Categories view tests
    /// </summary>
    public class JobProfileCategoriesViewTests
    {
        /// <summary>
        /// DFCs the 302 a1 job profile categories.
        /// </summary>
        /// <param name="isContentAuthoring">if set to <c>true</c> [is content authoring].</param>
        /// <param name="categoriesSetup">if set to <c>true</c> [categories setup].</param>
        /// <param name="urlName">Name of the URL.</param>
        /// <param name="otherCategoryTitle">The other category title.</param>
        [Theory]
        [InlineData(true, true, "test-url-bn", "otherCategory title")]
        [InlineData(true, false, "test-url", "otherCategory title")]
        [InlineData(false, true, "test-url", "otherCategory title")]
        [InlineData(false, true, "TEST-URL", "otherCategory title")]
        [InlineData(false, false, "test-url", "otherCategory title")]

        //This also covers DFC_274 A1 as it is to see whether Job Categories are displayed
        //As a citizen, I was to see a list of Job Categories when I am on the Job Category page
        public void DFC_302_A1_JobProfileCategories(bool isContentAuthoring, bool categoriesSetup, string urlName, string otherCategoryTitle)
        {
            // Arrange
            var indexView = new _MVC_Views_JobProfileCategories_RelatedJobCategories_cshtml();
            var jobProfileByCategoryViewModel =
                GenerateJobProfileCategoriesViewModelDummy(isContentAuthoring, categoriesSetup, urlName, otherCategoryTitle);
            var filteredJobProfileCategories =
                jobProfileByCategoryViewModel.JobProfileCategories?.Where(
                    jpCat => !string.Equals(jpCat.Url.ToLower(), urlName.ToLower(), StringComparison.InvariantCultureIgnoreCase));

            jobProfileByCategoryViewModel.JobProfileCategories = filteredJobProfileCategories;

            // Act
            var htmlDom = indexView.RenderAsHtml(jobProfileByCategoryViewModel);

            // Assert
            GetH2Heading(htmlDom).Should().BeEquivalentTo(jobProfileByCategoryViewModel.OtherCategoriesTitle);

            var displayedJobProfileCategories = GetDisplayedJobProfileCategories(htmlDom);
            displayedJobProfileCategories.Should().BeEquivalentTo(filteredJobProfileCategories);
        }

        /// <summary>
        /// Dummies the job profile categories.
        /// </summary>
        /// <param name="urlName">Name of the URL.</param>
        /// <returns>enumerable</returns>
        public IEnumerable<JobProfileCategory> DummyJobProfileCategories(string urlName)
        {
            yield return new JobProfileCategory
            {
                Title = $"1 {nameof(JobProfileCategory.Title)}",
                Url = nameof(JobProfileCategory.Url)
            };
            yield return new JobProfileCategory
            {
                Title = $"2 {nameof(JobProfileCategory.Title)}",
                Url = urlName
            };
            yield return new JobProfileCategory
            {
                Title = $"3 {nameof(JobProfileCategory.Title)}",
                Url = nameof(JobProfileCategory.Url)
            };
        }

        /// <summary>
        /// Gets the h2 heading.
        /// </summary>
        /// <param name="htmlDom">The HTML DOM.</param>
        /// <returns>static string</returns>
        private static string GetH2Heading(HtmlDocument htmlDom)
        {
            var h2Element = htmlDom.DocumentNode.Descendants("h2").FirstOrDefault();
            return h2Element?.InnerText;
        }

        /// <summary>
        /// Gets the displayed job profile categories.
        /// </summary>
        /// <param name="htmlDom">The HTML DOM.</param>
        /// <returns>static list</returns>
        private static List<JobProfileCategory> GetDisplayedJobProfileCategories(HtmlDocument htmlDom)
        {
            return htmlDom.DocumentNode.Descendants("li")
                .Select(n =>
                {
                    var firstOrDefault = n.Descendants("a").FirstOrDefault();
                    if (firstOrDefault != null)
                    {
                        return new JobProfileCategory { Title = firstOrDefault.InnerText, Url = firstOrDefault.GetAttributeValue("href", string.Empty) };
                    }

                    return null;
                })
                .ToList();
        }

        /// <summary>
        /// Generates the job profile categories view model dummy.
        /// </summary>
        /// <param name="isContentAuthoring">if set to <c>true</c> [is content authoring].</param>
        /// <param name="categoriesSetup">if set to <c>true</c> [categories setup].</param>
        /// <param name="urlName">Name of the URL.</param>
        /// <param name="otherCategoryText">The other category text.</param>
        /// <returns>RelatedJobProfileCategoriesViewModel</returns>
        private RelatedJobProfileCategoriesViewModel GenerateJobProfileCategoriesViewModelDummy(bool isContentAuthoring, bool categoriesSetup, string urlName, string otherCategoryText)
        {
            return new RelatedJobProfileCategoriesViewModel
            {
                JobProfileCategories = categoriesSetup
                    ? DummyJobProfileCategories(urlName)
                    : new List<JobProfileCategory>(),
                IsContentAuthoring = isContentAuthoring,
                OtherCategoriesTitle = otherCategoryText
            };
        }
    }
}