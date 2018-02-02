using System.Linq;

namespace DFC.Digital.Data.Model
{
    public class JobProfileCategory
    {
        public string Name { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public IQueryable<JobProfileCategory> SubCategories { get; set; }

        public string Url { get; set; }
    }
}