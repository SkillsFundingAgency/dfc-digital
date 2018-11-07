namespace DFC.Digital.Data.Model
{
    public class ApprenticeshipVacancyReport : CmsReportItem
    {
        public string Name { get; set; }

        public SocCodeReport SocCode { get; set; }
    }
}
