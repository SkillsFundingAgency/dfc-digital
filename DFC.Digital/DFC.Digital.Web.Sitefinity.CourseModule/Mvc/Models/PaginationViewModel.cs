using System;

namespace DFC.Digital.Web.Sitefinity.CourseModule
{
    public class PaginationViewModel
    {
        public Uri NextPageUrl { get; set; }

        public Uri PreviousPageUrl { get; set; }

        public string NextPageUrlText { get; set; }

        public string PreviousPageUrlText { get; set; }

        public bool HasPreviousPage { get; set; }

        public bool HasNextPage { get; set; }

    }
}