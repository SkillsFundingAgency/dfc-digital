using DFC.Digital.Data.Model;
using System.Collections.Generic;
using System.Linq;

namespace DFC.Digital.Web.Sitefinity.JobProfileModule.Mvc.Models
{
    /// <summary>
    /// Job Profile Categories View Model
    /// </summary>
    public class JobProfileCategoriesViewModel
    {
        /// <summary>
        /// Gets or sets the job profile categories.
        /// </summary>
        /// <value>
        /// The job profile categories.
        /// </value>
        public IEnumerable<JobProfileCategory> JobProfileCategories { get; set; } = Enumerable.Empty<JobProfileCategory>();

        /// <summary>
        /// Gets a value indicating whether [job profile categories available].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [job profile categories available]; otherwise, <c>false</c>.
        /// </value>
        public bool JobProfileCategoriesAvailable => JobProfileCategories != null && JobProfileCategories.Any();

        public bool IsContentAuthoring { get; set; }
    }
}