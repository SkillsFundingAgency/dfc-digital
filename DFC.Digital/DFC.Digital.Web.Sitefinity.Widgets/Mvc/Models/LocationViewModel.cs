using DFC.Digital.Data.Model;
using System.Collections.Generic;

namespace DFC.Digital.Web.Sitefinity.Widgets.Mvc.Models
{
    /// <summary>
    /// Breadcrumb Widget View Model
    /// </summary>
    public class LocationViewModel
    {
        /// <summary>
        /// Gets or sets the Region or City.
        /// </summary>
        /// <value>
        /// Region.
        /// </value>
        public string Region { get; set; }

        /// <summary>
        /// Gets or sets the country.
        /// </summary>
        /// <value>
        /// Country.
        /// </value>
        public string Country { get; set; }

        /// <summary>
        /// Gets or sets the CountryCode.
        /// </summary>
        /// <value>
        /// CountryCode.
        /// </value>
        public string CountryCode { get; set; }

        /// <summary>
        /// Gets or sets the IPAddress.
        /// </summary>
        /// <value>
        /// IPAddress.
        /// </value>
        public string IPAddress { get; set; }
    }
}