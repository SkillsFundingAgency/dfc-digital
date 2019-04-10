using DFC.Digital.Data.Model;
using System.Collections.Generic;
using System.Linq;

namespace DFC.Digital.AutomationTest.Utilities
{
    public sealed class DummyJobProfile
    {
        private DummyJobProfile()
        {
        }

        public static IEnumerable<JobProfile> GetDummyJobProfiles()
        {
            yield return new JobProfile { Title = "A Job Profile 1", AlternativeTitle = "D Alternative Title 1", Overview = "Overview 1", UrlName = "urlName_1", SalaryStarter = 0, SalaryExperienced = 0, SOCCode = "Soc 1", MinimumHours = 0, MaximumHours = 0 };
            yield return new JobProfile { Title = "B Job Profile 2", AlternativeTitle = "E Alternative Title 2", Overview = "Overview 2", UrlName = "urlName_2", SalaryStarter = 0, SalaryExperienced = 0, SOCCode = "Soc 2", MaximumHours = 0, MinimumHours = 0 };
            yield return new JobProfile { Title = "C Job Profile 3", AlternativeTitle = null, Overview = "Overview 3 - No alterative title", UrlName = "urlName_3", SalaryStarter = 0, SalaryExperienced = 0, SOCCode = "Soc 3", MinimumHours = 0, MaximumHours = 0 };
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