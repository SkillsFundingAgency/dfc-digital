using DFC.Digital.Data.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DFC.Digital.Data.Interfaces
{
    public interface IJobProfileIndexEnhancer
    {
        void Initialise(JobProfileIndex initialiseJobProfileIndex, bool isPublishing);

        void PopulateRelatedFieldsWithUrl();

        Task<JobProfileSalary> PopulateSalary(string socCode, string jobProfileUrlName);

        IEnumerable<JobProfileOverloadSearchExtended> GetAllSearchProfiles();
    }
}