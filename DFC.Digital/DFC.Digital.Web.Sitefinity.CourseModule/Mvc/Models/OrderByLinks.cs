using System;
using DFC.Digital.Data.Model;

namespace DFC.Digital.Web.Sitefinity.CourseModule
{
    public class OrderByLinks
    {

        public CourseSearchSortBy CourseSearchSortBy { get; set; }
        public Uri OrderByStartDateUrl { get; set; }
        public Uri OrderByRelevanceUrl { get; set; }

        public Uri OrderByDistanceUrl { get; set; }
    }
}