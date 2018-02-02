using DFC.Digital.Data.Model;
using System.Linq;

namespace DFC.Digital.Data.Interfaces
{
    public interface IJobProfileSocCodeRepository
    {
        IQueryable<ApprenticeVacancy> GetBySocCode(string socCode);
    }
}