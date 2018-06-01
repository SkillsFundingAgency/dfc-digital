using DFC.Digital.Data.Model;
using DFC.Digital.Repository.SitefinityCMS.Base;
using DFC.Digital.Repository.SitefinityCMS.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using Telerik.Sitefinity.DynamicModules.Model;
using Telerik.Sitefinity.Model;
using Telerik.Sitefinity.RelatedData;

namespace DFC.Digital.Repository.SitefinityCMS.Modules
{
    public class HowToBecomeConverter : IContentPropertyConverter<HowToBecome>
    {
        private const string WorkField = "Work";
        private const string VolunteeringField = "Volunteering";
        private const string DirectApplicationField = "DirectApplication";
        private const string OtherRoutesField = "OtherRoutes";
        private const string ProfessionalAndIndustryBodiesField = "ProfessionalAndIndustryBodies";
        private const string CareerTipsField = "CareerTips";
        private const string FurtherInformationField = "FurtherInformation";
        private const string IntroTextField = "EntryRoutes";
        private const string UniversityRelevantSubjectsField = "UniversityRelevantSubjects";
        private const string UniversityFurtherRouteInfoField = "UniversityFurtherRouteInfo";
        private const string UniversityRequirementsField = "UniversityEntryRequirements";
        private const string RelatedUniversityRequirementField = "RelatedUniversityRequirement";
        private const string RelatedUniversityLinksField = "RelatedUniversityLinks";
        private const string CollegeRelevantSubjectsField = "CollegeRelevantSubjects";
        private const string CollegeFurtherRouteInfoField = "CollegeFurtherRouteInfo";
        private const string CollegeRequirementsField = "CollegeEntryRequirements";
        private const string RelatedCollegeRequirementField = "RelatedCollegeRequirements";
        private const string RelatedCollegeLinksField = "RelatedCollegeLinks";
        private const string ApprenticeshipRelevantSubjectsField = "ApprenticeshipRelevantSubjects";
        private const string ApprenticeshipFurtherRouteInfoField = "ApprenticeshipFurtherRouteInfo";
        private const string ApprenticeshipRequirementsField = "ApprenticeshipEntryRequirements";
        private const string RelatedApprenticeshipRequirementField = "RelatedApprenticeshipRequirements";
        private const string RelatedApprenticeshipLinksField = "RelatedApprenticeshipLinks";
        private const string RelatedRegistrationsField = "RelatedRegistrations";
        private const string OtherRequirementsField = "OtherRequirements";
        private const string RelatedRestrictionsField = "RelatedRestrictions";

        private readonly IRelatedClassificationsRepository classificationsRepository;

        public HowToBecomeConverter(IRelatedClassificationsRepository classificationsRepository)
        {
            this.classificationsRepository = classificationsRepository;
        }

        public static IEnumerable<MoreInformationLink> GetRelatedLinkItems(DynamicContent content, string relatedField)
        {
            var linkItems = new List<MoreInformationLink>();
            var relatedItems = content.GetRelatedItems<DynamicContent>(relatedField);
            if (relatedItems != null)
            {
                foreach (var relatedItem in relatedItems)
                {
                    var linkItem = new MoreInformationLink
                    {
                        Title = relatedItem.GetValueOrDefault<Lstring>(nameof(MoreInformationLink.Title)),
                        Url = relatedItem.GetValueOrDefault<Lstring>(nameof(MoreInformationLink.Url))
                    };
                    linkItems.Add(linkItem);
                }
            }

            return linkItems;
        }

        public static IEnumerable<EntryRequirement> GetEntryRequirements(DynamicContent content, string relatedField)
        {
          var requirements = new List<EntryRequirement>();
            var relatedItems = content.GetRelatedItems<DynamicContent>(relatedField);
            if (relatedItems != null)
            {
                foreach (var relatedItem in relatedItems)
                {
                    var infoItem = new EntryRequirement
                    {
                        Title = relatedItem.GetValueOrDefault<Lstring>(nameof(InfoItem.Title)),
                        Info = relatedItem.GetValueOrDefault<Lstring>(nameof(InfoItem.Info))
                    };
                    requirements.Add(infoItem);
                }
            }

            return requirements;
        }

        public static IEnumerable<Restriction> GetRestrictions(DynamicContent content, string relatedField)
        {
            var restrictions = new List<Restriction>();
            var relatedItems = content.GetRelatedItems<DynamicContent>(relatedField);
            if (relatedItems != null)
            {
                foreach (var relatedItem in relatedItems)
                {
                    var infoItem = new Restriction
                    {
                        Title = relatedItem.GetValueOrDefault<Lstring>(nameof(InfoItem.Title)),
                        Info = relatedItem.GetValueOrDefault<Lstring>(nameof(InfoItem.Info))
                    };
                    restrictions.Add(infoItem);
                }
            }

            return restrictions;
        }

        public static IEnumerable<Registration> GetRegistrations(DynamicContent content, string relatedField)
        {
            var requirements = new List<Registration>();
            var relatedItems = content.GetRelatedItems<DynamicContent>(relatedField);
            if (relatedItems != null)
            {
                foreach (var relatedItem in relatedItems)
                {
                    var infoItem = new Registration
                    {
                        Title = relatedItem.GetValueOrDefault<Lstring>(nameof(InfoItem.Title)),
                        Info = relatedItem.GetValueOrDefault<Lstring>(nameof(InfoItem.Info))
                    };
                    requirements.Add(infoItem);
                }
            }

            return requirements;
        }

        public HowToBecome ConvertFrom(DynamicContent content)
        {
            return new HowToBecome
            {
                IntroText = content?.GetValueOrDefault<Lstring>(IntroTextField),
                ExtraInformation = new ExtraInformation
                {
                    // OTHER
                    Work = content?.GetValueOrDefault<Lstring>(WorkField),
                    Volunteering = content?.GetValueOrDefault<Lstring>(VolunteeringField),
                    DirectApplication = content?.GetValueOrDefault<Lstring>(DirectApplicationField),
                    OtherRoutes = content?.GetValueOrDefault<Lstring>(OtherRoutesField)
                },
                FurtherInformation = new MoreInformation
                {
                    ProfessionalAndIndustryBodies =
                        content?.GetValueOrDefault<Lstring>(ProfessionalAndIndustryBodiesField),
                    CareerTips = content?.GetValueOrDefault<Lstring>(CareerTipsField),
                    FurtherInformation = content?.GetValueOrDefault<Lstring>(FurtherInformationField)
                },
                RouteEntries = new List<RouteEntry>
                {
                    // UNIVERSITY
                    new RouteEntry
                    {
                        RouteName = RouteEntryType.University,
                        RouteSubjects = content?.GetValueOrDefault<Lstring>(UniversityRelevantSubjectsField),
                        FurtherRouteInformation = content?.GetValueOrDefault<Lstring>(UniversityFurtherRouteInfoField),
                        RouteRequirement = classificationsRepository.GetRelatedClassifications(content, UniversityRequirementsField, UniversityRequirementsField).FirstOrDefault(),
                        EntryRequirements = GetEntryRequirements(content, RelatedUniversityRequirementField),
                        MoreInformationLinks = GetRelatedLinkItems(content, RelatedUniversityLinksField)
                    },

                    // College
                    new RouteEntry
                    {
                        RouteName = RouteEntryType.College,
                        RouteSubjects = content?.GetValueOrDefault<Lstring>(CollegeRelevantSubjectsField),
                        FurtherRouteInformation = content?.GetValueOrDefault<Lstring>(CollegeFurtherRouteInfoField),
                        RouteRequirement = classificationsRepository.GetRelatedClassifications(content, CollegeRequirementsField, CollegeRequirementsField).FirstOrDefault(),
                        EntryRequirements = GetEntryRequirements(content, RelatedCollegeRequirementField),
                        MoreInformationLinks = GetRelatedLinkItems(content, RelatedCollegeLinksField)
                    },

                    // Apprenticeship
                    new RouteEntry
                    {
                        RouteName = RouteEntryType.Apprenticeship,
                        RouteSubjects = content?.GetValueOrDefault<Lstring>(ApprenticeshipRelevantSubjectsField),
                        FurtherRouteInformation = content?.GetValueOrDefault<Lstring>(ApprenticeshipFurtherRouteInfoField),
                        RouteRequirement = classificationsRepository.GetRelatedClassifications(content, ApprenticeshipRequirementsField, ApprenticeshipRequirementsField).FirstOrDefault(),
                        EntryRequirements = GetEntryRequirements(content, RelatedApprenticeshipRequirementField),
                        MoreInformationLinks = GetRelatedLinkItems(content, RelatedApprenticeshipLinksField)
                    }
                },
                Registrations = GetRegistrations(content, RelatedRegistrationsField),
                Restrictions = GetRestrictions(content, RelatedRestrictionsField),
                OtherRequirements = content?.GetValueOrDefault<Lstring>(OtherRequirementsField)
            };
        }
    }
}
