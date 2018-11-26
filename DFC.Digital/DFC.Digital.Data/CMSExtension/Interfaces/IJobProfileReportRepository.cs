using DFC.Digital.Data.Model;
using System.Collections.Generic;

namespace DFC.Digital.Data.Interfaces
{
    public interface IJobProfileReportRepository
    {
        IEnumerable<JobProfileApprenticeshipVacancyReport> GetJobProfileApprenticeshipVacancyReport();
    }
}
