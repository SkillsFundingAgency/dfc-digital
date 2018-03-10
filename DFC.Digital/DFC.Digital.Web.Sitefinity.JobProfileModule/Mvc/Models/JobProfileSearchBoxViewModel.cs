using System.Web.Mvc;

namespace DFC.Digital.Web.Sitefinity.JobProfileModule.Mvc.Models
{
    /// <summary>
    /// Job Profile Search Box Widget View Model
    /// </summary>
    public class JobProfileSearchBoxViewModel
    {
        /// <summary>
        /// Gets or sets the place holder text.
        /// </summary>
        /// <value>
        /// The place holder text.
        /// </value>
        public string PlaceholderText { get; set; }

        /// <summary>
        /// Gets or sets the header text.
        /// </summary>
        /// <value>
        /// The header text.
        /// </value>
        public string HeaderText { get; set; }

        /// <summary>
        /// Gets or sets message to be displayed when there are no results
        /// </summary>
        public string TotalResultsMessage { get; set; }

        /// <summary>
        /// Gets or sets the SearchTerm.
        /// [TODO] Field Validation to disallow illegal characters explicity
        /// </summary>
        /// <value>
        /// The SearchTerm.
        /// </value>
        [AllowHtml]
        public string SearchTerm { get; set; }

        /// <summary>
        /// Gets or sets the job profile URL.
        /// </summary>
        /// <value>
        /// The job profile URL.
        /// </value>
        public string JobProfileUrl { get; set; }

        /// <summary>
        /// Gets or sets the AutoComplete Minimum Characters
        /// </summary>
        /// <value>
        /// The AutoComplete Minimum Characters.
        /// </value>
        public int AutoCompleteMinimumCharacters { get; set; }

        /// <summary>
        /// Gets or sets the Maximum Number ff displayed Suggestions.
        /// </summary>
        /// <value>
        /// The Maximum Number ff displayed Suggestions.
        /// </value>
        public int MaximumNumberOfDisplayedSuggestions { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [use fuzzy automatic complete matching].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [use fuzzy automatic complete matching]; otherwise, <c>false</c>.
        /// </value>
        public bool UseFuzzyAutoCompleteMatching { get; set; }
    }
}