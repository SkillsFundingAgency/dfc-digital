using DFC.Digital.Data.Model;
using System.Collections.Generic;
using System.Linq;

namespace DFC.Digital.AutomationTest.Utilities
{
    public static class DummyJobProfile
    {
        public static IEnumerable<JobProfile> GetDummyJobProfiles()
        {
            yield return new JobProfile
            {
                Title = "A Job Profile 1", AlternativeTitle = "D Alternative Title 1", Overview = "Overview 1", UrlName = "urlName_1", SalaryStarter = null, SocCode = "Soc 1"
            };
            yield return new JobProfile
            {
                Title = "B Job Profile 2", AlternativeTitle = "E Alternative Title 2", Overview = "Overview 2", UrlName = "urlName_2", SalaryStarter = null, SocCode = "Soc 2"
            };
            yield return new JobProfile
            {
                Title = "C Job Profile 3", AlternativeTitle = null, Overview = "Overview 3 - No alterative title", UrlName = "urlName_3", SalaryStarter = null, SocCode = "Soc 3"
            };
        }

        public static IEnumerable<JobProfile> GetDummyJobProfilesForCategory()
        {
            return GetDummyJobProfiles().Select(j => new JobProfile
            {
                Title = j.Title,
                AlternativeTitle = j.AlternativeTitle,
                Overview = j.Overview
            });
        }
    }
}