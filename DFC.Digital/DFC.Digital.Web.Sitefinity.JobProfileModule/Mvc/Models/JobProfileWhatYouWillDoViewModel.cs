using System.Collections.Generic;

namespace DFC.Digital.Web.Sitefinity.JobProfileModule.Mvc.Models
{
    public class JobProfileWhatYouWillDoViewModel : JobProfileSectionViewModel
    {
        public bool IsWhatYouWillDoCadView { get; set; }

        public string DailyTasksSectionTitle { get; set; }

        public string EnvironmentTitle { get; set; }

        public string SectionId { get; set; }

        public string Location { get; set; }

        public string Environment { get; set; }

        public string Uniform { get; set; }

        public bool IsIntroActive { get; set; }

        public string Introduction { get; set; }

        public string DailyTasks { get; set; }
    }
}