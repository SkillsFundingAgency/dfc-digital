using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using System;
using System.Collections.Generic;

namespace DFC.Digital.Repository.SitefinityCMS
{
    public class ManageBauJobProfilesService : IManageBauJobProfilesService
    {
        public IEnumerable<BauJobProfile> SelectMarkedJobProfiles(IEnumerable<BauJobProfile> bauJobProfiles, IEnumerable<string> jobProfileUrlNames)
        {
            return bauJobProfiles;
        }
    }
}
