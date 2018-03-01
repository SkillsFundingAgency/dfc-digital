using DFC.Digital.Data.Model;
using System.Threading.Tasks;

namespace DFC.Digital.Data.Interfaces
{
    public interface IJobProfileIndexEnhancer
    {
        JobProfileIndex GetRelatedFieldsWithUrl(JobProfileIndex jobProfileIndex);

        void Initialise(JobProfileIndex jobProfile);

        Task<JobProfileIndex> GetSalaryRangeAsync(JobProfileIndex jobProfileIndex);
    }
}