using DFC.Digital.Core;
using DFC.Digital.Core.Interceptors;
using DFC.Digital.Data;
using DFC.Digital.Data.Model;
using DFC.Digital.Data.Model.OrchardCore;
using DFC.Digital.Data.Model.OrchardCore.Uniform;
using DFC.Digital.Repository.SitefinityCMS.Modules;
using DFC.Digital.Repository.SitefinityCMS.OrchardCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Telerik.Sitefinity;
using Telerik.Sitefinity.Data;
using Telerik.Sitefinity.Data.Linq.Dynamic;
using Telerik.Sitefinity.DynamicModules;
using Telerik.Sitefinity.DynamicModules.Model;
using Telerik.Sitefinity.GenericContent.Model;
using Telerik.Sitefinity.Model;
using Telerik.Sitefinity.Security;
using Telerik.Sitefinity.Utilities.TypeConverters;
using Telerik.Sitefinity.Versioning;

namespace DFC.Digital.Repository.SitefinityCMS
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public sealed class DynamicModuleRepository<T> : IDynamicModuleRepository<T>, IDisposable
    {
        private const string IncludeInSitemapFieldName = "IncludeInSitemap";
        private const string OwnerFieldName = "Owner";
        private const string PublicationDateFieldName = "PublicationDate";
        private const string DraftApprovalWorkflowState = "Draft";

        private static readonly OrchardCoreIdGenerator OrchardCoreIdGenerator = new OrchardCoreIdGenerator();
        private static readonly MappingRepository MappingToolRepository = new MappingRepository();
        private readonly IApplicationLogger applicationLogger;
        private readonly IDynamicContentExtensions dynamicContentExtensions;

        private DynamicModuleManager dynamicModuleManager;

        private Type dynamicModuleContentType;

        private string providerName;

        public DynamicModuleRepository(IApplicationLogger applicationLogger, IDynamicContentExtensions dynamicContentExtensions)
        {
            this.applicationLogger = applicationLogger;
            this.dynamicContentExtensions = dynamicContentExtensions;
        }

        #region NotImplemented

        public void Delete(DynamicContent entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(Expression<Func<DynamicContent, bool>> where)
        {
            throw new NotImplementedException();
        }

        #endregion NotImplemented

        #region IRepository implementations

        #region Temp hide
        [IgnoreInputInInterception]
        public void Add(DynamicContent entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            Add(entity, null);
        }

        [IgnoreInputInInterception]
        public void Update(DynamicContent entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            Update(entity, null);
        }

        public void Initialise(string contentType, string dynamicModuleName)
        {
            //GSR had to add this as we were getting not results returned on instances where the jobprofiles
            //modue had been added as an addon - think it adds it with a diffrent provider.
            providerName = DynamicModuleManager.GetDefaultProviderName(dynamicModuleName);
            dynamicModuleManager = DynamicModuleManager.GetManager(providerName);
            dynamicModuleContentType = TypeResolutionService.ResolveType(contentType);
        }

        public Type GetContentType()
        {
            return dynamicModuleContentType;
        }

        public string GetProviderName()
        {
            return providerName;
        }

        [IgnoreOutputInInterception]
        public DynamicContent Create()
        {
            return dynamicModuleManager.CreateDataItem(dynamicModuleContentType);
        }

        [IgnoreInputInInterception]
        public void Add(DynamicContent entity, string changeComment)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            entity.SetValue(IncludeInSitemapFieldName, false);
            entity.SetValue(OwnerFieldName, SecurityManager.GetCurrentUserId());
            entity.SetValue(PublicationDateFieldName, DateTime.UtcNow);
            Publish(entity, changeComment);
        }

        [IgnoreInputInInterception]
        public void Update(DynamicContent entity, string changeComment)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            // Set a transaction name and get the version manager
            var transactionName = $"{entity.GetType().Name}-{DateTime.Now.Ticks}";

            applicationLogger.Info($"Updating entity with transaction name {transactionName} for {entity.UrlName}");
            var versionManager = VersionManager.GetManager(null, transactionName);
            CreateVersion(entity, changeComment, versionManager, WorkflowStatus.Draft);

            // Commit the transaction in order for the items to be actually persisted to data store
            TransactionManager.CommitTransaction(transactionName);
        }

        [IgnoreInputInInterception]
        public void Publish(DynamicContent entity, string changeComment)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            //CodeReview: Consider audit / log transaction name as well, might be an useful instrument for prd troubleshooting.
            var transactionName = $"{entity.GetType().Name}-{DateTime.Now.Ticks}";

            applicationLogger.Info($"Publishing entity with transaction name {transactionName} for {entity.UrlName}");

            var versionManager = VersionManager.GetManager(null, transactionName);

            // You need to set appropriate workflow status
            // Now the item is published and can be seen in the page
            CreateVersion(entity, changeComment, versionManager, WorkflowStatus.Published);

            // We can now call the following to publish the item
            dynamicModuleManager.Lifecycle.Publish(entity);

            // Commit the transaction in order for the items to be actually persisted to data store
            TransactionManager.CommitTransaction(transactionName);
        }

        [IgnoreOutputInInterception]
        public DynamicContent Checkout(string urlName)
        {
            var master = Get(item => item.UrlName == urlName && item.Status == ContentLifecycleStatus.Master);
            return dynamicModuleManager.Lifecycle.CheckOut(master) as DynamicContent;
        }

        [IgnoreInputInInterception]
        [IgnoreOutputInInterception]
        public DynamicContent Get(Expression<Func<DynamicContent, bool>> where)
        {
            return GetAll().FirstOrDefault(where);
        }

        [IgnoreOutputInInterception]
        public IQueryable<DynamicContent> GetAll()
        {
            return dynamicModuleManager.GetDataItems(dynamicModuleContentType);
        }

        #endregion Temp hide

        #region Registration

        public IEnumerable<OcRegistration> GetAllRegistrations()
        {
            dynamicModuleContentType = TypeResolutionService.ResolveType(DynamicTypes.RegistrationContentType);
            var dynamicModuleItems = dynamicModuleManager.GetDataItems(dynamicModuleContentType).Where(item => item.Status == ContentLifecycleStatus.Live && item.Visible);

            //var something = JsonConvert.SerializeObject(dynamicModuleItems);
            if (dynamicModuleItems.Any())
            {
                var registrations = new List<OcRegistration>();
                foreach (var dynamicModuleItem in dynamicModuleItems)
                {
                    registrations.Add(ConvertFromToRegistration(dynamicModuleItem));
                }

                return registrations;
            }

            return Enumerable.Empty<OcRegistration>();
        }

        public OcRegistration ConvertFromToRegistration(DynamicContent content)
        {
            var registration = new OcRegistration
            {
                SitefinityId = dynamicContentExtensions.GetFieldValue<Guid>(content, SitefinityFields.Id),
                ContentItemId = OrchardCoreIdGenerator.GenerateUniqueId(),
                ContentType = ItemTypes.Registration,
                DisplayText = dynamicContentExtensions.GetFieldValue<Lstring>(content, SitefinityFields.Title),
                Latest = true,
                Published = true,
                ModifiedUtc = dynamicContentExtensions.GetFieldValue<DateTime>(content, SitefinityFields.LastModified),
                PublishedUtc = dynamicContentExtensions.GetFieldValue<DateTime>(content, SitefinityFields.PublicationDate),
                CreatedUtc = dynamicContentExtensions.GetFieldValue<DateTime>(content, SitefinityFields.DateCreated),
                UniqueTitlePart = new Uniquetitlepart() { Title = dynamicContentExtensions.GetFieldValue<Lstring>(content, SitefinityFields.Title) },
                TitlePart = new Titlepart() { Title = dynamicContentExtensions.GetFieldValue<Lstring>(content, SitefinityFields.Title) },
                Registration = new IntRegistration() { Info = new OcInfoHtml() { Html = dynamicContentExtensions.GetFieldValue<Lstring>(content, SitefinityFields.Info) } },
                GraphSyncPart = new Graphsyncpart() { Text = $"<<contentapiprefix>>/registration/{dynamicContentExtensions.GetFieldValue<Guid>(content, SitefinityFields.OriginalContentId)}" },
                AuditTrailPart = new Audittrailpart() { Comment = null, ShowComment = false }
            };

            return registration;
        }

        #endregion Registration

        #region Restriction

        public IEnumerable<OcRestriction> GetAllRestrictions()
        {
            dynamicModuleContentType = TypeResolutionService.ResolveType(DynamicTypes.RestrictionContentType);
            var dynamicModuleItems = dynamicModuleManager.GetDataItems(dynamicModuleContentType).Where(item => item.Status == ContentLifecycleStatus.Live && item.Visible);

            if (dynamicModuleItems.Any())
            {
                var restrictions = new List<OcRestriction>();
                foreach (var dynamicModuleItem in dynamicModuleItems)
                {
                    restrictions.Add(ConvertFromToRestriction(dynamicModuleItem));
                }

                return restrictions;
            }

            return Enumerable.Empty<OcRestriction>();
        }

        public OcRestriction ConvertFromToRestriction(DynamicContent content)
        {
            var restriction = new OcRestriction
            {
                SitefinityId = dynamicContentExtensions.GetFieldValue<Guid>(content, SitefinityFields.Id),
                ContentItemId = OrchardCoreIdGenerator.GenerateUniqueId(),
                ContentType = ItemTypes.Restriction,
                DisplayText = dynamicContentExtensions.GetFieldValue<Lstring>(content, SitefinityFields.Title),
                Latest = true,
                Published = true,
                ModifiedUtc = dynamicContentExtensions.GetFieldValue<DateTime>(content, SitefinityFields.LastModified),
                PublishedUtc = dynamicContentExtensions.GetFieldValue<DateTime>(content, SitefinityFields.PublicationDate),
                CreatedUtc = dynamicContentExtensions.GetFieldValue<DateTime>(content, SitefinityFields.DateCreated),
                UniqueTitlePart = new Uniquetitlepart() { Title = dynamicContentExtensions.GetFieldValue<Lstring>(content, SitefinityFields.Title) },
                TitlePart = new Titlepart() { Title = dynamicContentExtensions.GetFieldValue<Lstring>(content, SitefinityFields.Title) },
                Restriction = new IntRestriction() { Info = new OcInfoHtml() { Html = dynamicContentExtensions.GetFieldValue<Lstring>(content, SitefinityFields.Info) } },
                GraphSyncPart = new Graphsyncpart() { Text = $"<<contentapiprefix>>/restriction/{dynamicContentExtensions.GetFieldValue<Guid>(content, SitefinityFields.OriginalContentId)}" },
                AuditTrailPart = new Audittrailpart() { Comment = null, ShowComment = false }
            };

            return restriction;
        }

        #endregion Restriction

        #region ApprenticeshipLink

        public IEnumerable<OcApprenticeshipLink> GetAllApprenticeshipLinks()
        {
            dynamicModuleContentType = TypeResolutionService.ResolveType(DynamicTypes.ApprenticeshipLinkContentType);
            var dynamicModuleItems = dynamicModuleManager.GetDataItems(dynamicModuleContentType).Where(item => item.Status == ContentLifecycleStatus.Live && item.Visible);

            if (dynamicModuleItems.Any())
            {
                var apprenticeshipLinks = new List<OcApprenticeshipLink>();
                foreach (var dynamicModuleItem in dynamicModuleItems)
                {
                    apprenticeshipLinks.Add(ConvertFromToApprenticeshipLink(dynamicModuleItem));
                }

                return apprenticeshipLinks;
            }

            return Enumerable.Empty<OcApprenticeshipLink>();
        }

        public OcApprenticeshipLink ConvertFromToApprenticeshipLink(DynamicContent content)
        {
            var apprenticeshipLink = new OcApprenticeshipLink
            {
                SitefinityId = dynamicContentExtensions.GetFieldValue<Guid>(content, SitefinityFields.Id),
                ContentItemId = OrchardCoreIdGenerator.GenerateUniqueId(),
                ContentType = ItemTypes.ApprenticeshipLink,
                DisplayText = dynamicContentExtensions.GetFieldValue<Lstring>(content, SitefinityFields.Title),
                Latest = true,
                Published = true,
                ModifiedUtc = dynamicContentExtensions.GetFieldValue<DateTime>(content, SitefinityFields.LastModified),
                PublishedUtc = dynamicContentExtensions.GetFieldValue<DateTime>(content, SitefinityFields.PublicationDate),
                CreatedUtc = dynamicContentExtensions.GetFieldValue<DateTime>(content, SitefinityFields.DateCreated),
                UniqueTitlePart = new Uniquetitlepart() { Title = dynamicContentExtensions.GetFieldValue<Lstring>(content, SitefinityFields.Title) },
                TitlePart = new Titlepart() { Title = dynamicContentExtensions.GetFieldValue<Lstring>(content, SitefinityFields.Title) },
                ApprenticeshipLink = new Apprenticeshiplink() { Text = new OcText { Text = dynamicContentExtensions.GetFieldValue<Lstring>(content, SitefinityFields.Text) }, URL = new OcURL { Text = dynamicContentExtensions.GetFieldValue<Lstring>(content, SitefinityFields.Url) } },
                GraphSyncPart = new Graphsyncpart() { Text = $"<<contentapiprefix>>/apprenticeshiplink/{dynamicContentExtensions.GetFieldValue<Guid>(content, SitefinityFields.OriginalContentId)}" },
                AuditTrailPart = new Audittrailpart() { Comment = null, ShowComment = false }
            };

            return apprenticeshipLink;
        }

        #endregion ApprenticeshipLink

        #region CollegeLink

        public IEnumerable<OcCollegeLink> GetAllCollegeLinks()
        {
            dynamicModuleContentType = TypeResolutionService.ResolveType(DynamicTypes.CollegeLinkContentType);
            var dynamicModuleItems = dynamicModuleManager.GetDataItems(dynamicModuleContentType).Where(item => item.Status == ContentLifecycleStatus.Live && item.Visible);

            if (dynamicModuleItems.Any())
            {
                var collegeLinks = new List<OcCollegeLink>();
                foreach (var dynamicModuleItem in dynamicModuleItems)
                {
                    collegeLinks.Add(ConvertFromToCollegeLink(dynamicModuleItem));
                }

                return collegeLinks;
            }

            return Enumerable.Empty<OcCollegeLink>();
        }

        public OcCollegeLink ConvertFromToCollegeLink(DynamicContent content)
        {
            var collegeLink = new OcCollegeLink
            {
                SitefinityId = dynamicContentExtensions.GetFieldValue<Guid>(content, SitefinityFields.Id),
                ContentItemId = OrchardCoreIdGenerator.GenerateUniqueId(),
                ContentType = ItemTypes.CollegeLink,
                DisplayText = dynamicContentExtensions.GetFieldValue<Lstring>(content, SitefinityFields.Title),
                Latest = true,
                Published = true,
                ModifiedUtc = dynamicContentExtensions.GetFieldValue<DateTime>(content, SitefinityFields.LastModified),
                PublishedUtc = dynamicContentExtensions.GetFieldValue<DateTime>(content, SitefinityFields.PublicationDate),
                CreatedUtc = dynamicContentExtensions.GetFieldValue<DateTime>(content, SitefinityFields.DateCreated),
                UniqueTitlePart = new Uniquetitlepart() { Title = dynamicContentExtensions.GetFieldValue<Lstring>(content, SitefinityFields.Title) },
                TitlePart = new Titlepart() { Title = dynamicContentExtensions.GetFieldValue<Lstring>(content, SitefinityFields.Title) },
                CollegeLink = new Collegelink() { Text = new OcText { Text = dynamicContentExtensions.GetFieldValue<Lstring>(content, SitefinityFields.Text) }, URL = new OcURL { Text = dynamicContentExtensions.GetFieldValue<Lstring>(content, SitefinityFields.Url) } },
                GraphSyncPart = new Graphsyncpart() { Text = $"<<contentapiprefix>>/collegelink/{dynamicContentExtensions.GetFieldValue<Guid>(content, SitefinityFields.OriginalContentId)}" },
                AuditTrailPart = new Audittrailpart() { Comment = null, ShowComment = false }
            };

            return collegeLink;
        }

        #endregion CollegeLink

        #region UniversityLink

        public IEnumerable<OcUniversityLink> GetAllUniversityLinks()
        {
            dynamicModuleContentType = TypeResolutionService.ResolveType(DynamicTypes.UniversityLinkContentType);
            var dynamicModuleItems = dynamicModuleManager.GetDataItems(dynamicModuleContentType).Where(item => item.Status == ContentLifecycleStatus.Live && item.Visible);

            if (dynamicModuleItems.Any())
            {
                var universityLinks = new List<OcUniversityLink>();
                foreach (var dynamicModuleItem in dynamicModuleItems)
                {
                    universityLinks.Add(ConvertFromToUniversityLink(dynamicModuleItem));
                }

                return universityLinks;
            }

            return Enumerable.Empty<OcUniversityLink>();
        }

        public OcUniversityLink ConvertFromToUniversityLink(DynamicContent content)
        {
            var universityLink = new OcUniversityLink
            {
                SitefinityId = dynamicContentExtensions.GetFieldValue<Guid>(content, SitefinityFields.Id),
                ContentItemId = OrchardCoreIdGenerator.GenerateUniqueId(),
                ContentType = ItemTypes.UniversityLink,
                DisplayText = dynamicContentExtensions.GetFieldValue<Lstring>(content, SitefinityFields.Title),
                Latest = true,
                Published = true,
                ModifiedUtc = dynamicContentExtensions.GetFieldValue<DateTime>(content, SitefinityFields.LastModified),
                PublishedUtc = dynamicContentExtensions.GetFieldValue<DateTime>(content, SitefinityFields.PublicationDate),
                CreatedUtc = dynamicContentExtensions.GetFieldValue<DateTime>(content, SitefinityFields.DateCreated),
                UniqueTitlePart = new Uniquetitlepart() { Title = dynamicContentExtensions.GetFieldValue<Lstring>(content, SitefinityFields.Title) },
                TitlePart = new Titlepart() { Title = dynamicContentExtensions.GetFieldValue<Lstring>(content, SitefinityFields.Title) },
                UniversityLink = new Universitylink() { Text = new OcText { Text = dynamicContentExtensions.GetFieldValue<Lstring>(content, SitefinityFields.Text) }, URL = new OcURL { Text = dynamicContentExtensions.GetFieldValue<Lstring>(content, SitefinityFields.Url) } },
                GraphSyncPart = new Graphsyncpart() { Text = $"<<contentapiprefix>>/universitylink/{dynamicContentExtensions.GetFieldValue<Guid>(content, SitefinityFields.OriginalContentId)}" },
                AuditTrailPart = new Audittrailpart() { Comment = null, ShowComment = false }
            };

            return universityLink;
        }

        #endregion UniversityLink

        #region ApprenticeshipRequirement

        public IEnumerable<OcApprenticeshipRequirement> GetAllApprenticeshipRequirements()
        {
            dynamicModuleContentType = TypeResolutionService.ResolveType(DynamicTypes.ApprenticeshipRequirementContentType);
            var dynamicModuleItems = dynamicModuleManager.GetDataItems(dynamicModuleContentType).Where(item => item.Status == ContentLifecycleStatus.Live && item.Visible);

            if (dynamicModuleItems.Any())
            {
                var apprenticeshipRequirements = new List<OcApprenticeshipRequirement>();
                foreach (var dynamicModuleItem in dynamicModuleItems)
                {
                    apprenticeshipRequirements.Add(ConvertFromToApprenticeshipRequirement(dynamicModuleItem));
                }

                return apprenticeshipRequirements;
            }

            return Enumerable.Empty<OcApprenticeshipRequirement>();
        }

        public OcApprenticeshipRequirement ConvertFromToApprenticeshipRequirement(DynamicContent content)
        {
            var apprenticeshipRequirement = new OcApprenticeshipRequirement
            {
                SitefinityId = dynamicContentExtensions.GetFieldValue<Guid>(content, SitefinityFields.Id),
                ContentItemId = OrchardCoreIdGenerator.GenerateUniqueId(),
                ContentType = OcItemTypes.ApprenticeshipRequirements,
                DisplayText = dynamicContentExtensions.GetFieldValue<Lstring>(content, SitefinityFields.Title),
                Latest = true,
                Published = true,
                ModifiedUtc = dynamicContentExtensions.GetFieldValue<DateTime>(content, SitefinityFields.LastModified),
                PublishedUtc = dynamicContentExtensions.GetFieldValue<DateTime>(content, SitefinityFields.PublicationDate),
                CreatedUtc = dynamicContentExtensions.GetFieldValue<DateTime>(content, SitefinityFields.DateCreated),
                UniqueTitlePart = new Uniquetitlepart() { Title = dynamicContentExtensions.GetFieldValue<Lstring>(content, SitefinityFields.Title) },
                TitlePart = new Titlepart() { Title = dynamicContentExtensions.GetFieldValue<Lstring>(content, SitefinityFields.Title) },
                ApprenticeshipRequirements = new Apprenticeshiprequirements() { Info = new OcInfoHtml { Html = dynamicContentExtensions.GetFieldValue<Lstring>(content, SitefinityFields.Info) } },
                GraphSyncPart = new Graphsyncpart() { Text = $"<<contentapiprefix>>/apprenticeshiprequirements/{dynamicContentExtensions.GetFieldValue<Guid>(content, SitefinityFields.OriginalContentId)}" },
                AuditTrailPart = new Audittrailpart() { Comment = null, ShowComment = false }
            };

            return apprenticeshipRequirement;
        }

        #endregion ApprenticeshipRequirement

        #region CollegeRequirement

        public IEnumerable<OcCollegeRequirement> GetAllCollegeRequirements()
        {
            dynamicModuleContentType = TypeResolutionService.ResolveType(DynamicTypes.CollegeRequirementContentType);
            var dynamicModuleItems = dynamicModuleManager.GetDataItems(dynamicModuleContentType).Where(item => item.Status == ContentLifecycleStatus.Live && item.Visible);

            if (dynamicModuleItems.Any())
            {
                var collegeRequirements = new List<OcCollegeRequirement>();
                foreach (var dynamicModuleItem in dynamicModuleItems)
                {
                    collegeRequirements.Add(ConvertFromToCollegeRequirement(dynamicModuleItem));
                }

                return collegeRequirements;
            }

            return Enumerable.Empty<OcCollegeRequirement>();
        }

        public OcCollegeRequirement ConvertFromToCollegeRequirement(DynamicContent content)
        {
            var collegeRequirement = new OcCollegeRequirement
            {
                SitefinityId = dynamicContentExtensions.GetFieldValue<Guid>(content, SitefinityFields.Id),
                ContentItemId = OrchardCoreIdGenerator.GenerateUniqueId(),
                ContentType = OcItemTypes.CollegeRequirements,
                DisplayText = dynamicContentExtensions.GetFieldValue<Lstring>(content, SitefinityFields.Title),
                Latest = true,
                Published = true,
                ModifiedUtc = dynamicContentExtensions.GetFieldValue<DateTime>(content, SitefinityFields.LastModified),
                PublishedUtc = dynamicContentExtensions.GetFieldValue<DateTime>(content, SitefinityFields.PublicationDate),
                CreatedUtc = dynamicContentExtensions.GetFieldValue<DateTime>(content, SitefinityFields.DateCreated),
                UniqueTitlePart = new Uniquetitlepart() { Title = dynamicContentExtensions.GetFieldValue<Lstring>(content, SitefinityFields.Title) },
                TitlePart = new Titlepart() { Title = dynamicContentExtensions.GetFieldValue<Lstring>(content, SitefinityFields.Title) },
                CollegeRequirements = new Collegerequirements() { Info = new OcInfoHtml { Html = dynamicContentExtensions.GetFieldValue<Lstring>(content, SitefinityFields.Info) } },
                GraphSyncPart = new Graphsyncpart() { Text = $"<<contentapiprefix>>/collegerequirements/{dynamicContentExtensions.GetFieldValue<Guid>(content, SitefinityFields.OriginalContentId)}" },
                AuditTrailPart = new Audittrailpart() { Comment = null, ShowComment = false }
            };

            return collegeRequirement;
        }

        #endregion CollegeRequirement

        #region UniversityRequirement

        public IEnumerable<OcUniversityRequirement> GetAllUniversityRequirements()
        {
            dynamicModuleContentType = TypeResolutionService.ResolveType(DynamicTypes.UniversityRequirementContentType);
            var dynamicModuleItems = dynamicModuleManager.GetDataItems(dynamicModuleContentType).Where(item => item.Status == ContentLifecycleStatus.Live && item.Visible);

            if (dynamicModuleItems.Any())
            {
                var universityRequirements = new List<OcUniversityRequirement>();
                foreach (var dynamicModuleItem in dynamicModuleItems)
                {
                    universityRequirements.Add(ConvertFromToUniversityRequirement(dynamicModuleItem));
                }

                return universityRequirements;
            }

            return Enumerable.Empty<OcUniversityRequirement>();
        }

        public OcUniversityRequirement ConvertFromToUniversityRequirement(DynamicContent content)
        {
            var universityRequirement = new OcUniversityRequirement
            {
                SitefinityId = dynamicContentExtensions.GetFieldValue<Guid>(content, SitefinityFields.Id),
                ContentItemId = OrchardCoreIdGenerator.GenerateUniqueId(),
                ContentType = OcItemTypes.UniversityRequirements,
                DisplayText = dynamicContentExtensions.GetFieldValue<Lstring>(content, SitefinityFields.Title),
                Latest = true,
                Published = true,
                ModifiedUtc = dynamicContentExtensions.GetFieldValue<DateTime>(content, SitefinityFields.LastModified),
                PublishedUtc = dynamicContentExtensions.GetFieldValue<DateTime>(content, SitefinityFields.PublicationDate),
                CreatedUtc = dynamicContentExtensions.GetFieldValue<DateTime>(content, SitefinityFields.DateCreated),
                UniqueTitlePart = new Uniquetitlepart() { Title = dynamicContentExtensions.GetFieldValue<Lstring>(content, SitefinityFields.Title) },
                TitlePart = new Titlepart() { Title = dynamicContentExtensions.GetFieldValue<Lstring>(content, SitefinityFields.Title) },
                UniversityRequirements = new Universityrequirements() { Info = new OcInfoHtml { Html = dynamicContentExtensions.GetFieldValue<Lstring>(content, SitefinityFields.Info) } },
                GraphSyncPart = new Graphsyncpart() { Text = $"<<contentapiprefix>>/universityrequirements/{dynamicContentExtensions.GetFieldValue<Guid>(content, SitefinityFields.OriginalContentId)}" },
                AuditTrailPart = new Audittrailpart() { Comment = null, ShowComment = false }
            };

            return universityRequirement;
        }

        #endregion UniversityRequirement

        #region Uniform

        public IEnumerable<OcUniform> GetAllUniforms()
        {
            dynamicModuleContentType = TypeResolutionService.ResolveType(DynamicTypes.UniformContentType);
            var dynamicModuleItems = dynamicModuleManager.GetDataItems(dynamicModuleContentType).Where(item => item.Status == ContentLifecycleStatus.Live && item.Visible);

            if (dynamicModuleItems.Any())
            {
                var uniforms = new List<OcUniform>();
                foreach (var dynamicModuleItem in dynamicModuleItems)
                {
                    uniforms.Add(ConvertFromToUniform(dynamicModuleItem));
                }

                return uniforms;
            }

            return Enumerable.Empty<OcUniform>();
        }

        public OcUniform ConvertFromToUniform(DynamicContent content)
        {
            var uniform = new OcUniform
            {
                SitefinityId = dynamicContentExtensions.GetFieldValue<Guid>(content, SitefinityFields.Id),
                ContentItemId = OrchardCoreIdGenerator.GenerateUniqueId(),
                ContentType = ItemTypes.Uniform,
                DisplayText = dynamicContentExtensions.GetFieldValue<Lstring>(content, SitefinityFields.Title),
                Latest = true,
                Published = true,
                ModifiedUtc = dynamicContentExtensions.GetFieldValue<DateTime>(content, SitefinityFields.LastModified),
                PublishedUtc = dynamicContentExtensions.GetFieldValue<DateTime>(content, SitefinityFields.PublicationDate),
                CreatedUtc = dynamicContentExtensions.GetFieldValue<DateTime>(content, SitefinityFields.DateCreated),
                UniqueTitlePart = new Uniquetitlepart() { Title = dynamicContentExtensions.GetFieldValue<Lstring>(content, SitefinityFields.Title) },
                TitlePart = new Titlepart() { Title = dynamicContentExtensions.GetFieldValue<Lstring>(content, SitefinityFields.Title) },
                Uniform = new Uniform() { Description = new OcDescriptionText() { Text = dynamicContentExtensions.GetFieldValue<Lstring>(content, SitefinityFields.Description) } },
                GraphSyncPart = new Graphsyncpart() { Text = $"<<contentapiprefix>>/uniform/{dynamicContentExtensions.GetFieldValue<Guid>(content, SitefinityFields.OriginalContentId)}" },
                AuditTrailPart = new Audittrailpart() { Comment = null, ShowComment = false }
            };

            return uniform;
        }

        #endregion Uniform

        #region Location

        public IEnumerable<OcLocation> GetAllLocations()
        {
            dynamicModuleContentType = TypeResolutionService.ResolveType(DynamicTypes.LocationContentType);
            var dynamicModuleItems = dynamicModuleManager.GetDataItems(dynamicModuleContentType).Where(item => item.Status == ContentLifecycleStatus.Live && item.Visible);

            if (dynamicModuleItems.Any())
            {
                var locations = new List<OcLocation>();
                foreach (var dynamicModuleItem in dynamicModuleItems)
                {
                    locations.Add(ConvertFromToLocation(dynamicModuleItem));
                }

                return locations;
            }

            return Enumerable.Empty<OcLocation>();
        }

        public OcLocation ConvertFromToLocation(DynamicContent content)
        {
            var location = new OcLocation
            {
                SitefinityId = dynamicContentExtensions.GetFieldValue<Guid>(content, SitefinityFields.Id),
                ContentItemId = OrchardCoreIdGenerator.GenerateUniqueId(),
                ContentType = ItemTypes.Location,
                DisplayText = dynamicContentExtensions.GetFieldValue<Lstring>(content, SitefinityFields.Title),
                Latest = true,
                Published = true,
                ModifiedUtc = dynamicContentExtensions.GetFieldValue<DateTime>(content, SitefinityFields.LastModified),
                PublishedUtc = dynamicContentExtensions.GetFieldValue<DateTime>(content, SitefinityFields.PublicationDate),
                CreatedUtc = dynamicContentExtensions.GetFieldValue<DateTime>(content, SitefinityFields.DateCreated),
                UniqueTitlePart = new Uniquetitlepart() { Title = dynamicContentExtensions.GetFieldValue<Lstring>(content, SitefinityFields.Title) },
                TitlePart = new Titlepart() { Title = dynamicContentExtensions.GetFieldValue<Lstring>(content, SitefinityFields.Title) },
                Location = new IntLocation() { Description = new OcDescriptionText() { Text = dynamicContentExtensions.GetFieldValue<Lstring>(content, SitefinityFields.Description) } },
                GraphSyncPart = new Graphsyncpart() { Text = $"<<contentapiprefix>>/location/{dynamicContentExtensions.GetFieldValue<Guid>(content, SitefinityFields.OriginalContentId)}" },
                AuditTrailPart = new Audittrailpart() { Comment = null, ShowComment = false }
            };

            return location;
        }

        #endregion Location

        #region Environment

        public IEnumerable<OcEnvironment> GetAllEnvironments()
        {
            dynamicModuleContentType = TypeResolutionService.ResolveType(DynamicTypes.EnvironmentContentType);
            var dynamicModuleItems = dynamicModuleManager.GetDataItems(dynamicModuleContentType).Where(item => item.Status == ContentLifecycleStatus.Live && item.Visible);

            if (dynamicModuleItems.Any())
            {
                var environments = new List<OcEnvironment>();
                foreach (var dynamicModuleItem in dynamicModuleItems)
                {
                    environments.Add(ConvertFromToEnvironment(dynamicModuleItem));
                }

                return environments;
            }

            return Enumerable.Empty<OcEnvironment>();
        }

        public OcEnvironment ConvertFromToEnvironment(DynamicContent content)
        {
            var environment = new OcEnvironment
            {
                SitefinityId = dynamicContentExtensions.GetFieldValue<Guid>(content, SitefinityFields.Id),
                ContentItemId = OrchardCoreIdGenerator.GenerateUniqueId(),
                ContentType = ItemTypes.Environment,
                DisplayText = dynamicContentExtensions.GetFieldValue<Lstring>(content, SitefinityFields.Title),
                Latest = true,
                Published = true,
                ModifiedUtc = dynamicContentExtensions.GetFieldValue<DateTime>(content, SitefinityFields.LastModified),
                PublishedUtc = dynamicContentExtensions.GetFieldValue<DateTime>(content, SitefinityFields.PublicationDate),
                CreatedUtc = dynamicContentExtensions.GetFieldValue<DateTime>(content, SitefinityFields.DateCreated),
                UniqueTitlePart = new Uniquetitlepart() { Title = dynamicContentExtensions.GetFieldValue<Lstring>(content, SitefinityFields.Title) },
                TitlePart = new Titlepart() { Title = dynamicContentExtensions.GetFieldValue<Lstring>(content, SitefinityFields.Title) },
                Environment = new IntEnvironment() { Description = new OcDescriptionText() { Text = dynamicContentExtensions.GetFieldValue<Lstring>(content, SitefinityFields.Description) } },
                GraphSyncPart = new Graphsyncpart() { Text = $"<<contentapiprefix>>/environment/{dynamicContentExtensions.GetFieldValue<Guid>(content, SitefinityFields.OriginalContentId)}" },
                AuditTrailPart = new Audittrailpart() { Comment = null, ShowComment = false }
            };

            return environment;
        }

        #endregion Environment

        #region SOCCode

        public IEnumerable<OcSocCode> GetAllSOCCodes()
        {
            dynamicModuleContentType = TypeResolutionService.ResolveType(DynamicTypes.JobProfileSocContentType);
            var dynamicModuleItems = dynamicModuleManager.GetDataItems(dynamicModuleContentType).Where(item => item.Status == ContentLifecycleStatus.Live && item.Visible);

            if (dynamicModuleItems.Any())
            {
                var socCodes = new List<OcSocCode>();
                foreach (var dynamicModuleItem in dynamicModuleItems)
                {
                    socCodes.Add(ConvertFromToSocCode(dynamicModuleItem));
                }

                return socCodes;
            }

            return Enumerable.Empty<OcSocCode>();
        }

        public OcSocCode ConvertFromToSocCode(DynamicContent content)
        {
            var relatedClasificationsApprenticeshipstandards = dynamicContentExtensions.GetFieldValue<IList<Guid>>(content, SitefinityFields.Apprenticeshipstandards);
            var socCode = new OcSocCode
            {
                SitefinityId = dynamicContentExtensions.GetFieldValue<Guid>(content, SitefinityFields.Id),
                ContentItemId = OrchardCoreIdGenerator.GenerateUniqueId(),
                ContentItemVersionId = OrchardCoreIdGenerator.GenerateUniqueId(),
                ContentType = OcItemTypes.SOCCode,
                DisplayText = dynamicContentExtensions.GetFieldValue<Lstring>(content, SitefinityFields.SOCCode),
                Latest = true,
                Published = true,
                ModifiedUtc = dynamicContentExtensions.GetFieldValue<DateTime>(content, SitefinityFields.LastModified),
                PublishedUtc = dynamicContentExtensions.GetFieldValue<DateTime>(content, SitefinityFields.PublicationDate),
                CreatedUtc = dynamicContentExtensions.GetFieldValue<DateTime>(content, SitefinityFields.DateCreated),
                UniqueTitlePart = new Uniquetitlepart() { Title = dynamicContentExtensions.GetFieldValue<Lstring>(content, SitefinityFields.SOCCode) },
                TitlePart = new Titlepart() { Title = dynamicContentExtensions.GetFieldValue<Lstring>(content, SitefinityFields.SOCCode) },
                SOCCode = new Soccode()
                {
                    Description = new OcDescriptionText() { Text = dynamicContentExtensions.GetFieldValue<Lstring>(content, SitefinityFields.Description) },
                    OnetOccupationCode = new Onetoccupationcode() { Text = dynamicContentExtensions.GetFieldValue<Lstring>(content, SitefinityFields.ONetOccupationalCode) },
                    ApprenticeshipStandardsSitefinityIds = new ApprenticeshipstandardsSitefinity() { SitefinityIds = relatedClasificationsApprenticeshipstandards.ToList() }
                },
                GraphSyncPart = new Graphsyncpart() { Text = $"<<contentapiprefix>>/soccode/{dynamicContentExtensions.GetFieldValue<Guid>(content, SitefinityFields.OriginalContentId)}" },
                AuditTrailPart = new Audittrailpart() { Comment = null, ShowComment = false }
            };

            return socCode;
        }

        #endregion SOCCode

        #region JobProfiles

        public IEnumerable<JobProfileUrl> GetAllJobProfileUrls()
        {
            dynamicModuleContentType = TypeResolutionService.ResolveType(DynamicTypes.JobprofileContentType);
            var jobProfilesDynamicContentItems = dynamicModuleManager.GetDataItems(dynamicModuleContentType).Where(item => item.Status == ContentLifecycleStatus.Live && item.Visible);

            //var jobProfilesDynamicContentItems = repository.GetMany(item => item.Status == ContentLifecycleStatus.Live && item.Visible).ToList();
            if (jobProfilesDynamicContentItems.Any())
            {
                var jobProfileUrls = new List<JobProfileUrl>();

                foreach (var jobProfilesDynamicContentItem in jobProfilesDynamicContentItems)
                {
                    jobProfileUrls.Add(ConvertFromToJobProfileUrl(jobProfilesDynamicContentItem));
                }

                return jobProfileUrls;
            }

            return Enumerable.Empty<JobProfileUrl>();
        }

        public JobProfileUrl ConvertFromToJobProfileUrl(DynamicContent content)
        {
            var jobProfileUrl = new JobProfileUrl
            {
                Id = dynamicContentExtensions.GetFieldValue<Guid>(content, nameof(JobProfileUrl.Id)),
                Title = dynamicContentExtensions.GetFieldValue<Lstring>(content, nameof(JobProfileUrl.Title)),
                UrlName = dynamicContentExtensions.GetFieldValue<Lstring>(content, nameof(JobProfileUrl.UrlName))
            };

            return jobProfileUrl;
        }

        public OcJobProfile GetJobProfileByUrlName(string urlName)
        {
            dynamicModuleContentType = TypeResolutionService.ResolveType(DynamicTypes.JobprofileContentType);
            var jobProfilesDynamicContentItem = dynamicModuleManager.GetDataItems(dynamicModuleContentType).Where(item => item.UrlName == urlName && item.Status == ContentLifecycleStatus.Live && item.Visible).FirstOrDefault();
            return ConvertDynamicContentToOcJobProfile(jobProfilesDynamicContentItem);

            //return ConvertDynamicContentToOcJobProfile(repository.Get(item => item.UrlName == urlName && item.Status == ContentLifecycleStatus.Live && item.Visible == true));
        }

        public OcJobProfile ConvertDynamicContentToOcJobProfile(DynamicContent content)
        {
            var relatedHiddenAlternativeTitles = dynamicContentExtensions.GetFieldValue<IList<Guid>>(content, ItemTypes.HiddenAlternativeTitle);
            var relatedWorkingHoursDetails = dynamicContentExtensions.GetFieldValue<IList<Guid>>(content, ItemTypes.WorkingHoursDetails);
            var relatedWorkingPattern = dynamicContentExtensions.GetFieldValue<IList<Guid>>(content, ItemTypes.WorkingPattern);
            var relatedWorkingPatternDetails = dynamicContentExtensions.GetFieldValue<IList<Guid>>(content, ItemTypes.WorkingPatternDetails);
            var relatedJobProfileSpecialism = dynamicContentExtensions.GetFieldValue<IList<Guid>>(content, ItemTypes.JobProfileSpecialism);
            var relatedUniversityEntryRequirements = dynamicContentExtensions.GetFieldValue<IList<Guid>>(content, ItemTypes.UniversityEntryRequirements);
            var relatedCollegeEntryRequirements = dynamicContentExtensions.GetFieldValue<IList<Guid>>(content, ItemTypes.CollegeEntryRequirements);
            var relatedApprenticeshipEntryRequirements = dynamicContentExtensions.GetFieldValue<IList<Guid>>(content, ItemTypes.ApprenticeshipEntryRequirements);
            var relatedJobProfileCategories = dynamicContentExtensions.GetFieldValue<IList<Guid>>(content, SitefinityFields.RelatedJobProfileCategories);

            var relatedUniversityRequirements = dynamicContentExtensions.GetRelatedItemsIds(content, SitefinityFields.RelatedUniversityRequirement);
            var relatedUniversityLinks = dynamicContentExtensions.GetRelatedItemsIds(content, SitefinityFields.RelatedUniversityLinks);
            var relatedCollegeRequirements = dynamicContentExtensions.GetRelatedItemsIds(content, SitefinityFields.RelatedCollegeRequirements);
            var relatedCollegeLinks = dynamicContentExtensions.GetRelatedItemsIds(content, SitefinityFields.RelatedCollegeLinks);
            var relatedApprenticeshipRequirements = dynamicContentExtensions.GetRelatedItemsIds(content, SitefinityFields.RelatedApprenticeshipRequirements);
            var relatedApprenticeshipLinks = dynamicContentExtensions.GetRelatedItemsIds(content, SitefinityFields.RelatedApprenticeshipLinks);
            var relatedRestrictions = dynamicContentExtensions.GetRelatedItemsIds(content, SitefinityFields.RelatedRestrictions);
            var relatedLocations = dynamicContentExtensions.GetRelatedItemsIds(content, SitefinityFields.RelatedLocations);
            var relatedEnvironments = dynamicContentExtensions.GetRelatedItemsIds(content, SitefinityFields.RelatedEnvironments);
            var relatedUniforms = dynamicContentExtensions.GetRelatedItemsIds(content, SitefinityFields.RelatedUniforms);
            var relatedCareerProfiles = dynamicContentExtensions.GetRelatedItemsIds(content, SitefinityFields.RelatedCareerProfiles);
            var relatedSOC = dynamicContentExtensions.GetRelatedItemsIds(content, SitefinityFields.SOC);
            var relatedRegistrations = dynamicContentExtensions.GetRelatedItemsIds(content, SitefinityFields.RelatedRegistrations);

            // Related SSMs
            var relatedSSMTitles = dynamicContentExtensions.GetRelatedItemsTitles(content, SitefinityFields.RelatedSkills);
            var relatedSkillsTitles = new List<string>();

            foreach (var relatedSSM in relatedSSMTitles ?? new List<string>())
            {
                relatedSkillsTitles.Add($"\u00ABc#: await Content.GetContentItemIdByDisplayText(\"SOCSkillsMatrix\", \"{relatedSSM}\")\u00BB");
            }

            var ocJobProfile = new OcJobProfile
            {
                SitefinityId = dynamicContentExtensions.GetFieldValue<Guid>(content, SitefinityFields.Id),
                ContentItemId = OrchardCoreIdGenerator.GenerateUniqueId(),
                ContentItemVersionId = OrchardCoreIdGenerator.GenerateUniqueId(),
                ContentType = ItemTypes.JobProfile,
                DisplayText = dynamicContentExtensions.GetFieldValue<Lstring>(content, SitefinityFields.Title),
                Latest = true,
                Published = true,
                ModifiedUtc = dynamicContentExtensions.GetFieldValue<DateTime>(content, SitefinityFields.LastModified),
                PublishedUtc = dynamicContentExtensions.GetFieldValue<DateTime>(content, SitefinityFields.PublicationDate),
                CreatedUtc = dynamicContentExtensions.GetFieldValue<DateTime>(content, SitefinityFields.DateCreated),
                TitlePart = new Titlepart() { Title = dynamicContentExtensions.GetFieldValue<Lstring>(content, SitefinityFields.Title) },
                JobProfile = new Jobprofile()
                {
                    AlternativeTitle = new Alternativetitle() { Text = dynamicContentExtensions.GetFieldValue<Lstring>(content, nameof(JobProfile.AlternativeTitle)) },
                    WidgetContentTitle = new Widgetcontenttitle() { Text = dynamicContentExtensions.GetFieldValue<Lstring>(content, nameof(JobProfile.WidgetContentTitle)) },
                    Overview = new Overview() { Text = dynamicContentExtensions.GetFieldValue<Lstring>(content, nameof(JobProfile.Overview)) },
                    Salarystarterperyear = new Salarystarterperyear() { Value = (float?)dynamicContentExtensions.GetFieldValue<decimal?>(content, nameof(JobProfile.SalaryStarter)) },
                    Salaryexperiencedperyear = new Salaryexperiencedperyear() { Value = (float?)dynamicContentExtensions.GetFieldValue<decimal?>(content, nameof(JobProfile.SalaryExperienced)) },
                    Minimumhours = new Minimumhours() { Value = (float?)dynamicContentExtensions.GetFieldValue<decimal?>(content, nameof(JobProfile.MinimumHours)) },
                    Maximumhours = new Maximumhours() { Value = (float?)dynamicContentExtensions.GetFieldValue<decimal?>(content, nameof(JobProfile.MaximumHours)) },
                    HiddenAlternativeTitleSf = relatedHiddenAlternativeTitles?.ToList(),
                    HiddenAlternativeTitle = new HiddenalternativetitleIds(),
                    WorkingHoursDetailsSf = relatedWorkingHoursDetails?.ToList(),
                    WorkingHoursDetails = new Workinghoursdetails(),
                    WorkingPatternSf = relatedWorkingPattern?.ToList(),
                    WorkingPattern = new Workingpattern(),
                    WorkingPatternDetailsSf = relatedWorkingPatternDetails?.ToList(),
                    WorkingPatternDetails = new Workingpatterndetails(),
                    JobProfileSpecialismSf = relatedJobProfileSpecialism?.ToList(),
                    JobProfileSpecialism = new JobprofilespecialismIds(),
                    Entryroutes = new Entryroutes() { Html = dynamicContentExtensions.GetFieldValue<Lstring>(content, SitefinityFields.EntryRoutes) },
                    Universityrelevantsubjects = new Universityrelevantsubjects() { Html = dynamicContentExtensions.GetFieldValue<Lstring>(content, SitefinityFields.UniversityRelevantSubjects) },
                    Universityfurtherrouteinfo = new Universityfurtherrouteinfo() { Html = dynamicContentExtensions.GetFieldValue<Lstring>(content, SitefinityFields.UniversityFurtherRouteInfo) },
                    UniversityEntryRequirementsSf = relatedUniversityEntryRequirements?.ToList(),
                    UniversityEntryRequirements = new UniversityentryrequirementsIds(),
                    RelatedUniversityRequirementsSf = relatedUniversityRequirements?.ToList(),
                    RelatedUniversityRequirements = new Relateduniversityrequirements(),
                    Collegerelevantsubjects = new Collegerelevantsubjects() { Html = dynamicContentExtensions.GetFieldValue<Lstring>(content, SitefinityFields.CollegeRelevantSubjects) },
                    Collegefurtherrouteinfo = new Collegefurtherrouteinfo() { Html = dynamicContentExtensions.GetFieldValue<Lstring>(content, SitefinityFields.CollegeFurtherRouteInfo) },
                    RelatedUniversityLinksSf = relatedUniversityLinks?.ToList(),
                    RelatedUniversityLinks = new Relateduniversitylinks(),
                    CollegeEntryRequirementsSf = relatedCollegeEntryRequirements?.ToList(),
                    CollegeEntryRequirements = new CollegeentryrequirementsIds(),
                    RelatedCollegeRequirementsSf = relatedCollegeRequirements?.ToList(),
                    RelatedCollegeRequirements = new Relatedcollegerequirements(),
                    RelatedCollegeLinksSf = relatedCollegeLinks?.ToList(),
                    RelatedCollegeLinks = new Relatedcollegelinks(),
                    Apprenticeshiprelevantsubjects = new Apprenticeshiprelevantsubjects() { Html = dynamicContentExtensions.GetFieldValue<Lstring>(content, SitefinityFields.ApprenticeshipRelevantSubjects) },
                    Apprenticeshipfurtherroutesinfo = new Apprenticeshipfurtherroutesinfo() { Html = dynamicContentExtensions.GetFieldValue<Lstring>(content, SitefinityFields.ApprenticeshipFurtherRouteInfo) },
                    ApprenticeshipEntryRequirementsSf = relatedApprenticeshipEntryRequirements?.ToList(),
                    ApprenticeshipEntryRequirements = new ApprenticeshipentryrequirementsIds(),
                    RelatedApprenticeshipRequirementsSf = relatedApprenticeshipRequirements?.ToList(),
                    RelatedApprenticeshipRequirements = new Relatedapprenticeshiprequirements(),
                    RelatedApprenticeshipLinksSf = relatedApprenticeshipLinks?.ToList(),
                    RelatedApprenticeshipLinks = new Relatedapprenticeshiplinks(),
                    Work = new Work() { Html = dynamicContentExtensions.GetFieldValue<Lstring>(content, SitefinityFields.Work) },
                    Volunteering = new Volunteering() { Html = dynamicContentExtensions.GetFieldValue<Lstring>(content, SitefinityFields.Volunteering) },
                    Directapplication = new Directapplication() { Html = dynamicContentExtensions.GetFieldValue<Lstring>(content, SitefinityFields.DirectApplication) },
                    Otherroutes = new Otherroutes() { Html = dynamicContentExtensions.GetFieldValue<Lstring>(content, SitefinityFields.OtherRoutes) },
                    Careertips = new Careertips() { Html = dynamicContentExtensions.GetFieldValue<Lstring>(content, SitefinityFields.CareerTips) },
                    Furtherinformation = new Furtherinformation() { Html = dynamicContentExtensions.GetFieldValue<Lstring>(content, SitefinityFields.FurtherInformation) },
                    RelatedrestrictionsSf = relatedRestrictions?.ToList(),
                    Relatedrestrictions = new Relatedrestrictions(),
                    Otherrequirements = new Otherrequirements() { Html = dynamicContentExtensions.GetFieldValue<Lstring>(content, SitefinityFields.OtherRequirements) },
                    DigitalSkills = new Digitalskills() { ContentItemIds = GetDigitalSkill(dynamicContentExtensions.GetFieldChoiceLabel(content, nameof(SitefinityFields.DigitalSkillsLevel))) },

                    //Relatedskills = new Relatedskills() { ContentItemIds = new string[] { } },
                    Relatedskills = new Relatedskills() { ContentItemIds = relatedSkillsTitles.ToArray() },
                    JobProfileCategorySf = relatedJobProfileCategories?.ToList(),
                    JobProfileCategory = new JobprofilecategoryIds(),
                    Daytodaytasks = new Daytodaytasks() { Html = dynamicContentExtensions.GetFieldValue<Lstring>(content, SitefinityFields.WYDDayToDayTasks) },
                    RelatedLocationsSf = relatedLocations?.ToList(),
                    RelatedLocations = new Relatedlocations(),
                    RelatedEnvironmentsSf = relatedEnvironments?.ToList(),
                    RelatedEnvironments = new Relatedenvironments(),
                    RelatedUniformsSf = relatedUniforms?.ToList(),
                    RelatedUniforms = new Relateduniforms(),
                    Coursekeywords = new Coursekeywords() { Text = dynamicContentExtensions.GetFieldValue<Lstring>(content, SitefinityFields.CourseKeywords) },
                    Careerpathandprogression = new Careerpathandprogression() { Html = dynamicContentExtensions.GetFieldValue<Lstring>(content, SitefinityFields.CareerPathAndProgression) },
                    RelatedcareerprofilesSf = relatedCareerProfiles?.ToList(),
                    Relatedcareerprofiles = new Relatedcareerprofiles() { ContentItemIds = new string[] { } },
                    SOCCodeSf = relatedSOC?.ToList(),
                    SOCCode = new SoccodeIds(),
                    RelatedRegistrationsSf = relatedRegistrations?.ToList(),
                    RelatedRegistrations = new Relatedregistrations(),
                    Professionalandindustrybodies = new Professionalandindustrybodies() { Html = dynamicContentExtensions.GetFieldValue<Lstring>(content, SitefinityFields.ProfessionalAndIndustryBodies) },
                    DynamicTitlePrefix = new Dynamictitleprefix() { ContentItemIds = GetDynamicTitlePrefix(dynamicContentExtensions.GetFieldChoiceLabel(content, nameof(SitefinityFields.DynamicTitlePrefix))) }
                },
                PreviewPart = new Previewpart() { }, //????
                PageLocationPart = new Pagelocationpart() { UrlName = dynamicContentExtensions.GetFieldValue<Lstring>(content, SitefinityFields.UrlName), DefaultPageForLocation = false, RedirectLocations = null, FullUrl = $"/{dynamicContentExtensions.GetFieldValue<Lstring>(content, SitefinityFields.UrlName)}" },
                SitemapPart = new Sitemappart() { OverrideSitemapConfig = false, ChangeFrequency = 0, Priority = 5, Exclude = false },
                ContentApprovalPart = new Contentapprovalpart { ReviewStatus = 0, ReviewType = 0, IsForcePublished = false },
                GraphSyncPart = new Graphsyncpart() { Text = $"<<contentapiprefix>>/jobprofile/{dynamicContentExtensions.GetFieldValue<Guid>(content, SitefinityFields.OriginalContentId)}" },
                AuditTrailPart = new Audittrailpart() { Comment = "Initial version imported by Migration Tool.", ShowComment = false }
            };

            return ocJobProfile;
        }

        public string[] GetDigitalSkill(string digitalSkill)
        {
            string digitalSkillOrchardCoreId = string.Empty;
            switch (digitalSkill.Trim().ToLowerInvariant())
            {
                case DigitalSkills.ComputerSystemsAndApplications:
                    digitalSkillOrchardCoreId = "4nbq6e0zgpcnj0r4jqn89mcq55";
                    break;
                case DigitalSkills.SoftwarePackages:
                    digitalSkillOrchardCoreId = "4jewsa5k17pr75c1ptm9k7xkng";
                    break;
                case DigitalSkills.ComputerAndTheMainSoftwarePackages:
                    digitalSkillOrchardCoreId = "4k0twbe5kxane7x548sc730pxz";
                    break;
                case DigitalSkills.BasicTasksOnComputer:
                    digitalSkillOrchardCoreId = "42ek1wf6g6k7479ygtb8tzqhnm";
                    break;
                default:
                    digitalSkillOrchardCoreId = "4pggmxezyz04cxqhtm89cvf8x3";
                    break;
            }

            return new string[] { digitalSkillOrchardCoreId };
        }

        public string[] GetDynamicTitlePrefix(string dynamicTitlePrefix)
        {
            string dynamicTitlePrefixOrchardCoreId = string.Empty;
            switch (dynamicTitlePrefix.Trim().ToLowerInvariant())
            {
                case DynamicTitlePrefixes.PrefixWithA:
                    dynamicTitlePrefixOrchardCoreId = "45stsxyyd5wxcygw4mkcr3g05b";
                    break;
                case DynamicTitlePrefixes.PrefixWithAn:
                    dynamicTitlePrefixOrchardCoreId = "4bntcf1j07w6b3ptj63f1vxsm1";
                    break;
                case DynamicTitlePrefixes.NoPrefix:
                    dynamicTitlePrefixOrchardCoreId = "4avc9zyzc0h482tvn9kqjps253";
                    break;
                case DynamicTitlePrefixes.NoTitle:
                    dynamicTitlePrefixOrchardCoreId = "41dd2nk0h3jqa0fgce4aq0y2c5";
                    break;
                case DynamicTitlePrefixes.AsDefined:
                    dynamicTitlePrefixOrchardCoreId = "4efw08fyykqqe7qn7jqtcvtdch";
                    break;
                default:
                    dynamicTitlePrefixOrchardCoreId = "4cesm5r8t0aa827bp3gn4pgsxx";
                    break;
            }

            return new string[] { dynamicTitlePrefixOrchardCoreId };
        }

        public OcJobProfile GetJobProfileByUrlNameRCP(string urlName)
        {
            dynamicModuleContentType = TypeResolutionService.ResolveType(DynamicTypes.JobprofileContentType);
            var jobProfilesDynamicContentItem = dynamicModuleManager.GetDataItems(dynamicModuleContentType).Where(item => item.UrlName == urlName && item.Status == ContentLifecycleStatus.Live && item.Visible).FirstOrDefault();
            return ConvertDynamicContentToOcJobProfileRCP(jobProfilesDynamicContentItem);
        }

        public OcJobProfile ConvertDynamicContentToOcJobProfileRCP(DynamicContent content)
        {
            var relatedCareerProfilesTitles = dynamicContentExtensions.GetRelatedItemsTitles(content, SitefinityFields.RelatedCareerProfiles);

            var ocJobProfile = new OcJobProfile
            {
                SitefinityId = dynamicContentExtensions.GetFieldValue<Guid>(content, SitefinityFields.Id),
                DisplayText = dynamicContentExtensions.GetFieldValue<Lstring>(content, SitefinityFields.Title),
                JobProfile = new Jobprofile()
                {
                    RelatedcareerprofilesSfTitles = relatedCareerProfilesTitles?.ToList()
                }
            };

            return ocJobProfile;
        }

        public OcJobProfile GetJobProfileByUrlNameSkills(string urlName)
        {
            dynamicModuleContentType = TypeResolutionService.ResolveType(DynamicTypes.JobprofileContentType);
            var jobProfilesDynamicContentItem = dynamicModuleManager.GetDataItems(dynamicModuleContentType).Where(item => item.UrlName == urlName && item.Status == ContentLifecycleStatus.Live && item.Visible).FirstOrDefault();
            return ConvertDynamicContentToOcJobProfileSkills(jobProfilesDynamicContentItem);
        }

        public OcJobProfile ConvertDynamicContentToOcJobProfileSkills(DynamicContent content)
        {
            var relatedSkillsTitles = dynamicContentExtensions.GetRelatedItemsTitles(content, SitefinityFields.RelatedSkills);
            var relatedSOCTitles = dynamicContentExtensions.GetRelatedSOCCodes(content, SitefinityFields.SOC);

            var ocJobProfile = new OcJobProfile
            {
                SitefinityId = dynamicContentExtensions.GetFieldValue<Guid>(content, SitefinityFields.Id),
                DisplayText = dynamicContentExtensions.GetFieldValue<Lstring>(content, SitefinityFields.Title),
                JobProfile = new Jobprofile()
                {
                    RelatedSkillsSfTitles = relatedSkillsTitles?.ToList(),
                    SOCCodeSfTitles = relatedSOCTitles?.ToList()
                }
            };

            return ocJobProfile;
        }

        #endregion JobProfiles

        #region FilteringQuestions

        public IEnumerable<OcFilteringQuestion> GetFilteringQuestions()
        {
            var providerName = "dynamicProvider6";
            DynamicModuleManager dynamicModuleFQManager = DynamicModuleManager.GetManager(providerName);
            dynamicModuleContentType = TypeResolutionService.ResolveType(DynamicTypes.FilteringQuestionType);
            var dynamicModuleItems = dynamicModuleFQManager.GetDataItems(dynamicModuleContentType).Where(item => item.Status == ContentLifecycleStatus.Master);

            if (dynamicModuleItems.Any())
            {
                var allSSMTitles = new List<string>();

                var providerSSMName = "dynamicProvider2";
                DynamicModuleManager dynamicModuleSSMManager = DynamicModuleManager.GetManager(providerSSMName);
                var dynamicModuleSSMContentType = TypeResolutionService.ResolveType(DynamicTypes.SocSkillMatrixContentType);
                var dynamicModuleSSMItems = dynamicModuleSSMManager.GetDataItems(dynamicModuleSSMContentType).Where(item => item.Status == ContentLifecycleStatus.Master);

                if (dynamicModuleSSMItems.Any())
                {
                    foreach (var dynamicModuleSSMItem in dynamicModuleSSMItems)
                    {
                        var ssmTitle = dynamicContentExtensions.GetFieldValue<Lstring>(dynamicModuleSSMItem, SitefinityFields.Title);
                        allSSMTitles.Add(ssmTitle);
                    }
                }

                var filteringQuestions = new List<OcFilteringQuestion>();
                foreach (var dynamicModuleItem in dynamicModuleItems)
                {
                    var filteringQuestionTitle = dynamicContentExtensions.GetFieldValue<Lstring>(dynamicModuleItem, SitefinityFields.Title);

                    if (IsDraftFilteringQuestion(filteringQuestionTitle))
                    {
                        var relatedSkill = dynamicContentExtensions.GetFilteringQuestionRelatedItemsTitles(dynamicModuleItem, SitefinityFields.RelatedSkill);
                        var relatedSSMs = allSSMTitles.Where(ssm => ssm.Contains(relatedSkill.FirstOrDefault()));
                        var relatedSSMTitles = new List<string>();

                        //foreach (var relatedSSM in relatedSSMs ?? new List<string>())
                        //{
                        //    relatedSSMTitles.Add($"\u00ABc#: await Content.GetContentItemIdByDisplayText(\"SOCSkillsMatrix\", \"{relatedSSM}\")\u00BB");
                        //}
                        if (!string.IsNullOrEmpty(relatedSSMs.FirstOrDefault()))
                        {
                            relatedSSMTitles.Add($"\u00ABc#: await Content.GetContentItemIdByDisplayText(\"SOCSkillsMatrix\", \"{relatedSSMs.FirstOrDefault()}\")\u00BB");
                        }

                        filteringQuestions.Add(ConvertFromToFilteringQuestion(dynamicModuleItem, relatedSSMTitles?.ToList()));
                    }
                }

                return filteringQuestions;
            }

            return Enumerable.Empty<OcFilteringQuestion>();
        }

        public OcFilteringQuestion ConvertFromToFilteringQuestion(DynamicContent content, List<string> relatedSSMTitles)
        {
            //var relatedSkill = dynamicContentExtensions.GetFilteringQuestionRelatedItemsTitles(content, SitefinityFields.RelatedSkill);
            var filteringQuestion = new OcFilteringQuestion
            {
                SitefinityId = dynamicContentExtensions.GetFieldValue<Guid>(content, SitefinityFields.Id),
                ContentItemId = OrchardCoreIdGenerator.GenerateUniqueId(),
                ContentItemVersionId = OrchardCoreIdGenerator.GenerateUniqueId(),
                ContentType = OcItemTypes.PersonalityFilteringQuestion,
                DisplayText = dynamicContentExtensions.GetFieldValue<Lstring>(content, SitefinityFields.Title),
                Latest = true,
                Published = false,
                ModifiedUtc = dynamicContentExtensions.GetFieldValue<DateTime>(content, SitefinityFields.LastModified),
                PublishedUtc = dynamicContentExtensions.GetFieldValue<DateTime>(content, SitefinityFields.PublicationDate),
                CreatedUtc = dynamicContentExtensions.GetFieldValue<DateTime>(content, SitefinityFields.DateCreated),
                TitlePart = new Titlepart() { Title = dynamicContentExtensions.GetFieldValue<Lstring>(content, SitefinityFields.Title) },
                PersonalityFilteringQuestion = new Personalityfilteringquestion()
                {
                    SOCSkillsMatrix = new Socskillsmatrix() { ContentItemIds = relatedSSMTitles?.ToArray() },
                    Text = new OcText() { Text = dynamicContentExtensions.GetFieldValue<Lstring>(content, SitefinityFields.QuestionText) },
                    Info = new OcText() { Text = dynamicContentExtensions.GetFieldValue<Lstring>(content, SitefinityFields.Description) }
                },
                GraphSyncPart = new Graphsyncpart() { Text = $"<<contentapiprefix>>/personalityfilteringquestion/{dynamicContentExtensions.GetFieldValue<Guid>(content, SitefinityFields.Id)}" }
            };

            return filteringQuestion;
        }

        public bool IsDraftFilteringQuestion(string filteringQuestionTitle)
        {
            var isDraftFilteringQuestion = true;
            var existingPublishedFilteringQuestions = new List<string>()
            {
                "Concern for Others",
                "Fine Manipulative Abilities",
                "Adaptability/Flexibility",
                "Reading Comprehension",
                "Stress Tolerance",
                "Verbal Abilities",
                "Initiative",
                "Cooperation",
                "Analytical Thinking",
                "Speaking, Verbal Abilities",
                "Quantitative Abilities, Mathematics Knowledge",
                "Self Control",
            };

            if (existingPublishedFilteringQuestions.Contains(filteringQuestionTitle, StringComparer.OrdinalIgnoreCase))
            {
                isDraftFilteringQuestion = false;
            }

            return isDraftFilteringQuestion;
        }

        #endregion FilteringQuestions

        [IgnoreOutputInInterception]
        public DynamicContent GetById(string id)
        {
            return dynamicModuleManager.GetDataItems(dynamicModuleContentType)
                .FirstOrDefault(item => item.Status == ContentLifecycleStatus.Live && item.Visible &&
                                        item.Id == new Guid(id));
        }

        [IgnoreInputInInterception]
        [IgnoreOutputInInterception]
        public IQueryable<DynamicContent> GetMany(Expression<Func<DynamicContent, bool>> where)
        {
            return GetAll().Where(where);
        }

        [IgnoreInputInInterception]
        [IgnoreOutputInInterception]
        public DynamicContent GetMaster(DynamicContent entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            return dynamicModuleManager.Lifecycle.GetMaster(entity) as DynamicContent;
        }

        [IgnoreInputInInterception]
        [IgnoreOutputInInterception]
        public DynamicContent GetTemp(DynamicContent entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            return dynamicModuleManager.Lifecycle.CheckOut(entity) as DynamicContent;
        }

        [IgnoreInputInInterception]
        [IgnoreOutputInInterception]
        public DynamicContent CheckinTemp(DynamicContent entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            return dynamicModuleManager.Lifecycle.CheckIn(entity) as DynamicContent;
        }

        public bool IsCheckedOut(DynamicContent entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            var master = entity.Status == ContentLifecycleStatus.Master
                ? entity
                : dynamicModuleManager.Lifecycle.GetMaster(entity);
            var workFlowItem = master as DynamicContent;
            var inprogress = dynamicModuleManager.Lifecycle.IsCheckedOut(master) || workFlowItem?.ApprovalWorkflowState == DraftApprovalWorkflowState;

            return inprogress;
        }

        #endregion IRepository implementations

        public void Commit()
        {
            dynamicModuleManager.SaveChanges();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (dynamicModuleManager != null)
                {
                    dynamicModuleManager.Dispose();
                    dynamicModuleManager = null;
                }
            }
        }

        private void CreateVersion(DynamicContent entity, string changeComment, VersionManager versionManager, WorkflowStatus status)
        {
            entity.SetWorkflowStatus(dynamicModuleManager.Provider.ApplicationName, status.ToString());

            var change = versionManager.CreateVersion(entity, status == WorkflowStatus.Published);
            if (changeComment != null)
            {
                change.Comment = changeComment;
            }
        }
    }
}