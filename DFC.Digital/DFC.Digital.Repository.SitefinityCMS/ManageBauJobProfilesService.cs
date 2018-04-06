using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DFC.Digital.Repository.SitefinityCMS
{
    public class ManageBauJobProfilesService : IManageBauJobProfilesService
    {
        public IEnumerable<BauJobProfile> SelectMarkedJobProfiles(IEnumerable<BauJobProfile> bauJobProfiles, IEnumerable<string> jobProfileUrlNames)
        {
            var profileUrlNames = jobProfileUrlNames as IList<string> ?? jobProfileUrlNames.ToList();
            return profileUrlNames.Any() ? bauJobProfiles.Where(jp => profileUrlNames.Any(url => url.Equals(jp.UrlName, StringComparison.OrdinalIgnoreCase))) : new List<BauJobProfile>();
        }
    }
}
