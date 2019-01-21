using DFC.Digital.Data.Interfaces;
using System;

namespace DFC.Digital.Data.Model
{
    public class DfcPageSiteNode : IDigitalDataModel
    {
        /// <summary>
        /// Gets or sets the Url.
        /// </summary>
        /// <value>
        /// A string that represents the Url of the DFC Page Node.
        /// </value>
        public Uri Url { get; set; }

        /// <summary>
        /// Gets or sets the Title.
        /// </summary>
        /// <value>
        /// A string that represents the Title of the DFC Page Node.
        /// </value>
        public string Title { get; set; }

        public bool Visible { get; set; }

        public bool IsStandardPage { get; set; }

        public string Parent { get; set; }
    }
}