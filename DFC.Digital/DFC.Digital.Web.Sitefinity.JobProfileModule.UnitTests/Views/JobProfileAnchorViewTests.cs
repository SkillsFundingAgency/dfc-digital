using ASP;
using DFC.Digital.Web.Sitefinity.JobProfileModule.Mvc.Models;
using FluentAssertions;
using HtmlAgilityPack;
using RazorGenerator.Testing;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace DFC.Digital.Web.Sitefinity.JobProfileModule.Tests.Views
{
    /// <summary>
    /// Job Profile Anchor view tests
    /// Testing the view correctly displays the anchorlinks passed in by the viewmodel
    /// </summary>
    public class JobProfileAnchorViewTests
    {
        /// <summary>
        /// DFCs the 800 a1 job profile anchor links.
        /// </summary>
        /// <param name="validLinks">if set to <c>true</c> [valid links].</param>
        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void DFC_800_A1_JobProfileAnchorLinks(bool validLinks)
        {
            // Arrange
            var indexView = new _MVC_Views_JobProfileAnchorLinks_Index_cshtml();
            var jobProfileAnchorlistViewModel =
                GenerateJobProfileAnchorListViewModelDummy(validLinks);

            // Act
            var htmlDom = indexView.RenderAsHtml(jobProfileAnchorlistViewModel);

            // Assert
            GetViewAnchorLinks(htmlDom).ShouldAllBeEquivalentTo(jobProfileAnchorlistViewModel.AnchorLinks);
        }

        /// <summary>
        /// Gets the view anchor links.
        /// </summary>
        /// <param name="htmlDom">The HTML DOM.</param>
        /// <returns>returns anchorlink</returns>
        private IEnumerable<AnchorLink> GetViewAnchorLinks(HtmlDocument htmlDom)
        {
            return htmlDom.DocumentNode.Descendants("li")
                  .Select(n =>
                  {
                      var firstOrDefault = n.Descendants("a").FirstOrDefault();
                      if (firstOrDefault != null)
                      {
                          return new AnchorLink
                          {
                              LinkText = firstOrDefault.InnerText,
                              LinkTarget = firstOrDefault.GetAttributeValue("href", string.Empty).Replace("#", string.Empty)
                          };
                      }

                      return null;
                  })
                  .ToList();
        }

        /// <summary>
        /// Gets the dummy links.
        /// </summary>
        /// <returns>enumerator</returns>
        private IEnumerable<AnchorLink> GetDummyLinks()
        {
            return new List<AnchorLink>
            {
                new AnchorLink
                {
                    LinkTarget = $"dummy {nameof(AnchorLink.LinkTarget)}",
                    LinkText = $"dummy {nameof(AnchorLink.LinkTarget)}"
                },
                new AnchorLink
                {
                    LinkTarget = $"dummy {nameof(AnchorLink.LinkTarget)}",
                    LinkText = $"dummy {nameof(AnchorLink.LinkTarget)}"
                }
            };
        }

        /// <summary>
        /// Generates the job profile anchor ListView model dummy.
        /// </summary>
        /// <param name="validLinks">if set to <c>true</c> [valid links].</param>
        /// <returns>jobprofileanchorelinkviewmodel</returns>
        private JobProfileAnchorLinksViewModel GenerateJobProfileAnchorListViewModelDummy(
            bool validLinks)
        {
            return new JobProfileAnchorLinksViewModel
            {
                AnchorLinks = validLinks ?
                    GetDummyLinks() : new List<AnchorLink>()
            };
        }
    }
}