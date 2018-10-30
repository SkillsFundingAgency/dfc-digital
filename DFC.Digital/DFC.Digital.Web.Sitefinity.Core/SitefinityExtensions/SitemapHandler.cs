using DFC.Digital.Data.Interfaces;
using System.Collections.Generic;
using System.Linq;
using Telerik.Sitefinity.SitemapGenerator.Data;

namespace DFC.Digital.Web.Sitefinity.Core
{
    public class SitemapHandler
    {
        private readonly IJobProfileCategoryRepository categoryRepository;

        public SitemapHandler(IJobProfileCategoryRepository categoryRepository)
        {
            this.categoryRepository = categoryRepository;
        }

        public IEnumerable<SitemapEntry> ManipulateSitemap(List<SitemapEntry> entries)
        {
            // Add categories
            var jobCategoryPageEntry = entries.FirstOrDefault(x => x.Location.ToUpperInvariant().EndsWith("/JOB-CATEGORIES"));
            if (jobCategoryPageEntry != null)
            {
                var cats = categoryRepository.GetJobProfileCategories();

                foreach (var category in cats)
                {
                    // adds the new sitemap entry to the collection of the entries
                    entries.Add(new SitemapEntry
                    {
                        Location = $"{jobCategoryPageEntry.Location}/{category.Url}",
                        Priority = jobCategoryPageEntry.Priority
                    });
                }
            }

            //Clean up
            entries.RemoveAll(x => x.Location.ToUpperInvariant().EndsWith("/JOB-CATEGORIES")
                                || x.Location.ToUpperInvariant().EndsWith("/JOB-PROFILES"));

            return entries.OrderBy(x => x.Location);
        }
    }
}