using DFC.Digital.Web.Sitefinity.JobProfileModule.Mvc.Models;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using TestStack.Seleno.PageObjects;

namespace DFC.Digital.AcceptanceTest.Infrastructure
{
    public class SearchPage : DFCPageWithViewModel<JobProfileSearchResultViewModel>
    {
        #region const IDs and Classnames

        private const string SearchResultItem = "dfc-code-search-resultitem";
        private const string SearchJpTitle = "dfc-code-search-jpTitle";
        private const string SearchDataRank = "data-ga-rank";

        #endregion const IDs and Classnames
        public int TotalPageCount
        {
            get
            {
                var pagingButtonText = HasNextPage ? NextPageNumbersLabel : HasPreviousPage ? PreviousPageNumbersLabel : null;
                var totalPageNumber = pagingButtonText?.Substring(pagingButtonText.IndexOf("of", StringComparison.Ordinal) + 2).Trim();
                return Convert.ToInt32(totalPageNumber ?? "1");
            }
        }

        public int NumberOfJobProfilesDisplayed => Find.Elements(By.ClassName("dfc-code-search-resultitem")).Count();
        #region Internal Properties

        internal bool HasJobProfileTitle => DoesElementExistWithValue("dfc-code-search-jpTitle");

        internal bool HasAlternativeTitle => DoesElementExistWithValue("dfc-code-search-jpAltTitle");

        internal bool HasSearchBox => Find.Element(By.Id("search-main")) != null;

        internal bool HasFoundInField => Find.Element(By.ClassName("results-categories")) != null;

        internal string HasDidYouMeanText => Find.OptionalElement(By.ClassName("search-dym"))?.Text;

        internal string SearchTitle => Find.Element(By.ClassName("search-title")).Text;

        internal bool HasSignPostBanner => Find.OptionalElement(By.ClassName("signpost")) != null;

        internal bool HasRankId
        {
            get
            {
                var rankId = Find.OptionalElement(By.ClassName(SearchResultItem))
                    ?.GetAttribute(SearchDataRank);

                return !string.IsNullOrWhiteSpace(rankId);
            }
        }

        internal int DistinctRankIds
        {
            get
            {
                return Find.Elements(By.ClassName("dfc-code-search-resultitem"))
                    .Select(item => item.GetAttribute("data-ga-rank"))
                    .Distinct().Count();
            }
        }

        internal int HighestRankOnPage
        {
            get
            {
                return Find.Elements(By.ClassName("dfc-code-search-resultitem"))
                    .Select(item => Convert.ToInt32(item.GetAttribute("data-ga-rank")))
                    .Max();
            }
        }

        internal int LowestRankOnPage
        {
            get
            {
                return Find.Elements(By.ClassName("dfc-code-search-resultitem"))
                    .Select(item => Convert.ToInt32(item.GetAttribute("data-ga-rank")))
                    .Min();
            }
        }

        internal string DisplayedTotalNumberOfResults => Find.Element(By.ClassName("result-count")).Text;

        internal string DidYouMeanText => Find.OptionalElement(By.CssSelector(".search-dym a"))?.Text;

        internal int TotalResultsCount
        {
            get
            {
                var totalCountMessage = DisplayedTotalNumberOfResults;
                var totalCount = totalCountMessage.Substring(0, totalCountMessage.IndexOf("result", StringComparison.Ordinal)).Trim();
                return Convert.ToInt32(totalCount);
            }
        }

        internal string SearchBoxValue => Find.Element(By.Id("search-main")).GetAttribute("value");

        internal string FirstJobProfileUrl => Find.OptionalElement(By.ClassName("dfc-code-search-jpTitle"))?.GetAttribute("href");

        internal bool HasNextPage => Find.OptionalElement(By.ClassName("dfc-code-search-next")) != null;

        internal bool HasPreviousPage => Find.OptionalElement(By.ClassName("dfc-code-search-previous")) != null;

        internal string PreviousPageNumbersLabel => Find.OptionalElement(By.CssSelector(".dfc-code-search-previous .page-numbers"))?.Text;

        internal string PreviousPageLabel => Find.OptionalElement(By.CssSelector(".dfc-code-search-previous .pagination-label"))?.Text;

        internal string NextPageNumbersLabel => Find.OptionalElement(By.CssSelector(".dfc-code-search-next .page-numbers"))?.Text;

        internal string NextPageLabel => Find.OptionalElement(By.CssSelector(".dfc-code-search-next .pagination-label"))?.Text;

        internal IEnumerable<string> DisplayedJobProfileTitles => Find.Elements(By.ClassName(SearchJpTitle)).Select(title => title.Text);
        #region Methods

        public string GetCategoryInSearchTitle(int result)
        {
            var results = Find.Elements(By.ClassName("dfc-code-search-resultitem")).ElementAt(result - 1);
            var categoryLink = results.FindElement(By.CssSelector(".results-categories a"));
            return categoryLink.Text;
        }

        public T Search<T>(JobProfileSearchResultViewModel model)
            where T : UiComponent, new()
        {
            Input.Model(model);
            return NavigateTo<T>(By.ClassName("button"));
        }

        public T GoToResult<T>(int index)
            where T : UiComponent, new()
        {
            var findBy = By.ClassName("dfc-code-search-jpTitle");
            var results = Find.Elements(findBy).ToList();
            return NavigateTo<T>(results[index - 1].GetAttribute("href"), findBy);
        }

        public T NextPage<T>()
            where T : UiComponent, new()
        {
            return NavigateTo<T>(By.ClassName("dfc-code-search-nextlink"));
        }

        public T PreviousPage<T>()
            where T : UiComponent, new()
        {
            return NavigateTo<T>(By.ClassName("dfc-code-search-previouslink"));
        }

        public T DidYouMeanPage<T>()
            where T : UiComponent, new()
        {
            return NavigateTo<T>(By.CssSelector(".search-dym a"));
        }

        public T GoToFirstCategoryLink<T>(int resultNo)
            where T : UiComponent, new()
        {
            var findBy = By.ClassName("dfc-code-search-resultitem");
            var results = Find.Elements(findBy).ElementAt(resultNo - 1);
            var categoryLink = results.FindElement(By.CssSelector(".results-categories a"));
            return NavigateTo<T>(categoryLink.GetAttribute("href"), findBy);
        }

        public T ClickSearchSignpostBanner<T>()
            where T : UiComponent, new()
        {
            return NavigateTo<T>(By.ClassName("signpost"));
        }

        #endregion

        internal string SelectedProfileUrl(int index)
        {
            string href = Find.Elements(By.ClassName(SearchJpTitle)).ElementAt(index - 1).GetAttribute("href");
            string url = href.Substring(href.LastIndexOf('/') + 1);
            return url;
        }

        internal string SelectedProfileTitle(int index) => Find.Elements(By.ClassName(SearchJpTitle)).ElementAt(index - 1).Text;

        #endregion Internal Properties

        #region Private helpers

        private bool DoesElementExistWithValue(string className)
        {
            return !string.IsNullOrWhiteSpace(Find.OptionalElement(By.ClassName(className))?.Text);
        }

        #endregion Private helpers
    }
}