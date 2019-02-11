namespace DFC.Digital.Data.Model
{
    public class CourseSearchRequest
    {
        public string SearchTerm { get; set; }

        public int PageNumber { get; set; }

        public int RecordsPerPage { get; set; }

        public bool Dfe1619Funded { get; set; }
    }
}
