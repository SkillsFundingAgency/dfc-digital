using DFC.Digital.Data.Model;
using System.Collections.Generic;

namespace DFC.Digital.Data.Interfaces
{
    public interface IManageBauJobProfilesService
    {
        IEnumerable<BauJobProfile> SelectMarkedJobProfiles(IEnumerable<BauJobProfile> bauJobProfiles, IEnumerable<string> jobProfileUrlNames);
    }
}
