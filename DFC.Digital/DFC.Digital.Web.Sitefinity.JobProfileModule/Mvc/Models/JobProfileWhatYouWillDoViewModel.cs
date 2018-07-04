namespace DFC.Digital.Web.Sitefinity.JobProfileModule.Mvc.Models
{
    public class JobProfileWhatYouWillDoViewModel : JobProfileSectionViewModel
    {
        public bool IsWhatYouWillDoCadView { get; set; }

        public string DayToDayTasksSectionTitle { get; set; }

        public string EnvironmentTitle { get; set; }

        public string SectionId { get; set; }

        public string Location { get; set; }

        public string Environment { get; set; }

        public string Uniform { get; set; }

        public bool IsWYDIntroActive { get; set; }

        public string WYDIntroduction { get; set; }

        public string WYDDayToDayTasks { get; set; }
    }
}