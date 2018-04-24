using System;
using System.Collections.Generic;

namespace DFC.Digital.Data.Model
{
   public class JobProfileImporting
    {
        public JobProfileImporting()
        {
            RelatedCareers = new List<JobProfileImporting>();
        }

        public Guid Id { get; set; }

        public string UrlName { get; set; }

        public string Title { get; set; }

        public string AlternativeTitles { get; set; }

        public string CourseKeywords { get; set; }

        public string Overview { get; set; }

        public string SalaryDescription { get; set; }

        public string SkillsYoullNeed { get; set; }

        public string EntryRequirements { get; set; }

        public string WhatYoullDo { get; set; }

        public string WorkingHoursPatternsAndEnvironment { get; set; }

        public string CareerPathAndProgression { get; set; }

        public IEnumerable<JobProfileImporting> RelatedCareers { get; set; }
    }
}
