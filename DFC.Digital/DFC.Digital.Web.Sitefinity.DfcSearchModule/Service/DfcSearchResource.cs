using Telerik.Sitefinity.Localization;
using Telerik.Sitefinity.Localization.Data;

namespace DFC.Digital.Web.Sitefinity.DfcSearchModule
{
    [ObjectInfo("DfcSearchServiceResources", ResourceClassId = "DfcSearchServiceResources")]
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class DfcSearchResource : Resource
    {
        public DfcSearchResource()
        {
        }

        public DfcSearchResource(string dataProviderName) : base(dataProviderName)
        {
        }

        public DfcSearchResource(ResourceDataProvider dataProvider) : base(dataProvider)
        {
        }

        #region Class Description

        /// <summary>
        /// Gets amazon cloud search
        /// </summary>
        [ResourceEntry("DfcSearchResourcesTitle", Value = "Dfc cloud search", Description = "The title of this class.", LastModified = "2017/08/07")]
        public string DfcSearchResourcesTitle
        {
            get
            {
                return this["SearchResourcesTitle"];
            }
        }

        /// <summary>
        /// Gets Contains localizable resources for Amazon cloud search.
        /// </summary>
        [ResourceEntry("DfcSearchResourcesDescription", Value = "Contains localizable resources for Dfc cloud search.", Description = "The description of this class.", LastModified = "2017/08/07")]
        public string DfcSearchResourcesDescription
        {
            get
            {
                return this["DfcSearchResourcesDescription"];
            }
        }

        #endregion Class Description

        /// <summary>
        /// Gets phrase: Amazon Search
        /// </summary>
        /// <value>Amazon Search</value>
        [ResourceEntry("DfcSearchService", Value = "Digital first careers cloud search service", Description = "phrase: Digital first careers Cloud Search ", LastModified = "2017/08/09")]
        public string DfcSearchService
        {
            get
            {
                return this["DfcSearchService"];
            }
        }
    }
}