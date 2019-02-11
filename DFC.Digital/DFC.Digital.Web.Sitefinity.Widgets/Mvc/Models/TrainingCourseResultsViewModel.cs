using DFC.Digital.Data.Model;
using System.Collections.Generic;
using System.Linq;

namespace DFC.Digital.Web.Sitefinity.Widgets.Mvc.Models
{
    public class TrainingCourseResultsViewModel
    {
        public IEnumerable<Course> Courses { get; set; } = Enumerable.Empty<Course>();

        public int ResultsCount { get; set; }

        public int CurrentPageNumber { get; set; }

        public int TotalPagesCount { get; set; }

        public int RecordsPerPage { get; set; }

        public string NextPageLabel { get; set; }

        public string PreviousPageLabel { get; set; }

        public bool HasPreviousPage => CurrentPageNumber > 1;

        public bool HasNextPage => CurrentPageNumber < TotalPagesCount;

        public string RecordsOnPageLabel => $"{((CurrentPageNumber - 1) * RecordsPerPage) + 1} - {((CurrentPageNumber - 1) * RecordsPerPage) + Courses.Count()}";
    }
}