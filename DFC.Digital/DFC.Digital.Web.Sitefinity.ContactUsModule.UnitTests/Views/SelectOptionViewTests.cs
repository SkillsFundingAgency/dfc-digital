using ASP;
using DFC.Digital.Web.Sitefinity.ContactUsModule.Mvc.Models;
using FluentAssertions;
using HtmlAgilityPack;
using RazorGenerator.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Xunit;

namespace DFC.Digital.Web.Sitefinity.ContactUsModule.UnitTests
{
    public class SelectOptionViewTests
    {
        [Theory]
        [InlineData("selectoption")]
        public void DFC7630ScenarioA1ForSelectOption(int currentPage)
        {
            var resultsView = new _MVC_Views_Select_Option_cshtml();

            var contactUsViewModel = GenerateDummyContactUsViewModel(currentPage);

            var htmlDom = resultsView.RenderAsHtml(contactUsViewModel);

            var mainPageTitle = GetFirstTagText(htmlDom, "h1");
            var secondaryText = GetFirstTagTextWithClass(htmlDom, "p", "filter-results-subheading");
            var backText = GetFirstTagText(htmlDom, "button");
            var backUrl = GetPreviouspageUrl(htmlDom);

            mainPageTitle.Should().BeEquivalentTo(contactUsViewModel.MainPageTitle);
            secondaryText.Should().BeEquivalentTo(contactUsViewModel.SecondaryText);

            backText.Should().BeEquivalentTo(contactUsViewModel.BackPageUrlText);
            backUrl.Should().BeEquivalentTo(contactUsViewModel.BackPageUrl.OriginalString);

            if (contactUsViewModel.HasNextPage)
            {
                GetPaginationNextVisible(htmlDom).Should().BeTrue();
                GetNavigationUrl(htmlDom, true, "dfc-code-search-next next").Should().BeEquivalentTo(contactUsViewModel.NextPageUrlText);
                GetNavigationUrl(htmlDom, false, "dfc-code-search-next next").Should().BeEquivalentTo(contactUsViewModel.NextPageUrl.OriginalString);
            }

            if (contactUsViewModel.HasPreviousPage)
            {
                GetPaginationPreviousVisible(htmlDom).Should().BeTrue();
                GetNavigationUrl(htmlDom, true, "dfc-code-search-previous previous").Should().BeEquivalentTo(contactUsViewModel.PreviousPageUrlText);
                GetNavigationUrl(htmlDom, false, "dfc-code-search-previous previous").Should().BeEquivalentTo(contactUsViewModel.PreviousPageUrl.OriginalString);
            }
        }

        private ContactUsViewModel GenerateDummyContactUsViewModel(bool isValidModel)
        {
            if (isValidModel)
            {
                return new ContactUsViewModel
                {
                    ContactOption = ContactOption.ContactAdviser
                };
            }

            return new ContactUsViewModel();
        }

        private string GetNavigationUrl(HtmlDocument htmlDom, bool url, string className)
        {
            if (url)
            {
                return htmlDom.DocumentNode.Descendants("li").FirstOrDefault(li => li.HasAttributes &&
                    li.Attributes["class"].Value.Equals(className))?.Descendants("span").FirstOrDefault(span => span.HasAttributes &&
                    span.Attributes["class"].Value.Equals("page-numbers"))?.InnerText;
            }

            return htmlDom.DocumentNode.Descendants("li").FirstOrDefault(li => li.HasAttributes &&
                                                                               li.Attributes["class"].Value.Equals(className))?.Descendants("form").FirstOrDefault()?.GetAttributeValue("action", string.Empty);
        }

        private string GetPreviouspageUrl(HtmlDocument htmlDocument)
        {
            return htmlDocument.DocumentNode.Descendants("button").FirstOrDefault(ol => ol.Attributes["id"].Value.Equals("filter-home"))?.GetAttributeValue("formaction", string.Empty);
        }

        private string GetFirstTagText(HtmlDocument htmlDocument, string tag)
        {
            return htmlDocument.DocumentNode.Descendants(tag).FirstOrDefault()?.InnerText;
        }

        private string GetFirstTagTextWithClass(HtmlDocument htmlDocument, string tag, string className)
        {
            return htmlDocument.DocumentNode.Descendants(tag).FirstOrDefault(tg => tg.HasAttributes && tg.Attributes["class"].Value.Equals(className))?.InnerText;
        }

        private bool GetPaginationNextVisible(HtmlDocument htmlDocument)
        {
            return !string.IsNullOrWhiteSpace(htmlDocument.DocumentNode.Descendants("li").FirstOrDefault(ul => ul.HasAttributes && ul.Attributes["class"].Value.Contains("dfc-code-search-next"))?.InnerHtml);
        }

        private bool GetPaginationPreviousVisible(HtmlDocument htmlDocument)
        {
            return !string.IsNullOrWhiteSpace(htmlDocument.DocumentNode.Descendants("li").FirstOrDefault(ul => ul.HasAttributes && ul.Attributes["class"].Value.Contains("dfc-code-search-previous"))?.InnerHtml);
        }
    }
}