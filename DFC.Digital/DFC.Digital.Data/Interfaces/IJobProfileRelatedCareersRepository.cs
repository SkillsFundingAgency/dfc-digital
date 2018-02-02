using DFC.Digital.Data.Model;
using System.Collections.Generic;

namespace DFC.Digital.Data.Interfaces
{
    public interface IJobProfileRelatedCareersRepository
    {
        IEnumerable<JobProfileRelatedCareer> GetByParentName(string urlName, int maximumItemsToReturn);

        IEnumerable<JobProfileRelatedCareer> GetByParentNameForPreview(string urlName, int maximumItemsToReturn);
    }
}