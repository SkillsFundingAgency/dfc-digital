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
        public void DFC800A1JobProfileAnchorLinks(bool validLinks)
        {
            // Arrange
            var indexView = new _MVC_Views_JobProfileAnchorLinks_Index_cshtml();
            var jobProfileAnchorlistViewModel =
                GenerateJobProfileAnchorListViewModelDummy(validLinks);

            // Act
            var htmlDom = indexView.RenderAsHtml(jobProfileAnchorlistViewModel);

            // Assert
            GetViewAnchorLinks(htmlDom).Should().BeEquivalentTo(jobProfileAnchorlistViewModel.AnchorLinks);
        }

        /// <summary>
        /// Gets the view anchor links.
        /// </summary>
        /// <param name="htmlDom">The HTML DOM.</param>
        /// <returns>returns anchorlink</returns>
        private IEnumerable<AnchorLink> GetViewAnchorLinks(HtmlDocument htmlDom)
        {
            return htmlDom.DocumentNode.Descendants("a")
                  .Select(n => new AnchorLink
                {
                    LinkText = n.InnerText.Trim(),
                    LinkTarget = n.GetAttributeValue("href", string.Empty).Replace("#", string.Empty).Trim()
                })
                  .ToList();
        }

        /// <summary>
        /// Gets the dummy links.
        /// </summary>
        /// <returns>enumerator</returns>
        private IEnumerable<AnchorLink> GetDummyLinks()
        {
            yield return new AnchorLink
            {
                LinkTarget = $"dummy {nameof(AnchorLink.LinkTarget)}",
                LinkText = $"dummy {nameof(AnchorLink.LinkText)}"
            };
            yield return new AnchorLink
            {
                LinkTarget = $"dummy {nameof(AnchorLink.LinkTarget)}",
                LinkText = $"dummy {nameof(AnchorLink.LinkText)}"
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