using DFC.Digital.Data.Model;

namespace DFC.Digital.Web.Sitefinity.JobProfileModule.Mvc.Models
{
    /// <summary>
    /// Job Profile Section View
    /// </summary>
    public class JobProfileSectionViewModel
    {
        /// <summary>
        /// Gets or sets the content of the top section.
        /// </summary>
        /// <value>
        /// The content of the top section.
        /// </value>
        public string TopSectionContent { get; set; }

        /// <summary>
        /// Gets or sets the content of the bottom section.
        /// </summary>
        /// <value>
        /// The content of the bottom section.
        /// </value>
        public string BottomSectionContent { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        /// <value>
        /// The property value.
        /// </value>
        public string PropertyValue { get; set; }

        /// <summary>
        /// Gets or sets the name of the property.
        /// </summary>
        /// <value>
        /// The name of the property.
        /// </value>
        public string PropertyName { get; set; }

        /// <summary>
        /// Gets or sets the restrictions other requirements.
        /// </summary>
        /// <value>
        /// The restrictions other requirements.
        /// </value>
        public RestrictionsOtherRequirements RestrictionsOtherRequirements { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is what it takes view.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is what it takes view; otherwise, <c>false</c>.
        /// </value>
        public bool IsWhatItTakesCadView { get; set; }
    }
}