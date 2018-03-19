using ASP;
using DFC.Digital.Data.Model;
using DFC.Digital.Web.Sitefinity.JobProfileModule.Mvc.Models;
using FluentAssertions;
using HtmlAgilityPack;
using RazorGenerator.Testing;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace DFC.Digital.Web.Sitefinity.JobProfileModule.UnitTests
{
    public class JobProfileRelatedCareersViewTest
    {
        [Fact]

        //Related Careers links are correctly displayed with a section header and links.
        public void DFC1335ForRelatedCareersOnJobProfilesTest()
        {
            var index = new _MVC_Views_JobProfileRelatedCareers_Index_cshtml();
            var jobProfileApprenticeViewModel = GenerateRelatedCareersViewModel();

            var htmlDom = index.RenderAsHtml(jobProfileApprenticeViewModel);

            var sectionText = htmlDom.DocumentNode.SelectNodes("//h2[contains(@class, 'heading-medium')]").FirstOrDefault().InnerText;

            sectionText.Should().BeEquivalentTo("Related Careers");

            GetViewAnchorLinks(htmlDom).Should().BeEquivalentTo(GetDummyRelatedProfiles());
        }

        private IEnumerable<JobProfileRelatedCareer> GetViewAnchorLinks(HtmlDocument htmlDom)
        {
            return htmlDom.DocumentNode.Descendants("li")
                  .Select(n =>
                  {
                      var firstOrDefault = n.Descendants("a").FirstOrDefault();
                      if (firstOrDefault != null)
                      {
                          return new JobProfileRelatedCareer
                          {
                              Title = firstOrDefault.InnerText,
                              ProfileLink = firstOrDefault.GetAttributeValue("href", string.Empty)
                          };
                      }

                      return null;
                  })
                  .ToList();
        }

        private JobProfileRelatedCareersModel GenerateRelatedCareersViewModel()
        {
            return new JobProfileRelatedCareersModel() { Title = "Related Careers", RelatedCareers = GetDummyRelatedProfiles() };
        }

        private IEnumerable<JobProfileRelatedCareer> GetDummyRelatedProfiles()
        {
            yield return new JobProfileRelatedCareer() { Title = "Title one", ProfileLink = "http://linkone" };
            yield return new JobProfileRelatedCareer() { Title = "Title two", ProfileLink = "http://linktwo" };
            yield return new JobProfileRelatedCareer() { Title = "Title three", ProfileLink = "http://linkthree" };
        }
    }
}