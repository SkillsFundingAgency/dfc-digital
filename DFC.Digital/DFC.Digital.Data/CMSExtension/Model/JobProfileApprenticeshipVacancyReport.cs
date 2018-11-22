using DFC.Digital.Data.Interfaces;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace DFC.Digital.Data.Model
{
    public class JobProfileApprenticeshipVacancyReport : IDigitalDataModel
    {
        public JobProfileReport JobProfile { get; set; }

        public SocCodeReport SocCode { get; set; }

        [JsonIgnore]
        public IEnumerable<ApprenticeshipVacancyReport> ApprenticeshipVacancies { get; set; }
    }
}
