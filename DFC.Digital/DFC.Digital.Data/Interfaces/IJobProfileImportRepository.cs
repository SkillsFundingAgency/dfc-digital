using DFC.Digital.Data.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DFC.Digital.Data.Interfaces
{
    public interface IJobProfileImportRepository
    {
        Task<IEnumerable<string>> ImportAsync(IEnumerable<JobProfile> jobProfiles, bool allowOverwrite, bool publish);

        Task<IEnumerable<string>> UpdateRelatedCareersAsync(string jobProfileUrl, IEnumerable<string> relatedJobProfiles, bool publish);
    }
}