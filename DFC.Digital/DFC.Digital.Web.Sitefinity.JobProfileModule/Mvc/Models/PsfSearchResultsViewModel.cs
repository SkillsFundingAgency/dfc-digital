using System;
using System.Web.Mvc;

namespace DFC.Digital.Web.Sitefinity.JobProfileModule.Mvc.Models
{
    /// <summary>
    /// Pre Search Filter Results View Model
    /// </summary>
    /// <seealso cref="DFC.Digital.Web.Sitefinity.JobProfileModule.Mvc.Models.JobProfileSearchResultViewModel" />
    public class PsfSearchResultsViewModel : JobProfileSearchResultViewModel
    {
        /// <summary>
        /// Gets or sets the main page title.
        /// </summary>
        /// <value>
        /// The main page title.
        /// </value>
        public string MainPageTitle { get; set; }

        /// <summary>
        /// Gets or sets the secondary text.
        /// </summary>
        /// <value>
        /// The secondary text.
        /// </value>
        public string SecondaryText { get; set; }

        /// <summary>
        /// Gets or sets the back page URL.
        /// </summary>
        /// <value>
        /// The back page URL.
        /// </value>
        public Uri BackPageUrl { get; set; }

        /// <summary>
        /// Gets or sets the previous page URL text.
        /// </summary>
        /// <value>
        /// The back page URL text.
        /// </value>
        public string BackPageUrlText { get; set; }

        /// <summary>
        /// Gets or sets the pre search filters model.
        /// </summary>
        /// <value>
        /// The pre search filters model.
        /// </value>
        public PsfModel PreSearchFiltersModel { get; set; }

        public MvcHtmlString CaveatTagMarkup { get; internal set; }

        public MvcHtmlString CaveatMarkup { get; internal set; }
    }
}