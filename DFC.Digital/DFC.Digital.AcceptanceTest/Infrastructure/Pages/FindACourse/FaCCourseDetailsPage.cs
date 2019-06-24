using OpenQA.Selenium;

namespace DFC.Digital.AcceptanceTest.Infrastructure.Pages
{
    public class FaCCourseDetailsPage : DFCPage
    {
        public string CourseDetailsTitle => Find.Element(By.ClassName("govuk-heading-l")).Text;

        public bool HasQualificationSection => Find.Element(By.Id("QualificationDetailsLabel")) != null;

        public bool HasCourseDescriptionSection => Find.Element(By.Id("CourseDescriptionLabel")) != null;

        public bool HasEntryRequirementSection => Find.Element(By.Id("EntryRequirementsLabel")) != null;

        public bool HasEquipmentRequiredSection => Find.Element(By.Id("EquipmentRequiredLabel")) != null;

        public bool HasAssessmentMethodSection => Find.Element(By.Id("AssessmentMethodLabel")) != null;

        public bool HasVenueSection => Find.Element(By.Id("VenueLabel")) != null;

        public bool HasOtherDatesAndVenuesSection => Find.Element(By.Id("OtherDatesAndVenuesLabel")) != null;
    }
}
