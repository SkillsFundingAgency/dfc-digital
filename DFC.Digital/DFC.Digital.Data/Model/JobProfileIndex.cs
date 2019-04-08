using DFC.Digital.Data.Attributes;
using DFC.Digital.Data.Interfaces;
using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace DFC.Digital.Data.Model
{
    public class JobProfileIndex : IDigitalDataModel
    {
        //private string _ignored;
        [Key]
        public string IdentityField { get; set; }

        [IsFilterable, IsSortable, IsFacetable]
        public string SocCode { get; set; }

        [IsSearchable, IsFilterable, IsSortable, IsSuggestible]
        [Analyzer(AnalyzerName.AsString.EnLucene)]
        [AddWeighting(7)]
        public string Title { get; set; }

        [IsSearchable]
        [Analyzer(AnalyzerName.AsString.Keyword)]
        [AddWeighting(100)]
        public string TitleAsKeyword
        {
            get
            {
                return Title.ToLower();
            }
        }

        [IsSearchable, IsFilterable, IsSuggestible]
        [Analyzer(AnalyzerName.AsString.EnLucene)]
        [AddWeighting(6)]
        public IEnumerable<string> AlternativeTitle { get; set; }

        [IsSearchable]
        [Analyzer(AnalyzerName.AsString.Keyword)]
        [AddWeighting(100)]
        public IEnumerable<string> AltTitleAsKeywords
        {
            get
            {
                return AlternativeTitle?.Select(a => a.ToLower());
            }
        }

        [IsSearchable]
        [AddWeighting(5)]
        [Analyzer(AnalyzerName.AsString.EnLucene)]
        public string Overview { get; set; }

        [IsFilterable, IsSortable, IsFacetable]
        public double SalaryStarter { get; set; }

        [IsFilterable, IsSortable, IsFacetable]
        public double SalaryExperienced { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1056:UriPropertiesShouldNotBeStrings", Justification = "This is an application field of type string, last segment and is not a complete uri")]
        [IsFilterable]
        public string UrlName { get; set; }

        [IsSearchable, IsFilterable]
        [AddWeighting(4)]
        [Analyzer(AnalyzerName.AsString.EnLucene)]
        public IEnumerable<string> JobProfileCategories { get; set; }

        [IsSearchable, IsFilterable]
        [Analyzer(AnalyzerName.AsString.EnLucene)]
        [AddWeighting(3)]
        public IEnumerable<string> JobProfileSpecialism { get; set; }

        [IsSearchable, IsFilterable]
        [Analyzer(AnalyzerName.AsString.EnLucene)]
        [AddWeighting(3)]
        public IEnumerable<string> HiddenAlternativeTitle { get; set; }

        public IEnumerable<string> JobProfileCategoriesWithUrl { get; set; }

        [IsFilterable]
        public IEnumerable<string> Interests { get; set; }

        [IsFilterable]
        public IEnumerable<string> Enablers { get; set; }

        [IsFilterable]
        public IEnumerable<string> EntryQualifications { get; set; }

        [IsFilterable]
        public IEnumerable<string> TrainingRoutes { get; set; }

        [IsFilterable]
        public IEnumerable<string> PreferredTaskTypes { get; set; }

        [IsFilterable]
        public IEnumerable<string> JobAreas { get; set; }

        [IsSearchable]
        [Analyzer(AnalyzerName.AsString.EnLucene)]
        public string CollegeRelevantSubjects { get; set; }

        [IsSearchable]
        [Analyzer(AnalyzerName.AsString.EnLucene)]
        public string UniversityRelevantSubjects { get; set; }

        [IsSearchable]
        [Analyzer(AnalyzerName.AsString.EnLucene)]
        public string ApprenticeshipRelevantSubjects { get; set; }

        [IsSearchable]
        [Analyzer(AnalyzerName.AsString.EnLucene)]
        public string WYDDayToDayTasks { get; set; }

        [IsSearchable]
        [Analyzer(AnalyzerName.AsString.EnLucene)]
        public string CareerPathAndProgression { get; set; }

        [IsSearchable]
        [Analyzer(AnalyzerName.AsString.EnLucene)]
        public IEnumerable<string> WorkingPattern { get; set; }

        [IsSearchable]
        [Analyzer(AnalyzerName.AsString.EnLucene)]
        public IEnumerable<string> WorkingPatternDetails { get; set; }

        [IsSearchable]
        [Analyzer(AnalyzerName.AsString.EnLucene)]
        public IEnumerable<string> WorkingHoursDetails { get; set; }

        [IsFilterable, IsSortable, IsFacetable]
        public double MinimumHours { get; set; }

        [IsFilterable, IsSortable, IsFacetable]
        public double MaximumHours { get; set; }
    }
}