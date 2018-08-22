using DFC.Digital.Data.Model;
using System.Collections.Generic;
using System.Linq;

namespace DFC.Digital.Data.Interfaces
{
    public interface IJobProfileSocCodeRepository
    {
        IQueryable<ApprenticeVacancy> GetApprenticeVacanciesBySocCode(string socCode);

        RepoActionResult UpdateSocOccupationalCode(SocCode socCode);

        IQueryable<SocCode> GetSocCodes();

        IEnumerable<SocSkillMatrix> GetSocSkillMatricesBySocCode(string socCode);

        IEnumerable<JobProfileOverloadForWhatItTakes> GetLiveJobProfilesBySocCode(string socCode);
    }
}