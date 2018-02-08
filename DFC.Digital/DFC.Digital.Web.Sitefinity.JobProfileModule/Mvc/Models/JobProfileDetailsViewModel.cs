namespace DFC.Digital.Web.Sitefinity.JobProfileModule.Mvc.Models
{
    /// <summary>
    /// Job Profile Details ViewModel
    /// </summary>
    public class JobProfileDetailsViewModel : JobProfileSearchBoxViewModel
    {
        #region JobProfile Data

        public string Title { get; set; }

        public string AlternativeTitle { get; set; }

        public string Overview { get; set; }

        public string SalaryRange { get; set; }

        public string UrlName { get; set; }

        public string MinimumHours { get; set; }

        public string MaximumHours { get; set; }

        public bool? IsLMISalaryFeedOverriden { get; set; }

        public decimal? SalaryStarter { get; set; }

        public decimal? SalaryExperienced { get; set; }

        public string WorkingPatternText { get; set; }

        public string WorkingPatternSpanText { get; set; }

        public string WorkingPattern { get; set; }

        public string WorkingPatternDetails { get; set; }

        #endregion JobProfile Data

        #region supporting fields

        public string HoursText { get; set; }

        public string SalaryText { get; set; }

        public string SalaryTextSpan { get; set; }

        public string SalaryBlankText { get; set; }

        public string SalaryStarterText { get; set; }

        public string SalaryExperiencedText { get; set; }

        public string MaxAndMinHoursAreBlankText { get; set; }

        public string HoursTimePeriodText { get; set; }

        #endregion supporting fields
    }
}