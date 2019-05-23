using System;

namespace DFC.Digital.Web.Sitefinity.CourseModule
{
    public class PaginationViewModel
    {
        public Uri NextPageUrl { get; set; }

        public Uri PreviousPageUrl { get; set; }

        public string NextPageText { get; set; }

        public string PreviousPageText { get; set; }

        public bool HasPreviousPage { get; set; }

        public bool HasNextPage { get; set; }
    }
}