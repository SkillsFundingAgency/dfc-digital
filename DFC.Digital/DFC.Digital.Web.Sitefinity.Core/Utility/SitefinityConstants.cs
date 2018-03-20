namespace DFC.Digital.Web.Sitefinity.Core
{
    public sealed class SitefinityConstants
    {
        /// <summary>
        /// The custom widget section
        /// </summary>
        public const string CustomWidgetSection = "DFC Widgets";

        /// <summary>
        /// The custom admin widget section
        /// </summary>
        public const string CustomAdminWidgetSection = "DFC Admin Widgets";

        /// <summary>
        /// Global JobProfile Settings Widget
        /// </summary>
        public const string JobProfileSettingsWidget = "JobProfile Settings and Preview";

        public const string DefaultJobProfileUrlName = "DefaultJobProfileUrlName";

        /// <summary>
        /// Initializes a new instance of the <see cref="SitefinityConstants"/> class.
        /// Prevent this class from being initialised
        /// </summary>
        private SitefinityConstants()
        {
        }
    }
}