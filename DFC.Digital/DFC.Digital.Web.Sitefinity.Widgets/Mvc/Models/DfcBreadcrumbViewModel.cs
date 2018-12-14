﻿using DFC.Digital.Data.Model;
using System.Collections.Generic;

namespace DFC.Digital.Web.Sitefinity.Widgets.Mvc.Models
{
    /// <summary>
    /// Breadcrumb Widget View Model
    /// </summary>
    public class DfcBreadcrumbViewModel
    {
        /// <summary>
        /// Gets or sets the Text of the home page.
        /// </summary>
        /// <value>
        /// The Text of the home page.
        /// </value>
        public string HomepageText { get; set; }

        /// <summary>
        /// Gets or sets the Link of the home page.
        /// </summary>
        /// <value>
        /// The Link of the home page.
        /// </value>
        public string HomepageLink { get; set; }

        /// <summary>
        /// Gets or sets the Title of the breadcrumbed page.
        /// </summary>
        /// <value>
        /// The breadcrumbedpPage Title text
        /// </value>
        public string BreadcrumbPageTitleText { get; set; }

        public IList<BreadCrumbLink> BreadcrumbLinks { get; set; }
    }
}