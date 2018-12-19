using System.Collections.Generic;

namespace DFC.Digital.Web.Sitefinity.JobProfileModule.Mvc.Models
{
    public class JobProfileSearchResultItemViewModel
    {
        public int Rank { get; set; }

        public string ResultItemTitle { get; set; }

        public string ResultItemAlternativeTitle { get; set; }

        public string ResultItemOverview { get; set; }

        public string ResultItemSalaryRange { get; set; }

        public string ResultItemUrlName { get; set; }

        public IEnumerable<string> JobProfileCategoriesWithUrl { get; set; }

        public string Score { get; set; }
    }
}