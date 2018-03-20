using ASP;
using DFC.Digital.Web.Sitefinity.JobProfileModule.Mvc.Models;
using FluentAssertions;
using HtmlAgilityPack;
using RazorGenerator.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Xunit;

namespace DFC.Digital.Web.Sitefinity.JobProfileModule.UnitTests
{
    public class PsfSearchViewTests
    {
        [Theory]
        [InlineData(1, 1, 1)]
        [InlineData(20, 2, 1)]
        [InlineData(50, 5, 5)]
        public void DFC1940ScenarioA1ForShowPsfResults(int count, int totalPages, int currentPage)
        {
            var resultsView = new _MVC_Views_PsfSearch_SearchResult_cshtml();

            var psfSearchResultsViewModel = GenerateDummyJobProfileSearchResultViewModel(count, totalPages, DummyMultipleJobProfileSearchResults(count), " result found", currentPage);

            var htmlDom = resultsView.RenderAsHtml(psfSearchResultsViewModel);

            var mainPageTitle = GetFirstTagText(htmlDom, "h1");
            var secondaryText = GetFirstTagTextWithClass(htmlDom, "p", "filter-results-subheading");
            var backText = GetFirstTagText(htmlDom, "button");
            var backUrl = GetPreviouspageUrl(htmlDom);
            var searchResults = GetSearchResults(htmlDom);

            mainPageTitle.Should().BeEquivalentTo(psfSearchResultsViewModel.MainPageTitle);
            secondaryText.Should().BeEquivalentTo(psfSearchResultsViewModel.SecondaryText);
            searchResults.Should().BeEquivalentTo(psfSearchResultsViewModel.SearchResults);
            backText.Should().BeEquivalentTo(psfSearchResultsViewModel.BackPageUrlText);
            backUrl.Should().BeEquivalentTo(psfSearchResultsViewModel.BackPageUrl.OriginalString);

            if (psfSearchResultsViewModel.HasNextPage)
            {
                GetPaginationNextVisible(htmlDom).Should().BeTrue();
                GetNavigationUrl(htmlDom, true, "dfc-code-search-next next").Should().BeEquivalentTo(psfSearchResultsViewModel.NextPageUrlText);
                GetNavigationUrl(htmlDom, false, "dfc-code-search-next next").Should().BeEquivalentTo(psfSearchResultsViewModel.NextPageUrl.OriginalString);
            }

            if (psfSearchResultsViewModel.HasPreviousPage)
            {
                GetPaginationPreviousVisible(htmlDom).Should().BeTrue();
                GetNavigationUrl(htmlDom, true, "dfc-code-search-previous previous").Should().BeEquivalentTo(psfSearchResultsViewModel.PreviousPageUrlText);
                GetNavigationUrl(htmlDom, false, "dfc-code-search-previous previous").Should().BeEquivalentTo(psfSearchResultsViewModel.PreviousPageUrl.OriginalString);
            }
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(3)]
        public void DFC1495ForPSFResultsAlsoFoundInCategories(int numberOfLinkedJobCategories)
        {
            var searchResultsView = new _MVC_Views_PsfSearch_SearchResult_cshtml();

            var psfSearchResultsViewModel = GenerateDummyJobProfileSearchResultViewModel(1, 1, DummyMultipleJobProfileSearchResults(10, numberOfLinkedJobCategories), " result found", 1);

            var htmlDom = searchResultsView.RenderAsHtml(psfSearchResultsViewModel);

            var linkedCategorySection = htmlDom.DocumentNode.SelectSingleNode("//p[contains(@class, 'results-categories')]");

            if (numberOfLinkedJobCategories <= 0)
            {
                //The section should not be displayed if no linked categories
                linkedCategorySection.Should().BeNull();
            }
            else
            {
                //Should have found in section
                linkedCategorySection.InnerText.Should().Contain("Found in:");
                var foundInCategoryLinks = GetDisplayedViewAnchorLinks(linkedCategorySection);
                var expectedCategoryLinks = GetLinkedCategories(numberOfLinkedJobCategories, "/job-categories/");
                foundInCategoryLinks.Should().BeEquivalentTo(expectedCategoryLinks);
            }
        }

        [Theory]
        [InlineData("testText", "", "testText")]
        [InlineData("testText", "testRange", "testRange")]
        public void DFC1510ForSalaryVariableText(string salaryText, string salaryRange, string expectedValue)
        {
            //Assign
            var searchResultsView = new _MVC_Views_PsfSearch_SearchResult_cshtml();
            var psfSearchResultsViewModel = GenerateDummyJobProfileSearchResultViewModel(1, 1, DummyMultipleJobProfileSearchResults(1, 0, salaryRange), " result found", 1);
            psfSearchResultsViewModel.SalaryBlankText = salaryText;

            //Act
            var htmlDom = searchResultsView.RenderAsHtml(psfSearchResultsViewModel);

            //Assert
            GetSalaryText(htmlDom).ShouldBeEquivalentTo(expectedValue);
        }

        private string GetSalaryText(HtmlDocument htmlDocument)
        {
            return htmlDocument.DocumentNode.Descendants("span")
                .FirstOrDefault(span => span.Attributes["class"].Value.Equals("dfc-code-search-jpSalary"))?.InnerText.Trim();
        }

        private IEnumerable<string> GetDisplayedViewAnchorLinks(HtmlNode htmlNode)
        {
            return htmlNode.Descendants("a").Select(n => $"{n.InnerText}|{n.GetAttributeValue("href", string.Empty)}")
                .ToList();
        }

        /// <summary>
        /// Generates test data for categorys linked to a job profile.
        /// </summary>
        /// <param name="numberOfLinkedJobCategories">Number of entries to create</param>
        /// <param name="baseUrl">For genetaring the model this will be blank, when used to check the results in the view this should be the base url for the categories controller route</param>
        /// <returns>List of requested categories</returns>
        private IEnumerable<string> GetLinkedCategories(int numberOfLinkedJobCategories, string baseUrl)
        {
            if (numberOfLinkedJobCategories <= 0)
            {
                yield return null;
            }

            for (int ii = 0; ii < numberOfLinkedJobCategories; ii++)
            {
                yield return $"category{ii}|{baseUrl}categoryURL{ii}";
            }
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

        private IEnumerable<JobProfileSearchResultItemViewModel> GetSearchResults(HtmlDocument htmlDom)
        {
            foreach (var n in htmlDom.DocumentNode.Descendants("ol").FirstOrDefault(ol => ol.HasAttributes && ol.Attributes["class"].Value.Equals("results-list"))?.Descendants("li"))
            {
                yield return new JobProfileSearchResultItemViewModel
                {
                    ResultItemTitle = n.Descendants("a").FirstOrDefault()?.InnerText,
                    ResultItemAlternativeTitle = n.Descendants("p").FirstOrDefault(p => p.Attributes["class"].Value.Contains("dfc-code-search-jpAltTitle"))?.InnerText,
                    ResultItemOverview = n.Descendants("p").FirstOrDefault(p => p.Attributes["class"].Value.Contains("dfc-code-search-jpOverview"))?.InnerText,
                    ResultItemSalaryRange = HttpUtility.HtmlDecode(n.Descendants("span").FirstOrDefault()?.InnerText.Trim()),
                    ResultItemUrlName = n.Descendants("a").FirstOrDefault(a => a.Attributes["class"].Value.Contains("dfc-code-search-jpTitle"))?.GetAttributeValue("href", string.Empty)
                };
            }
        }

        private PsfSearchResultsViewModel GenerateDummyJobProfileSearchResultViewModel(int count, int totalPages, IEnumerable<JobProfileSearchResultItemViewModel> jobProfileSearchResult, string resultMessage, int currentPage)
        {
            return new PsfSearchResultsViewModel
            {
                MainPageTitle = nameof(PsfSearchResultsViewModel.MainPageTitle),
                SecondaryText = nameof(PsfSearchResultsViewModel.SecondaryText),
                PageNumber = currentPage,
                NextPageUrl = new Uri(nameof(PsfSearchResultsViewModel.NextPageUrl), UriKind.RelativeOrAbsolute),
                NextPageUrlText = nameof(PsfSearchResultsViewModel.NextPageUrlText),
                PreviousPageUrl = new Uri(nameof(PsfSearchResultsViewModel.PreviousPageUrl), UriKind.RelativeOrAbsolute),
                PreviousPageUrlText = nameof(PsfSearchResultsViewModel.PreviousPageUrlText),
                Count = count,
                TotalPages = totalPages,
                SearchResults = jobProfileSearchResult,
                SearchTerm = "*",
                TotalResultsMessage = resultMessage,
                JobProfileCategoryPage = "/job-categories/",
                PreSearchFiltersModel = GeneratePreSearchFiltersViewModel(),
                BackPageUrl = new Uri(nameof(PsfSearchResultsViewModel.BackPageUrl), UriKind.RelativeOrAbsolute),
                BackPageUrlText = nameof(PsfSearchResultsViewModel.BackPageUrlText)
            };
        }

        private PsfModel GeneratePreSearchFiltersViewModel()
        {
            var filtersModel = new PsfModel { Sections = new List<PsfSection>() };

            var filterSectionOne = new PsfSection
            {
                Name = "Multi Select Section One",
                Description = "Dummy Title One",
                SingleSelectOnly = false,
                NextPageUrl = "NextSectionURL",
                PreviousPageUrl = "HomePageURL",
                PageNumber = 1,
                TotalNumberOfPages = 2,
                SectionDataType = "Dummy Data Type One",
                Options = new List<PsfOption>()
            };

            for (int ii = 0; ii < 3; ii++)
            {
                var iiString = ii.ToString();
                filterSectionOne.Options.Add(new PsfOption { Id = iiString, IsSelected = false, Name = $"Title-{iiString}", Description = $"Description-{iiString}", OptionKey = $"{iiString}-UrlName", ClearOtherOptionsIfSelected = false });
            }

            //Add one thats Non Applicable
            filterSectionOne.Options.Add(new PsfOption { Id = "3", IsSelected = false, Name = "Title-3", Description = "Description-3", OptionKey = "3-UrlName", ClearOtherOptionsIfSelected = true });

            filtersModel.Sections.Add(filterSectionOne);

            var filterSectionTwo = new PsfSection
            {
                Name = "Single Select Section Two",
                Description = "Dummy Title Two",
                SingleSelectOnly = true,
                NextPageUrl = "SearchPageURL",
                PreviousPageUrl = "PreviousPageURL",
                PageNumber = 2,
                TotalNumberOfPages = 2,
                SectionDataType = "Dummy Data Type Two"
            };

            filterSectionTwo.Options = new List<PsfOption>();
            for (int ii = 0; ii < 3; ii++)
            {
                var iiString = ii.ToString();
                filterSectionTwo.Options.Add(new PsfOption { Id = iiString, IsSelected = false, Name = $"Title-{iiString}", Description = $"Description-{iiString}", OptionKey = $"{iiString}-UrlName", ClearOtherOptionsIfSelected = false });
            }

            filtersModel.Sections.Add(filterSectionTwo);
            return filtersModel;
        }

        private IEnumerable<JobProfileSearchResultItemViewModel> DummyMultipleJobProfileSearchResults(int countofResults, int numberOfLinkedJobCategories = 0, string salaryRange = "Salary Average = £23,000 - £101,000")
        {
            for (var i = 0; i < countofResults; i++)
            {
                yield return new JobProfileSearchResultItemViewModel
                {
                    ResultItemTitle = $"Test Result Title {i}",
                    ResultItemAlternativeTitle = $"Alt Title {i}",
                    ResultItemOverview = "OverView Text",
                    ResultItemSalaryRange = salaryRange,
                    ResultItemUrlName = "Test URL Name",
                    JobProfileCategoriesWithUrl = numberOfLinkedJobCategories > 0 ? GetLinkedCategories(numberOfLinkedJobCategories, string.Empty) : null
                };
            }
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