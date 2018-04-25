using DFC.Digital.Core;
using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DFC.Digital.Repository.SitefinityCMS
{
    public class ManageBauJobProfilesService : IManageBauJobProfilesService
    {
        private readonly IBauJobProfileOdataRepository bauJobProfileRepository;
        private readonly IAsyncHelper asyncHelper;

        public ManageBauJobProfilesService(IBauJobProfileOdataRepository bauJobProfileRepository, IAsyncHelper asyncHelper)
        {
            this.bauJobProfileRepository = bauJobProfileRepository;
            this.asyncHelper = asyncHelper;
        }

        public IEnumerable<JobProfileImporting> SelectMarkedJobProfiles(IEnumerable<JobProfileImporting> bauJobProfiles, List<JobProfileImporting> markedJobProfiles)
        {
            if (markedJobProfiles.Any())
            {
                var selectedJobProfiles = bauJobProfiles.Where(o => markedJobProfiles.Any(f => f.UrlName.Equals(o.UrlName, StringComparison.OrdinalIgnoreCase)));

                foreach (var selectedJobProfile in selectedJobProfiles)
                {
                    selectedJobProfile.CourseKeywords = markedJobProfiles.SingleOrDefault(jp => jp.UrlName.Equals(selectedJobProfile.UrlName, StringComparison.OrdinalIgnoreCase)).CourseKeywords;
                }

                return selectedJobProfiles;
            }
            else
            {
                return new List<JobProfileImporting>();
            }
        }
    }
}
