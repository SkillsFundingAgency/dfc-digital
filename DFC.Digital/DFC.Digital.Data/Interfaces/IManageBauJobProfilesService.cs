using DFC.Digital.Data.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DFC.Digital.Data.Interfaces
{
    public interface IManageBauJobProfilesService
    {
        IEnumerable<JobProfileImporting> SelectMarkedJobProfiles(IEnumerable<JobProfileImporting> bauJobProfiles, List<JobProfileImporting> markedJobPriles);
    }
}
