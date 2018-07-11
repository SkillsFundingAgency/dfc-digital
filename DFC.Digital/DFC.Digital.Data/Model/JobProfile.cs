using DFC.Digital.Data.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DFC.Digital.Data.Model
{
    /// <summary>
    /// Job Profile Data Model
    /// </summary>
    /// <seealso cref="DFC.Digital.Data.Interfaces.IDigitalDataModel" />
    public class JobProfile : IDigitalDataModel
    {
        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the alternative title.
        /// </summary>
        /// <value>
        /// The alternative title.
        /// </value>
        public string AlternativeTitle { get; set; }

        /// <summary>
        /// Gets or sets the overview.
        /// </summary>
        /// <value>
        /// The overview.
        /// </value>
        public string Overview { get; set; }

        /// <summary>
        /// Gets or sets the salary range.
        /// </summary>
        /// <value>
        /// The salary range.
        /// </value>
        public string SalaryRange { get; set; }

        /// <summary>
        /// Gets or sets the soc code.
        /// </summary>
        /// <value>
        /// The soc code.
        /// </value>
        public string SOCCode { get; set; }

        /// <summary>
        /// Gets or sets the name of the URL.
        /// </summary>
        /// <value>
        /// The name of the URL.
        /// </value>
        public string UrlName { get; set; }

        /// <summary>
        /// Gets or sets the how to become.
        /// </summary>
        /// <value>
        /// The how to become.
        /// </value>
        public string HowToBecome { get; set; }

        /// <summary>
        /// Gets or sets the what you will do.
        /// </summary>
        /// <value>
        /// The what you will do.
        /// </value>
        public string WhatYouWillDo { get; set; }

        /// <summary>
        /// Gets or sets the skills.
        /// </summary>
        /// <value>
        /// The skills.
        /// </value>
        public string Skills { get; set; }

        public string DigitalSkillsLevel { get; set; }

        public IEnumerable<WhatItTakesSkill> WhatItTakesSkills { get; set; }

        public IEnumerable<Restriction> Restrictions { get; set; }

        public string OtherRequirements { get; set; }

        /// <summary>
        /// Gets or sets the salary.
        /// </summary>
        /// <value>
        /// The salary.
        /// </value>
        public string Salary { get; set; }

        /// <summary>
        /// Gets or sets the working hours patterns and environment.
        /// </summary>
        /// <value>
        /// The working hours patterns and environment.
        /// </value>
        public string WorkingHoursPatternsAndEnvironment { get; set; }

        /// <summary>
        /// Gets or sets the career path and progression.
        /// </summary>
        /// <value>
        /// The career path and progression.
        /// </value>
        public string CareerPathAndProgression { get; set; }

        /// <summary>
        /// Gets or sets the course keywords.
        /// </summary>
        /// <value>
        /// The course keywords.
        /// </value>
        public string CourseKeywords { get; set; }

        public decimal? MinimumHours { get; set; }

        public decimal? MaximumHours { get; set; }

        public string WorkingHoursDetails { get; set; }

        public IList<Guid> JobProfileCategoryIdCollection { get; set; }

        public bool? DoesNotExistInBAU { get; set; }

        public string BAUSystemOverrideUrl { get; set; }

        /// <summary>
        /// Gets or sets the related interests identifier URL name collection.
        /// </summary>
        /// <value>
        /// The related interests identifier URL name collection.
        /// </value>
        [JsonIgnore]
        public IQueryable<string> RelatedInterests { get; set; }

        /// <summary>
        /// Gets or sets the related entry qualifications identifier URL name collection.
        /// </summary>
        /// <value>
        /// The related entry qualifications identifier URL name collection.
        /// </value>
        [JsonIgnore]
        public IQueryable<string> RelatedEntryQualifications { get; set; }

        /// <summary>
        /// Gets or sets the related enablers identifier URL name collection.
        /// </summary>
        /// <value>
        /// The related enablers identifier URL name collection.
        /// </value>
        [JsonIgnore]
        public IQueryable<string> RelatedEnablers { get; set; }

        /// <summary>
        /// Gets or sets the related training routes identifier URL name collection.
        /// </summary>
        /// <value>
        /// The related training routes identifier URL name collection.
        /// </value>
        [JsonIgnore]
        public IQueryable<string> RelatedTrainingRoutes { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether IsLMISalaryFeedOverriden is selected.
        /// </summary>
        /// <value>
        /// The IsLMISalaryFeedOverriden.
        /// </value>
        public bool? IsLMISalaryFeedOverriden { get; set; }

        /// <summary>
        /// Gets or sets the SalaryStarter.
        /// </summary>
        /// <value>
        /// The SalaryStarter.
        /// </value>
        public decimal? SalaryStarter { get; set; }

        /// <summary>
        /// Gets or sets the SalaryExperienced.
        /// </summary>
        /// <value>
        /// The SalaryExperienced.
        /// </value>
        public decimal? SalaryExperienced { get; set; }

        /// <summary>
        /// Gets or sets the related preferred task types.
        /// </summary>
        /// <value>
        /// The related preferred task types.
        /// </value>
        [JsonIgnore]
        public IQueryable<string> RelatedPreferredTaskTypes { get; set; }

        /// <summary>
        /// Gets or sets the related job areas.
        /// </summary>
        /// <value>
        /// The related job areas.
        /// </value>
        [JsonIgnore]
        public IQueryable<string> RelatedJobAreas { get; set; }

        public string WorkingPattern { get; set; }

        public string WorkingPatternDetails { get; set; }

        public bool? IsImported { get; set; }

        public HowToBecome HowToBecomeData { get; set; }
    }
}