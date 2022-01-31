using DFC.Digital.Core;
using DFC.Digital.Core.Interceptors;
using DFC.Digital.Data;
using DFC.Digital.Data.Model;
using DFC.Digital.Data.Model.OrchardCore;
using DFC.Digital.Data.Model.OrchardCore.Uniform;
using DFC.Digital.Repository.SitefinityCMS.Modules;
using DFC.Digital.Repository.SitefinityCMS.OrchardCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Telerik.Sitefinity;
using Telerik.Sitefinity.Data;
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
                Registration = new IntRegistration() { Description = new OcDescriptionHtml() { Html = dynamicContentExtensions.GetFieldValue<Lstring>(content, SitefinityFields.Info) } },
                GraphSyncPart = new Graphsyncpart() { Text = $"<<contentapiprefix>>/registration/{dynamicContentExtensions.GetFieldValue<Guid>(content, SitefinityFields.Id)}" },
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
                GraphSyncPart = new Graphsyncpart() { Text = $"<<contentapiprefix>>/restriction/{dynamicContentExtensions.GetFieldValue<Guid>(content, SitefinityFields.Id)}" },
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
                GraphSyncPart = new Graphsyncpart() { Text = $"<<contentapiprefix>>/apprenticeshiplink/{dynamicContentExtensions.GetFieldValue<Guid>(content, SitefinityFields.Id)}" },
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
                GraphSyncPart = new Graphsyncpart() { Text = $"<<contentapiprefix>>/collegelink/{dynamicContentExtensions.GetFieldValue<Guid>(content, SitefinityFields.Id)}" },
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
                GraphSyncPart = new Graphsyncpart() { Text = $"<<contentapiprefix>>/universitylink/{dynamicContentExtensions.GetFieldValue<Guid>(content, SitefinityFields.Id)}" },
                AuditTrailPart = new Audittrailpart() { Comment = null, ShowComment = false }
            };

            return universityLink;
        }

        #endregion UniversityLink

        #region ApprenticeshipRequirement

        public IEnumerable<OcApprenticeshipRequirement> GetAllApprenticeshipRequirements()
        {
            dynamicModuleContentType = TypeResolutionService.ResolveType(DynamicTypes.UniversityRequirementContentType);
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
                GraphSyncPart = new Graphsyncpart() { Text = $"<<contentapiprefix>>/apprenticeshiprequirements/{dynamicContentExtensions.GetFieldValue<Guid>(content, SitefinityFields.Id)}" },
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
                GraphSyncPart = new Graphsyncpart() { Text = $"<<contentapiprefix>>/collegerequirements/{dynamicContentExtensions.GetFieldValue<Guid>(content, SitefinityFields.Id)}" },
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
                GraphSyncPart = new Graphsyncpart() { Text = $"<<contentapiprefix>>/universityrequirements/{dynamicContentExtensions.GetFieldValue<Guid>(content, SitefinityFields.Id)}" },
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
                Uniform = new Uniform() { Description = new OcDescriptionHtml() { Html = dynamicContentExtensions.GetFieldValue<Lstring>(content, SitefinityFields.Description) } },
                GraphSyncPart = new Graphsyncpart() { Text = $"<<contentapiprefix>>/uniform/{dynamicContentExtensions.GetFieldValue<Guid>(content, SitefinityFields.Id)}" },
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
                Location = new IntLocation() { Description = new OcDescriptionHtml() { Html = dynamicContentExtensions.GetFieldValue<Lstring>(content, SitefinityFields.Description) } },
                GraphSyncPart = new Graphsyncpart() { Text = $"<<contentapiprefix>>/location/{dynamicContentExtensions.GetFieldValue<Guid>(content, SitefinityFields.Id)}" },
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
                Environment = new IntEnvironment() { Description = new OcDescriptionHtml() { Html = dynamicContentExtensions.GetFieldValue<Lstring>(content, SitefinityFields.Description) } },
                GraphSyncPart = new Graphsyncpart() { Text = $"<<contentapiprefix>>/environment/{dynamicContentExtensions.GetFieldValue<Guid>(content, SitefinityFields.Id)}" },
                AuditTrailPart = new Audittrailpart() { Comment = null, ShowComment = false }
            };

            return environment;
        }

        #endregion Environment

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