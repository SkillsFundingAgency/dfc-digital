using DFC.Digital.Data.Model;
using System.Collections.Generic;

namespace DFC.Digital.Data.Interfaces
{
    /// <summary>
    /// Page Content service interface
    /// </summary>
    public interface IJobProfilePage
    {
        IEnumerable<JobProfileSection> GetJobProfileSections(IEnumerable<JobProfileSectionFilter> sectionFilters);

        JobProfileSection GetFirstMatchedJobProfileSection(JobProfileSectionFilter sectionFilter);
    }
}