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
            var homePageEntry = entries.Single(x => x.Location.EndsWith("/home", System.StringComparison.InvariantCultureIgnoreCase));
            var homePage = homePageEntry.Location.Substring(0, homePageEntry.Location.IndexOf("/home"));
            entries.Add(new SitemapEntry
            {
                Location = $"{homePage}",
                Priority = homePageEntry.Priority
            });

            entries.RemoveAll(x => x.Location.ToUpperInvariant().EndsWith("/JOB-CATEGORIES")
                                || x.Location.ToUpperInvariant().EndsWith("/JOB-PROFILES")
                                || x.Location.EndsWith("/home", System.StringComparison.InvariantCultureIgnoreCase));

            return entries.OrderBy(x => x.Location);
        }
    }
}