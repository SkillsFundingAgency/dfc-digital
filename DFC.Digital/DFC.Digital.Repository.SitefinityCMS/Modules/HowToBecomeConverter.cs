using DFC.Digital.Data.Model;
using DFC.Digital.Repository.SitefinityCMS;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Telerik.Sitefinity.Data.ContentLinks;
using Telerik.Sitefinity.DynamicModules.Model;
using Telerik.Sitefinity.GenericContent.Model;
using Telerik.Sitefinity.Model;
using Telerik.Sitefinity.Model.ContentLinks;

namespace DFC.Digital.Repository.SitefinityCMS.Modules
{
    public class HowToBecomeConverter : IContentPropertyConverter<HowToBecome>
    {
        #region Fields

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

        private readonly IRelatedClassificationsRepository classificationsRepository;
        private readonly IDynamicContentExtensions dynamicContentExtensions;
        private readonly IDynamicModuleRepository<MoreInformationLink> moreInfoRepository;
        private readonly IDynamicModuleRepository<JobProfile> repository;

        #endregion Fields

        #region Ctor

        public HowToBecomeConverter(IRelatedClassificationsRepository classificationsRepository, IDynamicContentExtensions dynamicContentExtensions, IDynamicModuleRepository<MoreInformationLink> moreInfoRepository, IDynamicModuleRepository<JobProfile> repository)
        {
            this.classificationsRepository = classificationsRepository;
            this.dynamicContentExtensions = dynamicContentExtensions;
            this.moreInfoRepository = moreInfoRepository;
            this.repository = repository;
        }

        #endregion Ctor

        #region Public methods

        public HowToBecome ConvertFrom(DynamicContent content)
        {
            var isCadReady = dynamicContentExtensions.GetFieldValue<bool>(content, nameof(HowToBecome.IsHTBCaDReady));
            return !isCadReady ? new HowToBecome() : new HowToBecome
            {
                IsHTBCaDReady = true,
                IntroText = dynamicContentExtensions.GetFieldValue<Lstring>(content, IntroTextField),
                FurtherRoutes = new FurtherRoutes
                {
                    // OTHER
                    Work = dynamicContentExtensions.GetFieldValue<Lstring>(content, WorkField),
                    Volunteering = dynamicContentExtensions.GetFieldValue<Lstring>(content, VolunteeringField),
                    DirectApplication = dynamicContentExtensions.GetFieldValue<Lstring>(content, DirectApplicationField),
                    OtherRoutes = dynamicContentExtensions.GetFieldValue<Lstring>(content, OtherRoutesField)
                },
                FurtherInformation = new MoreInformation
                {
                    ProfessionalAndIndustryBodies =
                            dynamicContentExtensions.GetFieldValue<Lstring>(content, ProfessionalAndIndustryBodiesField),
                    CareerTips = dynamicContentExtensions.GetFieldValue<Lstring>(content, CareerTipsField),
                    FurtherInformation = dynamicContentExtensions.GetFieldValue<Lstring>(content, FurtherInformationField)
                },
                RouteEntries = new List<RouteEntry>
                {
                    // UNIVERSITY
                    new RouteEntry
                    {
                        RouteName = RouteEntryType.University,
                        RouteSubjects = dynamicContentExtensions.GetFieldValue<Lstring>(content, UniversityRelevantSubjectsField),
                        FurtherRouteInformation = dynamicContentExtensions.GetFieldValue<Lstring>(content, UniversityFurtherRouteInfoField),
                        RouteRequirement = classificationsRepository.GetRelatedClassifications(content, UniversityRequirementsField, UniversityRequirementsField).FirstOrDefault(),
                        EntryRequirements = GetEntryRequirements(content, RelatedUniversityRequirementField),
                        MoreInformationCmLinks = GetRelatedLinksByCm(content, RelatedUniversityLinksField),
                        MoreInformationDmLinks = GetRelatedLinkItemsByDm(content, RelatedUniversityLinksField),
                        MoreInformationLinks = new List<MoreInformationLink>()
                    },

                    // College
                    new RouteEntry
                    {
                        RouteName = RouteEntryType.College,
                        RouteSubjects = dynamicContentExtensions.GetFieldValue<Lstring>(content, CollegeRelevantSubjectsField),
                        FurtherRouteInformation = dynamicContentExtensions.GetFieldValue<Lstring>(content, CollegeFurtherRouteInfoField),
                        RouteRequirement = classificationsRepository.GetRelatedClassifications(content, CollegeRequirementsField, CollegeRequirementsField).FirstOrDefault(),
                        EntryRequirements = GetEntryRequirements(content, RelatedCollegeRequirementField),
                        MoreInformationLinks = GetRelatedLinkItems(content, RelatedCollegeLinksField)
                    },

                    // Apprenticeship
                    new RouteEntry
                    {
                        RouteName = RouteEntryType.Apprenticeship,
                        RouteSubjects = dynamicContentExtensions.GetFieldValue<Lstring>(content, ApprenticeshipRelevantSubjectsField),
                        FurtherRouteInformation = dynamicContentExtensions.GetFieldValue<Lstring>(content, ApprenticeshipFurtherRouteInfoField),
                        RouteRequirement = classificationsRepository.GetRelatedClassifications(content, ApprenticeshipRequirementsField, ApprenticeshipRequirementsField).FirstOrDefault(),
                        EntryRequirements = GetEntryRequirements(content, RelatedApprenticeshipRequirementField),
                        MoreInformationLinks = GetRelatedLinkItems(content, RelatedApprenticeshipLinksField)
                    }
                },
                Registrations = GetRegistrations(content, RelatedRegistrationsField),
            };
        }

        #endregion Public methods

        #region private methods

        private IEnumerable<MoreInformationLink> GetRelatedLinkItems(DynamicContent content, string relatedField)
        {
            var linkItems = new List<MoreInformationLink>();
            var relatedItems = dynamicContentExtensions.GetRelatedItems(content, relatedField);
            if (relatedItems != null)
            {
                foreach (var relatedItem in relatedItems)
                {
                    var link = dynamicContentExtensions.GetFieldValue<Lstring>(relatedItem, nameof(MoreInformationLink.Url));
                    linkItems.Add(new MoreInformationLink
                    {
                        Title = dynamicContentExtensions.GetFieldValue<Lstring>(relatedItem, nameof(MoreInformationLink.Title)),
                        Url = !string.IsNullOrWhiteSpace(link) ? new Uri(link, UriKind.RelativeOrAbsolute) : default,
                        Text = dynamicContentExtensions.GetFieldValue<Lstring>(relatedItem, nameof(MoreInformationLink.Text)),
                    });
                }
            }

            return linkItems;
        }

        private TimedLinks GetRelatedLinkItemsByDm(DynamicContent content, string relatedField)
        {
            var watch = Stopwatch.StartNew();
            var linkItems = new List<MoreInformationLink>();
            var relatedItems = dynamicContentExtensions.GetRelatedItems(content, relatedField);
            if (relatedItems != null)
            {
                foreach (var relatedItem in relatedItems)
                {
                    var link = dynamicContentExtensions.GetFieldValue<Lstring>(relatedItem, nameof(MoreInformationLink.Url));
                    linkItems.Add(new MoreInformationLink
                    {
                        Title = dynamicContentExtensions.GetFieldValue<Lstring>(relatedItem, nameof(MoreInformationLink.Title)),
                        Url = !string.IsNullOrWhiteSpace(link) ? new Uri(link, UriKind.RelativeOrAbsolute) : default,
                        Text = dynamicContentExtensions.GetFieldValue<Lstring>(relatedItem, nameof(MoreInformationLink.Text)),
                    });
                }
            }

            return new TimedLinks
            {
                MoreInformationLinks = linkItems,
                TimeToExecute = $"Took {watch.Elapsed}"
            };
        }

        private TimedLinks GetRelatedLinksByCm(DynamicContent content, string relatedField)
        {
            var watch = Stopwatch.StartNew();
            var contentLinkManager = new ContentLinksManager();
            var masterContentItem = repository.GetMaster(content);

            var relatedLinksByMaster = contentLinkManager.GetContentLinks()
                .Where(cl => cl.ParentItemId == masterContentItem.Id && cl.ComponentPropertyName == relatedField).AsQueryable();

            var guidList = relatedLinksByMaster.Where(rl => content.Status == ContentLifecycleStatus.Live ? rl.AvailableForLive : rl.AvailableForTemp).Select(rl => rl.ChildItemId).ToList();

            var relatedItems = moreInfoRepository.GetMany(item => guidList.Contains(item.Id));

            var linkItems = new List<MoreInformationLink>();
            foreach (var relatedItem in relatedItems)
            {
                var linkurl =
                    dynamicContentExtensions.GetFieldValue<Lstring>(relatedItem, nameof(MoreInformationLink.Url));

                linkItems.Add(new MoreInformationLink
                {
                    Title = dynamicContentExtensions.GetFieldValue<Lstring>(relatedItem, nameof(MoreInformationLink.Title)),
                    Url = !string.IsNullOrWhiteSpace(linkurl) ? new Uri(linkurl, UriKind.RelativeOrAbsolute) : default,
                    Text = dynamicContentExtensions.GetFieldValue<Lstring>(relatedItem, nameof(MoreInformationLink.Text)),
                });
            }

            return new TimedLinks
            {
                MoreInformationLinks = linkItems,
                TimeToExecute = $"Took {watch.Elapsed}"
            };
        }

        private IEnumerable<EntryRequirement> GetEntryRequirements(DynamicContent content, string relatedField)
        {
            var requirements = new List<EntryRequirement>();
            var relatedItems = dynamicContentExtensions.GetRelatedItems(content, relatedField);
            if (relatedItems != null)
            {
                foreach (var relatedItem in relatedItems)
                {
                    requirements.Add(new EntryRequirement
                    {
                        Title = dynamicContentExtensions.GetFieldValue<Lstring>(relatedItem, nameof(InfoItem.Title)),
                        Info = dynamicContentExtensions.GetFieldValue<Lstring>(relatedItem, nameof(InfoItem.Info))
                    });
                }
            }

            return requirements;
        }

        private IEnumerable<Registration> GetRegistrations(DynamicContent content, string relatedField)
        {
            var requirements = new List<Registration>();
            var relatedItems = dynamicContentExtensions.GetRelatedItems(content, relatedField);
            if (relatedItems != null)
            {
                foreach (var relatedItem in relatedItems)
                {
                    requirements.Add(new Registration
                    {
                        Title = dynamicContentExtensions.GetFieldValue<Lstring>(relatedItem, nameof(InfoItem.Title)),
                        Info = dynamicContentExtensions.GetFieldValue<Lstring>(relatedItem, nameof(InfoItem.Info))
                    });
                }
            }

            return requirements;
        }

        #endregion private methods
    }
}