using DFC.Digital.Data.Model;
using System.Collections.Generic;
using System.Linq;

namespace DFC.Digital.Data.Interfaces
{
    public interface IJobProfileSocCodeRepository
    {
        IQueryable<ApprenticeVacancy> GetBySocCode(string socCode);

        RepoActionResult UpdateSocOccupationalCode(SocCode socCode);

        IQueryable<SocCode> GetLiveSocCodes();

        IEnumerable<SocSkillMatrix> GetSocSkillMatricesBySocCode(string socCode);
    }
}