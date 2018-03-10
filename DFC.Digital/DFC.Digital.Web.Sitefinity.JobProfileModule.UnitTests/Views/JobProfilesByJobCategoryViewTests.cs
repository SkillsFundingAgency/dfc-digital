using ASP;
using DFC.Digital.AutomationTest.Utilities;
using DFC.Digital.Data.Model;
using DFC.Digital.Web.Sitefinity.JobProfileModule.Mvc.Models;
using FluentAssertions;
using HtmlAgilityPack;
using RazorGenerator.Testing;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace DFC.Digital.Web.Sitefinity.JobProfileModule.View.Tests
{
    public class JobProfilesByJobCategoryViewTests
    {
        [Fact]

        //As a Citizen, I want to be able to view the job profiles on the job category page
        public void DFC_275_A1_A2_A3_A4_JobProfilesByCategory()
        {
            // Arrange
            var indexView = new _MVC_Views_JobProfilesByCategory_Index_cshtml();
            var jobProfileByCategoryViewModel = GenerateJobProfileByCategoryViewModelDummy();

            // Act
            var htmlDom = indexView.RenderAsHtml(jobProfileByCategoryViewModel);

            // Asserts

            //Category title is displayed
            GetH1Heading(htmlDom).Should().BeEquivalentTo(jobProfileByCategoryViewModel.Title);

            //Profiles displayed in alphabetical order - dont think this is the correct place to test this
            var displayedJobProfiles = GetDisplayedJobProfiles(htmlDom);

            //Compare what is displayed Title, Overview and AlernativeTitle only when its there else no h3 element
            displayedJobProfiles.Should().BeEquivalentTo(DummyJobProfile.GetDummyJobProfilesForCategory());
        }

        private string GetH1Heading(HtmlDocument htmlDom)
        {
            return htmlDom.DocumentNode.SelectSingleNode("h1").InnerText;
        }

        private List<JobProfile> GetDisplayedJobProfiles(HtmlDocument htmlDom)
        {
            List<JobProfile> displayedJobProfiles = new List<JobProfile>();
            foreach (HtmlNode n in htmlDom.DocumentNode.SelectNodes("//li"))
            {
                JobProfile p = new JobProfile();
                p.Title = n.Descendants("a").FirstOrDefault().InnerText;

                //h3 should not exist if there was no alternative title, so should get null
                p.AlternativeTitle = n.Descendants("h3").FirstOrDefault()?.InnerText;

                p.Overview = n.Descendants("p").FirstOrDefault().InnerText;
                displayedJobProfiles.Add(p);
            }

            return displayedJobProfiles;
        }

        private JobProfileByCategoryViewModel GenerateJobProfileByCategoryViewModelDummy()
        {
            return new JobProfileByCategoryViewModel
            {
                Title = $"dummy{nameof(JobProfileByCategoryViewModel.Title)}",
                Description = $"dummy{nameof(JobProfileByCategoryViewModel.Description)}",
                JobProfiles = DummyJobProfile.GetDummyJobProfiles(),
                JobProfileUrl = "DummyURL",
            };
        }
    }
}