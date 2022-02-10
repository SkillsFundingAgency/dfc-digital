using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DFC.Digital.Data.Model.OrchardCore
{

    public class OcJobProfile
    {
        [JsonIgnore]
        public Guid SitefinityId { get; set; }
        public string ContentItemId { get; set; }
        [JsonIgnore]
        public string ContentItemVersionId { get; set; }
        public string ContentType { get; set; }
        public string DisplayText { get; set; }
        public bool Latest { get; set; }
        public bool Published { get; set; }
        public DateTime ModifiedUtc { get; set; }
        public DateTime PublishedUtc { get; set; }
        public DateTime CreatedUtc { get; set; }
        [JsonIgnore]
        public string Owner { get; set; }
        [JsonIgnore]
        public string Author { get; set; }
        public Titlepart TitlePart { get; set; }
        public Jobprofile JobProfile { get; set; }
        public Previewpart PreviewPart { get; set; }
        public Pagelocationpart PageLocationPart { get; set; }
        public Sitemappart SitemapPart { get; set; }
        public Contentapprovalpart ContentApprovalPart { get; set; }
        public Graphsyncpart GraphSyncPart { get; set; }
        public Audittrailpart AuditTrailPart { get; set; }
    }

    public class Jobprofile
    {
        public Alternativetitle AlternativeTitle { get; set; }
        public Widgetcontenttitle WidgetContentTitle { get; set; }
        public Overview Overview { get; set; }
        public Salarystarterperyear Salarystarterperyear { get; set; }
        public Salaryexperiencedperyear Salaryexperiencedperyear { get; set; }
        public Minimumhours Minimumhours { get; set; }
        public Maximumhours Maximumhours { get; set; }
        [JsonIgnore]
        public List<Guid> HiddenAlternativeTitleSf { get; set; }
        public HiddenalternativetitleIds HiddenAlternativeTitle { get; set; }
        [JsonIgnore]
        public List<Guid> WorkingHoursDetailsSf { get; set; }
        public Workinghoursdetails WorkingHoursDetails { get; set; }
        [JsonIgnore]
        public List<Guid> WorkingPatternSf { get; set; }
        public Workingpattern WorkingPattern { get; set; }
        [JsonIgnore]
        public List<Guid> WorkingPatternDetailsSf { get; set; }
        public Workingpatterndetails WorkingPatternDetails { get; set; }
        [JsonIgnore]
        public List<Guid> JobProfileSpecialismSf { get; set; }
        public JobprofilespecialismIds JobProfileSpecialism { get; set; }
        public Entryroutes Entryroutes { get; set; }
        public Universityrelevantsubjects Universityrelevantsubjects { get; set; }
        public Universityfurtherrouteinfo Universityfurtherrouteinfo { get; set; }
        [JsonIgnore]
        public List<Guid> UniversityEntryRequirementsSf { get; set; }
        public UniversityentryrequirementsIds UniversityEntryRequirements { get; set; }
        [JsonIgnore]
        public List<Guid> RelatedUniversityRequirementsSf { get; set; }
        public Relateduniversityrequirements RelatedUniversityRequirements { get; set; }
        public Collegerelevantsubjects Collegerelevantsubjects { get; set; }
        public Collegefurtherrouteinfo Collegefurtherrouteinfo { get; set; }
        [JsonIgnore]
        public List<Guid> RelatedUniversityLinksSf { get; set; }
        public Relateduniversitylinks RelatedUniversityLinks { get; set; }
        [JsonIgnore]
        public List<Guid> CollegeEntryRequirementsSf { get; set; }
        public CollegeentryrequirementsIds CollegeEntryRequirements { get; set; }
        [JsonIgnore]
        public List<Guid> RelatedCollegeRequirementsSf { get; set; }
        public Relatedcollegerequirements RelatedCollegeRequirements { get; set; }
        [JsonIgnore]
        public List<Guid> RelatedCollegeLinksSf { get; set; }
        public Relatedcollegelinks RelatedCollegeLinks { get; set; }
        public Apprenticeshiprelevantsubjects Apprenticeshiprelevantsubjects { get; set; }
        public Apprenticeshipfurtherroutesinfo Apprenticeshipfurtherroutesinfo { get; set; }
        [JsonIgnore]
        public List<Guid> ApprenticeshipEntryRequirementsSf { get; set; }
        public ApprenticeshipentryrequirementsIds ApprenticeshipEntryRequirements { get; set; }
        [JsonIgnore]
        public List<Guid> RelatedApprenticeshipRequirementsSf { get; set; }
        public Relatedapprenticeshiprequirements RelatedApprenticeshipRequirements { get; set; }
        [JsonIgnore]
        public List<Guid> RelatedApprenticeshipLinksSf { get; set; }
        public Relatedapprenticeshiplinks RelatedApprenticeshipLinks { get; set; }
        public Work Work { get; set; }
        public Volunteering Volunteering { get; set; }
        public Directapplication Directapplication { get; set; }
        public Otherroutes Otherroutes { get; set; }
        public Careertips Careertips { get; set; }
        public Furtherinformation Furtherinformation { get; set; }
        [JsonIgnore]
        public List<Guid> RelatedrestrictionsSf { get; set; }
        public Relatedrestrictions Relatedrestrictions { get; set; }
        public Otherrequirements Otherrequirements { get; set; }
        public Digitalskills DigitalSkills { get; set; }
        public Relatedskills Relatedskills { get; set; }
        [JsonIgnore]
        public List<Guid> JobProfileCategorySf { get; set; }
        public JobprofilecategoryIds JobProfileCategory { get; set; }
        public Daytodaytasks Daytodaytasks { get; set; }
        [JsonIgnore]
        public List<Guid> RelatedLocationsSf { get; set; }
        public Relatedlocations RelatedLocations { get; set; }
        [JsonIgnore]
        public List<Guid> RelatedEnvironmentsSf { get; set; }
        public Relatedenvironments RelatedEnvironments { get; set; }
        [JsonIgnore]
        public List<Guid> RelatedUniformsSf { get; set; }
        public Relateduniforms RelatedUniforms { get; set; }
        public Coursekeywords Coursekeywords { get; set; }
        public Careerpathandprogression Careerpathandprogression { get; set; }
        [JsonIgnore]
        public List<Guid> RelatedcareerprofilesSf { get; set; }
        public Relatedcareerprofiles Relatedcareerprofiles { get; set; }
        [JsonIgnore]
        public List<Guid> SOCCodeSf { get; set; }
        public SoccodeIds SOCCode { get; set; }
        [JsonIgnore]
        public List<Guid> RelatedRegistrationsSf { get; set; }
        public Relatedregistrations RelatedRegistrations { get; set; }
        public Professionalandindustrybodies Professionalandindustrybodies { get; set; }
        public Dynamictitleprefix DynamicTitlePrefix { get; set; }
    }

    public class Alternativetitle
    {
        public string Text { get; set; }
    }

    public class Widgetcontenttitle
    {
        public string Text { get; set; }
    }

    public class Overview
    {
        public string Text { get; set; }
    }

    public class Salarystarterperyear
    {
        public float Value { get; set; }
    }

    public class Salaryexperiencedperyear
    {
        public float Value { get; set; }
    }

    public class Minimumhours
    {
        public float Value { get; set; }
    }

    public class Maximumhours
    {
        public float Value { get; set; }
    }

    public class HiddenalternativetitleIds
    {
        public string[] ContentItemIds { get; set; }
    }

    public class Workinghoursdetails
    {
        public string[] ContentItemIds { get; set; }
    }

    public class Workingpattern
    {
        public string[] ContentItemIds { get; set; }
    }

    public class Workingpatterndetails
    {
        public string[] ContentItemIds { get; set; }
    }

    public class JobprofilespecialismIds
    {
        public string[] ContentItemIds { get; set; }
    }

    public class Entryroutes
    {
        public string Html { get; set; }
    }

    public class Universityrelevantsubjects
    {
        public string Html { get; set; }
    }

    public class Universityfurtherrouteinfo
    {
        public string Html { get; set; }
    }

    public class UniversityentryrequirementsIds
    {
        public string[] ContentItemIds { get; set; }
    }

    public class Relateduniversityrequirements
    {
        public string[] ContentItemIds { get; set; }
    }

    public class Collegerelevantsubjects
    {
        public string Html { get; set; }
    }

    public class Collegefurtherrouteinfo
    {
        public string Html { get; set; }
    }

    public class Relateduniversitylinks
    {
        public string[] ContentItemIds { get; set; }
    }

    public class CollegeentryrequirementsIds
    {
        public string[] ContentItemIds { get; set; }
    }

    public class Relatedcollegerequirements
    {
        public string[] ContentItemIds { get; set; }
    }

    public class Relatedcollegelinks
    {
        public string[] ContentItemIds { get; set; }
    }

    public class Apprenticeshiprelevantsubjects
    {
        public string Html { get; set; }
    }

    public class Apprenticeshipfurtherroutesinfo
    {
        public string Html { get; set; }
    }

    public class ApprenticeshipentryrequirementsIds
    {
        public string[] ContentItemIds { get; set; }
    }

    public class Relatedapprenticeshiprequirements
    {
        public string[] ContentItemIds { get; set; }
    }

    public class Relatedapprenticeshiplinks
    {
        public string[] ContentItemIds { get; set; }
    }

    public class Work
    {
        public string Html { get; set; }
    }

    public class Volunteering
    {
        public string Html { get; set; }
    }

    public class Directapplication
    {
        public string Html { get; set; }
    }

    public class Otherroutes
    {
        public string Html { get; set; }
    }

    public class Careertips
    {
        public string Html { get; set; }
    }

    public class Furtherinformation
    {
        public string Html { get; set; }
    }

    public class Relatedrestrictions
    {
        public object[] ContentItemIds { get; set; }
    }

    public class Otherrequirements
    {
        public string Html { get; set; }
    }

    public class Digitalskills
    {
        public object[] ContentItemIds { get; set; }
    }

    public class Relatedskills
    {
        public string[] ContentItemIds { get; set; }
    }

    public class JobprofilecategoryIds
    {
        public string[] ContentItemIds { get; set; }
    }

    public class Daytodaytasks
    {
        public string Html { get; set; }
    }

    public class Relatedlocations
    {
        public object[] ContentItemIds { get; set; }
    }

    public class Relatedenvironments
    {
        public object[] ContentItemIds { get; set; }
    }

    public class Relateduniforms
    {
        public object[] ContentItemIds { get; set; }
    }

    public class Coursekeywords
    {
        public string Text { get; set; }
    }

    public class Careerpathandprogression
    {
        public string Html { get; set; }
    }

    public class Relatedcareerprofiles
    {
        public object[] ContentItemIds { get; set; }
    }

    public class SoccodeIds
    {
        public string[] ContentItemIds { get; set; }
    }

    public class Relatedregistrations
    {
        public string[] ContentItemIds { get; set; }
    }

    public class Professionalandindustrybodies
    {
        public string Html { get; set; }
    }

    public class Dynamictitleprefix
    {
        public string[] ContentItemIds { get; set; }
    }

    public class Previewpart
    {
    }

    public class Pagelocationpart
    {
        public string UrlName { get; set; }
        public bool DefaultPageForLocation { get; set; }
        public object RedirectLocations { get; set; }
        public string FullUrl { get; set; }
    }

    public class Sitemappart
    {
        public bool OverrideSitemapConfig { get; set; }
        public int ChangeFrequency { get; set; }
        public int Priority { get; set; }
        public bool Exclude { get; set; }
    }

    public class Contentapprovalpart
    {
        public int ReviewStatus { get; set; }
        public int ReviewType { get; set; }
        public bool IsForcePublished { get; set; }
    }
}
