using DFC.FindACourseClient.Models.ExternalInterfaceModels;
using System;

namespace DFC.Digital.Web.Sitefinity.CourseModule
{
    public class OrderByLinks
    {
        public CourseSearchOrderBy OrderBy { get; set; }

        public Uri OrderByStartDateUrl { get; set; }

        public Uri OrderByRelevanceUrl { get; set; }

        public Uri OrderByDistanceUrl { get; set; }

        public string OrderByText { get; set; }

        public string RelevanceOrderByText { get; set; }

        public string DistanceOrderByText { get; set; }

        public string StartDateOrderByText { get; set; }
    }
}