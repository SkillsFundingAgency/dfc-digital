using System;
using System.Linq;

namespace DFC.Digital.Data.Model
{
    public class JobProfileCategory
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public IQueryable<JobProfileCategory> Subcategories { get; set; }

        public string Url { get; set; }
    }
}