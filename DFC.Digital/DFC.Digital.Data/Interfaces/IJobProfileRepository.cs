using DFC.Digital.Data.Model;
using System;
using System.Collections.Generic;

namespace DFC.Digital.Data.Interfaces
{
    public interface IJobProfileRepository
    {
        JobProfile GetByUrlName(string urlName);

        JobProfile GetByUrlNameForPreview(string urlName);

        string GetProviderName();

        Type GetContentType();

        JobProfile GetByUrlNameForSearchIndex(string urlName, bool isPublishing);

        IEnumerable<JobProfileOverloadForWhatItTakes> GetLiveJobProfiles();

        RepoActionResult UpdateDigitalSkill(JobProfileOverloadForWhatItTakes jobProfile);

        RepoActionResult UpdateSocSkillMatrices(JobProfileOverloadForWhatItTakes jobProfile, IEnumerable<SocSkillMatrix> socSkillMatrices);
    }
}