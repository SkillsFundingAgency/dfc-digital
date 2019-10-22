using DFC.Digital.Data.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DFC.Digital.Data.Model
{
    /// <summary>
    /// Job Profile Message Data Model
    /// </summary>
    public class JobProfileMessage : IDigitalDataModel
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        /// <value>
        /// The id.
        /// </value>
        public Guid JobProfileId { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the HtBTitlePrefix.
        /// </summary>
        /// <value>
        /// HtBTitlePrefix.
        /// </value>
        public string DynamicTitlePrefix { get; set; }

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
        public string SocLevelTwo { get; set; }

        /// <summary>
        /// Gets or sets the name of the URL.
        /// </summary>
        /// <value>
        /// The name of the URL.
        /// </value>
        public string UrlName { get; set; }

        public string DigitalSkillsLevel { get; set; }

        public IEnumerable<RestrictionItem> Restrictions { get; set; }

        public string OtherRequirements { get; set; }

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

        public IEnumerable<Classification> WorkingPattern { get; set; }

        public IEnumerable<Classification> WorkingPatternDetails { get; set; }

        public IEnumerable<Classification> WorkingHoursDetails { get; set; }

        public IEnumerable<Classification> HiddenAlternativeTitle { get; set; }

        public IEnumerable<Classification> JobProfileSpecialism { get; set; }

        public bool? IsImported { get; set; }

        public HowToBecome HowToBecomeData { get; set; }

        public WhatYouWillDoData WhatYouWillDoData { get; set; }

        public SocCodeItem SocCodeData { get; set; }

        //public IEnumerable<JobProfileRelatedCareer> RelatedCareersData { get; set; }
        public IEnumerable<JobProfileRelatedCareerItem> RelatedCareersData { get; set; }

        public IEnumerable<SocSkillMatrixItem> RelatedSkills { get; set; }

        public IEnumerable<JobProfileCategoryItem> JobProfileCategories { get; set; }

        public string ONetOccupationalCode { get; set; }

        public bool HasRelatedSocSkillMatrices { get; set; }

        public string WidgetContentTitle { get; set; }

        //Additional properties that needs to be exposed
        public string SocCodeId { get; set; }

        public string CType { get; set; }

        public DateTime LastModified { get; set; }

        public string CanonicalName { get; set; }

        public bool IncludeInSiteMap { get; set; }

        public string ActionType { get; set; }
    }
}