using ASP;
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
    public class JobProfileSearchViewTests
    {
        /// <summary>
        /// DFC 106 - correct search result text should be displayed
        /// </summary>
        /// <param name="nextUrl"> Term to search with</param>
        /// <param name="nextUrlText"> The expected text to be displayed dependant on the number of search resnextUrlTextults</param>
        /// <param name="previousUrl"> The expected text to be displayed dependant on the number of search previousUrl</param>
        /// <param name="previousUrlText"> The expected text to be displayed dependant on the number of search previousUrlText</param>
        /// <param name="count"> The expected text to be displayed dependant on the number of search count</param>
        /// <param name="totalPages"> The expected text to be displayed dependant on the number of search totalPages</param>
        //Acceptance Criteria - As a Citizen, I want the search results to display the total number of
        //results found.
        [Theory]
        [InlineData("next-url", "nextpage", "prev-url", "prevpage", 1, 1)]
        public void DFC_106_SearchResultText(string nextUrl, string nextUrlText, string previousUrl, string previousUrlText, int count, int totalPages)
        {
            var searchResultsView = new _MVC_Views_JobProfileSearchBox_SearchResult_cshtml();

            var jobProfileSearchResultsViewModel = GenerateDummyJobProfileSearchResultViewModel(
                1, nextUrl, nextUrlText, previousUrl, previousUrlText, count, totalPages, DummyJobProfileSearchResultsWithAltTitle(), "1 result found", "Test");

            var htmlDom = searchResultsView.RenderAsHtml(jobProfileSearchResultsViewModel);

            var resultText = htmlDom.DocumentNode.SelectNodes("//div[contains(@class, 'result-count')]").FirstOrDefault().InnerText;
            resultText.Should().Contain("1 result found");
        }

        //Acceptance Criteria - As a Citizen - having ran a search I want to be able to see Job Profile in my search results
        //Scenario 1 - Display of Job Profile/s containing Alternate Title
        [Theory]
        [InlineData("next-url", "nextpage", "prev-url", "prevpage", 1, 1)]
        public void DFC_164_A1_WithAltTitle(string nextUrl, string nextUrlText, string previousUrl, string previousUrlText, int count, int totalPages)
        {
            var searchView = new _MVC_Views_JobProfileSearchBox_SearchResult_cshtml();

            var jobProfileSearchViewModel = GenerateDummyJobProfileSearchResultViewModel(
                1, nextUrl, nextUrlText, previousUrl, previousUrlText, count, totalPages, DummyJobProfileSearchResultsWithAltTitle(), "1 result found", "Test");

            var htmlDom = searchView.RenderAsHtml(jobProfileSearchViewModel);

            var title = htmlDom.DocumentNode.SelectNodes("//a[contains(@class, 'dfc-code-search-jpTitle')]").FirstOrDefault().InnerText;
            var altTitle = htmlDom.DocumentNode.SelectNodes("//p[contains(@class, 'dfc-code-search-jpAltTitle')]").FirstOrDefault().InnerText;

            title.Should().Contain("Test Result Title");
            altTitle.Should().Contain("Test Alternative Title");
        }

        //Acceptance Criteria - As a Citizen - having ran a search I want to be able to see Job Profile in my search results
        //Scenario 2 - Display of Job Profile/s without Alternate Title
        [Theory]
        [InlineData("next-url", "nextpage", "prev-url", "prevpage", 1, 1)]
        public void DFC_164_A2_WithNoAltTitle(string nextUrl, string nextUrlText, string previousUrl, string previousUrlText, int count, int totalPages)
        {
            var searchView = new _MVC_Views_JobProfileSearchBox_SearchResult_cshtml();

            var jobProfileSearchViewModel = GenerateDummyJobProfileSearchResultViewModel(
                1, nextUrl, nextUrlText, previousUrl, previousUrlText, count, totalPages, DummyJobProfileSearchResultsWithoutAltTitle(), "1 result found", "Test");

            var htmlDom = searchView.RenderAsHtml(jobProfileSearchViewModel);

            var title = htmlDom.DocumentNode.SelectNodes("//a[contains(@class, 'dfc-code-search-jpTitle')]").FirstOrDefault().InnerText;
            var altTitle = htmlDom.DocumentNode.SelectNodes("//p[contains(@class, 'dfc-code-search-jpAltTitle')]");

            title.Should().Contain("Test Result Title");
            altTitle.Should().BeNullOrEmpty();
        }

        //Below test also covers DFC 185 A1 and A2
        //Acceptance Criteria - As a Citizen - having ran a search I want to be able to see Job Profile in my search results
        //Scenario 2 - Display of Rank No for job profile result
        [Theory]
        [InlineData("next-url", "nextpage", "prev-url", "prevpage", 1, 1)]
        public void DFC_164_A3_RankNo(string nextUrl, string nextUrlText, string previousUrl, string previousUrlText, int count, int totalPages)
        {
            var searchView = new _MVC_Views_JobProfileSearchBox_SearchResult_cshtml();

            var jobProfileSearchViewModel = GenerateDummyJobProfileSearchResultViewModel(1, nextUrl, nextUrlText, previousUrl, previousUrlText, count, totalPages, DummyMultipleJobProfileSearchResults(), "2 results found", "Test");

            var htmlDom = searchView.RenderAsHtml(jobProfileSearchViewModel);

            var result1 = htmlDom.DocumentNode.SelectNodes(".//*[@id='results']/div/div/ol/li[1]").FirstOrDefault();
            var result2 = htmlDom.DocumentNode.SelectNodes(".//*[@id='results']/div/div/ol/li[2]").FirstOrDefault();

            var rankId1 = result1.GetAttributeValue("data-ga-rank", "Attribute not found");
            var rankId2 = result2.GetAttributeValue("data-ga-rank", "Attribute not found");

            rankId1.Should().Be("1");
            rankId2.Should().Be("2");
        }

        //As a Citizen, I want to see the search term in the search box when displaying the search results
        //A1 - Search and check search term plus search box
        //A2 - Re-Search and check searchbox contains new search term
        [Theory]
        [InlineData("next-url", "nextpage", "prev-url", "prevpage", 1, 1)]
        public void DFC_187_A1_A2_RetainSearchTerm(string nextUrl, string nextUrlText, string previousUrl, string previousUrlText, int count, int totalPages)
        {
            var indexView = new _MVC_Views_JobProfileSearchBox_Index_cshtml();
            var searchResultsView = new _MVC_Views_JobProfileSearchBox_SearchResult_cshtml();

            var jobProfileSearchResultsViewModel = GenerateDummyJobProfileSearchResultViewModel(
                1, nextUrl, nextUrlText, previousUrl, previousUrlText, count, totalPages, DummyJobProfileSearchResultsWithAltTitle(), "1 result found", "Test");

            var htmlDom = searchResultsView.RenderAsHtml(jobProfileSearchResultsViewModel);

            var searchTermText = htmlDom.DocumentNode.SelectNodes("//*[@id='search-main']").FirstOrDefault()
                .GetAttributeValue("value", "Attribute not found");
            var searchBox = htmlDom.DocumentNode.SelectNodes("//input[contains(@id, 'search-main')]");
            searchTermText.Should().Be("Test");
            searchBox.Should().NotBeNull();

            var resultText = htmlDom.DocumentNode.SelectNodes("//div[contains(@class, 'result-count')]").FirstOrDefault().InnerText;
            resultText.Should().Contain("1 result found");

            var newJobProfileSearch = GenerateDummyJobProfileSearchResultViewModel(
                1, nextUrl, nextUrlText, previousUrl, previousUrlText, count, totalPages, DummyMultipleJobProfileSearchResults(), "2 results found", "New Search Test");

            htmlDom = searchResultsView.RenderAsHtml(newJobProfileSearch);

            searchTermText = htmlDom.DocumentNode.SelectNodes("//*[@id='search-main']").FirstOrDefault()
                            .GetAttributeValue("value", "Attribute not found");
            searchTermText.Should().Be("New Search Test");

            resultText = htmlDom.DocumentNode.SelectNodes("//div[contains(@class, 'result-count')]").FirstOrDefault().InnerText;
            resultText.Should().Contain("2 results found");
        }

        //As a Citizen having not entered a search term on the Search Results page, I want to remain on the Search Results Page
        //A1 - Empty search term - no results displayed and search box displayed
        [Fact]
        public void DFC_324_EmptySearchViewTest()
        {
            var searchPage = new _MVC_Views_JobProfileSearchBox_SearchResult_cshtml();

            var jobProfileSearchViewModel = GenerateDummyJobProfileSearchResultViewModel(1, null, null, null, null, 0, 0, null, "0 results found", "''");

            var htmlDom = searchPage.RenderAsHtml(jobProfileSearchViewModel);

            var searchTitle = htmlDom.DocumentNode.SelectNodes("//h1[contains(@class, 'search-title')]").FirstOrDefault().InnerText;
            var searchBox = htmlDom.DocumentNode.SelectNodes("//input[contains(@id, 'search-main')]");
            var resultMessage = htmlDom.DocumentNode.SelectNodes("//div[contains(@class, 'result-count')]").FirstOrDefault().InnerText;

            searchTitle.Should().NotBeNullOrEmpty();
            searchBox.Should().NotBeNullOrEmpty();
            resultMessage.Should().NotBeNullOrEmpty();
            resultMessage.Should().Contain("0 results found");
        }

        //As a Citizen, I want to navigate using the Next & Previous hyperlinks
        [Theory]
        [InlineData(1, "test.co.uk", "2 of 10", null, null, 20, 10, "102 results found", "nurse")]
        [InlineData(10, null, null, "test.co.uk", "9 of 10", 20, 10, "102 results found", "nurse")]
        [InlineData(5, "test.co.uk", "6 of 10", "test.co.uk", "4 of 10", 20, 10, "102 results found", "nurse")]
        public void DFC_167_PaginationControlTests(int pageNumber, string nextUrl, string nextUrlText, string previousUrl, string previousUrlText, int count, int totalPages, string resultsMessage, string searchTerm)
        {
            var searchView = new _MVC_Views_JobProfileSearchBox_SearchResult_cshtml();

            var jobProfileSearchViewModel = GenerateDummyJobProfileSearchResultViewModel(pageNumber, nextUrl, nextUrlText, previousUrl, previousUrlText, count, totalPages, DummyJobProfileSearchResultsWithAltTitle(), resultsMessage, searchTerm);

            var htmlDom = searchView.RenderAsHtml(jobProfileSearchViewModel);

            var nextUrllocal = htmlDom.DocumentNode.SelectNodes("//a[contains(@class, 'dfc-code-search-nextlink')]");
            string nextUrlTextlocal;
            var previousUrllocal = htmlDom.DocumentNode.SelectNodes("//a[contains(@class, 'dfc-code-search-previouslink')]");
            string previousUrlTextlocal;

            if (pageNumber == 1)
            {
                nextUrlTextlocal = htmlDom.DocumentNode.SelectNodes("//span[contains(@class, 'page-numbers')]").ElementAt(0).InnerText;

                nextUrllocal.Should().NotBeNullOrEmpty();
                nextUrlTextlocal.Should().Contain(pageNumber + 1 + " of " + totalPages);
                previousUrllocal.Should().BeNullOrEmpty();
            }
            else if (pageNumber == totalPages)
            {
                previousUrlTextlocal = htmlDom.DocumentNode.SelectNodes("//span[contains(@class, 'page-numbers')]").ElementAt(0).InnerText;

                nextUrllocal.Should().BeNullOrEmpty();
                previousUrllocal.Should().NotBeNullOrEmpty();
                previousUrlTextlocal.Should().Contain(pageNumber - 1 + " of " + totalPages);
            }
            else
            {
                nextUrlTextlocal = htmlDom.DocumentNode.SelectNodes("//span[contains(@class, 'page-numbers')]").ElementAt(0).InnerText;
                previousUrlTextlocal = htmlDom.DocumentNode.SelectNodes("//span[contains(@class, 'page-numbers')]").ElementAt(1).InnerText;

                nextUrllocal.Should().NotBeNullOrEmpty();
                nextUrlTextlocal.Should().Contain(pageNumber + 1 + " of " + totalPages);
                previousUrllocal.Should().NotBeNullOrEmpty();
                previousUrlTextlocal.Should().Contain(pageNumber - 1 + " of " + totalPages);
            }
        }

        //As a Citizen, I want to see the search term in the search box when displaying the search results
        //A1 - Search and check search term plus search box
        //A2 - Re-Search and check searchbox contains new search term
        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(3)]
        public void DFC_1495_AlsoFoundInCategories(int numberOfLinkedJobCategories)
        {
            var searchResultsView = new _MVC_Views_JobProfileSearchBox_SearchResult_cshtml();

            var jobProfileSearchResultsViewModel = GenerateDummyJobProfileSearchResultViewModel(
                1, "dummy-nextURL", "dummy-NextUrlText", "dummy-PreviousUrl", "dummy-PreviousUrlText", 1, 1, DummyJobProfileSearchResultsWithJobProfileCategories(numberOfLinkedJobCategories), "dummy result found", "Test");

            var htmlDom = searchResultsView.RenderAsHtml(jobProfileSearchResultsViewModel);

            // var foundInText = htmlDom.DocumentNode.SelectSingleNode("//p[contains(@class, 'results-categories')]").InnerText;
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
        [InlineData("20,000 to 30,000")]
        [InlineData("")]
        public void DFC_2047_JobProfileSalaryRange(string salaryRange)
        {
            // Arrange
            var searchView = new _MVC_Views_JobProfileSearchBox_SearchResult_cshtml();

            string salaryBlankText = "Vairable";

            var jobProfileSearchResultsViewModel = GenerateDummyJobProfileSearchResultViewModel(
                1, "next-url", "nextpage", "prev-url", "prevpage", 1, 1, DummyJobProfileSearchResultWithSalaryRange(salaryRange), "1 result found", "Test", salaryBlankText);

            // Act
            var htmlDom = searchView.RenderAsHtml(jobProfileSearchResultsViewModel);

            // Asserts
            var salaryRangeElement = htmlDom.DocumentNode.SelectNodes("//span[contains(@class, 'dfc-code-search-jpSalary')]").FirstOrDefault();
            var salaryRangeText = salaryRangeElement?.InnerText?.Trim();
            if (string.IsNullOrEmpty(salaryRange))
            {
                salaryRangeText.Should().Be(salaryBlankText);
            }
            else
            {
                salaryRangeText.Should().Be(salaryRange);
            }
        }

        public IEnumerable<JobProfileSearchResultItemViewModel> DummyJobProfileSearchResultWithSalaryRange(string salaryRange)
        {
            yield return new JobProfileSearchResultItemViewModel
            {
                Rank = 1,
                ResultItemTitle = "Test Result Title",
                ResultItemAlternativeTitle = "Test Alternative Title",
                ResultItemOverview = "OverView Text",
                ResultItemSalaryRange = salaryRange,
                ResultItemUrlName = "Test URL Name",
            };
        }

        public JobProfileSearchResultViewModel GenerateDummyJobProfileSearchResultViewModel(int pageNumber, string nextUrl, string nextUrlText, string previousUrl, string previousUrlText, int count, int totalPages, IEnumerable<JobProfileSearchResultItemViewModel> jobProfileSearchResult, string resultMessage, string searchTerm, string salaryBlankText = "Variable")
        {
            return new JobProfileSearchResultViewModel
            {
                PageNumber = pageNumber,
                NextPageUrl = new Uri(nextUrl),
                NextPageUrlText = nextUrlText,
                PreviousPageUrl = new Uri(previousUrl),
                PreviousPageUrlText = previousUrlText,
                Count = count,
                TotalPages = totalPages,
                SearchResults = jobProfileSearchResult,
                SearchTerm = searchTerm,
                TotalResultsMessage = resultMessage,
                JobProfileCategoryPage = "/job-categories/",
                SalaryBlankText = salaryBlankText
            };
        }

        public IEnumerable<JobProfileSearchResultItemViewModel> DummyJobProfileSearchResultsWithAltTitle()
        {
            yield return new JobProfileSearchResultItemViewModel
            {
                Rank = 1,
                ResultItemTitle = "Test Result Title",
                ResultItemAlternativeTitle = "Test Alternative Title",
                ResultItemOverview = "OverView Text",
                ResultItemSalaryRange = "Salary Average = £23,000 - £101,000",
                ResultItemUrlName = "Test URL Name",
            };
        }

        public IEnumerable<JobProfileSearchResultItemViewModel> DummyJobProfileSearchResultsWithoutAltTitle()
        {
            yield return new JobProfileSearchResultItemViewModel
            {
                Rank = 1,
                ResultItemTitle = "Test Result Title",
                ResultItemOverview = "OverView Text",
                ResultItemSalaryRange = "Salary Average = £23,000 - £101,000",
                ResultItemUrlName = "Test URL Name",
            };
        }

        public IEnumerable<JobProfileSearchResultItemViewModel> DummyJobProfileSearchResultsWithJobProfileCategories(int numberOfLinkedJobCategories)
        {
            yield return new JobProfileSearchResultItemViewModel
            {
                Rank = 1,
                ResultItemTitle = "Test Result Title",
                ResultItemAlternativeTitle = "Test Alternative Title",
                ResultItemOverview = "OverView Text",
                ResultItemSalaryRange = "Salary Average = £23,000 - £101,000",
                ResultItemUrlName = "Test URL Name",
                JobProfileCategoriesWithUrl = numberOfLinkedJobCategories > 0 ? GetLinkedCategories(numberOfLinkedJobCategories, string.Empty) : null
            };
        }

        /// <summary>
        /// Generates test data for categorys linked to a job profile.
        /// </summary>
        /// <param name="numberOfLinkedJobCategories">Number of entries to create</param>
        /// <param name="baseUrl">For genetaring the model this will be blank, when used to check the results in the view this should be the base url for the categories controller route</param>
        /// <returns>List of requested categories</returns>
        public IEnumerable<string> GetLinkedCategories(int numberOfLinkedJobCategories, string baseUrl)
        {
            if (numberOfLinkedJobCategories <= 0)
            {
                yield return null;
            }

            for (int ii = 0; ii < numberOfLinkedJobCategories; ii++)
            {
                yield return $"category{ii.ToString()}|{baseUrl}categoryURL{ii.ToString()}";
            }
        }

        public IEnumerable<JobProfileSearchResultItemViewModel> DummyMultipleJobProfileSearchResults()
        {
            yield return new JobProfileSearchResultItemViewModel
            {
                Rank = 1,
                ResultItemTitle = "Test Result Title",
                ResultItemAlternativeTitle = "Alt Title 1",
                ResultItemOverview = "OverView Text",
                ResultItemSalaryRange = "Salary Average = £23,000 - £101,000",
                ResultItemUrlName = "Test URL Name",
            };
            yield return new JobProfileSearchResultItemViewModel
            {
                Rank = 2,
                ResultItemTitle = "Test Title 2",
                ResultItemAlternativeTitle = "Alt Title 2",
                ResultItemOverview = "OverView Text 2",
                ResultItemSalaryRange = "Salary Average = £23,000 - £101,000",
                ResultItemUrlName = "Test URL Name 2",
            };
        }

        // DFC-1496 Autocomplete
        // A1 - Check whether AutoCompleteMinimumCharacters gets displayed as data attribute for input field
        [Theory]
        [InlineData(2)]
        [InlineData(0)]
        public void DFC_1496_A1_Autocomplete(int autoCompleteMinimumCharacters)
        {
            var indexView = new _MVC_Views_JobProfileSearchBox_Index_cshtml();
            var jobProfileSearchBoxViewModel = GenerateDummyJobProfileSearchBoxViewModel(string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, autoCompleteMinimumCharacters);
            var htmlDom = indexView.RenderAsHtml(jobProfileSearchBoxViewModel);
            var minlengthValue = htmlDom.DocumentNode.SelectNodes("//input").FirstOrDefault().Attributes["data-autocomplete-minlength"].Value;
            minlengthValue.Should().Be(autoCompleteMinimumCharacters.ToString());
        }

        public JobProfileSearchBoxViewModel GenerateDummyJobProfileSearchBoxViewModel(string placeHolderText, string headerText, string totalResultsMessage, string searchTerm, string jobProfileUrl, int autoCompleteMinimumCharacters)
        {
            return new JobProfileSearchBoxViewModel
            {
                PlaceholderText = placeHolderText,
                HeaderText = headerText,
                TotalResultsMessage = totalResultsMessage,
                SearchTerm = searchTerm,
                JobProfileUrl = new Uri(jobProfileUrl),
                AutoCompleteMinimumCharacters = autoCompleteMinimumCharacters
            };
        }

        private IEnumerable<string> GetDisplayedViewAnchorLinks(HtmlNode htmlNode)
        {
            return htmlNode.Descendants("a").Select(n =>
                {
                    return $"{n.InnerText}|{n.GetAttributeValue("href", string.Empty)}";
                })
                .ToList();
        }
    }
}